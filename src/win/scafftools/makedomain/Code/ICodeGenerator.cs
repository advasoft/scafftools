
using scafftools.Model;

namespace makedomain.Code
{
    public interface ICodeGenerator
    {
        string GenerateClass(Table table, string rootNamespace, Db model);
        string GetExtension();
        string GetTypeName(ColumnTypes type);
    }
}
