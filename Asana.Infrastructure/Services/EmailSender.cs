using Asana.Application.Common.Enums;
using Asana.Application.Common.Interfaces;
using Asana.Application.Common.Models;
using Asana.Infrastructure.Persistence.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Asana.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly EmailOptions _emailConfig;
        private readonly IWebHostEnvironment _env;

        public EmailSender(ILogger<EmailSender> logger, IOptions<EmailOptions> options, IWebHostEnvironment env)
        {
            _logger = logger;
            _emailConfig = options.Value;
            _env = env;
        }

        public void SendEmail(EmailMessage emailMessage , EmailType type)
        {
            var message = CreateEmailMessage(emailMessage,type);
            send(message);
        }

        public async Task SendEmailAsync(EmailMessage emailMessage , EmailType type)
        {
            var message = await CreateEmailMessageAsync(emailMessage,type);
            await SendAsync(message);
        }



        private MimeMessage CreateEmailMessage(EmailMessage message ,EmailType type)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.Name,_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string messageBody;
            switch (type)
            {
                case EmailType.VerifiedEmail:
                     messageBody = CreateEmailContent(message.Content, "verifiedEmail.html");

                  emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) 
                  { Text = messageBody }; 
                    break;

                case EmailType.ResetPassword:

                     messageBody = CreateEmailContent(message.Content, "resetPassword.html");

                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    { Text = messageBody };
                    break;
            }
            
            return emailMessage;
        }

        private async Task<MimeMessage> CreateEmailMessageAsync(EmailMessage message, EmailType type)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var messageBody = "";
            switch (type)
            {
                case EmailType.VerifiedEmail:
                    messageBody =await CreateEmailContentAsync(message.Content, "verifiedEmail.html");

                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    { Text = messageBody };
                    break;

                case EmailType.ResetPassword:

                    messageBody =await CreateEmailContentAsync(message.Content, "resetPassword.html");

                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    { Text = messageBody };
                    break;
            }

            return emailMessage;
        }


        private string CreateEmailContent(string Link , string templateName)
        {
            var templatePath = _env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "templates"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate"
                + Path.DirectorySeparatorChar.ToString()
                + templateName;

            var builder = new BodyBuilder();

            using (StreamReader stream = new StreamReader(templatePath))
            {
                builder.HtmlBody = stream.ReadToEnd();
            }
            string messageBody = string.Format(builder.HtmlBody,Link);

            return messageBody;
        }

        private async Task<string> CreateEmailContentAsync(string Link, string templateName)
        {
            var templatePath = _env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "templates"
                + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate"
                + Path.DirectorySeparatorChar.ToString()
                + templateName;

            var builder = new BodyBuilder();

            using (var stream = new StreamReader(templatePath))
            {
                builder.HtmlBody = await stream.ReadToEndAsync();
            }
            string messageBody = string.Format(builder.HtmlBody, Link);

            return messageBody;
        }


        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception)
                {
                    _logger.LogError("Cant Send Email");
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        public void send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception)
                {
                    _logger.LogError("Cant Send Email");
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
