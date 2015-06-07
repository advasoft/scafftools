/* Copyright (C) Advasoft Development 2015
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights 
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included 
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
 * AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Written by Denis Kozlov <deniskozlov@outlook.com>, June, 5 2015
 */


using makedomain.Code;
using Newtonsoft.Json;
using scafftools.makedomain.Utilities;
using scafftools.Model;
using scafftools.Utilities;
using System;
using System.IO;

namespace makedomain
{
    public class Program
    {
        static void Main(string[] args)
        {
            OptionSet options = default(OptionSet);

            try
            {
                options = ArgumentsParser.ParseArguments<OptionSet>(args);
            }
            catch (ConsoleArgumentsParseException cexception)
            {
                Console.WriteLine(cexception.Message);
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong");
                return;
            }


            if(string.IsNullOrEmpty(options.ConnectionString) && string.IsNullOrEmpty(options.ModelPath))
            {
                Console.WriteLine("Model path or server type and connection string must be passed");
                return;
            }

            Db model = default(Db);

            if (!string.IsNullOrEmpty(options.ModelPath))
            {
                if(!File.Exists(options.ModelPath))
                {
                    Console.WriteLine("Model file '" + options.ModelPath + "' not exist");
                    return;
                }

                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamReader sm = File.OpenText(options.ModelPath))
                    {
                        using (JsonTextReader reader = new JsonTextReader(sm))
                        {
                            model = serializer.Deserialize<Db>(reader);
                        }
                    }


                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
            {
                //TODO: implement later...
            }

            var basePath = options.OutputPath;
            ICodeGenerator generator = CodeGeneratorFactory.GetCodeGenerator(options.LanguageType);
            if(generator != null)
            {
                try
                {
                    foreach (var table in model.Tables)
                    {
                        string filePath = string.Empty;

                        var tableNameStings = table.Name.Split('.');
                        if (tableNameStings.Length > 1)
                        {
                            string directory = basePath;
                            for (int i = 0; i < tableNameStings.Length - 1; i++)
                            {
                                directory = Path.Combine(directory, tableNameStings[i]);
                            }
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fileName = tableNameStings[tableNameStings.Length - 1];
                            if (fileName.ToLower().EndsWith("s"))
                                fileName = fileName.Remove(fileName.Length - 1);
                            filePath = Path.Combine(directory, Path.ChangeExtension(fileName, ("." + generator.GetExtension())));
                        }
                        else
                        {
                            var fileName = table.Name;
                            if (fileName.ToLower().EndsWith("s"))
                                fileName = fileName.Remove(fileName.Length - 1);

                            filePath = Path.Combine(basePath, Path.ChangeExtension(fileName, ("." + generator.GetExtension())));
                        }

                        string rootNamespace = model.Name + ".domains";

                        var codeString = generator.GenerateClass(table, rootNamespace, model);

                        File.WriteAllText(filePath, codeString);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                throw new ApplicationException("Unknown language generator");
            }

        }
    }
}
