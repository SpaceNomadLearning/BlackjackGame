#!/bin/bash

# Install reportgenerator tool
dotnet tool install --global dotnet-reportgenerator-globaltool

# Restore + Build
dotnet build --nologo || exit

# REM Test + Coverage collect
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura --nologo --no-build

# REM Remove previous Coverage Report.
rm -r "/coverage-report/*"

# Generate Coverage Report
reportgenerator "-reports:tests/**/coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html
