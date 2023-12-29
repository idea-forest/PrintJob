namespace ProjectLoc.Dtos.Printer
{
    public class CreateDeviceDTO
    {
        public string DeviceId { get; set; }
        public string MachineName { get; set; }
        public int TeamId { get; set; }
        public string IpAddress { get; set; }
        public string Os { get; set; }
        public bool DeviceStatus { get; set; }
    }
}
