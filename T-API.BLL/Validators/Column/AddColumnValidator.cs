using FluentValidation;
using T_API.Core.DTO.Column;

namespace T_API.BLL.Validators.Column
{
    public class AddColumnValidator:AbstractValidator<AddColumnDto>
    {
        public AddColumnValidator()
        {
            RuleFor(x => x.ColumnName).NotEmpty().NotNull();
            RuleFor(x => x.DataType).NotEmpty().NotNull();
            RuleFor(x => x.TableName).NotEmpty().NotNull();
        }
    }
}