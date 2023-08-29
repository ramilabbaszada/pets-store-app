using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dto;
using System.Security.Cryptography;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private ICacheManager _cacheManager;
        private IEmailService _emailService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICacheManager cacheManager,IEmailService emailService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _cacheManager=cacheManager;
            _emailService = emailService;
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(EmailValidator))]
        public async Task<IResult> ForgetPassword(string mailAddress)
        {
            IDataResult<User> user = await _userService.GetByMail(mailAddress);

            if (user.Data==null)
                return new ErrorResult(Messages.UserDoesNotExists);

            string resetToken = GenerateUrlSafeToken(64);
            _cacheManager.Add(resetToken, mailAddress, 5);

            _emailService.SendForgetPasswordMailAsync(mailAddress, resetToken, user.Data.FirstName+" "+user.Data.LastName);
            return new SuccessResult(Messages.PasswordResetMailSent);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IResult> ForgetPasswordHandler(ForgetPasswordDto forgetPasswordDto)
        {
            string mailAddress = _cacheManager.Get<string>(forgetPasswordDto.ResetToken);

            if (mailAddress==null)
                return new ErrorResult(Messages.ForgetPasswordError);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(forgetPasswordDto.Password, out passwordHash, out passwordSalt);

            User user=(await _userService.GetByMail(mailAddress)).Data;

            user.PasswordHash = passwordHash;
            user.PasswordSalt= passwordSalt;

            await _userService.Update(user);

            _emailService.SendPasswordUpdateWarningMessageAsync(mailAddress,user.FirstName+" "+user.LastName);

            return new SuccessResult(Messages.PasswordUpdated);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        public async Task<IResult> Register(UserForRegisterDto userForRegisterDto)
        {
            string confirmationToken = GenerateUrlSafeToken(64);

            _cacheManager.Add(confirmationToken, userForRegisterDto, 5);

            _emailService.SendConfirmationMailAsync(userForRegisterDto.Email, confirmationToken, userForRegisterDto.FirstName + " " + userForRegisterDto.LastName);

            return new SuccessResult(Messages.ConfirmationMailSent);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IDataResult<User>> ConfirmRegistration(string token) {

            UserForRegisterDto userForRegisterDto = _cacheManager.Get<UserForRegisterDto>(token);

            if (userForRegisterDto == null)
                return new ErrorDataResult<User>(Messages.RegistrationError);

            IResult DoesUserExists = await UserExists(userForRegisterDto.Email);

            if (!DoesUserExists.Success)
                return new ErrorDataResult<User>(DoesUserExists.Message);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
            string refreshToken = GenerateToken(64);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RefreshToken= refreshToken,
                Status = true
            };
            await _userService.Add(user);
            _emailService.SendCongratulationMessageAsync(user.Email, user.FirstName + " " + user.LastName);
            return new SuccessDataResult<User>(user,Messages.UserHasBeenCreated);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByMail(userForLoginDto.Email);

            if (userToCheck.Data==null)
                return new ErrorDataResult<User>(Messages.UserOrPasswordNotFound);

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
                return new ErrorDataResult<User>(Messages.UserOrPasswordNotFound);

            string refreshToken = GenerateToken(64);
            User user = userToCheck.Data;
            user.RefreshToken = refreshToken;
            _userService.Update(user);
            return new SuccessDataResult<User>(user);
        }

        [ValidationAspect(typeof(EmailValidator))]
        public async Task<IResult> UserExists(string email)
        {
            IDataResult<User> UserData = await _userService.GetByMail(email);
            if (UserData.Data != null)
                return new ErrorResult(Messages.UserExists);
            
            return new SuccessResult();
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            var claims = await _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public string GenerateUrlSafeToken(int length)
        {
            string token = GenerateToken(length);

            string safeBase64Token = token
                                        .TrimEnd('=') 
                                        .Replace('+', '-') 
                                        .Replace('/', '_'); 
            return safeBase64Token;
        }

        public string GenerateToken(int length)
        {
            byte[] randomBytes = new byte[length];
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
