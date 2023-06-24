using SimpleCalculator.CalculateLogic.Core.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SimpleCalculator.Wpf.CalculatorView.Converters
{
    /// <summary>
    /// Converts the binding source value which type is <c>string</c> to the binding target value which type is <c>string</c>.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    internal class OperatorConverter : IValueConverter
    {
        private readonly IReadOnlyDictionary<string, string> operatorConversions;

        /// <summary>
        /// Intializes a new instance of the <see cref="OperatorConverter"/> class.
        /// </summary>
        public OperatorConverter()
        {
            this.operatorConversions = new Dictionary<string, string>()
            {
                { OperatorConstants.Add, CalculatorSymbolConstants.Add },
                { OperatorConstants.Subtract, CalculatorSymbolConstants.Subtract },
                { OperatorConstants.Multiply, CalculatorSymbolConstants.Multiply },
                { OperatorConstants.Divide, CalculatorSymbolConstants.Divide },
                { OperatorConstants.LeftRoundBracket, CalculatorSymbolConstants.RoundBracket.Left },
                { OperatorConstants.RightRoundBracket, CalculatorSymbolConstants.RoundBracket.Right },
            };
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logicOperatorValue = value as string;
            if (string.IsNullOrEmpty(logicOperatorValue))
            {
                return DependencyProperty.UnsetValue;
            }

            foreach(var viewOperatorValue in operatorConversions.Keys)
            {
                if (logicOperatorValue.Equals(operatorConversions[viewOperatorValue], StringComparison.OrdinalIgnoreCase))
                {
                    return viewOperatorValue;
                }
            }

            Debug.Assert(false, $"Found unknown operator: {logicOperatorValue}");
            return logicOperatorValue;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var viewOperatorValue = value as string;
            if (string.IsNullOrEmpty(viewOperatorValue))
            {
                return Binding.DoNothing;
            }

            if (TryConvertViewToLogicString(viewOperatorValue, out var logicOperatorValue))
            {
                return logicOperatorValue;
            }

            Debug.Assert(false, $"Found unknown operator: {viewOperatorValue}");
            return viewOperatorValue;
        }

        /// <summary>
        /// Try to convert view string to logic string.
        /// </summary>
        /// <param name="operatorViewString">The operator view string.</param>
        /// <param name="result">The logic string.</param>
        /// <returns><c>true</c> if the conversion is successful, otherwise, <c>false</c>.</returns>
        public bool TryConvertViewToLogicString(string operatorViewString, out string result)
        {
            return this.operatorConversions.TryGetValue(operatorViewString, out result);
        }
    }
}
