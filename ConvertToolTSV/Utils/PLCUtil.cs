using PLTextToolTSV.Common;
using System;

namespace PLTextToolTSV.Utils
{
    public class PLCUtil
    {
        /// <summary>
        /// Decode PLC data
        /// </summary>
        /// <param name="type">PLC type</param>
        /// <param name="offSet">Offset data</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        public static void PLCanaDataMDT(EnumCanaDataMDT type, uint offSet, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumCanaDataMDT.CanaDataType0:
                    break;
                case EnumCanaDataMDT.CanaDataType1:
                    break;
                case EnumCanaDataMDT.CanaDataType2:
                    CanaDataMDTDecodeCanaDataType2(offSet, data, filePath);
                    break;
                case EnumCanaDataMDT.CanaDataType3:
                    CanaDataMDTDecodeCanaDataType3(offSet, data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Decode CanaDataType3
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaDataMDTDecodeCanaDataType3(uint offSet, byte[] data, string filePath)
        {
            uint dType = data[offSet];
            uint dTiming = data[offSet + 1];
            Constant.EXT_Total_Tick += dTiming;           
            byte dData = 0x00;

            LogWriter.SetToBuff(filePath, string.Format("       メタData<{0}H>\tTimingByte({1})\tメタデータ=", ByteUtil.IntToHex2(dType), dTiming), false);

            byte[] dText = new byte[16];

            int size = (int)((dType & 0x0f) - 1);
            int valIndex = 0;
            for (int index = 0; index < size; index++)
            {
                dData = data[offSet + index + 2];
                LogWriter.SetToBuff(filePath, string.Format("<{0}H>", ByteUtil.IntToHex2(dData), dTiming), false);
                if (dData != 0x00)
                {
                    dText[valIndex] = dData;
                    valIndex++;
                }
            }

            LogWriter.SetToBuff(filePath, string.Format("[{0}]\tTotal-Tick({1})", ByteUtil.ConvertBytesToDefault(dText), Constant.EXT_Total_Tick), false);
            LogWriter.SetToBuff(filePath, string.Format(""));
        }

        /// <summary>
        /// Decode CanaDataType2
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaDataMDTDecodeCanaDataType2(uint offSet, byte[] data, string filePath)
        {
            uint dType = data[offSet];
            uint dTiming = data[offSet + 1];
            uint dData = data[offSet + 2];

            Constant.EXT_Total_Tick += dTiming;
            LogWriter.SetToBuff(filePath, string.Format("       同期Data<{0}H>\tTimingByte({1})\t同期データ<{2}H>=", ByteUtil.IntToHex2(dType), dTiming, ByteUtil.IntToHex2(dData)), false);

            #region Decode data
            switch (dData)
            {
                case 0:
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック色替え開始 速度({0})]", data[offSet + 3]), false);
                    break;
                case 1:
                    LogWriter.SetToBuff(filePath, string.Format("[色替えスピード 速度({0})]", data[offSet + 3]), false);
                    break;
                case 2:
                    LogWriter.SetToBuff(filePath, string.Format("[画面表示]"), false);
                    break;
                case 3:
                    LogWriter.SetToBuff(filePath, string.Format("[画面消去]"), false);
                    break;
                case 4:
                    LogWriter.SetToBuff(filePath, string.Format("[次画面表示]"), false);
                    break;
                case 5:
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック消去 ブロック数({0})]", data[offSet + 3]), false);
                    break;
                case 6:
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック表示 ブロック数({0})]", data[offSet + 3]), false);
                    break;
                case 7:
                    LogWriter.SetToBuff(filePath, string.Format("[サブブロック色替え開始 速度({0})]", data[offSet + 3]), false);
                    break;
                case 8:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ色替えスピード 速度({0})]", data[offSet + 3]), false);
                    break;
                case 12:
                    LogWriter.SetToBuff(filePath, string.Format("[低速ブロック色替え開始 速度({0})]", data[offSet + 3]), false);
                    break;
                case 13:
                    LogWriter.SetToBuff(filePath, string.Format("[低速色替えスピード 速度({0})]", data[offSet + 3]), false);
                    break;
                case 14:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ低速ブロック色替え開始 速度({0})]", data[offSet + 3]), false);
                    break;
                case 15:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ低速色替えスピード 速度({0})]", data[offSet + 3]), false);
                    break;
                case 16:
                    LogWriter.SetToBuff(filePath, string.Format("[MX用同期Flag]"), false);
                    break;
                case 17:
                    LogWriter.SetToBuff(filePath, string.Format("[タイトル消去エフェクト開始]"), false);
                    break;
                case 20:
                    switch (Constant.EXT_PLType)
                    {
                        case "C":
                        case "D":
                            LogWriter.SetToBuff(filePath, string.Format("[ADPCM-Read ADP番号({0})]", data[offSet + 3]), false);
                            break;
                        case "E":
                        case "F":
                            LogWriter.SetToBuff(filePath, string.Format("[生音Read 生音({0})]", data[offSet + 3]), false);
                            break;
                        default:
                            break;
                    }
                    break;
                case 21:
                    switch (Constant.EXT_PLType)
                    {
                        case "C":
                        case "D":
                            LogWriter.SetToBuff(filePath, string.Format("[ADPCM-再生]"), false);
                            break;
                        case "E":
                            LogWriter.SetToBuff(filePath, string.Format("[生音再生 生音({0})]", data[offSet + 3]), false);
                            break;
                        case "F":
                            LogWriter.SetToBuff(filePath, string.Format("[生音再生 生音({0}) 音量({1})]", data[offSet + 3], data[offSet + 4]), false);
                            break;
                        default:
                            break;
                    }
                    break;
                case 22:
                    LogWriter.SetToBuff(filePath, string.Format("[拍手-Read]"), false);
                    break;
                case 23:
                    LogWriter.SetToBuff(filePath, string.Format("[拍手-再生]"), false);
                    break;
                case 24:
                    LogWriter.SetToBuff(filePath, string.Format("[KP-ON]"), false);
                    break;
                case 25:
                    LogWriter.SetToBuff(filePath, string.Format("[KP-OFF]"), false);
                    break;
                case 30:
                    LogWriter.SetToBuff(filePath, string.Format("[コーラス開始]"), false);
                    break;
                case 31:
                    LogWriter.SetToBuff(filePath, string.Format("[後奏カット]"), false);
                    break;
                case 32:
                    LogWriter.SetToBuff(filePath, string.Format("[1コーラス終了]"), false);
                    break;
                case 33:
                    LogWriter.SetToBuff(filePath, string.Format("[2コーラス終了]"), false);
                    break;
                case 34:
                    LogWriter.SetToBuff(filePath, string.Format("[歌唱開始]"), false);
                    break;
                case 35:
                    LogWriter.SetToBuff(filePath, string.Format("[歌唱終了]"), false);
                    break;
                case 36:
                    LogWriter.SetToBuff(filePath, string.Format("[アシスト用EQ開始]"), false);
                    break;
                case 40:
                    LogWriter.SetToBuff(filePath, string.Format("[歌唱アシストフラグ 種別({0}) 音番号({1}) 音量({2})]", data[offSet + 3], data[offSet + 4], data[offSet + 5]), false);
                    break;
                default:
                    LogWriter.SetToBuff(filePath, string.Format("[異常データ]"), false);
                    break;
            }
            #endregion

            LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick), false);
            LogWriter.SetToBuff(filePath, string.Format(""));
        }

        /// <summary>
        ///  Decode PLC type
        /// </summary>
        /// <param name="type">EnumCanaType</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        public static void PLCanaType(EnumCanaType type, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumCanaType.課金タイプ:// Pending
                    // CanaTypeDecodeBillingType(data, filePath);
                    break;
                case EnumCanaType.SND先行読み出しFlag:
                    CanaTypeDecodeSNDReadDheadFlag(data, filePath);
                    break;
                case EnumCanaType.エフェクトFlag:
                    CanaTypeDecodeEffectFlag(data, filePath);
                    break;
                case EnumCanaType.同時色替えFlag:
                    CanaTypeDecodeChangeFlag(data, filePath);
                    break;
                case EnumCanaType.CodecType:
                    CanaTypeDecodeCodecType(data, filePath);
                    break;
                case EnumCanaType.全体機能フラグ:
                    CanaTypeDecodeOverallFunctionFlag(data, filePath);
                    break;
                case EnumCanaType.フォント識別コード:
                    CanaTypeDecodeFontIdCode(data, filePath);
                    break;
                case EnumCanaType.文字色輪郭色:
                    CanaTypeDecodeTextAndOutlineColor(data, filePath);
                    break;
                case EnumCanaType.PalletData:
                    CanaTypeDecodePalletData(data, filePath);
                    break;
                case EnumCanaType.JVジャンル:
                    CanaTypeJVGenre(data, filePath);
                    break;
                case EnumCanaType.曲属性Table:
                    CanaTypeSongAttTable(data, filePath);
                    break;
                case EnumCanaType.原曲キー情報:
                    CanaTypeOriginalSongKeyInfo(data, filePath);
                    break;
                case EnumCanaType.ジャンルTable:
                    CanaTypeGenreTable(data, filePath);
                    break;
                case EnumCanaType.作成日時:
                    DateTime dateTime = ByteUtil.BytesToDateTime(data);
                    LogWriter.SetToBuff(filePath, string.Format("[{0}]", dateTime.ToString("yyyy/M/d HH:mm:ss")), false);
                    break;
                case EnumCanaType.管理曲所有会社番号:
                    CanaTypeSongOwnerCompany(data, filePath);
                    break;
                case EnumCanaType.MD5:
                    CanaTypeMD5(data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Decode CodecType
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeCodecType(byte[] data, string filePath)
        {
            uint cType = data[0];

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string cTypeString = string.Empty;
            switch (cType)
            {
                case 0x10:
                    cTypeString = "[MP2]";
                    break;
                case 0x20:
                    cTypeString = "[OggVorbis]";
                    break;
                case 0x30:
                    cTypeString = "[AAC]";
                    break;
                default:
                    cTypeString = "[異常データ]";
                    break;
            }

            LogWriter.SetToBuff(filePath, cTypeString, false);
        }

        /// <summary>
        /// Decode OverallFunctionFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeOverallFunctionFlag(byte[] data, string filePath)
        {
            uint aType = data[0];

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(aType)), false);
            string cTypeString = string.Empty;

            if ((aType & 0x02) == 0x02)
            {
                cTypeString = "[キーコン禁止]";
            }
            else
            {
                cTypeString = "[キーコン許可]";
            }
            if ((aType & 0x04) == 0x04)
            {
                cTypeString = "[巻き戻し禁止(PLE)]";
            }
            else
            {
                cTypeString = "[巻き戻し許可(PLE)]";
            }

            LogWriter.SetToBuff(filePath, cTypeString, false);
        }

        /// <summary>
        /// Decode FontIdCode
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeFontIdCode(byte[] data, string filePath)
        {
            FontCodeCollection fonts = new FontCodeCollection();
            int fontId = (int)data[0];
            string fontCode = fonts.GetFontCodeById(fontId);

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2((uint)fontId)), false);
            LogWriter.SetToBuff(filePath, fontCode, false);
        }

        /// <summary>
        /// Decode EffectFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeEffectFlag(byte[] data, string filePath)
        {
            uint effectFlag = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(effectFlag)), false);
            string flagInfo = string.Empty;
            switch (data[0])
            {
                case 0x00:
                    flagInfo = "[エフェクト無し]";
                    break;
                case 0x01:
                    flagInfo = "[フェードイン]";
                    break;
                case 0x10:
                    flagInfo = "[フェードアウト]";
                    break;
                case 0x11:
                    flagInfo = "[フェードイン・フェードアウト両方]";
                    break;
                default:
                    flagInfo = "[異常データ]";
                    break;
            }

            LogWriter.SetToBuff(filePath, flagInfo, false);
        }

        /// <summary>
        /// Decode SNDReadDheadFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeSNDReadDheadFlag(byte[] data, string filePath)
        {
            uint sFlag = data[0];

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(sFlag)), false);
            string sflagInfo = string.Empty;
            if ((sFlag & 0x01) == 0x01)
            {
                sflagInfo = "[先行読み出しをする]";
            }
            else
            {
                sflagInfo = "[先行読み出しをしない]";
            }

            LogWriter.SetToBuff(filePath, sflagInfo, false);
        }

        /// <summary>
        /// Decode ChangeFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeChangeFlag(byte[] data, string filePath)
        {
            uint flag = data[0];
            string flagHex = flag == 0xff ? ByteUtil.IntToHex2(flag) : ByteUtil.IntToHex2(flag);

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", flagHex), false);
            string flagInfo = string.Empty;

            switch (flag)
            {
                case 0x00:
                    flagInfo = "[Main]";
                    break;
                case 0x01:
                    flagInfo = "[Sub]";
                    break;
                case 0xff:
                    flagInfo = "[色替え無し]";
                    break;
                default:
                    flagInfo = "[異常データ]";
                    break;
            }

            LogWriter.SetToBuff(filePath, flagInfo, false);
        }

        /// <summary>
        /// Decode TextAndOutlineColor
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodeTextAndOutlineColor(byte[] data, string filePath)
        {
            uint color = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(color)), false);
            LogWriter.SetToBuff(filePath, string.Format("[表示色({0})][色替え色({1})]", ((color & 0xf0) >> 4), (color & 0x0f)), false);
        }

        /// <summary>
        /// Decode PalletData
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeDecodePalletData(byte[] data, string filePath)
        {
            uint pallet = (uint)BitConverter.ToInt16(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex4(pallet)), false);
            int red = (int)((pallet & 0x7c00) >> 10);
            LogWriter.SetToBuff(filePath, string.Format("Red({0}) ", red.ToString().PadLeft(2, ' ')), false);
            int green = (int)((pallet & 0x03e0) >> 5);
            LogWriter.SetToBuff(filePath, string.Format("Green({0}) ", green.ToString().PadLeft(2, ' ')), false);
            int blue = (int)(pallet & 0x001f);
            LogWriter.SetToBuff(filePath, string.Format("Blue({0}) ", blue.ToString().PadLeft(2, ' ')), false);

            if ((pallet & 0x8000) == 0x8000)
            {
                LogWriter.SetToBuff(filePath, string.Format("[透明色]"), false);
            }
            else
            {
            	// 作成時備考：本機能はPLF以降は未対応なので不要とのことから"[通常色]"を""とした（念のため0x8000の[透明色]は残した）
                LogWriter.SetToBuff(filePath, string.Format(""), false);
            }
        }

        /// <summary>
        /// Decode TypeJVGenre
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeJVGenre(byte[] data, string filePath)
        {
            int jvGenre = (int)data[0];

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.BytesToHex2(data[0])), false);

            byte genre = (byte)(jvGenre & 0x1f);

            if (genre <= 25)
            {
                jvGenre = (byte)('A' + genre);
            }
            else
            {
                jvGenre = (byte)('a' + genre);
            }

            LogWriter.SetToBuff(filePath, string.Format("[{0}ジャンル]", (char)jvGenre));
        }

        /// <summary>
        /// Decode SongAttTable
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeSongAttTable(byte[] data, string filePath)
        {
            int songAttKey = (int)data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.BytesToHex2(data[0])), false);

            string songAttTable = string.Empty;

            switch (data[0])
            {
                case 0x0000:
                    songAttTable = "[色換え開始Timing\t0.3秒]";
                    break;
                case 0x0001:
                    songAttTable = "[色換え開始Timing\t0.4秒]";
                    break;
                default:
                    songAttTable = "[異常データ]";
                    break;
            }

            LogWriter.SetToBuff(filePath, songAttTable);
        }

        /// <summary>
        /// Decode TypeSongOwnerCompany
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeSongOwnerCompany(byte[] data, string filePath)
        {
            SongOwnerCompanyCollection managementSongOwnerCompany = new SongOwnerCompanyCollection();

            string valCompare = string.Format("{0}{1}", Convert.ToChar(data[0]), Convert.ToChar(data[1]));
            LogWriter.SetToBuff(filePath, string.Format("[{0}]=", valCompare), false);

            string songtype = managementSongOwnerCompany.GetSongType(valCompare);
            LogWriter.SetToBuff(filePath, songtype);

            managementSongOwnerCompany = null;
        }

        /// <summary>
        /// Decode TypeOriginalSongKeyInfo
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeOriginalSongKeyInfo(byte[] data, string filePath)
        {
            int keysong = (int)data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.BytesToHex2(data[0])), false);

            OriginalSongKeyCollection songKeys = new OriginalSongKeyCollection();
            string songInfo = songKeys.GetSongInfo(data[0]);
            LogWriter.SetToBuff(filePath, songInfo);
            songKeys = null;
        }

        /// <summary>
        /// Decode TypeGenreTable
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeGenreTable(byte[] data, string filePath)
        {
            ushort tableType = (ushort)BitConverter.ToInt16(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex4(tableType)), false);

            if ((tableType & 0x0001) == 0x0000)
            {
                LogWriter.SetToBuff(filePath, string.Format("[演歌系]"), false);
            }
            else
            {
                LogWriter.SetToBuff(filePath, string.Format("[Pops系]"), false);
            }
            if ((tableType & 0x2000) == 0x0000)
            {
                LogWriter.SetToBuff(filePath, string.Format("[通常曲]"), false);
            }
            else
            {
                LogWriter.SetToBuff(filePath, string.Format("[管理曲]"), false);
            }

            switch (tableType & 0xc000)
            {
                case 0x0000:
                    LogWriter.SetToBuff(filePath, string.Format("[Solo]"), false);
                    break;
                case 0x4000:
                    LogWriter.SetToBuff(filePath, string.Format("[男女]"), false);
                    break;
                case 0x8000:
                    LogWriter.SetToBuff(filePath, string.Format("[男男]"), false);
                    break;
                case 0xc000:
                    LogWriter.SetToBuff(filePath, string.Format("[女女]"), false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Decode TypeMD5
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void CanaTypeMD5(byte[] data, string filePath)
        {
            for (int i = 0; i < 16; i++)
            {
                string val = ByteUtil.BytesToHex2(data[i]);
                LogWriter.SetToBuff(filePath, string.Format("<{0}H>", val), false);
            }
        }

        public static void CanaTypeDecodeBillingType(byte[] data, string filePath)
        {
            uint dType = data[0];
            uint dTiming = data[1];
            uint dData = data[0];
        }
    }
}
