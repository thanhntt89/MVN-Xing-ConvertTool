using PLTextToolTXT.Common;
using System;
using System.Collections.Generic;
using System.IO;
using static PLTextToolTXT.SystemEvent;

namespace PLTextToolTXT.Decode
{
    public class DecodeManagement
    {

        public DisplayMessageHandle displayMessageEvent;
        public ProcessPercentHandle ProcessPercentEvent;

        private Decode decode = null;

        /// <summary>
        /// Decode main
        /// </summary>
        /// <param name="listFile"></param>
        /// <returns></returns>
        public int Decode(List<string> listFile, string folderOutput = "")
        {
            int count = 0;
            int error = 0;
            int index = 0;
            int rst = 0;
            string fileExtention = string.Empty;

            foreach (string filePath in listFile)
            {
                index++;

                ProcessPercent(index);

                fileExtention = Path.GetExtension(filePath).ToLower();
                DisplayMessage(string.Format("{0}: {1} is processing...", index, Path.GetFileName(filePath)));
                rst = 0;

                try
                {

                    switch (fileExtention)
                    {
                        case Constant.EXTENSION_PLF:
                            rst = DecodePLF(filePath, folderOutput);
                            break;
                        case Constant.EXTENSION_PLG:
                            rst = DecodePLG(filePath, folderOutput);
                            break;
                        default:
                            break;
                    }

                    if (rst > 0)
                    {
                        DisplayMessage(string.Format("Done\n"));
                        count++;
                    }
                    else
                    {
                        DisplayMessage(string.Format("Fails\n"));
                        error++;
                    }
                }
                catch (Exception ex)
                {
                    DisplayMessage(string.Format("Fails - Error:{0}", ex.Message));
                }
            }

            if (count > 0x7fff || error > 0x7fff)
            {
                count = error > 0 ? count *= -1 : count;
            }
            else
            {
                count = (error * 0x10000) + count; 
            }
            return count;
        }

        /// <summary>
        /// Decode plg file
        /// </summary>
        /// <param name="filePath">file output path</param>
        /// <returns></returns>
        private int DecodePLG(string filePath, string folderOuput = "")
        {
            decode = new DecodePLG();
            decode.DisplayMessageEvent += DisplayMessage;
            return decode.CheckFile(filePath, folderOuput);
        }

        /// <summary>
        /// Decode PLF 
        /// </summary>
        /// <param name="filePath">file output path </param>
        /// <returns></returns>
        private int DecodePLF(string filePath, string folderOuput = "")
        {
            decode = new DecodePLF();
            decode.DisplayMessageEvent += DisplayMessage;
            return decode.CheckFile(filePath, folderOuput);
        }

        public void DisplayMessage(string message)
        {
            if (displayMessageEvent != null)
            {
                displayMessageEvent(message);
            }
        }

        private void ProcessPercent(int value)
        {
            if (ProcessPercentEvent != null)
                ProcessPercentEvent(value);
        }
    }
}
