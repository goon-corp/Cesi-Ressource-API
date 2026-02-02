using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;
using Ressource_API.Features.UserRoles.Services;

namespace Ressource_API.Features.UserRoles;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly IUserRoleService _service;
    private readonly ILogger<UserRoleController> _logger;

    public UserRoleController(IUserRoleService service, ILogger<UserRoleController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all userroles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserRole>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserRole>>> GetAllUserRoles(CancellationToken cancellationToken)
    {
        try
        {
            var userroles = await _service.GetAllUserRolesAsync(cancellationToken);
            return Ok(userroles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all userroles");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving userroles");
        }
    }

    /// <summary>
    /// Get a userrole by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserRole), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserRole>> GetUserRoleById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var userrole = await _service.GetUserRoleByIdAsync(id, cancellationToken);

            if (userrole == null)
            {
                return NotFound($"UserRole with ID {id} not found");
            }

            return Ok(userrole);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving userrole with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the userrole");
        }
    }

    /// <summary>
    /// Create a new userrole
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserRole), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserRole>> CreateUserRole([FromBody] CreateUserRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUserRole = await _service.CreateUserRoleAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetUserRoleById),
                new { id = createdUserRole.Id },
                createdUserRole
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating userrole");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the userrole");
        }
    }

    /// <summary>
    /// Update an existing userrole
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserRole), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserRole>> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUserRole = await _service.UpdateUserRoleAsync(id, dto, cancellationToken);

            if (updatedUserRole == null)
            {
                return NotFound($"UserRole with ID {id} not found");
            }

            return Ok(updatedUserRole);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating userrole with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the userrole");
        }
    }

    /// <summary>
    /// Delete a userrole
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserRole(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteUserRoleAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"UserRole with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting userrole with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the userrole");
        }
    }
}
