using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EXPRecordStruct
    {
        public uint mae_posi;
        public uint ato_posi;
    }
}
