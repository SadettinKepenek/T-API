using FluentValidation;
using T_API.Core.DTO.DatabasePackage;

namespace T_API.BLL.Validators.DatabasePackageValidator
{
    public class UpdateDatabasePackageValidator:AbstractValidator<UpdateDatabasePackageDto>
    {
        public UpdateDatabasePackageValidator()
        {
            RuleFor(x => x.PackageId).NotNull().NotEmpty();
            RuleFor(x => x.ApiRequestCount).NotNull().NotEmpty();
            RuleFor(x => x.IsApiSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsAuthSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsJobSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsStorageSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsStoredProcedureSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsTriggerSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsUserDefinedFunctionSupport).NotNull().NotEmpty();
            RuleFor(x => x.IsViewSupport).NotNull().NotEmpty();
            RuleFor(x => x.MaxColumnPerTable).NotNull().NotEmpty();
            RuleFor(x => x.MaxJobCount).NotNull().NotEmpty();
            RuleFor(x => x.MaxStoredProcedureCount).NotNull().NotEmpty();
            RuleFor(x => x.MaxTableCount).NotNull().NotEmpty();
            RuleFor(x => x.MaxTriggerCount).NotNull().NotEmpty();
            RuleFor(x => x.MaxUserDefinedFunctionCount).NotNull().NotEmpty();
            RuleFor(x => x.MaxViewCount).NotNull().NotEmpty();
            RuleFor(x => x.PackageName).NotNull().NotEmpty();
        }
    }
}