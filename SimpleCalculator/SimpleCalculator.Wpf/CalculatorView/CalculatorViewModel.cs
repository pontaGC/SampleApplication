using Prism.Commands;
using SimpleCalculator.CalculateLogic.Core.Services;
using SimpleCalculator.CalculateLogic.Core.Tokens;
using SimpleCalculator.CalculateLogic.Core.Tokens.MathOperators;
using SimpleCalculator.Core.Extensions;
using SimpleCalculator.Core.Mvvm;
using SimpleCalculator.Core.Utils;
using SimpleCalculator.Wpf.CalculatorView.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCalculator.Wpf.CalculatorView
{
    /// <summary>
    /// The view-model of a calculator.
    /// </summary>
    internal class CalculatorViewModel : ViewModelBase
    {
        private readonly IInfixNotationService infixNotationService;
        private readonly CalculatorSymbolTokens symbolTokens;
        private readonly List<CalculatorToken> calculatorTokens;

        private readonly CalculatorToken zeroToken = CalculatorToken.CreateNumberToken(0);
        private readonly StringBuilder intermidiateNubmerString = new StringBuilder();

        private string currentExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorViewModel"/> class.
        /// </summary>
        /// <param name="infixNotationService">The infix notation service.</param>
        /// <param name="symbolTokens">The calculator symbol tokens.</param>
        public CalculatorViewModel(
            IInfixNotationService infixNotationService,
            CalculatorSymbolTokens symbolTokens)
        {
            this.infixNotationService = infixNotationService;
            this.symbolTokens = symbolTokens;

            this.intermidiateNubmerString.Append(NumericalStringConstants.Zero);

            this.SelectNumberCommand = new DelegateCommand<string>(this.SelectNumber);
            this.SelectBinaryOperatorCommand = new DelegateCommand<string>(this.SelectBinaryOperator);
            this.ResetCommand = new DelegateCommand(this.InitializeCalculator);
            this.CalculateCommand = new DelegateCommand(this.Calculate);
        }
        
        /// <summary>
        /// Gets or sets a current expresion to calculate.
        /// </summary>
        public string CurrentExpression
        {
            get => this.currentExpression ?? string.Empty;
            set => this.SetProperty(ref this.currentExpression, value);
        }

        /// <summary>
        /// Gets a command that is executed when a number is selected.
        /// </summary>
        public ICommand SelectNumberCommand { get; }

        /// <summary>
        /// Gets a command that is executed when an opeartor is selected.
        /// </summary>
        public ICommand SelectBinaryOperatorCommand { get; }

        /// <summary>
        /// Gets a command that initializes a calculator.
        /// </summary>
        public ICommand ResetCommand { get; }

        /// <summary>
        /// Gets a command that calculates.
        /// </summary>
        public ICommand CalculateCommand { get; }

        #region Private Methods

        private void SelectNumber(string selectedNumber)
        {
            if (string.IsNullOrEmpty(selectedNumber))
            {
                return;
            }

            if (this.IsCurrentNumberZero())
            {
                if (this.IsZeroString(selectedNumber))
                {
                    // Not need to update
                    return;
                }

                // Changed initial value (Zero) to selected number
                this.intermidiateNubmerString.Clear();
                this.intermidiateNubmerString.Append(selectedNumber);

                this.CurrentExpression = selectedNumber;
                return;
            }

            this.intermidiateNubmerString.Append(selectedNumber);
            this.CurrentExpression += selectedNumber;
        }

        private void SelectBinaryOperator(string operatorViewString)
        {
            var operatorConverter = new OperatorConverter();
            if (!operatorConverter.TryConvertViewToLogicString(operatorViewString, out var operatorLogicString))
            {
                return;
            }

            var binaryOperator = this.symbolTokens.FindBinaryOperator(operatorLogicString);
            if (binaryOperator is null)
            {
                // Not binary operator
                return;
            }

            // At first, a binary operator is selected
            if (this.calculatorTokens.IsEmpty())
            {
                this.calculatorTokens.Add(zeroToken);
                this.calculatorTokens.Add(binaryOperator);
                this.UpdateCurrentExpression();

                return;
            }

            var currentBinaryOperator = this.GetCurrentBinaryOperator();
            if (currentBinaryOperator is not null)
            {
                // Remove to update current binary operator
                this.calculatorTokens.Remove(currentBinaryOperator);
            }

            // Determines the number
            this.AddNumberTokenToCalculatorToken();

            this.calculatorTokens.Add(binaryOperator);
            this.UpdateCurrentExpression();
        }

        private void Calculate()
        {
            try
            {
                var calculatedResult = this.infixNotationService.Calculate(this.calculatorTokens);
                this.CurrentExpression = calculatedResult.ToString();
                this.InitializeCalculator();
            }
            catch (Exception ex)
            {
                this.CurrentExpression = ex.Message;
            }
        }

        private void InitializeCalculator()
        {
            this.calculatorTokens.Clear();
            this.intermidiateNubmerString.Clear();
        }

        private bool AddNumberTokenToCalculatorToken()
        {
            var numberString = this.intermidiateNubmerString.ToString();
            if (!DoubleHelper.TryParseInvariantCulture(numberString, out var result))
            {
                return false;
            }

            var token = CalculatorToken.CreateNumberToken(result);
            this.calculatorTokens.Add(token);
            this.intermidiateNubmerString.Clear();
            return true;
        }

        private bool IsZeroString(string numberString)
        {
            return numberString == NumericalStringConstants.Zero || numberString == NumericalStringConstants.ZeroZero;
        }

        private bool IsCurrentNumberZero()
        {
            var numberString = this.intermidiateNubmerString.ToString();
            return string.Equals(numberString, NumericalStringConstants.Zero, StringComparison.OrdinalIgnoreCase);
        }

        private MathOperator GetCurrentBinaryOperator()
        {
            var currentToken = this.calculatorTokens.Last();
            if (currentToken is not MathOperator @operator)
            {
                return null;
            }

            if (@operator.OperatorType != CalculateLogic.Core.Constants.MathOperatorType.Binary)
            {
                return null;
            }

            return @operator;
        }

        private void UpdateCurrentExpression()
        {
            this.CurrentExpression = string.Empty;
            this.CurrentExpression = string.Join(' ', this.calculatorTokens);
        }

        #endregion
    }
}
