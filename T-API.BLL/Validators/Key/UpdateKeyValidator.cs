using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using T_API.Core.DTO.Key;

namespace T_API.BLL.Validators.Key
{
    public class UpdateKeyValidator:AbstractValidator<UpdateKeyDto>
    {
        public UpdateKeyValidator()
        {
            RuleFor(x => x.KeyName).NotNull().NotEmpty();
            RuleFor(x => x.KeyColumn).NotNull().NotEmpty();
            RuleFor(x => x.TableName).NotNull().NotEmpty();
        }
    }
}
