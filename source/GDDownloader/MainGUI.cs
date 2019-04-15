using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GDDownloader
{
    public partial class MainGUI : Form
    {
        public static double ID;
        public static string Path;
        public static string SongName;

        public MainGUI()
        {
            InitializeComponent();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            Path = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\GeometryDash"; //Default save path
            ID = double.Parse(textBoxID.Text);

            if (string.IsNullOrWhiteSpace(ID.ToString()) || string.IsNullOrWhiteSpace(textBoxAudioName.Text))
            {
                return;
            }
            if (ID < 469775) //First ID with the known name pattern.
            {
                MessageBox.Show("No support for ID's minor than 469775\nThe known name pattern can't be used.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Directory.Exists(Path)) //If default save path doesn't exists.
            {
                MessageBox.Show("The default save path:\n" + Path + "\nwasn't found. Please select other save path.", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxSavePath.SelectedIndex != 0) //Alternate save path or other.
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (comboBoxSavePath.SelectedIndex == 1)
                {
                    fbd.ShowNewFolderButton = false;
                    fbd.Description = "Select the Geometry Dash folder";
                }
                else
                {
                    fbd.Description = "Select the save folder";
                }
                DialogResult result = fbd.ShowDialog();
                Path = fbd.SelectedPath;
                if (result != DialogResult.OK)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(Path))
                {
                    MessageBox.Show("You selected a invalid folder", "GD Downloader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            Path += "\\" + ID + ".mp3"; //Add the ID and audio extension in any save path case.

            if (File.Exists(Path)) //If the audio file exists, ask if overwrite.
            {
                if (MessageBox.Show("There's already an audio named \"" + ID + ".mp3\". Overwrite?", "GD Downloader", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            SongName = Regex.Replace(textBoxAudioName.Text.Replace(" ", "-").Replace("&", "amp").Replace("<", "lt").Replace(">", "gt").Replace("\"", "quot"), "[^\\w-_]", "");
            if (SongName.Length > 26)
                SongName = SongName.Substring(0, 26);

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
            PBGUI.Show(); //Show the progress bar GUI
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can select one of the three audio save paths:\n\nDefault path: " + "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\GeometryDash" + "\nAlternate path: Geometry Dash folder\\Resources\nOther: To specify.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            int MatchCount = Regex.Matches(textBoxID.Text, "[^0-9]").Count; //Get match count
            if (MatchCount != 0) //If contains non-numbers
            {
                int LastIndex = textBoxID.SelectionStart; //Character index
                textBoxID.Text = Regex.Replace(textBoxID.Text, "[^0-9]", ""); //Delete all non-numbers, SelectionStart will be set to 0.
                textBoxID.SelectionStart = LastIndex - MatchCount;
            }
        }
    }
}