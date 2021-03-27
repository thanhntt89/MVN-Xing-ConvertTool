using System.Runtime.InteropServices;

namespace PLTextToolTSV.FileType.PLF
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CHFHeaderStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] file_id;
        public ushort version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] revision;
        public ushort kigen_start;
        public ushort kigen_end;
        public uint song_no;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] content_flag;
        public byte trf_priority_flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] filler1;
        public uint backup_flag;
        public ushort play_time;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] kanri_kyoku_syoyukaisha;        
        public uint maked_date;       
        public uint kobetsu_data_pointer;
        public uint kobetsu_data_size;
        public uint kobetsu_data2_pointer;
        public uint kobetsu_data2_size;
        public ushort service_num;
        public ushort filler3;
    }
}
