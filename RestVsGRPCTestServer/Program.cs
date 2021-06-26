using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace RestVsGRPCTestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        // Some unique configuration here to expose HTTP1.x and HTTP2.x endpoints and to have both use SSL
                        // all REST calls will go through port 5000 but gRPC will use port 5001:
                        serverOptions.ListenLocalhost(5000, o => 
                                                        { 
                                                            o.Protocols = HttpProtocols.Http1;      // HTTP 1.x (REST) on Port 5000 over TLS
                                                            o.UseHttps();
                                                        });
                        serverOptions.ListenLocalhost(5001, o =>
                                                        {
                                                            o.Protocols = HttpProtocols.Http2;      // HTTP 2.x (gRPC) on port 5001 over TLS
                                                            o.UseHttps();
                                                        });  
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
