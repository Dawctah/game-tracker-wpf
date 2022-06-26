using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class CompleteGameCommandTests
    {
        private readonly CompleteGameCommand underTest;

        public CompleteGameCommandTests()
        {
            var toPlay = new List<Game>();

            for (int i = 0; i < 10; i++)
            {
                toPlay.Add(new Game($"Game{i}"));
            }

            toPlay = new AutoSetPriorityCommand(toPlay).Execute().ToList();

            var gameData = new GameData(toPlay, Enumerable.Empty<Game>());
            underTest = new CompleteGameCommand(gameData);
        }

        [TestMethod]
        public void VerifyGameIsAddedToCompletedList()
        {
            var result = underTest.Execute();

            Assert.IsTrue(result.CompletedGames.Any());
        }

        [TestMethod]
        public void VerifyCorrectGameIsAddedToCompletedList()
        {
            var result = underTest.Execute();

            Assert.AreEqual("Game0", result.CompletedGames.First().Name);
        }

        [TestMethod]
        public void VerifyPrioritiesAreSet()
        {
            var result = underTest.Execute();
            result = new CompleteGameCommand(result).Execute();
            result = new CompleteGameCommand(result).Execute();

            foreach (var game in result.CompletedGames)
            {
                Assert.AreEqual(0, game.Priority);
            }
        }
    }
}
