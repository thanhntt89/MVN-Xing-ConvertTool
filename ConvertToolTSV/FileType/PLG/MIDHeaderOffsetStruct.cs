using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDHeaderOffsetStruct
    {
        public uint offset;
        public uint size;
    }
}
