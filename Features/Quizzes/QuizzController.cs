using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Quizzes.Models;
using Ressource_API.Features.Quizzes.QuizzDtos;
using Ressource_API.Features.Quizzes.Services;

namespace Ressource_API.Features.Quizzes;

[ApiController]
[Route("api/[controller]")]
public class QuizzController : ControllerBase
{
    private readonly IQuizzeservice _service;
    private readonly ILogger<QuizzController> _logger;

    public QuizzController(IQuizzeservice service, ILogger<QuizzController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all Quizzes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Quizz>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Quizz>>> GetAllQuizzes(CancellationToken cancellationToken)
    {
        try
        {
            var Quizzes = await _service.GetAllQuizzesAsync(cancellationToken);
            return Ok(Quizzes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Quizzes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Quizzes");
        }
    }

    /// <summary>
    /// Get a quizz by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Quizz), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Quizz>> GetQuizzById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var quizz = await _service.GetQuizzByIdAsync(id, cancellationToken);

            if (quizz == null)
            {
                return NotFound($"Quizz with ID {id} not found");
            }

            return Ok(quizz);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving quizz with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the quizz");
        }
    }

    /// <summary>
    /// Create a new quizz
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Quizz), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Quizz>> CreateQuizz([FromBody] CreateQuizzDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdQuizz = await _service.CreateQuizzAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetQuizzById),
                new { id = createdQuizz.Id },
                createdQuizz
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating quizz");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the quizz");
        }
    }

    /// <summary>
    /// Update an existing quizz
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Quizz), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Quizz>> UpdateQuizz(int id, [FromBody] UpdateQuizzDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedQuizz = await _service.UpdateQuizzAsync(id, dto, cancellationToken);

            if (updatedQuizz == null)
            {
                return NotFound($"Quizz with ID {id} not found");
            }

            return Ok(updatedQuizz);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating quizz with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the quizz");
        }
    }

    /// <summary>
    /// Delete a quizz
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuizz(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteQuizzAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Quizz with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting quizz with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the quizz");
        }
    }
}
