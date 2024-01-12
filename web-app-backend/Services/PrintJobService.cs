using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProjectLoc.Services;
using ProjectLoc.Data;
using ProjectLoc.Models;
using ProjectLoc.Dtos.Printer;
using Microsoft.AspNetCore.Mvc;

namespace ProjectLoc.Services
{
    public class PrintJobService
    {
        private readonly ApiDbContext _context;
        private readonly IManageImage _iManageImage;
        private readonly IHubContext<PrintJobHub, PrintJobHubClient> _hubContext;

        public PrintJobService(IManageImage iManageImage, ApiDbContext dbContext, IHubContext<PrintJobHub, PrintJobHubClient> hubContext)
        {
            _context = dbContext;
            _hubContext = hubContext;
            _iManageImage = iManageImage;
        }

        public async Task<int> GetTeamIdByDeviceAsync(string DeviceId)
        {
            Device? device = await _context.Devices.FirstOrDefaultAsync(p => p.DeviceId == DeviceId);
            if (device == null)
            {
                return 0;
            }
            return device.TeamId;
        }

        public async Task UpdatePrintJob(int JobId, string Status, string Message, int PageNo, string Type)
        {
            PrintJob? printJob = await _context.PrintJobs.FirstOrDefaultAsync(t => t.Id == JobId);
            if(printJob != null)
            {
                printJob.Status = Status;
                printJob.Message = Message;
                printJob.Page = PageNo;
                printJob.Type = Type;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreatePrintJob([FromBody] CreatePrintJobDTO job)
        {
            int getTeamId = await GetTeamIdByDeviceAsync(job.DeviceId);
            if (getTeamId != 0)
            {
                PrintJob newPrintJob = new PrintJob()
                {
                    FilePath = job.FilePath,
                    Color = job.Color,
                    StartPage = job.StartPage,
                    Status = "pending",
                    EndPage = job.EndPage,
                    Copies = job.Copies,
                    TeamId = getTeamId,
                    DeviceId = job.DeviceId,
                    PrinterName = job.PrinterName,
                    PaperName = job.PaperName,
                    UserId = job.UserId,
                    LandScape = job.LandScape,
                    CreatedAt = DateTime.UtcNow
                };
                _context.PrintJobs.Add(newPrintJob);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.ReceivePrintJobs(newPrintJob);
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string uploadedFilePath = await _iManageImage.UploadFile(file);
            return uploadedFilePath;
        }
    }
}
