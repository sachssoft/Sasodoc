# Package Build and Publish Guide

This guide describes how to build and publish the Sachssoft Sasodoc packages to NuGet.org.

## Configuration

Set the package version and NuGet API key:

```powershell
$version = "<version>"
$apiKey = "<api-key>"
```

Example:

```powershell
$version = "2.0.0"
$apiKey = "YOUR_NUGET_API_KEY"
```

## Project Structure

The Sasodoc packages are located in the `Projects` directory:

```text
Projects
├── Sachssoft.Sasodoc
├── Sachssoft.Sasodoc.Json
├── Sachssoft.Sasodoc.Xml
├── Sachssoft.Sasodoc.Ini
├── Sachssoft.Sasodoc.Yaml
└── Sachssoft.Sasodoc.Binary
```

The format packages depend on the main `Sachssoft.Sasodoc` package.

For this reason, the main package should be published first when publishing a new version.

## Build Core Package

Navigate to the Sasodoc repository directory:

```powershell
cd <project-directory-path>
```

Create the package output directory if it does not already exist:

```powershell
New-Item -ItemType Directory -Force -Path .\Packages
```

First, build the main Sasodoc package:

```powershell
dotnet pack .\Projects\Sachssoft.Sasodoc\Sachssoft.Sasodoc.csproj -c Release -o .\Packages
```

The generated package should be located at:

```text
Packages\Sachssoft.Sasodoc.<version>.nupkg
```

## Publish Core Package

Publish the main Sasodoc package to NuGet.org before publishing the dependent format packages:

```powershell
dotnet nuget push ".\Packages\Sachssoft.Sasodoc.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"
```

NuGet.org may require a short amount of time before the newly published package version becomes available for dependency resolution.

## Build Format Packages

After the main Sasodoc package has been published, build the format packages:

```powershell
dotnet pack .\Projects\Sachssoft.Sasodoc.Json\Sachssoft.Sasodoc.Json.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Xml\Sachssoft.Sasodoc.Xml.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Ini\Sachssoft.Sasodoc.Ini.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Yaml\Sachssoft.Sasodoc.Yaml.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Binary\Sachssoft.Sasodoc.Binary.csproj -c Release -o .\Packages
```

## Publish Format Packages

After all format packages have been built successfully, publish them to NuGet.org:

```powershell
dotnet nuget push ".\Packages\Sachssoft.Sasodoc.Json.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"

dotnet nuget push ".\Packages\Sachssoft.Sasodoc.Xml.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"

dotnet nuget push ".\Packages\Sachssoft.Sasodoc.Ini.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"

dotnet nuget push ".\Packages\Sachssoft.Sasodoc.Yaml.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"

dotnet nuget push ".\Packages\Sachssoft.Sasodoc.Binary.$version.nupkg" `
    --api-key $apiKey `
    --source "https://api.nuget.org/v3/index.json"
```

## Build All Packages

After the core package version is available for dependency resolution, all packages can be built with the following commands:

```powershell
dotnet pack .\Projects\Sachssoft.Sasodoc\Sachssoft.Sasodoc.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Json\Sachssoft.Sasodoc.Json.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Xml\Sachssoft.Sasodoc.Xml.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Ini\Sachssoft.Sasodoc.Ini.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Yaml\Sachssoft.Sasodoc.Yaml.csproj -c Release -o .\Packages

dotnet pack .\Projects\Sachssoft.Sasodoc.Binary\Sachssoft.Sasodoc.Binary.csproj -c Release -o .\Packages
```

## Verify Generated Packages

Before publishing, verify that the expected packages exist in the `Packages` directory:

```powershell
Get-ChildItem .\Packages\*.nupkg
```

Expected packages:

```text
Sachssoft.Sasodoc.<version>.nupkg
Sachssoft.Sasodoc.Json.<version>.nupkg
Sachssoft.Sasodoc.Xml.<version>.nupkg
Sachssoft.Sasodoc.Ini.<version>.nupkg
Sachssoft.Sasodoc.Yaml.<version>.nupkg
Sachssoft.Sasodoc.Binary.<version>.nupkg
```

## Troubleshooting Version Issues

If a newly published package version cannot be found, clear the local NuGet caches:

```powershell
dotnet nuget locals all --clear
```

Then rebuild the affected projects in Visual Studio using the context menu.

If the package version is still not found, restore the packages without using the local cache:

```powershell
dotnet restore --no-cache
```

This step is normally optional and should only be necessary if clearing the NuGet caches and rebuilding the projects does not resolve the issue.

## Notes

* Always publish `Sachssoft.Sasodoc` before packages that depend on the same newly published version.
* Ensure that `$version` matches the version configured for the packages.
* Do not publish packages if the Release build produces warnings or errors.
* Review the generated `.nupkg` files before publishing a new stable release.
* Published NuGet package versions are immutable and cannot be replaced with a different package using the same version number.
