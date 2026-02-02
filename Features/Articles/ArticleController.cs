using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.ArticleDtos;
using Ressource_API.Features.Articles.Services;

namespace Ressource_API.Features.Articles;

[ApiController]
[Route("api/[controller]")]
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
    /// Get all articles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Article>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Article>>> GetAllArticles(CancellationToken cancellationToken)
    {
        try
        {
            var articles = await _service.GetAllArticlesAsync(cancellationToken);
            return Ok(articles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all articles");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving articles");
        }
    }

    /// <summary>
    /// Get a article by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Article>> GetArticleById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var article = await _service.GetArticleByIdAsync(id, cancellationToken);

            if (article == null)
            {
                return NotFound($"Article with ID {id} not found");
            }

            return Ok(article);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving article with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the article");
        }
    }

    /// <summary>
    /// Create a new article
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Article), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Article>> CreateArticle([FromBody] CreateArticleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdArticle = await _service.CreateArticleAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetArticleById),
                new { id = createdArticle.Id },
                createdArticle
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating article");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the article");
        }
    }

    /// <summary>
    /// Update an existing article
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Article), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Article>> UpdateArticle(int id, [FromBody] UpdateArticleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedArticle = await _service.UpdateArticleAsync(id, dto, cancellationToken);

            if (updatedArticle == null)
            {
                return NotFound($"Article with ID {id} not found");
            }

            return Ok(updatedArticle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating article with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the article");
        }
    }

    /// <summary>
    /// Delete a article
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteArticle(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteArticleAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Article with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting article with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the article");
        }
    }
}
