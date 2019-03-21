#!/usr/bin/pwsh

Param(
    [parameter(Mandatory, HelpMessage = "Name of the project that will be built (no extension).")]
    [Alias("Project")][String] $project_name,
    [parameter(Mandatory, HelpMessage = "The MyGet pre-auth'd URL")]
    [Alias("MyGetSource")][String] $myget_source,
    [parameter(HelpMessage = "Directory to which the binaries will be published. Default 'out'")]
    [Alias("Output")][String] $output_directory = "out",
    [parameter(HelpMessage = "Build Configuration. Default 'Release'")]
    [Alias("Configuration")][String] $build_config = "Release"
)

function attempt() {
    try {
        $cmd, $params = $args
        $params = @($params)   
        Write-Output "Executing: $cmd $params"     
        $output = & $cmd @params 2>&1
        if (-Not $?) {
            throw $output
        }
        $output
    } catch {
        Write-Output "COMMAND FAILED: $cmd $params --> $_"
        exit 1
    }
}

function publish_binaries() {
    attempt dotnet restore src -s $myget_source -s https://api.nuget.org/v3/index.json
    attempt dotnet publish src/$project_name/$project_name.csproj -o $output_directory -c $build_config
}

$ErrorActionPreference="Stop"

attempt publish_binaries