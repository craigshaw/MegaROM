using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    /// <summary>
    /// Loads ROMs from file system (synchronously). Currently only supports raw .bin formats
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

        public byte[] LoadROM()
        {
            return File.ReadAllBytes(romPath);
        }

        public void SaveROM(byte[] romData)
        {
            File.WriteAllBytes(romPath, romData);
        }
    }
}
