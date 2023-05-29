using UnityEngine;

namespace DI.Sample
{
    public class CustomerRepository : ICustomerRepository
    {
        public void AddCustomer(string name)
        {
            Debug.Log($"Adding customer: {name}");
        }
    }
}