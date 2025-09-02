using InnovaGraphics.Dtos;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [Authorize(Roles = "Teacher")]
        //CREATE
        [HttpPost("create-test")]
        public async Task<ActionResult<TestResponseGeneralDto>> CreateTest([FromBody] CreateTestDto newTestDto)
        {
            if (newTestDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _testService.CreateTestAsync(newTestDto);

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return StatusCode((int)HttpStatusCode.Created, response);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("tests/{testId}/add-questions-answers")]
        public async Task<IActionResult> AddQuestionsAndAnswers(Guid testId, [FromBody] List<CreateQuestionWithAnswersDto> inputData)
        {
            if (inputData == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _testService.AddQuestionsWithAnswersToTestAsync(inputData, testId);

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetTestById), new { id = testId }, response);
        }

        // Новий ендпоінт для початку тесту
        [HttpPost("start-test/{testId}")]
        public async Task<IActionResult> StartTest(Guid testId, [FromBody] StartTestRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserEmail) || testId == Guid.Empty)
            {
                return BadRequest(new { Message = "Невалідний запит для початку тесту." });
            }

            var response = await _testService.StartTestAsync(request.UserEmail, testId);

            if (response == null)
            {
                return NotFound(new { Message = $"Тест з ID '{testId}' не знайдено або користувача не знайдено." });
            }

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        //READ
        [HttpGet("get-all-tests")]
        public async Task<ActionResult<IEnumerable<TestResponseGeneralDto>>> GetAllTests()
        {
            var tests = await _testService.GetAllAsync();
            return Ok(tests);
        }

        [HttpGet("get-test-with-id/{id}")]
        public async Task<ActionResult<TestResponseGeneralDto>> GetTestById(Guid id)
        {
            var test = await _testService.GetByIdAsync(id);

            if (test == null)
            {
                return NotFound(new { Message = $"Тест з ID '{id}' не знайдено." });
            }

            return Ok(test);
        }

        [HttpGet("get-test-with-questions/{id}")]
        public async Task<ActionResult<TestResponseGeneralDto>> GetTestByIdWithQuestions(Guid id)
        {
            var test = await _testService.GetTestByIdWithQuestionsAsync(id);

            if (test == null)
            {
                return NotFound(new { Message = $"Тест з ID '{id}' не знайдено." });
            }

            return Ok(test);
        }

        [HttpGet("get-test-with-questions-answers/{id}")]
        public async Task<ActionResult<Test>> GetTestByIdWithQuestionsAndAnswers(Guid id)
        {
            var test = await _testService.GetTestByIdWithQuestionsAndAnswersAsync(id);

            if (test == null)
            {
                return NotFound(new { Message = $"Тест з ID '{id}' не знайдено." });
            }

            return Ok(test);
        }

        [Authorize(Roles = "Teacher")]
        //UPDATE
        [HttpPatch("update-test/{id}")]
        public async Task<IActionResult> UpdateTest(Guid id, [FromBody] JsonPatchDocument<Test> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(new { Message = "Дані для оновлення відсутні." });
            }

            var response = await _testService.UpdateTestAsync(id, patchDoc);

            if (response == null)
            {
                return NotFound(new { Message = $"Тест з ID '{id}' не знайдено." });
            }

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // ЗАВЕРШИТИ ТЕСТ
        [HttpPost("complete-test/{testId}")]
        public async Task<IActionResult> CompleteTest(Guid testId, [FromBody] CompleteTestRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserEmail) || testId == Guid.Empty || request.UserAnswers == null || string.IsNullOrEmpty(request.EndTime))
            {
                return BadRequest(new { Message = "Невалідний запит для завершення тесту." });
            }

            var response = await _testService.CompleteTestAsync(request.UserEmail, testId, request.UserAnswers, request.EndTime);

            if (response == null)
            {
                return NotFound(new { Message = $"Тест з ID '{testId}' або користувача не знайдено." });
            }

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Teacher")]
        //DELETE
        [HttpDelete("delete-test/{id}")]
        public async Task<IActionResult> DeleteTest(Guid id)
        {
            var response = await _testService.DeleteTest(id);

            if (response == null)
            {
                return NotFound(new { Message = $"Тест з ID '{id}' не знайдено." });
            }

            if (response.ValidationErrors != null && response.ValidationErrors.Any())
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("get-test-by-subtopic/{subTopic}")]
        public async Task<ActionResult<TestResponseGeneralDto>> GetTestBySubTopic([FromRoute] string subTopic)
        {
            if (string.IsNullOrEmpty(subTopic))
            {
                return BadRequest(new { Message = "SubTopic не може бути порожнім." });
            }

            var response = await _testService.GetTestBySubTopicAsync(subTopic);

            if (response == null || !string.IsNullOrEmpty(response.Message) && response.Message.Contains("не знайдено"))
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("get-test-by-planet-id/{planetId}")]
        public async Task<ActionResult<TestResponseGeneralDto>> GetTestByPlanetId([FromRoute] Guid planetId)
        {
            if (planetId == Guid.Empty)
            {
                return BadRequest(new { Message = "ID планети не може бути порожнім." });
            }

            var response = await _testService.GetTestByPlanetIdAsync(planetId);

            if (response == null || !string.IsNullOrEmpty(response.Message) && response.Message.Contains("не знайдено"))
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}