using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Departments.Models;
using Ressource_API.Features.Departments.DepartmentDtos;
using Ressource_API.Features.Departments.Services;

namespace Ressource_API.Features.Departments;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Department>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments(CancellationToken cancellationToken)
    {
        try
        {
            var departments = await _service.GetAllDepartmentsAsync(cancellationToken);
            return Ok(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving departments");
        }
    }

    /// <summary>
    /// Get a department by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Department>> GetDepartmentById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _service.GetDepartmentByIdAsync(id, cancellationToken);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found");
            }

            return Ok(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the department");
        }
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Department), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Department>> CreateDepartment([FromBody] CreateDepartmentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdDepartment = await _service.CreateDepartmentAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetDepartmentById),
                new { id = createdDepartment.Id },
                createdDepartment
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the department");
        }
    }

    /// <summary>
    /// Update an existing department
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Department), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Department>> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDepartment = await _service.UpdateDepartmentAsync(id, dto, cancellationToken);

            if (updatedDepartment == null)
            {
                return NotFound($"Department with ID {id} not found");
            }

            return Ok(updatedDepartment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the department");
        }
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDepartment(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteDepartmentAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Department with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the department");
        }
    }
}
