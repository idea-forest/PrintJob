namespace ProjectLoc.Models
{
    public class PrintJob
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public bool Color { get; set; }
        public int StartPage { get; set; }
        public int Page { get; set; }
        public int EndPage { get; set; }
        public int Copies { get; set; }
        public int TeamId { get; set; }
        public string DeviceId { get; set; }
        public string PrinterName { get; set; }
        public string? Type { get; set; }
        public string PaperName { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool LandScape { get; set; }
    }
}
