using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGDataHeaderStruct
    {
        public uint pointer;
        public uint size;      
        public uint src_size;
    }
}
