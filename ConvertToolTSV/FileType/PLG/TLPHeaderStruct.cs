using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TLPHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        public byte version;
        public ushort moji_offset;
        public ushort disp_offset;
        public ushort change_offset;
        public byte font_size;
        public byte filler1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public ushort[] pallet_data;
        public ushort filler2;
    }
}
