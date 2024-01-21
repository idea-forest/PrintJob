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
using OfficeOpenXml;
using PdfiumViewer;
using PrintLoc.View;
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
                    if (!string.IsNullOrEmpty(tempFilePath))
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
                                    printStatus = await PrintImage(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId, fileExtension);
                                    break;
                                case ".pdf":
                                    Console.WriteLine("started printing");
                                    printStatus = await PrintPdf(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId, fileExtension);
                                    break;
                                case ".doc":
                                case ".docx":
                                    printStatus = await PrintWordDocument(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId, fileExtension);
                                    break;
                                case ".xls":
                                case ".xlsx":
                                    printStatus = await PrintExcelDocument(tempFilePath, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId, fileExtension);
                                    break;
                                default:
                                    Console.WriteLine("Unsupported file type for printing.");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                PrintFailureWindow printFailureWindow = new PrintFailureWindow();
                                printFailureWindow.ShowDialog();
                            });
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
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            PrintFailureWindow printFailureWindow = new PrintFailureWindow();
                            printFailureWindow.Show();
                        });
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
            int JobId,
            string fileExtension)
        {
            int counter = 0;
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
                        printDocument.OriginAtMargins = true;
                        printDocument.PrinterSettings.FromPage = startPage;
                        printDocument.PrinterSettings.ToPage = endPage;
                        printDocument.PrintPage += (s, e) =>
                        {
                            Point loc = new Point(100, 100);
                            e.Graphics.DrawImage(imageToPrint, loc);
                            counter++;
                        };
                        printDocument.Print();
                    }
  
                }

                Console.WriteLine("Document printed"+ counter);
                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    PrintSuccessWindow printSuccessWindow = new PrintSuccessWindow();
                    printSuccessWindow.ShowDialog();
                });
                return true;
            }
            catch(Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    PrintFailureWindow printFailureWindow = new PrintFailureWindow();
                    printFailureWindow.ShowDialog();
                });
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
            int JobId,
            string fileExtension)
        {
            int counter = 0;
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
                        printDocument.PrintPage += (sender, e) => counter++;
                        printDocument.Print();
                    }
                }

                Console.WriteLine("Document printed" + counter);
                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    //ProcessingWindow processingWindow = new ProcessingWindow();
                    //processingWindow.Close();

                    //PrintSuccessWindow printSuccessWindow = new PrintSuccessWindow();
                    //printSuccessWindow.ShowDialog();
                });
                return true;
            }
            catch (Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    PrintFailureWindow printFailureWindow = new PrintFailureWindow();
                    printFailureWindow.ShowDialog();
                });
                return false;
            }
        }

        //public PaperSizeType GetPaperType(string paperName)
        //{
        //    PaperSizeType result;
        //    switch (paperName.ToLower())
        //    {
        //        case "a3":
        //            result = PaperSizeType.PaperA3;
        //            break;
        //        case "a4":
        //            result = PaperSizeType.PaperA4;
        //            break;
        //        case "letter":
        //            result = PaperSizeType.PaperLetter;
        //            break;
        //        case "legal":
        //            result = PaperSizeType.PaperLegal;
        //            break;
        //        default:
        //            result = PaperSizeType.PaperLetter;
        //            break;
        //    }
        //    return result;
        //}

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

        //public async Task<bool> PrintWordDocument(
        //   string documentPath,
        //   string printerName,
        //   string paperName,
        //   bool isColor,
        //   int startPage,
        //   int endPage,
        //   bool landscape,
        //   int numberOfCopies,
        //   int jobId,
        //   string fileExtension)
        //{
        //    bool printStatus = false;
        //    int counter = 0;
        //    Application wordApp = null;
        //    Document document = null;

        //    try
        //    {
        //        wordApp = new Application();
        //        object missing = System.Reflection.Missing.Value;
        //        document = wordApp.Documents.Open(documentPath, ReadOnly: true, Visible: false);
        //        document.Activate();
        //        wordApp.ActivePrinter = printerName;
        //        wordApp.Options.PrintBackground = isColor;
        //        wordApp.Options.PrintBackgrounds = isColor;

        //        counter = document.ComputeStatistics(WdStatistic.wdStatisticPages);

        //        foreach (Section section in document.Sections)
        //        {
        //            PageSetup pageSetup = section.PageSetup;
        //            pageSetup.Orientation = landscape ? WdOrientation.wdOrientLandscape : WdOrientation.wdOrientPortrait;

        //            if (pageSetup.PaperSize != GetWordPaperSize(paperName))
        //            {
        //                try
        //                {
        //                    pageSetup.PaperSize = GetWordPaperSize(paperName);
        //                }
        //                catch (ArgumentException)
        //                {
        //                    Console.WriteLine($"Paper size '{paperName}' not found. Using default paper size.");
        //                }
        //            }
        //        }

        //        object range = WdPrintOutRange.wdPrintRangeOfPages;
        //        object fromPage = startPage;
        //        object toPage = endPage;


        //        object ranges = WdPrintOutRange.wdPrintFromTo;
        //        document.PrintOut(
        //            Copies: numberOfCopies,
        //            Background: true,
        //            Range: ranges,
        //            From: fromPage,
        //            To: toPage);

        //        document.Close(SaveChanges: false);
        //        wordApp.Quit();
        //        Marshal.ReleaseComObject(document);
        //        Marshal.ReleaseComObject(wordApp);
        //        printStatus = true;

        //        string status = "printed";
        //        string message = "Document printed successfully";
        //        await AccountManager.updatePrintJob(jobId, status, message, counter, fileExtension);
        //    }
        //    catch (Exception ex)
        //    {
        //        string status = "failed";
        //        string message = $"Error printing your document: {ex.Message}";
        //        await AccountManager.updatePrintJob(jobId, status, message, counter, fileExtension);
        //        printStatus = false;

        //        if (document != null)
        //        {
        //            document.Close(SaveChanges: false);
        //            Marshal.ReleaseComObject(document);
        //        }

        //        if (wordApp != null)
        //        {
        //            wordApp.Quit();
        //            Marshal.ReleaseComObject(wordApp);
        //        }
        //    }

        //    return printStatus;
        //}

        public async Task<bool> PrintWordDocument(
                    string documentPath,
                    string printerName,
                    string paperName,
                    bool isColor,
                    int startPage,
                    int endPage,
                    bool landScape,
                    int numberOfCopies,
                    int JobId,
                    string fileExtension)
        {
            bool printStatus = false;
            int counter = 0;
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

                object fromPage = startPage;
                object toPage = endPage;
                object ranges = WdPrintOutRange.wdPrintFromTo;
                document.PrintOut(Copies: numberOfCopies, Background: true, Range: ranges, From: fromPage, To: toPage);
                document.Close(SaveChanges: false);
                wordApp.Quit();
                printStatus = true;

                String Status = "printed";
                String Message = "Document printed succesfully";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
            }
            catch (Exception ex)
            {

                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
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
            int JobId,
            string fileExtension)
        {
            bool printStatus = false;
            int counter = 0;
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = excelApp.Workbooks.Open(documentPath);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Item[1];

                excelApp.ActivePrinter = printerName;
                workbook.Activate();

                foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in workbook.Sheets)
                {
                    sheet.PageSetup.Orientation = landScape ? Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape : Microsoft.Office.Interop.Excel.XlPageOrientation.xlPortrait;
                    if (sheet.PageSetup.PaperSize != XlPaperSize.xlPaperLetter)
                    {
                        try
                        {
                            sheet.PageSetup.PaperSize = GetExcelPaperSize(paperName);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Paper size '{paperName}' not found. Using default paper size.");
                        }
                    }
                }

                object fromPage = startPage;
                object toPage = endPage;
                object copies = numberOfCopies;
                object pages = $"{startPage}-{endPage}";

                worksheet.PrintOut(
                    From: fromPage,
                    To: toPage,
                    Copies: copies,
                    Preview: false,
                    ActivePrinter: printerName,
                    PrintToFile: false,
                    Collate: true);

                workbook.Close(SaveChanges: false);
                excelApp.Quit();
                printStatus = true;

                String Status = "printed";
                String Message = "Document printed successfully";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
            }
            catch (Exception ex)
            {
                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(JobId, Status, Message, counter, fileExtension);
                printStatus = false;
            }
            finally
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    Marshal.ReleaseComObject(excelApp);
                }
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
