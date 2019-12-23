using FluentValidation;
using T_API.BLL.Validators.Column;
using T_API.BLL.Validators.ForeignKey;
using T_API.BLL.Validators.Index;
using T_API.BLL.Validators.Key;
using T_API.Core.DTO.Table;

namespace T_API.BLL.Validators.Table
{
    public class AddTableValidator:AbstractValidator<AddTableDto>
    {
        public AddTableValidator()
        {
            RuleFor(x => x.TableName).NotEmpty().NotNull();
            RuleFor(x => x.Columns).NotNull().NotEmpty();
            RuleForEach(x => x.Columns).SetValidator(new AddColumnValidator()).When(x => x.Columns != null);
            RuleForEach(x => x.Indices).SetValidator(new AddIndexValidator()).When(x => x.Indices != null);
            RuleForEach(x => x.ForeignKeys).SetValidator(new AddForeignKeyValidator()).When(x => x.ForeignKeys != null);
            RuleForEach(x => x.Keys).SetValidator(new AddKeyValidator()).When(x => x.Keys != null);
        }
    }
}