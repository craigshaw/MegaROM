using System;
using MegadriveUtilities;
using ChecksumValidator.DTOs;
using System.Threading.Tasks;

namespace ChecksumValidator.Transaction_Scripts
{
    internal class Validator
    {
        public async Task<ValidationResults> ValidateChecksum(string filePath)
        {
            ValidationResults results = new ValidationResults() { ROMFilePath = filePath };

            // Load ROM content
            ROM rom = new ROM(new BinROMLoader(filePath, new BinROMValidator()));
            await rom.LoadAsync();

            // Get the checksum from the ROM. Also run a checksum calculation and compare the results. If not the same,
            // update the ROM and save it
            results.ActualChecksum = rom.Checksum;
            results.CalculatedChecksum = rom.CalculateChecksum();
            results.HasChanged = results.ActualChecksum != results.CalculatedChecksum;

            if (results.ActualChecksum != results.CalculatedChecksum)
            {
                rom.Checksum = results.CalculatedChecksum;
                await rom.SaveAsync();
            }

            return results;
        }
    }
}
