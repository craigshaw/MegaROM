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
        Task<byte[]> LoadROMAsync();
        Task SaveROMAsync(byte[] romData, bool backupOriginal=false);
    }
}
