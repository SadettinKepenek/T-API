using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using T_API.Core.DTO.Key;

namespace T_API.BLL.Validators.Key
{
    public class DeleteKeyValidator : AbstractValidator<DeleteKeyDto>
    {
        public DeleteKeyValidator()
        {
            RuleFor(x => x.KeyName).NotNull().NotEmpty();
            RuleFor(x => x.Provider).NotNull().NotEmpty();
            RuleFor(x => x.TableName).NotNull().NotEmpty();
            RuleFor(x => x.DatabaseId).NotNull().NotEmpty();
        }
    }
}
