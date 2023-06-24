﻿using SimpleCalculator.CalculateLogic.Core.Constants;

using System.Diagnostics;

namespace SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators
{
    /// <summary>
    /// Reprensets the math operator.
    /// </summary>
    public abstract class MathOperator : CalculatorToken
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MathOperator"/> class.
        /// </summary>
        /// <param name="priority">The operator's priority.</param>
        /// <param name="value">The operator string.</param>
        /// <param name="type">The type of the operator.</param>
        public MathOperator(uint priority, string value, MathOperatorType type)
            : base(priority, value, CalculatorTokenType.Symbol)
        {
            OperatorType = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a type of the math operator.
        /// </summary>
        public MathOperatorType OperatorType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the unary operator function.
        /// </summary>
        /// <param name="x">The target value.</param>
        /// <returns>The unary operator function result.</returns>
        /// <remarks>Returns <c>double.NaN</c> if the default function is invoked.</remarks>
        public virtual double UnaryOperate(double x)
        {
            Debug.Assert(OperatorType == MathOperatorType.Unary, "this.Type == MathOperatorType.Unary");
            return double.NaN;
        }

        /// <summary>
        /// Invokes the binary operator function.
        /// </summary>
        /// <param name="x">The target value indicating left term.</param>
        /// <param name="y">The target value indicating right term.</param>
        /// <returns>The binary operator function result.</returns>
        /// <remarks>Returns <c>double.NaN</c> if the default function is invoked.</remarks>
        public virtual double BinaryOperate(double x, double y)
        {
            Debug.Assert(OperatorType == MathOperatorType.Binary, "this.Type == MathOperatorType.Binary");
            return double.NaN;
        }

        #endregion
    }
}
