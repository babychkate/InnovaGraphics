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
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;

        public AnswerController(IAnswerService answerService, IQuestionService questionService)
        {
            _answerService = answerService;
            _questionService = questionService;
        }

        [HttpGet("get-question-answers")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswersByQuestionId(Guid questionId)
        {
            var answers = await _answerService.GetAnswersByQuestionIdAsync(questionId);
            return Ok(answers);
        }

        [HttpGet("get-answer-by-id/{id}")]
        public async Task<ActionResult<Answer>> GetAnswerById(Guid id)
        {
            var answer = await _answerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound("Відповідь не знайдено");
            }
            return Ok(answer);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("create-answer")]
        public async Task<ActionResult<Response>> CreateAnswer(Guid questionId, [FromBody] CreateAnswerDto newAnswerDto)
        {
            var questionExists = await _questionService.GetQuestionByIdAsync(questionId);
            if (questionExists == null)
            {
                return NotFound($"Питання з ID '{questionId}' не знайдено.");
            }

            var response = await _answerService.CreateAnswerAsync(questionId, newAnswerDto);
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
        [HttpPatch("update-answer/{id}")]
        public async Task<ActionResult<Response>> UpdateAnswer(Guid id, [FromBody] JsonPatchDocument<Answer> patchDoc)
        {
            var response = await _answerService.UpdateAnswerAsync(id, patchDoc);
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
        [HttpDelete("delete-answer/{id}")]
        public async Task<ActionResult<Response>> DeleteAnswer(Guid id)
        {
            var response = await _answerService.DeleteAnswerAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}