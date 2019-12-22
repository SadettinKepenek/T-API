namespace T_API.BLL.Abstract
{
    public interface ISqlCodeGeneratorFactory
    {
        ISqlCodeGenerator CreateGenerator(string provider);
    }
}