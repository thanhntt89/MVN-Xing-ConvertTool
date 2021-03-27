using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DBHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] data_id;
        public byte version;
        public ushort happyo_year;
        public ushort genre_table;        
        public ushort offset_songname;
        public ushort offset_singername;
        public ushort offset_sakusiname;
        public ushort offset_sakkyokuname;       
        public byte original_key;
        public byte song_att_table;       
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] kanri_kyoku_syoyukaisha;
        public byte jv_genre;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] filler;
        public ushort moji_num;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filler1;
    }
}
