using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace T_API.BLL.Abstract
{
    public interface ICacheService
    {
        Task RemoveCache(int userId);
    }
}
