using System.Net;
using System.Net.Http.Json;
using DeviceManagementApi.Models;
using DeviceManagementApi.Tests.Helpers;

namespace DeviceManagementApi.Tests
{
    public class DevicesControllerTests : IClassFixture<TestWebAppFactory>
    {
        private readonly HttpClient _client;

        public DevicesControllerTests(TestWebAppFactory factory)
        {
            _client = factory.CreateClient();
        }

        //GET ALL
        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfDevices()
        {
            var response = await _client.GetAsync("/api/Devices");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var devices = await response.Content.ReadFromJsonAsync<List<Device>>();
            Assert.NotNull(devices);
            Assert.True(devices.Count >= 2);
        }

        // GET BY ID
       [Fact]
public async Task GetById_ExistingId_ReturnsDevice()
{
    var response = await _client.GetAsync("/api/Devices/1");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var device = await response.Content.ReadFromJsonAsync<Device>();
    Assert.NotNull(device);
    Assert.Equal(1, device.Id); // verificăm ID-ul în loc de nume
}
        //GET BY ID notfound
        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Devices/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        //POST create new dev
        [Fact]
        public async Task Create_ValidDevice_ReturnsCreated()
        {
            var newDevice = new Device
            {
                Name = "Pixel 8",
                Manufacturer = "Google",
                Type = "Phone",
                OperatingSystem = "Android",
                OsVersion = "14",
                Processor = "Tensor G3",
                RamAmount = "8 GB",
                Description = "New test device",
                UserId = null
            };

            var response = await _client.PostAsJsonAsync("/api/Devices", newDevice);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<Device>();
            Assert.NotNull(created);
            Assert.Equal("Pixel 8", created.Name);
            Assert.True(created.Id > 0);
        }

        //PUT update device existent
        [Fact]
        public async Task Update_ExistingDevice_ReturnsNoContent()
        {
            var updatedDevice = new Device
            {
                Id = 1,
                Name = "Samsung S20 FE Updated",
                Manufacturer = "Samsung",
                Type = "Phone",
                OperatingSystem = "Android",
                OsVersion = "14",
                Processor = "Exynos 990",
                RamAmount = "6 GB",
                Description = "Updated",
                UserId = 1
            };

            var response = await _client.PutAsJsonAsync("/api/Devices/1", updatedDevice);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        //PUT ID mismatch
        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var updatedDevice = new Device
            {
                Id = 99,
                Name = "Wrong",
                Manufacturer = "X",
                Type = "Phone",
                OperatingSystem = "Android",
                OsVersion = "14",
                Processor = "X",
                RamAmount = "4 GB",
                Description = "X"
            };

            var response = await _client.PutAsJsonAsync("/api/Devices/1", updatedDevice);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // ✅ DELETE device existent
        [Fact]
        public async Task Delete_ExistingDevice_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/api/Devices/2");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        //DELETE in case of device inexistent
        [Fact]
        public async Task Delete_NonExistingDevice_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/Devices/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}