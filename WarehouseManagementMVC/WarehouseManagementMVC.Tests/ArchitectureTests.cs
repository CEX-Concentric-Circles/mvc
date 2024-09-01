using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace WarehouseManagementTests
{
    public class ArchitectureTests
    {
        [Fact]
        public void ControllersShouldOnlyDependOnInterfaces()
        {
            var controllerAssembly = Assembly.GetAssembly(typeof(WarehouseManagementMVC.Controllers.CustomersController));

            var controllers = controllerAssembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .ToList();

            foreach (var controller in controllers)
            {
                var constructors = controller.GetConstructors();
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        Assert.True(parameter.ParameterType.IsInterface,
                            $"Controller {controller.Name} should depend on interfaces, but depends on {parameter.ParameterType.Name}.");
                    }
                }
            }
        }

        [Fact]
        public void ServicesShouldImplementCorrectInterfaces()
        {
            var serviceAssembly = Assembly.GetAssembly(typeof(WarehouseManagementMVC.Services.ProductService));
        
            var services = serviceAssembly.GetTypes()
                .Where(t => t.Name.EndsWith("Service"))
                .ToList();
        
            foreach (var service in services)
            {
                if (service.IsClass && !service.IsAbstract)
                {
                    var interfaces = service.GetInterfaces();
                    
                    Assert.True(interfaces.Any(),
                        $"Service class {service.Name} does not implement any interface.");
                }
            }
        }

        [Fact]
        public void ControllersShouldNotManipulateModelsDirectly()
        {
            var controllerAssembly = Assembly.GetAssembly(typeof(WarehouseManagementMVC.Controllers.CustomersController));

            var controllers = controllerAssembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .ToList();

            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    var parameters = method.GetParameters();
                    foreach (var param in parameters)
                    {
                        if (IsModelEntity(param.ParameterType))
                        {
                            Assert.False(true, $"Controller {controller.Name}, method {method.Name} directly manipulates a model entity {param.ParameterType.Name} as a parameter.");
                        }
                    }

                    if (IsModelEntity(method.ReturnType))
                    {
                        Assert.False(true, $"Controller {controller.Name}, method {method.Name} directly returns a model entity {method.ReturnType.Name}.");
                    }
                }
            }
        }

        private bool IsModelEntity(Type type)
        {
            return type.Namespace == "WarehouseManagementMVC.Models" && !type.IsPrimitive && type != typeof(string);
        }
    }
}
