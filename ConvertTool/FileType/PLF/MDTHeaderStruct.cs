using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MDTHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;        
        public uint tempo;       
        public byte timebase;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] sn3_flg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] sn3_chorus_no;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] mid_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] filler1;
    }
}
