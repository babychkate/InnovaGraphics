using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class HintService : IHintService
    {
        private readonly IHintRepository _hintRepository;

        public HintService(IHintRepository hintRepository)
        {
            _hintRepository = hintRepository;
        }

        public async Task<IEnumerable<Hint>> GetAllAsync()
        {
            return await _hintRepository.GetAllAsync();
        }

        public async Task<Hint> GetByIdAsync(Guid id)
        {
            return await _hintRepository.GetByIdAsync(id);
        }

        public async Task<Response> AddAsync(Hint hint)
        {
            if (hint == null)
            {
                return new Response { Success = false, Message = "Invalid hint data" };
            }

            await _hintRepository.AddAsync(hint);
            return new Response { Success = true, Message = "Hint added successfully" };
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Hint> patchDoc)
        {
            var entity = await _hintRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return new Response { Success = false, Message = "Hint not found" };
            }

            patchDoc.ApplyTo(entity);
            await _hintRepository.UpdateAsync(entity);

            return new Response { Success = true, Message = "Hint updated successfully" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var entity = await _hintRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return new Response { Success = false, Message = "Hint not found" };
            }

            await _hintRepository.DeleteAsync(id);
            return new Response { Success = true, Message = "Hint deleted successfully" };
        }
    }
}