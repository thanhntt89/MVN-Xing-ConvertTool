using PLTextToolTXT.Common;
using PLTextToolTXT.Decode;
using PLTextToolTXT.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PLTextTool
{
    public partial class ConvertToolMain : Form
    {
        private DecodeManagement decodeManagement = new DecodeManagement();
        private PLTextToolTSV.Decode.DecodeManagement decodeManagementTsv = new PLTextToolTSV.Decode.DecodeManagement();

        private List<string> listFile = new List<string>();
        private Thread threadDecode;

        public ConvertToolMain()
        {
            InitializeComponent();
            decodeManagement.ProcessPercentEvent += ProcessPercent;
            decodeManagementTsv.ProcessPercentEvent += ProcessPercent;
            this.Text = string.Format("{0} - Ver {1}",this.Text,Constant.VERSION);
        }

        private void ConvertToolMain_Load(object sender, EventArgs e)
        {
            txtInputFolder.Text = Utilities.LoadSetting(Constant.XingRegistryKey, Constant.LastFolderInput);
            txtOutputFolder.Text = Utilities.LoadSetting(Constant.XingRegistryKey, Constant.LastFolderOutput);
            //chkTsvOutput.Checked = Utilities.LoadSetting(Constant.XingRegistryKey, Constant.ExtentionOuput).ToLower().Equals(Constant.EXTENSION_TSV);
            LoadInit();
        }

        private void LoadInit()
        {
            int heigh = btnConverting.Location.Y + 3 * btnConverting.Height + 5;
            this.Size = new Size(600, heigh);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = string.IsNullOrWhiteSpace(txtOutputFolder.Text) ? txtInputFolder.Text : txtOutputFolder.Text;
            openFileDialog.Filter = chkTsvOutput.Checked? Constant.FilterFileOpenTsv : Constant.FilterFileOpenTxt;
            openFileDialog.ShowDialog();
            if (!File.Exists(openFileDialog.FileName))
                return;
            DisplayText display = new DisplayText();
            display.DisplayData(openFileDialog.FileName);
            display.ShowDialog();
        }

        private void btnInputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = btnInputFolder.Text;
            folderBrowser.SelectedPath = txtInputFolder.Text;
            folderBrowser.ShowDialog();
            txtInputFolder.Text = folderBrowser.SelectedPath;
            lblInputNotif.Visible = false;
        }

        private void btnOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = btnOutputFolder.Text;
            folderBrowser.SelectedPath = txtOutputFolder.Text;
            folderBrowser.ShowDialog();
            txtOutputFolder.Text = folderBrowser.SelectedPath;
            lblOuputNotif.Visible = false;
        }

        private void btnConverting_Click(object sender, EventArgs e)
        {
            Converting();
        }

        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtInputFolder.Text))
            {
                MessageBox.Show("フォルダーを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInputFolder.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtInputFolder.Text))
            {
                MessageBox.Show("フォルダーを選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInputFolder.Focus();
                return false;
            }

            // 
            if (!Directory.Exists(txtInputFolder.Text))
            {
                MessageBox.Show("パスは存在していません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInputFolder.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(txtOutputFolder.Text) && !Directory.Exists(txtOutputFolder.Text))
            {
                MessageBox.Show("パスは存在していません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOutputFolder.Focus();
                return false;
            }


            listFile = Utilities.GetAllFiles(txtInputFolder.Text, Constant.EXTENSIONS);

            if (listFile.Count == 0)
            {
                MessageBox.Show("指定されたフォルダーに復号化ファイルが含まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnOutputFolder.Focus();
                return false;
            }

            prgConverting.Maximum = listFile.Count;
            int heigh = btnConverting.Location.Y + btnConverting.Height + 3 * prgConverting.Size.Height + 10;
            this.Size = new Size(600, heigh);
            return true;
        }

        private void Converting()
        {
            if (!Valid())
                return;
            btnConverting.Enabled = false;
            btnOpenFile.Visible = false;
            btnInputFolder.Enabled = false;
            btnOutputFolder.Enabled = false;
            txtInputFolder.ReadOnly = true;
            txtOutputFolder.ReadOnly = true;
            chkTsvOutput.Enabled = false;

            Utilities.SaveSetting(Constant.XingRegistryKey, Constant.LastFolderInput, txtInputFolder.Text);
            Utilities.SaveSetting(Constant.XingRegistryKey, Constant.LastFolderOutput, txtOutputFolder.Text);

            if (threadDecode == null)
                threadDecode = new Thread(ThreadDecode);
            threadDecode.Start();
        }

        private void ThreadDecode()
        {
            int result;

            if (chkTsvOutput.Checked)
                result = decodeManagementTsv.Decode(listFile, txtOutputFolder.Text);
            else
                result = decodeManagement.Decode(listFile, txtOutputFolder.Text);


            string resultMessage = "";
            string resultTitle = "";
            MessageBoxIcon mesIcon = MessageBoxIcon.Exclamation;

            if (result == -1)
            {
                resultTitle = "注意！";
                resultMessage = "フォルダーの指定がありません。";
            }
            if (result == -2)
            {
                resultTitle = "注意！";
                resultMessage = "指定されたフォルダーがありません。";
            }
            else if (result < 0)
            {
                result *= -1;
                resultTitle = "注意！";
                resultMessage = String.Format("{0}ファイルの出力を完了しました。\nエラーがありました！", result);
            }
            else if (result > 0x10000)
            {
                resultTitle = "注意！";
                resultMessage = String.Format("{0}ファイルの出力を完了しました。\n{1}ファイルがエラーでした。", result*0xffff, result>>16);
            }
            else
            {
                resultTitle = "結果";
                resultMessage = String.Format("{0}ファイルの出力を完了しました。", result);
                mesIcon = MessageBoxIcon.None;
            }

            Invoke(new Action(() =>
            {
                btnOpenFile.Visible = true;
                LoadInit();
                btnConverting.Enabled = true;
                btnInputFolder.Enabled = true;
                btnOutputFolder.Enabled = true;
                txtInputFolder.ReadOnly = false;
                txtOutputFolder.ReadOnly = false;
                chkTsvOutput.Enabled = true;
                MessageBox.Show(resultMessage, resultTitle, MessageBoxButtons.OK, mesIcon);
            }));

            threadDecode.Interrupt();
            threadDecode = null;
        }

        private void ProcessPercent(int proceeValue)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (proceeValue <= prgConverting.Maximum)
                        prgConverting.Value = proceeValue;
                }));

            }
            catch
            {

            }
        }

        private void lblInputNotif_Click(object sender, EventArgs e)
        {
            lblInputNotif.Visible = false;
        }

        private void lblOuputNotif_Click(object sender, EventArgs e)
        {
            lblOuputNotif.Visible = false;
        }

        private void txtInputFolder_Click(object sender, EventArgs e)
        {
            lblInputNotif_Click(sender, e);
        }

        private void txtOutputFolder_TextChanged(object sender, EventArgs e)
        {
            lblOuputNotif_Click(sender, e);
        }

        private void txtInputFolder_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInputFolder.Text))
                lblInputNotif.Visible = true;
        }

        private void txtOutputFolder_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOutputFolder.Text))
                lblOuputNotif.Visible = true;
        }

        private void txtInputFolder_TextChanged(object sender, EventArgs e)
        {
            lblInputNotif_Click(sender, e);
        }

        private void txtOutputFolder_Click(object sender, EventArgs e)
        {
            lblOuputNotif_Click(sender, e);
        }

        private void chkTsvOutput_CheckedChanged(object sender, EventArgs e)
        {
            //string extiontOuput = Constant.EXTENSION_TXT;
            //if (chkTsvOutput.Checked)
            //   extiontOuput = Constant.EXTENSION_TSV;
            //Utilities.SaveSetting(Constant.XingRegistryKey, Constant.ExtentionOuput, extiontOuput);
        }
    }
}
