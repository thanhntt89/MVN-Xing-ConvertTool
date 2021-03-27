namespace checkTSV
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnFolder = new System.Windows.Forms.Button();
            this.TBoxFolder = new System.Windows.Forms.TextBox();
            this.TBoxTarget = new System.Windows.Forms.TextBox();
            this.BtnStart = new System.Windows.Forms.Button();
            this.RTBtxt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BtnFolder
            // 
            this.BtnFolder.Location = new System.Drawing.Point(16, 14);
            this.BtnFolder.Name = "BtnFolder";
            this.BtnFolder.Size = new System.Drawing.Size(50, 23);
            this.BtnFolder.TabIndex = 0;
            this.BtnFolder.Text = "Folder";
            this.BtnFolder.UseVisualStyleBackColor = true;
            this.BtnFolder.Click += new System.EventHandler(this.BtnFolder_Click);
            // 
            // TBoxFolder
            // 
            this.TBoxFolder.Location = new System.Drawing.Point(72, 17);
            this.TBoxFolder.Name = "TBoxFolder";
            this.TBoxFolder.Size = new System.Drawing.Size(701, 19);
            this.TBoxFolder.TabIndex = 1;
            this.TBoxFolder.Text = "";
            // 
            // TBoxTarget
            // 
            this.TBoxTarget.Location = new System.Drawing.Point(72, 42);
            this.TBoxTarget.Name = "TBoxTarget";
            this.TBoxTarget.Size = new System.Drawing.Size(229, 19);
            this.TBoxTarget.TabIndex = 1;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(16, 40);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(50, 23);
            this.BtnStart.TabIndex = 4;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // RTBtxt
            // 
            this.RTBtxt.Location = new System.Drawing.Point(18, 71);
            this.RTBtxt.Name = "RTBtxt";
            this.RTBtxt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.RTBtxt.Size = new System.Drawing.Size(755, 370);
            this.RTBtxt.TabIndex = 7;
            this.RTBtxt.Text = "";
            this.RTBtxt.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 450);
            this.Controls.Add(this.RTBtxt);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.TBoxTarget);
            this.Controls.Add(this.TBoxFolder);
            this.Controls.Add(this.BtnFolder);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnFolder;
        private System.Windows.Forms.TextBox TBoxFolder;
        private System.Windows.Forms.TextBox TBoxTarget;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.RichTextBox RTBtxt;
    }
}

