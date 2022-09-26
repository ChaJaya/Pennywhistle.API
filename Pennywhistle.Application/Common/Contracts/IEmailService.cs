namespace Pennywhistle.Application.Common.Contracts
{
    /// <summary>
    /// Email service contract
    /// </summary>
    public interface IEmailService
    {
        void SendEmail(string customerEmail);
    }
}
