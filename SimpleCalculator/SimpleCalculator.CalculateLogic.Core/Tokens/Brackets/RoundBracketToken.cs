﻿using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.Brackets
{
    /// <summary>
    /// Represents a round bracket ("(", ")") token.
    /// </summary>
    public sealed class RoundBracketToken : CalculatorToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundBracketToken"/> class.
        /// </summary>
        /// <param name="value">The token value.</param>
        public RoundBracketToken(string value) 
            : base(
                  CalculatorTokenPriority.RoundBracket,
                  value,
                  CalculatorTokenType.Symbol)
        {
        }
    }
}
