
namespace scafftools.makedb.Model
{
    using scafftools.makedb.Utilities;
    using scafftools.Model;
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class MkdbParser
    {
        public static Db Parse(string fileContent)
        {
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline;

            Regex dbRgx = new Regex("^\\s*db\\s+(?<db>[\\w\\/]+)?", options);
            Regex tableRgx = new Regex("^\\s*t\\s+(?<table>[\\w\\/]+)?", options);
            Regex commandRgx = new Regex("^\\s*(?<name>\\w+)\\s+(?<type>bi|bn|b|c|i|dtmo|dtm|dt|d|nvc|tx|ntx|n|f|m|nc|si|sm|v|t|ts|u|vb|vc|x)(?<type_qual>[\\d\\.]+)?(?<nullable>\\s+(z))?(\\s+(?<asc><)?(?<key_index>k|i)(?<desc>>)?(\\s+(?<auto_key>a)(?<auto_key_qual>[\\d\\.]+))?)?(\\s+(?<foreign>f)\\s+(?<outer_table>[\\w\\/]+)(>)(?<outer_column>\\w+)(\\s+(?<update_action>uc|un|ud)?\\s+(?<delete_action>dc|dn|dd)?)?)?", options);

            Db lastDb = null;
            Table lastTable = null;

            string[] mkdbLines = fileContent.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

			var length = mkdbLines.Length;
			for (int i = 0; i < length; i++)
			{
			    var mkdbLine = mkdbLines[i];
			    if (dbRgx.IsMatch(mkdbLine)) //IS DB
			    {
			        var match = dbRgx.Match(mkdbLine);
			        var dbName = match.Groups["db"].Value;
                    if(string.IsNullOrEmpty(dbName))
                        throw new InvalidMkdbParseException(string.Format("Invalid database definition. Database must contains Name")) { Line = i };

                    lastDb = new Db()
                    {
                        Name = dbName
                    };

			    }
                else if (tableRgx.IsMatch(mkdbLine)) //IS TABLE
                {
                    var match = tableRgx.Match(mkdbLine);
                    var tableName = match.Groups["table"].Value;
                    if (string.IsNullOrEmpty(tableName))
                        throw new InvalidMkdbParseException(string.Format("Invalid table definition. Table must contains Name")) { Line = i };

                    lastTable = new Table()
                    {
                        Name = GetSuccessTableName(tableName)
                    };
                    lastDb.Tables.Add(lastTable);
                }
                else if (commandRgx.IsMatch(mkdbLine)) // IS COLUMN
                {
                    var match = commandRgx.Match(mkdbLine);
                    var columnName = match.Groups["name"].Value;
                    var columnType = match.Groups["type"].Value;

                    if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(columnType))
                        throw new InvalidMkdbParseException(string.Format("Invalid column definition. Column must contains Name and Type")) {Line = i};

                    var columnTypeQualificator = match.Groups["type_qual"].Value.Trim();
                    var columnNullable = match.Groups["nullable"].Value.Trim();
                    var columnKeyIndex = match.Groups["key_index"].Value.Trim();
                    var columnKeyIndexAsc = match.Groups["asc"].Value.Trim();
                    var columnKeyIndexDesc = match.Groups["desc"].Value.Trim();
                    var columnKeyAuto = match.Groups["auto_key"].Value.Trim();
                    var columnKeyAutoQualificator = match.Groups["auto_key_qual"].Value.Trim();
                    var columnForeignKey = match.Groups["foreign"].Value.Trim();
                    var columnForeignKeyTable = match.Groups["outer_table"].Value.Trim();
                    var columnForeignKeyColumn = match.Groups["outer_column"].Value.Trim();
                    var columnForeignKeyUpdateAction = match.Groups["update_action"].Value.Trim();
                    var columnForeignKeyDeleteAction = match.Groups["delete_action"].Value.Trim();

                    var column = new Column();
                    column.CanBeNull = columnNullable == "z";
                    column.Name = columnName;
                    column.Type = ColumnTypesHelper.GetColumnTypeFromString(columnType);

                    if (!string.IsNullOrEmpty(columnTypeQualificator))
                    {
                        if (columnTypeQualificator.Contains("."))
                        {
                            var qp = columnTypeQualificator.Split('.');
                            column.IntegerPart = int.Parse(qp[0]);
                            column.FactorialPart = int.Parse(qp[1]);
                        }
                        else
                        {
                            column.IntegerPart = int.Parse(columnTypeQualificator);
                            column.Length = int.Parse(columnTypeQualificator);
                        }
                    }
                    lastTable.Columns.Add(column);

                    if (columnKeyIndex == "k")
                    {
                        var key = new Key();
                        key.UniqueColumn = column;
                        key.Sorting = SortDirectory.Asc;

                        if (columnKeyIndexDesc == ">")
                        {
                            key.Sorting = SortDirectory.Desc;
                        }

                        if(columnKeyAuto == "a")
                        {
                            key.AutoGenerated = true;
                            key.StartIncrement = 0;
                            key.IncrementStep = 1;
                        }

                        if(!string.IsNullOrEmpty(columnKeyAutoQualificator))
                        {
                            if (!columnKeyAutoQualificator.Contains(".")) throw new ApplicationException("Autogenerate key step is not defined like '1.1'");

                            var qp = columnKeyAutoQualificator.Split('.');
                            key.StartIncrement = int.Parse(qp[0]);
                            key.IncrementStep = int.Parse(qp[1]);
                        }

                        lastTable.Keys.Add(key);
                    }

                    if (columnKeyIndex == "i")
                    {
                        var index = new Index();
                        index.UniqueColumn = column;
                        index.Sorting = SortDirectory.Asc;

                        if (columnKeyIndexDesc == ">")
                        {
                            index.Sorting = SortDirectory.Desc;
                        }

                        lastTable.Indexes.Add(index);
                    }

                    if (columnForeignKey == "f")
                    {
                        var relation = new Relation();
                        relation.UniqueColumnKey = column;

                        var outerTableName = GetSuccessTableName(columnForeignKeyTable);
                        var outerTable = lastDb.Tables.Where(t => t.Name == outerTableName).FirstOrDefault();
                        relation.OuterTable = outerTable;
                        relation.OuterColumnKey = outerTable.Columns.Where(c => c.Name == columnForeignKeyColumn).FirstOrDefault();

                        if(!string.IsNullOrEmpty(columnForeignKeyUpdateAction))
                        {
                            switch (columnForeignKeyUpdateAction)
                            {
                                case "u":
                                    relation.ActionOnUpdate = UpdateDeleteAction.NoAction;
                                    break;
                                case "uc":
                                    relation.ActionOnUpdate = UpdateDeleteAction.Cascade;
                                    break;
                                case "un":
                                    relation.ActionOnUpdate = UpdateDeleteAction.SetNull;
                                    break;
                                case "ud":
                                    relation.ActionOnUpdate = UpdateDeleteAction.SetDefault;
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(columnForeignKeyDeleteAction))
                        {
                            switch (columnForeignKeyDeleteAction)
                            {
                                case "d":
                                    relation.ActionOnDelete = UpdateDeleteAction.NoAction;
                                    break;
                                case "dc":
                                    relation.ActionOnDelete = UpdateDeleteAction.Cascade;
                                    break;
                                case "dn":
                                    relation.ActionOnDelete = UpdateDeleteAction.SetNull;
                                    break;
                                case "dd":
                                    relation.ActionOnDelete = UpdateDeleteAction.SetDefault;
                                    break;
                            }
                        }

                        lastTable.Relations.Add(relation);

                    }


                }
                else
                {
                    throw new InvalidMkdbFileStructureException() { Line = i };
                }
			}

            return lastDb;
        }

        private static string GetSuccessTableName(string sourceName)
        {
            return sourceName.Replace("/", ".");
        }
    }
}
