using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PLFHeaderStruct
    {       
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[]file_id;
        public ushort version; 
        public ushort divide_count;
    }
}
