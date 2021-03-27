using System;
using System.IO;
using System.Text;

namespace PLTextToolTSV
{
    public class LogWriter
    {
        private static StringBuilder stringbuilder = new StringBuilder();

        public static void Write(ErrorEntity error)
        {
            SetToBuff(error.FilePath, error.GetLogInfo());
        }

        public static void Write(byte[] data, string filePath)
        {
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(filePath, FileMode.Append, FileAccess.Write)))
            {
                // Write binary   
                binWriter.Write(data);
            }
        }

        public static void CreateFile(string filePath, bool isDeleteExistFile = false)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return;

                if (File.Exists(filePath) && isDeleteExistFile)
                {
                    File.Delete(filePath);
                    File.Create(filePath).Close();
                }
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Set data to buffer
        /// </summary>
        /// <param name="filePath">File path to save data</param>
        /// <param name="message">Message data</param>
        /// <param name="isNewLine">Default value: True | False (same line)</param>
        public static void SetToBuff(string filePath, string message, bool isNewLine = true)
        {
            if (message != "" && message.Length >= 2 && message.Substring(0,2) == "  ")
            {
                string head = "\t";
                string substr = message.Substring(2);
                while(substr != "" && substr.Substring(0, 1) == " ")
                {
                    head += "\t";
                    substr = substr.Substring(1);
                }
                message = head + substr;
            }
            try
            {               
                if (isNewLine)
                    stringbuilder.AppendLine(message);
                else
                    stringbuilder.Append(message);

                // When data reacher max capacity write to file
                if (stringbuilder.Capacity >= stringbuilder.MaxCapacity)
                {
                    Write(filePath);
                    stringbuilder.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static void WriteLog(string filePath, string message, bool isNewLine = true, bool isDeleteExistFile = false)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return;

                if (File.Exists(filePath) && isDeleteExistFile)
                {
                    File.Delete(filePath);
                    File.Create(filePath).Close();
                }
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                string endCoding = "shift_jis";

                using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.GetEncoding(endCoding)))
                {
                    if (isNewLine)
                        writer.WriteLine(message);
                    else
                        writer.Write(message);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static void Write(string filePath, bool isDeleteExistFile = false)
        {
            try
            {
                CreateFile(filePath, isDeleteExistFile);

                string endCoding = "shift_jis";

                using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(file, Encoding.GetEncoding(endCoding)))
                {
                    writer.Write(stringbuilder.ToString());
                    writer.Close();
                    stringbuilder.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }
    }

    public class ErrorEntity
    {
        public string LogTime { get; set; }
        public string ModuleName { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorNumber { get; set; }
        public string FilePath { get; set; }
        public string GetLogInfo()
        {
            return string.Format("***************************************************************\n【発生日時】 {0}\n【発生箇所】 {1}\n【障害内容】 {2}\n", this.LogTime, this.ModuleName, this.ErrorMessage);
        }
    }
}
