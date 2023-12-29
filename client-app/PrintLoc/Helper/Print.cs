using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PrintLoc.Helper
{
    public class Print
    {
        public async Task<bool> PrintFileFromUrl(
            string fileUrl,
            string printerName,
            bool isColor,
            int startPage,
            int endPage,
            int numberOfCopies)
        {
            bool printStatus = false;
            string tempFilePath = await DownloadFileAsync(fileUrl);
            if (!string.IsNullOrEmpty(tempFilePath))
            {
                try
                {
                    PrinterSettings printerSettings = new PrinterSettings();
                    printerSettings.PrinterName = printerName;
                    printerSettings.PrintFileName = tempFilePath;
                    printerSettings.Copies = (short)numberOfCopies;
                    printerSettings.DefaultPageSettings.Color = isColor;
                    if (startPage > 0 && endPage > 0 && startPage <= endPage)
                    {
                        printerSettings.FromPage = startPage;
                        printerSettings.ToPage = endPage;
                    }
                    PrintDocument printDoc = new PrintDocument();
                    printDoc.PrinterSettings = printerSettings;
                    printDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", 210, 290);
                    printDoc.PrinterSettings.DefaultPageSettings.Landscape = false;
                    printDoc.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
                    printDoc.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
                    printDoc.Print();
                    printStatus = true;

                    Console.WriteLine($"temp path {tempFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error printing file: {ex.Message}");
                    printStatus = false;
                }
                finally
                {
                    //File.Delete(tempFilePath);
                }
            }
            return printStatus;
        }

        private string DownloadFile(string fileUrl)
        {
            try
            {
                WebClient webClient = new WebClient();
                string tempFilePath = Path.GetTempFileName();
                webClient.DownloadFile(fileUrl, tempFilePath);
                Console.WriteLine($"temp url {tempFilePath}");
                return tempFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                return null;
            }
        }

        private static async Task<string> DownloadFileAsync(string fileUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(fileUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string fileName = Path.GetFileName(fileUrl);
                        string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

                        using (FileStream fileStream = File.Create(tempFilePath))
                        {
                            await response.Content.CopyToAsync(fileStream);
                            return tempFilePath;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: Failed to download the file. Status code: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                return null;
            }
        }
    }
}
