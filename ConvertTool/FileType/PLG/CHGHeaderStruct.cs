using System.Runtime.InteropServices;

namespace PLTextToolTXT.FileType.PLG
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CHGHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] file_id;
        public ushort version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] revision;
        public ushort kigen_start;
        public ushort kigen_end;
        public uint song_no;
        public uint content_flag;       
        public byte trg_priority_flag;
        public byte premium_contents_flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] filler1;
        public uint backup_flag;
        public ushort play_time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] kanri_kyoku_syoyukaisha;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] maked_date;
        public uint kobetsu_data_pointer;
        public uint kobetsu_data_size;
        public uint kobetsu_data2_pointer;
        public uint kobetsu_data2_size;
        public ushort service_num;
        public ushort filler3;
    }
}
