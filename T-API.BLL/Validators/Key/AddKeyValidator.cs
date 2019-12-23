using FluentValidation;
using T_API.Core.DTO.Key;

namespace T_API.BLL.Validators.Key
{
    public class AddKeyValidator:AbstractValidator<AddKeyDto>
    {
        public AddKeyValidator()
        {
            RuleFor(x => x.KeyName).NotNull().NotEmpty();
            RuleFor(x => x.KeyColumn).NotNull().NotEmpty();
            RuleFor(x => x.TableName).NotNull().NotEmpty();
        }
    }
}