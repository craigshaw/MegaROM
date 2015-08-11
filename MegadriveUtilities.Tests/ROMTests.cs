using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace MegadriveUtilities.Tests
{
    [TestClass]
    public class ROMTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsWhenGivenANullLoader()
        {
            ROM rom = new ROM(null);
        }

        [TestMethod]
        public async Task LoadAsync()
        {
            var loader = A.Fake<IROMLoader>();

            A.CallTo(() => loader.LoadROMAsync()).Returns(new BigEndianBinaryAccessor(new byte[] { 0x01, 0x02, 0x03 }));

            ROM rom = new ROM(loader);

            await rom.LoadAsync();

            A.CallTo(() => loader.LoadROMAsync()).MustHaveHappened();
        }
    }
}
