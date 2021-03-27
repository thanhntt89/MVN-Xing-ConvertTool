using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DBHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] version;       
        public ushort happyo_year;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] genre_table;
        public ushort offset_songname;
        public ushort offset_singername;
        public ushort offset_sakusiname;
        public ushort offset_sakkyokuname;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] original_key;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] song_att_table;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] kanri_kyoku_syoyukaisha;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] jv_genre;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler;
        public ushort moji_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filler1;
    }
}
