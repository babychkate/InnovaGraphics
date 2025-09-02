using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Models;
using InnovaGraphics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace InnovaGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ITestService _testService;


        public QuestionController(IQuestionService questionService, ITestService testService)
        {
            _questionService = questionService;
            _testService = testService;
        }

        [HttpGet("get-test-questions")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsByTestId(Guid testId)
        {
            var questions = await _questionService.GetQuestionsByTestIdAsync(testId);
            return Ok(questions);
        }

        [HttpGet("get-question-by-id/{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(Guid id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("create-question")]
        public async Task<ActionResult<Response>> CreateQuestion(Guid testId, [FromBody] CreateQuestionDto newQuestionDto)
        {
            var testExists = await _testService.GetByIdAsync(testId);
            if (testExists == null)
            {
                return NotFound($"Тест з ID '{testId}' не знайдено.");
            }

            var response = await _questionService.CreateQuestionAsync(newQuestionDto, testId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch("update-question/{id}")]
        public async Task<ActionResult<Response>> UpdateQuestion(Guid id, [FromBody] JsonPatchDocument<Question> patchDoc)
        {
            var response = await _questionService.UpdateQuestionAsync(id, patchDoc);
            if (response.Success)
            {
                return Ok(response); // Можна змінити на NoContent (204) якщо не потрібно повертати тіло
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("delete-question/{id}")]
        public async Task<ActionResult<Response>> DeleteQuestion(Guid id)
        {
            var response = await _questionService.DeleteQuestionAsync(id);
            if (response.Success)
            {
                return Ok(response); // Можна змінити на NoContent (204) якщо не потрібно повертати тіло
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}