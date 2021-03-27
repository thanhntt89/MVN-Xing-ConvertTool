using PLTextToolTXT.Common;
using PLTextToolTXT.Decode;
using PLTextToolTXT.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace PLTextToolCommand
{
    class Program
    {
        static DecodeManagement decodeManagement = new DecodeManagement();
        static string dataFolder = string.Empty;
        static int count = 0;

        static int Main(string[] args)
        {

            decodeManagement.displayMessageEvent += DisplayMessage;

            DisplayMessage(string.Format("楽曲フォーマット解析ツール Ver {0}\n", Constant.VERSION));
            DisplayMessage("ConvertToolCommand.exe Dir\n");
#if true // Final
            if (args.Length == 0)
            {
                return -1;
            }

            dataFolder = Path.Combine(Constant.CurrentFolder, args[0]);

            if (!Directory.Exists(dataFolder))
            {
                DisplayMessage(string.Format("Not exist folder path {0}", dataFolder));
                return -2;
            }

            DisplayMessage(string.Format("Folder path {0}", dataFolder));
            List<string> listFile = Utilities.GetAllFiles(dataFolder, Constant.EXTENSIONS);
            DisplayMessage(string.Format("Total count=[{0}]\n", listFile.Count));
            DisplayMessage("Started converting:\n");
            count = decodeManagement.Decode(listFile);

#else  // Debug     

            dataFolder = Path.Combine(Constant.CurrentFolder, "data");
            DisplayMessage(string.Format("Folder path {0}", dataFolder));
            List<string> listFile = Utilities.GetAllFiles(dataFolder, Constant.EXTENSIONS);
            DisplayMessage(string.Format("Total count=[{0}]\n", listFile.Count));
            DisplayMessage("Started converting:\n");
            count = decodeManagement.Decode(listFile);
#endif
            DisplayMessage(string.Format("Total converted: {0}", count));
            return count;
        }


        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
