﻿using System.Data;
using FluentValidation;
using T_API.BLL.Validators.Table;
using T_API.Core.DTO.Database;

namespace T_API.BLL.Validators.Database
{
    public class AddDatabaseValidator:AbstractValidator<AddDatabaseDto>
    {
        public AddDatabaseValidator()
        {
            RuleFor(x => x.DatabaseName).NotNull().NotEmpty();
            RuleFor(x => x.EndDate).NotNull().NotEmpty();
            RuleFor(x => x.StartDate).NotNull().NotEmpty();
            RuleFor(x => x.Provider).NotNull().NotEmpty();
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.PackageId).NotNull().NotEmpty();
        }
    }
}