using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class AddGameCommandTests
    {
        [TestMethod]
        public void VerifyGameIsAddedToList()
        {
            var list = new List<Game>();
            var underTest = new AddGameCommand(new(list, new Game("Test game")));
            var result = underTest.Execute();
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void VerifyNullGameIsNotAdded()
        {
            var list = new List<Game>();
            var underTest = new AddGameCommand(new(list, null!));

            var result = underTest.Execute();
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void VerifyGameWithNoNameIsNotAdded()
        {
            var list = new List<Game>();
            var underTest = new AddGameCommand(new(list, new()));

            var result = underTest.Execute();
            Assert.IsTrue(!result.Any());
        }
    }
}
