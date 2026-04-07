using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.Services;

namespace Ressource_API.Features.RessourceMedias;

[ApiController]
[Route("api/ressource-medias")]
public class RessourceMediaController : ControllerBase
{
    private readonly IRessourceMediaService _service;
    private readonly ILogger<RessourceMediaController> _logger;

    public RessourceMediaController(IRessourceMediaService service, ILogger<RessourceMediaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateRessourceMediaDto dto)
    {
        var media = await _service.CreateMedia(dto);
        return Ok(media);
    }

    [Authorize]
    [HttpDelete("{mediaId:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid mediaId)
    {
        await _service.DeleteMedia(mediaId);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{mediaId:Guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid mediaId)
    {
        var (stream, mimeType) = await _service.GetMediaStream(mediaId);
        Response.Headers.CacheControl = "public, max-age=31536000, immutable";
        return File(stream, mimeType);
    }
}