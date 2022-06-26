using Core.Commands;
using Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Commands
{
    [TestClass]
    public class AddGameCommandTests
    {
        private readonly AddGameCommand underTest;
        public AddGameCommandTests()
        {
            var testList = new List<Game>() { };
            underTest = new AddGameCommand(new(testList, new Game()));
        }

        [TestMethod]
        public void VerifyGameIsAddedToList()
        {
            var result = underTest.Execute();
            Assert.IsTrue(result.Count() == 1);
        }
    }
}
