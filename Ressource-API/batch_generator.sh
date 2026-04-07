#!/bin/bash

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

# ============================================================================
# VALIDATION
# ============================================================================

validate_arguments() {
    if [ $# -eq 0 ]; then
        echo -e "${RED}Error: Models directory path is required${NC}"
        echo "Usage: ./batchCreateFeatures.sh [models_directory]"
        echo "Example: ./batchCreateFeatures.sh ./MyModels"
        exit 1
    fi
}

validate_directory() {
    local dir_path=$1
    
    if [ ! -d "$dir_path" ]; then
        echo -e "${RED}Error: Directory '$dir_path' does not exist${NC}"
        exit 1
    fi
}

# ============================================================================
# MAIN
# ============================================================================

main() {
    validate_arguments "$@"
    
    local models_dir=$1
    validate_directory "$models_dir"
    
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Batch Feature Creation${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
    
    local success_count=0
    local fail_count=0
    local file_count=0
    
    # Loop through each .cs file
    for model_file in "$models_dir"/*.cs; do
        # Check if file actually exists (handles case where no .cs files exist)
        if [ ! -e "$model_file" ]; then
            continue
        fi
        
        # Verify it's a regular file, not a directory
        if [ ! -f "$model_file" ]; then
            echo -e "${YELLOW}⚠ Skipping non-file: $model_file${NC}"
            continue
        fi
        
        ((file_count++))
        
        # Extract filename without extension
        local filename=$(basename "$model_file" .cs)
        
        echo -e "${BLUE}[$file_count] Processing: ${filename}...${NC}"
        
        # Call the modified createFeature.sh script
        if ./createFeature.sh "$filename" "$model_file"; then
            ((success_count++))
            echo -e "${GREEN}✓ Successfully created feature for ${filename}${NC}"
        else
            ((fail_count++))
            echo -e "${RED}✗ Failed to create feature for ${filename}${NC}"
        fi
        
        echo ""
    done
    
    # Check if no files were found
    if [ $file_count -eq 0 ]; then
        echo -e "${YELLOW}No .cs files found in '$models_dir'${NC}"
        exit 0
    fi
    
    # Summary
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Summary${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo -e "${GREEN}Successful: $success_count${NC}"
    echo -e "${RED}Failed: $fail_count${NC}"
    echo -e "${BLUE}Total: $file_count${NC}"
}

main "$@"