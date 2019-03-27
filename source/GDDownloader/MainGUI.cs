using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class MainGUI : Form
    {
        public static string ID;
        public static string Path = "C:\\Users\\" + Environment.UserName + " \\AppData\\Local\\GeometryDash";
        public static string SongName;

        public MainGUI()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxID.Text) || string.IsNullOrWhiteSpace(textBoxAudioName.Text))
            {
                return;
            }
            if (Double.Parse(textBoxID.Text) < 469775) //First ID with the known name pattern.
            {
                MessageBox.Show("No support for ID's minor than 469775\nThe known name pattern can't be used.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxSavePath.SelectedIndex != 0) //Alternate save path or other.
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = (comboBoxSavePath.SelectedIndex == 1) ? "Select the Geometry Dash folder" : "Select the save folder";
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Path = fbd.SelectedPath;
                    if (comboBoxSavePath.SelectedIndex == 1)
                    {
                        if (!Directory.Exists(fbd.SelectedPath + "\\Resources")) //Check if Resources folder doesn't exist.
                        {
                            MessageBox.Show("\"Resources\" folder, the alternate save folder, wasn't found.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Path += "\\Resources";
                    }
                }
                else
                {
                    MessageBox.Show("You didn't selected a folder or it isn't a valid folder", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            ID = textBoxID.Text;
            Path += "\\" + ID + ".mp3"; //Finally, add the ID and audio extension in any save path case.
            if (File.Exists(Path))
            {
                if (MessageBox.Show("There's already an audio named \"" + ID + ".mp3\". Overwrite?", "GD Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    Path = "C:\\Users\\" + Environment.UserName + " \\AppData\\Local\\GeometryDash"; //Reset the save path
                    return;
                }
            }
            SongName = Regex.Replace(textBoxAudioName.Text.Replace(" ", "-").Replace("&", "amp").Replace("<", "lt").Replace(">", "gt").Replace("\"", "quot"), "[^\\w-_]", "");
            if (SongName.Length > 26)
                SongName = SongName.Substring(0, 26);
            this.Hide(); //Hide the main GUI
            ProgressBarGUI form2 = new ProgressBarGUI();
            form2.Show(); //Show the progress bar GUI
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can select one of the three audio save paths:\n\nNormal path: " + Path + "\nAlternate path: Geometry Dash folder\\Resources\nOther: To specify.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxID.Text, "[^0-9]")) //If contains non-numerical
            {
                textBoxID.Text = Regex.Replace(textBoxID.Text, "[^0-9]", ""); //Keep the numbers
                textBoxID.SelectionStart = textBoxID.Text.Length; //Move cursor to end
            }
        }
    }
}