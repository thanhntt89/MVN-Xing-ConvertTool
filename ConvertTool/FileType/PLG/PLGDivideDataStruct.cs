using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PLGDivideDataStruct
    {      
        public uint pointer;
        public uint size;
        public uint entry_flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] md5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] extension;
    }
}
