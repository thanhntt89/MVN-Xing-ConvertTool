using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SHHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] version;
        public byte filler1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] volume;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] func_flag;
        public byte data_channel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att2; 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] channel_att4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] filler2;

    }
}
