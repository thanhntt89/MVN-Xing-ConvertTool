using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGDataHeaderStruct
    {
        public uint pointer;
        public uint size;      
        public uint src_size;
    }
}
