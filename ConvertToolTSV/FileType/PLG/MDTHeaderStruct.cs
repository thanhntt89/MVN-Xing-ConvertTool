using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MDTHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        public byte version;
        public uint tempo;
        public byte timebase;
        public byte sn3_flg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] sn3_chorus_no;
        public byte mid_num;
        public byte filler1;
    }
}
