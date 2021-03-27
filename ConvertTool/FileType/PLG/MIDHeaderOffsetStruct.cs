using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDHeaderOffsetStruct
    {
        public uint offset;
        public uint size;
    }
}
