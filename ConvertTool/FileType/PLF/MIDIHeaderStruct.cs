using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIHeaderStruct
    {       
        public ushort vocal_tr;       
        public ushort rhythm_tr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] ongen_type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler1;       
        public uint ongen_version;
        public uint neiro_version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] midi_ch_a;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
        public uint[] track_point;
    }
}
