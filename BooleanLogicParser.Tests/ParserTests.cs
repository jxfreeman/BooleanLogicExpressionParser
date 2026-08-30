using System;
using NUnit.Framework;

namespace BooleanLogicParser.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [TestCase("true", ExpectedResult = true)]
        [TestCase("false", ExpectedResult = false)]
        [TestCase("true ", ExpectedResult = true)]
        [TestCase("false ", ExpectedResult = false)]
        [TestCase(" true", ExpectedResult = true)]
        [TestCase(" false", ExpectedResult = false)]
        [TestCase(" true ", ExpectedResult = true)]
        [TestCase(" false ", ExpectedResult = false)]
        [TestCase("(false)", ExpectedResult = false)]
        [TestCase("(true)", ExpectedResult = true)]
        [TestCase("true and false", ExpectedResult = false)]
        [TestCase("false and true", ExpectedResult = false)]
        [TestCase("false and false", ExpectedResult = false)]
        [TestCase("true and true", ExpectedResult = true)]
        [TestCase("!true", ExpectedResult = false)]
        [TestCase("!(true)", ExpectedResult = false)]
        [TestCase("!(!(true))", ExpectedResult = true)]
        [TestCase("!false", ExpectedResult = true)]
        [TestCase("!(false)", ExpectedResult = true)]
        [TestCase("(!(false)) and (!(true))", ExpectedResult = false)]
        [TestCase("!((!(false)) and (!(true)))", ExpectedResult = true)]
        [TestCase("!false and !true", ExpectedResult = false)]
        [TestCase("false and true and true", ExpectedResult = false)]
        [TestCase("false or true or false", ExpectedResult = true)]
        public bool CanParseSingleToken(string expression)
        {
            var tokens = new Tokenizer(expression).Tokenize();
            var parser = new Parser(tokens);
            return parser.Parse();
        }

        [TestCase(")")]
        [TestCase("az")]
        [TestCase("")]
        [TestCase("()")]
        [TestCase("true and")]
        [TestCase("!(true")]
        public void ThrowsForInvalidExpression(string expression)
        {
            Assert.That(() =>
            {
                var tokens = new Tokenizer(expression).Tokenize();
                var parser = new Parser(tokens);
                parser.Parse();
            }, Throws.InstanceOf<Exception>());
        }
    }
}
