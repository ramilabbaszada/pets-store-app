using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Business.Concrete
{
    public class EmailManager : IEmailService
    {
        private IConfiguration _configuration;
        private EmailSettings emailSettings;
        private string appUrl;
        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
            emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            appUrl = _configuration.GetValue<string>("AppUrl");
        }

        public  async Task<IResult> SendConfirmationMailAsync(string toEmail, string token, string userName)
        {
            string mailText = ReadTemplateAndFillPlaceHolders(Directory.GetCurrentDirectory().Split("WebApi")[0] + "Business\\Html_Templates\\Confirmation_Mail_Template.html", userName);

            mailText=mailText.Replace("[link]", appUrl+"/confirm/" + token);

            await SendMailAsync(toEmail, "Confirmation", mailText);

            return new SuccessResult();
        }

        public async Task<IResult> SendCongratulationMessageAsync(string toEmail, string userName)
        {
            string mailText = ReadTemplateAndFillPlaceHolders(Directory.GetCurrentDirectory().Split("WebApi")[0] + "Business\\Html_Templates\\Congratulation_Mail_Template.html", userName);

            await SendMailAsync(toEmail, "Congratulation", mailText);

            return new SuccessResult();
        }

        public async Task<IResult> SendForgetPasswordMailAsync(string toEmail, string token, string userName)
        {
            string mailText = ReadTemplateAndFillPlaceHolders(Directory.GetCurrentDirectory().Split("WebApi")[0] + "Business\\Html_Templates\\Password_Reset_Template.html", userName);

            mailText = mailText.Replace("[link]", appUrl+"/forget-password-form/" + token);

            await SendMailAsync(toEmail, "Forget Password", mailText);

            return new SuccessResult();
        }

        public async Task<IResult> SendMailAsync(string toEmail, string subject, string body)
        {
            var networkCredential = new NetworkCredential
            {
                
                Password = emailSettings.Password,
                UserName = emailSettings.UserName,   
                
            };

            var mailMsg = new MailMessage
            {
                Body = body,
                Subject = subject,
                IsBodyHtml = true,
            };

            mailMsg.To.Add(toEmail);

            mailMsg.From = new MailAddress(emailSettings.UserName, emailSettings.Name);

            var smtpClient = new SmtpClient(emailSettings.Host)
            {
                Port = Convert.ToInt32(emailSettings.Port),
                EnableSsl = emailSettings.IsSsl,
                Credentials = networkCredential
            };


            try {
                await smtpClient.SendMailAsync(mailMsg);
            } catch(Exception e) {
                Console.WriteLine(e);
            }

            return new SuccessResult();
        }

        public async Task<IResult> SendPasswordUpdateWarningMessageAsync(string toEmail, string userName)
        {
            string mailText = ReadTemplateAndFillPlaceHolders(Directory.GetCurrentDirectory().Split("WebApi")[0] + "Business\\Html_Templates\\Password_Update_Mail.html", userName);

            await SendMailAsync(toEmail, "Password Updated Warning", mailText);

            return new SuccessResult(Messages.PasswordUpdated);
        }

        public string ReadTemplateAndFillPlaceHolders(string path, string userName) {
            StreamReader str = new StreamReader(path);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[Recipient Name]", userName);
            MailText = MailText.Replace("[Your Company Team]", _configuration.GetValue<string>("SupportTeam"));

            return MailText;
        }

    }
}
