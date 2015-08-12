using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    public interface IROMValidator
    {
        bool IsValidROM(BigEndianBinaryAccessor accessor);
    }
}
