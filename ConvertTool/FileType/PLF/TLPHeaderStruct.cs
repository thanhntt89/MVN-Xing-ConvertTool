using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TLPHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;
        public ushort moji_offset;
        public ushort disp_offset;
        public ushort change_offset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] font_size;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] filler1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public ushort[] pallet_data;
        public ushort filler2;
    }
}
