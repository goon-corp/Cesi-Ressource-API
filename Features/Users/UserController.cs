using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Logins.Services;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Users.Dtos;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.Query;
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

    // /// <summary>
    // /// Get all users
    // /// </summary>
    // [HttpGet]
    // [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    // public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         var users = await _service.GetAllUsersAsync(cancellationToken);
    //         return Ok(users);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error retrieving all users");
    //         return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users");
    //     }
    // }

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
    /// Return the list of aside ressources (watchlist) for a user
    /// </summary>
    [HttpGet("{id}/aside-ressources")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<ReturnRessourceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserAsideRessources(Guid id, [FromQuery] PagedQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetUserAsideRessourcesById(id, query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving aside ressources for user {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user aside ressources");
        }
    }

    /// <summary>
    /// Return the list of exploited (consulted) ressources for a user
    /// </summary>
    [HttpGet("{id}/exploited-ressources")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<ReturnRessourceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserExploitedRessources(Guid id, [FromQuery] PagedQueryParameters query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.GetUserExploitedRessourcesById(id, query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving exploited ressources for user {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the user exploited ressources");
        }
    }

    /// <summary>
    /// Get all users (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<UserInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<UserInfoDto>>> GetPaginatedUsers(
        [FromQuery] UserQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedUsersAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get the current authenticated user
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserInfoDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var result = await _service.GetCurrentUserAsync(User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => Unauthorized(error));
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    [HttpGet("get-user-by-id/{id:guid}")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserInfoDto>> GetUserByIdAdmin(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetUserByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserInfoDto>> CreateUser(
        [FromBody] CreateUserDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateUserAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetUserByIdAdmin),
                new { id = data.Id },
                data),
            onFailure: error => Conflict(error));
    }

    /// <summary>
    /// Update a user
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserInfoDto>> UpdateUser(
        Guid id,
        [FromBody] UpdateUserDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateUserAsync(id, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => error.Contains("not found")
                ? NotFound(error)
                : Conflict(error));
    }

    /// <summary>
    /// Soft delete a user
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteUserAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}
