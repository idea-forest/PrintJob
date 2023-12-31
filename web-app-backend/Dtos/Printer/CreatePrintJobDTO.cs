using System.ComponentModel.DataAnnotations;

namespace ProjectLoc.Dtos.Printer
{
    public class CreatePrintJobDTO
    {
        //public IFormFile File { get; set; }

        [Required]
        public bool Color { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        [Required]
        public int Copies { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string PrinterName { get; set; }

        [Required]
        public string PaperName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public bool LandScape { get; set; }
    }
}
