using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class RemoveGameCommandTests
    {
        [TestMethod]
        public void VerifyGameIsRemoved()
        {
            var game = new Game("Test game", 0, 0, false, 0);
            var list = new List<Game>() { game };
            var underTest = new RemoveGameCommand(new(list, game));
            var result = underTest.Execute();
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void TestGameThatIsNotInList()
        {
            var game = new Game("Test game", 0, 0, false, 0);
            var list = new List<Game>() { new Game() };
            var underTest = new RemoveGameCommand(new(list, game));
            var result = underTest.Execute();
            Assert.AreEqual(1, result.Count());
        }
    }
}
