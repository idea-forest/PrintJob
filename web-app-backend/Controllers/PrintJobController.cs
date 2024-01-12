using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLoc.Data;
using ProjectLoc.Services;
using ProjectLoc.Dtos.Printer;

namespace ProjectLoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintJobController : ControllerBase
    {
        private readonly PrintJobService _printJobService;

        public PrintJobController([FromBody] PrintJobService printJobService)
        {
            _printJobService = printJobService;
        }

        [HttpPost("CreatePrintJob")]
        public async Task<IActionResult> CreatePrintJob(CreatePrintJobDTO job)
        {
            await _printJobService.CreatePrintJob(job);
            return Ok("Print Job Sent Succesfully");
        }

        [HttpGet("UpdatePrintJob/{JobId}/{Status}/{Message}/{PageNo}/{Type}")]
        public async Task<IActionResult> UpdatePrintJob(int JobId, string Status, string Message, int PageNo, string Type)
        {
            await _printJobService.UpdatePrintJob(JobId, Status, Message, PageNo, Type);
            return Ok("Print Job Updated Successfully");
        }
    }
}
