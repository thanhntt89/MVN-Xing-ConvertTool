using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EXPHeaderStruct
    {
        public ushort record_num;
        public ushort filler;
    }
}
