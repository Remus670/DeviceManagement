using DeviceManagementApi.Data;
using DeviceManagementApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagementApi.Tests.Helpers
{
    public class TestWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedTestData(db);
            });
        }

        private static void SeedTestData(ApplicationDbContext db)
        {
            if (db.Users.Any()) return;

            db.Users.Add(new User
            {
                Id = 1,
                Name = "Test User",
                Role = "Developer",
                Location = "Cluj"
            });

            db.Devices.AddRange(
                new Device
                {
                    Id = 1,
                    Name = "Samsung S20 FE",
                    Manufacturer = "Samsung",
                    Type = "Phone",
                    OperatingSystem = "Android",
                    OsVersion = "13",
                    Processor = "Exynos 990",
                    RamAmount = "6 GB",
                    Description = "Test device",
                    UserId = 1
                },
                new Device
                {
                    Id = 2,
                    Name = "iPhone 17",
                    Manufacturer = "Apple",
                    Type = "Phone",
                    OperatingSystem = "iOS",
                    OsVersion = "18",
                    Processor = "A19 Bionic",
                    RamAmount = "8 GB",
                    Description = "Test device 2",
                    UserId = null
                }
            );

            db.SaveChanges();
        }
    }
}