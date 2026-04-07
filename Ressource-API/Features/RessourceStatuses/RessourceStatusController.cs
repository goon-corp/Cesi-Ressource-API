using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceStatuses.Services;

namespace Ressource_API.Features.RessourceStatuses;

[ApiController]
[Route("api/ressource-statuses")]
public class RessourceStatusController : ControllerBase
{
    private readonly IRessourceStatusService _service;

    public RessourceStatusController(IRessourceStatusService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceStatusInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: statuses => Ok(statuses),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, new { error })
        );
    }
}
