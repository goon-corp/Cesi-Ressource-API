using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Logins.Services;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Features.Users.Services;

namespace Ressource_API.Features.Users;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILoginService _loginService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService service, ILoginService loginService, ILogger<UserController> logger)
    {
        _service = service;
        _loginService = loginService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(CancellationToken cancellationToken)
    {
        try
        {
            var users = await _service.GetAllUsersAsync(cancellationToken);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users");
        }
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _service.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user");
        }
    }
    
    /// <summary>
    /// Get a user profile by UseId
    /// </summary>
    [HttpGet("{id}/profile")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileTest(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _service.GetUserProfileById(id, cancellationToken);

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user");
        }
    }
    
    /// <summary>
    /// Return the list of liked ressources for a user
    /// </summary>
    [HttpGet("{id}/liked-ressources")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ReturnRessourceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserLikedRessources(Guid id, [FromQuery] PagedQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var likedRessources = await _service.GetUserLikedRessourcesById(id, query, cancellationToken);

            return Ok(likedRessources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user liked ressources");
        }
    }
    
    
    /// <summary>
    /// Return the list of favorites ressources for a user
    /// </summary>
    [HttpGet("{id}/fav-ressources")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ReturnRessourceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserFavoritesRessources(Guid id, [FromQuery] PagedQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var favRessources = await _service.GetUserFavoriteRessourcesById(id, query, cancellationToken);

            return Ok(favRessources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user fav ressources");
        }
    }
    
        
    /// <summary>
    /// Return the list of authored ressources for a user
    /// </summary>
    [HttpGet("{id}/authored-ressources")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<ReturnRessourceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserAuthoredRessources(Guid id, [FromQuery] PagedQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var authoredRessources = await _service.GetUserAuthoredRessourcesById(id, query, cancellationToken);

            return Ok(authoredRessources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user fav ressources");
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> UpdateUser(Guid id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _service.UpdateUserAsync(id, dto, cancellationToken);

            if (!updatedUser.IsSuccess)
            {
                return NotFound($"User not updated");
            }

            return Ok(updatedUser.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the user");
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrateur")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteUserAsync(id, cancellationToken);

            if (!deleted.IsSuccess)
            {
                return NotFound($"User with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the user");
        }
    }
}
