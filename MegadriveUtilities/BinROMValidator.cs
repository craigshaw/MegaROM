using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    public class BinROMValidator : IROMValidator
    {
        public bool IsValidROM(BigEndianBinaryAccessor accessor)
        {
            if (!(accessor.CompareBytesAt(0x100, Encoding.ASCII.GetBytes("SEGA GENESIS"))
                || accessor.CompareBytesAt(0x100, Encoding.ASCII.GetBytes("SEGA MEGADRIVE"))))
                return false;

            return true;
        }
    }
}
