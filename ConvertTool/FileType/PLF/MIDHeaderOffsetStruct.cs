using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDHeaderOffsetStruct
    {
        public uint offset;
        public uint size;
    }
}
