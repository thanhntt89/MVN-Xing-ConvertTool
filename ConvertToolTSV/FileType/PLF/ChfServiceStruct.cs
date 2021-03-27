using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CHFServiceStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_type;      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] data_file;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] filler2;
    }
}
