using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, Type> _registeredTypes = new();
        private readonly Dictionary<Type, object> _singletonInstances = new();

        public void RegisterTransient<TInterface, TImplementation>()
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        public void RegisterSingleton<TInterface, TImplementation>()
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
            
            var instance = CreateInstance(typeof(TImplementation));
            _singletonInstances[typeof(TInterface)] = instance;
        }
        
        public void RegisterSingleton<TInterface>(object instance)
        {
            _registeredTypes[typeof(TInterface)] = instance.GetType();
            
            _singletonInstances[typeof(TInterface)] = instance;
        }

        public TInterface Resolve<TInterface>()
        {
            return (TInterface)Resolve(typeof(TInterface));
        }

        public void Unregister<TInterface>()
        {
            var type = typeof(TInterface);
            if (_registeredTypes.ContainsKey(type))
            {
                _registeredTypes.Remove(type);
            }

            if (_singletonInstances.ContainsKey(type))
            {
                _singletonInstances.Remove(type);
            }
        }

        private object Resolve(Type type)
        {
            if (_singletonInstances.TryGetValue(type, out var singletonInstance))
            {
                return singletonInstance;
            }

            var instance = CreateInstance(type);
            InjectProperties(instance);

            return instance;
        }

        private object CreateInstance(Type type)
        {
            var constructor = GetConstructorWithAttribute(type);
            var constructorParams = constructor.GetParameters();

            if (constructorParams.Length == 0)
            {
                var instance = Activator.CreateInstance(type);
                return instance;
            }

            var resolvedParams = new List<object>();

            foreach (var param in constructorParams)
            {
                var paramType = param.ParameterType;
                var resolvedParam = Resolve(paramType);
                resolvedParams.Add(resolvedParam);
            }

            return Activator.CreateInstance(type, resolvedParams.ToArray());
        }

        private ConstructorInfo GetConstructorWithAttribute(Type type)
        {
            var constructors = type.GetConstructors();
            var constructor =
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