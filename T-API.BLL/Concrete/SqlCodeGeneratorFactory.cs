using System.Reflection;
using T_API.BLL.Abstract;

namespace T_API.BLL.Concrete
{
    public class SqlCodeGeneratorFactory
    {
        private static MySqlCodeGenerator MySqlCodeGenerator { get; } = new MySqlCodeGenerator();

        public static ISqlCodeGenerator CreateGenerator(string provider)
        {
            if (provider.Equals("MySql"))
            {
                return MySqlCodeGenerator;
            }
            else
            {
                throw new AmbiguousMatchException("Provider tipi için destek bulunamadı");
            }
        }
    }
}