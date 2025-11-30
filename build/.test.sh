#!/usr/bin/env bash

cd ..
dotnet test --test-adapter-path:. --logger:"junit;LogFilePath=../../results/{assembly}.TestResult.xml;MethodFormat=Class;FailureBodyFormat=Verbose" --settings build/coverlet.runsettings --results-directory results/coverage
reportgenerator -reports:results/coverage/*/coverage.opencover.xml -targetdir:results/report -reportTypes:"TextSummary;Html"
firefox results/report/index.html
cat results/report/Summary.txt