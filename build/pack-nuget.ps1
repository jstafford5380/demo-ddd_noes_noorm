#!/usr/bin/pwsh

Param(
    [parameter( HelpMessage = "Names of projects to be packed.")]
    [Alias("Projects")][String[]] $pack_projects,
    [parameter(HelpMessage = "The build number that will be used to version the packages.")]
    [Alias("BuildNumber")][Int32] $build_number,
    [parameter(Mandatory, HelpMessage = "The MyGet pre-auth'd URL")]
    [Alias("MyGetSource")][String] $myget_source,
    [parameter(HelpMessage = "If the Symbols server URL is included, this script will push symbols to it.")]
    [Alias("SymbolsSource")][String] $myget_symbols,
    [parameter(HelpMessage = "The build configuration. Default 'Release'")]
    [Alias("BuildConfiguration")][String] $build_config = "Release",
    [parameter(HelpMessage = "Flag for whether or not the package should be pushed after packing.")]
    [Alias("Push")][Switch] $push_package,
    [parameter(HelpMessage = "If set, script will delete all nupkg files before beginning.")]
    [Alias("ClearPackages")][Switch] $clear_packages,
    [parameter(HelpMessage = "If set, the build action will be skipped")]
    [Alias("NoBuild")][Switch] $no_build
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

function build_project() {
    attempt dotnet build src -c $build_config
}

function pack_projects() {
    foreach($project in $pack_projects){
        if($myget_symbols) {            
            dotnet pack src/$project/$project.csproj -c $build_config /p:BuildNumber=$build_number --include-symbols
        } else {            
            dotnet pack src/$project/${project}.csproj -c $build_config /p:BuildNumber=$build_number
        }        
    }    
}

function push_packages() {
    if($push_package) {
        foreach($package in (Get-ChildItem -Path . -File -Recurse -Filter "*.nupkg")) {
            Write-Output $package.FullName
            if($myget_symbols) {
                attempt dotnet nuget push $package.FullName -s $myget_source -ss $myget_symbols
            } else {
                attempt dotnet nuget push $package.FullName -s $myget_source
            }
        }
    }
}

$ErrorActionPreference="Stop"

if(-Not $no_build) {
    build_project
}

if($clear_packages) {    
    foreach($package in (Get-ChildItem -Path . -File -Recurse -Filter "*.nupkg")) {        
        Remove-Item -Path $package.FullName  
    }
}

# run this explicitly because sometimes build wont run the pack even if the option is set in the csproj
attempt pack_projects 
attempt push_packages
