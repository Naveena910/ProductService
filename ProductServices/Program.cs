using ProductServices;

namespace ProductServices
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHost => { webHost.UseStartup<Startup>(); }).ConfigureLogging(builder =>
            {
                builder.AddLog4Net("Log4Net.config");
                builder.SetMinimumLevel(LogLevel.Trace);
            });

    }
}