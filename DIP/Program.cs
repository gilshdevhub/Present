using DIP.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DIP
{
    class Program
    {
        private static IConfiguration _configuration;
        private static ServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            SetConfiguration();
            RegisterServices();

            IServiceScope scope = _serviceProvider.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ConsoleApplication>().RunAsync();
            
            DisposeServices();
        }

        private static void SetConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }
        private static void RegisterServices()
        {
            _serviceProvider = new ServiceCollection()
                .AddDbContext<RailDbContext>(o => o.UseSqlServer(_configuration.GetConnectionString("vouchers")))
                .AddSingleton<IVouchersService, VouchersService>()
                .AddSingleton<ConsoleApplication>()
                .AddSingleton<IConfiguration>(_configuration)
                .BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }        
    }
}
