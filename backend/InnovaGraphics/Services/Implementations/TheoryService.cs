using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class TheoryService : ITheoryService
    {
        private readonly IRepository<Theory> _theoryRepository;

        public TheoryService(IRepository<Theory> theoryRepository)
        {
            _theoryRepository = theoryRepository;
        }

        public async Task<Response> CreateAsync(CreateTheoryDto dto)
        {
            var newTheory = new Theory
            {
                Id = Guid.NewGuid(),
                Content = dto.Content,
                PlanetId = dto.PlanetId
            };

            await _theoryRepository.AddAsync(newTheory);
            return new Response { Success = true, StatusCode = 200 };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var existingTheory = await _theoryRepository.GetByIdAsync(id);
            if (existingTheory == null) {
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = $"Теорію з ID '{id}' не знайдено."
                };
            }

            await _theoryRepository.DeleteAsync(id);
            return new Response
            {
                Success = true,
                StatusCode = 200,
                Message = $"Теорію з ID '{id}' успішно видалено."
            };
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Theory> dto)
        {
            var theoryToUpdate = await _theoryRepository.GetByIdAsync(id);
            if (theoryToUpdate == null)
            {
                return new Response { Success = false, StatusCode = 404, Message = $"Теорію з ID '{id}' не знайдено." };
            }
            dto.ApplyTo(theoryToUpdate);

            await _theoryRepository.UpdateAsync(theoryToUpdate);
            return new Response { Success = true, StatusCode = 204, Message = "Теорію успішно оновлено" };
        }

        public async Task<IEnumerable<Theory>> GetAllAsync()
        {
            return await _theoryRepository.GetAllAsync();
        }

        public async Task<Theory> GetByIdAsync(Guid id)
        {
            return await _theoryRepository.GetByIdAsync(id);
        }        
    }
}
