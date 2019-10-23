using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class MainGUI : Form
    {
        public static string AudioName;
        public static int ID;
        public static string Path;
        public static string DefaultPath = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\GeometryDash"; //Default save path

        private string[,] Filter =
        {
            { " ", "-"}, {"&", "amp"}, {"<", "lt"}, {">", "gt"}, {"\"", "quot"}
        };

        public MainGUI()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            AudioName = textBoxAudioName.Text;
            if (string.IsNullOrWhiteSpace(textBoxID.Text) || string.IsNullOrWhiteSpace(AudioName))
            {
                return;
            }
            ID = int.Parse(textBoxID.Text);
            if (ID < 469775) //First ID with the known name pattern.
            {
                MessageBox.Show("No support for ID's minor than 469775\nThe known name pattern can't be used.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
                    Path = fbd.SelectedPath;
                }
                if (!Directory.Exists(Path)) //(string.IsNullOrWhiteSpace(Path))
                {
                    MessageBox.Show("The selected folder is invalid or inaccessible.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (comboBoxSavePath.SelectedIndex == 1)
                {
                    Path += "\\Resources";
                    if (!Directory.Exists(Path)) //Check if Resources folder doesn't exist.
                    {
                        MessageBox.Show("\"Resources\" folder, the alternate save folder, wasn't found.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (!Directory.Exists(DefaultPath)) //If default save path doesn't exists.
                {
                    MessageBox.Show("The default save path:\n" + DefaultPath + "\nwasn't found. Please select other save path.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Path = DefaultPath;
            }
            Path += "\\" + ID + ".mp3"; //Add the ID and audio extension in any save path case.
            if (File.Exists(Path) && MessageBox.Show("There's already an audio named \"" + ID + ".mp3\". Overwrite?", "GD Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) //If the audio file exists, ask if overwrite.
            {
                return;
            }
            for (int i = 0; i < Filter.GetLength(0); i++)
                AudioName = AudioName.Replace(Filter[i, 0], Filter[i, 1]);
            AudioName = Regex.Replace(AudioName, "[^\\w-_]", "");
            if (AudioName.Length > 26)
                AudioName = AudioName.Substring(0, 26);
            /*
             * Special chars conversion:
             *
             * (Space) = -
             *       & = amp
             *       < = lt
             *       > = gt
             *       " = quot
             *
             * Others chars except "-" and "_" will be deleted
             * Characters limit = 26
             */
            this.Hide(); //Hide the main GUI
            ProgressBarGUI PBGUI = new ProgressBarGUI();
            PBGUI.ShowDialog(); //Show the progress bar GUI
            PBGUI.Dispose();
            this.Show();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can select one of the three audio save paths:\n\nDefault path: " + DefaultPath + "\nAlternate path: (Geometry Dash folder)\\Resources\nOther: To specify.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            //A way to detect non-numbers, delete them, and set the text selection where it should be, without using NumericUpDown or MaskedTextBox. Also works for copy paste.
            int MatchCount = Regex.Matches(textBoxID.Text, "[^0-9]").Count; //Get count of non-numbers
            if (MatchCount != 0) //If contains non-numbers
            {
                int LastIndex = textBoxID.SelectionStart; //Character index
                textBoxID.Text = Regex.Replace(textBoxID.Text, "[^0-9]", ""); //Delete all non-numbers, SelectionStart will be set to 0.
                textBoxID.SelectionStart = LastIndex - MatchCount; //Set the index where it should be.
            }
        }
    }
}