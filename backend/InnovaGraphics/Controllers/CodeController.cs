using InnovaGraphics.Interactions;
using InnovaGraphics.Services.Interfaces;
using InnovaGraphics.Utils.Strategy.Interfaces;
using InnovaGraphics.Utils.Strategy.Lab1.Implementations;
using InnovaGraphics.Utils.Strategy.Lab2.Implementations;
using InnovaGraphics.Utils.Strategy.Lab3.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InnovaGraphics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly ICodeExecutor<CodeRequest, CodeResponse> _lab1Executor;
        private readonly ICodeExecutor<CodeRequest, CodeResponse> _lab2Executor;
        private readonly ICodeExecutor<CodeRequest, CodeResponse> _lab3Executor;
        private readonly ICodeExecutor<CodeRequest4, CodeResponse4> _lab4Executor;
        private readonly ICodeExecutor<CodeRequest5, CodeResponse5> _lab5Executor;

        private readonly ICodeTestService _testService;
        private readonly IUserService _userService;
        private readonly IUserPlanetService _userPlanetService; 

        public CodeController(
           ICodeExecutor<CodeRequest, CodeResponse> lab1Executor, 
           ICodeExecutor<CodeRequest, CodeResponse> lab2Executor,
           ICodeExecutor<CodeRequest, CodeResponse> lab3Executor,
           ICodeExecutor<CodeRequest4, CodeResponse4> lab4Executor,
           ICodeExecutor<CodeRequest5, CodeResponse5> lab5Executor,          
           ICodeTestService testService,
            IUserService userService,
            IUserPlanetService userPlanetService) 
        {
            _lab1Executor = lab1Executor;
            _lab2Executor = lab2Executor;
            _lab4Executor = lab4Executor;
            _lab5Executor = lab5Executor;
            _testService = testService;
            _userService = userService;
            _userPlanetService = userPlanetService;
        }

        [Authorize(Roles = "Student")]
        [HttpPost("run1")]
        public async Task<IActionResult> Run1([FromBody] CodeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode))
            {
                return BadRequest("Вихідний код потрібен для завдання Трикутників.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            var lab1Executor = new CodeExecutorLab1();
            CodeResponse result = await lab1Executor.ExecuteAsync(request);
            if (result.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 80, 20);
                bool connectionResult = await _userPlanetService.ConnectUserToPlanetAfterExerciseAsync(userId, request.ExerciseId);

                if (!connectionResult)
                {
                    Console.WriteLine($"Помилка при встановленні зв'язку користувача {userId} з планетою після вправи {request.ExerciseId}.");
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("run")]
        public async Task<IActionResult> Run([FromBody] CodeRequest request) 
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode))
            {
                return BadRequest("Вихідний код потрібен для завдання Безьє.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            var lab2Executor = new CodeExecutorLab2();
            CodeResponse result = await lab2Executor.ExecuteAsync(request);
            if (result.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 100, 40);
                bool connectionResult = await _userPlanetService.ConnectUserToPlanetAfterExerciseAsync(userId, request.ExerciseId);

                if (!connectionResult)
                {
                    Console.WriteLine($"Помилка при встановленні зв'язку користувача {userId} з планетою після вправи {request.ExerciseId}.");
                }
            }
            return Ok(result); 
        }

        [Authorize(Roles = "Student")]
        [HttpPost("run3")]
        public async Task<IActionResult> Run3([FromBody] CodeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode))
            {
                return BadRequest("Вихідний код потрібен для завдання Безьє.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            var lab3Executor = new CodeExecutorLab3();
            CodeResponse result = await lab3Executor.ExecuteAsync(request);
            if (result.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 200, 50);
                bool connectionResult = await _userPlanetService.ConnectUserToPlanetAfterExerciseAsync(userId, request.ExerciseId);

                if (!connectionResult)
                {
                    Console.WriteLine($"Помилка при встановленні зв'язку користувача {userId} з планетою після вправи {request.ExerciseId}.");
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("run4")] 
        public async Task<IActionResult> Run([FromBody] CodeRequest4 request) 
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode) || string.IsNullOrEmpty(request.SourceImageBase64))
            {
                return BadRequest("SourceCode і SourceImageBase64 потрібні для завдання ImageProcessing.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            CodeResponse4 result = await _lab4Executor.ExecuteAsync(request);
            if (result.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 190, 110);
                bool connectionResult = await _userPlanetService.ConnectUserToPlanetAfterExerciseAsync(userId, request.ExerciseId);

                if (!connectionResult)
                {
                    Console.WriteLine($"Помилка при встановленні зв'язку користувача {userId} з планетою після вправи {request.ExerciseId}.");
                }
            }
            return Ok(result); 
        }

        [Authorize(Roles = "Student")]
        [HttpPost("run5")]
        public async Task<IActionResult> Run5([FromBody] CodeRequest5 request)
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode))
            {
                return BadRequest("Вихідний код потрібен для завдання Безьє.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            CodeResponse5 result = await _lab5Executor.ExecuteAsync(request);
            if (result.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 170, 60);
                bool connectionResult = await _userPlanetService.ConnectUserToPlanetAfterExerciseAsync(userId, request.ExerciseId);

                if (!connectionResult)
                {
                    Console.WriteLine($"Помилка при встановленні зв'язку користувача {userId} з планетою після вправи {request.ExerciseId}.");
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = "Student")]
        [HttpPost("test")]
        public async Task<IActionResult> ExecuteCodeWithTests([FromBody] CodeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SourceCode) || string.IsNullOrEmpty(request.TaskType) || request.ExerciseId == Guid.Empty)
            {
                return BadRequest("Потрібні SourceCode, TaskType та ExerciseId.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Ідентифікатор користувача не знайдено в токені.");
            }

            TestResult testResult = await _testService.CompileAndRunTest(request.SourceCode, request.TaskType, request.ExerciseId);
            if (testResult.Success)
            {
                var addPointsResult = await _userService.AddMarksAndCoinsAsync(userId, 90, 40);
            }

            return Ok(testResult);
        }
    }
}