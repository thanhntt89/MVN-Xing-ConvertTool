using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGSpeedStruct
    {
        public uint timing;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] rate;
        public ushort speed;
    }
}
