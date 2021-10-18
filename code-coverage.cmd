@echo off

REM Install reportgenerator tool
dotnet tool install --global dotnet-reportgenerator-globaltool

REM Restore + Build
dotnet build --nologo || exit /b

REM Test + Coverage collect
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --nologo --no-build

REM Remove previous Coverage Report.
del "coverage-report/*"

REM Generate Coverage Report
reportgenerator "-reports:**/coverage.cobertura.xml" "-targetdir:coverage-report" -reporttypes:Html
