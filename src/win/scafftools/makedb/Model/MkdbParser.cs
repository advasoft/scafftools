
namespace scafftools.makedb.Model
{
    using System;
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

                    var columnTypeQualificator = match.Groups["type_qual"].Value;
                    var columnNullable = match.Groups["nullable"].Value;
                    var columnKeyIndex = match.Groups["key_index"].Value;
                    var columnKeyIndexAsc = match.Groups["asc"].Value;
                    var columnKeyIndexDesc = match.Groups["desc"].Value;
                    var columnKeyAuto = match.Groups["auto_key"].Value;
                    var columnKeyAutoQualificator = match.Groups["auto_key_qual"].Value;
                    var columnForeignKey = match.Groups["foreign"].Value;
                    var columnForeignKeyTable = match.Groups["outer_table"].Value;
                    var columnForeignKeyColumn = match.Groups["outer_column"].Value;
                    var columnForeignKeyUpdateAction = match.Groups["update_action"].Value;
                    var columnForeignKeyDeleteAction = match.Groups["delete_action"].Value;

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
