namespace DI.Sample
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}