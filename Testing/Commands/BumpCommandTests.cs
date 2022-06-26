using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class BumpCommandTests
    {
        private readonly IEnumerable<Game> games;
        private readonly Game firstGame;
        private readonly Game secondGame;
        private readonly Game thirdGame;

        public BumpCommandTests()
        {
            firstGame = new Game("Test 1", 1);
            secondGame = new Game("Test 2", 2);
            thirdGame = new Game("Test 3", 3);
            games = new List<Game>()
            {
                firstGame, secondGame, thirdGame
            };
        }

        [TestMethod]
        public void SecondGameBumpedUpMovesToFirstPosition()
        {
            var result = new BumpCommand(games, secondGame, true).Execute();
            Assert.AreEqual(secondGame, result.ToList()[0]);
        }

        [TestMethod]
        public void FirstGameBumpedUpDoesNotMove()
        {
            var result = new BumpCommand(games, firstGame, true).Execute();
            Assert.AreEqual(firstGame, result.ToList()[0]);
        }

        [TestMethod]
        public void VerifySecondGamePriorityChangesWhenBumpedUp()
        {
            var result = new BumpCommand(games, secondGame, true).Execute();
            Assert.AreEqual(1, result.ToList()[0].Priority);
        }

        [TestMethod]
        public void VerifyFirsGamePriorityChangesWhenSecondGameBumpedUp()
        {
            var result = new BumpCommand(games, secondGame, true).Execute();
            Assert.AreEqual(2, result.ToList()[1].Priority);
        }

        [TestMethod]
        public void SecondGameBumpedDownMovesToThirdPosition()
        {
            var result = new BumpCommand(games, secondGame, false).Execute();
            Assert.AreEqual(secondGame, result.Last());
        }

        [TestMethod]
        public void ThirdGameBumpedDownDoesNotMove()
        {
            var result = new BumpCommand(games, thirdGame, false).Execute();
            Assert.AreEqual(thirdGame, result.Last());
        }
    }
}
