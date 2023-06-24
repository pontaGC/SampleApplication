using SimpleCalculator.CalculateLogic.Core.Constants;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators
{
    /// <summary>
    /// Represents an operator to add.
    /// </summary>
    public sealed class AddOperator : MathOperator
    {
        #region Constructors

        public AddOperator()
            : base(
                  CalculatorTokenPriority.Add,
                  CalculatorSymbolConstants.Add,
                  MathOperatorType.Binary)
        {
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double BinaryOperate(double x, double y)
        {
            return x + y;
        }

        #endregion
    }
}
