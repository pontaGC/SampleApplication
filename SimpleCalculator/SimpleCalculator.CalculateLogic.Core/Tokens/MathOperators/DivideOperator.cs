using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators
{
    /// <summary>
    /// Represents an operator to divide.
    /// </summary>
    public sealed class DivideOperator : MathOperator
    {
        #region Constractors

        public DivideOperator()
            : base(
                  CalculatorTokenPriority.Divide,
                  CalculatorSymbolConstants.Divide,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x / y;
        }

        #endregion
    }
}
