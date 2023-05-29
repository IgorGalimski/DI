using System;

namespace DI
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
    }
}