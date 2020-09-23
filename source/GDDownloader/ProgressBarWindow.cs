using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class ProgressBarWindow : Form
    {
        private readonly uint _id;
        private readonly string _url;
        private readonly string _savePath;
        private double _currentSize;
        private double _finalSize = 0;

        private readonly WebClient _webClient = new WebClient();
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public ProgressBarWindow(uint id, string audioName, string savePath)
        {
            _id = id;
            _savePath = savePath;
            _url = GenerateUrl(id, audioName);

            InitializeComponent();
        }

        private void ProgressBarWindow_Load(object sender, EventArgs e)
        {
            _webClient.DownloadProgressChanged += DownloadProgressChanged;
            _webClient.DownloadFileCompleted += DownloadFileCompleted;

            _stopWatch.Start();
            _webClient.DownloadFileAsync(new Uri(_url), _savePath);
        }

        private void ProgressBarWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_webClient.IsBusy)
            {
                _webClient.CancelAsync();
            }
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _stopWatch.Stop();
            _webClient.Dispose();

            this.Hide();
            if (e.Cancelled)  
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(_savePath))
                {
                    File.Delete(_savePath);
                }
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Download cancelled.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (File.Exists(_savePath))
                {
                    File.Delete(_savePath);
                }
            }
            else
            {
                MessageBox.Show("Audio downloaded successfully.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _currentSize = Math.Round((double)e.BytesReceived / 1000000, 2);

            if (_finalSize == 0)
            {
                _finalSize = Math.Round((double)e.TotalBytesToReceive / 1000000, 2);
            }

            labelDownloading.Text = $"Downloading ({Math.Round(e.BytesReceived / _stopWatch.Elapsed.TotalSeconds / 1000, 1)} KB/s)";
            progressBar.Value = e.ProgressPercentage;
            labelPercent.Text = e.ProgressPercentage + "%";

            this.Text = $"Downloading {_id}.mp3 ({_currentSize} MB / {_finalSize} MB)";

            // Align the label to the center.
            //labelDownloading.Left = (labelDownloading.Parent.Width - labelDownloading.Width - labelDownloading.Text.Length) / 2;
        }

        private string GenerateUrl(uint id, string audioName)
        {
            return $"{Constants.NgDownloadsBaseUrl}{id - (id % 1000)}/{id}_{audioName}.mp3";
        }
    }
}