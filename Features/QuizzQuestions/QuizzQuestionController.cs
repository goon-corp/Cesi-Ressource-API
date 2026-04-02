using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.QuizzQuestions.Query;
using Ressource_API.Features.QuizzQuestions.Services;

namespace Ressource_API.Features.QuizzQuestions;

[ApiController]
[Route("api/quizzes-questions")]
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
    /// Get all quizz questions (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<QuizzQuestionInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QuizzQuestionInfoDto>>> GetPaginatedQuizzQuestions(
        [FromQuery] QuizzQuestionQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedQuizzQuestionsAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a quizz question by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(QuizzQuestionInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuizzQuestionInfoDto>> GetQuizzQuestionById(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetQuizzQuestionByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new quizz question
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(QuizzQuestionInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzQuestionInfoDto>> CreateQuizzQuestion(
        [FromBody] CreateQuizzQuestionDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateQuizzQuestionAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetQuizzQuestionById),
                new { id = data.Id },
                data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Update a quizz question
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(QuizzQuestionInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzQuestionInfoDto>> UpdateQuizzQuestion(
        [FromRoute]Guid id,
        [FromBody] UpdateQuizzQuestionDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateQuizzQuestionAsync(id, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }
    
    /// <summary>
    /// Update a quizz count for player
    /// </summary>
    [HttpPut("{id:guid}/participate/{userId:guid}")]
    [ProducesResponseType(typeof(QuizzQuestionInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuizzQuestionInfoDto>> UpdateQuizzQuestionParticipation(
        [FromRoute]Guid id,
        [FromRoute]Guid userId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateQuizzQuestionAsyncPlayer(id, userId, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Soft delete a quizz question
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuizzQuestion(
        [FromRoute]Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteQuizzQuestionAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}