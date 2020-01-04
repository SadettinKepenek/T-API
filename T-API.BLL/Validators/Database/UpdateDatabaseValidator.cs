using FluentValidation;
using T_API.Core.DTO.Database;

namespace T_API.BLL.Validators.Database
{
    public class UpdateDatabaseValidator:AbstractValidator<UpdateDatabaseDto>
    {
        public UpdateDatabaseValidator()
        {
            RuleFor(x => x.DatabaseId).NotNull().NotEmpty();
            RuleFor(x => x.DatabaseName).NotNull().NotEmpty();
            RuleFor(x => x.EndDate).NotNull().NotEmpty();
            RuleFor(x => x.StartDate).NotNull().NotEmpty();
            RuleFor(x => x.IsActive).NotNull().NotEmpty();
            RuleFor(x => x.PackageId).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.Port).NotNull().NotEmpty();
            RuleFor(x => x.Provider).NotNull().NotEmpty();
            RuleFor(x => x.Server).NotNull().NotEmpty();
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Username).NotNull().NotEmpty();
        }
    }
}