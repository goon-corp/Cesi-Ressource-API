using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;
using Ressource_API.Features.FriendsRequests.Services;

namespace Ressource_API.Features.FriendsRequests;

[ApiController]
[Route("api/friends-requests")]
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
        var result = await _service.GetPaginatedFriendsRequestsAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error =>
            {
                _logger.LogError("Error retrieving paginated friendsrequests: {Error}", error);
                return StatusCode(StatusCodes.Status500InternalServerError, error);
            });
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
        var result = await _service.GetFriendsRequestByIdsAsync(userSenderId, userReceiverId, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new friendsrequest
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FriendsRequestInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FriendsRequestInfoDto>> CreateFriendsRequest(
        [FromBody] CreateFriendsRequestDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateFriendsRequestAsync(dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetFriendsRequestByIds),
                new { userSenderId = data.UserSenderId, userReceiverId = data.UserReceiverId },
                data),
            onFailure: error => Conflict(error));
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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateFriendsRequestAsync(userSenderId, userReceiverId, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
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
        var result = await _service.DeleteFriendsRequestAsync(userSenderId, userReceiverId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}