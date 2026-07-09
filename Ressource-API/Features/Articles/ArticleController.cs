using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.Dtos;
using Ressource_API.Features.Articles.Services;

namespace Ressource_API.Features.Articles;

[ApiController]
[Authorize]
[Route("api/articles")]
public class ArticleController : ControllerBase
{
    private readonly IArticleService _service;
    private readonly ILogger<ArticleController> _logger;

    public ArticleController(IArticleService service, ILogger<ArticleController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get a article by ressource ID
    /// </summary>
    [HttpGet("{ressourceId:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Article>> GetArticleById(Guid ressourceId, CancellationToken cancellationToken)
    {
        var result = await _service.GetArticleByRessourceIdAsync(ressourceId, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Create a new article
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReturnArticleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnArticleDto>> CreateArticle([FromForm] CreateArticleDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateArticleAsync(dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Created("article", data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Update an existing article
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Article>> UpdateArticle(Guid id, [FromBody] UpdateArticleDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateArticleAsync(id, dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Delete a article
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteArticleAsync(id, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}