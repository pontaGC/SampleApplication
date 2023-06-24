using SimpleCalculator.CalculateLogic.Core.Services;
using SimpleCalculator.CalculateLogic.Services;
using SimpleCalculator.Core.Injectors;

namespace SimpleCalculator.CalculateLogic
{
    /// <summary>
    /// The depdency registrant.
    /// </summary>
    public sealed class CalculatorLogicDependencyRegistrant : IDependencyRegistrant
    {
        /// <inheritdoc />
        public void RegisterServices(IIoCContainer container)
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register<IInfixNotationService, InfixNotationService>();
        }
    }
}
