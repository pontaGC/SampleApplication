using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleCalculator.CalculateLogic.Core.Tokens;

namespace SimpleCalculator.CalculateLogic.Core.Services
{
    /// <summary>
    /// Responsible for dealing with an expression written by infix notation.
    /// </summary>
    public interface IInfixNotationService
    {
        /// <summary>
        /// Calculates an expression with infix notation.
        /// </summary>
        /// <param name="infixExpression">The expersion to calculate.</param>
        /// <returns>A calculated value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="infixExpression"/> is <c>null</c> or empty.</exception>
        /// <exception cref="ArithmeticException">Failed to calculate the given expression. See inner exception for details.</exception>
        /// <remarks>
        /// Tokens in <paramref name="infixExpression"/> must be separated by whitespace.
        /// (数式内の各トークンが空白で区切られている必要があります。)
        /// </remarks>
        double Calculate(string infixExpression);

        /// <summary>
        /// Calculates an expression with infix notation.
        /// </summary>
        /// <param name="infixExpressionTokens">The tokens in infix expression to calculate.</param>
        /// <returns>A calculated value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="infixExpressionTokens"/> is <c>null</c>.</exception>
        /// <exception cref="ArithmeticException">Failed to calculate the given expression. See inner exception for details.</exception>
        double Calculate(IEnumerable<CalculatorToken> infixExpressionTokens);

        /// <summary>
        /// Converts the infix notation to the reverse Polish notation (RPN).
        /// </summary>
        /// <param name="infixExpression">The expression written by infix notation.</param>
        /// <returns>A converted tokens.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="infixExpression"/> is <c>null</c> or empty.</exception>
        /// <exception cref="SyntaxErrorException">
        /// <para>Found an unknown token <paramref name="infixExpression"/>.</para>
        /// <para>Found an invalid bracket format the given expression.</para>
        /// </exception>
        /// <remarks>
        /// Tokens in <paramref name="infixExpression"/> must be separated by whitespace.
        /// (数式内の各トークンが空白で区切られている必要があります。)
        /// </remarks>
        Stack<CalculatorToken> ConvertInFixToRPN(string infixExpression);
    }
}
