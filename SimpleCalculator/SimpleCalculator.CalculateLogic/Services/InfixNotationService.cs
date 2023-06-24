using SimpleCalculator.CalculateLogic.Core.Constants;
using SimpleCalculator.CalculateLogic.Core.Extensions;
using SimpleCalculator.CalculateLogic.Core.Services;
using SimpleCalculator.CalculateLogic.Core.Tokens;
using SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators;
using SimpleCalculator.Core.Extensions;

using System.Diagnostics;
using System.Data;

namespace SimpleCalculator.CalculateLogic.Services
{
    /// <summary>
    /// Responsible for dealing with an expression written by infix notation.
    /// </summary>
    internal class InfixNotationService : NotationServiceBase, IInfixNotationService
    {
        #region Public Methods

        /// <inheritdoc />
        public double Calculate(string infixExpression)
        {
            infixExpression.ThrowArgumentNullOrEmptyException(nameof(infixExpression));

            try
            {
                var symbolTokens = new CalculatorSymbolTokens();
                var infixNotationTokens = this.GetTokensBySplitingWithWhitespace(infixExpression, symbolTokens);
                return this.Calculate(infixNotationTokens);
            }
            catch (Exception e)
            {
                throw new ArithmeticException("Failed to calculate the given expression.", e);
            }
        }

        /// <inheritdoc />
        public double Calculate(IEnumerable<CalculatorToken> infixExpressionTokens)
        {
            infixExpressionTokens.ThrowArgumentNullException(nameof(infixExpressionTokens));
            if (infixExpressionTokens.IsEmpty())
            {
                return 0.0;
            }

            try
            {
                var rpnStack = ConvertInFixToRPNInternal(infixExpressionTokens);
                if (rpnStack.IsNullOrEmpty())
                {
                    return 0.0;
                }

                var workingStack = new Stack<double>();
                while (rpnStack.Any())
                {
                    var topToken = rpnStack.Pop();
                    if (topToken.IsNumber())
                    {
                        workingStack.Push(topToken.GetNumber());
                        continue;
                    }

                    if (topToken is MathOperator @operator)
                    {
                        switch (@operator.OperatorType)
                        {
                            case MathOperatorType.Unary:
                                throw new NotSupportedException("Unary operator is not supported");

                            case MathOperatorType.Binary:
                                if (workingStack.Count() < 2)
                                {
                                    throw new InvalidDataException($"The given expression could not be calculated. {infixExpressionTokens.ToExpressionString()})");
                                }

                                var num2 = workingStack.Pop();
                                var num1 = workingStack.Pop();
                                var calculatedResult = @operator.BinaryOperate(num1, num2);
                                workingStack.Push(calculatedResult);
                                break;

                            default:
                                throw new IndexOutOfRangeException($"Unknown operator is found: {@operator.Value}");
                        }
                    }
                }

                Debug.Assert(workingStack.Any(), $"workingStack.Any()");
                return workingStack.Pop();
            }
            catch(Exception e)
            {
                throw new ArithmeticException("Failed to calculate the given expression.", e);
            }
        }

        /// <inheritdoc />
        public Stack<CalculatorToken> ConvertInFixToRPN(string infixExpression)
        {
            infixExpression.ThrowArgumentNullOrEmptyException(nameof(infixExpression));

            var symbolTokens = new CalculatorSymbolTokens();
            var infixNotationTokens = this.GetTokensBySplitingWithWhitespace(infixExpression, symbolTokens);

            var unknownTokens = infixNotationTokens.FirstOrDefault(t => t.TokenType == CalculatorTokenType.Unknown);
            if (unknownTokens is not null)
            {
                throw new SyntaxErrorException($"Found invalid token in the expression: {unknownTokens.Value}");
            }

            var result = this.ConvertInFixToRPNInternal(infixNotationTokens);
            if (result.Where(r => r.IsSymbol()).HasBrackets())
            {
                throw new SyntaxErrorException("The given expression has invalid bracket format.");
            }

            return result;
        }

        #endregion

        #region Private Methods

        private Stack<CalculatorToken> ConvertInFixToRPNInternal(IEnumerable<CalculatorToken> infixNotationTokens)
        {
            var results = new List<CalculatorToken>();
            var workingStack = new Stack<CalculatorToken>();

            foreach (var token in infixNotationTokens)
            {
                if (token.IsLeftRoundBracket())
                {
                    // Left bracket will be removed when right bracket is found
                    workingStack.Push(token);
                    continue;
                }

                if (token.IsRightRoundBracket())
                {
                    if (this.UpdateWorkingStackWhenRightRoundBracketFound(workingStack, results) == false)
                    {
                        // Failed
                        throw new SyntaxErrorException("The given expression has invalid bracket format.");
                    }

                    continue;
                }

                if (token is MathOperator foundOperator)
                {
                    this.UpdateWorkingStackWhenMathOperatorFound(foundOperator, workingStack, results);
                    continue;
                }

                if (token.IsNumber())
                {
                    results.Add(token);
                    continue;
                }
            }

            // Stores remained operators
            while (workingStack.Any())
            {
                results.Add(workingStack.Pop());
            }

            // Sorts the left-hand side of the expreession
            // to the top of the stack for calculation
            results.Reverse();
            return new Stack<CalculatorToken>(results);
        }

        private bool UpdateWorkingStackWhenRightRoundBracketFound(
            Stack<CalculatorToken> workingStack,
            ICollection<CalculatorToken> moveDestination)
        {
            bool success = false;

            // Moves tokens in working stack to result
            while (workingStack.TryPeek(out var topToken))
            {
                if (topToken.IsLeftRoundBracket())
                {
                    success = true;

                    // Removed left round bracket
                    workingStack.Pop();
                    break;
                }

                moveDestination.Add(workingStack.Pop());
            }

            return success;
        }

        private void UpdateWorkingStackWhenMathOperatorFound(
            MathOperator foundOperator,
            Stack<CalculatorToken> workingStack,
            ICollection<CalculatorToken> moveDestination)
        {
            if (workingStack.TryPeek(out var topToken) == false)
            {
                // Working stack is empty
                workingStack.Push(foundOperator);
                return;
            }

            if (foundOperator.HasHigherPriority(topToken))
            {
                workingStack.Push(foundOperator);
                return;
            }

            while (workingStack.TryPeek(out topToken) && topToken.HasSameOrHigherPriority(foundOperator))
            {
                if (topToken.IsLeftRoundBracket())
                {
                    // Left bracket must be popped when right bracket found
                    break;
                }

                 // Moves top token
                 moveDestination.Add(workingStack.Pop());
            }

            workingStack.Push(foundOperator);
        }

        #endregion
    }
}
