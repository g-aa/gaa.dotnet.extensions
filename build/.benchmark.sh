#!/usr/bin/env bash

cd ..
dotnet run --project ./benchmark/Gaa.Extensions.Benchmark --configuration Release --filter 'Gaa.Extensions.Benchmark.Observer*' --join
cat ./BenchmarkDotNet.Artifacts/results/*.md