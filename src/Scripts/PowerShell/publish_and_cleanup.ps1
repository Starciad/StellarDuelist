# Clear the console window
Clear-Host

# Define solutions and publishing directories
$desktopGL = "..\StardustDefender\StardustDefender.DesktopGL.csproj"
$windowsDX = "..\StardustDefender\StardustDefender.WindowsDX.csproj"
$outputDirectory = "..\Publish"

# List of target platforms
$platforms = @("win-x64", "linux-x64", "osx-x64")

# Function to publish a project for a given platform
function Publish-Project($projectPath, $platform) {
    Write-Host "Publishing $projectPath for $platform..."
    dotnet publish $projectPath -c Release -r $platform --output "$outputDirectory\stardust-defender-v-$platform"
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
Publish-Project $windowsDX "win-x64"

Start-Sleep -Milliseconds 2500
Clear-Host

# Publish DesktopGL for all platforms
Write-Host "Publishing DesktopGL to all platforms..."
foreach ($platform in $platforms) {
    Publish-Project $desktopGL $platform
    Start-Sleep -Milliseconds 1000
    Write-Host "Next..."
    Start-Sleep -Milliseconds 1500
    Clear-Host
}

Write-Host "All publishing processes have been completed."
Start-Sleep -Milliseconds 2500
Clear-Host

# Copy content folder and delete specific subdirectories
Clear-Host
Write-Host "Copying content folder..."

$source = "..\StardustDefender\Content"
$destination = "$outputDirectory\Content"
$subdirectoriesToDelete = @("bin", "obj")

# Copy the source folder to the destination
Copy-Item -Path $source -Destination $destination -Recurse

# Delete the specified subdirectories
Delete-Subdirectories $destination $subdirectoriesToDelete

Write-Host "Operation completed. The folder has been copied to $destination, and the specified subdirectories have been deleted."

Start-Sleep -Milliseconds 2500
Clear-Host