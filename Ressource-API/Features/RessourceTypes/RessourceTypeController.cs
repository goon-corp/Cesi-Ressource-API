using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.Services;

namespace Ressource_API.Features.RessourceTypes;

[ApiController]
[Route("api/ressource-types")]
public class RessourceTypeController : ControllerBase
{
    private readonly IRessourceTypeService _service;
    private readonly ILogger<RessourceTypeController> _logger;

    public RessourceTypeController(IRessourceTypeService service, ILogger<RessourceTypeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressource types
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceType>>> GetAllRessourceTypes(CancellationToken cancellationToken)
    {
        try
        {
            var ressourcetypes = await _service.GetAllRessourceTypesAsync(cancellationToken);
            return Ok(ressourcetypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourcetypes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourcetypes");
        }
    }
}
