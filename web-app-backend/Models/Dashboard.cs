namespace ProjectLoc.Models
{
    public class Dashboard
    {
        public int TotalDevice { get; set; }
        public int TotalActiveDevice { get; set; }
        public int TotalPrinter { get; set; }
        public List<PrintJob> PrintJob { get; set; }
    }
}
