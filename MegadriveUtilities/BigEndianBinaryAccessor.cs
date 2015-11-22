using System;
using System.Runtime.InteropServices;

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

        public UInt16 GetUInt16(uint offset)
        {
            return GetValue<UInt16>(offset, (data, index) => BitConverter.ToUInt16(data, index));
        }

        public Int16 GetInt16(uint offset)
        {
            return GetValue<Int16>(offset, (data, index) => BitConverter.ToInt16(data, index));
        }

        public UInt32 GetUInt32(uint offset)
        {
            return GetValue<UInt32>(offset, (data, index) => BitConverter.ToUInt32(data, index));
        }

        public Int32 GetInt32(uint offset)
        {
            return GetValue<Int32>(offset, (data, index) => BitConverter.ToInt32(data, index));
        }

        public unsafe void SetValue<T>(T value, uint offset) where T : struct
        {
            if (offset >= binData.Length)
                throw new ArgumentException("offset >= data length", "offset");

            int typeSize = Marshal.SizeOf(typeof(T));
            UInt64 temp = (UInt64)Convert.ChangeType(value, typeof(UInt64));

            fixed (byte* bufferPtr = &binData[offset])
            {
                byte* currentByte = bufferPtr;
                for (int i = 0; i < typeSize; i++, currentByte++)
                    *currentByte = (byte)(temp >> ((typeSize - 1 - i) << 3));
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

        private T GetValue<T>(uint offset, Conversion<T> conversion)
        {
            if (offset >= binData.Length)
                throw new ArgumentException("offset >= data length", "offset");

            int typeSize = Marshal.SizeOf(typeof(T));
            byte[] bytes = new byte[typeSize];
            Array.Copy(binData, offset, bytes, 0, typeSize);
            Array.Reverse(bytes);
            return conversion(bytes, 0);
        }
    }
}
