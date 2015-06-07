
using scafftools;

namespace makedomain.Code
{
    public static class CodeGeneratorFactory
    {
        public static ICodeGenerator GetCodeGenerator(LanguageTypeEnum languageType)
        {
            ICodeGenerator generator = default(ICodeGenerator);
            switch (languageType)
            {
                case LanguageTypeEnum.cs:
                    generator = new CsCodeGenerator();
                    break;
            }
            return generator;
        }
    }
}
