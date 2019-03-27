using System;
using System.IO;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace GDMainGUI
{
    public partial class ProgressBarGUI : Form
    {
        public static string ID = MainGUI.ID;
        public static string SongName = MainGUI.SongName;
        public static string Path = MainGUI.Path;
        public static int ModID = int.Parse(ID) - int.Parse(ID.Substring(ID.Length - 3));
        public static string URL = "http://audio.ngfiles.com/" + ModID + "/" + ID + "_" + SongName + ".mp3";
        //some URLs uses "https://audio-download.ngfiles.com" (since 848720?) but that doesn't seem to be a problem.
        public static double final;
        
        public ProgressBarGUI()
        {
            InitializeComponent();
        }

        WebClient client = new WebClient();
        private void ProgressBarGUI_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(async () =>
            {
                Uri uri = new Uri(URL);
                await client.DownloadFileTaskAsync(uri, Path);
            });
            thread.Start();

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }
        
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //this.Hide();
            if (e.Error == null)
            {
                MessageBox.Show("Audio downloaded successfully.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show((e.Error.Message.Contains("404")) ? "The audio wasn't found.\nBe sure to write the ID and name correctly (Upper/Lower case)." : e.Error.Message, "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                File.Delete(Path);
            }
            Environment.Exit(1);
        }

        DateTime StartedAt = DateTime.Now;

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
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
                if (total != 0)
                    final = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = receive / total * 100;
                labelPercent.Text = String.Format("{0:0.##} %", percentage);
                progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
                this.Text = "Downloading " + ID + ".mp3 (" + Math.Round(receive / 1048576, 2) + " MB / " + Math.Round(total / 1048576, 2) + " MB)";
            }));
        }
    }
}