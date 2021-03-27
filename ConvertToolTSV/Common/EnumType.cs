namespace PLTextToolTSV.Common
{
    public enum EnumPLFanaType
    {
        VocalTrackTable = 0,
        音源指定 = 1,
        バージョン情報 = 2,
        MIDI_Channel番号 = 3,
        CodecType = 4,
        全体機能フラグ = 5,
        チャンネル属性 = 6,
        チャンネル音声タイプ = 7,
        楽器 = 8,
        コンテンツ判別フラグ = 9,
        サービス種別 = 10,
        サービスデータタイプ = 11,
        サービスデータファイル = 12,
        登録データフラグ = 13
    }

    public enum EnumPLGanaType
    {
        CONTENT_FLAG = 9,
        SH = 104,
        SERVICE_DATA = 105
    }

    public enum EnumEanaType
    {
        映像指定方式 = 33,
        系列 = 34,
        番組番号 = 35,
        再生色指定 = 36,
        フェード指定 = 37
    }

    public enum EnumCanaType
    {
        コンテンツタイプ = 0,
        録音可否フラグ = 1,
        課金タイプ = 2,
        管理曲所有会社番号 = 3,
        作成日時 = 4,
        MD5 = 5,
        ジャンルTable = 6,
        原曲キー情報 = 7,
        曲属性Table = 8,
        透過色Pallet番号 = 9,
        PalletData = 10,
        圧縮Data形式番号 = 11,
        DVDレコード数 = 12,
        JVジャンル = 13,
        DVDジャンル = 14,
        再生方法指定 = 15,
        DVDレコード = 16,
        画面情報 = 17,
        同時色替えFlag = 18,
        文字色輪郭色 = 19,
        フォント識別コード = 20,
        CodecType = 21,
        全体機能フラグ = 22,
        チャンネル音声タイプ = 23,
        SND先行読み出しFlag = 28,
        MIDI_Channel番号 = 30,
        バックアップフラグ = 32,
        エフェクトFlag = 33
    }
}
