using System;
using System.Collections.Generic;
using System.IO;

namespace PLTextToolTSV.Common
{
    public class Constant
    {
        public static int Sleep = 0;
        public static HashSet<string> EXTENSIONS = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase) { EXTENSION_PLB, EXTENSION_PLC, EXTENSION_PLD, EXTENSION_PLE, EXTENSION_PLF, EXTENSION_PLG, EXTENSION_PL8, EXTENSION_MI3 };

        public const string EXTENSION_PLB = ".plb";
        public const string EXTENSION_PLC = ".plc";
        public const string EXTENSION_PLD = ".pld";
        public const string EXTENSION_PLE = ".ple";
        public const string EXTENSION_PLF = ".plf";
        public const string EXTENSION_PLG = ".plg";
        public const string EXTENSION_PL8 = ".pl8";
        public const string EXTENSION_MI3 = ".mi3";

        public const string XingRegistryKey = @"Software\XingConvertToolTSV";
        public const string ExtentionOuput = "ExtentionOuput";
        public const string LastFolderInput = "LastFolderInput";
        public const string LastFolderOutput = "LastFolderOutput";

        // Out put file extention
        public const string EXTENSION_TXT = ".txt";
        public const string EXTENSION_TSV = ".tsv";
        public const string FilterFileOpen = ".tsv|*.tsv|txt|*.txt";
        public const string FilterFileOpenTxt = "txt|*.txt";
        public const string FilterFileOpenTsv = "tsv|*.tsv";

        public static string ExtensionOutPut
        {
            get
            {
                return EXTENSION_TSV;

                //string extension = Utilities.LoadSetting(XingRegistryKey, ExtentionOuput).ToLower();
                //extension = string.IsNullOrWhiteSpace(extension) ? EXTENSION_TSV : extension;
                //return extension;
            }
        }
        public const byte DEF_TB48 = 0xc2;
        public const byte DEF_TB480 = 0xd0;

        public static string CurrentFolder = Directory.GetCurrentDirectory();

        public static string TMP_PLF_FILE_PATH = Path.Combine(Path.GetTempPath(), "PL_Decode.ntt");
        public static string LTP_SOURCE_FILE_PATH = Path.Combine(Path.GetTempPath(), "PL_Src.ntt");

        public static string LogFilePath = Directory.GetCurrentDirectory() + "\\log.txt";


        public static string EXT_PLType { get; set; }
        public static byte EXT_TB { get; set; }
        public static uint EXT_Total_Tick { get; set; }
        public static bool IsTsvOutPutBoolen { get; set; }
    }

}
