namespace T_API.DAL.Abstract
{
    public interface IRealDbRepositoryFactory
    {
        IRealDbRepository CreateRepository(string provider);
    }
}