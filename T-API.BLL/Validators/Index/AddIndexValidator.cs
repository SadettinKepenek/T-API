using FluentValidation;
using T_API.Core.DTO.Index;

namespace T_API.BLL.Validators.Index
{
    public class AddIndexValidator:AbstractValidator<AddIndexDto>
    {
        public AddIndexValidator()
        {
            RuleFor(x => x.IndexColumn).NotEmpty().NotNull();
            RuleFor(x => x.IndexName).NotEmpty().NotNull();
            RuleFor(x => x.IsUnique).NotEmpty().NotNull();
            RuleFor(x => x.TableName).NotEmpty().NotNull();
        }
    }
}