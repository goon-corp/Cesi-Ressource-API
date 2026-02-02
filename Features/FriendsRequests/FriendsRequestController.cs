using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Services;

namespace Ressource_API.Features.FriendsRequests;

[ApiController]
[Route("api/[controller]")]
public class FriendsRequestController : ControllerBase
{
    private readonly IFriendsRequestService _service;
    private readonly ILogger<FriendsRequestController> _logger;

    public FriendsRequestController(IFriendsRequestService service, ILogger<FriendsRequestController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all friendsrequests
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FriendsRequest>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FriendsRequest>>> GetAllFriendsRequests(CancellationToken cancellationToken)
    {
        try
        {
            var friendsrequests = await _service.GetAllFriendsRequestsAsync(cancellationToken);
            return Ok(friendsrequests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all friendsrequests");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving friendsrequests");
        }
    }

    /// <summary>
    /// Get a friendsrequest by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FriendsRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FriendsRequest>> GetFriendsRequestById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var friendsrequest = await _service.GetFriendsRequestByIdAsync(id, cancellationToken);

            if (friendsrequest == null)
            {
                return NotFound($"FriendsRequest with ID {id} not found");
            }

            return Ok(friendsrequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving friendsrequest with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the friendsrequest");
        }
    }

    /// <summary>
    /// Create a new friendsrequest
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FriendsRequest), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FriendsRequest>> CreateFriendsRequest([FromBody] CreateFriendsRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFriendsRequest = await _service.CreateFriendsRequestAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetFriendsRequestById),
                new { id = createdFriendsRequest.UserSenderId },
                createdFriendsRequest
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating friendsrequest");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the friendsrequest");
        }
    }

    /// <summary>
    /// Update an existing friendsrequest
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FriendsRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FriendsRequest>> UpdateFriendsRequest(int id, [FromBody] UpdateFriendsRequestDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedFriendsRequest = await _service.UpdateFriendsRequestAsync(id, dto, cancellationToken);

            if (updatedFriendsRequest == null)
            {
                return NotFound($"FriendsRequest with ID {id} not found");
            }

            return Ok(updatedFriendsRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating friendsrequest with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the friendsrequest");
        }
    }

    /// <summary>
    /// Delete a friendsrequest
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFriendsRequest(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteFriendsRequestAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"FriendsRequest with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting friendsrequest with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the friendsrequest");
        }
    }
}
