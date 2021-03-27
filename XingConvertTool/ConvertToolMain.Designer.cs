namespace PLTextTool
{
    partial class ConvertToolMain
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertToolMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblOuputNotif = new System.Windows.Forms.Label();
            this.lblInputNotif = new System.Windows.Forms.Label();
            this.btnOutputFolder = new System.Windows.Forms.Button();
            this.btnInputFolder = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.btnConverting = new System.Windows.Forms.Button();
            this.prgConverting = new System.Windows.Forms.ProgressBar();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.chkTsvOutput = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lblOuputNotif);
            this.panel1.Controls.Add(this.lblInputNotif);
            this.panel1.Controls.Add(this.btnOutputFolder);
            this.panel1.Controls.Add(this.btnInputFolder);
            this.panel1.Controls.Add(this.txtOutputFolder);
            this.panel1.Controls.Add(this.txtInputFolder);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 61);
            this.panel1.TabIndex = 2;
            // 
            // lblOuputNotif
            // 
            this.lblOuputNotif.AutoSize = true;
            this.lblOuputNotif.BackColor = System.Drawing.Color.Transparent;
            this.lblOuputNotif.Location = new System.Drawing.Point(3, 42);
            this.lblOuputNotif.Name = "lblOuputNotif";
            this.lblOuputNotif.Size = new System.Drawing.Size(171, 13);
            this.lblOuputNotif.TabIndex = 7;
            this.lblOuputNotif.Text = " 出力フォルダーを選択してください。";
            this.lblOuputNotif.Click += new System.EventHandler(this.lblOuputNotif_Click);
            // 
            // lblInputNotif
            // 
            this.lblInputNotif.AutoSize = true;
            this.lblInputNotif.BackColor = System.Drawing.Color.Transparent;
            this.lblInputNotif.Location = new System.Drawing.Point(3, 10);
            this.lblInputNotif.Name = "lblInputNotif";
            this.lblInputNotif.Size = new System.Drawing.Size(144, 13);
            this.lblInputNotif.TabIndex = 6;
            this.lblInputNotif.Text = "フォルダーを選択してください。";
            this.lblInputNotif.Click += new System.EventHandler(this.lblInputNotif_Click);
            // 
            // btnOutputFolder
            // 
            this.btnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputFolder.Location = new System.Drawing.Point(485, 36);
            this.btnOutputFolder.Name = "btnOutputFolder";
            this.btnOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnOutputFolder.TabIndex = 3;
            this.btnOutputFolder.Text = "出力先選択 ";
            this.btnOutputFolder.UseVisualStyleBackColor = true;
            this.btnOutputFolder.Click += new System.EventHandler(this.btnOutputFolder_Click);
            // 
            // btnInputFolder
            // 
            this.btnInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInputFolder.Location = new System.Drawing.Point(485, 4);
            this.btnInputFolder.Name = "btnInputFolder";
            this.btnInputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnInputFolder.TabIndex = 1;
            this.btnInputFolder.Text = "ソース選択";
            this.btnInputFolder.UseVisualStyleBackColor = true;
            this.btnInputFolder.Click += new System.EventHandler(this.btnInputFolder_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtOutputFolder.Location = new System.Drawing.Point(0, 39);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(479, 20);
            this.txtOutputFolder.TabIndex = 2;
            this.txtOutputFolder.Click += new System.EventHandler(this.txtOutputFolder_Click);
            this.txtOutputFolder.TextChanged += new System.EventHandler(this.txtOutputFolder_TextChanged);
            this.txtOutputFolder.Leave += new System.EventHandler(this.txtOutputFolder_Leave);
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFolder.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtInputFolder.Location = new System.Drawing.Point(0, 7);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(479, 20);
            this.txtInputFolder.TabIndex = 0;
            this.txtInputFolder.Click += new System.EventHandler(this.txtInputFolder_Click);
            this.txtInputFolder.TextChanged += new System.EventHandler(this.txtInputFolder_TextChanged);
            this.txtInputFolder.Leave += new System.EventHandler(this.txtInputFolder_Leave);
            // 
            // btnConverting
            // 
            this.btnConverting.Location = new System.Drawing.Point(12, 82);
            this.btnConverting.Name = "btnConverting";
            this.btnConverting.Size = new System.Drawing.Size(94, 23);
            this.btnConverting.TabIndex = 4;
            this.btnConverting.Text = "変換";
            this.btnConverting.UseVisualStyleBackColor = true;
            this.btnConverting.Click += new System.EventHandler(this.btnConverting_Click);
            // 
            // prgConverting
            // 
            this.prgConverting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgConverting.Location = new System.Drawing.Point(12, 118);
            this.prgConverting.Name = "prgConverting";
            this.prgConverting.Size = new System.Drawing.Size(560, 27);
            this.prgConverting.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgConverting.TabIndex = 4;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(126, 82);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(99, 23);
            this.btnOpenFile.TabIndex = 5;
            this.btnOpenFile.Text = "結果確認";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Visible = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // chkTsvOutput
            // 
            this.chkTsvOutput.AutoSize = true;
            this.chkTsvOutput.Location = new System.Drawing.Point(497, 82);
            this.chkTsvOutput.Name = "chkTsvOutput";
            this.chkTsvOutput.Size = new System.Drawing.Size(71, 17);
            this.chkTsvOutput.TabIndex = 6;
            this.chkTsvOutput.Text = "TSV出力";
            this.chkTsvOutput.UseVisualStyleBackColor = true;
            this.chkTsvOutput.CheckedChanged += new System.EventHandler(this.chkTsvOutput_CheckedChanged);
            // 
            // ConvertToolMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(584, 155);
            this.Controls.Add(this.chkTsvOutput);
            this.Controls.Add(this.btnConverting);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.prgConverting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConvertToolMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLテキスト化ツール";
            this.Load += new System.EventHandler(this.ConvertToolMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOutputFolder;
        private System.Windows.Forms.Button btnInputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Button btnConverting;
        private System.Windows.Forms.ProgressBar prgConverting;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Label lblOuputNotif;
        private System.Windows.Forms.Label lblInputNotif;
        private System.Windows.Forms.CheckBox chkTsvOutput;
    }
}

