
using System;
using scafftools.Model;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;

namespace makedomain.Code
{
    public class CsCodeGenerator : ICodeGenerator
    {
        public string GenerateClass(Table table, string rootNamespace, Db model)
        {
            StringBuilder builder = new StringBuilder();
            string @namespace = rootNamespace;

            StringCollection addedNameSpaces = new StringCollection();
            addedNameSpaces.Add(GetFullNamespace(rootNamespace, table.Name));

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
                var nms = GetFullNamespace(@namespace, relation.OuterTable.Name);
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
                    var nms = GetFullNamespace(@namespace, outTable.Name);
                    if (!addedNameSpaces.Contains(nms))
                    {
                        builder.AppendFormat("using {0};\r\n", nms);
                        addedNameSpaces.Add(nms);
                    }
                }
            }

            builder.AppendLine("");

            //namespace
            builder.AppendFormat("namespace {0}\r\n", GetFullNamespace(rootNamespace, table.Name));
            builder.AppendLine("{");

            //class

            var className = GetCleanTableName(table.Name);
            if(className.ToLower().EndsWith("s"))
            {
                className = className.Remove(className.Length - 1);
            }

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
                builder.AppendFormat("\t\tpublic {0}{1} {2} ", typeName, nullable == true ? "?" : "", field.Name);
                builder.Append("{ get; set; }");
                builder.AppendFormat("\r\n");
                //end of property
            }

            foreach(var relation in table.Relations)
            {
                var propertyName = relation.UniqueColumnKey.Name;
                if(propertyName.ToLower().EndsWith("id"))
                {
                    propertyName = propertyName.Remove(propertyName.Length - 2);
                }
                else if(propertyName.ToLower().EndsWith("_id"))
                {
                    propertyName = propertyName.Remove(propertyName.Length - 3);
                }

                builder.AppendFormat("\t\tpublic virtual {0} {1} ", 
                    GetCleanTableName(relation.OuterTable.Name), propertyName);
                builder.Append("{ get; set; }");
                builder.AppendFormat("\r\n");

            }

            var outTables = model.Tables.Where(t => t.Name != table.Name).ToList();// && table.Relations.Any(a => a.OuterTable.Name == table.Name)).ToList(); //t.Name != table.Name &&
            foreach (var outTable in outTables)
            {
                foreach(var relation in outTable.Relations.Where(r => r.OuterTable.Name == table.Name))
                {
                    var propertyName = relation.UniqueColumnKey.Name;
                    if (propertyName.ToLower().EndsWith("id"))
                    {
                        propertyName = propertyName.Remove(propertyName.Length - 2);
                    }
                    else if (propertyName.ToLower().EndsWith("_id"))
                    {
                        propertyName = propertyName.Remove(propertyName.Length - 3);
                    }

                    builder.AppendFormat("\t\tpublic virtual ICollection<{0}> {1} ",
                        GetCleanTableName(outTable.Name), propertyName + "s");
                    builder.Append("{ get; set; }");
                    builder.AppendFormat("\r\n");

                }
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

        private string GetFullNamespace(string rootNamespace, string tableName)
        {
            string @namespace = rootNamespace;
            var nameArray = tableName.Split('.');
            if (nameArray.Length > 1)
                return rootNamespace;

            for(int i = 0; i < nameArray.Length - 1; i++)
            {
                @namespace = string.Concat(@namespace, ".", nameArray[i]);
            }

            return @namespace;
        }

        private string GetCleanTableName(string tableName)
        {
            var nameArray = tableName.Split('.');
            if (nameArray.Length > 1)
                return nameArray[nameArray.Length - 1];

            return tableName;
        }
    }
}
