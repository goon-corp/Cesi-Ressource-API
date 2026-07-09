using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;
using Ressource_API.Features.PasswordInfos.Services;

namespace Ressource_API.Features.PasswordInfos;

[ApiController]
[Route("api/[controller]")]
public class PasswordInfoController : ControllerBase
{
    private readonly IPasswordInfoService _service;
    private readonly ILogger<PasswordInfoController> _logger;

    public PasswordInfoController(IPasswordInfoService service, ILogger<PasswordInfoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all passwordinfos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PasswordInfo>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasswordInfo>>> GetAllPasswordInfos(CancellationToken cancellationToken)
    {
        try
        {
            var passwordinfos = await _service.GetAllPasswordInfosAsync(cancellationToken);
            return Ok(passwordinfos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all passwordinfos");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving passwordinfos");
        }
    }

    /// <summary>
    /// Get a passwordinfo by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PasswordInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PasswordInfo>> GetPasswordInfoById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var passwordinfo = await _service.GetPasswordInfoByIdAsync(id, cancellationToken);

            if (passwordinfo == null)
            {
                return NotFound($"PasswordInfo with ID {id} not found");
            }

            return Ok(passwordinfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving passwordinfo with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the passwordinfo");
        }
    }

    /// <summary>
    /// Create a new passwordinfo
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PasswordInfo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordInfo>> CreatePasswordInfo([FromBody] CreatePasswordInfoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPasswordInfo = await _service.CreatePasswordInfoAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetPasswordInfoById),
                new { id = createdPasswordInfo.Id },
                createdPasswordInfo
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating passwordinfo");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the passwordinfo");
        }
    }

    /// <summary>
    /// Update an existing passwordinfo
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PasswordInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordInfo>> UpdatePasswordInfo(int id, [FromBody] UpdatePasswordInfoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPasswordInfo = await _service.UpdatePasswordInfoAsync(id, dto, cancellationToken);

            if (updatedPasswordInfo == null)
            {
                return NotFound($"PasswordInfo with ID {id} not found");
            }

            return Ok(updatedPasswordInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating passwordinfo with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the passwordinfo");
        }
    }

    /// <summary>
    /// Delete a passwordinfo
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePasswordInfo(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeletePasswordInfoAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"PasswordInfo with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting passwordinfo with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the passwordinfo");
        }
    }
}
