using UnityEngine;

namespace DI.Sample
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            Debug.Log($"Sending email to: {to}, Subject: {subject}, Body: {body}");
        }
    }
}