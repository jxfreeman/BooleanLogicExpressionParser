using System;
using System.Collections.Generic;

namespace BooleanLogicParser
{
    // Expression         := [ "!" ] <Boolean> { <BooleanOperator> <Boolean> } ...
    // Boolean            := <BooleanConstant> | <Expression> | "(" <Expression> ")"
    // BooleanOperator    := "And" | "Or" 
    // BooleanConstant    := "True" | "False"
    public class Parser
    {
        private readonly IEnumerator<Token> _tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _tokens.MoveNext();
        }

        public bool Parse()
        {
            while (_tokens.Current != null)
            {
                var isNegated = _tokens.Current is NegationToken;
                if (isNegated)
                    _tokens.MoveNext();

                var boolean = ParseBoolean();
                if (isNegated)
                    boolean = !boolean;

                while (_tokens.Current is OperandToken)
                {
                    var operand = _tokens.Current;
                    if (!_tokens.MoveNext())
                    {
                        throw new Exception("Missing expression after operand");
                    }
                    var nextBoolean = ParseBoolean();

                    if (operand is AndToken)
                        boolean = boolean && nextBoolean;
                    else
                        boolean = boolean || nextBoolean;

                }

                return boolean;
            }

            throw new Exception("Empty expression");
        }

        private bool ParseBoolean()
        {
            switch (_tokens.Current)
            {
                case BooleanValueToken:
                {
                    var current = _tokens.Current;
                    _tokens.MoveNext();

                    return current is TrueToken;
                }
                case OpenParenthesisToken:
                {
                    _tokens.MoveNext();

                    var expInPars = Parse();

                    if (_tokens.Current is not ClosedParenthesisToken)
                        throw new Exception("Expecting Closing Parenthesis");
                    
                    _tokens.MoveNext(); 

                    return expInPars;
                }
                case ClosedParenthesisToken:
                    throw new Exception("Unexpected Closed Parenthesis");
                default:
                {
                    // since it's not a BooleanConstant or Expression in parentheses, it must be an expression again
                    var val = Parse();
                    return val;
                }
            }
        }
    }
}