using System.ComponentModel;
using SimpleCalculator.CalculateLogic.Core.Services;
using SimpleCalculator.Core.Mvvm;
using SimpleCalculator.Wpf.CalculatorView;

namespace SimpleCalculator.Wpf.MainWindows
{
    /// <summary>
    /// The view-model of the main window.
    /// </summary>
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly IInfixNotationService infixNotationService;
        private INotifyPropertyChanged contentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="infixNotationService">The infix notation service.</param>
        public MainWindowViewModel(IInfixNotationService infixNotationService)
        {
            this.infixNotationService = infixNotationService;
            this.ContentViewModel = new CalculatorViewModel(this.infixNotationService, new CalculateLogic.Core.Tokens.CalculatorSymbolTokens());
        }

        /// <summary>
        /// Gets or sets a view-model of the calculator.
        /// </summary>
        public INotifyPropertyChanged ContentViewModel 
        {
            get => this.contentViewModel;
            set => this.SetProperty(ref this.contentViewModel, value);
        }
    }
}
