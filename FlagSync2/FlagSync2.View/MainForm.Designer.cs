namespace FlagSync2.View
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.overviewTabPage = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FolderA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.startButton = new System.Windows.Forms.Button();
            this.configurationTabPage = new System.Windows.Forms.TabPage();
            this.newSnchronizationButton = new System.Windows.Forms.Button();
            this.newSynchronizationButton = new System.Windows.Forms.Button();
            this.jobConfigurationPanel = new System.Windows.Forms.Panel();
            this.deleteJobButton = new System.Windows.Forms.Button();
            this.jobsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.syncInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.overviewTabPage.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.configurationTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.syncInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip.Size = new System.Drawing.Size(758, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.tabControl);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 24);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(758, 380);
            this.mainPanel.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.overviewTabPage);
            this.tabControl.Controls.Add(this.configurationTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(758, 380);
            this.tabControl.TabIndex = 0;
            // 
            // overviewTabPage
            // 
            this.overviewTabPage.Controls.Add(this.tabControl1);
            this.overviewTabPage.Controls.Add(this.panel1);
            this.overviewTabPage.Location = new System.Drawing.Point(4, 22);
            this.overviewTabPage.Name = "overviewTabPage";
            this.overviewTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.overviewTabPage.Size = new System.Drawing.Size(750, 354);
            this.overviewTabPage.TabIndex = 0;
            this.overviewTabPage.Text = "Overview";
            this.overviewTabPage.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(750, 330);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(742, 304);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.FolderA,
            this.Direction,
            this.FolderB});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(740, 300);
            this.dataGridView1.TabIndex = 0;
            // 
            // Check
            // 
            this.Check.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Check.HeaderText = "Check";
            this.Check.Name = "Check";
            this.Check.Width = 44;
            // 
            // FolderA
            // 
            this.FolderA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FolderA.HeaderText = "Folder A ";
            this.FolderA.Name = "FolderA";
            this.FolderA.Width = 74;
            // 
            // Direction
            // 
            this.Direction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Direction.HeaderText = "Direction";
            this.Direction.Name = "Direction";
            this.Direction.Width = 74;
            // 
            // FolderB
            // 
            this.FolderB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FolderB.HeaderText = "Folder B";
            this.FolderB.Name = "FolderB";
            this.FolderB.Width = 71;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 30);
            this.panel1.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(0, 0);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 30);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // configurationTabPage
            // 
            this.configurationTabPage.Controls.Add(this.newSnchronizationButton);
            this.configurationTabPage.Controls.Add(this.newSynchronizationButton);
            this.configurationTabPage.Controls.Add(this.jobConfigurationPanel);
            this.configurationTabPage.Controls.Add(this.deleteJobButton);
            this.configurationTabPage.Controls.Add(this.jobsCheckedListBox);
            this.configurationTabPage.Location = new System.Drawing.Point(4, 22);
            this.configurationTabPage.Name = "configurationTabPage";
            this.configurationTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.configurationTabPage.Size = new System.Drawing.Size(750, 354);
            this.configurationTabPage.TabIndex = 1;
            this.configurationTabPage.Text = "Configuration";
            this.configurationTabPage.UseVisualStyleBackColor = true;
            // 
            // newSnchronizationButton
            // 
            this.newSnchronizationButton.Location = new System.Drawing.Point(130, 50);
            this.newSnchronizationButton.Name = "newSnchronizationButton";
            this.newSnchronizationButton.Size = new System.Drawing.Size(130, 40);
            this.newSnchronizationButton.TabIndex = 6;
            this.newSnchronizationButton.Text = "New Synchronization";
            this.newSnchronizationButton.UseVisualStyleBackColor = true;
            // 
            // newSynchronizationButton
            // 
            this.newSynchronizationButton.Location = new System.Drawing.Point(130, 10);
            this.newSynchronizationButton.Name = "newSynchronizationButton";
            this.newSynchronizationButton.Size = new System.Drawing.Size(130, 40);
            this.newSynchronizationButton.TabIndex = 5;
            this.newSynchronizationButton.Text = "New Backup";
            this.newSynchronizationButton.UseVisualStyleBackColor = true;
            this.newSynchronizationButton.Click += new System.EventHandler(this.newSynchronizationButton_Click);
            // 
            // jobConfigurationPanel
            // 
            this.jobConfigurationPanel.Location = new System.Drawing.Point(270, 10);
            this.jobConfigurationPanel.Name = "jobConfigurationPanel";
            this.jobConfigurationPanel.Size = new System.Drawing.Size(470, 330);
            this.jobConfigurationPanel.TabIndex = 4;
            // 
            // deleteJobButton
            // 
            this.deleteJobButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteJobButton.Location = new System.Drawing.Point(0, 320);
            this.deleteJobButton.Name = "deleteJobButton";
            this.deleteJobButton.Size = new System.Drawing.Size(120, 30);
            this.deleteJobButton.TabIndex = 3;
            this.deleteJobButton.Text = "Delete Job";
            this.deleteJobButton.UseVisualStyleBackColor = true;
            // 
            // jobsCheckedListBox
            // 
            this.jobsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.jobsCheckedListBox.FormattingEnabled = true;
            this.jobsCheckedListBox.Location = new System.Drawing.Point(0, 10);
            this.jobsCheckedListBox.Name = "jobsCheckedListBox";
            this.jobsCheckedListBox.Size = new System.Drawing.Size(120, 304);
            this.jobsCheckedListBox.TabIndex = 1;
            this.jobsCheckedListBox.SelectedValueChanged += new System.EventHandler(this.jobsCheckedListBox_SelectedValueChanged);
            // 
            // syncInfoBindingSource
            // 
            this.syncInfoBindingSource.DataSource = typeof(FlagSync2.Core.SyncInfo);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 404);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "FlagSync";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.overviewTabPage.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.configurationTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.syncInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage overviewTabPage;
        private System.Windows.Forms.TabPage configurationTabPage;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox jobsCheckedListBox;
        private System.Windows.Forms.Panel jobConfigurationPanel;
        private System.Windows.Forms.Button deleteJobButton;
        private System.Windows.Forms.Button newSnchronizationButton;
        private System.Windows.Forms.Button newSynchronizationButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.BindingSource syncInfoBindingSource;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderA;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderB;
        private System.Windows.Forms.Panel panel1;
    }
}

