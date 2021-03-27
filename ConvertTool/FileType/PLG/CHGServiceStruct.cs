using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CHGServiceStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data_type;
        public byte kind_type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filler1;
        public uint data_file;
        public uint filler2;
    }
}
