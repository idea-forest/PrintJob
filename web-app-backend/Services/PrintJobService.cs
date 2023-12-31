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

        public async Task UpdatePrintJob(int JobId, string Status, string Message)
        {
            Console.WriteLine(Message);
            PrintJob printJob = await _context.PrintJobs.FirstOrDefaultAsync(t => t.Id == JobId);
            if(printJob != null)
            {
                printJob.Status = Status;
                printJob.Message = Message;
            }
            _context.PrintJobs.Update(printJob);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePrintJob([FromBody] CreatePrintJobDTO job)
        {
            PrintJob newPrintJob = new PrintJob()
            {
                FilePath = "https://umanitoba.ca/faculties/graduate_studies/media/InteriorDesign_200609.doc",
                Color = job.Color,
                StartPage = job.StartPage,
                Status = "pending",
                EndPage = job.EndPage,
                Copies = job.Copies,
                TeamId = job.TeamId,
                DeviceId = job.DeviceId,
                PrinterName = job.PrinterName,
                PaperName = job.PaperName,
                UserId = job.UserId,
                LandScape = job.LandScape,
                CreatedAt = new DateTime()
            };
            _context.PrintJobs.Add(newPrintJob);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.ReceivePrintJobs(newPrintJob);
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string uploadedFilePath = await _iManageImage.UploadFile(file);
            return uploadedFilePath;
        }
    }
}
