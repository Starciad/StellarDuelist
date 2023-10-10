#!/bin/bash

# Clear the terminal screen
clear

# Define solutions and publishing directories
desktopGL="../StardustDefender/StardustDefender.DesktopGL.csproj"
windowsDX="../StardustDefender/StardustDefender.WindowsDX.csproj"
outputDirectory="../Publish"

# List of target platforms
platforms=("win-x64" "linux-x64" "osx-x64")

# Function to publish a project for a given platform
publish_project() {
    project_path="$1"
    platform="$2"
    echo "Publishing $project_path for $platform..."
    dotnet publish "$project_path" -c Release -r "$platform" --output "$outputDirectory/stardust-defender-v-$platform"
    echo "Publishing to $platform completed."
}

# Function to delete specified subdirectories
delete_subdirectories() {
    destination="$1"
    subdirectories_to_delete=("$2")

    for subdirectory in "${subdirectories_to_delete[@]}"; do
        subdirectory_path="$destination/$subdirectory"
        if [ -d "$subdirectory_path" ]; then
            rm -r "$subdirectory_path"
            echo "Subdirectory $subdirectory deleted successfully."
        else
            echo "Subdirectory $subdirectory not found in $destination."
        fi
    done
}

# Delete existing directories
if [ -d "$outputDirectory" ]; then
    rm -r "$outputDirectory"
    echo "Existing directory deleted."
fi

# Publish WindowsDX
clear
echo "Publishing WindowsDX for Win-x64..."
publish_project "$windowsDX" "win-x64"
sleep 2
clear

# Publish DesktopGL for all platforms
echo "Publishing DesktopGL to all platforms..."
for platform in "${platforms[@]}"; do
    publish_project "$desktopGL" "$platform"
    sleep 1
    echo "Next..."
    sleep 1.5
    clear
done

echo "All publishing processes have been completed."
sleep 2
clear

# Copy content folder and delete specific subdirectories
clear
echo "Copying content folder..."

source="../StardustDefender/Content"
destination="$outputDirectory/Content"
subdirectories_to_delete=("bin" "obj")

# Copy the source folder to the destination
cp -r "$source" "$destination"

# Delete the specified subdirectories
delete_subdirectories "$destination" "${subdirectories_to_delete[@]}"

echo "Operation completed. The folder has been copied to $destination, and the specified subdirectories have been deleted."
sleep 2
clear