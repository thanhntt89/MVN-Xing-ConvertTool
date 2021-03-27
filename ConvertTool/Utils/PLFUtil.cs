using PLTextToolTXT.Common;
using PLTextToolTXT.FileType.PLF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PLTextToolTXT.Utils
{
    public class PLFUtil
    {
        private static int index = 0;

        /// <summary>
        /// DecodeHeader
        /// </summary>
        /// <param name="outPuthFileName">file output path</param>
        /// <param name="data">data all</param>
        /// <returns></returns>
        public static int DecodeHeader(string outPuthFileName, byte[] data)
        {
            // Reading file plf header           
            PLFHeaderStruct plfheader = new PLFHeaderStruct();

            try
            {
                plfheader = ByteUtil.BytesToType<PLFHeaderStruct>(data);

                LogWriter.SetToBuff(outPuthFileName, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(plfheader.file_id)));
                LogWriter.SetToBuff(outPuthFileName, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex4(plfheader.version)));
                LogWriter.SetToBuff(outPuthFileName, string.Format("  ファイル分割数:({0})", plfheader.divide_count));
            }
            catch 
            {
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// DecodeDivideData
        /// </summary>
        /// <param name="offSet">offset Divide Data</param>
        /// <param name="data">data all </param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="outPuthFileName">file output path</param>
        /// <returns></returns>
        public static int DecodeDivideData(uint offSet, byte[] data, int divideCount, string outPuthFileName)
        {
            // Reading file plf header           
            PLFDivideDataStruct divide = new PLFDivideDataStruct();
            uint offSetDive = offSet;
            try
            {
                for (int i = 0; i < divideCount; i++)
                {
                    LogWriter.SetToBuff(outPuthFileName, string.Format("/分割データ管理情報/:({0})", i + 1));

                    divide = ByteUtil.BytesToType<PLFDivideDataStruct>(data, offSetDive);

                    LogWriter.SetToBuff(outPuthFileName, string.Format("  分割データPointer:<{0}H>({1})", ByteUtil.IntToHex8(divide.pointer), divide.pointer));
                    LogWriter.SetToBuff(outPuthFileName, string.Format("  分割データSize:<{0}H>({1})", ByteUtil.IntToHex8(divide.size), divide.size));
                    LogWriter.SetToBuff(outPuthFileName, string.Format("  登録データフラグ:"), false);
                    PLFanaType(EnumPLFanaType.登録データフラグ, BitConverter.GetBytes(divide.entry_flag), outPuthFileName);
                    LogWriter.SetToBuff(outPuthFileName, string.Format(""), true);
                    LogWriter.SetToBuff(outPuthFileName, string.Format("  分割データMD5:"), false);
                    PLCUtil.PLCanaType(EnumCanaType.MD5, divide.md5, outPuthFileName);
                    LogWriter.SetToBuff(outPuthFileName, string.Format(""), true);
                    // MD5 Check 
                    PLEUtil.PLEanaData(EnumEanaData.MD5, (uint)divide.pointer, (uint)divide.size, data, outPuthFileName);
                    LogWriter.SetToBuff(outPuthFileName, string.Format(""), true);
                    LogWriter.SetToBuff(outPuthFileName, string.Format("  ファイル拡張子:[{0}]", ByteUtil.ConvertBytesToString(divide.extension)));

                    // Next block PLFDivideDataStruct
                    offSetDive += (uint)Marshal.SizeOf(typeof(PLFDivideDataStruct));
                }
            }
            catch
            {               
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// DecodeExtension
        /// </summary>
        /// <param name="offSet">Offset extension</param>
        /// <param name="data">data all</param>
        /// <param name="divideCount">divideCount</param>
        /// <param name="filePath">file output path</param>
        /// <returns></returns>
        public static int DecodeExtension(uint offSet, byte[] data, int divideCount, string filePath)
        {
            // Reading file plf header           
            PLFDivideDataStruct divide = new PLFDivideDataStruct();

            try
            {
                while (divideCount > 0)
                {
                    divide = ByteUtil.BytesToType<PLFDivideDataStruct>(data, offSet);
                    EnumPLFanaData extentsion = Utilities.ParseEnum<EnumPLFanaData>(ByteUtil.ConvertBytesToString(divide.extension));

                    // Decode
                    PLFanaData(extentsion, divide.pointer, divide.size, data, filePath);

                    // Next block PLFDivideDataStruct
                    offSet += (uint)Marshal.SizeOf(typeof(PLFDivideDataStruct));

                    divideCount--;                    
                }
            }
            catch 
            {
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// Main decode PLF Data
        /// </summary>
        /// <param name="type">Enum PLFanaData</param>
        /// <param name="offSet">offset DecodeVSN</param>
        /// <param name="size">size of PLFanaData data</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        public static void PLFanaData(EnumPLFanaData type, ulong offSet, ulong size, byte[] data, string filePath)
        {
            // Decode         
            switch (type)
            {
                case EnumPLFanaData.CHF:
                    DecodeDataCHFHeader(offSet, data, filePath);
                    break;
                case EnumPLFanaData.TRF:
                    DecodeDataTRFHeader(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MDN:
                case EnumPLFanaData.MDB:
                case EnumPLFanaData.MDC:
                    DecodeMDXHeader(offSet, data, filePath);
                    break;
                case EnumPLFanaData.SDN:
                case EnumPLFanaData.SDB:
                case EnumPLFanaData.SDC:
                    DecodeSD3Header(offSet, data, filePath);
                    break;
                case EnumPLFanaData.VSN:
                    DecodeVSN(offSet, data, filePath);
                    break;
                case EnumPLFanaData.VS3:
                    DecodeVS3(offSet, data, filePath);
                    break;
                case EnumPLFanaData.VON:
                    DecodeVON(offSet, data, filePath);
                    break;
                case EnumPLFanaData.VO3:
                    DecodeVO3(offSet, data, filePath);
                    break;
                case EnumPLFanaData.GTN:
                    DecodeGTN(offSet, data, filePath);
                    break;
                case EnumPLFanaData.GTC:
                    DecodeGTC(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MT:
                    DecodeDataMT(offSet, data, filePath);
                    break;
                case EnumPLFanaData.DB:
                    DcodeDataDBHeader(offSet, data, filePath);
                    break;
                case EnumPLFanaData.SNG:
                    DecodeDataSNG(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MID:
                    DecodeDataMID(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MID_d:
                    DecodeDataMIDData(offSet, size, data, filePath);
                    break;
                case EnumPLFanaData.DB2:
                    DecodeDataDB2Header(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MDT:
                    DecodeDataMDT(offSet, data, filePath);
                    break;
                case EnumPLFanaData.MIDI:
                    DecodeDataMIDI(offSet, data, filePath);
                    break;
                case EnumPLFanaData.SN3:
                    DecodeDataSN3(offSet, data, filePath);
                    break;
                case EnumPLFanaData.CA:
                    DecodeDataCA(offSet, data, filePath);
                    break;
                case EnumPLFanaData.TLP:
                    DecodeDataTLPHeader(offSet, data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// DecodeVSN
        /// </summary>
        /// <param name="offSet">offset DecodeVSN</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeVSN(ulong offSet, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VSNデータあります/"));
        }

        /// <summary>
        /// DecodeVS3
        /// </summary>
        /// <param name="offSet">offset DecodeVS3</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeVS3(ulong offSet, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VS3データあります/"));
        }

        /// <summary>
        /// DecodeVON
        /// </summary>
        /// <param name="offSet">offset DecodeVON</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeVON(ulong offSet, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VONデータあります/"));
        }

        /// <summary>
        /// DecodeVO3
        /// </summary>
        /// <param name="offSet">offset DecodeVO3</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeVO3(ulong offSet, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format(" /VO3データあります/"));
        }

        /// <summary>
        /// DecodeGTN
        /// </summary>
        /// <param name="offSet">offset DecodeGTN</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeGTN(ulong offSet, byte[] data, string filePath)
        {
            GTNHeaderStruct gtnHeader = ByteUtil.BytesToType<GTNHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(gtnHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex2(gtnHeader.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("  サービス種別:"), false);

            PLFanaType(EnumPLFanaType.サービス種別, gtnHeader.service, filePath);
            LogWriter.SetToBuff(filePath, string.Format("  GCC部Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gcc_offset), gtnHeader.gcc_offset));
            LogWriter.SetToBuff(filePath, string.Format("  GCC部Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gcc_size), gtnHeader.gcc_size));

            LogWriter.SetToBuff(filePath, string.Format("  GCD部Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gcd_offset), gtnHeader.gcd_offset));
            LogWriter.SetToBuff(filePath, string.Format("  GCD部Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gcd_size), gtnHeader.gcd_size));

            LogWriter.SetToBuff(filePath, string.Format("  GRM部Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.grm_offset), gtnHeader.grm_offset));
            LogWriter.SetToBuff(filePath, string.Format("  GRM部Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.grm_size), gtnHeader.grm_size));

            LogWriter.SetToBuff(filePath, string.Format("  GNT部Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gnt_offset), gtnHeader.gnt_offset));
            LogWriter.SetToBuff(filePath, string.Format("  GNT部Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gnt_size), gtnHeader.gnt_size));

            LogWriter.SetToBuff(filePath, string.Format("  GSR部Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gsr_offset), gtnHeader.gsr_offset));
            LogWriter.SetToBuff(filePath, string.Format("  GSR部Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.gsr_size), gtnHeader.gsr_size));

            LogWriter.SetToBuff(filePath, string.Format("  ENC部B Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_b_offset), gtnHeader.enc_b_offset));
            LogWriter.SetToBuff(filePath, string.Format("  ENC部B Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_b_size), gtnHeader.enc_b_size));

            LogWriter.SetToBuff(filePath, string.Format("  ENC部G1 Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g1_offset), gtnHeader.enc_g1_offset));
            LogWriter.SetToBuff(filePath, string.Format("  ENC部G1 Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g1_size), gtnHeader.enc_g1_size));

            LogWriter.SetToBuff(filePath, string.Format("  ENC部G2 Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g2_offset), gtnHeader.enc_g2_offset));
            LogWriter.SetToBuff(filePath, string.Format("  ENC部G2 Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g2_size), gtnHeader.enc_g2_size));

            LogWriter.SetToBuff(filePath, string.Format("  ENC部G3 Offset:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g3_offset), gtnHeader.enc_g3_offset));
            LogWriter.SetToBuff(filePath, string.Format("  ENC部G3 Size:<{0}H>({1})", ByteUtil.IntToHex8(gtnHeader.enc_g3_size), gtnHeader.enc_g3_size));
        }

        /// <summary>
        /// DecodeGTC
        /// </summary>
        /// <param name="offSet">offset DecodeGTC</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeGTC(ulong offSet, byte[] data, string filePath)
        {
            GTCHeaderStruct gtcHeader = ByteUtil.BytesToType<GTCHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(gtcHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex2(gtcHeader.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("  データ数:({0})", gtcHeader.count[0]));

            // Next block GNCInfoStruct
            offSet += (uint)Marshal.SizeOf(typeof(GTCHeaderStruct));
            GNCInfoStruct gncInfo = new GNCInfoStruct();
            for (int index = 0; index < gtcHeader.count[0]; index++)
            {
                gncInfo = ByteUtil.BytesToType<GNCInfoStruct>(data, (uint)offSet);

                LogWriter.SetToBuff(filePath, string.Format("  /GNC部{0} Offset:<{1}H>({2})", index + 1, ByteUtil.IntToHex8(gncInfo.gnc_offset), gncInfo.gnc_offset));

                LogWriter.SetToBuff(filePath, string.Format("  /GNC部{0} Size:<{1}H>({2})", index + 1, ByteUtil.IntToHex8(gncInfo.gnc_size), gncInfo.gnc_size));
                // Next block GNCInfoStruct
                offSet += (uint)Marshal.SizeOf(typeof(GNCInfoStruct));
            }
        }

        /// <summary>
        /// DecodeDataCHFHeader
        /// </summary>
        /// <param name="offSet">offset DecodeDataCHFHeader</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataCHFHeader(ulong offSet, byte[] data, string filePath)
        {
            byte[] buff = ByteUtil.GetBytes(data, (uint)offSet);

            CHFHeaderStruct chfHeader = ByteUtil.BytesToType<CHFHeaderStruct>(buff);

            LogWriter.SetToBuff(filePath, string.Format("/共通管理データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(chfHeader.file_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex4(chfHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("  リビジョン:[{0}{1}]", Convert.ToChar(chfHeader.revision[0]), Convert.ToChar(chfHeader.revision[1])));
            LogWriter.SetToBuff(filePath, string.Format("  有効期限開始日:({0})", chfHeader.kigen_start));
            LogWriter.SetToBuff(filePath, string.Format("  有効期限終了日:({0})", chfHeader.kigen_end));
            LogWriter.SetToBuff(filePath, string.Format("  曲番号:({0})", chfHeader.song_no));
            LogWriter.SetToBuff(filePath, string.Format("  コンテンツ判別フラグ:"), false);
            PLFanaType(EnumPLFanaType.コンテンツ判別フラグ, chfHeader.content_flag, filePath);
            LogWriter.SetToBuff(filePath, string.Format(""), true);
            LogWriter.SetToBuff(filePath, string.Format("  TRF優先フラグ:({0})", chfHeader.trf_priority_flag));
            LogWriter.SetToBuff(filePath, string.Format("  バックアップフラグ:({0})", chfHeader.backup_flag));
            LogWriter.SetToBuff(filePath, string.Format("  再生時間:({0})", chfHeader.play_time));
            LogWriter.SetToBuff(filePath, string.Format("  管理曲所有会社番号:"), false);
            PLCUtil.PLCanaType(EnumCanaType.管理曲所有会社番号, chfHeader.kanri_kyoku_syoyukaisha, filePath);
            LogWriter.SetToBuff(filePath, string.Format(""), false);
            LogWriter.SetToBuff(filePath, string.Format("  作成日時:"), false);
            PLCUtil.PLCanaType(EnumCanaType.作成日時, BitConverter.GetBytes(chfHeader.maked_date), filePath);
            LogWriter.SetToBuff(filePath, string.Format(""), true);
            LogWriter.SetToBuff(filePath, string.Format("  個別データ1Pointer:<{0}H>({1})", ByteUtil.IntToHex8(chfHeader.kobetsu_data_pointer), chfHeader.kobetsu_data_pointer));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ1Size:<{0}H>({1})", ByteUtil.IntToHex8(chfHeader.kobetsu_data_size), chfHeader.kobetsu_data_size));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ2Pointer:<{0}H>({1})", ByteUtil.IntToHex8(chfHeader.kobetsu_data2_pointer), chfHeader.kobetsu_data2_pointer));
            LogWriter.SetToBuff(filePath, string.Format("  個別データ2Size:<{0}H>({1})", ByteUtil.IntToHex8(chfHeader.kobetsu_data2_size), chfHeader.kobetsu_data2_size));
            LogWriter.SetToBuff(filePath, string.Format("  サービスデータ数:({0})", chfHeader.service_num));

            // Removed header ChfHeaderStruct
            buff = ByteUtil.RemovedBytes<CHFHeaderStruct>(buff);

            // サービスデータ
            DecodeCHFSevice(chfHeader.service_num, buff, filePath);

            //個別データ
            PLFanaData(EnumPLFanaData.MT, chfHeader.kobetsu_data_pointer + offSet, chfHeader.kobetsu_data_size, data, filePath);

            // 個別データ2
            if (chfHeader.kobetsu_data2_size > 0)
            {
                PLFanaData(EnumPLFanaData.DB2, chfHeader.kobetsu_data2_pointer + offSet, chfHeader.kobetsu_data2_size, data, filePath);
            }
        }

        /// <summary>
        /// DecodeCHFSevice
        /// </summary>
        /// <param name="offSet">offset DecodeCHFSevice</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeCHFSevice(int service_num, byte[] data, string filePath)
        {
            CHFServiceStruct chfService;
            uint offSet = 0;
            for (int i = 0; i < service_num; i++)
            {
                chfService = ByteUtil.BytesToType<CHFServiceStruct>(data, offSet);

                LogWriter.SetToBuff(filePath, string.Format("  /サービスデータ/:({0})", i + 1));
                LogWriter.SetToBuff(filePath, string.Format("   サービスデータタイプ:"), false);
                PLFanaType(EnumPLFanaType.サービスデータタイプ, chfService.data_type, filePath);
                LogWriter.SetToBuff(filePath, string.Format("   サービスデータファイル:"), false);
                PLFanaType(EnumPLFanaType.サービスデータファイル, chfService.data_file, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""), true);
                // Next block
                offSet += (uint)Marshal.SizeOf(typeof(CHFServiceStruct));
            }
        }

        /// <summary>
        /// DecodeDataTRFHeader
        /// </summary>
        /// <param name="offSet">offset DecodeDataTRFHeader</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataTRFHeader(ulong offSet, byte[] data, string filePath)
        {
            DIVHeaderStruct divHeaderStruct = ByteUtil.BytesToType<DIVHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(divHeaderStruct.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex4(divHeaderStruct.version)));
            LogWriter.SetToBuff(filePath, string.Format("  SNGデータPointer:<{0}H>({1})", ByteUtil.IntToHex8(divHeaderStruct.pointer), divHeaderStruct.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  SNGデータSize:<{0}H>({1})", ByteUtil.IntToHex8(divHeaderStruct.size), divHeaderStruct.size));

            //SNGデータ           
            PLFanaData(EnumPLFanaData.SNG, offSet + divHeaderStruct.pointer, divHeaderStruct.size, data, filePath);
        }

        /// <summary>
        /// DecodeSD3Header
        /// </summary>
        /// <param name="offSet">offset DecodeSD3Header</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeSD3Header(ulong offSet, byte[] data, string filePath)
        {
            SD3HeaderStruct sd3Header = ByteUtil.BytesToType<SD3HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(sd3Header.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex4(sd3Header.version)));
            LogWriter.SetToBuff(filePath, string.Format("  SN3データPointer:<{0}H>({1})", ByteUtil.IntToHex8(sd3Header.pointer), sd3Header.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  SN3データSize:<{0}H>({1})", ByteUtil.IntToHex8(sd3Header.size), sd3Header.size));
            // SN3データ
            PLFanaData(EnumPLFanaData.SN3, offSet + sd3Header.pointer, sd3Header.size, data, filePath);
        }

        /// <summary>
        /// DecodeMDXHeader
        /// </summary>
        /// <param name="offSet">offset DecodeMDXHeader</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeMDXHeader(ulong offSet, byte[] data, string filePath)
        {
            MDXHeaderStruct mdxHeader = ByteUtil.BytesToType<MDXHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("/分割データ/"));
            LogWriter.SetToBuff(filePath, string.Format("  File ID:[{0}]", ByteUtil.ConvertBytesToString(mdxHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("  FormatVersion番号:<{0}H>", ByteUtil.IntToHex4(mdxHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("  MIDデータPointer:<{0}H>({1})", ByteUtil.IntToHex8(mdxHeader.pointer), mdxHeader.pointer));
            LogWriter.SetToBuff(filePath, string.Format("  MIDデータSize:<{0}H>({1})", ByteUtil.IntToHex8(mdxHeader.size), mdxHeader.size));

            // MIDデータ
            PLFanaData(EnumPLFanaData.MID, offSet + mdxHeader.pointer, mdxHeader.size, data, filePath);
        }

        /// <summary>
        /// DecodeDataMT
        /// </summary>
        /// <param name="offSet">offset DecodeDataMT</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataMT(ulong offSet, byte[] data, string filePath)
        {
            MTHeaderStruct mtHeaderStruct;
            LogWriter.SetToBuff(filePath, string.Format("  /個別データ/"));
            // Get data from offset           
            mtHeaderStruct = ByteUtil.BytesToType<MTHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("   File ID:[{0}]", ByteUtil.ConvertBytesToString(mtHeaderStruct.data_id)));
            int version = mtHeaderStruct.version[0];
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号:<{0}H>", ByteUtil.BytesToHex2(mtHeaderStruct.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("   DBデータPointer:<{0}H>({1})", ByteUtil.IntToHex8(mtHeaderStruct.db_pointer), mtHeaderStruct.db_pointer));
            LogWriter.SetToBuff(filePath, string.Format("   DBデータSize:<{0}H>({1})", ByteUtil.IntToHex8(mtHeaderStruct.db_size), mtHeaderStruct.db_size));

            uint dbOffset = (uint)offSet + (uint)Marshal.SizeOf(typeof(MTHeaderStruct));
            PLFanaData(EnumPLFanaData.DB, dbOffset, mtHeaderStruct.db_size, data, filePath);
        }

        /// <summary>
        /// DcodeDataDBHeader
        /// </summary>
        /// <param name="offSet">offset DcodeDataDBHeader</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DcodeDataDBHeader(ulong offSet, byte[] data, string filePath)
        {
            // Get DBHeaderStruct
            DBHeaderStruct dbHeader = ByteUtil.BytesToType<DBHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  /DBデータ/"));
            LogWriter.SetToBuff(filePath, string.Format("   File ID:[{0}]", ByteUtil.ConvertBytesToString(dbHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号:<{0}H>", ByteUtil.BytesToHex2(dbHeader.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("   発表年:<{0}H>", ByteUtil.IntToHex4(dbHeader.happyo_year)));
            LogWriter.SetToBuff(filePath, string.Format("   ジャンルTable:"), false);
            PLCUtil.PLCanaType(EnumCanaType.ジャンルTable, dbHeader.genre_table, filePath);
            LogWriter.SetToBuff(filePath, string.Format(""), true);
            LogWriter.SetToBuff(filePath, string.Format("   ナビ専用曲名offset:({0})", dbHeader.offset_songname));
            LogWriter.SetToBuff(filePath, string.Format("   ナビ専用歌手名offset:({0})", dbHeader.offset_singername));
            LogWriter.SetToBuff(filePath, string.Format("   ナビ専用作詞者名offset:({0})", dbHeader.offset_sakusiname));
            LogWriter.SetToBuff(filePath, string.Format("   ナビ専用作曲者名offset:({0})", dbHeader.offset_sakkyokuname));
            LogWriter.SetToBuff(filePath, string.Format("   原曲キー情報:"), false);
            PLCUtil.PLCanaType(EnumCanaType.原曲キー情報, dbHeader.original_key, filePath);
            LogWriter.SetToBuff(filePath, string.Format("   曲属性Table:"), false);
            PLCUtil.PLCanaType(EnumCanaType.曲属性Table, dbHeader.song_att_table, filePath);
            LogWriter.SetToBuff(filePath, string.Format("   管理曲所有会社番号:"), false);
            PLCUtil.PLCanaType(EnumCanaType.管理曲所有会社番号, dbHeader.kanri_kyoku_syoyukaisha, filePath);
            LogWriter.SetToBuff(filePath, string.Format("   JVジャンル:"), false);
            PLCUtil.PLCanaType(EnumCanaType.JVジャンル, dbHeader.jv_genre, filePath);
            // 予備(3)
            LogWriter.SetToBuff(filePath, string.Format("   文字数:({0})", dbHeader.moji_num));
            // 予備(2)
            LogWriter.SetToBuff(filePath, string.Format("   文字:"), false);
            // Get data PLFanaTAG
            uint buffOfset = (uint)offSet + (uint)Marshal.SizeOf(typeof(DBHeaderStruct));

            byte[] buff = ByteUtil.GetBytes(data, buffOfset, dbHeader.moji_num);

            PLFanaTAG(buff, dbHeader.moji_num, filePath);

            uint startPointer = (uint)offSet;
            int naviSize = 1024;
            byte[] naviInfo = new byte[naviSize];

            // ナビ専用曲名
            if (dbHeader.offset_songname != 0)
            {
                naviInfo = ByteUtil.GetBytesToNull(data, naviSize, startPointer + dbHeader.offset_songname);
                LogWriter.SetToBuff(filePath, string.Format("   ナビ専用曲名:[{0}]", Encoding.Default.GetString(naviInfo)));
            }
            // ナビ専用歌手名
            if (dbHeader.offset_singername != 0)
            {
                naviInfo = ByteUtil.GetBytesToNull(data, naviSize, startPointer + dbHeader.offset_singername);
                LogWriter.SetToBuff(filePath, string.Format("   ナビ専用歌手名:[{0}]", Encoding.Default.GetString(naviInfo)));
            }
            // ナビ専用作詞者名
            if (dbHeader.offset_sakusiname != 0)
            {
                naviInfo = ByteUtil.GetBytesToNull(data, naviSize, startPointer + dbHeader.offset_sakusiname);
                LogWriter.SetToBuff(filePath, string.Format("   ナビ専用作詞者名:[{0}]", Encoding.Default.GetString(naviInfo)));
            }
            // ナビ専用作曲者名
            if (dbHeader.offset_sakkyokuname != 0)
            {
                naviInfo = ByteUtil.GetBytesToNull(data, naviSize, startPointer + dbHeader.offset_sakkyokuname);
                LogWriter.SetToBuff(filePath, string.Format("   ナビ専用作曲者名:[{0}]", Encoding.Default.GetString(naviInfo)));
            }
        }

        /// <summary>
        /// DecodeDataSNG
        /// </summary>
        /// <param name="offSet">offset DecodeDataSNG</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataSNG(ulong offSet, byte[] data, string filePath)
        {
            SNGHeaderStruct sngHeaderStruct = ByteUtil.BytesToType<SNGHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  /SNGデータ/"));
            LogWriter.SetToBuff(filePath, string.Format("   File ID:[{0}]", ByteUtil.ConvertBytesToString(sngHeaderStruct.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号:<{0}H>", ByteUtil.IntToHex2(sngHeaderStruct.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("   Data部個数:({0})", (int)sngHeaderStruct.data_num[0]));
            int dataCount = (int)sngHeaderStruct.data_num[0];

            // Offset SNGDataHeaderStruct
            uint offSetSngData = (uint)offSet + (uint)Marshal.SizeOf(typeof(SNGHeaderStruct));
            List<SNGDataHeaderStruct> sngDataHeaderList = new List<SNGDataHeaderStruct>();

            // 予備(3)           
            for (int index = 0; index < dataCount; index++)
            {
                SNGDataHeaderStruct sngDataStruct = ByteUtil.BytesToType<SNGDataHeaderStruct>(data, offSetSngData);
                sngDataHeaderList.Add(sngDataStruct);

                LogWriter.SetToBuff(filePath, string.Format("   /DATA部/:({0})", index + 1));
                LogWriter.SetToBuff(filePath, string.Format("    DATA部offset:<{0}H>({1})", ByteUtil.IntToHex8(sngDataStruct.pointer), sngDataStruct.pointer));
                LogWriter.SetToBuff(filePath, string.Format("    DATA部size:<{0}H>({1})", ByteUtil.IntToHex8(sngDataStruct.size), sngDataStruct.size));
                LogWriter.SetToBuff(filePath, string.Format("    DATA部圧縮前size:<{0}H>({1})", ByteUtil.IntToHex8(sngDataStruct.src_size), sngDataStruct.src_size));
                // Next SNGDataHeaderStruct
                offSetSngData += (uint)Marshal.SizeOf(typeof(SNGDataHeaderStruct));
            }

            int resource = 0;
            index = 0;

            // DATA部をファイル化し一時ファイルに解凍する
            foreach (SNGDataHeaderStruct sngData in sngDataHeaderList)
            {
                index++;

                // DATA部をファイル化し一時ファイルに解凍する
                resource = PLFDecode(data, sngData.pointer + offSet, sngData.size, sngData.src_size, Constant.TMP_PLF_FILE_PATH);

                if (resource != 0)
                {
                    LogWriter.SetToBuff(filePath, string.Format("   /DATA部/:({0})解凍失敗", index));

                    continue;
                }

                // TLP
                byte[] buff = File.ReadAllBytes(Constant.TMP_PLF_FILE_PATH);
                PLFanaData(EnumPLFanaData.TLP, 0, sngData.src_size, buff, filePath);
                // Delete tmp file
                File.Delete(Constant.TMP_PLF_FILE_PATH);
            }

            sngDataHeaderList = null;
        }

        /// <summary>
        /// DecodeDataMID
        /// </summary>
        /// <param name="offSet">offset DecodeDataMID</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataMID(ulong offSet, byte[] data, string filePath)
        {
            MIDHeaderStruct midHeader = ByteUtil.BytesToType<MIDHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("  /MIDデータ/"));
            LogWriter.SetToBuff(filePath, string.Format("    File ID:[{0}]", ByteUtil.ConvertBytesToString(midHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("    FormatVersion番号:<{0}H>", ByteUtil.IntToHex2((ushort)midHeader.version[0])));

            int data_num = midHeader.data_num[0];
            LogWriter.SetToBuff(filePath, string.Format("    Data数:({0})", data_num));

            MIDHeaderOffsetStruct midHeaderOffset = new MIDHeaderOffsetStruct();

            // Block start offset MIDHeaderOffsetStruct
            uint offsetHeader = (uint)offSet + (uint)Marshal.SizeOf(typeof(MIDHeaderStruct));

            uint offSetHeaderSize = (uint)Marshal.SizeOf(typeof(MIDHeaderOffsetStruct));
            // List MIDdata
            List<MIDHeaderOffsetStruct> midHeaderList = new List<MIDHeaderOffsetStruct>();

            // Header offset
            for (int index = 0; index < data_num; index++)
            {
                midHeaderOffset = ByteUtil.BytesToType<MIDHeaderOffsetStruct>(data, offsetHeader);
                midHeaderList.Add(midHeaderOffset);
                LogWriter.SetToBuff(filePath, string.Format("    /データ部({0})/", (index + 1)));
                LogWriter.SetToBuff(filePath, string.Format("     Data部 offset:<{0}H>({1})", ByteUtil.IntToHex8(midHeaderOffset.offset), midHeaderOffset.offset));
                LogWriter.SetToBuff(filePath, string.Format("     Data部 size:<{0}H>({1})", ByteUtil.IntToHex8(midHeaderOffset.size), midHeaderOffset.size));
                // Next block MIDHeaderOffsetStruct
                offsetHeader += offSetHeaderSize;
            }

            // Decode MIDData
            foreach (MIDHeaderOffsetStruct midHeaderD in midHeaderList)
            {
                PLFanaData(EnumPLFanaData.MID_d, offSet + midHeaderD.offset, midHeaderD.size, data, filePath);
            }
        }

        /// <summary>
        /// DecodeDataMIDData
        /// </summary>
        /// <param name="offSet">offset DecodeDataMIDData</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataMIDData(ulong offSet, ulong size, byte[] data, string filePath)
        {
            MIDDataStruct midData = ByteUtil.BytesToType<MIDDataStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("     曲データ属性:<{0}H>", ByteUtil.IntToHex4(midData.data_att)));
            LogWriter.SetToBuff(filePath, string.Format("     圧縮前 Size:({0})", midData.data_size));

            //DATA部をファイル化し一時ファイルに解凍する
            offSet += (uint)Marshal.SizeOf(typeof(MIDDataStruct));
            size = size - (uint)Marshal.SizeOf(typeof(MIDDataStruct));

            int rsult = PLFDecode(data, offSet, size, midData.data_size, Constant.TMP_PLF_FILE_PATH);
            if (rsult != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("    /DATA部/:({0})解凍失敗/", (index + 1)));
            }

            // MDT Read all data from decode file
            byte[] buff = File.ReadAllBytes(Constant.TMP_PLF_FILE_PATH);

            // Decode MDT
            PLFanaData(EnumPLFanaData.MDT, 0, midData.data_size, buff, filePath);

            File.Delete(Constant.TMP_PLF_FILE_PATH);
        }

        /// <summary>
        /// DecodeDataDB2Header
        /// </summary>
        /// <param name="offSet">offset DecodeDataDB2Header</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataDB2Header(ulong offSet, byte[] data, string filePath)
        {
            // Pending test
            DB2HeaderStruct db2HeaderStruct = ByteUtil.BytesToType<DB2HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  /個別データ2/"));
            // Get DBHeaderStruct
            LogWriter.SetToBuff(filePath, string.Format("   File ID:[{0}]", ByteUtil.ConvertBytesToString(db2HeaderStruct.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("   FormatVersion番号:<{0}H>", ByteUtil.BytesToHex2(db2HeaderStruct.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("   EINY-ID UM:[{0}]", ByteUtil.ConvertBytesToString(db2HeaderStruct.einy_id_UM)));
            LogWriter.SetToBuff(filePath, string.Format("   動画ID UM:[{0}]", ByteUtil.ConvertBytesToString(db2HeaderStruct.movie_id_UM)));
            LogWriter.SetToBuff(filePath, string.Format("   EINY-ID UV:[{0}]", ByteUtil.ConvertBytesToString(db2HeaderStruct.einy_id_UV)));
            LogWriter.SetToBuff(filePath, string.Format("   動画ID UV:[{0}]", ByteUtil.ConvertBytesToString(db2HeaderStruct.movie_id_UV)));
        }

        /// <summary>
        /// DecodeDataSN3
        /// </summary>
        /// <param name="offSet">offset DecodeDataSN3</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataSN3(ulong offSet, byte[] data, string filePath)
        {
            SN3HeaderStruct sn3Header = ByteUtil.BytesToType<SN3HeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("  /SN3データ/"));
            LogWriter.SetToBuff(filePath, string.Format("    File ID:[{0}]", ByteUtil.ConvertBytesToString(sn3Header.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("    FormatVersion番号:<{0}H>", ByteUtil.IntToHex2(sn3Header.version[0])));

            int data_num = sn3Header.data_num[0];
            LogWriter.SetToBuff(filePath, string.Format("    Data数:({0})", data_num));

            // Next block
            uint sn3SubOffSet = (uint)offSet + (uint)Marshal.SizeOf(typeof(SN3HeaderStruct));

            SN3HeaderSubStruct sn3SubHeader = new SN3HeaderSubStruct();

            List<SN3HeaderSubStruct> sn3SubHeaders = new List<SN3HeaderSubStruct>();

            for (int index = 0; index < data_num; index++)
            {
                sn3SubHeader = ByteUtil.BytesToType<SN3HeaderSubStruct>(data, (uint)sn3SubOffSet);

                sn3SubHeaders.Add(sn3SubHeader);

                LogWriter.SetToBuff(filePath, string.Format("    Data部({0})offset:<{1}H>({2})", index + 1, ByteUtil.IntToHex8(sn3SubHeader.offset), sn3SubHeader.offset));

                LogWriter.SetToBuff(filePath, string.Format("    Data部({0})size:<{1}H>({2})", index + 1, ByteUtil.IntToHex8(sn3SubHeader.size), sn3SubHeader.size));

                // Next block
                sn3SubOffSet += (uint)Marshal.SizeOf(typeof(SN3HeaderSubStruct));
            }

            index = 0;
            // CA
            foreach (SN3HeaderSubStruct sn3sub in sn3SubHeaders)
            {
                index++;
                LogWriter.SetToBuff(filePath, string.Format("    /CAデータ({0})/", index));
                PLFanaData(EnumPLFanaData.CA, sn3sub.offset + offSet, sn3sub.size, data, filePath);
            }
        }

        /// <summary>
        /// DecodeDataCA
        /// </summary>
        /// <param name="offSet">offset DecodeDataCA</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataCA(ulong offSet, byte[] data, string filePath)
        {
            CAHeaderStruct caHeader = ByteUtil.BytesToType<CAHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("     Data ID:[{0}]", ByteUtil.ConvertBytesToString(caHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("     Version番号:[{0}]", ByteUtil.ConvertBytesToString(caHeader.version)));
            LogWriter.SetToBuff(filePath, string.Format("     Data size:<{0}H>({1})", ByteUtil.IntToHex8(caHeader.data_size), caHeader.data_size));

            LogWriter.SetToBuff(filePath, string.Format("     Codec type:"), false);
            PLFanaType(EnumPLFanaType.CodecType, caHeader.codec_type, filePath);

            LogWriter.SetToBuff(filePath, string.Format("     全体機能フラグ:"), false);
            PLFanaType(EnumPLFanaType.全体機能フラグ, caHeader.func_flag, filePath);

            LogWriter.SetToBuff(filePath, string.Format("     データのチャンネル数:({0})", (uint)caHeader.data_channel[0]));
            LogWriter.SetToBuff(filePath, string.Format("     第1チャンネル属性:"));
            PLFanaType(EnumPLFanaType.チャンネル属性, caHeader.channel_att1, filePath);

            LogWriter.SetToBuff(filePath, string.Format("     第2チャンネル属性:"));
            PLFanaType(EnumPLFanaType.チャンネル属性, caHeader.channel_att2, filePath);

            LogWriter.SetToBuff(filePath, string.Format("     第3チャンネル属性:"));
            PLFanaType(EnumPLFanaType.チャンネル属性, caHeader.channel_att3, filePath);

            LogWriter.SetToBuff(filePath, string.Format("     第4チャンネル属性:"));
            PLFanaType(EnumPLFanaType.チャンネル属性, caHeader.channel_att4, filePath);

            // Next block
            offSet += (uint)Marshal.SizeOf(typeof(CAHeaderStruct));

            //展開情報
            EXPHeaderStruct expHeader = ByteUtil.BytesToType<EXPHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("     /展開情報/"));
            LogWriter.SetToBuff(filePath, string.Format("      レコード数:({0})", expHeader.record_num));

            // Next block EXPHeaderStruct
            offSet += (uint)Marshal.SizeOf(typeof(EXPHeaderStruct));
            EXPRecordStruct expRecord = new EXPRecordStruct();
            for (int index = 0; index < expHeader.record_num; index++)
            {
                expRecord = ByteUtil.BytesToType<EXPRecordStruct>(data, (uint)offSet);

                LogWriter.SetToBuff(filePath, string.Format("      展開情報レコード({0}):展開前位置({1}),展開後位置({2})", index + 1, expRecord.mae_posi, expRecord.ato_posi));

                // Next block EXPRecordStruct
                offSet += (uint)Marshal.SizeOf(typeof(EXPRecordStruct));
            }
        }

        /// <summary>
        /// DecodeDataMDT
        /// </summary>
        /// <param name="offSet">offset DecodeDataMDT</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataMDT(ulong offSet, byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("     /MDTデータ/"));
            MDTHeaderStruct mdtHeader = ByteUtil.BytesToType<MDTHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("      Data ID:[{0}]", ByteUtil.ConvertBytesToString(mdtHeader.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("      FormatVersion番号:<{0}H>", ByteUtil.IntToHex2((uint)mdtHeader.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("      Tempo:({0})", mdtHeader.tempo));
            // LogWriter.SetToBuff(filePath, string.Format("      Time Base値:<{0}H>({1})", ByteUtil.IntToHex2((uint)mdtHeader.timebase), (uint)mdtHeader.timebase));
        	if ((uint)mdtHeader.timebase == 0xc2)
        	{
        		LogWriter.SetToBuff(filePath, string.Format("      Time Base48:<{0}H>", ByteUtil.IntToHex2((uint)mdtHeader.timebase)));
        	}
        	else if ((uint)mdtHeader.timebase == 0xd0)
        	{
        		LogWriter.SetToBuff(filePath, string.Format("      Time Base480:<{0}H>", ByteUtil.IntToHex2((uint)mdtHeader.timebase)));
        	}
        	else
        	{
        		LogWriter.SetToBuff(filePath, string.Format("      不明な値:<{0}H>", ByteUtil.IntToHex2((uint)mdtHeader.timebase)));
        	}

            Constant.EXT_TB = mdtHeader.timebase;
            if (!Constant.EXT_PLType.Equals("G"))
            {
                LogWriter.SetToBuff(filePath, string.Format("      SND3先行読み出しFlag:"), false);

                PLCUtil.PLCanaType(EnumCanaType.SND先行読み出しFlag, mdtHeader.sn3_flg, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));

                for (int loop = 0; loop < 20; loop++)
                {
                    LogWriter.SetToBuff(filePath, string.Format("      先行読み出しchorus番号{0}:({1})", (loop + 1), (uint)mdtHeader.sn3_chorus_no[loop]));

                    Thread.Sleep(Constant.Sleep);
                }
            }

            int mid_num = mdtHeader.mid_num[0];
            LogWriter.SetToBuff(filePath, string.Format("      MIDIデータ数:({0})", mid_num));

            uint[] midAddressArray = new uint[20];
            uint midAddressSize = (uint)Marshal.SizeOf(typeof(uint));
            byte[] buff = new byte[midAddressSize];

            // OffSet Mid Address
            uint midOffSet = (uint)offSet + (uint)Marshal.SizeOf(typeof(MDTHeaderStruct));

            // Mid Address
            for (int loop = 0; loop < mid_num; loop++)
            {
                buff = ByteUtil.GetBytes(data, midOffSet, midAddressSize);
                midAddressArray[loop] = (uint)BitConverter.ToInt32(buff, 0);
                LogWriter.SetToBuff(filePath, string.Format("      MIDIデータ({0})アドレス:<{1}H>({2})", (loop + 1), ByteUtil.IntToHex8(midAddressArray[loop]), midAddressArray[loop]));
                midOffSet += midAddressSize;
            }

            // Decode Mid
            for (int loop = 0; loop < mid_num; loop++)
            {
                LogWriter.SetToBuff(filePath, string.Format("      /MIDIデータ({0})/", (loop + 1)));
                // Decode midi
                PLFanaData(EnumPLFanaData.MIDI, midAddressArray[loop] + offSet, 0, data, filePath);
            }
        }

        /// <summary>
        /// DecodeDataMIDI
        /// </summary>
        /// <param name="offSet">offset DecodeDataMIDI</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataMIDI(ulong offSet, byte[] data, string filePath)
        {
            MIDIHeaderStruct midiHeader = ByteUtil.BytesToType<MIDIHeaderStruct>(data, (uint)offSet);
            LogWriter.SetToBuff(filePath, string.Format("       Vocal Track Table:"), false);
            PLFanaType(EnumPLFanaType.VocalTrackTable, BitConverter.GetBytes(midiHeader.vocal_tr), filePath);
            LogWriter.SetToBuff(filePath, string.Format(""));
            LogWriter.SetToBuff(filePath, string.Format("       Rhythm Track Table:"), false);
            PLFanaType(EnumPLFanaType.VocalTrackTable, BitConverter.GetBytes(midiHeader.rhythm_tr), filePath);
            LogWriter.SetToBuff(filePath, string.Format(""));
            LogWriter.SetToBuff(filePath, string.Format("       音源指定:"), false);
            PLFanaType(EnumPLFanaType.音源指定, midiHeader.ongen_type, filePath);

            LogWriter.SetToBuff(filePath, string.Format("       音源バージョン情報:"), false);
            PLFanaType(EnumPLFanaType.バージョン情報, BitConverter.GetBytes(midiHeader.ongen_version), filePath);

            LogWriter.SetToBuff(filePath, string.Format("       音色バージョン情報:"), false);
            PLFanaType(EnumPLFanaType.バージョン情報, BitConverter.GetBytes(midiHeader.ongen_version), filePath);

            LogWriter.SetToBuff(filePath, string.Format("       A Port MIDI Channel番号:"), false);
            PLFanaType(EnumPLFanaType.MIDI_Channel番号, midiHeader.midi_ch_a, filePath);
            LogWriter.SetToBuff(filePath, string.Format(""));

            // track pointer
            for (int index = 0; index < 17; index++)
            {               
                LogWriter.SetToBuff(filePath, string.Format("       Track({0})アドレス:<{1}H>({2})", (index + 1), ByteUtil.IntToHex8(midiHeader.track_point[index]), midiHeader.track_point[index]));
            }

            // Track data
            uint trackData = data[data.Length - 1];

            for (int index = 0; index < 17; index++)
            {
                LogWriter.SetToBuff(filePath, string.Format("       /Track({0})/", (index + 1)));
                Constant.EXT_Total_Tick = 0;

                if (Constant.EXT_TB == Constant.DEF_TB48)
                {
                    PLEUtil.PLEanaDataMdt(EnumEanaDataMDT.MDT_1, midiHeader.track_point[index], data, filePath);
                }
                else
                {
                    PLEUtil.PLEanaDataMdt(EnumEanaDataMDT.MDT_10, midiHeader.track_point[index], data, filePath);
                }
            }

            trackData = 0;
        }

        /// <summary>
        /// DecodeDataTLPHeader
        /// </summary>
        /// <param name="offSet">offset DecodeDataTLPHeader</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void DecodeDataTLPHeader(ulong offSet, byte[] data, string filePath)
        {
            TLPHeaderStruct tlp = ByteUtil.BytesToType<TLPHeaderStruct>(data, (uint)offSet);

            LogWriter.SetToBuff(filePath, string.Format("    /TLPデータ/"));
            LogWriter.SetToBuff(filePath, string.Format("      File ID:[{0}]", ByteUtil.ConvertBytesToString(tlp.data_id)));
            LogWriter.SetToBuff(filePath, string.Format("      FormatVersion番号:<{0}H>", ByteUtil.IntToHex2(tlp.version[0])));
            LogWriter.SetToBuff(filePath, string.Format("      Block文字列 Data部 offset:<{0}H>({1})", ByteUtil.IntToHex4(tlp.moji_offset), tlp.moji_offset));
            LogWriter.SetToBuff(filePath, string.Format("      Block表示情報 Data部 offset:<{0}H>({1})", ByteUtil.IntToHex4(tlp.disp_offset), tlp.disp_offset));
            LogWriter.SetToBuff(filePath, string.Format("      色換速度情報 Data部 offset:<{0}H>({1})", ByteUtil.IntToHex4(tlp.change_offset), tlp.change_offset));
            LogWriter.SetToBuff(filePath, string.Format("      フォントサイズ:({0})", (int)tlp.font_size[0]));

            // Pallet Data
            for (int i = 0; i < tlp.pallet_data.Length; i++)
            {
                LogWriter.SetToBuff(filePath, string.Format("      Pallet Data{0}:", (i + 1)), false);
                byte[] tmp = BitConverter.GetBytes(tlp.pallet_data[i]);
                PLCUtil.PLCanaType(EnumCanaType.PalletData, tmp, filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));
            }

            // Block文字列Data部
            ushort blockSize = (ushort)((int)tlp.disp_offset - (int)tlp.moji_offset);
            PLEUtil.PLEanaData(EnumEanaData.BLOCK_文字_DATA_部, (uint)offSet + tlp.moji_offset, blockSize, data, filePath);

            // Block表示情報データ部
            PLEUtil.PLEanaData(EnumEanaData.BLOCK_表示情報データ部, (uint)offSet + tlp.disp_offset, 0, data, filePath);

            // 色換速度情報データ部
            PLEUtil.PLEanaData(EnumEanaData.色換速度情報データ部, (uint)offSet + tlp.change_offset, 0, data, filePath);
        }

        /// <summary>
        /// PLFanType decode
        /// </summary>
        /// <param name="type">Enum PLFanaType</param>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        public static void PLFanaType(EnumPLFanaType type, byte[] data, string filePath)
        {
            switch (type)
            {
                case EnumPLFanaType.サービス種別:
                    FanaTypeDecodeCodeServiceType(data, filePath);
                    break;
                case EnumPLFanaType.楽器:
                    FanaTypeDecodeCodeMusicalInstrument(data, filePath);
                    break;
                case EnumPLFanaType.チャンネル音声タイプ:
                    FanaTypeDecodeCodeChannelAudioType(data, filePath);
                    break;
                case EnumPLFanaType.チャンネル属性:
                    FanaTypeDecodeCodeChannelAttributes(data, filePath);
                    break;
                case EnumPLFanaType.全体機能フラグ:
                    FanaTypeDecodeCodeOverallFunctionFlag(data, filePath);
                    break;
                case EnumPLFanaType.CodecType:
                    FanaTypeDecodeCodecType(data, filePath);
                    break;
                case EnumPLFanaType.MIDI_Channel番号:
                    FanaTypeDecodeMIDIChannel(data, filePath);
                    break;
                case EnumPLFanaType.バージョン情報:
                    FanaTypeDecodeVersion(data, filePath);
                    break;
                case EnumPLFanaType.音源指定:
                    FanaTypeDecodeSoundSource(data, filePath);
                    break;
                case EnumPLFanaType.VocalTrackTable:
                    FanaTypeDecodeVocalTrackTable(data, filePath);
                    break;
                case EnumPLFanaType.サービスデータタイプ:
                    FanaTypeDecodeServiceDataType(data, filePath);
                    break;
                case EnumPLFanaType.サービスデータファイル:
                    FanaTypeDecodeServiceDataFile(data, filePath);
                    break;
                case EnumPLFanaType.コンテンツ判別フラグ:
                    FanaTypeDecodeContentDiscriminationFlag(data, filePath);
                    break;
                case EnumPLFanaType.登録データフラグ:
                    FanaTypeDecodeRegisterDataFlag(data, filePath);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// FanaTypeDecodeCodeServiceType
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodeServiceType(byte[] data, string filePath)
        {
            uint cType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string ctypeString = string.Empty;
            switch (cType)
            {
                case 0x01:
                    ctypeString = "[コード]";
                    break;
                case 0x02:
                    ctypeString = "[スコア]";
                    break;
                case 0x03:
                    ctypeString = "[両方]";
                    break;
                default:
                    ctypeString = "!!!未定義!!!";
                    break;
            }

            LogWriter.SetToBuff(filePath, ctypeString);
        }

        /// <summary>
        /// FanaTypeDecodeCodeMusicalInstrument
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodeMusicalInstrument(byte[] data, string filePath)
        {
            string musical = string.Empty;
            switch (data[0])
            {
                case 0x40:
                    musical = "エレキギター1)}";
                    break;
                case 0x41:
                    musical = "エレキギター2)}";
                    break;
                case 0x42:
                    musical = "エレキギター3)}";
                    break;
                case 0x43:
                    musical = "エレキギター4)}";
                    break;
                case 0x44:
                    musical = "アコギター)}";
                    break;
                case 0x45:
                    musical = "ベース)}";
                    break;
                case 0x50:
                    musical = "サックス)}";
                    break;
                case 0x51:
                    musical = "トランペット)}";
                    break;
                case 0x60:
                    musical = "ドラム)}";
                    break;
                case 0x61:
                    musical = "ドラム以外の打楽器)}";
                    break;
                case 0x70:
                case 0x71:
                case 0x72:
                case 0x73:
                case 0x74:
                case 0x75:
                case 0x76:
                case 0x77:
                case 0x78:
                case 0x79:
                case 0x7a:
                case 0x7b:
                case 0x7c:
                case 0x7d:
                case 0x7e:
                case 0x7f:
                    musical = "楽器音無属性)}";
                    break;
                default:
                    musical = "!!!未定義!!!)}";
                    break;
            }

            LogWriter.SetToBuff(filePath, string.Format("{0}", musical), false);
        }

        /// <summary>
        /// FanaTypeDecodeCodeChannelAudioType
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodeChannelAudioType(byte[] data, string filePath)
        {
            uint cType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string ctypeString = string.Empty;

            if (cType >= 0x90)
            {
                LogWriter.SetToBuff(filePath, "{無属性}");
                return;
            }
            if (cType >= 0x80)
            {
                LogWriter.SetToBuff(filePath, "{ガイドメロディ(" + string.Format("{0}", ByteUtil.IntToHex2(cType & 0x0f)) + ")}");
                return;
            }
            if (cType >= 0x40)
            {
                LogWriter.SetToBuff(filePath, "{楽器(", false);
                PLFanaType(EnumPLFanaType.楽器, BitConverter.GetBytes(cType), filePath);
                LogWriter.SetToBuff(filePath, string.Format(""));
                return;
            }
            if ((cType >= 0x20) && (cType <= 0x2f))
            {
                LogWriter.SetToBuff(filePath, "{ガイドボーカル(" + string.Format("{0}", cType & 0x0f) + ")}");
                return;
            }
            if ((cType >= 0x10) && (cType <= 0x1f))
            {
                LogWriter.SetToBuff(filePath, "{バックコーラス(" + string.Format("{0}", cType & 0x0f) + ")}");
                return;
            }

            LogWriter.SetToBuff(filePath, "{!!!未定義!!!}");
        }

        /// <summary>
        /// FanaTypeDecodeCodeChannelAttributes
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodeChannelAttributes(byte[] data, string filePath)
        {
            if (data[0] == 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("       該当チャンネルなし"));
                return;
            }

            LogWriter.SetToBuff(filePath, string.Format("       チャンネル音声タイプ="), false);

            PLFanaType(EnumPLFanaType.チャンネル音声タイプ, BitConverter.GetBytes(data[0]), filePath);
			
            LogWriter.SetToBuff(filePath, string.Format("       チャンネルL出力Volume値=<{0}H>=({1})", ByteUtil.IntToHex2(data[2]), data[2] & 0x7f));
        	// 制作時備考：旧ツールのバグが残存していたので修正
            // LogWriter.SetToBuff(filePath, string.Format("       チャンネルR出力Volume値=<{0}H>=({1})", ByteUtil.IntToHex2(data[3]), data[2] & 0x7f));
            LogWriter.SetToBuff(filePath, string.Format("       チャンネルR出力Volume値=<{0}H>=({1})", ByteUtil.IntToHex2(data[3]), data[3] & 0x7f));
        }

        /// <summary>
        /// FanaTypeDecodeCodeOverallFunctionFlag
        /// </summary>       
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodeOverallFunctionFlag(byte[] data, string filePath)
        {
            uint cType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string ctypeString = string.Empty;
            if ((cType & 0x02) != 0x00)
            {
                ctypeString += "{キーコン禁止}";
            }
            if ((cType & 0x04) != 0x00)
            {
                ctypeString += "{キーコン禁止}";
            }

            LogWriter.SetToBuff(filePath, string.Format("{0}", ctypeString), false);
            LogWriter.SetToBuff(filePath, string.Format(""));
        }

        /// <summary>
        /// FanaTypeDecodeCodecType
        /// </summary>       
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeCodecType(byte[] data, string filePath)
        {
            uint cType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string ctypeString = string.Empty;

            switch (cType)
            {
                case 0x10:
                    ctypeString = "MP2";
                    break;
                case 0x30:
                    ctypeString = "LC-AAC";
                    break;
                case 0x40:
                    ctypeString = "HE-AAC";
                    break;
                default:
                    ctypeString = "????不明????";
                    break;
            }

            LogWriter.SetToBuff(filePath, string.Format("{0}", ctypeString));
        }

        /// <summary>
        /// FanaTypeDecodeContentDiscriminationFlag
        /// </summary>      
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeContentDiscriminationFlag(byte[] data, string filePath)
        {
            uint contentFlag = (uint)BitConverter.ToInt16(data, 0);

            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex4(contentFlag)), false);
            if ((contentFlag & 0x01) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[ギタナビ]"), false);
            }
            if ((contentFlag & 0x02) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[生演奏]"), false);
            }
            if ((contentFlag & 0x04) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[ガイドボーカル]"), false);
            }
        }

        /// <summary>
        /// FanaTypeDecodeRegisterDataFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeRegisterDataFlag(byte[] data, string filePath)
        {
            uint dataFlag = (uint)BitConverter.ToInt32(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex8(dataFlag)), false);
            if ((dataFlag & 0x00000001) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[NB用]"), false);
            }
            if ((dataFlag & 0x00000002) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[SB用]"), false);
            }
            if ((dataFlag & 0x00000004) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[BB用]"), false);
            }
            if ((dataFlag & 0x00000008) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[HiBB用]"), false);
            }
        }

        /// <summary>
        /// FanaTypeDecodeRegisterDataFlag
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeServiceDataFile(byte[] data, string filePath)
        {
            uint serviceData = (uint)BitConverter.ToInt32(data, 0);
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex8(serviceData)), false);

            if ((serviceData & 0x00000001) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[CHF]"), false);
            }
            if ((serviceData & 0x00000002) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[TRF]"), false);
            }
            if ((serviceData & 0x00000004) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[VS3]"), false);
            }
            if ((serviceData & 0x00000008) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[VO3]"), false);
            }
            if ((serviceData & 0x00000010) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[VSN]"), false);
            }
            if ((serviceData & 0x00000020) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[VON]"), false);
            }
            if ((serviceData & 0x00000040) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[GTN]"), false);
            }
            if ((serviceData & 0x00000080) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[GTC]"), false);
            }
            if ((serviceData & 0x00000100) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[SSY]"), false);
            }
            if ((serviceData & 0x00010000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[MDN]"), false);
            }
            if ((serviceData & 0x00020000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[MDB]"), false);
            }
            if ((serviceData & 0x00040000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[MDC]"), false);
            }
            if ((serviceData & 0x01000000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[SDN]"), false);
            }
            if ((serviceData & 0x02000000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[SDB]"), false);
            }
            if ((serviceData & 0x04000000) != 0)
            {
                LogWriter.SetToBuff(filePath, string.Format("[SDC]"), false);
            }
        }

        /// <summary>
        /// FanaTypeDecodeMIDIChannel
        /// </summary>
        /// <param name="data">data all</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeMIDIChannel(byte[] data, string filePath)
        {
            for (int index = 0; index < 16; index++)
            {
                LogWriter.SetToBuff(filePath, string.Format("<{0}H>", ByteUtil.IntToHex2(data[index])), false);
            }
        }

        /// <summary>
        /// FanaTypeDecodeVersion
        /// </summary>
        /// <param name="data">3 byte data Decode Version</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeVersion(byte[] data, string filePath)
        {
            LogWriter.SetToBuff(filePath, string.Format("<{0}>=({1})({2})({3})", ByteUtil.IntToHex8((uint)data[0]), (uint)data[0], (uint)data[1], ByteUtil.ByteToChar(data[2])));
        }

        /// <summary>
        /// FanaTypeDecodeSoundSource
        /// </summary>
        /// <param name="data">1 byte data Sound Source</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeSoundSource(byte[] data, string filePath)
        {
            ushort cType = data[0];

            // Vocal Track Table / Rhythm Track Table
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(cType)), false);
            string cTypeString = string.Empty;
            switch (cType)
            {
                case 0x00:
                    cTypeString = "SC-8820";
                    break;
                case 0x01:
                    cTypeString = "SC-8820/SuperNatural";
                    break;
                case 0x02:
                    cTypeString = "Phoenix 1";
                    break;
                case 0x03:
                    cTypeString = "Phoenix 2";
                    break;
                case 0xff:
                    cTypeString = "送信ポート無し";
                    break;
                default:
                    cTypeString = "????不明????";
                    break;
            }

            LogWriter.SetToBuff(filePath, cTypeString);
        }

        /// <summary>
        /// FanaTypeDecodeVocalTrackTable
        /// </summary>
        /// <param name="data">2 bytes data  VocalTrack Table</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeVocalTrackTable(byte[] data, string filePath)
        {
            ushort vTable = (ushort)BitConverter.ToInt16(data, 0);
            ushort mask = 0x8000;
            // Vocal Track Table / Rhythm Track Table
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex4(vTable)), false);
            LogWriter.SetToBuff(filePath, "{", false);

            for (int index = 0; index < 16; index++)
            {
                if ((vTable & mask) == mask)
                {
                    LogWriter.SetToBuff(filePath, string.Format("1"), false);
                }
                else
                {
                    LogWriter.SetToBuff(filePath, string.Format("0"), false);
                }

                mask = (ushort)(mask >> 1);
            }
            LogWriter.SetToBuff(filePath, "}", false);
        }

        /// <summary>
        /// FanaTypeDecodeServiceDataType
        /// </summary>
        /// <param name="data">1 byte data  Service Data Type</param>
        /// <param name="filePath">file output path</param>
        private static void FanaTypeDecodeServiceDataType(byte[] data, string filePath)
        {
            uint serviceType = data[0];
            LogWriter.SetToBuff(filePath, string.Format("<{0}H>=", ByteUtil.IntToHex2(serviceType)), false);

            switch (serviceType)
            {
                case 0x001:
                    LogWriter.SetToBuff(filePath, string.Format("[NB用]"));
                    break;
                case 0x002:
                    LogWriter.SetToBuff(filePath, string.Format("[SB用]"));
                    break;
                case 0x003:
                    LogWriter.SetToBuff(filePath, string.Format("[BB用]"));
                    break;
                case 0x004:
                    LogWriter.SetToBuff(filePath, string.Format("[HiBB用]"));
                    break;
                default:
                    LogWriter.SetToBuff(filePath, string.Format("!!!未定義!!!"));
                    break;
            }
        }

        /// <summary>
        /// PLFanaTAG
        /// </summary>
        /// <param name="data">data  PLFanaTAG</param>
        /// <param name="filePath">file output path</param>
        public static void PLFanaTAG(byte[] data, int num, string filePath)
        {
            int index = 0;
            // Convert byte array to char array
            // ex:<are=0><X=1>
            char[] moji = Encoding.ASCII.GetChars(data);
            // Reading tag
            while (index < num)
            {
                char val = moji[index + 1];
                // Reading tag <br>
                if (val.Equals('b'))
                {
                    if (index != 0)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        "), false);
                    }
                    LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 4))), false);
                    LogWriter.SetToBuff(filePath, string.Format(""));
                    index += 4;
                    continue;
                }
               
                // Reading Tag <are>
                if (val.Equals('a'))
                {
                    if (index != 0)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        "), false);
                    }

                    LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 8))), false);
                    LogWriter.SetToBuff(filePath, string.Format(""));
                    index += 8;
                    continue;
                }
                //Reading tag <X>
                if (val.Equals('X'))
                {
                    if (index != 0)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        "), false);
                    }
                    LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 5))), false);
                    LogWriter.SetToBuff(filePath, string.Format(""));
                    index += 5;
                    continue;
                }
               
                // Reading tag <Size>
                if (val.Equals('s'))
                {
                    if (index != 0)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        "), false);
                    }
                    LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 8))), false);
                    LogWriter.SetToBuff(filePath, string.Format(""));
                    index += 8;
                    continue;
                }
                // Reading tag <char>
                if (val.Equals('c'))
                {
                    if (index != 0)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("        "), false);
                    }
                    LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 8))), false);
                    index += 8;

                    while (true)
                    {
                        LogWriter.SetToBuff(filePath, string.Format("({0}{1}H)", ByteUtil.BytesToHex2(data[index]), ByteUtil.BytesToHex2(data[index + 1])), false);
                        index += 2;
                        if (moji[index] == '>')
                        {
                            LogWriter.SetToBuff(filePath, string.Format("{0}", ByteUtil.ConvertBytesToString(ByteUtil.GetBytes(data, (uint)(index), 1))), false);
                            LogWriter.SetToBuff(filePath, string.Format(""));
                            index += 1;
                            break;
                        }
                    }

                    continue;
                }
                LogWriter.SetToBuff(filePath, string.Format("不明タグ"));
                break;
            }
        }

        /// <summary>
        /// PLFDecode
        /// </summary>        
        /// <param name="data">data  decode</param>
        /// <param name="offSet">offset data</param>
        /// <param name="size">size data decode</param>
        /// <param name="srcSize">size source</param>
        /// <param name="filePath">file output path</param>
        public static int PLFDecode(byte[] data, ulong offSet, ulong size, ulong srcSize, string filePath)
        {
            byte[] buff = new byte[2048];
            // Get Start data to reading
            data = ByteUtil.GetBytes(data, (uint)offSet);

            int write_count = (int)size;
            int read_count = 0;

            // Delete file source if exist
            if (File.Exists(Constant.LTP_SOURCE_FILE_PATH))
            {
                File.Delete(Constant.LTP_SOURCE_FILE_PATH);
            }
            uint offSetBuff = 0;
            while (write_count > 0)
            {
                if (write_count >= buff.Length)
                {
                    read_count = buff.Length;
                }
                else
                {
                    read_count = write_count;
                }

                buff = ByteUtil.GetBytes(data, offSetBuff, (uint)read_count);
                //Write to file
                LogWriter.Write(buff, Constant.LTP_SOURCE_FILE_PATH);

                // Next block   
                offSetBuff += (uint)read_count;

                write_count -= read_count;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            int rsult = Utilities.Decompress(Constant.LTP_SOURCE_FILE_PATH, filePath);

            // Delete source file
            if (File.Exists(Constant.LTP_SOURCE_FILE_PATH))
            {
                File.Delete(Constant.LTP_SOURCE_FILE_PATH);
            }

            if (rsult != 0)
            {
                return rsult;
            }

            // 解凍後のサイズ確認
            ulong outFileSize = (ulong)File.ReadAllBytes(filePath).Length;
            if (outFileSize == srcSize)
            {
                return rsult;
            }
            //Removed filt output if output data wrong 
            else
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return 1;
            }
        }
    }

}
