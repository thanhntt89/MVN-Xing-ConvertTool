using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CAHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] version;
        public uint data_size;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] codec_type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] filler1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] func_flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_channel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att4;
    }
}
