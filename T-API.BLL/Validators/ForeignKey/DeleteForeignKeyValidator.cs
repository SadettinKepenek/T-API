using FluentValidation;
using T_API.Core.DTO.ForeignKey;

namespace T_API.BLL.Validators.ForeignKey
{
    public class DeleteForeignKeyValidator:AbstractValidator<DeleteForeignKeyDto>
    {
        public DeleteForeignKeyValidator()
        {
            RuleFor(x => x.ForeignKeyName).NotEmpty().NotNull();
            RuleFor(x => x.SourceTable).NotEmpty().NotNull();
            RuleFor(x => x.DatabaseId).NotEmpty().NotNull();
           
        }   
    }
}