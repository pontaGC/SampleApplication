using EditorAppFramework.Presentation.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Wpf.CalculatorView.Commands
{
    internal class SelectNumberCommand : CommandActionBase
    {
        /// <inheritdoc />
        protected override bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <inheritdoc />
        protected override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
