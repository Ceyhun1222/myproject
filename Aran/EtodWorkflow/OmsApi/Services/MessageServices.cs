using Microsoft.Extensions.Options;
using OmsApi.Configuration;
using OmsApi.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OmsApi.Services
{
    public class MessageSender : ISmsSender, IOmsEmailSender
    {
        private SendGridConfig _sendGridSettings;
        private readonly EmailAddress _serviceFromAddress;
        private readonly EmailAddress _adminAddress;
        private MessageTemplateConfig _msgTemplateSettings;

        public MessageSender(Microsoft.Extensions.Configuration.IConfiguration configuration,
            SendGridConfig sendGridSettings, AdminConfig adminSettings, 
            MessageTemplateConfig msgTemplateSettings)
        {
            _sendGridSettings = sendGridSettings;
            _msgTemplateSettings = msgTemplateSettings;
            _serviceFromAddress = new EmailAddress(_sendGridSettings.FromAddress, _sendGridSettings.FromName);
            _adminAddress = new EmailAddress(adminSettings.Email);
        }

        public async Task Send2AdminRequestSubmitMessage(string fromUsername, string fullName, ILogger logger)
        {
            var msgBody = string.Format(_msgTemplateSettings.RequestSubmitBody, fullName);
            await SendEmailAsync(_adminAddress, _msgTemplateSettings.RequestSubmitSubject, msgBody,logger);
        }

        public async Task Send2AdminSignupMessage(string username, string fullName, ILogger logger)
        {
            var msgBody = string.Format(_msgTemplateSettings.SignupBody, username);
            await SendEmailAsync(_adminAddress, _msgTemplateSettings.SignupSubject, msgBody,logger);
        }

        public async Task Send2ClientForgotPasswordMessage(string email, string resetPasswordLink, ILogger logger)
        {
            var toEmail = new EmailAddress(email);
            var msgBody = string.Format(_msgTemplateSettings.ForgotPasswordBody, resetPasswordLink);
            await SendEmailAsync(toEmail, _msgTemplateSettings.ForgotPasswordSubject, msgBody,logger);
        }

        public async Task Send2ClientSignupResponseMessage(string email, string resultMessage, ILogger logger)
        {
            var toEmail = new EmailAddress(email);
            var msgBody = string.Format(_msgTemplateSettings.SignupResponseBody, resultMessage);
            await SendEmailAsync(toEmail, _msgTemplateSettings.SignupResponseSubject, msgBody,logger);
        }

        public async Task SendEmailAsync(EmailAddress to, string subject, string message,ILogger logger)
        {
            try
            {
                logger.LogTrace("Sending message ...");
                if (string.IsNullOrEmpty(to.Email))
                    return;
                logger.LogTrace($"To : {to.Email}");
                var client = new SendGridClient(_sendGridSettings.ApiKey);
                logger.LogTrace($"Api Key => ({_sendGridSettings.ApiKey})");
                logger.LogTrace($"Service address : {_serviceFromAddress.Name} ({_serviceFromAddress.Email})");
                logger.LogTrace($"Subject : {subject}");
                logger.LogTrace($"Body : {message}");
                logger.LogTrace($"Url path : {client.UrlPath}");
                var msg = MailHelper.CreateSingleEmail(_serviceFromAddress, to, subject, message, message);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while sending message");
                throw;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
