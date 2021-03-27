﻿using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SN3HeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        public byte version;
        public byte data_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler;
    }
}
