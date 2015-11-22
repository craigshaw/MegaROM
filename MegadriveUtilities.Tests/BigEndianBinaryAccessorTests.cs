using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Linq;

namespace MegadriveUtilities.Tests
{
    [TestClass]
    public class BigEndianBinaryAccessorTests
    {
        [TestMethod, TestCategory("BinaryAccessor")]
        public void ConstructsWithByteArray()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes("Testing construction"));
            Assert.IsNotNull(accessor);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsWhenConstructedWithNull()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(null);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void GetsUInt16()
        {
            byte[] testData = new byte[] { 0xea, 0x60 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            UInt16 result = accessor.GetUInt16(0);
            Assert.AreEqual(60000, result);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void GetsInt16()
        {
            byte[] testData = new byte[] { 0xD8, 0xF0 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            Int16 result = accessor.GetInt16(0);
            Assert.AreEqual(-10000, result);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void GetsUInt32()
        {
            UInt32 expectedResult = 2147483647;
            byte[] testData = new byte[] { 0x7F, 0xFF, 0xFF, 0xFF };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            UInt32 result = accessor.GetUInt32(0);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void GetsInt32()
        {
            Int32 expectedResult = -2000000;
            byte[] testData = new byte[] { 0xFF, 0xE1, 0x7B, 0x80 };
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);
            Int32 result = accessor.GetInt32(0);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetXIntXXThrowsIfOffsetIsInvalid()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(new byte[] { 0xFF, 0xFE, 0xFD });
            Int16 invalid = accessor.GetInt16(UInt16.MaxValue);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void CompareBytesAtReturnsTrueWhenBytesMatch()
        {
            string testData = "Some test data for this unit test";
            string match = "unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsTrue(accessor.CompareBytesAt(0x18, Encoding.ASCII.GetBytes(match)));
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void CompareBytesAtReturnsFalseWhenBytesDontMatch()
        {
            string testData = "Some test data for this unit test";
            string match = "unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsFalse(accessor.CompareBytesAt(0x10, Encoding.ASCII.GetBytes(match)));
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareBytesThrowsArgumentExceptionIfInvalidOffsetGiven()
        {
            string testData = "Some test data for this unit test";
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(Encoding.ASCII.GetBytes(testData));
            Assert.IsFalse(accessor.CompareBytesAt(0xFFFF, Encoding.ASCII.GetBytes("test")));
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void SetsUint16()
        {
            UInt16 testValue = 0x846A;
            byte[] testData = new byte[2];
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);

            accessor.SetValue(testValue, 0);

            Assert.AreEqual(0x84, testData[0]);
            Assert.AreEqual(0x6A, testData[1]);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void SetsInt16()
        {
            Int16 testValue = 16416; // 0x4020
            byte[] testData = new byte[2];
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);

            accessor.SetValue(testValue, 0);

            Assert.AreEqual(0x40, testData[0]);
            Assert.AreEqual(0x20, testData[1]);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void SetsInt32()
        {
            Int32 testValue = 68157953; // 0x04100201
            byte[] testData = new byte[4];
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);

            accessor.SetValue(testValue, 0);

            Assert.AreEqual(0x04, testData[0]);
            Assert.AreEqual(0x10, testData[1]);
            Assert.AreEqual(0x02, testData[2]);
            Assert.AreEqual(0x01, testData[3]);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void SetsUint32()
        {
            UInt32 testValue = 0xFFDFF63C; //-2099652
            byte[] testData = new byte[4];
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);

            accessor.SetValue(testValue, 0);

            Assert.AreEqual(0xFF, testData[0]);
            Assert.AreEqual(0xDF, testData[1]);
            Assert.AreEqual(0xF6, testData[2]);
            Assert.AreEqual(0x3C, testData[3]);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        public void SetAndGetUsingAccessor()
        {
            Int32 testValue = 268575300; // 0x10022244
            byte[] testData = new byte[10];
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(testData);

            accessor.SetValue(testValue, 4);
            Int32 readValue = accessor.GetInt32(4);

            Assert.AreEqual(testValue, readValue);
        }

        [TestMethod, TestCategory("BinaryAccessor")]
        [ExpectedException(typeof(ArgumentException))]
        public void SetValueThrowsIfOffsetIsInvalid()
        {
            BigEndianBinaryAccessor accessor = new BigEndianBinaryAccessor(new byte[] { 0xFF, 0xFE, 0xFD });
            accessor.SetValue(0xFF, 10);
        }
    }
}
