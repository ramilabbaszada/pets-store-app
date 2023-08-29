using FluentValidation;

namespace Business.ValidationRules
{
    public class EmailValidator: AbstractValidator<string>
    {
        public EmailValidator() {
            RuleFor(email => email).EmailAddress();
        }
        
    }
}
