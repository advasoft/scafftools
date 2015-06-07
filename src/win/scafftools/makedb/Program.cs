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
    using scafftools.Model;
    using scafftools.Utilities;
    using System.Runtime.Serialization.Json;
    using System.Xml;

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

            #region header
            Console.WriteLine("*********************************************************");
            Console.WriteLine("*\t\t\t\t\t\t\t*");
            Console.WriteLine("*\t\t\tMAKEDB  \t\t\t*");
            Console.WriteLine("*\t\tGENERATE DB SCHEMA APPLICATION \t\t*");
            Console.WriteLine("*\t\t\t\t\t\t\t*");
            Console.WriteLine("*\tAUTHOR DENIS KOZLOV (DENISKOZLOV@OUTLOOK.COM) \t*");
            Console.WriteLine("*\t\t\t 2015\t\t\t\t*");
            Console.WriteLine("*\t\t\t\t\t\t\t*");
            Console.WriteLine("*********************************************************");
            Console.WriteLine("");
            #endregion

            Console.Write("Read mkdb file....");
            string mkdbContent = "";
            try
            {
                mkdbContent = new MkdbFileReader(options.SourcePath).ReadFileContent();
                Console.Write("done\r\n");
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

            Console.Write("Create mkdb model....");
            Db model;
		    try
		    {
                model = MkdbParser.Parse(mkdbContent);
                Console.Write("done\r\n");
            }
            catch (InvalidMkdbFileStructureException e)
		    {
                Console.WriteLine("Invalid file structure at line {0}", e.Line);
                return;
            }

            string scheme = string.Empty;

            Console.Write("Create MS SQL Server database scheme....");
            ISchemeCreator creator = default(ISchemeCreator);
            try
            {
                creator = SchemeCreatorFactory.CreateSchemeCreator(options.ServerType);
                scheme = creator.GenerateScript(model);
                Console.Write("done\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //try save file
            XmlDictionaryWriter writer = default(XmlDictionaryWriter);
            FileStream fs = default(FileStream);
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
                    Console.WriteLine("Saved sql script to '{0}'", fullpath);
                }
                else if ((!options.Force) && File.Exists(fullpath))
                { }
                else
                {
                    File.WriteAllText(fullpath, scheme);
                    Console.WriteLine("Saved sql script to '{0}'", fullpath);
                }


                fs = new FileStream(jsonFullPath,
                    FileMode.Create);
                writer = JsonReaderWriterFactory.CreateJsonWriter(fs);
                DataContractJsonSerializer serializator = new DataContractJsonSerializer(typeof(Db));
                serializator.WriteObject(writer, model);
                Console.WriteLine("Saved mkdb model to '{0}'", jsonFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(writer != null)
                    writer.Close();
                if(fs != null)
                    fs.Close();

            }
            if(options.GenerateDb && !string.IsNullOrEmpty(options.ConnectionString) && !string.IsNullOrEmpty(scheme) && creator != null)
            {
                Console.Write("Generating database '{0}'....", model.Name);
                try
                {
                    creator.GenerateDatabase(options.ConnectionString, scheme);
                    Console.Write("done\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

            }

            Console.WriteLine("Completed");
            Console.ReadKey();
        }
	}
}
