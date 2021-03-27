using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GTNHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] service;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler1;
        public uint gcc_offset;
        public uint gcc_size;
        public uint gcd_offset;
        public uint gcd_size;
        public uint grm_offset;
        public uint grm_size;
        public uint gnt_offset;
        public uint gnt_size;
        public uint gsr_offset;
        public uint gsr_size;
        public uint enc_b_offset;
        public uint enc_b_size;
        public uint enc_g1_offset;
        public uint enc_g1_size;
        public uint enc_g2_offset;
        public uint enc_g2_size;
        public uint enc_g3_offset;
        public uint enc_g3_size;
    }
}
