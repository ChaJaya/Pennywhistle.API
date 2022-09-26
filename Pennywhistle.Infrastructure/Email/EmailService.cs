using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Pennywhistle.Application.Common.Contracts;

namespace Pennywhistle.Infrastructure.Email
{
    /// <summary>
    /// implements email service
    /// </summary>
    public class EmailService : IEmailService
    {
        #region Private variables
        private readonly IConfiguration _config;
        #endregion

        #region Public Constructor
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region Public Methods
        public void SendEmail(string customerEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(customerEmail));
            email.Subject = "Noification for order update";
            email.Body = new TextPart(TextFormat.Html) { Text = "Your order status changed, please check your current order status by loging in to the system." };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        } 
        #endregion
    }
}
