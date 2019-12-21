using FluentValidation;
using T_API.Core.DTO.Database;

namespace T_API.BLL.Validators.Database
{
    public class DeleteDatabaseValidator:AbstractValidator<DeleteDatabaseDto>
    {
        public DeleteDatabaseValidator()
        {
            RuleFor(x => x.DatabaseId).NotNull().NotEmpty();

        }
    }
}