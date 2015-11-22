using MegadriveUtilities;
using System;
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

        public async Task<ROMValidationResult> ValidateROM()
        {
            string result = "OK";
            string resultText = "Invalid";
            try
            {
                ROM rom = new ROM(new BinROMLoader(romPath, new BinROMValidator()));
                await rom.LoadAsync();

                UInt16 checksum = rom.Checksum;
                UInt16 calculatedChecksum = rom.CalculateChecksum();

                if (checksum == calculatedChecksum)
                    resultText = "Valid";
                else
                {
                    // Update ROM here
                }
            }
            catch (Exception ex)
            {
                result = "Failed";
                resultText = string.Format("Failed with: {0}", ex.Message);
            }

            return new ROMValidationResult() { Result = result, ResultText = resultText };
        }
    }
}
