using InnovaGraphics.Dtos;
using InnovaGraphics.Enums;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Factory;
using InnovaGraphics.Utils.Validators.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel;

namespace InnovaGraphics.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IValidator<Material, Guid> _materialValidator;
        private readonly MaterialMetadataFetcherFactory _fetcherFactory;
        private readonly int maxTypeCount = 2;
        private readonly int maxThemeCount = 5;


        public MaterialService(IMaterialRepository materialRepository, 
                               IValidator<Material, Guid> validator, 
                               MaterialMetadataFetcherFactory fetcherFactory)
        {
            _materialRepository = materialRepository;
            _materialValidator = validator;
            _fetcherFactory = fetcherFactory;
        }

        public async Task<MaterialResponseGeneral> CreateAsync(CreateMaterialDto dto)
        {
            var fetcher = _fetcherFactory.GetFetcher(dto.Type);
            var metadata = await fetcher.FetchAsync(dto.Link);

            if ((int)dto.Type > maxTypeCount || (int)dto.Theme.Count > maxThemeCount ||
            ((int)dto.Type > 0 && (dto.Link == null || dto.Link.Contains("youtube", StringComparison.OrdinalIgnoreCase))))
            {
                return new MaterialResponseGeneral
                {
                    Id = Guid.Empty,
                    Name = null,
                    Description = "Некоректні значення: Type повинен бути 0..2, Theme — 0..4, а якщо Type = 0, то це має бути посилання на youtube",
                    Type = 0,
                    Link = null,
                    PhotoPath = null,
                    Theme = null,
                };
            }


            var material = new Material
            {
                Id = Guid.NewGuid(),
                Name = metadata.Name,
                Type = dto.Type,
                Description = metadata.Description,
                Link = dto.Link,
                PhotoPath = metadata.PhotoPath,
                Theme = dto.Theme
            };

            var validationErrors = await _materialValidator.ValidateAsync(material, material.Id);

            if (validationErrors.Any())
            {
                var errorMessages = validationErrors
                    .SelectMany(kv => kv.Value.Select(msg => $"{kv.Key}: {msg}"))
                    .ToList();

                return new MaterialResponseGeneral
                {
                    Id = Guid.Empty,
                    Name = null,
                    Description = string.Join("; ", errorMessages),
                    Type = 0,
                    Link = null,
                    PhotoPath = null,
                    Theme = null,
                };

            }
            else
            {

                await _materialRepository.AddAsync(material);

                return new MaterialResponseGeneral
                {
                    Id = material.Id,
                    Name = material.Name,
                    Type = material.Type,
                    Description = material.Description,
                    Link = material.Link,
                    PhotoPath = material.PhotoPath,
                    Theme = material.Theme,
                };
            }
        }

        public Task<IEnumerable<Material>> GetAllAsync()
        {
            return _materialRepository.GetAllAsync();
        }

        public Task<Material> GetByIdAsync(Guid id)
        {
            return _materialRepository.GetByIdAsync(id);
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Material> dto)
        {
            if (dto == null)
                return new Response { Success = false, Message = "Patch document is null" };

            // Перевірка, що всі операції патча — тільки по Type, Theme, Link
            var allowedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "/Type",
        "/Theme",
        "/Link"
    };

            var forbiddenOps = dto.Operations
                .Where(op => !allowedPaths.Contains(op.path))
                .ToList();

            if (forbiddenOps.Any())
            {
                return new Response
                {
                    Success = false,
                    Message = "You can only modify the following fields: Type, Theme, Link."
                };
            }

            var material = await _materialRepository.GetByIdAsync(id);

            if (material == null)
                return new Response { Success = false, Message = $"Material with ID {id} not found" };

            dto.ApplyTo(material);

            var validationErrors = await _materialValidator.ValidateAsync(material, id);

            if (validationErrors.Any())
            {
                var errorMessages = validationErrors
                    .SelectMany(kv => kv.Value.Select(msg => $"{kv.Key}: {msg}"))
                    .ToList();

                return new Response
                {
                    Success = false,
                    Message = "Validation failed: " + string.Join("; ", errorMessages)
                };
            }

            await _materialRepository.UpdateAsync(material);

            return new Response { Success = true, Message = "Material updated successfully" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var material = await _materialRepository.GetByIdAsync(id);

            if (material == null)
                return new Response { Success = false, Message = $"Material with ID {id} not found" };

            await _materialRepository.DeleteAsync(material.Id);

            return new Response { Success = true, Message = "Material deleted successfully" };
        }

        public async Task<Response> GetAllMaterialsTypes()
        {
            var types = Enum.GetValues(typeof(TypeEnum))
                            .Cast<TypeEnum>()
                            .ToDictionary(t => (int)t, t => t.ToString());

            return new Response
            {
                Success = true,
                Message = "Material types retrieved successfully",
                Data = types
            };
        }

        public async Task<Response> GetAllMaterialsThemes()
        {
            var themes = Enum.GetValues(typeof(ThemeEnum))
                             .Cast<ThemeEnum>()
                             .Select((value, index) => new
                             {
                                 Index = index,
                                 Description = value.GetType()
                                                    .GetMember(value.ToString())
                                                    .First()
                                                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                    .Cast<DescriptionAttribute>()
                                                    .FirstOrDefault()?.Description ?? value.ToString()
                             })
                             .ToDictionary(x => x.Index, x => x.Description);

            return new Response
            {
                Success = true,
                Message = "Themes retrieved successfully",
                Data = themes
            };
        }


    }
}
