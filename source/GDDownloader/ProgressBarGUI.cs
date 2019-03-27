using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class ProgressBarGUI : Form
    {
        public static string ID = MainGUI.ID;
        public static double ModID = double.Parse(ID) - double.Parse(ID.Substring(ID.Length - 3));
        public static string Path = MainGUI.Path;
        public static string URL = "http://audio.ngfiles.com/" + ModID + "/" + ID + "_" + MainGUI.SongName + ".mp3";
        //some URLs also uses "https://audio-download.ngfiles.com" but that doesn't seem to be a problem.

        private WebClient client = new WebClient();
        private DateTime StartedAt = DateTime.Now;

        public ProgressBarGUI()
        {
            InitializeComponent();
        }

        private void ProgressBarGUI_Load(object sender, EventArgs e)
        {
            Uri uri = new Uri(URL);
            client.DownloadFileAsync(uri, Path);

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Invoke((MethodInvoker)delegate{this.Hide();}); //Hide the form
            if (e.Error == null)
            {
                MessageBox.Show("Audio downloaded successfully.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show((e.Error.Message.Contains("404")) ? "The audio wasn't found.\nBe sure to write the ID and name correctly (Upper/Lower case)." : e.Error.Message, "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Error); //, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly
                File.Delete(Path);
            }
            Environment.Exit(1);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (StartedAt == default(DateTime))
                {
                StartedAt = DateTime.Now;
                }
                else
                {
                    var timeSpan = DateTime.Now - StartedAt;
                    if (timeSpan.TotalSeconds > 1)
                    {
                        double bytesPerSecond = e.BytesReceived / (long)timeSpan.TotalSeconds;
                        labelDownloading.Text = "Downloading (" + Math.Round(bytesPerSecond / 1024, 1) + " KB/s)";
                    }
                }
                progressBar.Minimum = 0;
                double receive = double.Parse(e.BytesReceived.ToString());
                double total = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = receive / total * 100;
                labelPercent.Text = String.Format("{0:0.##} %", percentage);
                progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
                this.Text = "Downloading " + ID + ".mp3 (" + Math.Round(receive / 1048576, 2) + " MB / " + Math.Round(total / 1048576, 2) + " MB)";
            });
        }
    }
}