using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class ProgressBarGUI : Form
    {
        private static int ID = MainGUI.ID;
        private static string Path = MainGUI.Path;
        private static string URL = "https://audio-download.ngfiles.com/" + (ID - (ID % 1000)) + "/" + ID + "_" + MainGUI.AudioName + ".mp3";
        //Some Newgrounds audio downloads uses "http://audio.ngfiles.com" but that doesn't seem to be a problem.

        private double CurrentSize;
        private double FinalSize;

        private WebClient WebClient = new WebClient();
        private Stopwatch Stopwatch = new Stopwatch();

        public ProgressBarGUI()
        {
            InitializeComponent();
        }

        private void ProgressBarGUI_Load(object sender, EventArgs e)
        {
            Stopwatch.Start();
            WebClient.DownloadFileAsync(new Uri(URL), Path);

            WebClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Stopwatch.Stop();
            WebClient.Dispose();
            this.Hide();
            if (e.Error == null)
            {
                MessageBox.Show("Audio downloaded successfully.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(e.Error.Message.Contains("404") ? "The audio wasn't found.\nBe sure to write the ID and name correctly (Upper/Lower case)." : e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly
                if (File.Exists(Path))
                    File.Delete(Path);
            }
            this.Close();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //CurrentSize = Math.Round(double.Parse(e.BytesReceived.ToString()) / 1000000, 2);
            CurrentSize = Math.Round(Convert.ToDouble(e.BytesReceived) / 1000000, 2);
            //FinalSize = Math.Round(double.Parse(e.TotalBytesToReceive.ToString()) / 1000000, 2);
            FinalSize = Math.Round(Convert.ToDouble(e.TotalBytesToReceive) / 1000000, 2);

            labelDownloading.Text = "Downloading (" + Math.Round(e.BytesReceived / Stopwatch.Elapsed.TotalSeconds / 1000, 1) + " KB/s)";
            progressBar.Value = e.ProgressPercentage;
            labelPercent.Text = e.ProgressPercentage + "%";

            this.Text = "Downloading " + ID + ".mp3 (" + CurrentSize + " MB / " + FinalSize + " MB)";

            //Align the label to the center.
            //labelDownloading.Left = (labelDownloading.Parent.Width - labelDownloading.Width - labelDownloading.Text.Length) / 2;
        }

        private void ProgressBarGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WebClient.IsBusy)
                WebClient.CancelAsync();
        }
    }
}