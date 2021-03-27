using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GNCInfoStruct
    {
        public uint gnc_offset;
        public uint gnc_size;
    }
}
