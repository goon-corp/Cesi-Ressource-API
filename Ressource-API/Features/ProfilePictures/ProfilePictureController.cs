using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.ProfilePictures.ProfilePictureDtos;
using Ressource_API.Features.ProfilePictures.Services;

namespace Ressource_API.Features.ProfilePictures;

[ApiController]
[Route("api/[controller]")]
public class ProfilePictureController : ControllerBase
{
    private readonly IProfilePictureService _service;
    private readonly ILogger<ProfilePictureController> _logger;

    public ProfilePictureController(IProfilePictureService service, ILogger<ProfilePictureController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all profilepictures
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProfilePicture>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProfilePicture>>> GetAllProfilePictures(CancellationToken cancellationToken)
    {
        try
        {
            var profilepictures = await _service.GetAllProfilePicturesAsync(cancellationToken);
            return Ok(profilepictures);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all profilepictures");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving profilepictures");
        }
    }

    /// <summary>
    /// Get a profilepicture by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProfilePicture), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfilePicture>> GetProfilePictureById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var profilepicture = await _service.GetProfilePictureByIdAsync(id, cancellationToken);

            if (profilepicture == null)
            {
                return NotFound($"ProfilePicture with ID {id} not found");
            }

            return Ok(profilepicture);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving profilepicture with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the profilepicture");
        }
    }

    /// <summary>
    /// Create a new profilepicture
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProfilePicture), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfilePicture>> CreateProfilePicture([FromBody] CreateProfilePictureDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProfilePicture = await _service.CreateProfilePictureAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetProfilePictureById),
                new { id = createdProfilePicture.Id },
                createdProfilePicture
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating profilepicture");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the profilepicture");
        }
    }

    /// <summary>
    /// Update an existing profilepicture
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProfilePicture), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfilePicture>> UpdateProfilePicture(int id, [FromBody] UpdateProfilePictureDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProfilePicture = await _service.UpdateProfilePictureAsync(id, dto, cancellationToken);

            if (updatedProfilePicture == null)
            {
                return NotFound($"ProfilePicture with ID {id} not found");
            }

            return Ok(updatedProfilePicture);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profilepicture with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the profilepicture");
        }
    }

    /// <summary>
    /// Delete a profilepicture
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProfilePicture(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteProfilePictureAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"ProfilePicture with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profilepicture with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the profilepicture");
        }
    }
}
