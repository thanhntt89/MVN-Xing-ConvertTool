using Microsoft.Win32;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PLTextToolTXT.Utils
{
    public class Utilities
    {
        public static string LoadSetting(string xingRegistryKey, string key)
        {
            try
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(xingRegistryKey);
                if (rkey != null && rkey.GetValueNames().Contains(key))
                {
                    return rkey.GetValue(key).ToString();
                }
            }
            catch
            {

            }

            return string.Empty;
        }

        public static void SaveSetting(string xingRegistryKey, string key, string values)
        {
            try
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(xingRegistryKey, true);
                if(key == null)
                {
                    rkey.CreateSubKey(xingRegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                rkey.SetValue(key, values, RegistryValueKind.String);
                rkey.Close();
            }
            catch
            {

            }
        }

        public static int OpenFileByNotepad(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return -1;
                }

                var shellType = Type.GetTypeFromProgID("Wscript.Shell");
                dynamic shell = Activator.CreateInstance(shellType);
                //oShell.Run strArgs, 0, false

                var startArgs = new ProcessStartInfo
                {
                    Arguments = filePath,
                    FileName = "notepad.exe",
                    WorkingDirectory = Path.GetDirectoryName(filePath),
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                var shellProcess = Process.Start(startArgs);
                shellProcess.Close();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public static List<string> GetAllFiles(string path, HashSet<string> exts)
        {
            List<string> files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Where(currentFile => exts.Contains(Path.GetExtension(currentFile).ToLower())).ToList();

            return files;
        }


        public static int Decompress(string inFile, string outFile)
        {
            Decoder coder = new Decoder();
            FileStream input = null;
            FileStream output = null;
            try
            {
                input = new FileStream(inFile, FileMode.Open, FileAccess.Read);
                output = new FileStream(outFile, FileMode.Create, FileAccess.Write);

                // Read the decoder properties
                byte[] properties = new byte[5];
                input.Read(properties, 0, 5);

                // Read in the decompress file size.
                byte[] fileLengthBytes = new byte[8];
                input.Read(fileLengthBytes, 0, 8);
                long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

                coder.SetDecoderProperties(properties);
                coder.Code(input, output, input.Length, fileLength, null);

                input.Flush();
                input.Close();
                output.Flush();
                output.Close();
                return 0;
            }
            catch
            {
                input.Flush();
                input.Close();
                output.Flush();
                output.Close();
                return -1;
            }
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
