using PrintLoc.Model;
using PrintLoc.Properties;
using PrintLoc.ViewModel;
using System.IO;
using System.Windows;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace PrintLoc.Helper
{
    class AccountManager
    {
        private static readonly HttpClient client = new HttpClient();

        private static string apiUrl = ApiBaseUrl.BaseUrl;

        private static string retrievedDeviceId = DeviceIdManager.GetDeviceId();

        private static readonly DeviceInformation deviceInformation = new DeviceInformation();

        public static async Task<PrintJobModel> updatePrintJob(int Id, string Status, string Message)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "api/PrintJob/UpdatePrintJob/" + Id + $"/{Status}/" + $"/{Message}");
                if (response.IsSuccessStatusCode)
                {
                    PrintJobModel printJob = await response.Content.ReadAsAsync<PrintJobModel>();
                    return printJob;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static async Task<Printer> getAllPrinters(string DeviceId, string Token)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                HttpResponseMessage response = await client.GetAsync(apiUrl + "api/Printer/GetPrinterByDevice/" + DeviceId);
                if (response.IsSuccessStatusCode)
                {
                    Printer printer = await response.Content.ReadAsAsync<Printer>();
                    return printer;
                }
                return null;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public static async Task<Printer> StoreDevicePrinter(string DeviceId, string Name, string PrinterColor)
        {
            try
            {
                var requestBody = new
                {
                    DeviceId = DeviceId,
                    Name = Name,
                    PrinterColor = PrinterColor,
                };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl + "api/Printer/SyncPrinterByTeamName", content);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Printer storedDevice = await response.Content.ReadAsAsync<Printer>();
                    return storedDevice;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<Device> StoreDevice(string token, string teamid)
        {
            try
            {
                var requestBody = new
                {
                    DeviceId = deviceInformation.GetUniqueDeviceIdentifier(),
                    MachineName = deviceInformation.GetMachineName(),
                    TeamId = teamid,
                    IpAddress = deviceInformation.GetLocalIPAddress(),
                    Os = deviceInformation.GetOperatingSystemInfo(),
                    DeviceStatus = true
                };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.PostAsync(apiUrl + "api/Device/SyncDeviceByTeamName", content);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Device storedDevice = await response.Content.ReadAsAsync<Device>();
                    if(retrievedDeviceId != null)
                    {
                        DeviceIdManager.SaveDeviceId(storedDevice.DeviceId);
                    }
                    return storedDevice;
                }
                return null;
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static async Task<AuthResult> LoginAccount(string teamname, string email, string password)
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestBody = new
                {
                    UserName = teamname,
                    Email = email,
                    Password = password
                };
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl + "api/auth/loginByClientApp", content);
                if (response.IsSuccessStatusCode)
                {
                    AuthResult user = await response.Content.ReadAsAsync<AuthResult>();
                    if (user != null && !string.IsNullOrEmpty(user.Token))
                    {
                        AuthResult.Instance.Token = user.Token;
                        AuthResult.Instance.RefreshToken = user.RefreshToken;
                        AuthResult.Instance.User = user.User;
                        AuthResult.Instance.Success = user.Success;

                        await StoreDevice(user.Token, user.User.TeamId);
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static bool EmailExists(string email)
        {
            var Text = File.ReadAllText(Resources.TextPath);
            return Text.Contains(string.Format(":{0} ", email));
        }

        public static void RegisterAccount(string email, string password)
        {
            using (StreamWriter file = File.AppendText(Resources.TextPath))
            {
                file.WriteLine(string.Format(":{0} {1} ", email, password));
            }

            MessageBox.Show("Your account has been successfully created!");
        }

        public static bool AccountExists(string email, string password)
        {
            return File.ReadAllText(Resources.TextPath).Contains(string.Format(":{0} {1} ", UserModel.Instance.Email, UserModel.Instance.Password));
        }

        public static void ChangePassword(Window window, string email, string newPassword)
        {
            var Text = File.ReadAllText(Resources.TextPath);
            int index1 = Text.IndexOf(string.Format(":{0} ", email));
            int index2 = Text.IndexOf('\n', index1);
            string oldCredentials = Text.Substring(index1, index2 - index1);
            string newCredentials = string.Format(":{0} {1} ", email, newPassword);
            Text = Text.Replace(oldCredentials, newCredentials);
            File.WriteAllText(Resources.TextPath, Text);
            MessageBox.Show("Password successfully changed");
            var loginViewModel = new LoginViewModel(window);
            WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
            if (loginViewModel.CloseAction == null)
            {
                loginViewModel.CloseAction = () => window.Close();
            }
        }
    }
}
