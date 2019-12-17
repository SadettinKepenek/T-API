namespace T_API.Core.Exception
{
    public class ValidationException:System.Exception
    {
        public ValidationException(string errorMessage):base(message:errorMessage.Replace('~','\n'))
        {
        }

    }
}