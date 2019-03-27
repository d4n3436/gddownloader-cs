namespace GDMainGUI
{
    partial class MainGUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.labelAudioName = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.textBoxAudioName = new System.Windows.Forms.TextBox();
            this.labelPrompt = new System.Windows.Forms.Label();
            this.labelSavePath = new System.Windows.Forms.Label();
            this.comboBoxSavePath = new System.Windows.Forms.ComboBox();
            this.groupBoxData.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(90, 180);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(180, 180);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 2;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.textBoxID);
            this.groupBoxData.Controls.Add(this.labelAudioName);
            this.groupBoxData.Controls.Add(this.labelID);
            this.groupBoxData.Controls.Add(this.textBoxAudioName);
            this.groupBoxData.Location = new System.Drawing.Point(29, 37);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(210, 72);
            this.groupBoxData.TabIndex = 3;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "Data";
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(15, 40);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(56, 20);
            this.textBoxID.TabIndex = 4;
            this.textBoxID.TextChanged += new System.EventHandler(this.textBoxID_TextChanged);
            // 
            // labelAudioName
            // 
            this.labelAudioName.AutoSize = true;
            this.labelAudioName.Location = new System.Drawing.Point(110, 26);
            this.labelAudioName.Name = "labelAudioName";
            this.labelAudioName.Size = new System.Drawing.Size(65, 13);
            this.labelAudioName.TabIndex = 3;
            this.labelAudioName.Text = "Audio Name";
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(34, 26);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(18, 13);
            this.labelID.TabIndex = 2;
            this.labelID.Text = "ID";
            // 
            // textBoxAudioName
            // 
            this.textBoxAudioName.Location = new System.Drawing.Point(94, 40);
            this.textBoxAudioName.Name = "textBoxAudioName";
            this.textBoxAudioName.Size = new System.Drawing.Size(100, 20);
            this.textBoxAudioName.TabIndex = 1;
            // 
            // labelPrompt
            // 
            this.labelPrompt.AutoSize = true;
            this.labelPrompt.Location = new System.Drawing.Point(41, 11);
            this.labelPrompt.Name = "labelPrompt";
            this.labelPrompt.Size = new System.Drawing.Size(182, 13);
            this.labelPrompt.TabIndex = 4;
            this.labelPrompt.Text = "Enter the data to download the audio";
            // 
            // labelSavePath
            // 
            this.labelSavePath.AutoSize = true;
            this.labelSavePath.Location = new System.Drawing.Point(71, 124);
            this.labelSavePath.Name = "labelSavePath";
            this.labelSavePath.Size = new System.Drawing.Size(56, 13);
            this.labelSavePath.TabIndex = 5;
            this.labelSavePath.Text = "Save path";
            // 
            // comboBoxSavePath
            // 
            this.comboBoxSavePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSavePath.FormattingEnabled = true;
            this.comboBoxSavePath.Items.AddRange(new object[] {
            "Normal Path",
            "Alternate Path",
            "Other..."});
            this.comboBoxSavePath.Location = new System.Drawing.Point(74, 140);
            this.comboBoxSavePath.MaxDropDownItems = 3;
            this.comboBoxSavePath.SelectedIndex = 0;
            this.comboBoxSavePath.Name = "comboBoxSavePath";
            this.comboBoxSavePath.Size = new System.Drawing.Size(105, 21);
            this.comboBoxSavePath.TabIndex = 6;
            // 
            // MainGUI
            // 
            this.AcceptButton = this.buttonDownload;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(264, 210);
            this.Controls.Add(this.comboBoxSavePath);
            this.Controls.Add(this.labelSavePath);
            this.Controls.Add(this.labelPrompt);
            this.Controls.Add(this.groupBoxData);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GD Downloader";
            this.groupBoxData.ResumeLayout(false);
            this.groupBoxData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.TextBox textBoxAudioName;
        private System.Windows.Forms.Label labelAudioName;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Label labelPrompt;
        private System.Windows.Forms.Label labelSavePath;
        private System.Windows.Forms.ComboBox comboBoxSavePath;
        private System.Windows.Forms.TextBox textBoxID;
    }
}

