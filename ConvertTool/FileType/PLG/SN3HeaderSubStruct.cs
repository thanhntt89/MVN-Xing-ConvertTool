using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SN3HeaderSubStruct
    {
        public uint offset;
        public uint size;
    }
}
