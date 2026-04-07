using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Cities.CityDtos;
using Ressource_API.Features.Cities.Services;

namespace Ressource_API.Features.Cities;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICitieservice _service;
    private readonly ILogger<CityController> _logger;

    public CityController(ICitieservice service, ILogger<CityController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all Cities
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<City>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<City>>> GetAllCities(CancellationToken cancellationToken)
    {
        try
        {
            var Cities = await _service.GetAllCitiesAsync(cancellationToken);
            return Ok(Cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Cities");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Cities");
        }
    }

    /// <summary>
    /// Get a city by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(City), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<City>> GetCityById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var city = await _service.GetCityByIdAsync(id, cancellationToken);

            if (city == null)
            {
                return NotFound($"City with ID {id} not found");
            }

            return Ok(city);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving city with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the city");
        }
    }

    /// <summary>
    /// Create a new city
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(City), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<City>> CreateCity([FromBody] CreateCityDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCity = await _service.CreateCityAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetCityById),
                new { id = createdCity.Id },
                createdCity
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating city");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the city");
        }
    }

    /// <summary>
    /// Update an existing city
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(City), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<City>> UpdateCity(int id, [FromBody] UpdateCityDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCity = await _service.UpdateCityAsync(id, dto, cancellationToken);

            if (updatedCity == null)
            {
                return NotFound($"City with ID {id} not found");
            }

            return Ok(updatedCity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating city with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the city");
        }
    }

    /// <summary>
    /// Delete a city
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCity(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteCityAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"City with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting city with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the city");
        }
    }
}
