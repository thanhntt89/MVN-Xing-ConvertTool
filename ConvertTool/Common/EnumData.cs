namespace PLTextToolTXT.Common
{

    public enum EnumPLFanaData
    {
        CHF = 0,
        TRF = 1,
        MDN = 2,
        MDB = 3,
        MDC = 32,
        SDN = 4,
        SDB = 5,
        SDC = 33,
        VSN = 6,
        VS3 = 7,
        VON = 8,
        VO3 = 9,
        GTN = 10,
        GTC = 31,
        MT = 11,
        DB = 12,
        SNG = 13,
        MID = 14,
        MID_d = 15,
        DB2 = 16,
        MDT = 21,
        MIDI = 22,
        SN3 = 23,
        CA = 25,
        TLP = 24
    }

    public enum EnumPLGanaData
    {
        CHG = 0,
        TRG = 1,
        MDN = 2,
        MDB = 3,
        SDN = 4,
        SDB = 5,
        VS4F = 6,
        VS4B = 7,
        VO4F = 8,
        VO4B = 9,
        GTN = 10,
        GTC = 31,
        SN3 = 23,
        CA = 25,
        MDF = 100,
        SDF = 101,
        SDHN = 110,
        SDHB = 111,
        SDHF = 112,
        SDH = 113,
        SH = 114
    }

    public enum EnumEanaDataMDT
    {
        MDT_0 = 0,
        MDT_1 = 1,
        MDT_2 = 2,
        MDT_3 = 3,
        MDT_4 = 4,
        MDT_10 = 10
    }

    public enum EnumEanaData
    {
        BGV_データ = 1,
        DVD_レコード = 33,
        SNG_データ = 3,
        MD5 = 32,
        SNG_データヘッダ部 = 24,
        色換速度情報データ部 = 31,
        BLOCK_表示情報データ部 = 30,
        BLOCK_文字_DATA_部 = 26,
        MID_DATA_部 = 21,
        MIDデータ = 4,
        SN2データ = 7,
        SN3_DATA_部 = 27
    }

    public enum EnumCanaDataMDT
    {
        CanaDataType0 = 0,
        CanaDataType1 = 1,
        CanaDataType2 = 2,
        CanaDataType3 = 3,
    }

    public enum EnumCanaData
    {
        DBデータ = 0,
        BGVデータ = 1,
        TTLデータ = 2,
        SNGデータ = 3,
        MIDデータ = 4,
        CHRデータ = 5,
        LNKデータ = 6,
        SN2データ = 7,
        曲名 = 10,
        歌手名 = 11,
        作詞者名 = 12,
        作曲者名 = 13,
        曲名読みかな = 14,
        歌手名読みかな = 15,
        TTLData部 = 20,
        MIDData部 = 21,
        コードリスト = 22,
        ファイル名 = 23,
        SNGデータヘッダ部 = 24,
        SNG画面情報データ部 = 25,
        Block文字列Data部 = 26,
        SN2Data部 = 27,
        VS2データ = 28,
        CSWデータ = 29
    }
}
