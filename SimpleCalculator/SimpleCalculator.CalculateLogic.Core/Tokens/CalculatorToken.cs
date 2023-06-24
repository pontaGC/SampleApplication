using SimpleCalculator.CalculateLogic.Core.Constants;
using SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculateLogic.Core.Tokens
{
    /// <summary>
    /// Represents a token used by calculator.
    /// </summary>
    public class CalculatorToken
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorToken"/> class.
        /// </summary>
        /// <param name="priority">The token priority.</param>
        /// <param name="tokenType">The token type.</param>
        /// <param name="value">The token value.</param>
        public CalculatorToken(
            uint priority,
            string value,
            CalculatorTokenType tokenType)
        {
            this.Priority = priority;
            this.TokenType = tokenType;
            this.Value = value ?? string.Empty;
        }

        #endregion

        /// <summary>
        /// Gets an empty token.
        /// </summary>
        public static readonly CalculatorToken Empty
            = new CalculatorToken(
            CalculatorTokenPriority.Any,
            string.Empty,
            CalculatorTokenType.Unknown);

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not this token is empty.
        /// </summary>
        /// <value><c>true</c> if this token is different from <see cref="Empty"/>, otherwise, <c>false</c>.</value>
        public bool IsNotEmpty => this != Empty;

        /// <summary>
        /// Gets an integer indicating the calculator token priority.
        /// The higher values ​​have higher priority.
        /// </summary>
        public uint Priority { get; }

        /// <summary>
        /// Gets a token value.
        /// </summary>
        [MemberNotNull]
        public string Value { get; }

        /// <summary>
        /// Gets a token type.
        /// </summary>
        public CalculatorTokenType TokenType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of the number token.
        /// </summary>
        /// <param name="number">The number value.</param>
        /// <returns>An instance of the number token.</returns>
        public static CalculatorToken CreateNumberToken(double number)
        {
            return new CalculatorToken(CalculatorTokenPriority.Any, number.ToString(), CalculatorTokenType.Number);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is lower than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is lower than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasLowerPriority(CalculatorToken tokenToCompare)
        {
            return !this.HasSameOrHigherPriority(tokenToCompare);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is higher than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasHigherPriority(CalculatorToken tokenToCompare)
        {
            if (tokenToCompare is null)
            {
                return true;
            }

            return this.HasHigherPriority(tokenToCompare.Priority);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns><c>true</c> if this token's priority is higher than <paramref name="targetPriority"/>, otherwise, <c>false</c>.</returns>
        public bool HasHigherPriority(uint targetPriority)
        {
            return Priority > targetPriority;
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is equal to or higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns>
        /// <c>true</c> if this token's priority is equal to or higher than <paramref name="tokenToCompare"/>'s priority,
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool HasSameOrHigherPriority(CalculatorToken tokenToCompare)
        {
            if (tokenToCompare is null)
            {
                return true;
            }

            return this.HasSameOrHigherPriority(tokenToCompare.Priority);
        }

        /// <summary>
        /// Checks wthether or not this token's priorty is equal to or higher than target token's priority.
        /// </summary>
        /// <param name="tokenToCompare">The token to compare.</param>
        /// <returns><c>true</c> if this token's priority is equal to or higher than <paramref name="targetPriority"/>, otherwise, <c>false</c>.</returns>
        public bool HasSameOrHigherPriority(uint targetPriority)
        {
            return  Priority >= targetPriority;
        }

        /// <summary>
        /// Gets a value indicating this token is symbol.
        /// </summary>
        /// <returns><c>true</c> if the token type is <c>Symbol</c>, otherwise, <c>false</c>.</returns>
        public bool IsSymbol()
        {
            return this.TokenType == CalculatorTokenType.Symbol;
        }

        /// <summary>
        /// Gets a value indicating this token is numerical value.
        /// </summary>
        /// <returns><c>true</c> if the token type is <c>Number</c>, otherwise, <c>false</c>.</returns>
        public bool IsNumber()
        {
            return this.TokenType == CalculatorTokenType.Number;
        }

        /// <summary>
        /// Gets a numerical value of this token value.
        /// </summary>
        /// <returns>A numerical value of this token value.</returns>
        public double GetNumber()
        {
            Debug.Assert(this.IsNumber(), "this.IsNumber()");

            if (double.TryParse(this.Value, out var typedValue))
            {
                return typedValue;
            }

            const double Zero = 0.0;
            return Zero;
        }

        #endregion
    }
}
