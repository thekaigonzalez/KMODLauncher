using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows;

namespace KMODLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] KMODInstallations()
        {
            string[] KMODInstallations = { };
            if (Directory.Exists("C:/KMOD/Launcher") && Directory.Exists(@"C:\KMOD\Launcher\Installations"))
            {
                //await Task.Delay(300);
                // this.LOG.Content = "Found KMOD Launcher installation files, mounting installations...";
                //await Task.Delay(1000);
                foreach (string entry in Directory.GetDirectories(@"C:\KMOD\Launcher\Installations\", "*", SearchOption.TopDirectoryOnly))
                {

                    var dir = new DirectoryInfo(entry);
                    var dirName = dir.Name;
                    
                    KMODInstallations = KMODInstallations.Append(dirName).ToArray();
                }

                
                
            }

            
            return KMODInstallations;
        }
        public async void addKMODInstallations()
        {
            this.LOG.Visibility = Visibility.Visible;
            this.LOG.Content = "checking KMOD installations...";
            await Task.Delay(678);
            this.LOG.Content = "scanning KMOD directory...";
            await Task.Delay(567);
            if (Directory.Exists("C:/KMOD"))
            {
                this.LOG.Content = "looking for KMODLauncher files...";
                if (Directory.Exists("C:/KMOD/Launcher") && Directory.Exists(@"C:\KMOD\Launcher\Installations"))
                {
                    await Task.Delay(300);
                    this.LOG.Content = "Found KMOD Launcher installation files, mounting installations...";
                    await Task.Delay(1000);
                    foreach (string entry in Directory.GetDirectories(@"C:\KMOD\Launcher\Installations", "*", SearchOption.TopDirectoryOnly))
                    {

                        var dir = new DirectoryInfo(entry);
                        var dirName = dir.Name;
                        if (File.Exists(@"C:\KMOD\Launcher\Installations\" + dirName + @"\kmod.exe"))
                        {
                            this.LOG.Content = dirName;
                            this.Installation.Items.Add(dirName);
                        }

                        await Task.Delay(702);
                    }
                    await Task.Delay(947);
                    this.LOG.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.LOG.Content = "Missing files for KMODLauncher! attempting to fix...";
                    if (!Directory.Exists("C:/KMOD/Launcher")) Directory.CreateDirectory(@"C:\KMOD\Launcher");
                    if (!Directory.Exists("C:/KMOD/Launcher/Installations")) Directory.CreateDirectory("C:/KMOD/Launcher/Installations");
                    await Task.Delay(700);
                    this.LOG.Content = "repaired installation. Please reload!";
                    await Task.Delay(2000);
                    this.LOG.Visibility = Visibility.Hidden;
                }
            }
            
        }
        public MainWindow()
        {
            InitializeComponent();
            addKMODInstallations();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new WebClient();

            string m = client.DownloadString("https://thekaigonzalez.github.io/kmoddevs/kmod/RECENT.txt");

            string[] versions = KMODInstallations();

            bool HasVersion = false;

            if (Directory.Exists(@"C:\KMOD\Launcher\Installations\kmod" + m))
            {
                HasVersion = true;
            }
            if (HasVersion)
            {
                if (File.Exists(@"C:\KMOD\Launcher\Installations\kmod" + m + @"\kmod.exe") && File.Exists(@"C:\KMOD\Launcher\Installations\kmod" + m + @"\kmod.exe")) // do you have the binary?
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = "C:/KMOD/Launcher/Installations/kmod" + m + "/kmod.exe";
                    startInfo.Arguments = "";
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
            }
            //this.main.Content = m.Content;
        }

        protected bool CheckUrlStatus(string Website)
        {
            try
            {
                var request = WebRequest.Create(Website) as HttpWebRequest;
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string m = this.Installation.Text;
            if (Directory.Exists(@"C:\KMOD\Launcher\Installations\" + m) && File.Exists(@"C:\KMOD\Launcher\Installations\" + m + @"\kmod.exe"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.FileName = "C:/KMOD/Launcher/Installations/" + m + "/kmod.exe";
                startInfo.Arguments = "";
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(@"C:/KMOD/Launcher/Installations"))
            {
                WebClient webClient = new WebClient();
                string v = this.textbot.Text;
                if (CheckUrlStatus("https://thekaigonzalez.github.io/kmoddevs/kmod/kmod" + v + ".zip"))
                {
                    webClient.DownloadFile("https://thekaigonzalez.github.io/kmoddevs/kmod/kmod" + v + ".zip", "C:/Users/" + Environment.UserName + "/" + v + ".zip");

                    ZipFile.ExtractToDirectory("C:/Users/" + Environment.UserName + "/" + v + ".zip", "C:/KMOD/Launcher/Installations/kmod" + v);

                }
                
            } else
            {
                if (!Directory.Exists("C:/KMOD/Launcher")) Directory.CreateDirectory(@"C:\KMOD\Launcher");
                if (!Directory.Exists("C:/KMOD/Launcher/Installations")) Directory.CreateDirectory("C:/KMOD/Launcher/Installations");
            }
        }
    }
}
