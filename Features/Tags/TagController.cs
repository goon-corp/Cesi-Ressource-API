using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.Query;
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
    [ProducesResponseType(typeof(PaginatedList<Tag>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<Tag>>> GetAllTags(
        [FromQuery] TagQuery tagQuery,
        CancellationToken cancellationToken)
    {
        var tags = await _service.GetAllTagsAsync(tagQuery, cancellationToken);
        return Ok(tags);
    }

    /// <summary>
    /// Get a tag by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Tag>> GetTagById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var tag = await _service.GetTagByIdAsync(id, cancellationToken);
            return Ok(tag);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
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
        var createdTag = await _service.CreateTagAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(GetTagById),
            new { id = createdTag.Id },
            createdTag
        );
    }

    /// <summary>
    /// Update an existing tag
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Tag>> UpdateTag(Guid id, [FromBody] UpdateTagDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updatedTag = await _service.UpdateTagAsync(id, dto, cancellationToken);
            return Ok(updatedTag);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Delete a tag
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTag(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteTagAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}