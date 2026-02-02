using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.QuizzQuestions.Models;
using Ressource_API.Features.QuizzQuestions.QuizzQuestionDtos;
using Ressource_API.Features.QuizzQuestions.Services;

namespace Ressource_API.Features.QuizzQuestions;

[ApiController]
[Route("api/[controller]")]
public class QuizzQuestionController : ControllerBase
{
    private readonly IQuizzQuestionService _service;
    private readonly ILogger<QuizzQuestionController> _logger;

    public QuizzQuestionController(IQuizzQuestionService service, ILogger<QuizzQuestionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all quizzquestions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<QuizzQuestion>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QuizzQuestion>>> GetAllQuizzQuestions(CancellationToken cancellationToken)
    {
        try
        {
            var quizzquestions = await _service.GetAllQuizzQuestionsAsync(cancellationToken);
            return Ok(quizzquestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all quizzquestions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving quizzquestions");
        }
    }

    /// <summary>
    /// Get a quizzquestion by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuizzQuestion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuizzQuestion>> GetQuizzQuestionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var quizzquestion = await _service.GetQuizzQuestionByIdAsync(id, cancellationToken);

            if (quizzquestion == null)
            {
                return NotFound($"QuizzQuestion with ID {id} not found");
            }

            return Ok(quizzquestion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quizzquestion with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the quizzquestion");
        }
    }

    /// <summary>
    /// Create a new quizzquestion
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(QuizzQuestion), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzQuestion>> CreateQuizzQuestion([FromBody] CreateQuizzQuestionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdQuizzQuestion = await _service.CreateQuizzQuestionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetQuizzQuestionById),
                new { id = createdQuizzQuestion.Id },
                createdQuizzQuestion
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quizzquestion");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the quizzquestion");
        }
    }

    /// <summary>
    /// Update an existing quizzquestion
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(QuizzQuestion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzQuestion>> UpdateQuizzQuestion(int id, [FromBody] UpdateQuizzQuestionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedQuizzQuestion = await _service.UpdateQuizzQuestionAsync(id, dto, cancellationToken);

            if (updatedQuizzQuestion == null)
            {
                return NotFound($"QuizzQuestion with ID {id} not found");
            }

            return Ok(updatedQuizzQuestion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quizzquestion with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the quizzquestion");
        }
    }

    /// <summary>
    /// Delete a quizzquestion
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuizzQuestion(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteQuizzQuestionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"QuizzQuestion with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting quizzquestion with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the quizzquestion");
        }
    }
}
