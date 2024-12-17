#!/usr/bin/env bash

TEST_RESULTS_DIR="/test-results"

mkdir -p $TEST_RESULTS_DIR

ls -a

dotnet test DatabaseTests.csproj --logger "trx;LogFileName=$TEST_RESULTS_DIR/test-results.trx" --results-directory $TEST_RESULTS_DIR

TEST_EXIT_CODE=$?

echo "Test results directory content:"
ls -la $TEST_RESULTS_DIR

# Exit with the test result code
exit $TEST_EXIT_CODE
