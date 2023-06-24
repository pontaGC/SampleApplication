using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators
{
    /// <summary>
    /// Represents an operator to subtract.
    /// </summary>
    public sealed class SubtractOperator : MathOperator
    {
        #region Constructors

        public SubtractOperator()
            : base(
                  CalculatorTokenPriority.Subtract,
                  CalculatorSymbolConstants.Subtract,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x - y;
        }

        #endregion
    }
}
