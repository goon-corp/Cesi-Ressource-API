using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;
using Ressource_API.Features.RefreshTokens.Services;

namespace Ressource_API.Features.RefreshTokens;

[ApiController]
[Route("api/[controller]")]
public class RefreshTokenController : ControllerBase
{
    private readonly IRefreshTokenService _service;
    private readonly ILogger<RefreshTokenController> _logger;

    public RefreshTokenController(IRefreshTokenService service, ILogger<RefreshTokenController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all refreshtokens
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RefreshToken>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RefreshToken>>> GetAllRefreshTokens(CancellationToken cancellationToken)
    {
        try
        {
            var refreshtokens = await _service.GetAllRefreshTokensAsync(cancellationToken);
            return Ok(refreshtokens);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all refreshtokens");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving refreshtokens");
        }
    }

    /// <summary>
    /// Get a refreshtoken by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RefreshToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RefreshToken>> GetRefreshTokenById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var refreshtoken = await _service.GetRefreshTokenByIdAsync(id, cancellationToken);

            if (refreshtoken == null)
            {
                return NotFound($"RefreshToken with ID {id} not found");
            }

            return Ok(refreshtoken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving refreshtoken with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the refreshtoken");
        }
    }

    /// <summary>
    /// Create a new refreshtoken
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RefreshToken), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RefreshToken>> CreateRefreshToken([FromBody] CreateRefreshTokenDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRefreshToken = await _service.CreateRefreshTokenAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRefreshTokenById),
                new { id = createdRefreshToken.Id },
                createdRefreshToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating refreshtoken");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the refreshtoken");
        }
    }

    /// <summary>
    /// Update an existing refreshtoken
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RefreshToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RefreshToken>> UpdateRefreshToken(int id, [FromBody] UpdateRefreshTokenDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRefreshToken = await _service.UpdateRefreshTokenAsync(id, dto, cancellationToken);

            if (updatedRefreshToken == null)
            {
                return NotFound($"RefreshToken with ID {id} not found");
            }

            return Ok(updatedRefreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating refreshtoken with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the refreshtoken");
        }
    }

    /// <summary>
    /// Delete a refreshtoken
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRefreshToken(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRefreshTokenAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RefreshToken with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting refreshtoken with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the refreshtoken");
        }
    }
}
