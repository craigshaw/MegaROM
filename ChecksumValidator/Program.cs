using ChecksumValidator.DTOs;
using ChecksumValidator.Transaction_Scripts;
using System;
using System.IO;

namespace ChecksumValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private void Run(string[] args)
        {
            if (args.Length < 1)
                ShowUsageAndQuit();

            try
            {
                string romPath = args[0];

                if(!File.Exists(romPath))
                {
                    Console.WriteLine("{0} isn't a valid file path", romPath);
                }
                else
                {
                    ValidateChecksum(romPath);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("It looks like something went wrong. Last message is,{0}{1}", Environment.NewLine, ex.Message);
            }

            Console.ReadKey(false);
        }

        private async void ValidateChecksum(string romPath)
        {
            ValidationResults results = await new Validator().ValidateChecksum(romPath);

            DisplayResults(results);
        }

        private void DisplayResults(ValidationResults results)
        {
            Console.WriteLine("ROM file {0}", results.ROMFilePath);

            if (results.HasChanged)
            {
                Console.WriteLine("Checksum updated from 0x{0:X4} to 0x{1:X4}", results.ActualChecksum, results.CalculatedChecksum);
            }
            else
            {
                Console.WriteLine("Checksum is correct (0x{0:X4}), no changes made", results.ActualChecksum);
            }
        }

        private void ShowUsageAndQuit()
        {
            Console.WriteLine("Usage: ChecksumValidator.exe <.md to validate>");
            Console.WriteLine("Example: ChecksumValidator.exe \"c:\\roms\\Road Rash II (USA, Europe).md\"");

            Environment.Exit(1);
        }
    }
}
