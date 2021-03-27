using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDDataStruct
    {
        public ushort data_att;
        public ushort filler2;
        public uint data_size;
    }
}
