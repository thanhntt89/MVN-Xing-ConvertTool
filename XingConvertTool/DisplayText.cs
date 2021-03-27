using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PLTextTool
{
    public partial class DisplayText : Form
    {
        public DisplayText()
        {
            InitializeComponent();
        }

        public void DisplayData(string filePath)
        {
            try
            {
                this.Text = string.Format("{0}: {1}", "結果ファイル", Path.GetFileName(filePath));
                 txtRsult.Text = File.ReadAllText(filePath, Encoding.Default);                
            }
            catch 
            {

            }
        }

        private void btnClosed_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
