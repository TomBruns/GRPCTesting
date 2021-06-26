# GRPC vs HTTP Performance Comparison

![Overview Placeholder](doc/graph.jpg?raw=true)



``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1052 (21H1/May2021Update)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.301
  [Host]   : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT
  .NET 5.0 : .NET 5.0.7 (5.0.721.25508), X64 RyuJIT

Job=.NET 5.0  Runtime=.NET 5.0  

```
|                            Method | count |       Mean |    Error |   StdDev |     Median | Ratio | RatioSD |
|---------------------------------- |------ |-----------:|---------:|---------:|-----------:|------:|--------:|
| **CallGrpcEndpointAsyncReuseChannel** |     **1** |   **416.3 μs** |  **8.27 μs** | **11.86 μs** |   **414.0 μs** |  **0.20** |    **0.01** |
|   CallGrpcEndpointAsyncNewChannel |     1 | 2,021.1 μs | 38.30 μs | 35.82 μs | 2,016.5 μs |  0.98 |    0.03 |
| CallWebAPIEndpointReuseHTTPClient |     1 |   711.2 μs |  8.25 μs |  7.31 μs |   711.8 μs |  0.35 |    0.01 |
|   CallWebAPIEndpointNewHTTPClient |     1 | 2,053.3 μs | 39.19 μs | 36.66 μs | 2,053.1 μs |  1.00 |    0.00 |
|                                   |       |            |          |          |            |       |         |
| **CallGrpcEndpointAsyncReuseChannel** |    **10** |   **453.4 μs** |  **6.88 μs** |  **7.36 μs** |   **454.8 μs** |  **0.21** |    **0.01** |
|   CallGrpcEndpointAsyncNewChannel |    10 | 2,063.5 μs | 31.42 μs | 29.39 μs | 2,068.6 μs |  0.96 |    0.05 |
| CallWebAPIEndpointReuseHTTPClient |    10 |   761.3 μs |  3.97 μs |  3.32 μs |   761.5 μs |  0.36 |    0.02 |
|   CallWebAPIEndpointNewHTTPClient |    10 | 2,063.5 μs | 33.77 μs | 83.48 μs | 2,035.2 μs |  1.00 |    0.00 |
|                                   |       |            |          |          |            |       |         |
| **CallGrpcEndpointAsyncReuseChannel** |   **100** |   **544.5 μs** |  **7.27 μs** |  **6.80 μs** |   **542.3 μs** |  **0.24** |    **0.00** |
|   CallGrpcEndpointAsyncNewChannel |   100 | 2,097.0 μs | 12.33 μs | 10.29 μs | 2,094.7 μs |  0.93 |    0.02 |
| CallWebAPIEndpointReuseHTTPClient |   100 |   803.8 μs | 15.56 μs | 17.92 μs |   797.6 μs |  0.36 |    0.01 |
|   CallWebAPIEndpointNewHTTPClient |   100 | 2,253.8 μs | 41.20 μs | 36.52 μs | 2,270.3 μs |  1.00 |    0.00 |
|                                   |       |            |          |          |            |       |         |
| **CallGrpcEndpointAsyncReuseChannel** |   **500** |   **911.5 μs** |  **5.63 μs** |  **4.99 μs** |   **912.4 μs** |  **0.28** |    **0.01** |
|   CallGrpcEndpointAsyncNewChannel |   500 | 2,518.7 μs | 47.20 μs | 39.42 μs | 2,498.0 μs |  0.79 |    0.02 |
| CallWebAPIEndpointReuseHTTPClient |   500 | 1,813.7 μs | 35.15 μs | 34.52 μs | 1,806.0 μs |  0.56 |    0.01 |
|   CallWebAPIEndpointNewHTTPClient |   500 | 3,208.7 μs | 60.69 μs | 69.89 μs | 3,206.8 μs |  1.00 |    0.00 |


---
### How to execute

Open two (2) command windows each in one (1) of the project folders

- 1st start the server project

```markdown
    dotnet run --configuration Release
```    
- 2nd start the client (benchmark) project

    - dotnet run --configuration Release