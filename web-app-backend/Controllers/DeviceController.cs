using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectLoc.Models;
using ProjectLoc.Data;
using ProjectLoc.Dtos.Printer;

namespace ProjectLoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public DeviceController(ApiDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(string id)
        {
            Device? device = await _context.Devices.FirstOrDefaultAsync(t => t.DeviceId == id);
            if (!device.Equals(null))
            {
                return Ok(device);
            }

            return NotFound();
        }

        [HttpPost("SyncDeviceByTeamName")]
        public async Task<IActionResult> SyncDeviceByTeamName(CreateDeviceDTO device)
        {
            if (!device.Equals(null))
            {
                Device? existDevice = await _context.Devices.FirstOrDefaultAsync(t => t.DeviceId == device.DeviceId);
                if (existDevice != null)
                {
                    return Ok(existDevice);
                }

                Device newDevice = new Device()
                {
                    DeviceId = device.DeviceId,
                    MachineName = device.MachineName,
                    TeamId = device.TeamId,
                    IpAddress = device.IpAddress,
                    Os = device.Os,
                    DeviceStatus = device.DeviceStatus
                };
                await _context.Devices.AddAsync(newDevice);
                await _context.SaveChangesAsync();

                return Ok(newDevice);
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }
    }
}
