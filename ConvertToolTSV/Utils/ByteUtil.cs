using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PLTextToolTSV.Utils
{
    public class ByteUtil
    {
       public static DateTime CTimeToDate(Int64 CTime)
        {
            TimeSpan span = TimeSpan.FromTicks(CTime * TimeSpan.TicksPerSecond);
            DateTime t = new DateTime(1970, 1, 1).Add(span);
            return TimeZone.CurrentTimeZone.ToLocalTime(t);
        }

        public static DateTime BytesToDateTime(byte[] data)
        {
            int longVar = BitConverter.ToInt32(data, 0);
            DateTime baseDate = CTimeToDate(longVar);
            return baseDate;
        }

        public static string ConvertBytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(RemoveByteNull(data));
        }

        public static string ConvertBytesToDefault(byte[] data)
        {
            return Encoding.Default.GetString(RemoveByteNull(data));
        }

        public static string BytesToHex2(byte data)
        {
            string hex = Convert.ToByte(data).ToString("x2");
            return hex;
        }

        public static string IntToHex2(uint value, int padLef = 0, char chars = ' ')
        {
            string hex = value.ToString("x2");
            if (padLef > 0)
            {
                hex = hex.PadLeft(padLef, chars);
            }
            return hex;
        }

        public static string IntToHex4(uint value)
        {
            string hex = value.ToString("x4");
            return hex;
        }


        public static string IntToHex8(uint value)
        {
            string hex = value.ToString("x8");
            return hex;
        }

        public static byte[] RemoveByteNull(byte[] data)
        {
            byte[] new_data = data.TakeWhile((v, index) => data.Skip(index).Any(w => w != 0x00)).ToArray();
            return new_data;
        }

        public static T BytesToType<T>(byte[] data, uint offset = 0)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buff = new byte[size];
            Array.Copy(data, offset, buff, 0, size);

            GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return theStructure;
        }

        /// <summary>
        /// Get bytes from source
        /// </summary>
        /// <typeparam name="T">Struct Removed</typeparam>
        /// <param name="data">Bytes array source</param>
        /// <param name="count">Bytes removed</param>
        /// <returns></returns>
        public static byte[] RemovedBytes<T>(byte[] data, int count = 1)
        {
            int size = Marshal.SizeOf(typeof(T)) * count;

            byte[] buff = new byte[data.Length - size];
            Array.Copy(data, size, buff, 0, buff.Length);

            return buff;
        }

        public static byte[] GetBytes(byte[] data, uint offSet, uint size)
        {
            byte[] buff = new byte[size];
            Array.Copy(data, offSet, buff, 0, size);
            return buff;
        }

        public static byte[] GetBytesToNull(byte[] data, int size, uint offSet)
        {
            byte[] buff = new byte[size];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[offSet + i] == 0x00)
                    break;
                buff[i] = data[offSet + i];
            }
            return RemoveByteNull(buff);
        }

        public static byte[] GetBytes(byte[] data, uint offSet)
        {
            uint size = (uint)data.Length - offSet;
            byte[] buff = new byte[size];
            Array.Copy(data, offSet, buff, 0, size);
            return buff;
        }

        public static string ByteToChar(byte data)
        {
            string rsult = "0";
            if (data != 0)
                rsult = Convert.ToChar(data).ToString();
            return rsult;
        }
    }
}
