using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDHeaderOffsetStruct
    {
        public uint offset;
        public uint size;
    }
}
