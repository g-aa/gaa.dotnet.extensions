#!/usr/bin/env bash

cd ..
dotnet run --project ./benchmark/Gaa.Extensions.Benchmark --configuration Release --filter '*' --join
cat ./BenchmarkDotNet.Artifacts/results/*.md