using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Features.RessourceProgressions.Services;

namespace Ressource_API.Features.RessourceProgressions;

[ApiController]
[Route("api/ressource-progressions")]
public class RessourceProgressionController : ControllerBase
{
    private readonly IRessourceProgressionService _service;
    private readonly ILogger<RessourceProgressionController> _logger;

    public RessourceProgressionController(IRessourceProgressionService service,
        ILogger<RessourceProgressionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourceprogressions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RessourceProgression>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllRessourceProgressions(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllRessourceProgressionsAsync(cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a ressourceprogression by IDs
    /// </summary>
    [HttpGet("ressources/{ressourceId}/users/{userId}")]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetRessourceProgressionById([FromRoute] Guid ressourceId, [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new ressourceprogression
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRessourceProgression(
        [FromBody] CreateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.CreateRessourceProgressionAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetRessourceProgressionById),
                new { ressourceId = data.RessourceId, userId = data.UserId },
                data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Update an existing ressourceprogression
    /// </summary>
    [HttpPut("ressources/{ressourceId}/users/{userId}")] // Changé en HttpPut
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateRessourceProgression([FromRoute] Guid ressourceId, [FromRoute] Guid userId,
        [FromBody] UpdateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateRessourceProgressionAsync(ressourceId, userId, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete a ressourceprogression
    /// </summary>
    [HttpDelete("ressources/{ressourceId}/users/{userId}")] // Changé en HttpDelete
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceProgression([FromRoute] Guid ressourceId, [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRessourceProgressionAsync(ressourceId, userId, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return NoContent();
    }
}