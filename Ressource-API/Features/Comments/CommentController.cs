using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Features.Comments.Services;

namespace Ressource_API.Features.Comments;

[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _service;
    private readonly ILogger<CommentController> _logger;

    public CommentController(ICommentService service, ILogger<CommentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all comments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CommentInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllComments(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllCommentsAsync(cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get paginated comments for a resource
    /// </summary>
    [HttpGet("ressource/{ressourceId:guid}")]
    [ProducesResponseType(typeof(PagedResult<CommentInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCommentsByRessource(Guid ressourceId, [FromQuery] PagedQueryParameters paging, CancellationToken cancellationToken)
    {
        var result = await _service.GetCommentsByRessourceAsync(ressourceId, paging, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a comment by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CommentInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetCommentById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetCommentByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CommentInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateComment([FromBody] CreateCommentDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateCommentAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(nameof(GetCommentById), new { id = data.Id }, data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Update an existing comment
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CommentInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateComment(Guid id, [FromBody] UpdateCommentDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateCommentAsync(id, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComment(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteCommentAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}
