using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SNGBlockSpeedStruct
    {        
        public ushort count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] filler1;
    }
}
