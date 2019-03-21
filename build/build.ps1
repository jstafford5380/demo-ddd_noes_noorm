#!/usr/bin/pwsh

Param(
    [parameter(Mandatory)][Alias("Solution")][String] $project_name,
    [parameter(Mandatory)][Alias("MyGetSource")][String] $myget_source,    
    [parameter()][Alias("BuildConfiguration")][String] $build_configuration = "Release",
    [parameter()][Alias("SonarUrl")][String] $sonar_url,
    [parameter()][Alias("SonarKey")][String] $sonar_key,
    [parameter()][Alias("Test")][Switch] $run_tests,
    [parameter()][Alias("Analyze")][Switch] $run_analysis
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

function validate_parameters {    
    if($run_analysis) {                
        if([string]::IsNullOrEmpty($sonar_url) -or [string]::IsNullOrEmpty($sonar_key)) {            
            throw "SonarUrl and SonarKey are required to run static code analysis!"
        }
    }
}

function run_unit_tests {
    dotnet tool install -g trx2junit
    
    $testFailed = $false
    New-Item -ItemType Directory -Force -Path .\test-results    
    foreach($testProject in (Get-ChildItem -Path src -File -Recurse -Filter "*.Tests.csproj" )) {
        dotnet test $testProject.FullName `
        /p:CollectCoverage=true `
        /p:CoverletOutputFormat=opencover `
        /p:CoverletOutput=./test-results/coverage `
        --logger:trx -r ./test-results

        if($LASTEXITCODE -ne 0) {
            # record the fail but allow this to continue so that all tests are run
            $testFailed = $true
        }
        
        Get-ChildItem -Path "$($testProject.Directory.FullName)\test-results" -Recurse -File -Filter "*.trx" | `
        Move-Item -Force -Destination .\test-results `
    }

    foreach($testOutput in (Get-ChildItem -Path .\test-results -File -Filter "*.trx")) {
        & "${toolsPath}trx2junit" $testOutput.FullName
        Remove-Item $testOutput.FullName
    }

    if($testFailed) {
        exit 1
    }
}

$ErrorActionPreference="Stop"

$toolsPath = if($IsWindows -or ($Env:OS -and $Env:OS.Contains("Windows"))) { "$env:USERPROFILE\.dotnet\tools\" } else { "$HOME/.dotnet/tools/" }
attempt validate_parameters

attempt dotnet restore src -s $myget_source -s https://api.nuget.org/v3/index.json

if($run_analysis) {
    dotnet tool install -g dotnet-sonarscanner
    & "${toolsPath}dotnet-sonarscanner" begin `
        /k:"$project_name" `
        /d:sonar.host.url="$sonar_url" `
        /d:sonar.login="$sonar_key" `
        /d:sonar.cs.opencover.reportsPaths="./test-results/*.opencover.xml"
    if(-Not $?) { exit 1 }
}

attempt dotnet build src/${project_name}.sln -c $build_configuration

if($run_tests) {
    attempt run_unit_tests
}

if($run_analysis) {
    & "${toolsPath}dotnet-sonarscanner" end /d:sonar.login="$sonar_key"
    if(-Not $?) { exit 1 }
}