using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Services.Implementations
{
    public class PlanetService : IPlanetService
    {
        private readonly IPlanetRepository _planetRepository;
        private const int MinLength = 2;
        private const int MaxLength = 30;
        private const int ImageLength = 5000;

        public PlanetService(IPlanetRepository planetRepository) 
        {
            _planetRepository = planetRepository;
        }

        // CREATE
        public async Task<Response> CreatePlanetAsync(CreatePlanetDto newPlanetDto)
        {
            var validationErrors = await ValidatePlanetCreationAsync(newPlanetDto);

            if (validationErrors.Any())
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Невалідні дані для створення планети.",
                    ValidationErrors = validationErrors
                };
            }

            var newPlanet = new Planet
            {
                Id = Guid.NewGuid(),
                Name = newPlanetDto.Name,
                Topic = newPlanetDto.Topic,
                SubTopic = newPlanetDto.SubTopic,
                ImagePath = newPlanetDto.ImagePath,
                RequiredEnergy = newPlanetDto.RequiredEnergy,
                EnergyLost = newPlanetDto.EnergyLost,
                Number = newPlanetDto.Number,
                IsUnlock = (newPlanetDto.Number == 1) ? true: false,
                MaxHintCount = newPlanetDto.MaxHintCount,
                CurrentHintCount = 1,
                Theories = new List<Theory>(),
                Tests = new List<Test>(),
                Exercises = new List<Exercise>(),
                PlanetInfos = new List<PlanetInfo>(),
                Methods = new List<Method>(),
                Hints = new List<Hint>(),
                Users = new List<User>()

            };

            await _planetRepository.AddAsync(newPlanet);
            return new Response { Success = true, StatusCode = 201, Message = "Планету успішно створено!" };
        }


        private async Task<Dictionary<string, List<string>>> ValidatePlanetCreationAsync(CreatePlanetDto newPlanetDto)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Валідація Name
            if (string.IsNullOrEmpty(newPlanetDto.Name) || newPlanetDto.Name.Length < MinLength || newPlanetDto.Name.Length > MaxLength) // Припустимі межі довжини
            {
                if (!validationErrors.ContainsKey("name")) validationErrors["name"] = new List<string>();
                var message = string.IsNullOrEmpty(newPlanetDto.Name) ? "Назва планети не може бути порожнім полем. Будь ласка, введіть назву." :
                              $"Назва планети має бути від {MinLength} до {MaxLength} символів. Будь ласка, введіть назву повторно.";
                validationErrors["name"].Add(message);
            }
            else
            {
                // Перевірка на унікальність назви (можна винести в сервіс, але для прикладу тут)
                var existingPlanet = await _planetRepository.GetByNameAsync(newPlanetDto.Name);
                if (existingPlanet != null)
                {
                    if (!validationErrors.ContainsKey("name")) validationErrors["name"] = new List<string>();
                    validationErrors["name"].Add($"Планета з назвою '{newPlanetDto.Name}' вже існує. Будь ласка, оберіть іншу назву.");
                }
            }

            // Валідація Topic
            if (string.IsNullOrEmpty(newPlanetDto.Topic) || newPlanetDto.Topic.Length < MinLength || newPlanetDto.Topic.Length > MaxLength)
            {
                if (!validationErrors.ContainsKey("topic")) validationErrors["topic"] = new List<string>();
                var message = string.IsNullOrEmpty(newPlanetDto.Topic) ? "Тема планети не може бути порожнім полем. Будь ласка, введіть тему." :
                              $"Тема планети має бути від {MinLength} до {MaxLength} символів. Будь ласка, введіть тему повторно.";
                validationErrors["topic"].Add(message);
            }

            // Валідація SubTopic (може бути необов'язковим, залежить від вимог)
            if (newPlanetDto.SubTopic.Length < MinLength || newPlanetDto.SubTopic.Length > MaxLength)
            {
                if (!validationErrors.ContainsKey("subtopic")) validationErrors["subtopic"] = new List<string>();
                var message = string.IsNullOrEmpty(newPlanetDto.SubTopic) ? "Підтема планети не може бути порожнім полем. Будь ласка, введіть підтему." :
                              $"Підтема планети має бути від {MinLength} до {MaxLength} символів. Будь ласка, введіть підтему повторно.";
                validationErrors["subtopic"].Add(message);
            }

            // Валідація RequiredEnergy (припустимо, має бути позитивним числом)
            if (newPlanetDto.RequiredEnergy <= 0)
            {
                if (!validationErrors.ContainsKey("requiredenergy")) validationErrors["requiredenergy"] = new List<string>();
                validationErrors["requiredenergy"].Add("Необхідна енергія має бути більшою за нуль.");
            }

            // Валідація EnergyLost
            if (newPlanetDto.EnergyLost < 0)
            {
                if (!validationErrors.ContainsKey("energylost")) validationErrors["energylost"] = new List<string>();
                validationErrors["energylost"].Add("Втрачена енергія не може бути від'ємною.");
            }

            // Валідація Number
            if (newPlanetDto.Number <= 0 || newPlanetDto.Number > 5)
            {
                if (!validationErrors.ContainsKey("number")) validationErrors["number"] = new List<string>();
                validationErrors["number"].Add("Номер планети має бути більшим за нуль та меншим за 5.");
            }
            else
            {
                var existingPlanetWithNumber = await _planetRepository.GetByNumberAsync(newPlanetDto.Number);
                if (existingPlanetWithNumber != null)
                {
                    if (!validationErrors.ContainsKey("number")) validationErrors["number"] = new List<string>();
                    validationErrors["number"].Add($"Планета з номером '{newPlanetDto.Number}' вже існує.");
                }
            }

            if (newPlanetDto.MaxHintCount <= 0)
            {
                if (!validationErrors.ContainsKey("maxhintcount")) validationErrors["maxhintcount"] = new List<string>();
                validationErrors["maxhintcount"].Add("Максимальна кількість підказок має бути більшою за нуль.");
            }

            // ImagePath (можна додати валідацію формату, розміру тощо, якщо потрібно)
            if (!string.IsNullOrEmpty(newPlanetDto.ImagePath) && newPlanetDto.ImagePath.Length > ImageLength) // Припустима максимальна довжина шляху
            {
                if (!validationErrors.ContainsKey("imagepath")) validationErrors["imagepath"] = new List<string>();
                validationErrors["imagepath"].Add("Шлях до зображення не може бути довшим за 200 символів.");
            }

            return validationErrors;
        }

        // READ
        public async Task<Planet> GetByIdAsync(Guid id)
        {
            return await _planetRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Planet>> GetAllAsync()
        {
            return await _planetRepository.GetAllAsync();
        }

        //UPDATE
        public async Task<Response> UpdateAsync(Guid id, JsonPatchDocument<Planet> patchDoc)
        {
            var planetToUpdate = await _planetRepository.GetByIdAsync(id);
            if (planetToUpdate == null)
            {
                return new Response { Success = false, StatusCode = 404, Message = $"Планету з ID '{id}' не знайдено." };
            }

            // Застосовуємо зміни з patchDoc до planetToUpdate
            patchDoc.ApplyTo(planetToUpdate);

            var validationErrors = await ValidatePlanetUpdateAsync(planetToUpdate, patchDoc);

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

            await _planetRepository.UpdateAsync(planetToUpdate);
            return new Response { Success = true, StatusCode = 204, Message = "Планету успішно оновлено" };
        }

        private async Task<Dictionary<string, List<string>>> ValidatePlanetUpdateAsync(Planet planetToValidate, JsonPatchDocument<Planet> patchDoc)
        {
            var validationErrors = new Dictionary<string, List<string>>();

            // Валідація Name, якщо воно є в patchDoc
            if (patchDoc.Operations.Any(op => op.path.ToLower() == "/name"))
            {
                if (string.IsNullOrEmpty(planetToValidate.Name) || planetToValidate.Name.Length < MinLength || planetToValidate.Name.Length > MaxLength)
                {
                    if (!validationErrors.ContainsKey("name")) validationErrors["name"] = new List<string>();
                    var message = string.IsNullOrEmpty(planetToValidate.Name) ? "Назва планети не може бути порожнім полем." :
                                  $"Назва планети має бути від {MinLength} до {MaxLength} символів.";
                    validationErrors["name"].Add(message);
                }
                else
                {
                    // Перевірка на унікальність назви (виключаючи поточну планету)
                    var existingPlanet = await _planetRepository.GetByNameAsync(planetToValidate.Name);
                    if (existingPlanet != null && existingPlanet.Id != planetToValidate.Id)
                    {
                        if (!validationErrors.ContainsKey("name")) validationErrors["name"] = new List<string>();
                        validationErrors["name"].Add($"Планета з назвою '{planetToValidate.Name}' вже існує.");
                    }
                }

                // Валідація Topic, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/topic"))
                {
                    if (string.IsNullOrEmpty(planetToValidate.Topic) || planetToValidate.Topic.Length < MinLength || planetToValidate.Topic.Length > MaxLength)
                    {
                        if (!validationErrors.ContainsKey("topic")) validationErrors["topic"] = new List<string>();
                        var message = string.IsNullOrEmpty(planetToValidate.Topic) ? "Тема планети не може бути порожнім полем." :
                                      $"Тема планети має бути від {MinLength} до {MaxLength} символів.";
                        validationErrors["topic"].Add(message);
                    }
                }

                // Валідація SubTopic, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/subtopic"))
                {
                    if (planetToValidate.SubTopic.Length < MinLength || planetToValidate.SubTopic.Length > MaxLength)
                    {
                        if (!validationErrors.ContainsKey("subtopic")) validationErrors["subtopic"] = new List<string>();
                        var message = string.IsNullOrEmpty(planetToValidate.SubTopic) ? "Підема планети не може бути порожнім полем." :
                                      $"Підтема планети має бути від {MinLength} до {MaxLength} символів.";
                        validationErrors["subtopic"].Add(message);
                    }
                }

                // Валідація EnergyLost, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/energylost"))
                {
                    if (planetToValidate.EnergyLost < 0)
                    {
                        if (!validationErrors.ContainsKey("energylost")) validationErrors["energylost"] = new List<string>();
                        validationErrors["energylost"].Add("Втрачена енергія не може бути від'ємною.");
                    }
                }

                // Валідація Number, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/number"))
                {
                    if (planetToValidate.Number <= 0 || planetToValidate.Number > 5)
                    {
                        if (!validationErrors.ContainsKey("number")) validationErrors["number"] = new List<string>();
                        validationErrors["number"].Add("Номер планети має бути більшим за нуль та меншим за 5.");
                    }
                    else
                    {
                        // Перевірка на унікальність номера (виключаючи поточну планету)
                        var existingPlanetWithNumber = await _planetRepository.GetByNumberAsync(planetToValidate.Number);
                        if (existingPlanetWithNumber != null && existingPlanetWithNumber.Id != planetToValidate.Id)
                        {
                            if (!validationErrors.ContainsKey("number")) validationErrors["number"] = new List<string>();
                            validationErrors["number"].Add($"Планета з номером '{planetToValidate.Number}' вже існує.");
                        }
                    }
                }

                // Валідація MaxHintCount, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/maxhintcount"))
                {
                    if (planetToValidate.MaxHintCount <= 0)
                    {
                        if (!validationErrors.ContainsKey("maxhintcount")) validationErrors["maxhintcount"] = new List<string>();
                        validationErrors["maxhintcount"].Add("Максимальна кількість підказок має бути більшою за нуль.");
                    }
                }

                // Валідація ImagePath, якщо воно є в patchDoc
                if (patchDoc.Operations.Any(op => op.path.ToLower() == "/imagepath"))
                {
                    if (!string.IsNullOrEmpty(planetToValidate.ImagePath) && planetToValidate.ImagePath.Length > ImageLength)
                    {
                        if (!validationErrors.ContainsKey("imagepath")) validationErrors["imagepath"] = new List<string>();
                        validationErrors["imagepath"].Add("Шлях до зображення не може бути довшим за 200 символів.");
                    }
                }
            }
            
            return validationErrors;
        }

        // DELETE
        public async Task<Response> DeleteAsync(Guid id)
        {
            var existingPlanet = await _planetRepository.GetByIdAsync(id);
            if (existingPlanet == null)
            {
                return new Response
                {
                    Success = false,
                    StatusCode = 404,
                    Message = $"Планету з ID '{id}' не знайдено."
                };
            }

            await _planetRepository.DeleteAsync(id);

            return new Response
            {
                Success = true,
                StatusCode = 200,
                Message = $"Планету з ID '{id}' успішно видалено."
            };
        }

        // COLLECTION GETTERS
        public async Task<IEnumerable<string>> GetAllPlanetTopicsAsync()
        {
           return await _planetRepository.GetAllPlanetTopicsAsync();
        }
        public async Task<IEnumerable<string>> GetAllPlanetSubTopicsAsync()
        {
            return await _planetRepository.GetAllPlanetSubTopicsAsync();
        }
        public async Task<IEnumerable<string>> GetAllPlanetNamesAsync()
        {
            return await _planetRepository.GetAllPlanetNamesAsync();
        }

    }
}
