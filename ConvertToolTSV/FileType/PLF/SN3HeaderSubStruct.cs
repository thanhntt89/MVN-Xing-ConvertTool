using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SN3HeaderSubStruct
    {
        public uint offset;
        public uint size;
    }
}
