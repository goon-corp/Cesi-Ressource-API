using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.Notifications.NotificationDtos;
using Ressource_API.Features.Notifications.Services;

namespace Ressource_API.Features.Notifications;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(INotificationService service, ILogger<NotificationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all notifications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotifications(CancellationToken cancellationToken)
    {
        try
        {
            var notifications = await _service.GetAllNotificationsAsync(cancellationToken);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all notifications");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving notifications");
        }
    }

    /// <summary>
    /// Get a notification by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Notification>> GetNotificationById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var notification = await _service.GetNotificationByIdAsync(id, cancellationToken);

            if (notification == null)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return Ok(notification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notification with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the notification");
        }
    }

    /// <summary>
    /// Create a new notification
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Notification>> CreateNotification([FromBody] CreateNotificationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdNotification = await _service.CreateNotificationAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetNotificationById),
                new { id = createdNotification.Id },
                createdNotification
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the notification");
        }
    }

    /// <summary>
    /// Update an existing notification
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Notification>> UpdateNotification(int id, [FromBody] UpdateNotificationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedNotification = await _service.UpdateNotificationAsync(id, dto, cancellationToken);

            if (updatedNotification == null)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return Ok(updatedNotification);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the notification");
        }
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNotification(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteNotificationAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the notification");
        }
    }
}
