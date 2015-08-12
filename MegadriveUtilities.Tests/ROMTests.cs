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
        public async Task LoadAsyncLoadsWhenGivenALoader()
        {
            var loader = A.Fake<IROMLoader>();

            ROM rom = new ROM(loader);

            await rom.LoadAsync();

            A.CallTo(() => loader.LoadROMAsync()).MustHaveHappened();
        }
    }
}
