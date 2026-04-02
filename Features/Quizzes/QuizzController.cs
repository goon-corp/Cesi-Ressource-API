using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Quizzes.Dtos;
using Ressource_API.Features.Quizzes.Query;
using Ressource_API.Features.Quizzes.Services;

namespace Ressource_API.Features.Quizzes;

[ApiController]
[Authorize]
[Route("api/quizzes")]
public class QuizzController : ControllerBase
{
    private readonly IQuizzService _service;
    private readonly ILogger<QuizzController> _logger;

    public QuizzController(IQuizzService service, ILogger<QuizzController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all quizzes (paginated)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<QuizzInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QuizzInfoDto>>> GetPaginatedQuizzes(
        [FromQuery] QuizzQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedQuizzesAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a quizz by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(QuizzInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuizzInfoDto>> GetQuizzById(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetQuizzByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new quizz
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(QuizzInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzInfoDto>> CreateQuizz(
        [FromForm] CreateQuizzDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateQuizzAsync(dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetQuizzById),
                new { id = data.Id },
                data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Incremente participation for a quizz
    /// </summary>
    [HttpPut("{id:guid}/participate")]
    [ProducesResponseType(typeof(QuizzInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzInfoDto>> UpdateQuizz(
        [FromRoute]Guid id,
        // [FromBody] UpdateQuizzDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateQuizzAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete a quizz
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuizz(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteQuizzAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}