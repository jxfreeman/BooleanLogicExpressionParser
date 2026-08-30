using System;
using System.Linq;
using NUnit.Framework;

namespace BooleanLogicParser.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        [TestCase("And", ExpectedResult = typeof(AndToken))]
        [TestCase("and", ExpectedResult = typeof(AndToken))]
        [TestCase("Or", ExpectedResult = typeof(OrToken))]
        [TestCase("or", ExpectedResult = typeof(OrToken))]
        [TestCase("True", ExpectedResult = typeof(TrueToken))]
        [TestCase("False", ExpectedResult = typeof(FalseToken))]
        [TestCase("!", ExpectedResult = typeof(NegationToken))]
        [TestCase("(", ExpectedResult = typeof(OpenParenthesisToken))]
        [TestCase(")", ExpectedResult = typeof(ClosedParenthesisToken))]
        public Type CanParseSingleToken(string expression)
        {
            var tokens = new Tokenizer(expression).Tokenize();
            return (tokens.First().GetType());
        }

        [TestCase("a")]
        [TestCase("(trae)")]
        public void ThrowsForInvalidToken(string expression)
        {
            Assert.That(() => new Tokenizer(expression).Tokenize().ToList(), Throws.InstanceOf<Exception>());
        }

        [Test]
        public void CanParseComplexTokenStructure()
        {
            var tokens = new Tokenizer("!(True And False)").Tokenize();
            var list = tokens.ToList();
            Assert.That(list[0], Is.TypeOf<NegationToken>());
            Assert.That(list[1], Is.TypeOf<OpenParenthesisToken>());
            Assert.That(list[2], Is.TypeOf<TrueToken>());
            Assert.That(list[3], Is.TypeOf<AndToken>());
            Assert.That(list[4], Is.TypeOf<FalseToken>());
            Assert.That(list[5], Is.TypeOf<ClosedParenthesisToken>());
        }
    }
}
