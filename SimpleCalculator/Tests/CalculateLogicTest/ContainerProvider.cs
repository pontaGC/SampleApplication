using SimpleCalculator.CalculateLogic;
using SimpleCalculator.Core.Injectors;

namespace CalculateLogicTest
{
    internal class ContainerProvider
    {
        static ContainerProvider()
        {
            Container = ContainerFactory.Create();

            var serviceRegistrant = new CalculatorLogicDependencyRegistrant();
            serviceRegistrant.RegisterServices(Container);
        }

        private ContainerProvider()
        {
            // Do Not change accesibility for singleton instance
        }

        private static readonly IIoCContainer Container;

        internal static IIoCContainer GetContainer()
        {
            return Container;
        }
    }
}
