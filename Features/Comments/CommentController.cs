using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Features.Comments.Services;

namespace Ressource_API.Features.Comments;

[ApiController]
[Route("api/[controller]")]
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
    [ProducesResponseType(typeof(IEnumerable<Comment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Comment>>> GetAllComments(CancellationToken cancellationToken)
    {
        try
        {
            var comments = await _service.GetAllCommentsAsync(cancellationToken);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all comments");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving comments");
        }
    }

    /// <summary>
    /// Get a comment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Comment>> GetCommentById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var comment = await _service.GetCommentByIdAsync(id, cancellationToken);

            if (comment == null)
            {
                return NotFound($"Comment with ID {id} not found");
            }

            return Ok(comment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving comment with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the comment");
        }
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> CreateComment([FromBody] CreateCommentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdComment = await _service.CreateCommentAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetCommentById),
                new { id = createdComment.Id },
                createdComment
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating comment");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the comment");
        }
    }

    /// <summary>
    /// Update an existing comment
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> UpdateComment(int id, [FromBody] UpdateCommentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedComment = await _service.UpdateCommentAsync(id, dto, cancellationToken);

            if (updatedComment == null)
            {
                return NotFound($"Comment with ID {id} not found");
            }

            return Ok(updatedComment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating comment with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the comment");
        }
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComment(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteCommentAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Comment with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting comment with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the comment");
        }
    }
}
