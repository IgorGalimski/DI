namespace DI.Sample
{
    public class CustomerService : ICustomerService
    {
        private readonly IEmailService _emailService;

        [Inject] public ICustomerRepository Repository { get; set; }

        [Inject] public IEmailService EmailService { get; set; }

        public CustomerService()
        {
            // Default constructor
        }

        [Inject]
        public CustomerService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void AddCustomerAndSendEmail(string name)
        {
        }
    }
}