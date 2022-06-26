using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class SearchGameCommandTests
    {
        private readonly List<Game> toPlay;
        private readonly List<Game> completed;

        public SearchGameCommandTests()
        {
            toPlay = new List<Game>()
            {
                new ("First Game"),
                new ("Another Gmame"),
                new ("Woah it's a gaaaame"),
                new ("Woah it's another game"),
            };

            completed = new List<Game>()
            {
                new ("More like gayme, amirite"),
                new ("That was a joke, don't kill me."),
                new ("Game game"),
                new ("Another Gmame"),
            };


        }

        [TestMethod]
        public void GetGameFromList()
        {
            var result = new SearchGameCommand(toPlay, toPlay.First().Name).Execute().ToList();
            Assert.AreEqual(toPlay[0], result[0]);
        }

        [TestMethod]
        public void VerifyNoGameIsReturnedWithBadSearch()
        {
            var result = new SearchGameCommand(completed, Guid.NewGuid().ToString()).Execute();
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void VerifyNoGameIsReturnedWithEmptySearch()
        {
            var result = new SearchGameCommand(completed, string.Empty).Execute();
            Assert.IsTrue(!result.Any());
        }

        [TestMethod]
        public void SearchWithWrongCapitalizationReturnsTheCorrectGames()
        {
            var result = new SearchGameCommand(toPlay, "FIRst").Execute().ToList();
            Assert.AreEqual(toPlay[0], result[0]);
        }

        [TestMethod]
        public void SearchContainsMultipleResults()
        {
            var result = new SearchGameCommand(toPlay, "Another").Execute();
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void AnItemWithTheSearchMultipleTimesIsOnlyReturnedOnce()
        {
            var result = new SearchGameCommand(completed, "game").Execute().ToList();
            Assert.AreEqual(1, result.Count());
        }
    }
}
