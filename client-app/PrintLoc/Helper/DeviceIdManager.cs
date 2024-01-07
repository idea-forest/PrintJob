using System;
using System.IO;

public class DeviceIdManager
{
    private const string FileName = "DDDDD-FFFF-JJJJ.txt";
    public static void SaveDeviceId(string deviceId)
    {
        try
        {
            string filePath = Path.Combine(Path.GetTempPath(), FileName);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(deviceId);
            }
            Console.WriteLine("Device ID saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving device ID: {ex.Message}");
        }
    }

    public static string GetDeviceId()
    {
        try
        {
            string filePath = Path.Combine(Path.GetTempPath(), FileName);
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string deviceId = reader.ReadLine();
                    Console.WriteLine("Device ID retrieved successfully!");
                    return deviceId;
                }
            }
            else
            {
                Console.WriteLine("Device ID file does not exist.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving device ID: {ex.Message}");
            return null;
        }
    }

    public static void DeleteFile()
    {
        try
        {
            string filePath = Path.Combine(Path.GetTempPath(), FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("File deleted successfully!");
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file: {ex.Message}");
        }
    }
}
