﻿$VerbosePreference="Continue"

$version = $args[0]
if (!$version) {
    $version = "0.0.0"
}

Write-Host "Version: $version"

# Update NuGet package version
$FullPath = Resolve-Path $PSScriptRoot\..\DumpStackToCSharpCode\DumpStackToCSharpCode\DumpStackToCSharpCode.csproj
Write-Host $FullPath
[xml]$content = Get-Content $FullPath
$content.Project.PropertyGroup.PackageVersion = $version
$content.Save($FullPath)

# Update VSIX version
$FullPath = Resolve-Path $PSScriptRoot\..\DumpStackToCSharpCode\DumpStackToCSharpCode\source.extension.vsixmanifest
Write-Host $FullPath
[xml]$content = Get-Content $FullPath
$content.PackageManifest.Metadata.Identity.Version = $version
$content.Save($FullPath)
