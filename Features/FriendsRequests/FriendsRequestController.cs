using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;
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
    /// Get all friendsrequests (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<FriendsRequestInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<FriendsRequestInfoDto>>> GetPaginatedFriendsRequests(
        [FromQuery] FriendsRequestQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetPaginatedFriendsRequestsAsync(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated friendsrequests");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving friendsrequests");
        }
    }

    /// <summary>
    /// Get a friendsrequest by sender and receiver IDs
    /// </summary>
    [HttpGet("{userSenderId:guid}/{userReceiverId:guid}")]
    [ProducesResponseType(typeof(FriendsRequestInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FriendsRequestInfoDto>> GetFriendsRequestByIds(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken)
    {
        try
        {
            var friendsRequest = await _service.GetFriendsRequestByIdsAsync(userSenderId, userReceiverId, cancellationToken);

            if (friendsRequest == null)
                return NotFound($"FriendsRequest between {userSenderId} and {userReceiverId} not found");

            return Ok(friendsRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving friendsrequest between {UserSenderId} and {UserReceiverId}", userSenderId, userReceiverId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the friendsrequest");
        }
    }

    /// <summary>
    /// Create a new friendsrequest
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FriendsRequestInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FriendsRequestInfoDto>> CreateFriendsRequest(
        [FromBody] CreateFriendsRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TODO: Remplacer par l'extraction depuis le token JWT (ex: User.GetUserId())
            var userSenderId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User not authenticated"));

            var created = await _service.CreateFriendsRequestAsync(dto, userSenderId, cancellationToken);

            return CreatedAtAction(
                nameof(GetFriendsRequestByIds),
                new { userSenderId = created.UserSenderId, userReceiverId = created.UserReceiverId },
                created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating friendsrequest");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the friendsrequest");
        }
    }

    /// <summary>
    /// Update the status of an existing friendsrequest
    /// </summary>
    [HttpPut("{userSenderId:guid}/{userReceiverId:guid}")]
    [ProducesResponseType(typeof(FriendsRequestInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FriendsRequestInfoDto>> UpdateFriendsRequest(
        Guid userSenderId,
        Guid userReceiverId,
        [FromBody] UpdateFriendsRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateFriendsRequestAsync(userSenderId, userReceiverId, dto, cancellationToken);

            if (updated == null)
                return NotFound($"FriendsRequest between {userSenderId} and {userReceiverId} not found");

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating friendsrequest between {UserSenderId} and {UserReceiverId}", userSenderId, userReceiverId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the friendsrequest");
        }
    }

    /// <summary>
    /// Soft delete a friendsrequest
    /// </summary>
    [HttpDelete("{userSenderId:guid}/{userReceiverId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFriendsRequest(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteFriendsRequestAsync(userSenderId, userReceiverId, cancellationToken);

            if (!deleted)
                return NotFound($"FriendsRequest between {userSenderId} and {userReceiverId} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting friendsrequest between {UserSenderId} and {UserReceiverId}", userSenderId, userReceiverId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the friendsrequest");
        }
    }
}