using FlyingDutchmanAirlines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace FlyingDutchManAirlines
{
    class Program
    {
        static void Main(String[] args)
        {
            InitializeHost();
        }
        private static void InitializeHost() =>
            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
                builder.UseUrls("http://0.0.0.0:8080");
            }).Build().Run();
        
    }
}