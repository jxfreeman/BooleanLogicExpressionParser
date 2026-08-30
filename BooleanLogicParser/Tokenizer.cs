using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BooleanLogicParser
{
    public class Tokenizer
    {
        private readonly StringReader _reader;
        private readonly string _text;

        public Tokenizer(string text)
        {
            _text = text;
            _reader = new StringReader(text);
        }

        public IEnumerable<Token> Tokenize()
        {
            var tokens = new List<Token>();
            while (_reader.Peek() != -1)
            {
                while (char.IsWhiteSpace((char) _reader.Peek()))
                {
                    _reader.Read();
                }

                if (_reader.Peek() == -1)
                    break;

                var c = (char) _reader.Peek();
                switch (c)
                {
                    case '!':
                        tokens.Add(new NegationToken());
                        _reader.Read();
                        break;
                    case '(':
                        tokens.Add(new OpenParenthesisToken());
                        _reader.Read();
                        break;
                    case ')':
                        tokens.Add(new ClosedParenthesisToken());
                        _reader.Read();
                        break;
                    default:
                        if (char.IsLetter(c))
                        {
                            var token = ParseKeyword();
                            tokens.Add(token);
                        }
                        else
                        {
                            var remainingText = _reader.ReadToEnd() ?? string.Empty;
                            throw new Exception($"Unknown grammar found at position {_text.Length - remainingText.Length} : '{remainingText}'");
                        }
                        break;
                }
            }
            return tokens;
        }

        private Token ParseKeyword()
        {
            var text = new StringBuilder();
            while (char.IsLetter((char) _reader.Peek()))
            {
                text.Append((char) _reader.Read());
            }

            var potentialKeyword = text.ToString().ToLower();

            return potentialKeyword switch
            {
                "true" => new TrueToken(),
                "false" => new FalseToken(),
                "and" => new AndToken(),
                "or" => new OrToken(),
                _ => throw new Exception("Expected keyword (True, False, And, Or) but found " + potentialKeyword)
            };
        }
    }
}