using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            comboBoxSavePath.SelectedIndex = 0;
        }

        private void ButtonDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAudioName.Text))
            {
                return;
            }
            if (!uint.TryParse(textBoxID.Text, out uint id))
            {
                MessageBox.Show("Invalid ID.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (id < Constants.MinimunId)
            {
                MessageBox.Show($"No support for ID's minor than {Constants.MinimunId}\nThe known name pattern can't be used.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string savePath;
            if (comboBoxSavePath.SelectedIndex != 0) //Alternate save path or other.
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (comboBoxSavePath.SelectedIndex == 1)
                    {
                        fbd.ShowNewFolderButton = false;
                        fbd.Description = "Select the Geometry Dash folder";
                    }
                    else
                    {
                        fbd.Description = "Select the save folder";
                    }
                    if (fbd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    savePath = fbd.SelectedPath;
                }
                if (!Directory.Exists(savePath)) //(string.IsNullOrWhiteSpace(Path))
                {
                    MessageBox.Show("The selected folder is invalid or inaccessible.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (comboBoxSavePath.SelectedIndex == 1)
                {
                    savePath = Path.Combine(savePath, "Resources");
                    if (!Directory.Exists(savePath)) // Check if Resources folder doesn't exist.
                    {
                        MessageBox.Show("\"Resources\" folder, the alternate save folder, wasn't found.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (!Directory.Exists(Constants.DefaultSavePath)) // If default save path doesn't exists.
                {
                    MessageBox.Show($"The default save path:\n{Constants.DefaultSavePath}\nwasn't found. Please select other save path.", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                savePath = Constants.DefaultSavePath;
            }

            savePath = Path.Combine(savePath, $"{id}.mp3"); // Add the ID and audio extension in any save path case.

            // If the audio file exists, ask if overwrite.
            if (File.Exists(savePath) && MessageBox.Show($"There's already an audio named \"{id}.mp3\". Overwrite?", Constants.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            this.Hide(); // Hide the main window
            var pbWindow = new ProgressBarWindow(id, SanitizeAudioName(textBoxAudioName.Text), savePath);
            pbWindow.ShowDialog(); // Show the progress bar window
            pbWindow.Dispose();
            this.Show();
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"You can select one of the three audio save paths:\n\nDefault path: {Constants.DefaultSavePath ?? "?"}\nAlternate path: (Geometry Dash folder)\\Resources\nOther: To specify.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string SanitizeAudioName(string str)
        {
            foreach (var pair in Constants.Filter)
            {
                str = str.Replace(pair.Key, pair.Value);
            }

            str = Regex.Replace(str, "[^a-zA-Z0-9-_]", string.Empty);
            str = str.Substring(0, Math.Min(str.Length, Constants.MaxAudioNameLength));

            return str;
        }
    }
}