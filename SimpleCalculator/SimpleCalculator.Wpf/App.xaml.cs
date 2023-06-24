using System.Windows;
using SimpleCalculator.CalculateLogic;
using SimpleCalculator.Core.Injectors;
using SimpleCalculator.Wpf.MainWindows;

namespace SimpleCalculator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = ContainerFactory.Create();

            // Register sevices
            RegisterServicesToContainer(container, new CalculatorLogicDependencyRegistrant());

            // Shows main window
            container.Register<MainWindowViewModel>();
            var mainWindow = new MainWindow()
            {
                DataContext = container.Resolve<MainWindowViewModel>(),
            };
            mainWindow.ShowDialog();
        }

        private static void RegisterServicesToContainer(IIoCContainer container, IDependencyRegistrant registrant)
        {
            registrant.RegisterServices(container);
        }
    }
}
