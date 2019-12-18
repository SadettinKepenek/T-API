using FluentValidation;
using T_API.Core.DTO.User;

namespace T_API.BLL.Validators.User
{
    public class AddUserValidator:AbstractValidator<AddUserDto>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Firstname).NotNull().NotEmpty();
            RuleFor(x => x.Lastname).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}