using System;
using System.Diagnostics;

namespace PrintLoc.Helper
{
    class KillExcelProcess
    {
        public static void KillProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                        Console.WriteLine($"Process '{processName}' with ID {process.Id} has been terminated.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unable to kill process '{processName}': {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No process with the name '{processName}' found.");
            }
        }
    }
}
