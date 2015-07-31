using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MegadriveUtilities
{
    internal delegate T Conversion<T>(byte[] data, int offset);

    /// <summary>
    /// Utility class representing a Megadrive ROM
    /// Based on documentation found at http://www.zophar.net/documents/genesis/genesis-rom-format.html
    /// </summary>
    public class ROM
    {
        private byte[] rom;
        private ushort checksum;
        private uint startAddress = 0xFFFFFFFF;
        private uint endAddress;
        private IROMLoader loader;

        public ROM(IROMLoader loader)
        {
            if (loader == null)
                throw new ArgumentNullException("loader");

            this.loader = loader;
        }

        public async Task LoadAsync()
        {
            rom = await loader.LoadROMAsync();

            ValidateROM();
        }

        public async Task SaveAsync()
        {
            await loader.SaveROMAsync(this.rom, true);
        }

        public T GetValue<T>(uint offset) where T : struct
        {
            return GetValueImpl<T>(offset);
        }

        public UInt16 Checksum
        {
            get
            {
                if (checksum == 0)
                    checksum = GetUInt16(0x18E);

                return checksum;
            }
        }

        public UInt32 StartAddress
        {
            get
            {
                if (startAddress == 0xFFFFFFFF)
                    startAddress = GetUInt(0x01A0);
                
                return startAddress;
            }
        }

        public UInt32 EndAddress
        {
            get
            {
                if (endAddress == 0x00000000)
                    endAddress = GetUInt(0x01A4);

                return endAddress;
            }
        }

        private void ValidateROM()
        {
            if (!(CompareBytesAt(0x100, Encoding.ASCII.GetBytes("SEGA GENESIS"))
                || CompareBytesAt(0x100, Encoding.ASCII.GetBytes("SEGA MEGADRIVE"))))
                throw new ApplicationException("Invalid ROM format");
        }

        private unsafe bool CompareBytesAt(uint offset, byte[] compareTo)
        {
            fixed (byte* arrayPtr = &rom[offset])
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

        private UInt32 GetUInt32(uint offset)
        {
            if (offset >= rom.Length)
                throw new ArgumentException("offset >= rom length", "offset");

            byte[] bytes = new byte[4];
            Array.Copy(rom, offset, bytes, 0, 4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private UInt16 GetUInt16(uint offset)
        {
            return GetValueFromROM<UInt16>(offset, (data, index) => BitConverter.ToUInt16(data, index));
        }

        private UInt32 GetUInt(uint offset)
        {
            return GetValueFromROM<UInt32>(offset, (data, index) => BitConverter.ToUInt32(data, index));
        }

        private T GetValueFromROM<T>(uint offset, Conversion<T> conversion)
        {
            if (offset >= rom.Length)
                throw new ArgumentException("offset >= rom length", "offset");

            int typeSize = Marshal.SizeOf(typeof(T));
            byte[] bytes = new byte[typeSize];
            Array.Copy(rom, offset, bytes, 0, typeSize);
            Array.Reverse(bytes);
            return conversion(bytes, 0);
        }

        private T GetValueImpl<T>(uint offset) where T : struct
        {
            int typeSize = Marshal.SizeOf(typeof(T));
            UInt64 temp = 0;

            for (int i = typeSize - 1, j = 0; i >= 0; i--, j++)
            {
                temp |= (UInt64)rom[offset + i] << (j << 3);
            }

            return (T)Convert.ChangeType(temp, typeof(T));
        }

        // Would love to get this generic get value working. Not sure it's going to be possible with the restrictions
        // of bitwise operators on generic types
        //private T GetValue<T>(uint offset) where T : struct
        //{
        //    int typeSize = Marshal.SizeOf(typeof(T));
        //    T value = default(T);

        //    for (int i = typeSize - 1; i >= 0; i--)
        //    {
        //        value |= rom[offset + i] << (8 * i);
        //    }

        //    return value;
        //}

        public UInt16 CalculateChecksum()
        {
            UInt16 workingChecksum = 0;

            for (int index = 0x200; index < rom.Length; index += 2)
            {
                workingChecksum += (UInt16)(rom[index] * 0x100);
                workingChecksum += rom[index + 1];
            }

            return (UInt16)workingChecksum;
        }

        public string OutputAsByteArray()
        {
            int count = 0;
            StringBuilder output = new StringBuilder();

            output.Append("byte[] romData = {" + Environment.NewLine);

            foreach(byte b in rom)
            {
                output.AppendFormat("0x{0:X},{1}", b, (count++ % 20 == 0) ? Environment.NewLine : " ");
            }

            output.Append(Environment.NewLine + "};");

            return output.ToString();
        }

        public void SetValue<T>(uint offset, T value) where T : struct
        {
            int typeSize = Marshal.SizeOf(typeof(T));
            byte[] bytes = new byte[typeSize];
            UInt64 temp = (UInt64)Convert.ChangeType(value, typeof(UInt64));

            for (int i = 0; i < typeSize; i++)
            {
                bytes[i] = (byte)(temp >> ((typeSize - 1 - i) * 8));
            }

            Array.Copy(bytes, 0, rom, offset, bytes.Length);
        }
    }
}
