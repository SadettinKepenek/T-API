using FluentValidation;
using T_API.Core.DTO.ForeignKey;

namespace T_API.BLL.Validators.ForeignKey
{
    public class AddForeignKeyValidator:AbstractValidator<AddForeignKeyDto>
    {
        public AddForeignKeyValidator()
        {
            RuleFor(x => x.ForeignKeyName).NotEmpty().NotNull();
            RuleFor(x => x.SourceColumn).NotEmpty().NotNull();
            RuleFor(x => x.SourceTable).NotEmpty().NotNull();
            RuleFor(x => x.TargetColumn).NotEmpty().NotNull();
            RuleFor(x => x.TargetTable).NotEmpty().NotNull();
        }
    }
}