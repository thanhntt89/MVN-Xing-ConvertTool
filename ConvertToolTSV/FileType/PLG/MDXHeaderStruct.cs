using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MDXHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] data_id;
        public ushort version;
        public ushort filler1;
        public uint pointer;
        public uint size;
    }
}
