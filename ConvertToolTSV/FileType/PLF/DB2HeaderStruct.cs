using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DB2HeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] einy_id_UM;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] movie_id_UM;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filter_UM;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] einy_id_UV;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] movie_id_UV;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filter_UV;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 264)]
        public byte[] filler;
    }
}
