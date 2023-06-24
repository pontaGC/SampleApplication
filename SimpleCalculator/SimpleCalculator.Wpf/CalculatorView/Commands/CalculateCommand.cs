using EditorAppFramework.Presentation.Core.Commands;
using SimpleCalculator.CalculateLogic.Core.Services;
using SimpleCalculator.Core.Commands;
using SimpleCalculator.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Wpf.CalculatorView.Commands
{
    internal class CalculateCommand : CommandActionBase
    {
        private readonly IInfixNotationService infixNotationService;
        private Action<ICommandResult<double>> commandCallBack;

        public CalculateCommand(IInfixNotationService infixNotationService)
        {
            infixNotationService.ThrowArgumentNullException(nameof(infixNotationService));

            this.infixNotationService = infixNotationService;
        }

        /// <summary>
        /// Sets a call-back action to execute after the command is executed.
        /// </summary>
        /// <param name="callBackAction"></param>
        internal void SetCommandCallBack(Action<ICommandResult<double>> callBackAction)
        {
            this.commandCallBack = callBackAction;
        }

        protected override bool CanExecute(object? parameter)
        {
            return true;
        }

        protected override void Execute(object? parameter)
        {
            var expression = parameter as string;
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            try
            {
                var result = this.infixNotationService.Calculate(expression);
                var successResult = CommandResult<double>.Succeeded(result);
                this.commandCallBack?.Invoke(successResult);
            }
            catch(ArithmeticException arithmeticEx)
            {
                var failedResult = CommandResult<double>.Failed(arithmeticEx.Message);
                this.commandCallBack?.Invoke(failedResult);
            }
        }
    }
}
