using DeviceManagementApi.Data;
using DeviceManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAll()
        {
            var devices = await _context.Devices
                .Include(d => d.User)
                .ToListAsync();

            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetById(int id)
        {
            var device = await _context.Devices
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
                return NotFound();

            return Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult<Device>> Create(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Device updatedDevice)
        {
            if (id != updatedDevice.Id)
                return BadRequest();

            var existingDevice = await _context.Devices.FindAsync(id);

            if (existingDevice == null)
                return NotFound();

            existingDevice.Name = updatedDevice.Name;
            existingDevice.Manufacturer = updatedDevice.Manufacturer;
            existingDevice.Type = updatedDevice.Type;
            existingDevice.OperatingSystem = updatedDevice.OperatingSystem;
            existingDevice.OsVersion = updatedDevice.OsVersion;
            existingDevice.Processor = updatedDevice.Processor;
            existingDevice.RamAmount = updatedDevice.RamAmount;
            existingDevice.Description = updatedDevice.Description;
            existingDevice.UserId = updatedDevice.UserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
                return NotFound();

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}