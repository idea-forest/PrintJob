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
    public class PrinterController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PrinterController(ApiDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{DeviceId}")]
        public async Task<IActionResult> GetPrinterByDevice(string DeviceId)
        {
            List<Printer> printers = await _context.Printers.Where(p => p.DeviceId == DeviceId).ToListAsync();
            return Ok(printers);
        }

        public async Task<int> GetTeamIdByDevice(string DeviceId)
        {
            Device device = await _context.Devices.Where(p => p.DeviceId == DeviceId).FirstOrDefaultAsync();
            return device.TeamId;
        }

        [HttpPost("SyncPrinterByTeamName")]
        public async Task<IActionResult> SyncPrinterByTeamName(CreatePrinterDTO printer)
        {
            if (!printer.Equals(null))
            {
                Printer existingPrinter = await _context.Printers.FirstOrDefaultAsync(t => t.Name == printer.Name && t.DeviceId == printer.DeviceId);

                if (existingPrinter != null)
                {
                    throw new Exception("Printer with the specified name and device ID already exists.");
                }

                Printer newPrinter = new Printer()
                {
                    DeviceId = printer.DeviceId,
                    Name = printer.Name,
                    PrinterColor = printer.PrinterColor,
                    TeamId = await GetTeamIdByDevice(printer.DeviceId)
                };
                await _context.Printers.AddAsync(newPrinter);
                await _context.SaveChangesAsync();

                return Ok(newPrinter);
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }
    }
}
