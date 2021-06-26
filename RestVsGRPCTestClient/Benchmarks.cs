using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Grpc.Net.Client;

using rest = RestVsGRPCTestClient.Entities;

namespace RestVsGRPCTestClient
{
    // This contains BenchmarkDotNet performance tests
    //   https://benchmarkdotnet.org/
    [SimpleJob(RuntimeMoniker.Net50)]
    public class Benchmarks
    {
        // server endpoints
        const string HTTP_ENDPOINT = @"https://localhost:5000";
        const string GRPC_ENDPOINT = @"https://localhost:5001";

        // global variables
        HttpClient _httpClient;
        EchoTester.EchoTesterClient _client;

        // set params to test different payload sizes
        // (note: 1000 fails with default settings)
        [Params(1, 10, 100, 500)]
        public int count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            #region === GRPC setup ===========================================
            // The port number(5001) must match the port of the gRPC server.
            // https://docs.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-5.0
            // Creating a channel can be an expensive operation. Reusing a channel for gRPC calls provides performance benefits.
            var channel = GrpcChannel.ForAddress(GRPC_ENDPOINT);
            // gRPC clients are created with channels. gRPC clients are lightweight objects and don't need to be cached or reused
            // but I must be doing something wrong since I am exhausting the available ports!
            _client = new EchoTester.EchoTesterClient(channel);
            #endregion

            #region === REST setup ===========================================
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(HTTP_ENDPOINT);
            #endregion
        }

        // ==========================================
        // Call Service using GRPC (HTTP2)
        //  Note: we reuse the GRPC Client
        // ==========================================
        [Benchmark]
        public async Task CallGrpcEndpointAsyncReuseChannel()
        {
            var echoRequest = new EchoRequestDTO();
            for (int ctr = 1; ctr <= count; ctr++)
            {
                echoRequest.Books.Add(new BookDTO() { Title = $"The Goal [{ctr}]", Author = @"Eliyahu M. Goldratt" });
            };

            var reply = await _client.EchoAsync(echoRequest);

            if (reply.Books.Count != echoRequest.Books.Count)
            {
                throw new ApplicationException(@"Mismatch");
            }
        }

        // ==========================================
        // Call Service using GRPC (HTTP2)
        //  Note: we create a new GRPC Client
        // ==========================================
        [Benchmark]
        public async Task CallGrpcEndpointAsyncNewChannel()
        {
            var echoRequest = new EchoRequestDTO();
            for (int ctr = 1; ctr <= count; ctr++)
            {
                echoRequest.Books.Add(new BookDTO() { Title = $"The Goal [{ctr}]", Author = @"Eliyahu M. Goldratt" });
            };

            using var channel = GrpcChannel.ForAddress(GRPC_ENDPOINT);
            var client = new EchoTester.EchoTesterClient(channel);
            var reply = await client.EchoAsync(echoRequest);

            if (reply.Books.Count != echoRequest.Books.Count)
            {
                throw new ApplicationException(@"Mismatch");
            }
        }

        // ==========================================
        // Call Service using WebAPI (HTTP 1.x)
        //  Note: we reuse the HTTP Client
        // This is closer to how you would implement this in a ASP.NET application using IHttpClientFactory
        //      https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        // ==========================================
        [Benchmark]
        public async Task CallWebAPIEndpointReuseHTTPClient()
        {
            var echoRequest = new rest.EchoRequestDTO();
            echoRequest.Books = new List<BookDTO>();
            for (int ctr = 1; ctr <= count; ctr++)
            {
                echoRequest.Books.Add(new BookDTO() { Title = $"The Goal [{ctr}]", Author = @"Eliyahu M. Goldratt" });
            };

            var httpResponse = await _httpClient.PostAsJsonAsync("/api/EchoTest/echo", echoRequest);

            if (httpResponse.IsSuccessStatusCode)
            {
                var echoResponse = await httpResponse.Content.ReadFromJsonAsync<rest.EchoResponseDTO>();

                if (echoResponse.Books.Count != echoRequest.Books.Count)
                {
                    throw new ApplicationException();
                }
            }
            else
            {
                throw new ApplicationException();
            }
        }

        // ==========================================
        // Call Service using WebAPI (HTTP 1.x)
        //  Note: we create a new HTTP Client
        // ==========================================
        [Benchmark(Baseline = true)]
        public async Task CallWebAPIEndpointNewHTTPClient()
        {
            var echoRequest = new rest.EchoRequestDTO();
            echoRequest.Books = new List<BookDTO>();
            for (int ctr = 1; ctr <= count; ctr++)
            {
                echoRequest.Books.Add(new BookDTO() { Title = $"The Goal [{ctr}]", Author = @"Eliyahu M. Goldratt" });
            };

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(HTTP_ENDPOINT);
            var httpResponse = await httpClient.PostAsJsonAsync("/api/EchoTest/echo", echoRequest);

            if (httpResponse.IsSuccessStatusCode)
            {
                var echoResponse = await httpResponse.Content.ReadFromJsonAsync<rest.EchoResponseDTO>();

                if (echoResponse.Books.Count != echoRequest.Books.Count)
                {
                    throw new ApplicationException();
                }
            }
            else
            {
                throw new ApplicationException();
            }
        }

    }
}
