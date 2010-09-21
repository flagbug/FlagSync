namespace FlagSync2.View
{
    partial class BackupConfigurationPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nameLabel = new System.Windows.Forms.Label();
            this.sourceFolderButton = new System.Windows.Forms.Button();
            this.targetDirectoryButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.sourceDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.targetDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 40);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(100, 23);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sourceFolderButton
            // 
            this.sourceFolderButton.Location = new System.Drawing.Point(0, 70);
            this.sourceFolderButton.Name = "sourceFolderButton";
            this.sourceFolderButton.Size = new System.Drawing.Size(110, 40);
            this.sourceFolderButton.TabIndex = 1;
            this.sourceFolderButton.Text = "Source directory";
            this.sourceFolderButton.UseVisualStyleBackColor = true;
            this.sourceFolderButton.Click += new System.EventHandler(this.sourceFolderButton_Click);
            // 
            // targetDirectoryButton
            // 
            this.targetDirectoryButton.Location = new System.Drawing.Point(0, 110);
            this.targetDirectoryButton.Name = "targetDirectoryButton";
            this.targetDirectoryButton.Size = new System.Drawing.Size(110, 40);
            this.targetDirectoryButton.TabIndex = 2;
            this.targetDirectoryButton.Text = "Target directory";
            this.targetDirectoryButton.UseVisualStyleBackColor = true;
            this.targetDirectoryButton.Click += new System.EventHandler(this.targetDirectoryButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(120, 40);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(190, 20);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // sourceDirectoryTextBox
            // 
            this.sourceDirectoryTextBox.Location = new System.Drawing.Point(120, 80);
            this.sourceDirectoryTextBox.Name = "sourceDirectoryTextBox";
            this.sourceDirectoryTextBox.Size = new System.Drawing.Size(190, 20);
            this.sourceDirectoryTextBox.TabIndex = 4;
            this.sourceDirectoryTextBox.TextChanged += new System.EventHandler(this.sourceDirectoryTextBox_TextChanged);
            // 
            // targetDirectoryTextBox
            // 
            this.targetDirectoryTextBox.Location = new System.Drawing.Point(120, 120);
            this.targetDirectoryTextBox.Multiline = true;
            this.targetDirectoryTextBox.Name = "targetDirectoryTextBox";
            this.targetDirectoryTextBox.Size = new System.Drawing.Size(190, 20);
            this.targetDirectoryTextBox.TabIndex = 5;
            this.targetDirectoryTextBox.TextChanged += new System.EventHandler(this.targetDirectoryTextBox_TextChanged);
            // 
            // typeLabel
            // 
            this.typeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.typeLabel.Location = new System.Drawing.Point(0, 0);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(361, 23);
            this.typeLabel.TabIndex = 6;
            this.typeLabel.Text = "Backup";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BackupConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.targetDirectoryTextBox);
            this.Controls.Add(this.sourceDirectoryTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.targetDirectoryButton);
            this.Controls.Add(this.sourceFolderButton);
            this.Controls.Add(this.nameLabel);
            this.Name = "BackupConfigurationPanel";
            this.Size = new System.Drawing.Size(361, 174);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Button sourceFolderButton;
        private System.Windows.Forms.Button targetDirectoryButton;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox sourceDirectoryTextBox;
        private System.Windows.Forms.TextBox targetDirectoryTextBox;
        private System.Windows.Forms.Label typeLabel;
    }
}
