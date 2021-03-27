using PLTextToolTSV.Common;
using PLTextToolTSV.FileType.PLG;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace PLTextToolTSV.Utils
{
    public class PLGUtil
    {
        private static int index = 0;
        public static int DecodeHeader(string fileOutputPath, byte[] data)
        {
            PLGHeaderStruct plgheader = new PLGHeaderStruct();
            try
            {
                plgheader = ByteUtil.BytesToType<PLGHeaderStruct>(data);

                LogWriter.SetToBuff(fileOutputPath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(plgheader.file_id)));
                LogWriter.SetToBuff(fileOutputPath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(plgheader.version)));
                LogWriter.SetToBuff(fileOutputPath, string.Format("  ファイル分割数\t({0})", plgheader.divide_count));
            }
            catch 
            {
                return 0;
            }
            return 1;
        }

        public static int DecodeDivideData(uint offSet, byte[] data, int divideCount, string fileOutputPath)
        {
            PLGDivideDataStruct divide = new PLGDivideDataStruct();

            for (int index = 0; index < divideCount; index++)
            {
                LogWriter.SetToBuff(fileOutputPath, string.Format("/分割データ管理情報/\t({0})", index + 1));

                divide = ByteUtil.BytesToType<PLGDivideDataStruct>(data, offSet);

                LogWriter.SetToBuff(fileOutputPath, string.Format("  分割データPointer\t<{0}H>({1})", ByteUtil.IntToHex8(divide.pointer), divide.pointer));
                LogWriter.SetToBuff(fileOutputPath, string.Format("  分割データSize\t<{0}H>({1})", ByteUtil.IntToHex8(divide.size), divide.size));
                LogWriter.SetToBuff(fileOutputPath, string.Format("  登録データフラグ\t"), false);
                PLFUtil.PLFanaType(EnumPLFanaType.登録データフラグ, BitConverter.GetBytes(divide.entry_flag), fileOutputPath);
                LogWriter.SetToBuff(fileOutputPath, string.Format(""), true);
                LogWriter.SetToBuff(fileOutputPath, string.Format("  分割データMD5\t"), false);
                PLCUtil.PLCanaType(EnumCanaType.MD5, divide.md5, fileOutputPath);
                LogWriter.SetToBuff(fileOutputPath, string.Format(""), true);
                // MD5 Check
                PLEUtil.PLEanaData(EnumEanaData.MD5, (uint)divide.pointer, (uint)divide.size, data, fileOutputPath);
                LogWriter.SetToBuff(fileOutputPath, string.Format(""), true);
                LogWriter.SetToBuff(fileOutputPath, string.Format("  ファイル拡張子\t[{0}]", ByteUtil.ConvertBytesToString(divide.extension)));

                // Next block PLFDivideDataStruct
                offSet += (uint)Marshal.SizeOf(typeof(PLGDivideDataStruct));
            }
            return 1;
        }

        /// <summary>
        /// DecodeExtension
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        /// <returns>1</returns>
        public static int DecodeExtension(uint offSet, byte[] data, int divideCount, string fileOutputPath)
        {
            PLGDivideDataStruct divide = new PLGDivideDataStruct();

            while (divideCount > 0)
            {
                divide = ByteUtil.BytesToType<PLGDivideDataStruct>(data, offSet);
                EnumPLGanaData extentsion = Utilities.ParseEnum<EnumPLGanaData>(ByteUtil.ConvertBytesToString(divide.extension));
                switch (extentsion)
                {
                    case EnumPLGanaData.GTN:
                        PLFUtil.PLFanaData(EnumPLFanaData.GTN, divide.pointer, divide.size, data, fileOutputPath);
                        break;
                    case EnumPLGanaData.GTC:
                        PLFUtil.PLFanaData(EnumPLFanaData.GTC, divide.pointer, divide.size, data, fileOutputPath);
                        break;
                    default:                      
                        PLGanaData(extentsion, divide.pointer, divide.size, data, fileOutputPath);                       
                        break;
                }

                // Next block PLFDivideDataStruct
                offSet += (uint)Marshal.SizeOf(typeof(PLGDivideDataStruct));

                divideCount--;
            }

            return 1;
        }

        /// <summary>
        /// Main decode PLG
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>        
        public static void PLGanaData(EnumPLGanaData type, ulong offSet, ulong size, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumPLGanaData.CHG:
                    DecodeCHG(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.TRG:
                    DecodeTRG(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.MDN:
                case EnumPLGanaData.MDB:
                case EnumPLGanaData.MDF:
                    DecodeMDXHeader(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.SDN:
                case EnumPLGanaData.SDB:
                case EnumPLGanaData.SDF:
                    DecodeSDXHeader(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.SDHN:
                case EnumPLGanaData.SDHB:
                case EnumPLGanaData.SDHF:
                    DecodeSDHXHeader(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.VS4F:
                    DecodeVS4F(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.VS4B:
                    DecodeVS4B(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.VO4F:
                    DecodeVO4F(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.VO4B:
                    DecodeVO4B(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.GTN:
                    DecodeGTN(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.GTC:
                    DecodeGTC(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.SN3:
                    DecodeSN3(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.CA:
                    DecodeCA(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.SDH:
                    DecodeSDHHeader(offSet, size, data, filePath);
                    break;
                case EnumPLGanaData.SH:
                    DecodeSHeader(offSet, size, data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Decode SHeader
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>       
        private static void DecodeSHeader(ulong offSet, ulong size, byte[] data, string filePath)
        {
            SHHeaderStruct sHHeader = ByteUtil.BytesToType<SHHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("    Data ID\t[{0}]", ByteUtil.ConvertBytesToString(sHHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("    Version番号\t[{0}]", ByteUtil.ConvertBytesToString(sHHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("    Volume値\t"), false);
            PLGanaType(EnumPLGanaType.SH, sHHeader.volume, filePath);
            LogWriter.SetToBuff(filePath, string.Format("    全体機能フラグ\t"), false);
            PLFUtil.PLFanaType(EnumPLFanaType.全体機能フラグ, sHHeader.func_flag, filePath);

            LogWriter.SetToBuff(filePath, string.Format("    データのチャンネル数\t({0})", sHHeader.data_channel));

            LogWriter.SetToBuff(filePath, string.Format("    第1チャンネル属性"));
            PLFUtil.PLFanaType(EnumPLFanaType.チャンネル属性, sHHeader.channel_att1, filePath);

            LogWriter.SetToBuff(filePath, string.Format("    第2チャンネル属性"));
            PLFUtil.PLFanaType(EnumPLFanaType.チャンネル属性, sHHeader.channel_att2, filePath);

            LogWriter.SetToBuff(filePath, string.Format("    第3チャンネル属性"));
            PLFUtil.PLFanaType(EnumPLFanaType.チャンネル属性, sHHeader.channel_att3, filePath);

            LogWriter.SetToBuff(filePath, string.Format("    第4チャンネル属性"));
            PLFUtil.PLFanaType(EnumPLFanaType.チャンネル属性, sHHeader.channel_att4, filePath);
        }

        /// <summary>
        /// Decode SDHHeader
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>       
        private static void DecodeSDHHeader(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("  /SDHデータ/"));
            SN3HeaderStruct sN3Header = ByteUtil.BytesToType<SN3HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("   File ID\t[{0}]", ByteUtil.ConvertBytesToString(sN3Header.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号\t<{0}H>", ByteUtil.IntToHex2(sN3Header.version)));
            LogWriter.SetToBuff(filePath, string.Format("   Data数\t({0})", sN3Header.data_num));

            // SN3_Sub
            List<SN3HeaderSubStruct> sN3SubList = new List<SN3HeaderSubStruct>();
            SN3HeaderSubStruct sN3HeaderSub = new SN3HeaderSubStruct();
            uint offSetSN3Sub = (uint)offSet + (uint)Marshal.SizeOf(typeof(SN3HeaderStruct));

            for (int index = 0; index < sN3Header.data_num; index++)
            {
                sN3HeaderSub = ByteUtil.BytesToType<SN3HeaderSubStruct>(data, offSetSN3Sub);
                sN3SubList.Add(sN3HeaderSub);

                LogWriter.SetToBuff(filePath, string.Format("   Data部({0})offset\t<{1}H>({2})", (index + 1), ByteUtil.IntToHex8(sN3HeaderSub.offset), sN3HeaderSub.offset));
                LogWriter.SetToBuff(filePath, string.Format("   Data部({0})size\t<{1}H>({2})", (index + 1), ByteUtil.IntToHex8(sN3HeaderSub.size), sN3HeaderSub.size));

                offSetSN3Sub += (uint)Marshal.SizeOf(typeof(SN3HeaderSubStruct));
            }

            index = 0;
            foreach (SN3HeaderSubStruct sn3Sub in sN3SubList)
            {
                index++;
                LogWriter.SetToBuff(filePath, string.Format("   /SHデータ({0})/", index));
                PLGanaData(EnumPLGanaData.SH, offSet + sn3Sub.offset, sn3Sub.size, data, filePath);
            }
        }

        /// <summary>
        /// Decode SDHXHeader
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeSDHXHeader(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));

            SD3HeaderStruct sD3Header = ByteUtil.BytesToType<SD3HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(sD3Header.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(sD3Header.version)));
            LogWriter.SetToBuff(filePath, string.Format("  SDHデータPointer\t<{0}H>({1})", ByteUtil.IntToHex8(sD3Header.pointer), sD3Header.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  SDHデータSize\t<{0}H>({1})", ByteUtil.IntToHex8(sD3Header.size), sD3Header.size));

            PLGanaData(EnumPLGanaData.SDH, offSet + sD3Header.pointer, sD3Header.size, data, filePath);
        }

        /// <summary>
        /// Decode GTN
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeGTN(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /GTNデータあります/"));
        }

        /// <summary>
        /// Decode GTC
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeGTC(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /GTCデータあります/"));
        }

        /// <summary>
        /// Decode VO4B
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeVO4B(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VO4Bデータあります/"));
        }

        /// <summary>
        /// Decode VO4F
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeVO4F(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VO4Fデータあります"));
        }

        /// <summary>
        /// Decode VS4B
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeVS4B(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VS4Bデータあります/"));
        }

        /// <summary>
        /// Decode VS4F
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeVS4F(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VS4Fデータあります/"));
        }


        /// <summary>
        /// Decode CA
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeCA(ulong offSet, ulong size, byte[] data, string filePath)
        {
            CAHeaderStruct cAHeader = ByteUtil.BytesToType<CAHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("    Data ID\t[{0}]", ByteUtil.ConvertBytesToString(cAHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("    Version番号\t[{0}]", ByteUtil.ConvertBytesToString(cAHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("    Data size\t<{0}H>({1})", ByteUtil.IntToHex8(cAHeader.data_size), cAHeader.data_size));
            LogWriter.SetToBuff(filePath, string.Format("    Codec type\t"), false);
            PLFUtil.PLFanaType(EnumPLFanaType.CodecType, cAHeader.codec_type, filePath);
            // 予備(1)
            //展開情報

            EXPHeaderStruct eXPHeader = new EXPHeaderStruct();
            uint offSetExp = (uint)offSet + (uint)Marshal.SizeOf(typeof(CAHeaderStruct));
            eXPHeader = ByteUtil.BytesToType<EXPHeaderStruct>(data, offSetExp);

            LogWriter.SetToBuff(filePath, string.Format("    /展開情報/"));
            LogWriter.SetToBuff(filePath, string.Format("    レコード数\t({0})", eXPHeader.record_num));
            // 予備(2)

            EXPRecordStruct eXPRecord = new EXPRecordStruct();

            uint offSetRecord = offSetExp + (uint)Marshal.SizeOf(typeof(EXPHeaderStruct));

            // 項目名行を追加
            LogWriter.SetToBuff(filePath, string.Format("\t\t\t\tレコード\t展開前位置\t展開後位置"));

            for (int index = 0; index < eXPHeader.record_num; index++)
            {
                eXPRecord = ByteUtil.BytesToType<EXPRecordStruct>(data, offSetRecord);

                LogWriter.SetToBuff(filePath, string.Format("\t\t\t\t展開情報({0})\t({1})\t({2})", index + 1, eXPRecord.mae_posi, eXPRecord.ato_posi));

                offSetRecord += (uint)Marshal.SizeOf(typeof(EXPRecordStruct));
            }
        }

        /// <summary>
        /// Decode SN3
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeSN3(ulong offSet, ulong size, byte[] data, string filePath)
        {
            SN3HeaderStruct sN3 = ByteUtil.BytesToType<SN3HeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("  /SN3データ/"));
            LogWriter.SetToBuff(filePath, string.Format("   File ID\t[{0}]", ByteUtil.ConvertBytesToString(sN3.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号\t<{0}H>", ByteUtil.IntToHex2(sN3.version)));
            LogWriter.SetToBuff(filePath, string.Format("   Data数\t({0})", sN3.data_num));

            List<SN3HeaderSubStruct> sN3SubList = new List<SN3HeaderSubStruct>();
            SN3HeaderSubStruct sN3HeaderSub = new SN3HeaderSubStruct();
            uint offSetSN3Sub = (uint)offSet + (uint)Marshal.SizeOf(typeof(SN3HeaderStruct));
            for (int index = 0; index < sN3.data_num; index++)
            {
                sN3HeaderSub = ByteUtil.BytesToType<SN3HeaderSubStruct>(data, offSetSN3Sub);
                sN3SubList.Add(sN3HeaderSub);

                LogWriter.SetToBuff(filePath, string.Format("   Data部({0})offset\t<{1}H>({2})", (index + 1), ByteUtil.IntToHex8(sN3HeaderSub.offset), sN3HeaderSub.offset));
                LogWriter.SetToBuff(filePath, string.Format("   Data部({0})size\t<{1}H>({2})", (index + 1), ByteUtil.IntToHex8(sN3HeaderSub.size), sN3HeaderSub.size));

                offSetSN3Sub += (uint)Marshal.SizeOf(typeof(SN3HeaderSubStruct));
            }

            index = 0;
            foreach (SN3HeaderSubStruct sn3Sub in sN3SubList)
            {
                index++;
                LogWriter.SetToBuff(filePath, string.Format("   /CAデータ({0})/", index));
                PLGanaData(EnumPLGanaData.CA, offSet + sn3Sub.offset, sn3Sub.size, data, filePath);
            }
        }

        /// <summary>
        /// Decode SDXHeader
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeSDXHeader(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            SD3HeaderStruct sD3Header = ByteUtil.BytesToType<SD3HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(sD3Header.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(sD3Header.version)));
            LogWriter.SetToBuff(filePath, string.Format("  SN3データPointer\t<{0}H>({1})", ByteUtil.IntToHex8(sD3Header.pointer), sD3Header.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  SN3データSize\t<{0}H>({1})", ByteUtil.IntToHex8(sD3Header.size), sD3Header.size));

            PLGanaData(EnumPLGanaData.SN3, offSet + sD3Header.pointer, sD3Header.size, data, filePath);
        }

        /// <summary>
        /// Decode MDXHeader
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeMDXHeader(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));

            MDXHeaderStruct mdxHeader = ByteUtil.BytesToType<MDXHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(mdxHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(mdxHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("  MIDデータPointer\t<{0}H>({1})", ByteUtil.IntToHex8(mdxHeader.pointer), mdxHeader.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  MIDデータSize\t<{0}H>({1})", ByteUtil.IntToHex8(mdxHeader.size), mdxHeader.size));

            PLFUtil.PLFanaData(EnumPLFanaData.MID, offSet + mdxHeader.pointer, mdxHeader.size, data, filePath);
        }

        /// <summary>
        /// Decode TRG
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeTRG(ulong offSet, ulong size, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));

            DIVHeaderStruct divHeader = ByteUtil.BytesToType<DIVHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(divHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(divHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("  SNGデータPointer\t<{0}H>({1})", ByteUtil.IntToHex8(divHeader.pointer), divHeader.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  SNGデータSize\t<{0}H>({1})", ByteUtil.IntToHex8(divHeader.size), divHeader.size));

            PLFUtil.PLFanaData(EnumPLFanaData.SNG, offSet + divHeader.pointer, size, data, filePath);
        }

        /// <summary>
        /// Decode CHG
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeCHG(ulong offSet, ulong size, byte[] data, string filePath)
        {
            CHGHeaderStruct chgHeader = ByteUtil.BytesToType<CHGHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("/共通管理データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID\t[{0}]", ByteUtil.ConvertBytesToString(chgHeader.file_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号\t<{0}H>", ByteUtil.IntToHex4(chgHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("  リビジョン\t[{0}{1}]", Convert.ToChar(chgHeader.revision[0]), Convert.ToChar(chgHeader.revision[1])));
            LogWriter.SetToBuff(filePath, string.Format("  有効期限開始日\t({0})", chgHeader.kigen_start));
            LogWriter.SetToBuff(filePath, string.Format("  有効期限終了日\t({0})", chgHeader.kigen_end));
            LogWriter.SetToBuff(filePath, string.Format("  曲番号\t({0})", chgHeader.song_no));
            LogWriter.SetToBuff(filePath, string.Format("  コンテンツ判別フラグ\t"), false);
            PLGanaType(EnumPLGanaType.CONTENT_FLAG, BitConverter.GetBytes(chgHeader.content_flag), filePath);

            LogWriter.SetToBuff(filePath, string.Format("  TRG優先フラグ\t({0})", chgHeader.trg_priority_flag));
            LogWriter.SetToBuff(filePath, string.Format("  有償(プレミアム)コンテンツフラグ\t({0})", chgHeader.premium_contents_flag));
            // 予備(2)
            LogWriter.SetToBuff(filePath, string.Format("  バックアップフラグ\t({0})", chgHeader.backup_flag));
            LogWriter.SetToBuff(filePath, string.Format("  再生時間\t({0})", chgHeader.play_time));
            LogWriter.SetToBuff(filePath, string.Format("  管理曲所有会社番号\t"), false);
            PLCUtil.PLCanaType(EnumCanaType.管理曲所有会社番号, chgHeader.kanri_kyoku_syoyukaisha, filePath);
            LogWriter.SetToBuff(filePath, string.Format("  作成日時\t"), false);
            PLCUtil.PLCanaType(EnumCanaType.作成日時, chgHeader.maked_date, filePath); //BitConverter.GetBytes(
            LogWriter.SetToBuff(filePath, string.Format(""));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ1Pointer\t<{0}H>({1})", ByteUtil.IntToHex8(chgHeader.kobetsu_data_pointer), chgHeader.kobetsu_data_pointer));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ1Size\t<{0}H>({1})", ByteUtil.IntToHex8(chgHeader.kobetsu_data_size), chgHeader.kobetsu_data_size));


            LogWriter.SetToBuff(filePath, string.Format("  個別データ2Pointer\t<{0}H>({1})", ByteUtil.IntToHex8(chgHeader.kobetsu_data2_pointer), chgHeader.kobetsu_data2_pointer));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ2Size\t<{0}H>({1})", ByteUtil.IntToHex8(chgHeader.kobetsu_data2_size), chgHeader.kobetsu_data2_size));

            // 予備(4)  
            LogWriter.SetToBuff(filePath, string.Format("  サービスデータ数\t({0})", chgHeader.service_num));

            // 予備(2)
            // サービスデータ
            uint offSetService = (uint)offSet + (uint)Marshal.SizeOf(typeof(CHGHeaderStruct));
            DecodeCHGService(offSetService, chgHeader.service_num, data, filePath);

            // 個別データ
            PLFUtil.PLFanaData(EnumPLFanaData.MT, offSet + chgHeader.kobetsu_data_pointer, chgHeader.kobetsu_data_size, data, filePath);

            // 個別データ2
            if (chgHeader.kobetsu_data2_size > 0)
            {
                PLFUtil.PLFanaData(EnumPLFanaData.DB2, chgHeader.kobetsu_data2_pointer + offSet, chgHeader.kobetsu_data2_size, data, filePath);
            }
        }

        /// <summary>
        /// Decode CHGService
        /// </summary>
        /// <param name="offSet">offset data</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="fileOutputPath">fileOutputPath</param>
        private static void DecodeCHGService(uint offSet, int serviceNum, byte[] data, string filePath)
        {
            CHGServiceStruct chgService = new CHGServiceStruct();

            for (int index = 0; index < serviceNum; index++)
            {
                chgService = ByteUtil.BytesToType<CHGServiceStruct>(data, offSet);

                LogWriter.SetToBuff(filePath, string.Format("  /サービスデータ/\t({0})", index + 1));

                LogWriter.SetToBuff(filePath, string.Format("   サービスデータタイプ\t"), false);
                PLFUtil.PLFanaType(EnumPLFanaType.サービスデータタイプ, chgService.data_type, filePath);

                LogWriter.SetToBuff(filePath, string.Format("   機種フラグ\t({0})", chgService.kind_type));

                // 予備(2)
                LogWriter.SetToBuff(filePath, string.Format("   サービスデータファイル\t"), false);

                PLGanaType(EnumPLGanaType.SERVICE_DATA, BitConverter.GetBytes(chgService.data_file), filePath);
                // 予備(4)
                offSet += (uint)Marshal.SizeOf(typeof(CHGServiceStruct));
            }
        }

        /// <summary>
        ///  Main Decode PLG Type
        /// </summary>
        /// <param name="type">EnumPLGanaType</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        public static void PLGanaType(EnumPLGanaType type, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumPLGanaType.CONTENT_FLAG:
                    PLGTypeConentFlag(data, filePath);
                    break;
                case EnumPLGanaType.SH:
                    PLGTypeSH(data, filePath);
                    break;
                case EnumPLGanaType.SERVICE_DATA:
                    PLGTypeSeviceData(data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///  Decode TypeSeviceData
        /// </summary>       
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void PLGTypeSeviceData(byte[] data, string filePath)
        {
            uint cType = BitConverter.ToUInt32(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex8(cType)), false);
            string ctypeString = string.Empty;

            if ((cType & 0x00000001) != 0)
                ctypeString += "[CHG]";
            if ((cType & 0x00000002) != 0)
                ctypeString += "[TRG]";
            if ((cType & 0x00000004) != 0)
                ctypeString += "[VS4B]";
            if ((cType & 0x00000008) != 0)
                ctypeString += "[VO4B]";
            if ((cType & 0x00000010) != 0)
                ctypeString += "[VS4F]";
            if ((cType & 0x00000020) != 0)
                ctypeString += "[VO4F]";
            if ((cType & 0x00000040) != 0)
                ctypeString += "[GTN]";
            if ((cType & 0x00000080) != 0)
                ctypeString += "[GTC]";
            if ((cType & 0x00000100) != 0)
                ctypeString += "[SSY]";
            if ((cType & 0x00010000) != 0)
                ctypeString += "[MDN]";
            if ((cType & 0x00020000) != 0)
                ctypeString += "[MDB]";
            if ((cType & 0x00040000) != 0)
                ctypeString += "[MDF]";
            if ((cType & 0x01000000) != 0)
                ctypeString += "[SDN]";
            if ((cType & 0x02000000) != 0)
                ctypeString += "[SDB]";
            if ((cType & 0x04000000) != 0)
                ctypeString += "[SDF]";
            if ((cType & 0x08000000) != 0)
                ctypeString += "[SDHN]";
            if ((cType & 0x10000000) != 0)
                ctypeString += "[SDHB]";
            if ((cType & 0x20000000) != 0)
                ctypeString += "[SDHF]";

            LogWriter.SetToBuff(filePath, ctypeString, false);
            LogWriter.SetToBuff(filePath, string.Format(""));
        }

        /// <summary>
        ///  Decode TypeSH
        /// </summary>       
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void PLGTypeSH(byte[] data, string filePath)
        {
            uint cType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>={1}", ByteUtil.IntToHex2(cType), cType));
        }

        /// <summary>
        ///  Decode TypeConentFlag
        /// </summary>       
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void PLGTypeConentFlag(byte[] data, string filePath)
        {
            uint vFlag = BitConverter.ToUInt32(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex8(vFlag)), false);
        	if ((vFlag & 0x7777) == 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[設定無し]"), false);
            }
        	else
        	{
            	if ((vFlag & 0x00000001) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ギタナビ(NB)]"), false);
            	}
            	if ((vFlag & 0x00000100) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ギタナビ(SB)]"), false);
            	}
            	if ((vFlag & 0x00010000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ギタナビ(BB)]"), false);
            	}
            	if ((vFlag & 0x01000000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ギタナビ(HB)]"), false);
            	}
            	if ((vFlag & 0x00000002) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[生演奏(NB)]"), false);
            	}
            	if ((vFlag & 0x00000200) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[生演奏(SB)]"), false);
            	}
            	if ((vFlag & 0x00020000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[生演奏(BB)]"), false);
            	}
            	if ((vFlag & 0x02000000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[生演奏(HB)]"), false);
            	}
            	if ((vFlag & 0x00000004) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ガイドボーカル(NB)]"), false);
            	}
            	if ((vFlag & 0x00000400) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ガイドボーカル(SB)]"), false);
            	}
            	if ((vFlag & 0x00040000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ガイドボーカル(BB)]"), false);
            	}
            	if ((vFlag & 0x04000000) != 0)
            	{
            	    LogWriter.SetToBuff(filePath, string.Format("[ガイドボーカル(HB)]"), false);
            	}
        	}
            LogWriter.SetToBuff(filePath, string.Format(""));
        }
    }
}
