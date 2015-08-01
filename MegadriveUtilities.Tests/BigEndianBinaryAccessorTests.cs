using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace MegadriveUtilities.Tests
{
    [TestClass]
    public class BigEndianBinaryAccessorTests
    {
        [TestMethod]
        public void ConstructsWithByteArray()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes("Testing construction"));
            Assert.IsNotNull(accessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsWhenConstructedWithNull()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(null);
        }

        [TestMethod]
        public void GetsUInt16()
        {
            byte[] testData = new byte[] { 0xea, 0x60 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            UInt16 result = accessor.GetUInt16(0);
            Assert.AreEqual(60000, result);
        }

        [TestMethod]
        public void GetsInt16()
        {
            byte[] testData = new byte[] { 0xD8, 0xF0 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            Int16 result = accessor.GetInt16(0);
            Assert.AreEqual(-10000, result);
        }

        [TestMethod]
        public void GetsUInt32()
        {
            UInt32 expectedResult = 2147483647;
            byte[] testData = new byte[] { 0x7F, 0xFF, 0xFF, 0xFF };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            UInt32 result = accessor.GetUInt32(0);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetsInt32()
        {
            Int32 expectedResult = -2000000;
            byte[] testData = new byte[] { 0xFF, 0xE1, 0x7B, 0x80 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            Int32 result = accessor.GetInt32(0);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetXIntXXThrowsIfOffsetIsInvalid()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(new byte[] { 0xFF, 0xFE, 0xFD });
            Int16 invalid = accessor.GetInt16(UInt16.MaxValue);
        }

        [TestMethod]
        public void CompareBytesAtReturnsTrueWhenMatchedBytes()
        {
            string testData = "Some test data for this unit test";
            string match = "unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsTrue(accessor.CompareBytesAt(0x18, Encoding.ASCII.GetBytes(match)));
        }

        [TestMethod]
        public void CompareBytesAtReturnsFasleWhenNotMatching()
        {
            string testData = "Some test data for this unit test";
            string match = "unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsFalse(accessor.CompareBytesAt(0x10, Encoding.ASCII.GetBytes(match)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareBytesThrowsArgumentExceptionIfInvalidOffsetGiven()
        {
            string testData = "Some test data for this unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsFalse(accessor.CompareBytesAt(0xFFFF, Encoding.ASCII.GetBytes("test")));
        }
    }
}
