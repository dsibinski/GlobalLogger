#!/bin/bash
msbuild GlobalLogger.sln /t:Rebuild /p:Configuration=Release
printf "\n\nBuild finished.\n\n"
sleep 3
printf "\n\nExecution of Unit Tests starting...\n\n"
mono /usr/dev/NUnitTools/nunit3-console.exe ./GlobalLogger.Tests/GlobalLogger.Tests.csproj
