using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGDtaHeaderStruct
    {
        public uint pointer;
        public uint size;
        public uint src_size;
    }
}
