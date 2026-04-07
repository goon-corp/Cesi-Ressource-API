#!/bin/bash

# Colors for output
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

# ============================================================================
# VALIDATION FUNCTIONS
# ============================================================================

validate_arguments() {
    if [ $# -eq 0 ]; then
        echo -e "${RED}Error: Feature name is required${NC}"
        echo "Usage: ./createFeature.sh [FeatureName] [optional: path/to/Model.cs]"
        echo "Example: ./createFeature.sh User"
        echo "Example: ./createFeature.sh User ./Models/User.cs"
        exit 1
    fi
}

validate_feature_name() {
    local feature_name=$1
    
    if [[ ! $feature_name =~ ^[A-Z][a-zA-Z0-9]*$ ]]; then
        echo -e "${RED}Error: Feature name must start with an uppercase letter and contain only alphanumeric characters${NC}"
        echo "Example: User, Product, OrderItem"
        exit 1
    fi
}

validate_model_file() {
    local model_file=$1
    
    if [ -n "$model_file" ] && [ ! -f "$model_file" ]; then
        echo -e "${RED}Error: Model file '$model_file' does not exist${NC}"
        exit 1
    fi
    
    if [ -n "$model_file" ] && [[ ! $model_file =~ \.cs$ ]]; then
        echo -e "${RED}Error: Model file must be a .cs file${NC}"
        exit 1
    fi
}

# ============================================================================
# UTILITY FUNCTIONS
# ============================================================================

pluralize() {
    local word=$1
    echo "${word}s"
}

create_directory() {
    local dir_path=$1
    if [ ! -d "$dir_path" ]; then
        mkdir -p "$dir_path"
        echo -e "${GREEN}✓${NC} Created directory: $dir_path"
    fi
}

create_file_with_content() {
    local file_path=$1
    local content=$2
    
    echo "$content" > "$file_path"
    echo -e "${GREEN}✓${NC} Created file: $file_path"
}

move_model_file() {
    local source_file=$1
    local dest_dir=$2
    local feature_name=$3
    
    local dest_file="$dest_dir/${feature_name}.cs"
    
    # Copy the file instead of moving to preserve the original
    cp "$source_file" "$dest_file"
    echo -e "${GREEN}✓${NC} Copied model file to: $dest_file"
}

# ============================================================================
# DIRECTORY STRUCTURE FUNCTIONS
# ============================================================================

create_directory_structure() {
    local feature_name=$1
    local feature_plural=$2
    local base_path=$3
    
    echo -e "${BLUE}Creating directory structure for ${feature_plural}...${NC}"
    
    create_directory "$base_path"
    create_directory "$base_path/Models"
    create_directory "$base_path/${feature_name}Dtos"
    create_directory "$base_path/Services"
    create_directory "$base_path/Factories"
    create_directory "$base_path/Repositories"
}

# ============================================================================
# FILE CONTENT GENERATION FUNCTIONS
# ============================================================================

generate_create_dto_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
namespace Ressource_API.Features.${feature_plural}.${feature_name}Dtos;

public class Create${feature_name}Dto
{
    // TODO: Add properties needed to create a ${feature_name}
    // Example:
    // public string Name { get; set; } = string.Empty;
    // public string Description { get; set; } = string.Empty;
}
EOF
}

generate_update_dto_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
namespace Ressource_API.Features.${feature_plural}.${feature_name}Dtos;

public class Update${feature_name}Dto
{
    // TODO: Add properties needed to update a ${feature_name}
    // Example:
    // public string Name { get; set; } = string.Empty;
    // public string Description { get; set; } = string.Empty;
}
EOF
}

generate_info_dto_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
namespace Ressource_API.Features.${feature_plural}.${feature_name}Dtos;

public class ${feature_name}InfoDto
{
    public int Id { get; set; }
    // TODO: Add properties to return in responses
    // Example:
    // public string Name { get; set; } = string.Empty;
    // public DateTime CreatedAt { get; set; }
}
EOF
}

generate_iservice_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Features.${feature_plural}.${feature_name}Dtos;

namespace Ressource_API.Features.${feature_plural}.Services;

public interface I${feature_name}Service
{
    Task<IEnumerable<${feature_name}>> GetAll${feature_plural}Async(CancellationToken cancellationToken = default);
    Task<${feature_name}?> Get${feature_name}ByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<${feature_name}> Create${feature_name}Async(Create${feature_name}Dto dto, CancellationToken cancellationToken = default);
    Task<${feature_name}?> Update${feature_name}Async(int id, Update${feature_name}Dto dto, CancellationToken cancellationToken = default);
    Task<bool> Delete${feature_name}Async(int id, CancellationToken cancellationToken = default);
}
EOF
}

generate_service_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Features.${feature_plural}.${feature_name}Dtos;
using Ressource_API.Features.${feature_plural}.Repositories;
using Ressource_API.Features.${feature_plural}.Factories;

namespace Ressource_API.Features.${feature_plural}.Services;

