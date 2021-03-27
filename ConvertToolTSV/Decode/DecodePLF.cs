using PLTextToolTSV.Common;
using PLTextToolTSV.FileType.PLF;
using PLTextToolTSV.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PLTextToolTSV.Decode
{
    public class DecodePLF : Decode
    {
        public override int CheckFile(string filePath, string folderOutputPath = "")
        {
            // Check other folder
            folderOutputPath = string.IsNullOrWhiteSpace(folderOutputPath) ? Path.GetDirectoryName(filePath) : folderOutputPath;

            string outPuthFileName = string.Format("{0}\\{1}{2}", folderOutputPath, Path.GetFileName(filePath), Constant.ExtensionOutPut);

            if (!File.Exists(filePath))
            {
                DisplayMessage(string.Format("PLFname[{0}] Not Exist\n", filePath));
                return 0;
            }

            try
            {
                byte[] data = File.ReadAllBytes(filePath);

                if (data.Length == 0)
                {
                    DisplayMessage(string.Format("PLFname[{0}] Open Error\n", filePath));
                    return 0;
                }

                Constant.EXT_PLType = "F";

                DecodeData(outPuthFileName, data);

                return 1;
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
                return 0;
            }
        }

        public override int DecodeData(string outPuthFileName, byte[] data)
        {
            // Reading file plf header 
            try
            {
                LogWriter.SetToBuff(outPuthFileName, "//PLF//", true);
                LogWriter.SetToBuff(outPuthFileName, "/分割管理ヘッダ/");
                // Decode Header
                PLFUtil.DecodeHeader(outPuthFileName, data);

                PLFHeaderStruct plfheader = ByteUtil.BytesToType<PLFHeaderStruct>(data);
                uint offSet = (uint)Marshal.SizeOf(typeof(PLFHeaderStruct));

                // Decode plf_divide_data_t
                PLFUtil.DecodeDivideData(offSet, data, plfheader.divide_count, outPuthFileName);

                // Decode DecodeExtension
                PLFUtil.DecodeExtension(offSet, data, plfheader.divide_count, outPuthFileName);

                // End file must call function
                LogWriter.Write(outPuthFileName, true);
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
                return 0;
            }

            return 1;
        }

        public override void DisplayMessage(string message)
        {
            base.DisplayMessage(message);
        }
    }
}
