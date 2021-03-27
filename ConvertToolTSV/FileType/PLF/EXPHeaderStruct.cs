using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EXPHeaderStruct
    {
        public ushort record_num;
        public ushort filler;
    }
}
