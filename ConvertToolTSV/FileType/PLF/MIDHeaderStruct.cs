using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler1;
    }
}