public class ${feature_name}Service : I${feature_name}Service
{
    private readonly I${feature_name}Repository _repository;
    private readonly I${feature_name}Factory _factory;

    public ${feature_name}Service(
        I${feature_name}Repository repository,
        I${feature_name}Factory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<${feature_name}>> GetAll${feature_plural}Async(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<${feature_name}?> Get${feature_name}ByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<${feature_name}> Create${feature_name}Async(Create${feature_name}Dto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ${feature_name,,} = _factory.Create(dto);
        
        return await _repository.AddAsync(${feature_name,,}, cancellationToken);
    }

    public async Task<${feature_name}?> Update${feature_name}Async(int id, Update${feature_name}Dto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> Delete${feature_name}Async(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
EOF
}

generate_ifactory_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Features.${feature_plural}.${feature_name}Dtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.${feature_plural}.Factories;

public interface I${feature_name}Factory : IBaseFactory<${feature_name}>
{
    ${feature_name} Create(Create${feature_name}Dto dto);
}
EOF
}

generate_factory_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Features.${feature_plural}.${feature_name}Dtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.${feature_plural}.Factories;

public class ${feature_name}Factory : BaseFactory<${feature_name}>, I${feature_name}Factory
{
    /// <summary>
    /// Creates a ${feature_name} from a DTO
    /// </summary>
    public ${feature_name} Create(Create${feature_name}Dto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override ${feature_name} CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new ${feature_name}
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is Create${feature_name}Dto dto)
        {
            // Create from DTO
            return new ${feature_name}
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for ${feature_name} creation");
    }
}
EOF
}

generate_irepository_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.${feature_plural}.Repositories;

public interface I${feature_name}Repository : IBaseRepository<${feature_name}>
{
}
EOF
}

generate_repository_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Ressource_API.Common.Data;
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.${feature_plural}.Repositories;

public class ${feature_name}Repository : BaseRepository<${feature_name}>, I${feature_name}Repository
{
    public ${feature_name}Repository(ApplicationDbContext context) : base(context)
    {
    }
}
EOF
}

generate_controller_content() {
    local feature_name=$1
    local feature_plural=$2
    
    cat << EOF
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.${feature_plural}.Models;
using Ressource_API.Features.${feature_plural}.${feature_name}Dtos;
using Ressource_API.Features.${feature_plural}.Services;

namespace Ressource_API.Features.${feature_plural};

[ApiController]
[Route("api/[controller]")]
public class ${feature_name}Controller : ControllerBase
{
    private readonly I${feature_name}Service _service;
    private readonly ILogger<${feature_name}Controller> _logger;

    public ${feature_name}Controller(I${feature_name}Service service, ILogger<${feature_name}Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ${feature_plural,,}
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<${feature_name}>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<${feature_name}>>> GetAll${feature_plural}(CancellationToken cancellationToken)
    {
        try
        {
            var ${feature_plural,,} = await _service.GetAll${feature_plural}Async(cancellationToken);
            return Ok(${feature_plural,,});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ${feature_plural,,}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ${feature_plural,,}");
        }
    }

    /// <summary>
    /// Get a ${feature_name,,} by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(${feature_name}), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<${feature_name}>> Get${feature_name}ById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ${feature_name,,} = await _service.Get${feature_name}ByIdAsync(id, cancellationToken);

            if (${feature_name,,} == null)
            {
                return NotFound(\$"${feature_name} with ID {id} not found");
            }

            return Ok(${feature_name,,});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ${feature_name,,} with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ${feature_name,,}");
        }
    }

    /// <summary>
    /// Create a new ${feature_name,,}
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(${feature_name}), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<${feature_name}>> Create${feature_name}([FromBody] Create${feature_name}Dto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created${feature_name} = await _service.Create${feature_name}Async(dto, cancellationToken);

            return CreatedAtAction(
                nameof(Get${feature_name}ById),
                new { id = created${feature_name}.Id },
                created${feature_name}
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ${feature_name,,}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ${feature_name,,}");
        }
    }

    /// <summary>
    /// Update an existing ${feature_name,,}
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(${feature_name}), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<${feature_name}>> Update${feature_name}(int id, [FromBody] Update${feature_name}Dto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated${feature_name} = await _service.Update${feature_name}Async(id, dto, cancellationToken);

            if (updated${feature_name} == null)
            {
                return NotFound(\$"${feature_name} with ID {id} not found");
            }

            return Ok(updated${feature_name});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ${feature_name,,} with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ${feature_name,,}");
        }
    }

    /// <summary>
    /// Delete a ${feature_name,,}
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete${feature_name}(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.Delete${feature_name}Async(id, cancellationToken);

            if (!deleted)
            {
                return NotFound(\$"${feature_name} with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ${feature_name,,} with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ${feature_name,,}");
        }
    }
}
EOF
}

# ============================================================================
# FILE CREATION FUNCTIONS
# ============================================================================

