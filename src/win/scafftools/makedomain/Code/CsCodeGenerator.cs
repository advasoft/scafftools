﻿
using System;
using scafftools.Model;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using makedomain.Utilities;

namespace makedomain.Code
{
    public class CsCodeGenerator : ICodeGenerator
    {
        public string GenerateClass(Table table, string rootNamespace, Db model, string safedCode = "")
        {
            StringBuilder builder = new StringBuilder();
            string @namespace = rootNamespace;

            StringCollection addedNameSpaces = new StringCollection();
            addedNameSpaces.Add(NamingUtility.GetFullNamespace(rootNamespace, table.Name));

            // @@@ scafftools makedomain generated @@@
            // @@@ at 01.06.2015 00:43:59 @@@

            builder.AppendLine("// @@@ scafftools makedomain generated @@@");
            builder.AppendFormat("// @@@ at {0} @@@\r\n", DateTime.Now);

            //set using statements
            builder.AppendLine("");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            foreach (var relation in table.Relations)
            {
                var nms = NamingUtility.GetFullNamespace(@namespace, relation.OuterTable.Name);
                if (!addedNameSpaces.Contains(nms))
                {
                    builder.AppendFormat("using {0};\r\n", nms);
                    addedNameSpaces.Add(nms);
                }
            }
            foreach (var outTable in model.Tables.Where(t => t.Name != table.Name).ToList())
            {
                foreach (var relation in outTable.Relations.Where(r => r.OuterTable.Name == table.Name))
                {
                    var nms = NamingUtility.GetFullNamespace(@namespace, outTable.Name);
                    if (!addedNameSpaces.Contains(nms))
                    {
                        builder.AppendFormat("using {0};\r\n", nms);
                        addedNameSpaces.Add(nms);
                    }
                }
            }

            builder.AppendLine("");

            //namespace
            builder.AppendFormat("namespace {0}\r\n", NamingUtility.GetFullNamespace(rootNamespace, table.Name));
            builder.AppendLine("{");

            //class

            var className = NamingUtility.RemoveSFromName(NamingUtility.GetCleanTableName(table.Name));

            builder.AppendFormat("\tpublic class {0}\r\n", className);
            builder.AppendFormat("\t");
            builder.Append("{");
            builder.AppendFormat("\r\n");

            foreach (var field in table.Columns)
            {
                string typeName = GetTypeName(field.Type);
                bool nullable = false;

                if(GetTypeNameType(typeName).IsValueType && field.CanBeNull)
                {
                    nullable = true;
                }

                //property
                builder.AppendLine("\t\t// @@@ scafftools property generated @@@");
                builder.AppendFormat("\t\tpublic {0}{1} {2} ", typeName, nullable == true ? "?" : "", field.Name);
                builder.Append("{ get; set; }");
                builder.AppendFormat("\r\n");
                //end of property
            }

            foreach(var relation in table.Relations)
            {
                var propertyName = NamingUtility.RemoveIdFromName(relation.UniqueColumnKey.Name);

                var cleanTableName = NamingUtility.GetCleanTableName(relation.OuterTable.Name);

                builder.AppendLine("\t\t// @@@ scafftools makedomain generated @@@");
                builder.AppendFormat("\t\tpublic virtual {0} {1} ",
                    NamingUtility.RemoveSFromName(cleanTableName), propertyName);
                builder.Append("{ get; set; }");
                builder.AppendFormat("\r\n");

            }

            var outTables = model.Tables.Where(t => t.Name != table.Name).ToList();// && table.Relations.Any(a => a.OuterTable.Name == table.Name)).ToList(); //t.Name != table.Name &&
            foreach (var outTable in outTables)
            {
                foreach(var relation in outTable.Relations.Where(r => r.OuterTable.Name == table.Name))
                {
                    var propertyName = NamingUtility.GetCleanTableName(outTable.Name);

                    builder.AppendLine("\t\t// @@@ scafftools makedomain generated @@@");
                    builder.AppendFormat("\t\tpublic virtual ICollection<{0}> {1} ",
                        NamingUtility.RemoveSFromName(NamingUtility.GetCleanTableName(outTable.Name)), propertyName);
                    builder.Append("{ get; set; }");
                    builder.AppendFormat("\r\n");

                }
            }

            //back safed code
            if(safedCode != string.Empty)
            {
                builder.Append(safedCode);
            }

            //end of class
            builder.AppendFormat("\t");
            builder.Append("}");
            builder.AppendFormat("\r\n");


            //end of namespace
            builder.AppendLine("}");

            return builder.ToString();
        }
        public string GetExtension()
        {
            return "cs";
        }

