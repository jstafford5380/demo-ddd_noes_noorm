#!/usr/bin/pwsh

Param(
    [parameter(Mandatory, HelpMessage = "The name of the project that will be dockerized. Dockerfile must be in that directory")]
    [Alias("Project")][String] $project_name,
    [parameter(Mandatory, HelpMessage = "The image name that will be used to tag the image.")]
    [Alias("Name")][String] $image_name,
    [parameter(Mandatory, HelpMessage = "The tag that will be used to tag the image.")]
    [Alias("Tag")][String] $image_tag,
    [parameter(Mandatory, HelpMessage = "MyGet pre-auth'd URL")]
    [Alias("MyGetSource")][String] $myget_source,
    [parameter(HelpMessage = "Flag for whether or not this tag should also be tagged as latest.")]
    [Alias("Latest")][Switch] $tag_latest,
    [parameter(HelpMessage = "Flag for whether or not the images should also be pushed.")]
    [Alias("Push")][Switch] $push_images,
    [parameter(HelpMessage = "Flag to disable the publish step.")]
    [Alias("NoBuild")][Switch] $skip_publish,
    [parameter(HelpMessage = "Build configuration. Default 'Release'")]
    [Alias("Config")][String] $build_config = "Release",
    [parameter(HelpMessage = "Publish output directory. Default 'out'")]
    [Alias("Output")][String] $output_directory = "out"
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

function publish_project() {
    attempt dotnet restore src -s $myget_source -s https://api.nuget.org/v3/index.json
    attempt dotnet publish src/$project_name/$project_name.csproj -o $output_directory -c $build_config
}

function build_image() {
    $image = "${image_name}:${image_tag}"
    Write-Output "Building docker image called '$image' using binaries located in the directory 'src/$project_name/$output_directory'!"
    attempt docker build -t $image --build-arg bin_dir=$output_directory src/$project_name/
    if($tag_latest) {
        Write-Output "Tagging as latest..."
        attempt docker tag $image "${image_name}:latest"
    }
}

$ErrorActionPreference="Stop"

if(-Not $skip_publish) {
    publish_project
}

build_image

if($push_images) {
    attempt docker push "${image_name}:${image_tag}"

    if($tag_latest) {
        attempt docker push "${image_name}:latest"    
    }
}