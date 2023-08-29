using Core.Entities;

namespace Entities.Dto
{
    public class ForgetPasswordDto:IDto
    {
        public string ResetToken { get; set; }
        public string Password { get; set; }
    }
}
