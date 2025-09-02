using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace InnovaGraphics.Services.Implementations
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly IRepository<Exercise> _exerciseRepository;
        private const int MinLength = 10;
        private const int MaxLength = 100;

        public CaseService(ICaseRepository caseRepository, IRepository<Exercise> exerciseRepository) 
        {
            _caseRepository = caseRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Response> CreateAsync(CreateCaseDto dto)
        {
            var validationErrors = await ValidateCaseCreationAsync(dto);

            if (validationErrors.Any()) {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Невалідні дані для створення кейсу.",
                    ValidationErrors = validationErrors
                };
            }

            var newCase = new Case
            {
                Id = Guid.NewGuid(),
                Input = dto.Input,
                Output = dto.Output,
                ExerciseId = dto.ExerciseId
            };

            await _caseRepository.AddAsync(newCase);
            return new Response { Success = true, StatusCode = 201, Message = "Кейс успішно створено!" };
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            var existingCase = await _caseRepository.GetByIdAsync(id);
            if (existingCase == null) {
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = $"Кейсу з ID '{id}' не знайдено."
                };
            }

            await _caseRepository.DeleteAsync(id);
            return new Response
            {
                Success = true,
                StatusCode = 200,
                Message = $"Кейс з ID '{id}' успішно видалено."
            };
        }

        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Case> dto)
        {
            var caseToUpdate = await _caseRepository.GetByIdAsync(id);
            if (caseToUpdate == null)
            {
                return new Response { Success = false, StatusCode = 404, Message = $"Кейс з ID '{id}' не знайдено." };
            }
            dto.ApplyTo(caseToUpdate);

            var validationErrors = await ValidateCaseUpdateAsync(caseToUpdate, dto);

            if (validationErrors.Any())
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Невалідні дані для оновлення планети.",
                    ValidationErrors = validationErrors
                };
            }

            await _caseRepository.UpdateAsync(caseToUpdate);
            return new Response { Success = true, StatusCode = 204, Message = "Планету успішно оновлено" };
        }

        async Task<IEnumerable<Case>> IBaseService<Case>.GetAllAsync()
        {
            return await _caseRepository.GetAllAsync();
        }

        public async Task<Case> GetByIdAsync(Guid id)
        {
            return await _caseRepository.GetByIdAsync(id);
        }

        //VALIDATIONS
        private async Task<Dictionary<string, List<string>>> ValidateCaseCreationAsync(CreateCaseDto newCaseDto) {
            var validationErrors = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(newCaseDto.Input) || 
                newCaseDto.Input.Length < MinLength || newCaseDto.Input.Length > MaxLength)                
            {
                if (!validationErrors.ContainsKey("input")) validationErrors["input"] = new List<string>();
                var message = string.IsNullOrEmpty(newCaseDto.Input) ? "Вхідні дані для кейсу не може бути порожнім полем. Будь ласка, введіть назву." :
                              $"Вхідні дані мають бути від {MinLength} до {MaxLength} символів. Будь ласка, введіть назву повторно.";
                validationErrors["input"].Add(message);
            }

            if (string.IsNullOrEmpty(newCaseDto.Output) ||
               newCaseDto.Output.Length < MinLength || newCaseDto.Output.Length > MaxLength)
            {
                if (!validationErrors.ContainsKey("output")) validationErrors["output"] = new List<string>();
                
                var message = string.IsNullOrEmpty(newCaseDto.Output) ? "Вихідні дані для кейсу не може бути порожнім полем. Будь ласка, введіть назву." :
                              $"Вихідні дані мають бути від {MinLength} до {MaxLength} символів. Будь ласка, введіть назву повторно.";
                validationErrors["output"].Add(message);
            }

            var exercise = await _exerciseRepository.GetByIdAsync(newCaseDto.ExerciseId); // Використовуємо IRepository<Exercise>
            if (exercise == null)
            {
                if (!validationErrors.ContainsKey("exerciseId")) validationErrors["exerciseId"] = new List<string>();
                validationErrors["exerciseId"].Add("Вправа з вказаним ExerciseId не існує.");
            }

            return validationErrors;
        }

        private async Task<Dictionary<string, List<string>>> ValidateCaseUpdateAsync(Case caseToValidate, JsonPatchDocument<Case> patchDoc) {
            var validationErrors = new Dictionary<string, List<string>>();

            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/input"))
            {
                if (string.IsNullOrEmpty(caseToValidate.Input) || caseToValidate.Input.Length < MinLength ||
                    caseToValidate.Input.Length > MaxLength)
                {
                    if (!validationErrors.ContainsKey("input")) validationErrors["input"] = new List<string>();
                    var message = string.IsNullOrEmpty(caseToValidate.Input) ? "Вхідні дані не можуть бути порожнім полем." :
                                  $"Вхіні дані мають бути від {MinLength} до {MaxLength} символів.";
                    validationErrors["input"].Add(message);
                }
            }

            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/output"))
            {
                if (string.IsNullOrEmpty(caseToValidate.Input) || caseToValidate.Input.Length < MinLength ||
                    caseToValidate.Input.Length > MaxLength)
                {
                    if (!validationErrors.ContainsKey("output")) validationErrors["output"] = new List<string>();
                    var message = string.IsNullOrEmpty(caseToValidate.Input) ? "Вихідні дані не можуть бути порожнім полем." :
                                  $"Вихіні дані мають бути від {MinLength} до {MaxLength} символів.";
                    validationErrors["output"].Add(message);
                }
            }

            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/exerciseid"))
            {
                var exercise = await _exerciseRepository.GetByIdAsync(caseToValidate.ExerciseId);
                if (caseToValidate.ExerciseId == Guid.Empty || exercise == null) 
                {
                    if (!validationErrors.ContainsKey("exerciseid")) validationErrors["exerciseid"] = new List<string>();
                    validationErrors["exerciseid"].Add("Вправа з вказаним ExerciseId не існує.");
                }
            }

            return validationErrors;
        }
    
    
    }
}
