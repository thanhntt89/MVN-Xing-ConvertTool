using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GNCInfoStruct
    {
        public uint gnc_offset;
        public uint gnc_size;
    }
}
