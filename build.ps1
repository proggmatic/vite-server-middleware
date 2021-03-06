param (
    [switch]$noPause = $false
)
$ErrorActionPreference = "Stop"

Write-Host -------------------------------------------------------------------------------
Write-Host PACKING ViteServerMiddleware...
Write-Host -------------------------------------------------------------------------------


& dotnet pack ./src/Proggmatic.SpaServices.Vite/Proggmatic.SpaServices.Vite.csproj --output ./_builds


if (-not $noPause) {
    Write-Host "Successfully done! Your package is in './_builds' folder. Press any key to continue...`n" -NoNewLine -ForegroundColor Green
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
