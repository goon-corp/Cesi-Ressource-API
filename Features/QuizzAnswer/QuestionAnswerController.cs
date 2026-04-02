using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.QuestionAnswers.Dtos;
using Ressource_API.Features.QuizzAnswer.Query;
using Ressource_API.Features.QuizzAnswer.Services;

namespace Ressource_API.Features.QuizzAnswer;

[ApiController]
[Route("api/[controller]")]
public class QuestionAnswerController : ControllerBase
{
    private readonly IQuestionAnswerService _service;
    private readonly ILogger<QuestionAnswerController> _logger;

    public QuestionAnswerController(IQuestionAnswerService service, ILogger<QuestionAnswerController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all question answers (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<QuestionAnswerInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<QuestionAnswerInfoDto>>> GetPaginatedQuestionAnswers(
        [FromQuery] QuestionAnswerQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedQuestionAnswersAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a question answer by user and question IDs
    /// </summary>
    [HttpGet("{userId:guid}/{quizzQuestionId:guid}")]
    [ProducesResponseType(typeof(QuestionAnswerInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionAnswerInfoDto>> GetQuestionAnswer(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetQuestionAnswerAsync(userId, quizzQuestionId, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Submit an answer to a question
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(QuestionAnswerInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<QuestionAnswerInfoDto>> CreateQuestionAnswer(
        [FromBody] CreateQuestionAnswerDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateQuestionAnswerAsync(dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetQuestionAnswer),
                new { userId = data.UserId, quizzQuestionId = data.QuizzQuestionId },
                data),
            onFailure: error => Conflict(error));
    }

    /// <summary>
    /// Update an answer
    /// </summary>
    [HttpPut("{userId:guid}/{quizzQuestionId:guid}")]
    [ProducesResponseType(typeof(QuestionAnswerInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<QuestionAnswerInfoDto>> UpdateQuestionAnswer(
        Guid userId,
        Guid quizzQuestionId,
        [FromBody] UpdateQuestionAnswerDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateQuestionAnswerAsync(userId, quizzQuestionId, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete an answer
    /// </summary>
    [HttpDelete("{userId:guid}/{quizzQuestionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuestionAnswer(
        Guid userId,
        Guid quizzQuestionId,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteQuestionAnswerAsync(userId, quizzQuestionId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}