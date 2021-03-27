using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SN3HeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler;
    }
}
