using InnovaGraphics.Enums;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Utils.Validators.Interfaces;

namespace InnovaGraphics.Utils.Validators.Implementations
{
    public class MaterialValidator : IValidator<Material, Guid>
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialValidator(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<Dictionary<string, List<string>>> ValidateAsync(Material material, Guid id)
        {
            var errors = new Dictionary<string, List<string>>();

            // 1. Назва: не порожня, унікальна
            if (string.IsNullOrWhiteSpace(material.Name))
                AddValidationError(errors, nameof(material.Name), "Name can not be empty");

            var existing = await _materialRepository.GetByNameAsync(material.Name);
            if (existing != null && existing.Id != id)
                AddValidationError(errors, nameof(material.Name), "Material with such name already exists");

            // 2. Тип: повинен бути відомим enum'ом
            if (!Enum.IsDefined(typeof(TypeEnum), material.Type))
                AddValidationError(errors, nameof(material.Type), "Unknown type of material");

            // 3. Теми: перевірка кожного елемента списку
            if (material.Theme == null || material.Theme.Count == 0)
            {
                AddValidationError(errors, nameof(material.Theme), "Theme list cannot be empty");
            }
            else
            {
                foreach (var theme in material.Theme)
                {
                    if (!Enum.IsDefined(typeof(ThemeEnum), theme))
                        AddValidationError(errors, nameof(material.Theme), $"Unknown theme value: {theme}");
                }
            }

            // 4. Якщо Type = Video, то посилання має бути YouTube
            if (material.Type == TypeEnum.Video && (string.IsNullOrWhiteSpace(material.Link) || !material.Link.Contains("youtube", StringComparison.OrdinalIgnoreCase)))
            {
                AddValidationError(errors, nameof(material.Link), "If Type = Video, link must be a YouTube URL.");
            }

            // 5. Посилання: обов’язкове і має бути валідним URL
            if (string.IsNullOrWhiteSpace(material.Link))
                AddValidationError(errors, nameof(material.Link), "Link can not be empty");
            else if (!Uri.IsWellFormedUriString(material.Link, UriKind.Absolute))
                AddValidationError(errors, nameof(material.Link), "Link must be a valid URL address");

            return errors;
        }
        private void AddValidationError(Dictionary<string, List<string>> errors, string fieldName, string message)
        {
            if (!errors.ContainsKey(fieldName))
            {
                errors[fieldName] = new List<string>();
            }

            errors[fieldName].Add(message);
        }
    }
}
