using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators
{
    /// <summary>
    /// Represents an operator to multiply.
    /// </summary>
    public sealed class MultiplyOperator : MathOperator
    {
        #region Constructors

        public MultiplyOperator()
            : base(
                  CalculatorTokenPriority.Multiply,
                  CalculatorSymbolConstants.Multiply,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x * y;
        }

        #endregion
    }
}
