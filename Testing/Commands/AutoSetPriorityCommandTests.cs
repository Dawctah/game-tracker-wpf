using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Commands
{
    [TestClass]
    public class AutoSetPriorityCommandTests
    {
        private readonly IEnumerable<Game> games;

        public AutoSetPriorityCommandTests()
        {
            var testList = new List<Game>();
            for (int i = 1; i < 50; i++)
            {
                testList.Add(new Game($"Game{i}", 7));
            }

            games = testList;
        }

        [TestMethod]
        public void VerifyPrioritiesAreSetProperly()
        {
            var result = new AutoSetPriorityCommand(games).Execute().ToList();

            for (int i = 0; i < result.Count; i++)
            {
                if (i > 19)
                {
                    Assert.AreEqual(20, result[i].Priority);
                }
                else
                {
                    Assert.AreEqual(i + 1, result[i].Priority);
                }
            }
        }

        [TestMethod]
        public void VerifyIndexesAreSetProperly()
        {
            var result = new AutoSetPriorityCommand(games).Execute().ToList();
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(i + 1, result[i].Index);
            }
        }

        [TestMethod]
        public void VerifyIndexesAreSetProperlyAfterChangingPriority()
        {
            var result = new AutoSetPriorityCommand(games).Execute().ToList();

            result[13].Priority = 4;

            result = new AutoSetPriorityCommand(result).Execute().ToList();

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(i + 1, result[i].Index);
            }
        }
    }
}
