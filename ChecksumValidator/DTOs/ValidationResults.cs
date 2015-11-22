using System;

namespace ChecksumValidator.DTOs
{
    internal class ValidationResults
    {
        public string ROMFilePath { get; set; }
        public UInt16 ActualChecksum { get; set; }
        public UInt16 CalculatedChecksum { get; set; }
        public bool HasChanged { get; set; }
    }
}
