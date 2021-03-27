using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGDispTimingStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] change_flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] efect_flag;
        public ushort offset;
        public uint disp_timing;
        public uint change_timing;
        public uint clear_timing;
    }
}
