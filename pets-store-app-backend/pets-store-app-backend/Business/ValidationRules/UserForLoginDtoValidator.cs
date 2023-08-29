using Entities.Dto;
using FluentValidation;

namespace Business.ValidationRules
{
    public class UserForLoginDtoValidator:AbstractValidator<UserForLoginDto>
        {
            public UserForLoginDtoValidator()
            {
                RuleFor(user => user.Email).EmailAddress();
                RuleFor(user => user.Password).NotEmpty();
            }
    }
    
}
