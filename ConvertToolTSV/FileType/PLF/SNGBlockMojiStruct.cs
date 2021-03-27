using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGBlockMojiStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler1;       
        public ushort locate_x;
        public ushort locate_y;      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] moji_color;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] waku_color;
        public ushort waku_end_point;
        public ushort ruby_end_point;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] moji_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] font_code;
    }
}
