using PLTextToolTXT.Common;
using PLTextToolTXT.FileType.PLG;
using PLTextToolTXT.Utils;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PLTextToolTXT.Decode
{
    public class DecodePLG : Decode
    {
        public override int CheckFile(string filePath, string folderOutputPath="")
        {
            // Check other folder
            folderOutputPath = string.IsNullOrWhiteSpace(folderOutputPath) ? Path.GetDirectoryName(filePath) : folderOutputPath;

            string outPuthFileName = string.Format("{0}\\{1}{2}", folderOutputPath, Path.GetFileName(filePath), Constant.ExtensionOutPut);
            if (!File.Exists(filePath))
            {
                DisplayMessage(string.Format("PLGname[{0}] Not Exist\n", filePath));
                return 0;
            }

            try
            {
                byte[] data = File.ReadAllBytes(filePath);
                if (data.Length == 0)
                {
                    DisplayMessage(string.Format("PLGname[{0}] Open Error\n", filePath));
                    return 0;
                }

                Constant.EXT_PLType = "G";

                DecodeData(outPuthFileName, data);

                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public override int DecodeData(string outPuthFileName, byte[] data)
        {
            try
            {
                LogWriter.SetToBuff(outPuthFileName, "//PLG//", true);
                LogWriter.SetToBuff(outPuthFileName, "/分割管理ヘッダ/");

                PLGUtil.DecodeHeader(outPuthFileName, data);

                uint offSet = (uint)Marshal.SizeOf(typeof(PLGHeaderStruct));
                PLGHeaderStruct plgheader = ByteUtil.BytesToType<PLGHeaderStruct>(data);

                // Decode plg_divide_data_t
                PLGUtil.DecodeDivideData(offSet, data, plgheader.divide_count, outPuthFileName);

                // Decode DecodeExtension
                PLGUtil.DecodeExtension(offSet, data, plgheader.divide_count, outPuthFileName);

                // End file must call function
                LogWriter.Write(outPuthFileName, true);
            }
            catch 
            {
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
