using DeviceManagementApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DeviceManagementApi.Controllers
{
    [ApiController]
    [Route("api/devices")]
    [Authorize]
    public class AssignController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("{id}/assign")]
        public async Task<IActionResult> Assign(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var appUserId = int.Parse(userIdClaim);

            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound("Device not found");

            if (device.UserId != null)
                return BadRequest("Device is already assigned to another user");

            var appUser = await _context.AppUsers.FindAsync(appUserId);
            if (appUser == null) return NotFound("AppUser not found");

            if (appUser.LinkedUserId == null)
                return BadRequest("No user profile linked to this account");

            device.UserId = appUser.LinkedUserId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Device assigned successfully" });
        }

        [HttpPost("{id}/unassign")]
        public async Task<IActionResult> Unassign(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var appUserId = int.Parse(userIdClaim);

            var device = await _context.Devices
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device == null) return NotFound("Device not found");

            var appUser = await _context.AppUsers.FindAsync(appUserId);
            if (appUser == null) return NotFound("AppUser not found");

            if (device.UserId != appUser.LinkedUserId)
                return BadRequest("You can only unassign devices assigned to you");

            device.UserId = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Device unassigned successfully" });
        }
    }
}