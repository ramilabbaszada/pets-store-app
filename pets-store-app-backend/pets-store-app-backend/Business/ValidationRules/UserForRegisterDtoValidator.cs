using Entities.Dto;
using FluentValidation;

namespace Business.ValidationRules
{
    public class UserForRegisterDtoValidator : AbstractValidator<UserForRegisterDto>
        {
            public UserForRegisterDtoValidator()
            {
                RuleFor(user => user.Email).EmailAddress();
                RuleFor(user => user.Password).NotEmpty();
                RuleFor(user => user.FirstName).NotEmpty();
                RuleFor(user => user.LastName).NotEmpty();
            }
    }
    
}
