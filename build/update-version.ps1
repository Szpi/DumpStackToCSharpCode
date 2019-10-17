$VerbosePreference="Continue"

$version = $args[0]
if (!$version) {
    $version = "0.0.0"
}

Write-Host "Version: $version"

# Update VSIX version
$FullPath = Resolve-Path $PSScriptRoot\..\DumpStackToCSharpCode\DumpStackToCSharpCode\source.extension.vsixmanifest
Write-Host $FullPath
[xml]$content = Get-Content $FullPath
$content.PackageManifest.Metadata.Identity.Version = $version
$content.Save($FullPath)
