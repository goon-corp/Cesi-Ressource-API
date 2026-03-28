using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.RessourceConfidentialityTypes.Services;

namespace Ressource_API.Features.RessourceConfidentialityTypes;

[ApiController]
[Route("api/ressource-confidentiality-types")]
public class RessourceConfidentialityTypeController : ControllerBase
{
    private readonly IRessourceConfidentialityTypeService _service;

    public RessourceConfidentialityTypeController(IRessourceConfidentialityTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceConfidentialityTypeInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: types => Ok(types),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, new { error })
        );
    }
}
