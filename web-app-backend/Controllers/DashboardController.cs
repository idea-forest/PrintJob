using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjectLoc.Models;
using ProjectLoc.Data;
using ProjectLoc.Dtos.Printer;
using ProjectLoc.Services;
using Microsoft.AspNetCore.SignalR;

namespace ProjectLoc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {
        private readonly ApiDbContext _context;

        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardController(ApiDbContext dbContext, IHubContext<DashboardHub> hubContext)
        {
            _context = dbContext;
            _hubContext = hubContext;
        }

        [HttpGet("GetDashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            string? sub = HttpContext.GetSubFromHttpContext();
            ApplicationUser user = GetUserInformation(sub);
            Dashboard dashbaordData = new Dashboard()
            {
                TotalActiveDevice = getActiveDeviceTotal(user.TeamId),
                TotalDevice = getDeviceTotal(user.TeamId),
                TotalPrinter = getTotalPrinter(user.TeamId),
                PrintJob = GetAllRecentPrintJobs(user.TeamId)
            };
            return Ok(dashbaordData);
        }

        public int getDeviceTotal(int teamId)
        {
            int deviceCount = _context.Devices
                .Where(d => d.TeamId == teamId)
                .Count();
            return deviceCount;
        }

        public int getActiveDeviceTotal(int teamId)
        {
            int deviceCount = _context.Devices
                .Where(d => d.TeamId == teamId)
                .Where(d => d.DeviceStatus == true)
                .Count();
            return deviceCount;
        }

        public List<PrintJob> GetAllRecentPrintJobs(int teamId)
        {
            List<PrintJob> recentPrintJobs = _context.PrintJobs
                .Where(p => p.TeamId == teamId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return recentPrintJobs;
        }

        public int getTotalPrinter(int teamId)
        {
            int totalPrinter = _context.Printers
                .Where(d => d.TeamId == teamId)
                .Count();
            return totalPrinter;
        }

        public ApplicationUser GetUserInformation(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }
    }
}
