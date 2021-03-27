namespace PLTextTool
{
    partial class DisplayText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayText));
            this.txtRsult = new System.Windows.Forms.RichTextBox();
            this.btnClosed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRsult
            // 
            this.txtRsult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRsult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtRsult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRsult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRsult.Location = new System.Drawing.Point(12, 11);
            this.txtRsult.Name = "txtRsult";
            this.txtRsult.ReadOnly = true;
            this.txtRsult.Size = new System.Drawing.Size(660, 289);
            this.txtRsult.TabIndex = 0;
            this.txtRsult.Text = "";
            this.txtRsult.WordWrap = false;
            // 
            // btnClosed
            // 
            this.btnClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClosed.BackColor = System.Drawing.SystemColors.Control;
            this.btnClosed.Location = new System.Drawing.Point(597, 306);
            this.btnClosed.Name = "btnClosed";
            this.btnClosed.Size = new System.Drawing.Size(75, 21);
            this.btnClosed.TabIndex = 1;
            this.btnClosed.Text = "戻る";
            this.btnClosed.UseVisualStyleBackColor = false;
            this.btnClosed.Click += new System.EventHandler(this.btnClosed_Click);
            // 
            // DisplayText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(684, 333);
            this.Controls.Add(this.btnClosed);
            this.Controls.Add(this.txtRsult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DisplayText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DisplayText";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtRsult;
        private System.Windows.Forms.Button btnClosed;
    }
}