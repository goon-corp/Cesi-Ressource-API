using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Ressources;

[ApiController]
[Route("api/ressources")]
public class RessourceController : ControllerBase
{
    private readonly IRessourceService _service;
    private readonly ILogger<RessourceController> _logger;

    public RessourceController(IRessourceService service, ILogger<RessourceController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressources
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<ReturnRessourceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ReturnRessourceDto>>> GetAllRessources(
        [FromQuery] RessourceQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var ressources = await _service.GetAllRessourcesAsync(query, cancellationToken);
            return Ok(ressources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressources");
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while retrieving ressources");
        }
    }

    /// <summary>
    /// Create a new ressource
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Ressource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ressource>> CreateRessource([FromForm] CreateRessourceDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessource = await _service.CreateRessourceAsync(dto, User, cancellationToken);

            return CreatedAtAction(
                nameof(CreateRessource),
                new { id = createdRessource.Id },
                createdRessource
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressource");
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while creating the ressource");
        }
    }

    [HttpPost("{id:guid}/like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LikeRessource([FromRoute] Guid id)
    {
        var result = await _service.LikeRessource(id, User);

        return result.Match<IActionResult>(
            onSuccess: NoContent,
            onFailure: NotFound);
    }

    [HttpPost("{id:guid}/favorite")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FavoriteRessource([FromRoute] Guid id)
    {
        var result = await _service.FavoriteRessource(id, User);

        return result.Match<IActionResult>(
            onSuccess: NoContent,
            onFailure: NotFound);
    }
}