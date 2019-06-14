using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OmsApi.Interfaces
{
    public interface IOmsEmailSender
    {
        Task Send2AdminSignupMessage(string username, string fullName, ILogger logger);

        Task Send2AdminRequestSubmitMessage(string fromUsername, string fullName, ILogger logger);

        Task Send2ClientSignupResponseMessage(string email, string resultMessage, ILogger logger);

        Task Send2ClientForgotPasswordMessage(string email, string resetPasswordLink, ILogger logger);        
    }
}
