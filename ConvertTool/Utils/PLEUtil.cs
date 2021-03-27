using PLTextToolTXT.Common;
using PLTextToolTXT.FileType.PLF;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace PLTextToolTXT.Utils
{
    public class PLEUtil
    {
        public static void PLEanaData(EnumEanaData type, uint offSet, uint size, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumEanaData.色換速度情報データ部:
                    EnanaDataDecodeCCSIDataSection(offSet, size, data, filePath);
                    break;
                case EnumEanaData.BLOCK_表示情報データ部:
                    EnanaDataDecodeInfoDataSection(offSet, size, data, filePath);
                    break;
                case EnumEanaData.BLOCK_文字_DATA_部:
                    EnanaDataDecodeBlockCharacterData(offSet, size, data, filePath);
                    break;
                case EnumEanaData.MD5: // MD5
                    PLEanaDataMD5Decode(offSet, size, data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Calculate hash md5 
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="size">data size</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">Path file to save data</param>
        private static void PLEanaDataMD5Decode(uint offSet, uint size, byte[] data, string filePath)
        {
            byte[] digest = ByteUtil.GetBytes(data, offSet, size);

            using (MD5 md5 = MD5.Create())
            {
                digest = md5.ComputeHash(digest);
            }

            LogWriter.SetToBuff(filePath, string.Format("  計算したMD5は:"), false);
            // Write to file
            PLCUtil.PLCanaType(EnumCanaType.MD5, digest, filePath);
        }

        /// <summary>
        /// Decode CCSIDataSection 
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="size">data size</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">Path file to save data</param>
        private static void EnanaDataDecodeCCSIDataSection(uint offSet, uint size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("      /色換速度情報データ部/"));

            // Convert byte array to struct
            SNGBlockSpeedStruct sngBlockSpeed = ByteUtil.BytesToType<SNGBlockSpeedStruct>(data, offSet);

            LogWriter.SetToBuff(filePath, string.Format("        データ数:({0})", sngBlockSpeed.count));

            SNGSpeedStruct sngSpeed = new SNGSpeedStruct();

            // Next block off set
            uint offSetSpeed = offSet + (uint)Marshal.SizeOf(typeof(SNGBlockSpeedStruct));
            // Get stuct size of SNGSpeedStruct
            uint sngSpeedSize = (uint)Marshal.SizeOf(typeof(SNGSpeedStruct));

            for (int index = 0; index < sngBlockSpeed.count; index++)
            {
                LogWriter.SetToBuff(filePath, string.Format("        /色換速度データ{0}/", (index + 1)));
                sngSpeed = ByteUtil.BytesToType<SNGSpeedStruct>(data, offSetSpeed);

                LogWriter.SetToBuff(filePath, string.Format("          指示タイミング:({0})*10ms", sngSpeed.timing));
                LogWriter.SetToBuff(filePath, string.Format("          同時色換Flag:"), false);
                PLCUtil.PLCanaType(EnumCanaType.同時色替えFlag, sngSpeed.flag, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));
                LogWriter.SetToBuff(filePath, string.Format("          色換速度倍率:({0})", (uint)sngSpeed.rate[0]));
                LogWriter.SetToBuff(filePath, string.Format("          色換速度:({0})*0.01dot", sngSpeed.speed));

                offSetSpeed += sngSpeedSize;
            }
        }

        /// <summary>
        /// Decode InfoDataSection 
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="size">data size</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">Path file to save data</param>
        private static void EnanaDataDecodeInfoDataSection(uint offSet, uint size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("      /Block表示情報Data部/"));

            SNGBlockDispStruct snglockDisp = ByteUtil.BytesToType<SNGBlockDispStruct>(data, offSet);

            LogWriter.SetToBuff(filePath, string.Format("        Block数:({0})", snglockDisp.block_count));

            SNGDispTimingStruct sngTiming = new SNGDispTimingStruct();

            // Off set SNGDispTimingStruct
            uint offSetSngBlock = offSet + (uint)Marshal.SizeOf(typeof(SNGBlockDispStruct));

            for (int index = 0; index < snglockDisp.block_count; index++)
            {
                LogWriter.SetToBuff(filePath, string.Format("        /表示タイミング情報{0}/", (index + 1)));

                sngTiming = ByteUtil.BytesToType<SNGDispTimingStruct>(data, offSetSngBlock);

                LogWriter.SetToBuff(filePath, string.Format("          同時色換Flag:"), false);

                PLCUtil.PLCanaType(EnumCanaType.同時色替えFlag, sngTiming.change_flag, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));

                if (Constant.EXT_PLType.Equals("G"))
                {
                    LogWriter.SetToBuff(filePath, string.Format("          エフェクトFlag:"), false);
                    PLCUtil.PLCanaType(EnumCanaType.エフェクトFlag, sngTiming.efect_flag, filePath);
                }

                LogWriter.SetToBuff(filePath, string.Format("          Block Offset:<{0}H>({1})", ByteUtil.IntToHex4(sngTiming.offset), sngTiming.offset));
                LogWriter.SetToBuff(filePath, string.Format("          表示開始タイミング:({0})*10ms", sngTiming.disp_timing));
                LogWriter.SetToBuff(filePath, string.Format("          色換開始タイミング:({0})*10ms", sngTiming.change_timing));
                LogWriter.SetToBuff(filePath, string.Format("          消去タイミング:({0})*10ms", sngTiming.clear_timing));

                // Next SNGDispTimingStruct              
                offSetSngBlock += (uint)Marshal.SizeOf(typeof(SNGDispTimingStruct));
            }
        }


        /// <summary>
        /// Decode BlockCharacterData 
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="size">data size</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">Path file to save data</param>
        private static void EnanaDataDecodeBlockCharacterData(uint offSet, uint size, byte[] data, string filePath)
        {
            uint lastBlockSize = size;
            int lastSize = 0;
            short data_nume = 0;
            short moji_num = 0;
            int index = 0;

            while (lastBlockSize > 0)
            {
                index++;
                lastSize = 0;
                #region Display Block文字列Data部
                LogWriter.SetToBuff(filePath, string.Format("      /Block文字列Data部{0}/", index));

                SNGBlockMojiStruct sngData = ByteUtil.BytesToType<SNGBlockMojiStruct>(data, offSet);

                data_nume = (short)sngData.data_num[0];
                LogWriter.SetToBuff(filePath, string.Format("        Data byte数:({0})", data_nume));

                lastBlockSize -= (uint)data_nume;

                LogWriter.SetToBuff(filePath, string.Format("        表示位置 Ｘ座標:({0})", sngData.locate_x));
                LogWriter.SetToBuff(filePath, string.Format("        表示位置 Ｙ座標:({0})", sngData.locate_y));

                LogWriter.SetToBuff(filePath, string.Format("        文字色:"), false);
                PLCUtil.PLCanaType(EnumCanaType.文字色輪郭色, sngData.moji_color, filePath);

                LogWriter.SetToBuff(filePath, string.Format("        輪郭色:"), false);
                PLCUtil.PLCanaType(EnumCanaType.文字色輪郭色, sngData.waku_color, filePath);

                LogWriter.SetToBuff(filePath, string.Format("        文字輪郭終了位置:({0})", sngData.waku_end_point));
                LogWriter.SetToBuff(filePath, string.Format("        ルビ文字輪郭終了位置:({0})", sngData.ruby_end_point));

                moji_num = sngData.moji_num[0];
                LogWriter.SetToBuff(filePath, string.Format("        文字数:({0})", moji_num));

                LogWriter.SetToBuff(filePath, string.Format("        フォント識別コード:"), false);
                PLCUtil.PLCanaType(EnumCanaType.フォント識別コード, sngData.font_code, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));
                #endregion

                #region Display 文字コード
                int sngSize = Marshal.SizeOf(typeof(SNGBlockMojiStruct));
                lastSize = data_nume - sngSize;
                int codeSize = Marshal.SizeOf(typeof(ushort));
                byte[] mCode = new byte[codeSize];
                uint codeOffset = offSet + (uint)sngSize;

                for (int loop = 0; loop < moji_num; loop++)
                {
                    mCode = ByteUtil.GetBytes(data, codeOffset, (uint)codeSize);
                    lastSize -= codeSize;
                    Array.Reverse(mCode);
                    ushort dCode = (ushort)BitConverter.ToInt16(mCode, 0);
                    LogWriter.SetToBuff(filePath, string.Format("        文字コード{0}:<{1}H>", (loop + 1), ByteUtil.IntToHex4((uint)dCode)));
                    codeOffset += (uint)codeSize;
                }
                #endregion

                #region Display ルビ

                byte[] rubyBlock = new byte[1];
                byte[] rubyBlockCount = new byte[256];
                uint rubyCount = 0;
                uint rubyBlockSize = (uint)Marshal.SizeOf(typeof(byte));
                uint rubyOffset = codeOffset;
                uint offSetDataStart = 0;
                {
                    lastSize -= Marshal.SizeOf(typeof(byte));
                    rubyBlock = ByteUtil.GetBytes(data, rubyOffset, rubyBlockSize);
                    uint rubyBlockValue = rubyBlock[0];
                    LogWriter.SetToBuff(filePath, string.Format("        ルビBlock数:({0})", rubyBlockValue));
                    rubyOffset += rubyBlockSize;

                    for (int loop = 0; loop < rubyBlockValue; loop++)
                    {
                        rubyBlockCount[loop] = ByteUtil.GetBytes(data, rubyOffset, rubyBlockSize)[0];
                        lastSize -= (int)rubyBlockSize;
                        LogWriter.SetToBuff(filePath, string.Format("        ルビBlock{0} 文字数:({1})", (loop + 1), (uint)rubyBlockCount[loop]));
                        if ((uint)rubyBlockCount[loop] != 0)
                        {
                            rubyCount++;
                        }
                        // Next block
                        rubyOffset += rubyBlockSize;
                    }

                    offSetDataStart = rubyOffset;

                    uint offSetDataSize = (uint)Marshal.SizeOf(typeof(short));
                    byte[] offSetDataValue = new byte[offSetDataSize];
                    byte[] rCode = new byte[offSetDataSize];

                    for (int loop = 0; loop < rubyCount; loop++)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        /ルビ文字列Data{0}/", (loop + 1)));
                        lastSize -= (int)offSetDataSize;

                        // Get bytes for data value 
                        offSetDataValue = ByteUtil.GetBytes(data, offSetDataStart, offSetDataSize);
                        LogWriter.SetToBuff(filePath, string.Format("          ルビBlock相対Ｘ座標位置:({0})", BitConverter.ToInt16(offSetDataValue, 0)));
                        // Next block
                        offSetDataStart += offSetDataSize;

                        for (int turn = 0; turn < (int)rubyBlockCount[loop]; turn++)
                        {
                            // Get bytes for rCode
                            rCode = ByteUtil.GetBytes(data, offSetDataStart, offSetDataSize);
                            lastSize -= (int)offSetDataSize;
                            Array.Reverse(rCode);
                            ushort numCode = (ushort)BitConverter.ToInt16(rCode, 0);
                            LogWriter.SetToBuff(filePath, string.Format("          ルビ文字code{0}:<{1}H>", (turn + 1), ByteUtil.IntToHex4(numCode)));
                            offSetDataStart += offSetDataSize;
                        }
                    }
                }
                #endregion

                offSet = offSetDataStart + (uint)lastSize;
            }
        }

        /// <summary>
        /// Main Function decode MDT data
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="size">data size</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">Path file to save data</param>
        public static void PLEanaDataMdt(EnumEanaDataMDT dataType, uint offSet, byte[] data, string filePath)
        {
            switch (dataType)
            {
                case EnumEanaDataMDT.MDT_0:
                    break;
                case EnumEanaDataMDT.MDT_1:
                    DecodePLEanaDataMdt1(offSet, data, filePath);
                    break;
                case EnumEanaDataMDT.MDT_10:
                    DecodePLEanaDataMdt10(offSet, data, filePath);
                    break;
                case EnumEanaDataMDT.MDT_2:
                    DecodePLEanaDataMdt2(offSet, data, filePath);
                    break;
                case EnumEanaDataMDT.MDT_3:
                    DecodePLEanaDataMdt3(offSet, data, filePath);
                    break;
                case EnumEanaDataMDT.MDT_4:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 旧互換 Decode PLEana Data Mdt1
        /// </summary>
        /// <param name="offSet">offSet</param>
        /// <param name="data">data</param>
        /// <param name="filePath">filePath</param>
        private static void DecodePLEanaDataMdt1(uint offSet, byte[] data, string filePath)
        {
            uint sPosi = offSet;
            ushort oTiming = 0;
            uint dType = 0x00;
            int loop = 0;
            bool eFlag = false;
            byte[] buff;

            while (!eFlag)
            {
                dType = data[sPosi];

                switch (dType & 0xf0)
                {
                    #region check data
                    case 0x70: // Note
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        NOTE<{0}H>:TimingByte({1}) NOTE-No({2}) Velocity({3}) ", ByteUtil.IntToHex2(dType), (uint)data[sPosi + 1], (uint)data[sPosi + 2], (uint)data[sPosi + 3]), false);
                        if ((dType & 0x0f) == 0x04)
                        {
                            oTiming = data[sPosi + 4];
                        }
                        else
                        {
                            oTiming = data[sPosi + 4];
                            oTiming += (ushort)(data[sPosi + 5] * 0x100);
                        }
                        LogWriter.SetToBuff(filePath, string.Format("NOTE-OFF-TimingByte({0})\tTotal-Tick({1})", oTiming, Constant.EXT_Total_Tick), false);
                        LogWriter.SetToBuff(filePath, string.Format(""));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x60: // PROG
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        PROG<{0}H>:TimingByte({1}) Program-No({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), (uint)data[sPosi + 1], (uint)data[sPosi + 2], Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x40:// その他
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        その他<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), (uint)data[sPosi + 1]), false);

                        loop = (int)((dType & 0x0f) - 1);
                        for (int index = 0; index < loop; index++)
                        {
                            LogWriter.SetToBuff(filePath, string.Format("MIDI-Data<{0}H> ", ByteUtil.IntToHex2(data[sPosi + index + 2])), false);
                        }
                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick));
                        sPosi += (uint)dType & 0x0f;
                        break;
                    case 0x50:// System Message
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        System-Message<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), (uint)(data[sPosi + 1])), false);
                        loop = (int)((dType & 0x0f) - 1);
                        for (int index = 0; index < loop; index++)
                        {
                            LogWriter.SetToBuff(filePath, string.Format("MIDI-Data<{0}H> ", ByteUtil.IntToHex2(data[sPosi + index + 2])), false);
                        }
                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x30:// NOP
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        NOP<{0}H>:TimingByte({1})\tTotal-Tick({2})", ByteUtil.IntToHex2(dType), (uint)(data[sPosi + 1]), Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;

                        break;
                    case 0x20:// Tempo Command
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        Tempo-Command<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), (uint)(data[sPosi + 1])), false);

                        switch (data[sPosi + 2])
                        {
                            case 0xe0://	Set Tempo
                                LogWriter.SetToBuff(filePath, string.Format("Set-Tempo[e0] "), false);

                                byte[] val = ByteUtil.GetBytes(data, sPosi + 3, 4);
                                LogWriter.SetToBuff(filePath, string.Format("Tempo-Data({0})", BitConverter.ToInt32(val, 0)), false);

                                break;
                            case 0xe1:
                                LogWriter.SetToBuff(filePath, string.Format("Set-Tempo[e1] "), false);
                                LogWriter.SetToBuff(filePath, string.Format("Tempo-Data({0})", data[sPosi + 3]), false);
                                break;
                            default:
                                LogWriter.SetToBuff(filePath, string.Format("TempoCommand異常"), false);
                                break;
                        }

                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x10: // 同期Data
                        buff = ByteUtil.GetBytes(data, sPosi);
                        PLCUtil.PLCanaDataMDT(EnumCanaDataMDT.CanaDataType2, 0, buff, filePath);
                        sPosi += dType & 0x0f;
                        break;
                    case 0x80: // Maste Volume:SystemExclusive
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        Maste-Volume-SystemExclusive<{0}H>:TimingByte({1}) Volume({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), (uint)data[sPosi + 1], (uint)data[sPosi + 2], Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x90: // Volum:Control Change
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1]);
                        LogWriter.SetToBuff(filePath, string.Format("        Volum-Control-Change<{0}H>:TimingByte({1}) Volume({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), data[sPosi + 1], data[sPosi + 2], Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0xa0:// メタData
                        buff = ByteUtil.GetBytes(data, sPosi);
                        PLCUtil.PLCanaDataMDT(EnumCanaDataMDT.CanaDataType3, 0, buff, filePath);
                        sPosi += dType & 0x0f;
                        break;
                    case 0x00:// 終了
                        LogWriter.SetToBuff(filePath, string.Format("        終了<{0}H>", ByteUtil.IntToHex2(dType)));
                        eFlag = true;
                        sPosi += dType & 0x0f;
                        break;
                    default:
                        LogWriter.SetToBuff(filePath, string.Format("        異常データ<{0}H>", ByteUtil.IntToHex2(dType)));
                        eFlag = true;
                        break;
                        #endregion
                }

                sPosi++;
            }
        }

        /// <summary>
        /// 旧互換 Decode PLEana Data Mdt2
        /// </summary>
        /// <param name="offSet">offSet</param>
        /// <param name="data">data</param>
        /// <param name="filePath">filePath</param>
        private static void DecodePLEanaDataMdt2(uint offSet, byte[] data, string filePath)
        {
            uint dType = data[offSet];
            uint dTiming = (uint)(data[offSet + 1] + data[offSet + 2] * 0x100);
            uint dData = data[offSet + 3];
            Constant.EXT_Total_Tick += dTiming;

            LogWriter.SetToBuff(filePath, string.Format("        同期Data<{0}H>:TimingByte({1}) 同期データ<{2}H>=", ByteUtil.IntToHex2(dType), dTiming, ByteUtil.IntToHex2(dData)), false);
            #region Check
            switch (dData)
            {
                case 0:
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック色替え開始 速度({0})]", (uint)data[offSet + 4]), false);
                    break;
                case 1:
                    LogWriter.SetToBuff(filePath, string.Format("[色替えスピード 速度({0})]", (uint)data[offSet + 4]), false);
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
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック消去 ブロック数({0})]", data[offSet + 4]), false);
                    break;
                case 6:
                    LogWriter.SetToBuff(filePath, string.Format("[ブロック表示 ブロック数({0})]", data[offSet + 4]), false);
                    break;
                case 7:
                    LogWriter.SetToBuff(filePath, string.Format("[サブブロック色替え開始 速度({0})]", data[offSet + 4]), false);
                    break;
                case 8:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ色替えスピード 速度({0})]", data[offSet + 4]), false);
                    break;
                case 12:
                    LogWriter.SetToBuff(filePath, string.Format("[低速ブロック色替え開始 速度({0})]", data[offSet + 4]), false);
                    break;
                case 13:
                    LogWriter.SetToBuff(filePath, string.Format("[低速色替えスピード 速度({0})]", data[offSet + 4]), false);
                    break;
                case 14:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ低速ブロック色替え開始 速度({0})]", data[offSet + 4]), false);
                    break;
                case 15:
                    LogWriter.SetToBuff(filePath, string.Format("[サブ低速色替えスピード 速度({0})]", data[offSet + 4]), false);
                    break;
                case 16:
                    LogWriter.SetToBuff(filePath, string.Format("[MX用同期Flag]"), false);
                    break;
                case 17:
                    LogWriter.SetToBuff(filePath, string.Format("[タイトル消去エフェクト開始]"), false);
                    break;
                case 20:
                    if (Constant.EXT_PLType.Equals("C"))
                    {
                        LogWriter.SetToBuff(filePath, string.Format("[生音Read 生音番号({0})]", data[offSet + 4]), false);
                    }
                    break;
                case 21:
                    if (Constant.EXT_PLType.Equals("E"))
                    {
                        LogWriter.SetToBuff(filePath, string.Format("生音再生 生音番号({0})]", data[offSet + 4]), false);
                    }
                    else if (Constant.EXT_PLType.Equals("F"))
                    {
                        LogWriter.SetToBuff(filePath, string.Format("[生音再生 生音番号({0}) 音量({1})]", data[offSet + 4], data[offSet + 5]), false);
                    }
                    else if (Constant.EXT_PLType.Equals("G"))
                    {
                        LogWriter.SetToBuff(filePath, string.Format("[生音再生 生音番号({0})]", data[offSet + 4]), false);
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
                    LogWriter.SetToBuff(filePath, string.Format("[歌唱アシストフラグ 種別({0}) 音番号({1}) 音量({2})]", (uint)data[offSet + 4], (uint)data[offSet + 5], (uint)data[offSet + 6]), false);
                    break;
                case 41:
                    LogWriter.SetToBuff(filePath, string.Format("[小節線情報]"), false);
                    break;
                default:
                    LogWriter.SetToBuff(filePath, string.Format("[異常データ]"), false);
                    break;
            }
            #endregion

            LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick));
        }

        /// <summary>
        /// 旧互換 Decode PLEana Data Mdt3
        /// </summary>
        /// <param name="offSet">offSet</param>
        /// <param name="data">data</param>
        /// <param name="filePath">filePath</param>
        private static void DecodePLEanaDataMdt3(uint offSet, byte[] data, string filePath)
        {
            uint dType = data[offSet];
            uint dTiming = (uint)(data[offSet + 1] + data[offSet + 2] * 0x100);
            byte dData = 0;
            byte[] dText = new byte[16];
            Constant.EXT_Total_Tick += dTiming;

            LogWriter.SetToBuff(filePath, string.Format("        メタData<{0}H>:TimingByte({1}) メタデータ=", ByteUtil.IntToHex2(dType), dTiming), false);

            int size = (int)((dType & 0x0f) - 2);
            int valIndex = 0;
            for (int index = 0; index < size; index++)
            {
                dData = data[offSet + index + 3];
                LogWriter.SetToBuff(filePath, string.Format("<{0}H>", ByteUtil.IntToHex2(dData)), false);
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
        /// 旧互換 Decode PLEana Data Mdt10
        /// </summary>
        /// <param name="offSet">offSet</param>
        /// <param name="data">data</param>
        /// <param name="filePath">filePath</param>
        private static void DecodePLEanaDataMdt10(uint offSet, byte[] data, string filePath)
        {
            uint dType = 0x00;
            bool eFlag = false;
            uint sPosi = offSet;
            uint oTiming = 0;
            int loopSize = 0;
            byte[] buff;

            while (!eFlag)
            {
                dType = data[sPosi];

                switch (dType & 0xf0)
                {
                    case 0x70:// NOTE
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        NOTE<{0}H>:TimingByte({1}) NOTE-No({2}) Velocity({3}) ", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100, data[sPosi + 3], data[sPosi + 4]), false);
                        if ((dType & 0x0f) == 0x05)
                        {
                            oTiming = data[sPosi + 5];
                        }
                        else
                        {
                            if ((dType & 0x0f) == 0x06)
                            {
                                oTiming = data[sPosi + 5];
                                oTiming += (uint)(data[sPosi + 6] * 0x100);
                            }
                            else
                            {
                                oTiming = data[sPosi + 5];
                                oTiming += (uint)(data[sPosi + 6] * 0x100);
                                oTiming += (uint)(data[sPosi + 7] * 0x10000);
                            }
                        }

                        LogWriter.SetToBuff(filePath, string.Format("NOTE-OFF-TimingByte({0})\tTotal-Tick({1})", oTiming, Constant.EXT_Total_Tick), false);
                        LogWriter.SetToBuff(filePath, string.Format(""));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x60:// PROG
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);

                        LogWriter.SetToBuff(filePath, string.Format("        PROG<{0}H>:TimingByte({1}) Program-No({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100, data[sPosi + 3], Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x40:// その他
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        その他<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100), false);
                        loopSize = (int)((dType & 0x0f) - 2);
                        for (int index = 0; index < loopSize; index++)
                        {
                            LogWriter.SetToBuff(filePath, string.Format("MIDI-Data<{0}H> ", ByteUtil.IntToHex2(data[sPosi + index + 3])), false);
                        }
                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick), false);
                        LogWriter.SetToBuff(filePath, string.Format(""));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x50:// System Message
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        System-Message<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100), false);
                        loopSize = (int)((dType & 0x0f) - 2);
                        for (int index = 0; index < loopSize; index++)
                        {
                            LogWriter.SetToBuff(filePath, string.Format("MIDI-Data<{0}H> ", ByteUtil.IntToHex2(data[sPosi + index + 3])), false);
                        }
                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick), false);
                        LogWriter.SetToBuff(filePath, string.Format(""));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x30:  // NOP
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);

                        LogWriter.SetToBuff(filePath, string.Format("        NOP<{0}H>:TimingByte({1})\tTotal-Tick({2})", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100, Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x20:  // Tempo Command
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        Tempo-Command<{0}H>:TimingByte({1}) ", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100), false);
                        switch (data[sPosi + 3])
                        {
                            case 0xe0://	Set Tempo
                                oTiming = data[sPosi + 4];
                                oTiming += (uint)data[sPosi + 5] * 0x100;
                                oTiming += (uint)data[sPosi + 6] * 0x10000;
                                oTiming += (uint)data[sPosi + 7] * 0x1000000;
                                LogWriter.SetToBuff(filePath, string.Format("Set-Tempo[e0] "), false);
                                LogWriter.SetToBuff(filePath, string.Format("Tempo-Data({0})", oTiming), false);
                                break;
                            case 0xe1://	リラティブTempo
                                LogWriter.SetToBuff(filePath, string.Format("リラティブTempo[e1] "), false);
                                LogWriter.SetToBuff(filePath, string.Format("Tempo-Data({0})", data[sPosi + 4]), false);
                                break;
                            default:
                                LogWriter.SetToBuff(filePath, string.Format("TempoCommand異常"), false);
                                break;
                        }
                        LogWriter.SetToBuff(filePath, string.Format("\tTotal-Tick({0})", Constant.EXT_Total_Tick), false);
                        LogWriter.SetToBuff(filePath, string.Format(""));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x10:// 同期Data
                        buff = ByteUtil.GetBytes(data, sPosi);
                        PLEanaDataMdt(EnumEanaDataMDT.MDT_2, 0, buff, filePath);
                        sPosi += dType & 0x0f;
                        break;
                    case 0x80:// Maste Volume:SystemExclusive
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        Maste-Volume-SystemExclusive<{0}H>:TimingByte({1}) Volume({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100, data[sPosi + 3], Constant.EXT_Total_Tick));

                        sPosi += dType & 0x0f;
                        break;
                    case 0x90:// Volum:Control Change
                        Constant.EXT_Total_Tick += (uint)(data[sPosi + 1] + data[sPosi + 2] * 0x100);
                        LogWriter.SetToBuff(filePath, string.Format("        Volum-Control-Change<{0}H>:TimingByte({1}) Volume({2})\tTotal-Tick({3})", ByteUtil.IntToHex2(dType), data[sPosi + 1] + data[sPosi + 2] * 0x100, data[sPosi + 3], Constant.EXT_Total_Tick));
                        sPosi += dType & 0x0f;
                        break;
                    case 0x00:  // 終了
                        LogWriter.SetToBuff(filePath, string.Format("        終了<{0}H>", ByteUtil.IntToHex2(dType)));
                        eFlag = true;
                        sPosi += dType & 0x0f;
                        break;
                    case 0xa0:// メタData
                        buff = ByteUtil.GetBytes(data, sPosi);
                        PLEanaDataMdt(EnumEanaDataMDT.MDT_3, 0, buff, filePath);
                        sPosi += dType & 0x0f;
                        break;
                    default:
                        LogWriter.SetToBuff(filePath, string.Format("        異常データ<{0}H>", ByteUtil.IntToHex2(dType)));
                        eFlag = true;
                        break;
                }

                sPosi++;
            }
        }

    }
}
