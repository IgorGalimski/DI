using System;
using UnityEngine;

namespace DI.Sample
{
    public class SampleDIResolver : MonoBehaviour
    {
        public void Start()
        {
            var container = new Container();
            container.RegisterTransient<IEmailService, EmailService>();
            container.RegisterSingleton<ICustomerService>(new CustomerService());

            var customerService = container.Resolve<ICustomerService>();

            customerService.AddCustomerAndSendEmail("John Doe");
        }
    }
}