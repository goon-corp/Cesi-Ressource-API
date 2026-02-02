using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Features.Tags.Services;

namespace Ressource_API.Features.Tags;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _service;
    private readonly ILogger<TagController> _logger;

    public TagController(ITagService service, ILogger<TagController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all tags
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Tag>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Tag>>> GetAllTags(CancellationToken cancellationToken)
    {
        try
        {
            var tags = await _service.GetAllTagsAsync(cancellationToken);
            return Ok(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tags");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving tags");
        }
    }

    /// <summary>
    /// Get a tag by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tag>> GetTagById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var tag = await _service.GetTagByIdAsync(id, cancellationToken);

            if (tag == null)
            {
                return NotFound($"Tag with ID {id} not found");
            }

            return Ok(tag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tag with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the tag");
        }
    }

    /// <summary>
    /// Create a new tag
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Tag), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Tag>> CreateTag([FromBody] CreateTagDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTag = await _service.CreateTagAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetTagById),
                new { id = createdTag.Id },
                createdTag
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tag");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the tag");
        }
    }

    /// <summary>
    /// Update an existing tag
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Tag>> UpdateTag(int id, [FromBody] UpdateTagDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTag = await _service.UpdateTagAsync(id, dto, cancellationToken);

            if (updatedTag == null)
            {
                return NotFound($"Tag with ID {id} not found");
            }

            return Ok(updatedTag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tag with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the tag");
        }
    }

    /// <summary>
    /// Delete a tag
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTag(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteTagAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Tag with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tag with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the tag");
        }
    }
}
