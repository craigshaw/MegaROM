using System;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    internal delegate T Conversion<T>(byte[] data, int offset);

    /// <summary>
    /// Utility class representing a Megadrive ROM
    /// Based on documentation found at http://www.zophar.net/documents/genesis/genesis-rom-format.html
    /// </summary>
    public class ROM
    {
        private BigEndianBinaryAccessor rom;
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
        }

        public async Task SaveAsync()
        {
            await loader.SaveROMAsync(rom.BinaryData, true);
        }

        public UInt16 Checksum
        {
            get
            {
                if (checksum == 0)
                    checksum = rom.GetUInt16(0x18E);

                return checksum;
            }
        }

        public UInt32 StartAddress
        {
            get
            {
                if (startAddress == 0xFFFFFFFF)
                    startAddress = rom.GetUInt32(0x01A0);
                
                return startAddress;
            }
        }

        public UInt32 EndAddress
        {
            get
            {
                if (endAddress == 0x00000000)
                    endAddress = rom.GetUInt32(0x01A4);

                return endAddress;
            }
        }

        public UInt16 CalculateChecksum()
        {
            UInt16 workingChecksum = 0;
            byte[] raw = rom.BinaryData;

            for (int index = 0x200; index < raw.Length; index += 2)
            {
                workingChecksum += (UInt16)(raw[index] * 0x100);
                workingChecksum += raw[index + 1];
            }

            return (UInt16)workingChecksum;
        }

        public string OutputAsByteArray()
        {
            int count = 0;
            StringBuilder output = new StringBuilder();

            output.Append("byte[] romData = {" + Environment.NewLine);

            foreach(byte b in rom.BinaryData)
            {
                output.AppendFormat("0x{0:X},{1}", b, (count++ % 20 == 0) ? Environment.NewLine : " ");
            }

            output.Append(Environment.NewLine + "};");

            return output.ToString();
        }

        public UInt16 GetUInt16(uint offset)
        {
            return rom.GetUInt16(offset);
        }

        public void SetValue<T>(T value, uint offset) where T : struct
        {
            rom.SetValue(value, offset);
        }
    }
}
