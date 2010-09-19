namespace FlagSync
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryATextBox = new System.Windows.Forms.TextBox();
            this.directoriesGroupBox = new System.Windows.Forms.GroupBox();
            this.syncModeButton = new System.Windows.Forms.Button();
            this.browseDirectoryBButton = new System.Windows.Forms.Button();
            this.browseDirectoryAButton = new System.Windows.Forms.Button();
            this.directoryBTextBox = new System.Windows.Forms.TextBox();
            this.previewButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.controlsGroupBox = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenuStrip.SuspendLayout();
            this.directoriesGroupBox.SuspendLayout();
            this.controlsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(742, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(24, 20);
            this.helpToolStripMenuItem.Text = "?";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // directoryATextBox
            // 
            this.directoryATextBox.BackColor = System.Drawing.SystemColors.Window;
            this.directoryATextBox.Location = new System.Drawing.Point(87, 21);
            this.directoryATextBox.Name = "directoryATextBox";
            this.directoryATextBox.Size = new System.Drawing.Size(200, 20);
            this.directoryATextBox.TabIndex = 1;
            // 
            // directoriesGroupBox
            // 
            this.directoriesGroupBox.Controls.Add(this.syncModeButton);
            this.directoriesGroupBox.Controls.Add(this.browseDirectoryBButton);
            this.directoriesGroupBox.Controls.Add(this.browseDirectoryAButton);
            this.directoriesGroupBox.Controls.Add(this.directoryBTextBox);
            this.directoriesGroupBox.Controls.Add(this.directoryATextBox);
            this.directoriesGroupBox.Location = new System.Drawing.Point(12, 27);
            this.directoriesGroupBox.Name = "directoriesGroupBox";
            this.directoriesGroupBox.Size = new System.Drawing.Size(351, 74);
            this.directoriesGroupBox.TabIndex = 2;
            this.directoriesGroupBox.TabStop = false;
            this.directoriesGroupBox.Text = "Directories";
            // 
            // syncModeButton
            // 
            this.syncModeButton.AccessibleDescription = "";
            this.syncModeButton.AccessibleName = "";
            this.syncModeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.syncModeButton.Location = new System.Drawing.Point(294, 21);
            this.syncModeButton.Name = "syncModeButton";
            this.syncModeButton.Size = new System.Drawing.Size(49, 49);
            this.syncModeButton.TabIndex = 7;
            this.syncModeButton.Text = "A<->B";
            this.toolTip.SetToolTip(this.syncModeButton, "A<->B = Synchronization\r\nA-->B = Backup");
            this.syncModeButton.UseVisualStyleBackColor = true;
            this.syncModeButton.Click += new System.EventHandler(this.syncModeButton_Click);
            // 
            // browseDirectoryBButton
            // 
            this.browseDirectoryBButton.Location = new System.Drawing.Point(6, 48);
            this.browseDirectoryBButton.Name = "browseDirectoryBButton";
            this.browseDirectoryBButton.Size = new System.Drawing.Size(75, 23);
            this.browseDirectoryBButton.TabIndex = 6;
            this.browseDirectoryBButton.Text = "Directory B";
            this.browseDirectoryBButton.UseVisualStyleBackColor = true;
            this.browseDirectoryBButton.Click += new System.EventHandler(this.browseDirectoryBButton_Click);
            // 
            // browseDirectoryAButton
            // 
            this.browseDirectoryAButton.Location = new System.Drawing.Point(6, 19);
            this.browseDirectoryAButton.Name = "browseDirectoryAButton";
            this.browseDirectoryAButton.Size = new System.Drawing.Size(75, 23);
            this.browseDirectoryAButton.TabIndex = 5;
            this.browseDirectoryAButton.Text = "Directory A";
            this.browseDirectoryAButton.UseVisualStyleBackColor = true;
            this.browseDirectoryAButton.Click += new System.EventHandler(this.browseDirectoryAButton_Click);
            // 
            // directoryBTextBox
            // 
            this.directoryBTextBox.Location = new System.Drawing.Point(87, 50);
            this.directoryBTextBox.Name = "directoryBTextBox";
            this.directoryBTextBox.Size = new System.Drawing.Size(200, 20);
            this.directoryBTextBox.TabIndex = 3;
            // 
            // previewButton
            // 
            this.previewButton.Location = new System.Drawing.Point(6, 48);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(80, 25);
            this.previewButton.TabIndex = 3;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(92, 48);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(80, 25);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.syncButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(349, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 5;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.logTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.logTextBox.Location = new System.Drawing.Point(12, 108);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(718, 403);
            this.logTextBox.TabIndex = 6;
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(178, 48);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(80, 25);
            this.pauseButton.TabIndex = 7;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(264, 48);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(80, 25);
            this.stopButton.TabIndex = 8;
            this.stopButton.Text = " Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // controlsGroupBox
            // 
            this.controlsGroupBox.Controls.Add(this.stopButton);
            this.controlsGroupBox.Controls.Add(this.progressBar);
            this.controlsGroupBox.Controls.Add(this.previewButton);
            this.controlsGroupBox.Controls.Add(this.startButton);
            this.controlsGroupBox.Controls.Add(this.pauseButton);
            this.controlsGroupBox.Location = new System.Drawing.Point(369, 27);
            this.controlsGroupBox.Name = "controlsGroupBox";
            this.controlsGroupBox.Size = new System.Drawing.Size(361, 75);
            this.controlsGroupBox.TabIndex = 9;
            this.controlsGroupBox.TabStop = false;
            this.controlsGroupBox.Text = "Controls";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 523);
            this.Controls.Add(this.controlsGroupBox);
            this.Controls.Add(this.directoriesGroupBox);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.logTextBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "FlagSync";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.directoriesGroupBox.ResumeLayout(false);
            this.directoriesGroupBox.PerformLayout();
            this.controlsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox directoryATextBox;
        private System.Windows.Forms.GroupBox directoriesGroupBox;
        private System.Windows.Forms.TextBox directoryBTextBox;
        private System.Windows.Forms.Button browseDirectoryBButton;
        private System.Windows.Forms.Button browseDirectoryAButton;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox controlsGroupBox;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button syncModeButton;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

