using MegadriveUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 1)
                return;

            new Program().Run(args[0]);

            Console.ReadKey();
        }

        private async void Run(string romPath)
        {
            try
            {
                // Load ROM content
                ROM rom = new ROM(new BinROMLoader(romPath, new BinROMValidator()));
                await rom.LoadAsync();

                Console.WriteLine("System is {0} endian", BitConverter.IsLittleEndian ? "little" : "big");
                Console.WriteLine("Loaded ROM {0}", romPath);

                // Get current checksum
                Console.WriteLine("Checksum: 0x{0:X}", rom.Checksum);

                // Calculated checksum
                Console.WriteLine("Calculated Checksum: 0x{0:X}", rom.CalculateChecksum());

                // Get, then update the master code
                UInt16 masterCode = rom.GetUInt16(0xFF888);
                Console.WriteLine("Current Master Code: 0x{0:X}", masterCode);

                UInt16 check = rom.GetUInt16(0x18E);
                Console.WriteLine("Checksum (from generic method): 0x{0:X}", check);

                //await rom.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }
    }

}