        public string GetSafedCode(string code)
        {
            string safedCode = string.Empty;
            if (code.Contains("@@@ scafftools safed code begin"))
            {
                int startPosition = code.IndexOf("@@@ scafftools safed code begin") + 31;
                int endPosition = code.IndexOf("// @@@ scafftools safed code end");
                if(endPosition == -1)
                {
                    endPosition = code.IndexOf("//@@@ scafftools safed code end");
                }
                if (endPosition == -1)
                    endPosition = code.Length;
                StringBuilder b = new StringBuilder("\t\t// @@@ scafftools safed code begin\r\n");
                b.Append(code.Substring(startPosition, endPosition - startPosition));
                b.AppendLine("//@@@ scafftools safed code end");
                safedCode = b.ToString();

            }
            return safedCode;
        }


        public string GetTypeName(ColumnTypes type)
        {
            string typeString = "";
            
            switch (type)
            {
                case ColumnTypes.BigInt:
                    typeString = "long";
                    break;
                case ColumnTypes.Binary:
                    typeString = "byte[]";
                    break;
                case ColumnTypes.Bit:
                    typeString = "bool";
                    break;
                case ColumnTypes.Char:
                    typeString = "char";
                    break;
                case ColumnTypes.Date:
                    typeString = "DateTime";
                    break;
                case ColumnTypes.DateTime:
                    typeString = "DateTime";
                    break;
                case ColumnTypes.DateTime2:
                    typeString = "DateTime";
                    break;
                case ColumnTypes.DateTimeOffset:
                    typeString = "TimeSpan";
                    break;
                case ColumnTypes.Decimal:
                    typeString = "decimal";
                    break;
                case ColumnTypes.Float:
                    typeString = "float";
                    break;
                case ColumnTypes.Int:
                    typeString = "int";
                    break;
                case ColumnTypes.Money:
                    typeString = "decimal";
                    break;
                case ColumnTypes.NChar:
                    typeString = "string";
                    break;
                case ColumnTypes.NText:
                    typeString = "string";
                    break;
                case ColumnTypes.Numeric:
                    typeString = "decimal";
                    break;
                case ColumnTypes.NVarChar:
                    typeString = "string";
                    break;
                case ColumnTypes.SmallInt:
                    typeString = "short";
                    break;
                case ColumnTypes.SmallMoney:
                    typeString = "double";
                    break;
                case ColumnTypes.Text:
                    typeString = "string";
                    break;
                case ColumnTypes.Time:
                    typeString = "DateTime";
                    break;
                case ColumnTypes.TimeStamp:
                    typeString = "string";
                    break;
                case ColumnTypes.UniqueIdentifier:
                    typeString = "Guid";
                    break;
                case ColumnTypes.VarBinnary:
                    typeString = "byte[]";
                    break;
                case ColumnTypes.VarChar:
                    typeString = "string";
                    break;
                case ColumnTypes.Variant:
                    typeString = "object";
                    break;
                case ColumnTypes.Xml:
                    typeString = "string";
                    break;
                default:
                    throw new ApplicationException("Unknown type: " + type.ToString());
            }

            return typeString;
        }

        public Type GetTypeNameType(string typeName)
        {
            Type type = default(Type);
            switch(typeName)
            {
                case "long":
                    type = typeof(Int64);
                    break;
                case "byte[]":
                    type = typeof(ArrayList);
                    break;
                case "bool":
                    type = typeof(Boolean);
                    break;
                case "char":
                    type = typeof(Char);
                    break;
                case "DateTime":
                    type = typeof(DateTime);
                    break;
                 case "TimeSpan":
                    type = typeof(TimeSpan);
                    break;
                case "decimal":
                    type = typeof(Decimal);
                    break;
                case "float":
                    type = typeof(Double);
                    break;
                case "int":
                    type = typeof(Int32);
                    break;
                case "string":
                    type = typeof(String);
                    break;
                case "Guid":
                    type = typeof(Guid);
                    break;
                case "object":
                    type = typeof(Object);
                    break;
                default:
                    throw new ApplicationException("Unknown type: " + typeName);
            }

            return type;
        }

    }
}
