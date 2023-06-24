using SimpleCalculator.CalculateLogic.Core.Constants;
using SimpleCalculator.CalculateLogic.Core.Tokens;
using SimpleCalculator.Core.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCalculator.CalculateLogic.Services
{
    /// <summary>
    /// The common methods/properties for notation services.
    /// </summary>
    internal abstract class NotationServiceBase
    {
        #region Protected Methods

        /// <summary>
        /// Gets all tokens by spliting an expression with whitespaces.
        /// </summary>
        /// <param name="expression">The target expression to get.</param>
        /// <returns>A collection of split tokens.</returns>
        [return: NotNull]
        protected IEnumerable<CalculatorToken> GetTokensBySplitingWithWhitespace(
            string expression,
            CalculatorSymbolTokens symbolTokens)
        {
            if (string.IsNullOrEmpty(expression))
            {
                yield break;
            }

            foreach(var splitedValue in expression.SplitWithWhitespace())
            {
                var foundSymbol = symbolTokens.FindSymbolToken(splitedValue);
                if (foundSymbol.IsNotEmpty)
                {
                    yield return foundSymbol;
                    continue;
                }

                if (IsNumberToken(splitedValue))
                {
                    yield return new CalculatorToken(CalculatorTokenPriority.Any, splitedValue, CalculatorTokenType.Number);
                    continue;
                }

                yield return CalculatorToken.Empty;
            }
        }

        #endregion

        #region Private Methods

        private bool IsNumberToken(string tokenValue)
        {
            return double.TryParse(tokenValue, out var _);
        }

        #endregion
    }
}
