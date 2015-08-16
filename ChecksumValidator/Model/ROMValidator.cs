using MegadriveUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumValidator.Model
{
    internal class ROMValidator
    {
        private string romPath;

        public ROMValidator(string romPath)
        {
            if (string.IsNullOrEmpty(romPath))
                throw new ArgumentNullException("romPath");

            this.romPath = romPath;
        }

        public async Task<string> ValidateROM()
        {
            string result = "Invalid";
            try
            {
                ROM rom = new ROM(new BinROMLoader(romPath, new BinROMValidator()));
                await rom.LoadAsync();

                UInt16 checksum = rom.Checksum;
                UInt16 calculatedChecksum = rom.CalculateChecksum();

                if (checksum == calculatedChecksum)
                    result = "Valid";
            }
            catch (Exception ex)
            {
                result = string.Format("Failed with: {0}", ex.Message);
            }

            return result;
        }
    }
}
