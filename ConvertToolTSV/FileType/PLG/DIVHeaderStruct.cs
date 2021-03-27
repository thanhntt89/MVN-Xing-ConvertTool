using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DIVHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] data_id;
        public ushort version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filler;
        public uint pointer;
        public uint size;
    }
}
