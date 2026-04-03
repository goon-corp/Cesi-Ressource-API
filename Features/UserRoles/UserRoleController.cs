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
}
