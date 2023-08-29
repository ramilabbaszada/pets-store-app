using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IEmailService
    {
        public Task<IResult> SendMailAsync(string toEmail, string subject, string body);
        public Task<IResult> SendConfirmationMailAsync(string toEmail,string token, string userName);
        public Task<IResult> SendForgetPasswordMailAsync(string toEmail,string token, string userName);
        public Task<IResult> SendCongratulationMessageAsync(string toEmail, string userName);
        public Task<IResult> SendPasswordUpdateWarningMessageAsync(string toEmail, string userName);
    }
}
