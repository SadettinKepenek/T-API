using System.Threading.Tasks;

namespace T_API.BLL.Abstract
{
    public interface ICacheService
    {
        Task RemoveCache(int userId);
    }
}