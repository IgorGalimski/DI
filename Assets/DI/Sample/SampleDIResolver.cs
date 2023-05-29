using System;
using UnityEngine;

namespace DI.Sample
{
    public class SampleDIResolver : MonoBehaviour
    {
        public void Start()
        {
            var container = new Container();
            container.Register<IEmailService, EmailService>();
            container.Register<ICustomerService, CustomerService>();

            var customerService = container.Resolve<ICustomerService>();

            customerService.AddCustomerAndSendEmail("John Doe");
        }
    }
}