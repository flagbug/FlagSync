namespace FlagSync.GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.currentJobOverviewGroupBox = new System.Windows.Forms.GroupBox();
            this.currentJobNameLabel = new System.Windows.Forms.Label();
            this.currentJobNameDescriptionLabel = new System.Windows.Forms.Label();
            this.currentJobSyncModeLabel = new System.Windows.Forms.Label();
            this.currentDirectoryBLabel = new System.Windows.Forms.Label();
            this.currentDirectoryALabel = new System.Windows.Forms.Label();
            this.currentJobSyncModeDescriptionLabel = new System.Windows.Forms.Label();
            this.currentJobDirectoryBDescriptionLabel = new System.Windows.Forms.Label();
            this.currentJobDirectoryDescriptionALabel = new System.Windows.Forms.Label();
            this.controlsGroupBox = new System.Windows.Forms.GroupBox();
            this.controlButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stopButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.previewButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.syncRadioButton = new System.Windows.Forms.RadioButton();
            this.backupRadioButton = new System.Windows.Forms.RadioButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.overviewTabPage = new System.Windows.Forms.TabPage();
            this.textBoxPanel = new System.Windows.Forms.Panel();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.directorySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.jobSettingsNameTextBox = new System.Windows.Forms.TextBox();
            this.jobSettingsNameLabel = new System.Windows.Forms.Label();
            this.browseDirectoryBButton = new System.Windows.Forms.Button();
            this.browseDirectoryAButton = new System.Windows.Forms.Button();
            this.jobSettingsDirectoryBTextBox = new System.Windows.Forms.TextBox();
            this.jobSettingsDirectoryATextBox = new System.Windows.Forms.TextBox();
            this.deleteJobButton = new System.Windows.Forms.Button();
            this.jobSettingsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.newJobButton = new System.Windows.Forms.Button();
            this.jobSettingsSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.jobSettingsOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.currentJobOverviewGroupBox.SuspendLayout();
            this.controlsGroupBox.SuspendLayout();
            this.controlButtonsTableLayoutPanel.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.overviewTabPage.SuspendLayout();
            this.textBoxPanel.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.directorySettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.currentJobOverviewGroupBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.controlsGroupBox);
            // 
            // currentJobOverviewGroupBox
            // 
            resources.ApplyResources(this.currentJobOverviewGroupBox, "currentJobOverviewGroupBox");
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobNameLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobNameDescriptionLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobSyncModeLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentDirectoryBLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentDirectoryALabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobSyncModeDescriptionLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobDirectoryBDescriptionLabel);
            this.currentJobOverviewGroupBox.Controls.Add(this.currentJobDirectoryDescriptionALabel);
            this.currentJobOverviewGroupBox.Name = "currentJobOverviewGroupBox";
            this.currentJobOverviewGroupBox.TabStop = false;
            // 
            // currentJobNameLabel
            // 
            resources.ApplyResources(this.currentJobNameLabel, "currentJobNameLabel");
            this.currentJobNameLabel.Name = "currentJobNameLabel";
            // 
            // currentJobNameDescriptionLabel
            // 
            resources.ApplyResources(this.currentJobNameDescriptionLabel, "currentJobNameDescriptionLabel");
            this.currentJobNameDescriptionLabel.Name = "currentJobNameDescriptionLabel";
            // 
            // currentJobSyncModeLabel
            // 
            resources.ApplyResources(this.currentJobSyncModeLabel, "currentJobSyncModeLabel");
            this.currentJobSyncModeLabel.Name = "currentJobSyncModeLabel";
            // 
            // currentDirectoryBLabel
            // 
            resources.ApplyResources(this.currentDirectoryBLabel, "currentDirectoryBLabel");
            this.currentDirectoryBLabel.Name = "currentDirectoryBLabel";
            // 
            // currentDirectoryALabel
            // 
            resources.ApplyResources(this.currentDirectoryALabel, "currentDirectoryALabel");
            this.currentDirectoryALabel.Name = "currentDirectoryALabel";
            // 
            // currentJobSyncModeDescriptionLabel
            // 
            resources.ApplyResources(this.currentJobSyncModeDescriptionLabel, "currentJobSyncModeDescriptionLabel");
            this.currentJobSyncModeDescriptionLabel.Name = "currentJobSyncModeDescriptionLabel";
            // 
            // currentJobDirectoryBDescriptionLabel
            // 
            resources.ApplyResources(this.currentJobDirectoryBDescriptionLabel, "currentJobDirectoryBDescriptionLabel");
            this.currentJobDirectoryBDescriptionLabel.Name = "currentJobDirectoryBDescriptionLabel";
            // 
            // currentJobDirectoryDescriptionALabel
            // 
            resources.ApplyResources(this.currentJobDirectoryDescriptionALabel, "currentJobDirectoryDescriptionALabel");
            this.currentJobDirectoryDescriptionALabel.Name = "currentJobDirectoryDescriptionALabel";
            // 
            // controlsGroupBox
            // 
            resources.ApplyResources(this.controlsGroupBox, "controlsGroupBox");
            this.controlsGroupBox.Controls.Add(this.controlButtonsTableLayoutPanel);
            this.controlsGroupBox.Controls.Add(this.progressBar);
            this.controlsGroupBox.Name = "controlsGroupBox";
            this.controlsGroupBox.TabStop = false;
            // 
            // controlButtonsTableLayoutPanel
            // 
            resources.ApplyResources(this.controlButtonsTableLayoutPanel, "controlButtonsTableLayoutPanel");
            this.controlButtonsTableLayoutPanel.Controls.Add(this.stopButton, 3, 0);
            this.controlButtonsTableLayoutPanel.Controls.Add(this.pauseButton, 2, 0);
            this.controlButtonsTableLayoutPanel.Controls.Add(this.startButton, 1, 0);
            this.controlButtonsTableLayoutPanel.Controls.Add(this.previewButton, 0, 0);
            this.controlButtonsTableLayoutPanel.MaximumSize = new System.Drawing.Size(600, 30);
            this.controlButtonsTableLayoutPanel.Name = "controlButtonsTableLayoutPanel";
            // 
            // stopButton
            // 
            resources.ApplyResources(this.stopButton, "stopButton");
            this.stopButton.MaximumSize = new System.Drawing.Size(120, 25);
            this.stopButton.Name = "stopButton";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // pauseButton
            // 
            resources.ApplyResources(this.pauseButton, "pauseButton");
            this.pauseButton.MaximumSize = new System.Drawing.Size(120, 25);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // startButton
            // 
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.MaximumSize = new System.Drawing.Size(120, 25);
            this.startButton.Name = "startButton";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // previewButton
            // 
            resources.ApplyResources(this.previewButton, "previewButton");
            this.previewButton.MaximumSize = new System.Drawing.Size(120, 25);
            this.previewButton.Name = "previewButton";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Step = 1;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
            this.mainMenuStrip.Name = "mainMenuStrip";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            resources.ApplyResources(this.programToolStripMenuItem, "programToolStripMenuItem");
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            resources.ApplyResources(this.loadToolStripMenuItem, "loadToolStripMenuItem");
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.logTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.logTextBox, "logTextBox");
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 50;
            // 
            // syncRadioButton
            // 
            resources.ApplyResources(this.syncRadioButton, "syncRadioButton");
            this.syncRadioButton.Name = "syncRadioButton";
            this.toolTip.SetToolTip(this.syncRadioButton, resources.GetString("syncRadioButton.ToolTip"));
            this.syncRadioButton.UseVisualStyleBackColor = true;
            this.syncRadioButton.Click += new System.EventHandler(this.syncRadioButton_Click);
            // 
            // backupRadioButton
            // 
            resources.ApplyResources(this.backupRadioButton, "backupRadioButton");
            this.backupRadioButton.Checked = true;
            this.backupRadioButton.Name = "backupRadioButton";
            this.backupRadioButton.TabStop = true;
            this.toolTip.SetToolTip(this.backupRadioButton, resources.GetString("backupRadioButton.ToolTip"));
            this.backupRadioButton.UseVisualStyleBackColor = true;
            this.backupRadioButton.Click += new System.EventHandler(this.backupRadioButton_Click);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.overviewTabPage);
            this.tabControl.Controls.Add(this.settingsTabPage);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // overviewTabPage
            // 
            this.overviewTabPage.Controls.Add(this.textBoxPanel);
            this.overviewTabPage.Controls.Add(this.splitContainer);
            resources.ApplyResources(this.overviewTabPage, "overviewTabPage");
            this.overviewTabPage.Name = "overviewTabPage";
            this.overviewTabPage.UseVisualStyleBackColor = true;
            // 
            // textBoxPanel
            // 
            resources.ApplyResources(this.textBoxPanel, "textBoxPanel");
            this.textBoxPanel.Controls.Add(this.logTextBox);
            this.textBoxPanel.Name = "textBoxPanel";
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.settingsPanel);
            resources.ApplyResources(this.settingsTabPage, "settingsTabPage");
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsPanel
            // 
            resources.ApplyResources(this.settingsPanel, "settingsPanel");
            this.settingsPanel.Controls.Add(this.directorySettingsGroupBox);
            this.settingsPanel.Controls.Add(this.deleteJobButton);
            this.settingsPanel.Controls.Add(this.jobSettingsCheckedListBox);
            this.settingsPanel.Controls.Add(this.newJobButton);
            this.settingsPanel.Name = "settingsPanel";
            // 
            // directorySettingsGroupBox
            // 
            resources.ApplyResources(this.directorySettingsGroupBox, "directorySettingsGroupBox");
            this.directorySettingsGroupBox.Controls.Add(this.label1);
            this.directorySettingsGroupBox.Controls.Add(this.syncRadioButton);
            this.directorySettingsGroupBox.Controls.Add(this.backupRadioButton);
            this.directorySettingsGroupBox.Controls.Add(this.jobSettingsNameTextBox);
            this.directorySettingsGroupBox.Controls.Add(this.jobSettingsNameLabel);
            this.directorySettingsGroupBox.Controls.Add(this.browseDirectoryBButton);
            this.directorySettingsGroupBox.Controls.Add(this.browseDirectoryAButton);
            this.directorySettingsGroupBox.Controls.Add(this.jobSettingsDirectoryBTextBox);
            this.directorySettingsGroupBox.Controls.Add(this.jobSettingsDirectoryATextBox);
            this.directorySettingsGroupBox.MaximumSize = new System.Drawing.Size(450, 144);
            this.directorySettingsGroupBox.MinimumSize = new System.Drawing.Size(350, 144);
            this.directorySettingsGroupBox.Name = "directorySettingsGroupBox";
            this.directorySettingsGroupBox.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // jobSettingsNameTextBox
            // 
            resources.ApplyResources(this.jobSettingsNameTextBox, "jobSettingsNameTextBox");
            this.jobSettingsNameTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.jobSettingsNameTextBox.Name = "jobSettingsNameTextBox";
            this.jobSettingsNameTextBox.TextChanged += new System.EventHandler(this.jobSettingsNameTextBox_TextChanged);
            // 
            // jobSettingsNameLabel
            // 
            resources.ApplyResources(this.jobSettingsNameLabel, "jobSettingsNameLabel");
            this.jobSettingsNameLabel.Name = "jobSettingsNameLabel";
            // 
            // browseDirectoryBButton
            // 
            resources.ApplyResources(this.browseDirectoryBButton, "browseDirectoryBButton");
            this.browseDirectoryBButton.Name = "browseDirectoryBButton";
            this.browseDirectoryBButton.UseVisualStyleBackColor = true;
            this.browseDirectoryBButton.Click += new System.EventHandler(this.browseDirectoryBButton_Click);
            // 
            // browseDirectoryAButton
            // 
            resources.ApplyResources(this.browseDirectoryAButton, "browseDirectoryAButton");
            this.browseDirectoryAButton.Name = "browseDirectoryAButton";
            this.browseDirectoryAButton.UseVisualStyleBackColor = true;
            this.browseDirectoryAButton.Click += new System.EventHandler(this.browseDirectoryAButton_Click);
            // 
            // jobSettingsDirectoryBTextBox
            // 
            resources.ApplyResources(this.jobSettingsDirectoryBTextBox, "jobSettingsDirectoryBTextBox");
            this.jobSettingsDirectoryBTextBox.Name = "jobSettingsDirectoryBTextBox";
            this.jobSettingsDirectoryBTextBox.TextChanged += new System.EventHandler(this.jobSettingsDirectoryBTextBox_TextChanged);
            // 
            // jobSettingsDirectoryATextBox
            // 
            resources.ApplyResources(this.jobSettingsDirectoryATextBox, "jobSettingsDirectoryATextBox");
            this.jobSettingsDirectoryATextBox.BackColor = System.Drawing.SystemColors.Window;
            this.jobSettingsDirectoryATextBox.Name = "jobSettingsDirectoryATextBox";
            this.jobSettingsDirectoryATextBox.TextChanged += new System.EventHandler(this.jobSettingsDirectoryATextBox_TextChanged);
            // 
            // deleteJobButton
            // 
            resources.ApplyResources(this.deleteJobButton, "deleteJobButton");
            this.deleteJobButton.Name = "deleteJobButton";
            this.deleteJobButton.UseVisualStyleBackColor = true;
            this.deleteJobButton.Click += new System.EventHandler(this.deleteJobButton_Click);
            // 
            // jobSettingsCheckedListBox
            // 
            resources.ApplyResources(this.jobSettingsCheckedListBox, "jobSettingsCheckedListBox");
            this.jobSettingsCheckedListBox.FormattingEnabled = true;
            this.jobSettingsCheckedListBox.Name = "jobSettingsCheckedListBox";
            this.jobSettingsCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.jobSettingsCheckedListBox_SelectedIndexChanged);
            // 
            // newJobButton
            // 
            resources.ApplyResources(this.newJobButton, "newJobButton");
            this.newJobButton.Name = "newJobButton";
            this.newJobButton.UseVisualStyleBackColor = true;
            this.newJobButton.Click += new System.EventHandler(this.newJobButton_Click);
            // 
            // jobSettingsSaveFileDialog
            // 
            this.jobSettingsSaveFileDialog.DefaultExt = "xml";
            this.jobSettingsSaveFileDialog.FileName = "NewFlagSync";
            resources.ApplyResources(this.jobSettingsSaveFileDialog, "jobSettingsSaveFileDialog");
            // 
            // jobSettingsOpenFileDialog
            // 
            this.jobSettingsOpenFileDialog.DefaultExt = "xml";
            resources.ApplyResources(this.jobSettingsOpenFileDialog, "jobSettingsOpenFileDialog");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.mainMenuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.currentJobOverviewGroupBox.ResumeLayout(false);
            this.controlsGroupBox.ResumeLayout(false);
            this.controlButtonsTableLayoutPanel.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.overviewTabPage.ResumeLayout(false);
            this.textBoxPanel.ResumeLayout(false);
            this.settingsTabPage.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.directorySettingsGroupBox.ResumeLayout(false);
            this.directorySettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox controlsGroupBox;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage overviewTabPage;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.GroupBox currentJobOverviewGroupBox;
        private System.Windows.Forms.GroupBox directorySettingsGroupBox;
        private System.Windows.Forms.Button browseDirectoryBButton;
        private System.Windows.Forms.Button browseDirectoryAButton;
        private System.Windows.Forms.TextBox jobSettingsDirectoryBTextBox;
        private System.Windows.Forms.TextBox jobSettingsDirectoryATextBox;
        private System.Windows.Forms.Label currentJobDirectoryDescriptionALabel;
        private System.Windows.Forms.Label currentJobDirectoryBDescriptionLabel;
        private System.Windows.Forms.Label currentJobSyncModeDescriptionLabel;
        private System.Windows.Forms.Label currentDirectoryBLabel;
        private System.Windows.Forms.Label currentDirectoryALabel;
        private System.Windows.Forms.Label currentJobSyncModeLabel;
        private System.Windows.Forms.Label jobSettingsNameLabel;
        private System.Windows.Forms.CheckedListBox jobSettingsCheckedListBox;
        private System.Windows.Forms.TextBox jobSettingsNameTextBox;
        private System.Windows.Forms.Button newJobButton;
        private System.Windows.Forms.Button deleteJobButton;
        private System.Windows.Forms.RadioButton syncRadioButton;
        private System.Windows.Forms.RadioButton backupRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog jobSettingsSaveFileDialog;
        private System.Windows.Forms.OpenFileDialog jobSettingsOpenFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel textBoxPanel;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.TableLayoutPanel controlButtonsTableLayoutPanel;
        private System.Windows.Forms.Label currentJobNameLabel;
        private System.Windows.Forms.Label currentJobNameDescriptionLabel;
    }
}

