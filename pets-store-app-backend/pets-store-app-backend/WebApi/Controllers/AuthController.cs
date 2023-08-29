using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Constants;
using IResult = Core.Utilities.Results.IResult;

namespace TestProjectBackendWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;

        private CookieOptions refreshTokenCookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(30),
            HttpOnly = true,
            Secure=true,
            SameSite= SameSiteMode.None,
            IsEssential=true
        };

        private CookieOptions accessTokenCookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
            HttpOnly= true,
            Secure=true,
            SameSite=SameSiteMode.None,
            IsEssential=true          
        };

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
                return BadRequest(userToLogin);
            
            var accessToken = await _authService.CreateAccessToken(userToLogin.Data);

            Response.Cookies.Append("AccessToken", accessToken.Data.Token, accessTokenCookieOptions);
            Response.Cookies.Append("RefreshToken", userToLogin.Data.RefreshToken, refreshTokenCookieOptions); 

            return Ok(new UserDto {Email=userToLogin.Data.Email,FirstName=userToLogin.Data.FirstName,LastName=userToLogin.Data.LastName }); 
        }

        [HttpGet("login-by-cookies")]
        public async Task<ActionResult> Login() {
            Claim? nameIdentifier = Request.HttpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier != null)
            {
                int id = int.Parse(nameIdentifier.Value);
                IDataResult<User> userData = await _userService.GetById(id);
                if (!userData.Success)
                {
                    Response.Cookies.Delete("RefreshToken");
                    Response.Cookies.Delete("AccessToken");
                }
                else
                    return Ok(new UserDto { Email = userData.Data.Email, FirstName = userData.Data.FirstName, LastName = userData.Data.LastName });
            }

            return BadRequest(new ErrorResult( Messages.LoginWithCookiesError));
        }

        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            Request.Cookies.TryGetValue("RefreshToken",out string refreshToken);
            Request.Cookies.TryGetValue("AccessToken", out string accessToken);

            if (refreshToken == null && accessToken == null)
                return NoContent();

            Response.Cookies.Delete("AccessToken");

            IDataResult<User> userData =await _userService.GetByRefreshToken(refreshToken);
            if (!userData.Success)
                return NoContent();

            Response.Cookies.Delete("RefreshToken");
            User user = userData.Data;
            user.RefreshToken = null;
            await _userService.Update(user);
            return Ok(new SuccessResult(Messages.SuccessfullLogout));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = await _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
                return Ok(userExists);
            
            var registerResult =await _authService.Register(userForRegisterDto);
            return Ok(registerResult);        
        }

        [HttpPost("confirm-registeration")]
        public async Task<ActionResult> ConfirmRegistration([FromQuery]string token)
        {
            IDataResult<User> user=await _authService.ConfirmRegistration(token);
            if(!user.Success)
                return BadRequest(user);

            var accessToken = await _authService.CreateAccessToken(user.Data);

            Response.Cookies.Append("AccessToken", accessToken.Data.Token, accessTokenCookieOptions);
            Response.Cookies.Append("RefreshToken", user.Data.RefreshToken, refreshTokenCookieOptions);

            return Ok(new SuccessResult(user.Message));
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword([FromQuery]string email) {

            IResult result = await _authService.ForgetPassword(email);
            if(!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("forget-password-handler")]
        public async Task<ActionResult> ForgetPasswordHandler(ForgetPasswordDto forgetPasswordDto)
        {
            IResult result= await _authService.ForgetPasswordHandler(forgetPasswordDto);
            if(!result.Success)
                return BadRequest(result);

            return Ok(result);  
        }
    }
}