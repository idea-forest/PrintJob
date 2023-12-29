using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintLoc.Model
{
    public class PrintJobModel
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Color { get; set; }
        public string Page { get; set; }
        public string Copies { get; set; }
        public int TeamId { get; set; }
        public string DeviceId { get; set; }
        public string PrinterName { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
