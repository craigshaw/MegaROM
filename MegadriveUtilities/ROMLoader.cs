using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    /// <summary>
    /// Loads ROMs from file system. Currently only supports raw .bin formats
    /// </summary>
    public class ROMLoader : IROMLoader
    {
        private string romPath;

        public ROMLoader(string romPath)
        {
            if (!File.Exists(romPath))
                throw new ArgumentException("ROM file must exist", "romPath");

            this.romPath = romPath;
        }

        public async Task<byte[]> LoadROMAsync()
        {
            using(FileStream fs = new FileStream(romPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                byte[] rom = new byte[fs.Length];
                await fs.ReadAsync(rom, 0, rom.Length);

                // Not sure I like this here.
                ValidateROM(rom);

                return rom;
            }
        }

        private void ValidateROM(byte[] rom)
        {
            if (!(Compare(rom, 0x100, Encoding.ASCII.GetBytes("SEGA GENESIS"))
                ||Compare(rom, 0x100, Encoding.ASCII.GetBytes("SEGA MEGADRIVE"))))
                throw new ApplicationException("Invalid ROM format");
        }

        private unsafe bool Compare(byte[] source, uint sourceOffset, byte[] compareTo)
        {
            fixed (byte* arrayPtr = &source[sourceOffset])
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

        public async Task SaveROMAsync(byte[] romData, bool backupOriginal)
        {
            if(backupOriginal == true)
                await BackupROM();

            // Now save ROM data to original file
            using (FileStream fs = new FileStream(romPath, FileMode.Truncate, FileAccess.Write, FileShare.None, 4096, true))
            {
                await fs.WriteAsync(romData, 0, romData.Length);
            }
        }

        private async Task BackupROM()
        {
            // Copy the original file here with a .bak extension
            using (FileStream original = new FileStream(romPath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, true))
            {
                using (FileStream backup = new FileStream(romPath + ".bak", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                {
                    await original.CopyToAsync(backup);
                }
            }
        }
    }
}
