namespace DI
{
    public interface IContainer
    {
        void Register<TInterface, TImplementation>();
        TInterface Resolve<TInterface>();
    }
}