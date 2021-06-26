using System;
using System.Net.Http;
using System.Threading.Tasks;

using BenchmarkDotNet.Running;

namespace RestVsGRPCTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
