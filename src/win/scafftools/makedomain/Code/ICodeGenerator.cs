
using scafftools.Model;

namespace makedomain.Code
{
    public interface ICodeGenerator
    {
        string GenerateClass(Table table, string rootNamespace, Db model, string safedCode = "");
        string GetExtension();
        string GetTypeName(ColumnTypes type);
        string GetSafedCode(string code);
    }
}
