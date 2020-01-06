using FluentValidation;
using T_API.Core.DTO.Table;

namespace T_API.BLL.Validators.Table
{
    public class DeleteTableValidator:AbstractValidator<DeleteTableDto>
    {
        public DeleteTableValidator()
        {
            RuleFor(x => x.DatabaseId).NotEmpty().NotNull();
            RuleFor(x => x.DatabaseName).NotEmpty().NotNull();
            RuleFor(x => x.TableName).NotEmpty().NotNull();
        }
    }
}