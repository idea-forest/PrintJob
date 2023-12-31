using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using PdfiumViewer;
using Application = Microsoft.Office.Interop.Word.Application;
using PageSetup = Microsoft.Office.Interop.Word.PageSetup;
using Point = System.Drawing.Point;

namespace PrintLoc.Helper
{
    public class Print
    {
        public async Task<bool> PrintFileFromUrl(
            string fileUrl,
            string printerName,
            string paperName,
            bool isColor,
            int startPage,
            int endPage,
            bool landScape,
            int numberOfCopies,
            int JobId)
                {
                    bool printStatus = false;
                    string tempFilePath = await DownloadFileAsync(fileUrl);

                    if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                    {
                        try
                        {
                            string fileExtension = Path.GetExtension(tempFilePath).ToLower();

                            switch (fileExtension)
                            {
                                case ".jpg":
                                case ".jpeg":
                                case ".png":
                                case ".gif":
                                    printStatus = await PrintImage(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
                                    break;
                                case ".pdf":
                                    printStatus = await PrintPdf(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
                                    break;
                                case ".doc":
                                case ".docx":
                                    printStatus = await PrintWordDocument(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
                                    break;
                                case ".xls":
                                case ".xlsx":
                                case ".csv":
                                    printStatus = await PrintExcelDocument(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
                                    break;
                                default:
                                    Console.WriteLine("Unsupported file type for printing.");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error while identifying or printing the file: {ex.Message}");
                            printStatus = false;
                        }
                        finally
                        {
                            File.Delete(tempFilePath);
                        }
                    }
                    else
                    {
                        Console.WriteLine("File does not exist at the specified path.");
                    }

                    return printStatus;
                }


        public async Task<bool> PrintImage(
            string imagePath,
            string printerName,
            string paperName,
            bool isColor,
            int startPage,
            int endPage,
            bool landScape,
            int numberOfCopies,
            int JobId)
        {
            try
            {
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printerName,
                    Copies = (short)numberOfCopies,
                };
                var pageSettings = new PageSettings(printerSettings)
                {
                    Color = isColor,
                    Margins = new Margins(0, 0, 0, 0),
                    Landscape = landScape,
                };

                foreach (PaperSize papersize in printerSettings.PaperSizes)
                {
                    if (papersize.PaperName == paperName)
                    {
                        pageSettings.PaperSize = papersize;
                        break;
                    }
                }

                using (System.Drawing.Image imageToPrint = System.Drawing.Image.FromFile(imagePath))
                {
                    using (var printDocument = new PrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintPage += (s, e) =>
                        {
                            Point loc = new Point(100, 100);
                            e.Graphics.DrawImage(imageToPrint, loc);
                        };
                        printDocument.OriginAtMargins = true;
                        printDocument.PrinterSettings.FromPage = startPage;
                        printDocument.PrinterSettings.ToPage = endPage;
                        printDocument.Print();
                    }
  
                }
                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                return true;
            }
            catch(Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                return false;
            }
        }

        public async Task<bool> PrintPdf(
            string pdfPath,
            string printerName,
            string paperName,
            bool isColor,
            int startPage,
            int endPage,
            bool landScape,
            int numberOfCopies,
            int JobId)
        {
            try
            {
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printerName,
                    Copies = (short)numberOfCopies,
                };
                var pageSettings = new PageSettings(printerSettings)
                {
                    Color = isColor,
                    Margins = new Margins(0, 0, 0, 0),
                    Landscape = landScape,
                };
                foreach (PaperSize papersize in printerSettings.PaperSizes)
                {
                    if (papersize.PaperName == paperName)
                    {
                        pageSettings.PaperSize = papersize;
                        break;
                    }
                }

                using (var document = PdfDocument.Load(pdfPath))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.OriginAtMargins = true;
                        printDocument.PrinterSettings.FromPage = startPage;
                        printDocument.PrinterSettings.ToPage = endPage;
                        printDocument.Print();
                    }
                }
                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                return true;
            }
            catch (Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                return false;
            }
        }

        public WdPaperSize GetWordPaperSize(string paperName)
        {
            WdPaperSize result;
            switch (paperName.ToLower())
            {
                case "a3":
                    result = WdPaperSize.wdPaperA3;
                    break;
                case "a4":
                    result = WdPaperSize.wdPaperA4;
                    break;
                case "letter":
                    result = WdPaperSize.wdPaperLetter;
                    break;
                case "legal":
                    result = WdPaperSize.wdPaperLegal;
                    break;
                default:
                    result = WdPaperSize.wdPaperLetter;
                    break;
            }
            return result;
        }

        public WdOrientation setWordPageOrientation(bool orientation)
        {
            WdOrientation result;
            switch (orientation)
            {
                case true:
                    result = WdOrientation.wdOrientLandscape;
                    break;
                default:
                    result = WdOrientation.wdOrientPortrait;
                    break;
            }
            return result;
        }

        public XlPageOrientation setExcelPageOrientation(bool orientation)
        {
            XlPageOrientation result;
            switch (orientation)
            {
                case true:
                    result = XlPageOrientation.xlLandscape;
                    break;
                default:
                    result = XlPageOrientation.xlPortrait;
                    break;
            }
            return result;
        }

        public Microsoft.Office.Interop.Excel.XlPaperSize GetExcelPaperSize(string paperName)
        {
            Microsoft.Office.Interop.Excel.XlPaperSize result;
            switch (paperName.ToLower())
            {
                case "a3":
                    result = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA3;
                    break;
                case "a4":
                    result = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
                    break;
                case "letter":
                    result = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperLetter;
                    break;
                case "legal":
                    result = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperLegal;
                    break;
                default:
                    result = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperLetter;
                    break;
            }
            return result;
        }

        public async Task<bool> PrintWordDocument(
            string documentPath,
            string printerName,
            string paperName,
            bool isColor,
            int startPage,
            int endPage,
            bool landScape,
            int numberOfCopies,
            int JobId)
        {
            bool printStatus = false;

            try
            {
                Application wordApp = new Application();
                object missing = System.Reflection.Missing.Value;
                Document document = wordApp.Documents.Open(documentPath, ReadOnly: true, Visible: false);
                wordApp.ActivePrinter = printerName;
                wordApp.Options.PrintBackground = isColor;
                wordApp.Options.PrintBackgrounds = isColor;
                foreach (Section section in document.Sections)
                {
                    PageSetup pageSetup = section.PageSetup;
                    pageSetup.Orientation = setWordPageOrientation(landScape);
                    if (pageSetup.PaperSize != GetWordPaperSize(paperName))
                    {
                        try
                        {
                            pageSetup.PaperSize = GetWordPaperSize(paperName);
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine($"Paper size '{paperName}' not found. Using default paper size.");
                        }
                    }
                }
                object start = startPage;
                object end = endPage;
                object range = WdPrintOutRange.wdPrintRangeOfPages;

                document.PrintOut(
                    ref missing,
                    ref missing,
                    range,
                    isColor ? "Color" : "BlackAndWhite",
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing,
                    ref missing
                );

                object fromPage = startPage;
                object toPage = endPage;
                object ranges = WdPrintOutRange.wdPrintFromTo;
                document.PrintOut(Copies: numberOfCopies, Background: true, Range: ranges, From: fromPage, To: toPage);
                document.Close(SaveChanges: false);
                wordApp.Quit();
                printStatus = true;

                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message);
            }
            catch (Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                printStatus = false;
            }

            return printStatus;
        }

