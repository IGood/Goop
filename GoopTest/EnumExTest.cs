namespace GoopTest
{
    using Goop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumExTest
    {
        private const bool Success = true;
        private const bool Failure = false;
        private const bool CaseSensitive = false;
        private const bool CaseInsensitive = true;

        [DataTestMethod]
        [DataRow("Alvin", CaseSensitive, Success, Chipmonks.Alvin)]
        [DataRow(" Alvin ", CaseSensitive, Success, Chipmonks.Alvin)]
        [DataRow("alvin", CaseInsensitive, Success, Chipmonks.Alvin)]
        [DataRow("Theodore", CaseSensitive, Success, Chipmonks.Theodore)]
        [DataRow("theodore", CaseSensitive, Failure, default(Chipmonks))]
        [DataRow("derp", CaseSensitive, Failure, default(Chipmonks))]
        public void TryParse(string value, bool ignoreCase, bool expectedSuccess, Chipmonks expectedResult)
        {
            bool success = EnumEx.TryParse(typeof(Chipmonks), value, ignoreCase, out object result);
            Assert.AreEqual(expectedSuccess, success);
            Assert.AreEqual(expectedResult, result);
        }

        [DataTestMethod]
        [DataRow("Alvin", CaseSensitive, Success, Chipmonks.Alvin)]
        [DataRow(" Alvin ", CaseSensitive, Success, Chipmonks.Alvin)]
        [DataRow("alvin", CaseInsensitive, Success, Chipmonks.Alvin)]
        [DataRow("Theodore", CaseSensitive, Success, Chipmonks.Theodore)]
        [DataRow("theodore", CaseSensitive, Failure, default(Chipmonks))]
        [DataRow("derp", CaseSensitive, Failure, default(Chipmonks))]
        public void TryParseT(string value, bool ignoreCase, bool expectedSuccess, Chipmonks expectedResult)
        {
            bool success = EnumEx.TryParse(value, ignoreCase, out Chipmonks result);
            Assert.AreEqual(expectedSuccess, success);
            Assert.AreEqual(expectedResult, result);
        }

        public enum Chipmonks
        {
            Alvin,
            Simon,
            Theodore,
        }
    }
}
