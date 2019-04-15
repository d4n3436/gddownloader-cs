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
        public static double ID = MainGUI.ID;
        public static double ModID = ID - (ID % 1000);
        public static string Path = MainGUI.Path;
        public static string URL = "https://audio-download.ngfiles.com/" + ModID + "/" + ID + "_" + MainGUI.SongName + ".mp3";
        //Some Newgrounds audio downloads uses "http://audio.ngfiles.com" but that doesn't seem to be a problem.

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

            WebClient.DownloadProgressChanged += Client_DownloadProgressChanged;
            WebClient.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                Stopwatch.Stop(); //Stop the stopwatch
                this.Hide(); //Hide the form
            });
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
            Application.Exit();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => Invoke((MethodInvoker)delegate
        {
            double CurrentSize = Math.Round(double.Parse(e.BytesReceived.ToString()) / 1000000, 2);
            double FinalSize = Math.Round(double.Parse(e.TotalBytesToReceive.ToString()) / 1000000, 2);

            labelDownloading.Text = "Downloading (" + Math.Round(e.BytesReceived / Stopwatch.Elapsed.TotalSeconds / 1000, 1) + " KB/s)";
            progressBar.Value = e.ProgressPercentage;
            labelPercent.Text = e.ProgressPercentage + "%";

            this.Text = "Downloading " + ID + ".mp3 (" + CurrentSize + " MB / " + FinalSize + " MB)";

            //Align the label to the center.
            labelDownloading.Left = (labelDownloading.Parent.Width - labelDownloading.Width - labelDownloading.Text.Length) / 2;
        });

        private void ProgressBarGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            WebClient.CancelAsync();
            Application.Exit();
        }
    }
}