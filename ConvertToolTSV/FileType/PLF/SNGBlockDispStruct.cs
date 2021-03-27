using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGBlockDispStruct
    {
        public ushort block_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] filler1;
    }
}
