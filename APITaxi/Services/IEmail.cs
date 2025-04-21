using APITaxiV2.Models;
namespace APITaxiV2.Services
{
    public interface IEmail
    {
        void SendEmail(EmailDTO request);
    }
}
