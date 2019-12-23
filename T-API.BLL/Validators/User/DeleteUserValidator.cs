using FluentValidation;
using T_API.Core.DTO.User;

namespace T_API.BLL.Validators.User
{
    public class DeleteUserValidator:AbstractValidator<DeleteUserDto>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}