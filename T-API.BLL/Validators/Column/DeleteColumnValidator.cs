using FluentValidation;
using T_API.Core.DTO.Column;

namespace T_API.BLL.Validators.Column
{
    public class DeleteColumnValidator:AbstractValidator<DeleteColumnDto>
    {
        public DeleteColumnValidator()
        {
            RuleFor(x => x.ColumnName).NotEmpty().NotNull();
            RuleFor(x => x.DatabaseId).NotEmpty().NotNull();
            RuleFor(x => x.Provider).NotEmpty().NotNull();
            RuleFor(x => x.TableName).NotEmpty().NotNull();
        }
    }
}