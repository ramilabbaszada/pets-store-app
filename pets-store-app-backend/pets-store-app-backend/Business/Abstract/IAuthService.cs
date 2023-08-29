using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dto;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IResult> ForgetPasswordHandler(ForgetPasswordDto forgetPasswordDto);
        Task<IResult> ForgetPassword(string mailAddress);
        Task<IResult> Register(UserForRegisterDto userForRegisterDto);
        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);
        Task<IDataResult<AccessToken>> CreateAccessToken(User user);
        Task<IResult> UserExists(string email);
        Task<IDataResult<User>> ConfirmRegistration(string token);
        string GenerateToken(int length);
    }
}
