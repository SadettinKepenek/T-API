namespace T_API.Core.Exception
{
    public class DatabaseException:System.Exception
    {
        public DatabaseException(string message,System.Exception innerException=null) : base(message:message,innerException:innerException)
        {
            
        }   
    }
}