param(
    [ValidateNotNullOrEmpty()]
    [ValidateSet('all','linux','windows','x64','x86')]
    [string]$Platform="all",

    [ValidateNotNullOrEmpty()]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration="Release"
)

function ExitIfFailLastExitCode {
    if ($LastExitCode -ne 0) {
        exit $LastExitCode
    }
}

$rootDirectory = Resolve-Path "$(Split-Path -Parent $PSCommandPath)\..\..\..\..\.."
$nugetPath = (Resolve-Path "$rootDirectory\Build\Tools\nuget.exe").Path
$vsWhere = (Resolve-Path "$rootDirectory\Build\Tools\vswhere.exe").Path
$msBuildPath = & "$vsWhere" -products 'Microsoft.VisualStudio.Product.BuildTools' -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1
if (!$msBuildPath) {
    $msBuildPath = & "$vsWhere" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | select-object -first 1
}

Write-Host "Building Platform=$Platform and Configuration=$Configuration"

$nugetPath = "$rootDirectory\Build\Tools\nuget.exe"
$profilerRoot = "$rootDirectory\src\Agent\NewRelic\Profiler"
$profilerSolutionPath = "$profilerRoot\NewRelic.Profiler.sln"
$outputPath = "$rootDirectory\src\Agent\_profilerBuild"
$linuxOutputPath = "$outputPath\linux-release"

$restoreNuget = $Platform -eq "all" -or $Platform -eq "windows" -or $Platform -eq "x64" -or $Platform -eq "x86"
$buildx64 = $Platform -eq "all" -or $Platform -eq "windows" -or $Platform -eq "x64"
$buildx86 = $Platform -eq "all" -or $Platform -eq "windows" -or $Platform -eq "x86"
$buildLinux = $Platform -eq "all" -or $Platform -eq "linux"

if ($Platform -eq "all") {
    if (Test-Path $outputPath) { Remove-Item $outputPath -Recurse }
    if (Test-Path $outputPath) { Write-Error "Ouput path not cleared out: $outputPath"; exit 1; }
}

if ($restoreNuget) {
    Write-Host "-- Profiler build: Restoring NuGet packages"
    & $nugetPath restore $profilerSolutionPath -Source "https://www.nuget.org/api/v2"
    ExitIfFailLastExitCode
}

if ($buildx64) {
    Write-Host "-- Profiler build: x64-$Configuration"
    & $msBuildPath /p:Platform=x64 /p:Configuration=$Configuration $profilerSolutionPath
    ExitIfFailLastExitCode
}

if ($buildx86) {
    Write-Host "-- Profiler build: x86-$Configuration"
    & "$msBuildPath" /p:Platform=Win32 /p:Configuration=$Configuration $profilerSolutionPath
    ExitIfFailLastExitCode
}

if ($buildLinux) {
    Write-Host "-- Profiler build: linux-release"

    if ($Configuration -eq "Debug") {
        Write-Host "Configuration=Debug is not currently supported by this script when building the linux profiler. Building Configuration=Release instead."
    }

    & $profilerRoot\build\scripts\build_linux.ps1
    ExitIfFailLastExitCode

    if (!(Test-Path $linuxOutputPath)) { New-Item $linuxOutputPath -ItemType Directory }
    Move-Item "$profilerRoot\libNewRelicProfiler.so" "$linuxOutputPath"
}
