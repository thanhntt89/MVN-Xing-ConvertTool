using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace checkTSV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "処理するフォルダを指定します";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            if (TBoxFolder.Text == "")
            {
                fbd.SelectedPath = @"C:\Windows";
            }
            else
            {
                fbd.SelectedPath = TBoxFolder.Text;
            }
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                TBoxFolder.Text = fbd.SelectedPath;
            }
        }

        private void errorOut(int xline, int vline, string xstr, string vstr)
        {
            RTBtxt.AppendText("■" + TBoxTarget.Text +"\n");
            RTBtxt.AppendText(String.Format("　TXT line{0}\t{1}\n", xline+1, xstr));
            RTBtxt.AppendText(String.Format("　TSV line{0}\t{1}\n", vline+1, vstr));
            RTBtxt.Refresh();
        }
        
        private void Compare(List<string> strTxt, List<string> strTsv)
        {
            int Ltxt;
            int Ltsv;
            string txt;
            string tsv;
            string[] arrtxt;
            string[] arrtsv;

            for (Ltxt = 0, Ltsv = 0; Ltsv < strTsv.Count; )
            {
                if (strTsv[Ltsv].Substring(0,1) == "/" | strTsv[Ltsv].Substring(0, 2) == " /")
                {
                    // 大項目区切り
                    txt = strTxt[Ltxt].TrimStart();
                    tsv = strTsv[Ltsv].TrimStart();
                    arrtxt = txt.Split(':');
                    arrtsv = tsv.Split('\t');
                    if (arrtxt[0] != arrtsv[0])
                    {
                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                        return;
                    }
                    ++Ltsv;
                    ++Ltxt;
                }
                else
                {
                    txt = strTxt[Ltxt].TrimStart();  // 先頭の空白を削除
                    tsv = strTsv[Ltsv].TrimStart();  // 先頭の空白を削除
                    arrtxt = txt.Split(':');       // 項目名と値を分離
                    arrtsv = tsv.Split('\t');      // 項目名と値を分離
                    if (tsv.Contains("/Block文字列Data"))
                    {
                        if (!txt.Contains("/Block文字列Data"))
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                            return;
                        }

                        Ltsv += 2; // メニュー行スキップ
                        tsv = strTsv[Ltsv].TrimStart();
                        arrtsv = tsv.Split('\t');
                        // Block文字列Data部終了まで繰り返し
                        while (!strTxt[Ltxt].Contains("/Block表示情報Data"))
                        {
                            // Block文字列Data部番号処理
                            int tsvclm = 0;
                            string numstr = arrtxt[0].Substring(14, arrtxt[0].Length - 15);
                            if (numstr != arrtsv[tsvclm++])
                            {
                                errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                return;
                            }
                            ++Ltxt;
                            txt = strTxt[Ltxt].TrimStart();
                            arrtxt = txt.Split(':');
                            while (!strTxt[Ltxt].Contains("/Block文字列Data") && !strTxt[Ltxt].Contains("/Block表示情報Data"))
                            {
                                // 各項目処理

                                if (arrtxt[0].Contains("文字コード"))
                                {
                                    List<string> arrcode = new List<string>();
                                    string code = arrtsv[tsvclm++];
                                    int codecount = 0;
                                    while (codecount < code.Length)                                    {
                                        arrcode.Add(code.Substring(codecount,7));
                                        codecount += 7;
                                    }
                                    codecount = 0;
                                    while (codecount < arrcode.Count && arrtxt[0].Contains("文字コード"))
                                    {

                                        if (arrtxt[1] != arrcode[codecount++])
                                        {
                                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                            return;
                                        }
                                        ++Ltxt;
                                        txt = strTxt[Ltxt].TrimStart();
                                        arrtxt = txt.Split(':');
                                    }
                                }
                                else if (arrtxt[0].Contains("ルビBlock") && arrtxt[0].Contains("文字数"))
                                {
                                    string[] blk;
                                    string str = "";
                                    while (arrtxt[0].Contains("ルビBlock") && arrtxt[0].Contains("文字数"))
                                    {
                                        blk = arrtxt[0].Split(' ');
                                        str += blk[0].Substring(2, blk[0].Length - 2) + "文字数=" + arrtxt[1] + " ";
                                        ++Ltxt;
                                        txt = strTxt[Ltxt].TrimStart();
                                        arrtxt = txt.Split(':');
                                    }
                                    if (str != arrtsv[tsvclm++])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                }
                                else if (arrtxt[0].Contains("ルビ文字列Data"))
                                {
                                    while (arrtxt[0].Contains("ルビ文字列Data"))
                                    {
                                        string str = "ルビ文字列" + txt.Substring(10, txt.Length - 11) + ":";
                                        ++Ltxt;
                                        txt = strTxt[Ltxt].TrimStart();
                                        arrtxt = txt.Split(':');
                                        str += "相対Ｘ座標=" + arrtxt[1] + "/ルビ文字code=";
                                        ++Ltxt;
                                        txt = strTxt[Ltxt].TrimStart();
                                        arrtxt = txt.Split(':');
                                        while (arrtxt[0].Contains("ルビ文字code"))
                                        {
                                            str += arrtxt[1];
                                            ++Ltxt;
                                            txt = strTxt[Ltxt].TrimStart();
                                            arrtxt = txt.Split(':');
                                        }
                                        if (str != arrtsv[tsvclm++])
                                        {
                                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (arrtxt[1] != arrtsv[tsvclm++])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                    ++Ltxt;
                                    txt = strTxt[Ltxt].TrimStart();
                                    arrtxt = txt.Split(':');
                                }
                            }
                            ++Ltsv;
                            tsv = strTsv[Ltsv].TrimStart();
                            arrtsv = tsv.Split('\t');
                        }
                    }
                    else if (tsv.Contains("/Block表示情報Data"))
                    {
                        if (!txt.Contains("/Block表示情報Data"))
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                            return;
                        }

                        ++Ltxt; ++Ltsv;
                        txt = strTxt[Ltxt].TrimStart();
                        tsv = strTsv[Ltsv].TrimStart();
                        arrtxt = txt.Split(':');
                        arrtsv = tsv.Split('\t');

                        // Block数処理
                        if (arrtxt[1] != arrtsv[1])
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                            return;
                        }

                        ++Ltxt; ++Ltsv;

                        // Block表示情報Data部終了まで繰り返し
                        Ltsv += 1; // メニュー行スキップ
                        while (strTxt[Ltxt].Contains("/表示タイミング情報"))
                        {
                            int tsvclm = 0;
                            txt = strTxt[Ltxt].TrimStart();
                            tsv = strTsv[Ltsv].TrimStart();
                            arrtxt = txt.Split(':');
                            arrtsv = tsv.Split('\t');

                            // Block文字列Data部番号処理
                            string numstr = arrtxt[0].Substring(10, arrtxt[0].Length - 11);
                            if (numstr != arrtsv[tsvclm++])
                            {
                                errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                                return;
                            }
                            ++Ltxt;
                            txt = strTxt[Ltxt].TrimStart();
                            // 各項目処理
                            while (!strTxt[Ltxt].Contains("/表示タイミング情報") && !strTxt[Ltxt].Contains("/色換速度情報データ"))
                            {
                                string[] arrtxt2 = txt.Split(':');
                                if (arrtxt2[1] != arrtsv[tsvclm++])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                                    return;
                                }
                                ++Ltxt;
                                txt = strTxt[Ltxt].TrimStart();
                            }
                            ++Ltsv;
                        }
                    }
                    else if (tsv.Contains("/色換速度情報データ"))
                    {
                        if (!txt.Contains("/色換速度情報データ"))
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                            return;
                        }

                        ++Ltxt; ++Ltsv;
                        txt = strTxt[Ltxt].TrimStart();
                        tsv = strTsv[Ltsv].TrimStart();
                        arrtxt = txt.Split(':');
                        arrtsv = tsv.Split('\t');

                        // データ数処理
                        if (arrtxt[1] != arrtsv[1])
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                            return;
                        }

                        ++Ltxt; ++Ltsv;

                        // 色換速度情報データ部終了まで繰り返し
                        Ltsv += 1; // メニュー行スキップ
                        while (strTxt[Ltxt].Contains("/色換速度データ"))
                        {
                            int tsvclm = 0;
                            txt = strTxt[Ltxt].TrimStart();
                            tsv = strTsv[Ltsv].TrimStart();
                            arrtxt = txt.Split(':');
                            arrtsv = tsv.Split('\t');

                            // 色換速度情報データ部番号処理
                            string numstr = arrtxt[0].Substring(8, arrtxt[0].Length - 9);
                            if (numstr != arrtsv[tsvclm++])
                            {
                                errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                                return;
                            }
                            ++Ltxt;
                            txt = strTxt[Ltxt].TrimStart();
                            // 各項目処理
                            while (!strTxt[Ltxt].Contains("/色換速度データ") && !strTxt[Ltxt].Contains("/分割データ/"))
                            {
                                string[] arrtxt2 = txt.Split(':');
                                if (arrtxt2[1].Contains("*"))
                                {
                                    // ( )内数値だけ取り出し
                                    string str1 = Regex.Match(arrtxt2[1], @"\(([^)]*)\)").Groups[1].Value;
                                    string str2 = Regex.Match(arrtsv[tsvclm++], @"\(([^)]*)\)").Groups[1].Value;
                                    if (str1 != str2)
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                                        return;
                                    }
                                }
                                else
                                {
                                    if (arrtxt2[1] != arrtsv[tsvclm++])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致

                                        return;
                                    }
                                }
                                ++Ltxt;
                                txt = strTxt[Ltxt].TrimStart();
                            }
                            ++Ltsv;
                        }
                    }
                    else if (tsv.Contains("/Track"))
                    {
                        // Track番号チェック
                        if (txt != tsv)
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                            return;
                        }
                        ++Ltxt; ++Ltsv;

                        // while (!strTxt[Ltxt].Contains("/Track"))
                        while (true)
                        {
                            if (strTxt[Ltxt].Contains("終了<00H>"))
                            {
                                ++Ltxt; ++Ltsv;
                                break;
                            }
                            // メッセージ処理
                            txt = strTxt[Ltxt].TrimStart();  // 先頭の空白を削除
                            tsv = strTsv[Ltsv].TrimStart();  // 先頭の空白を削除
                            arrtxt = txt.Split(':');       // 項目名と値を分離
                            arrtsv = tsv.Split('\t');      // 項目名と値を分離

                            string[] midi = arrtxt[1].Split('\t');
                            string[] midi2 = midi[0].Split(' ');
                            midi[midi.Length - 1] = midi[midi.Length - 1].TrimStart();

                            if (arrtxt[0] != arrtsv[0])
                            {
                                errorOut(Ltxt, Ltsv, txt, tsv); // 不一致
                                return;
                            }

                            if (arrtxt[0].Contains("System-Message"))
                            {
                                if (midi2[0] != arrtsv[1])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                                string msg = "";
                                for (int m = 1; m < midi2.Length; ++m)
                                {
                                    if (midi2[m] == "")
                                    {
                                        continue;
                                    }
                                    msg += midi2[m].Substring(9, 5);
                                }
                                if (msg != arrtsv[2] || midi[1] != arrtsv[3])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                            }
                            else if (arrtxt[0].Contains("同期Data"))
                            {
                                int pos = midi[0].IndexOf("同期データ");
                                string tbyte = midi[0].Substring(0, pos - 1);
                                string value = midi[0].Substring(pos, midi[0].Length - pos);
                                if (arrtxt[0] != arrtsv[0] || tbyte != arrtsv[1] || value != arrtsv[2] || midi[1] != arrtsv[3])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                            }
                            else if (arrtxt[0].Contains("メタData"))
                            {
                                int pos = midi[0].IndexOf("メタデータ");
                                string tbyte = midi[0].Substring(0, pos - 1);
                                string value = midi[0].Substring(pos, midi[0].Length - pos);
                                if (arrtxt[0] != arrtsv[0] || tbyte != arrtsv[1] || value != arrtsv[2] || midi[1] != arrtsv[3])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                            }
                            else
                            {
                                int i;
                                for (i = 0; i < (arrtsv.Length - 2); ++i)
                                {
                                    if (midi2[i] != arrtsv[i + 1])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                }
                                if (midi[1] != arrtsv[i + 1])
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                            }
                            ++Ltxt; ++Ltsv;
                        }
                    }
                    else if (tsv.Contains("/展開情報/"))
                    {
                        if (!txt.Contains("/展開情報/"))
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                            return;
                        }

                        ++Ltxt; ++Ltsv;
                        txt = strTxt[Ltxt].TrimStart();  // 先頭の空白を削除
                        tsv = strTsv[Ltsv].TrimStart();  // 先頭の空白を削除
                        arrtxt = txt.Split(':');       // 項目名と値を分離
                        arrtsv = tsv.Split('\t');      // 項目名と値を分離
                        // レコード数処理
                        if (arrtxt[0] != arrtsv[0] || arrtsv[1] != arrtsv[1])
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                            return;
                        }

                        ++Ltxt; ++Ltsv;
                        Ltsv += 1; // メニュー行スキップ
                        txt = strTxt[Ltxt].TrimStart();  // 先頭の空白を削除
                        tsv = strTsv[Ltsv].TrimStart();  // 先頭の空白を削除
                        arrtxt = txt.Split(':');       // 項目名と値を分離
                        arrtsv = tsv.Split('\t');      // 項目名と値を分離
                        while (txt.Contains("展開情報") && tsv.Contains("展開情報"))
                        {
                            string rec1 = arrtxt[0].Substring(8, arrtxt[0].Length - 8);
                            string rec2 = arrtsv[0].Substring(4, arrtsv[0].Length - 4);
                            string[] pos = arrtxt[1].Split(',');
                            pos[0] = pos[0].Substring(5, pos[0].Length - 5);
                            pos[1] = pos[1].Substring(5, pos[1].Length - 5);
                            if (rec1 != rec2 || pos[0] != arrtsv[1] || pos[1] != arrtsv[2])
                            {
                                errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                return;
                            }
                            ++Ltxt; ++Ltsv;
                            if (Ltxt >= strTxt.Count)
                            {
                                break;
                            }
                            txt = strTxt[Ltxt].TrimStart();  // 先頭の空白を削除
                            tsv = strTsv[Ltsv].TrimStart();  // 先頭の空白を削除
                            arrtxt = txt.Split(':');       // 項目名と値を分離
                            arrtsv = tsv.Split('\t');      // 項目名と値を分離
                        }
                    }
                    else
                    {
                        if (arrtxt[0] != arrtsv[0])
                        {
                            errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                            return;
                        }
                        if (!arrtxt[0].Contains("チャンネル属性"))
                        {
                            if (arrtxt.Length > 1 && (arrtxt[1] != arrtsv[1]))
                            {
                                if (arrtsv[1].Contains("設定無し"))
                                {
                                    arrtsv[1] = arrtsv[1].Substring(0, arrtxt[1].Length);
                                    if (arrtxt[1] != arrtsv[1])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                }
                                else if (arrtxt[0] == "作成日時")
                                {
                                    arrtxt[1] = txt.Substring(5, txt.Length - 5);
                                    if (arrtxt[1] != arrtsv[1])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                }
                                else if (arrtxt[0] == "原曲キー情報")
                                {
                                    arrtxt[1] = txt.Substring(7, txt.Length - 7);
                                    if (arrtxt[1] != arrtsv[1])
                                    {
                                        errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                        return;
                                    }
                                }
                                else
                                {
                                    errorOut(Ltxt, Ltsv, txt, tsv);  // 不一致
                                    return;
                                }
                            }
                        }
                        ++Ltsv;
                        ++Ltxt;
                    }
                }
            }

        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            string[] filesTxt = Directory.GetFiles(@TBoxFolder.Text, "*.txt");
            string[] filesTsv = Directory.GetFiles(@TBoxFolder.Text, "*.tsv");
            // 拡張子を削除
            for (int i = 0; i < filesTxt.Length; ++i)
            {
                filesTxt[i] = filesTxt[i].Substring(0, (filesTxt[i].Length - 4));
            }
            for (int i = 0; i < filesTsv.Length; ++i)
            {
                filesTsv[i] = filesTsv[i].Substring(0, (filesTsv[i].Length - 4));
            }

            RTBtxt.Clear();
            RTBtxt.Refresh();

            // 配列をリストに
            var listTxt = new List<string>();
            listTxt.AddRange(filesTxt);

            // tsvと同名のtxtファイルを探して比較処理
            for (int i = 0; i < filesTsv.Length; ++i)
            {
                if (listTxt.Contains(filesTsv[i]))
                {
                    // 共通ファイルあり
                    TBoxTarget.Text = Path.GetFileName(filesTsv[i]);
                    TBoxTarget.Refresh();

                    string line = "";
                    string path1 = System.IO.Path.Combine(filesTsv[i] + ".txt");
                    string path2 = System.IO.Path.Combine(filesTsv[i] + ".tsv");
                    System.IO.StreamReader sr1 = new System.IO.StreamReader(path1, Encoding.GetEncoding("shift-jis"));
                    List<string> lineTxt = new List<string>();
                    while ((line = sr1.ReadLine()) != null)
                    {
                        lineTxt.Add(line);
                    }
                    System.IO.StreamReader sr2 = new System.IO.StreamReader(path2, Encoding.GetEncoding("shift-jis"));
                    List<string> lineTsv = new List<string>();
                    while ((line = sr2.ReadLine()) != null)
                    {
                        lineTsv.Add(line);
                    }

                    Compare(lineTxt, lineTsv);
                    System.Threading.Thread.Sleep(500);

                }
            }
            string result = String.Format("{0}ファイル完了", listTxt.Count);
            MessageBox.Show(result,
            "",
            MessageBoxButtons.OK,
            MessageBoxIcon.None);
            TBoxTarget.Text = "";
            TBoxTarget.Refresh();
        }
    }
}
