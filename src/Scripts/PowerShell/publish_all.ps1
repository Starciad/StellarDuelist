# =================================== #
#                                     #
#   This script aims to assist in     #
# compiling all projects and creating #
# standardized folders in accordance  #
#   with Stardust Defender version    #
#       publishing regulations.       #
#                                     #
# =================================== #

# Clear the console window
Clear-Host

# Define solutions and publishing directories
$desktopGL = "..\..\StardustDefender\StardustDefender.DesktopGL.csproj"
$windowsDX = "..\..\StardustDefender\StardustDefender.WindowsDX.csproj"
$outputDirectory = "..\..\Publish"

# List of target platforms
$platforms = @("win-x64", "linux-x64", "osx-x64")

# Function to publish a project for a given platform
function Publish-Project($projectName, $projectPath, $platform) {
    Write-Host "Publishing $projectPath for $platform..."
    dotnet publish $projectPath -c Release -r $platform --output "$outputDirectory\stardust-defender-$projectName-v0.0.0.0-$platform"
    Write-Host "Publishing to $platform completed."
}

# Function to delete specified subdirectories
function Delete-Subdirectories($destination, $subdirectoriesToDelete) {
    foreach ($subdirectory in $subdirectoriesToDelete) {
        $subdirectoryPath = Join-Path -Path $destination -ChildPath $subdirectory
        if (Test-Path $subdirectoryPath -PathType Container) {
            Remove-Item -Path $subdirectoryPath -Recurse -Force
            Write-Host "Subdirectory $subdirectory deleted successfully."
        } else {
            Write-Host "Subdirectory $subdirectory not found in $destination."
        }
    }
}

# Delete existing directories
if (Test-Path $outputDirectory -PathType Container) {
    Remove-Item -Path $outputDirectory -Recurse -Force
    Write-Host "Existing directory deleted."
}

# Publish WindowsDX
Clear-Host
Write-Host "Publishing WindowsDX for Win-x64..."
Publish-Project "windowsdx" $windowsDX $platforms[0]

# Publish DesktopGL for all platforms
Write-Host "Publishing DesktopGL to all platforms..."
foreach ($platform in $platforms) {
    Publish-Project "desktopgl" $desktopGL $platform
    Write-Host "Next..."
}

Write-Host "All publishing processes have been completed."

# Copy content folder and delete specific subdirectories
Write-Host "Copying content folder..."

$source = "..\..\StardustDefender\Content"
$destination = "$outputDirectory\stardust-defender-v0.0.0.0-assets-full\Content"
$subdirectoriesToDelete = @("bin", "obj")

# Copy the source folder to the destination
Copy-Item -Path $source -Destination $destination -Recurse

# Delete the specified subdirectories
Delete-Subdirectories $destination $subdirectoriesToDelete

Write-Host "Operation completed. The folder has been copied to $destination, and the specified subdirectories have been deleted."