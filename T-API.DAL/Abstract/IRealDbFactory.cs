namespace T_API.DAL.Abstract
{
    public interface IRealDbFactory
    {
        IRealDbRepository CreateRepository(string provider);
    }
}