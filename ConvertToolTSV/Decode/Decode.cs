using System.IO;
using static PLTextToolTSV.SystemEvent;

namespace PLTextToolTSV.Decode
{
    public class Decode
    {
        public DisplayMessageHandle DisplayMessageEvent;

        public virtual int CheckFile(string filePath, string folderOutputPath = "")
        {
            return 0;
        }

        public virtual int DecodeData(string outPuthFileName, byte[] data)
        {
            return 0;
        }

        public virtual void DisplayMessage(string message)
        {
            if (DisplayMessageEvent != null)
                DisplayMessageEvent(message);
        }
    }
}
