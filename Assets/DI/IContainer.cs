namespace DI
{
    public interface IContainer
    {
        void RegisterTransient<TInterface, TImplementation>();
        void RegisterSingleton<TInterface, TImplementation>();
        void RegisterSingleton<TInterface>(object instance);
        
        TInterface Resolve<TInterface>();
    }
}