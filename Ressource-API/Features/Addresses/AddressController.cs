using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Addresses.AddressDtos;
using Ressource_API.Features.Addresses.Services;

namespace Ressource_API.Features.Addresses;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddresseservice _service;
    private readonly ILogger<AddressController> _logger;

    public AddressController(IAddresseservice service, ILogger<AddressController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all Addresses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Address>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses(CancellationToken cancellationToken)
    {
        try
        {
            var Addresses = await _service.GetAllAddressesAsync(cancellationToken);
            return Ok(Addresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Addresses");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving Addresses");
        }
    }

    /// <summary>
    /// Get a address by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> GetAddressById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var address = await _service.GetAddressByIdAsync(id, cancellationToken);

            if (address == null)
            {
                return NotFound($"Address with ID {id} not found");
            }

            return Ok(address);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving address with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the address");
        }
    }

    /// <summary>
    /// Create a new address
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Address), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Address>> CreateAddress([FromBody] CreateAddressDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAddress = await _service.CreateAddressAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetAddressById),
                new { id = createdAddress.Id },
                createdAddress
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating address");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the address");
        }
    }

    /// <summary>
    /// Update an existing address
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Address>> UpdateAddress(int id, [FromBody] UpdateAddressDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedAddress = await _service.UpdateAddressAsync(id, dto, cancellationToken);

            if (updatedAddress == null)
            {
                return NotFound($"Address with ID {id} not found");
            }

            return Ok(updatedAddress);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the address");
        }
    }

    /// <summary>
    /// Delete a address
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAddress(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteAddressAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Address with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting address with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the address");
        }
    }
}
