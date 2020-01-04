using FluentValidation;
using T_API.Core.DTO.DatabasePackage;

namespace T_API.BLL.Validators.DatabasePackageValidator
{
    public class DeleteDatabasePackageValidator:AbstractValidator<DeleteDatabasePackageDto>
    {
        public DeleteDatabasePackageValidator()
        {
            RuleFor(x => x.PackageId).NotNull().NotEmpty();
        }
    }
}