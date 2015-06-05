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
 * Written by Denis Kozlov <deniskozlov@outlook.com>, April, 26 2015
 */


namespace scafftools.makedb
{
    using Scheme;
    using System;
    using Model;
    using Utilities;
    using System.IO;
    using Newtonsoft.Json;
    using scafftools.Model;
    using scafftools.Utilities;

    class Program
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

            string mkdbContent = "";
            try
            {
                mkdbContent = new MkdbFileReader(options.SourcePath).ReadFileContent();
            }
            catch (ApplicationException ae)
            {
                Console.WriteLine(ae.Message);
                return;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong");
                return;
            }

            Db model;
		    try
		    {
                model = MkdbParser.Parse(mkdbContent);
            }
            catch (InvalidMkdbFileStructureException e)
		    {
                Console.WriteLine("Invalid file structure at line {0}", e.Line);
                return;
            }

		    ISchemeCreator creator = SchemeCreatorFactory.CreateSchemeCreator(options.ServerType);
		    var scheme = creator.GenerateScript(model);

            //try save file
            try
            {
                if (options.OutputPath == string.Empty)
                {
                    options.OutputPath = Environment.CurrentDirectory;
                }

                var fullpath = Path.Combine(options.OutputPath, Path.ChangeExtension(model.Name, ".sql"));
                var jsonFullPath = Path.Combine(options.OutputPath, Path.ChangeExtension(model.Name, ".json"));
                if (!Directory.Exists(options.OutputPath))
                {
                    Directory.CreateDirectory(options.OutputPath);
                }

                if (options.Force && File.Exists(fullpath))
                {
                    File.Delete(fullpath);
                    File.WriteAllText(fullpath, scheme);

                }
                else if((!options.Force) && File.Exists(fullpath))
                {}
                else
                {
                    File.WriteAllText(fullpath, scheme);
                }

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(File.CreateText(jsonFullPath), model, typeof(Db));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

		}
	}
}
