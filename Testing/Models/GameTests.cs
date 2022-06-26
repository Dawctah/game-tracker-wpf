using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Testing.Models
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void PriorityDoesNotGoAboveTwenty()
        {
            var game = new Game("Test Game", 300);

            Assert.AreEqual(20, game.Priority);
        }

        [TestMethod]
        public void PriorityDoesNotGoBelowZero()
        {
            var game = new Game("Test Game", -100);
            Assert.AreEqual(0, game.Priority);
        }

        [TestMethod]
        public void VerifyToStringWhenPriorityIsAboveZero()
        {
            var game = new Game("Test Game", 3);
            var expected = "Test Game, Priority: 3 INDEX: 0";

            Assert.AreEqual(expected, game.ToString());
        }

        [TestMethod]
        public void VerifyToStringWhenPriorityIsZero()
        {
            var game = new Game("Test Game");
            var expected = "Test Game INDEX: 0";

            Assert.AreEqual(expected, game.ToString());
        }

        [TestMethod]
        public void VerifyToStringWhenPriorityIsZeroAndTimeIsProvided()
        {
            var now = DateTime.Now;
            var game = new Game("Test Game", now);
            var expected = $"Test Game {now.Year}/{now.Month}/{now.Day} INDEX: 0";

            Assert.AreEqual(expected, game.ToString());
        }
    }
}
