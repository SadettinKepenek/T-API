using FluentValidation;
using T_API.Core.DTO.User;

namespace T_API.BLL.Validators.User
{
    public class LoginUserValidator:AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}