        public async Task<bool> PrintExcelDocument(
            string documentPath,
            string printerName,
            string paperName,
            bool isColor,
            int startPage,
            int endPage,
            bool landScape,
            int numberOfCopies,
            int JobId)
        {
            bool printStatus = false;
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;

            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                workbook = excelApp.Workbooks.Open(documentPath);
                excelApp.ActivePrinter = printerName;

                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in workbook.Worksheets)
                {
                    worksheet.PageSetup.Orientation = setExcelPageOrientation(landScape);
                    if (worksheet.PageSetup.PaperSize != GetExcelPaperSize(paperName))
                    {
                        try
                        {
                            worksheet.PageSetup.PaperSize = GetExcelPaperSize(paperName);
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine($"Paper size '{paperName}' not found. Using default paper size.");
                        }
                    }
                }

                object fromPage = startPage;
                object toPage = endPage;
                workbook.PrintOutEx(Copies: numberOfCopies, From: fromPage, To: toPage, ActivePrinter: printerName);
                workbook.Close(SaveChanges: false);
                excelApp.Quit();
                printStatus = true;

                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message);
            }
            catch (Exception ex)
            {
                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message);
                printStatus = false;
            }
            finally
            {
                ReleaseObject(workbook);
                ReleaseObject(excelApp);
            }

            return printStatus;
        }

        public static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine($"Exception occurred while releasing object: {ex.Message}");
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static string RemoveSpecialCharacters(string str)
        {
            string pattern = @"[^\w.%]+";
            string cleanedString = Regex.Replace(str, pattern, "_");
            return cleanedString;
        }

        private static string GenerateUniqueFileName(string fileUrl)
        {
            string fileName = Path.GetFileName(fileUrl);
            string cleanedFileName = RemoveSpecialCharacters(fileName);
            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{cleanedFileName}";
            return uniqueFileName;
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
                        string fileName = GenerateUniqueFileName(fileUrl);
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
