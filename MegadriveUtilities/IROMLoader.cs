using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegadriveUtilities
{
    /// <summary>
    /// Loads ROM files
    /// </summary>
    public interface IROMLoader
    {
        byte[] LoadROM();
        void SaveROM(byte[] romData);
    }
}
