using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLoc.Data;

namespace ProjectLoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintJobController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PrintJobController(ApiDbContext dbContext)
        {
            _context = dbContext;
        }


    }
}
