using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;
using Ressource_API.Features.Logins.Services;

namespace Ressource_API.Features.Logins;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _service;
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILoginService service, ILogger<LoginController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all logins
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Login>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Login>>> GetAllLogins(CancellationToken cancellationToken)
    {
        try
        {
            var logins = await _service.GetAllLoginsAsync(cancellationToken);
            return Ok(logins);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all logins");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving logins");
        }
    }

    /// <summary>
    /// Get a login by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Login), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Login>> GetLoginById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var login = await _service.GetLoginByIdAsync(id, cancellationToken);

            if (login == null)
            {
                return NotFound($"Login with ID {id} not found");
            }

            return Ok(login);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving login with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the login");
        }
    }

    /// <summary>
    /// Create a new login
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Login), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Login>> CreateLogin([FromBody] CreateLoginDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdLogin = await _service.CreateLoginAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetLoginById),
                new { id = createdLogin.Id },
                createdLogin
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating login");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the login");
        }
    }

    /// <summary>
    /// Update an existing login
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Login), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Login>> UpdateLogin(int id, [FromBody] UpdateLoginDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedLogin = await _service.UpdateLoginAsync(id, dto, cancellationToken);

            if (updatedLogin == null)
            {
                return NotFound($"Login with ID {id} not found");
            }

            return Ok(updatedLogin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating login with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the login");
        }
    }

    /// <summary>
    /// Delete a login
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLogin(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteLoginAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Login with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting login with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the login");
        }
    }
}
