using InnovaGraphics.Models;
using InnovaGraphics.Repositories.Interfaces;
using InnovaGraphics.Services.Interfaces;

namespace InnovaGraphics.Services.Implementations
{
    public class UserPlanetService : IUserPlanetService
    {
        private readonly IUserService _userService;
        private readonly IExerciseService _exerciseService;
        private readonly IPlanetService _planetService;
        private readonly IUserRepository _userRepository;

        public UserPlanetService(IUserService userService, IExerciseService exerciseService, IPlanetService planetService, IUserRepository userRepository = null)
        {
            _userService = userService;
            _exerciseService = exerciseService;
            _planetService = planetService;
            _userRepository = userRepository;
        }

        public async Task<bool> ConnectUserToPlanetAfterExerciseAsync(string userId, Guid exerciseId)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);

            if (exercise != null && exercise.PlanetId.HasValue)
            {
                Guid planetId = exercise.PlanetId.Value;
                var user = await _userRepository.GetByIdWithPlanetsAsync(userId);

                if (user != null)
                {
                    var planet = await _planetService.GetByIdAsync(planetId);

                    if (planet != null)
                    {
                        if (!user.Planets.Any(p => p.Id == planet.Id))
                        {
                            await _userRepository.AddPlanetToUserAsync(userId, planetId);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Користувач {userId} вже пов'язаний з планетою {planetId}.");
                            return true;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Не вдалося отримати планету з ID {planetId}.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Не вдалося отримати користувача з ID {userId}.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"Не вдалося отримати вправу з ID {exerciseId} або вправа не пов'язана з планетою.");
                return false;
            }
        }

        public async Task<int> GetUserPlanetCountAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                return user.Planets?.Count ?? 0;
            }
            return 0;
        }
    }
}