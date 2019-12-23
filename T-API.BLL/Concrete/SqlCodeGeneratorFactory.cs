using System.Reflection;
using T_API.BLL.Abstract;

namespace T_API.BLL.Concrete
{
    public class SqlCodeGeneratorFactory 
    {
        private static MySqlCodeGenerator mySqlCodeGenerator;

        public static MySqlCodeGenerator MySqlCodeGenerator
        {
            get
            {
                if (mySqlCodeGenerator == null)
                    return new MySqlCodeGenerator();
                return mySqlCodeGenerator;
            }
            set { mySqlCodeGenerator = value; }
        }

        private static SqlServerCodeGenerator sqlServerCodeGenerator;

        public static SqlServerCodeGenerator SqlServerCodeGenerator
        {
            get
            {
                if (sqlServerCodeGenerator == null)
                    return new SqlServerCodeGenerator();
                return sqlServerCodeGenerator;

            }
            set { sqlServerCodeGenerator = value; }
        }


        public static ISqlCodeGenerator CreateGenerator(string provider)
        {
            if (provider.Equals("MySql"))
            {
                return MySqlCodeGenerator;
            }
            else if (provider.Equals("Mssql"))
            {
                return SqlServerCodeGenerator;
            }
            else
            {
                throw new AmbiguousMatchException("Provider tipi için destek bulunamadı");
            }
        }
    }
}