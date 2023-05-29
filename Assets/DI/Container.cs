using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type> _registeredTypes = new();

        public void Register<TInterface, TImplementation>()
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface));
        }

        private object Resolve(Type type)
        {
            var implementationType = _registeredTypes[type];
            var constructor = GetConstructorWithAttribute(implementationType);
            var constructorParams = constructor.GetParameters();

            if (constructorParams.Length == 0)
            {
                var instance = Activator.CreateInstance(implementationType);
                InjectProperties(instance);
                return instance;
            }

            List<object> resolvedParams = new List<object>();

            foreach (ParameterInfo param in constructorParams)
            {
                var paramType = param.ParameterType;
                var resolvedParam = Resolve(paramType);
                resolvedParams.Add(resolvedParam);
            }

            return Activator.CreateInstance(implementationType, resolvedParams.ToArray());
        }

        private ConstructorInfo GetConstructorWithAttribute(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            ConstructorInfo constructor =
                constructors.FirstOrDefault(c => c.GetCustomAttributes(typeof(InjectAttribute), true).Length > 0);

            if (constructor == null)
            {
                throw new Exception($"No constructor with [Inject] attribute found for type '{type}'.");
            }

            return constructor;
        }

        private void InjectProperties(object instance)
        {
            var instanceType = instance.GetType();
            var properties = instanceType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(InjectAttribute), true).Length > 0)
                {
                    var propertyType = property.PropertyType;
                    var resolvedProperty = Resolve(propertyType);
                    property.SetValue(instance, resolvedProperty);
                }
            }
        }
    }
}