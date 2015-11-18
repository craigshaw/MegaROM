using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    public class BigEndianBinaryAccessor
    {
        private byte[] binData;

        public BigEndianBinaryAccessor(byte[] binData)
        {
            if (binData == null)
                throw new ArgumentNullException("binData");

            this.binData = binData;
        }

        public byte[] BinaryData
        {
            get { return binData;  }
        }

        // Can't get this to worked with signed values, int16 and int32
        //public T GetValue<T>(uint offset) where T : struct
        //{
        //    int typeSize = Marshal.SizeOf(typeof(T));
        //    UInt64 temp = 0;

        //    for (int i = typeSize - 1, j = 0; i >= 0; i--, j++)
        //    {
        //        temp |= (UInt64)binData[offset + i] << (j << 3);
        //    }

        //    return (T)Convert.ChangeType(temp, typeof(T));
        //}

        public UInt16 GetUInt16(uint offset)
        {
            return GetValueFromROM<UInt16>(offset, (data, index) => BitConverter.ToUInt16(data, index));
        }

        public Int16 GetInt16(uint offset)
        {
            return GetValueFromROM<Int16>(offset, (data, index) => BitConverter.ToInt16(data, index));
        }

        public UInt32 GetUInt32(uint offset)
        {
            return GetValueFromROM<UInt32>(offset, (data, index) => BitConverter.ToUInt32(data, index));
        }

        public Int32 GetInt32(uint offset)
        {
            return GetValueFromROM<Int32>(offset, (data, index) => BitConverter.ToInt32(data, index));
        }

        public unsafe void SetValue<T>(T value, uint offset) where T : struct
        {
            int typeSize = Marshal.SizeOf(typeof(T));
            UInt64 temp = (UInt64)Convert.ChangeType(value, typeof(UInt64));

            fixed (byte* bufferPtr = &binData[offset])
            {
                byte* currentByte = bufferPtr;
                for (int i = 0; i < typeSize; i++)
                {
                    *currentByte = (byte)(temp >> ((typeSize - 1 - i) * 8));
                    currentByte++;
                }

            }
        }

        public unsafe bool CompareBytesAt(uint offset, byte[] compareTo)
        {
            if (offset >= binData.Length)
                throw new ArgumentException("offset >= data length", "offset");

            fixed (byte* arrayPtr = &binData[offset])
            {
                byte* currentByte = arrayPtr;
                for (int i = 0; i < compareTo.Length; i++)
                {
                    if (*currentByte != compareTo[i])
                        return false;

                    currentByte++;
                }
            }

            return true;
        }

        private T GetValueFromROM<T>(uint offset, Conversion<T> conversion)
        {
            if (offset >= binData.Length)
                throw new ArgumentException("offset >= data length", "offset");

            int typeSize = Marshal.SizeOf(typeof(T));
            byte[] bytes = new byte[typeSize];
            Array.Copy(binData, offset, bytes, 0, typeSize);
            Array.Reverse(bytes);
            return conversion(bytes, 0);
        }

        //public void SetValue<T>(uint offset, T value) where T : struct
        //{
        //    int typeSize = Marshal.SizeOf(typeof(T));
        //    byte[] bytes = new byte[typeSize];
        //    UInt64 temp = (UInt64)Convert.ChangeType(value, typeof(UInt64));

        //    for (int i = 0; i < typeSize; i++)
        //    {
        //        bytes[i] = (byte)(temp >> ((typeSize - 1 - i) * 8));
        //    }

        //    Array.Copy(bytes, 0, rom, offset, bytes.Length);
        //}
    }
}