create_all_files() {
    local feature_name=$1
    local feature_plural=$2
    local base_path=$3
    local model_file=$4
    
    echo -e "${BLUE}Creating files for ${feature_plural}...${NC}"
    
    # Handle model file
    if [ -n "$model_file" ]; then
        move_model_file "$model_file" "$base_path/Models" "$feature_name"
    else
        echo -e "${YELLOW}⚠${NC}  No model file provided - you'll need to create ${feature_name}.cs manually in $base_path/Models/"
    fi
    
    # DTOs
    create_file_with_content \
        "$base_path/${feature_name}Dtos/Create${feature_name}Dto.cs" \
        "$(generate_create_dto_content "$feature_name" "$feature_plural")"
    
    create_file_with_content \
        "$base_path/${feature_name}Dtos/Update${feature_name}Dto.cs" \
        "$(generate_update_dto_content "$feature_name" "$feature_plural")"
    
    create_file_with_content \
        "$base_path/${feature_name}Dtos/${feature_name}InfoDto.cs" \
        "$(generate_info_dto_content "$feature_name" "$feature_plural")"
    
    # Service Interface
    create_file_with_content \
        "$base_path/Services/I${feature_name}Service.cs" \
        "$(generate_iservice_content "$feature_name" "$feature_plural")"
    
    # Service Implementation
    create_file_with_content \
        "$base_path/Services/${feature_name}Service.cs" \
        "$(generate_service_content "$feature_name" "$feature_plural")"
    
    # Factory Interface
    create_file_with_content \
        "$base_path/Factories/I${feature_name}Factory.cs" \
        "$(generate_ifactory_content "$feature_name" "$feature_plural")"
    
    # Factory Implementation
    create_file_with_content \
        "$base_path/Factories/${feature_name}Factory.cs" \
        "$(generate_factory_content "$feature_name" "$feature_plural")"
    
    # Repository Interface
    create_file_with_content \
        "$base_path/Repositories/I${feature_name}Repository.cs" \
        "$(generate_irepository_content "$feature_name" "$feature_plural")"
    
    # Repository Implementation
    create_file_with_content \
        "$base_path/Repositories/${feature_name}Repository.cs" \
        "$(generate_repository_content "$feature_name" "$feature_plural")"
    
    # Controller
    create_file_with_content \
        "$base_path/${feature_name}Controller.cs" \
        "$(generate_controller_content "$feature_name" "$feature_plural")"
}

# ============================================================================
# MAIN EXECUTION
# ============================================================================

main() {
    # Validate input
    validate_arguments "$@"
    
    local feature_name=$1
    local model_file=${2:-""}
    
    validate_feature_name "$feature_name"
    validate_model_file "$model_file"
    
    # Generate feature names
    local feature_plural=$(pluralize "$feature_name")
    local base_path="Features/$feature_plural"
    
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Creating feature: ${feature_name}${NC}"
    echo -e "${BLUE}Plural form: ${feature_plural}${NC}"
    if [ -n "$model_file" ]; then
        echo -e "${BLUE}Model file: ${model_file}${NC}"
    fi
    echo -e "${BLUE}========================================${NC}"
    echo ""
    
    # Create directory structure
    create_directory_structure "$feature_name" "$feature_plural" "$base_path"
    echo ""
    
    # Create all files with content
    create_all_files "$feature_name" "$feature_plural" "$base_path" "$model_file"
    echo ""
    
    # Success message
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}✓ Feature ${feature_plural} created successfully!${NC}"
    echo -e "${GREEN}========================================${NC}"
    echo ""
    echo -e "${BLUE}Next steps:${NC}"
    echo "1. Complete the Factory CreateInstance method in ${feature_name}Factory.cs"
    echo "2. Complete the Update method property mapping in ${feature_name}Service.cs"
    echo "3. Fill in the DTO properties in Create${feature_name}Dto.cs and Update${feature_name}Dto.cs"
    echo "4. Add DbSet<${feature_name}> ${feature_plural} { get; set; } to your DbContext"
    echo "5. Register services in Program.cs:"
    echo "   - builder.Services.AddScoped<I${feature_name}Repository, ${feature_name}Repository>();"
    echo "   - builder.Services.AddScoped<I${feature_name}Factory, ${feature_name}Factory>();"
    echo "   - builder.Services.AddScoped<I${feature_name}Service, ${feature_name}Service>();"
    echo "6. Run migrations: dotnet ef migrations add Add${feature_plural}"
    echo "7. Update database: dotnet ef database update"
    echo ""
    echo -e "${BLUE}Available endpoints:${NC}"
    echo "  GET    /api/${feature_name}       - Get all ${feature_plural,,}"
    echo "  GET    /api/${feature_name}/{id}  - Get ${feature_name,,} by ID"
    echo "  POST   /api/${feature_name}       - Create new ${feature_name,,}"
    echo "  PUT    /api/${feature_name}/{id}  - Update ${feature_name,,}"
    echo "  DELETE /api/${feature_name}/{id}  - Delete ${feature_name,,}"
}

# Run the script
main "$@"