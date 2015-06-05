
using scafftools.Model;
using System;

namespace scafftools.makedb.Utilities
{
    public static class ColumnTypesHelper
    {
        public static ColumnTypes GetColumnTypeFromString(string columnTypeString)
        {
            ColumnTypes result = ColumnTypes.NVarChar;

            switch (columnTypeString)
            {
                case "i":
                    result = ColumnTypes.Int;
                    break;
                case "n":
                    result = ColumnTypes.Numeric;
                    break;
                case "bi":
                    result = ColumnTypes.BigInt;
                    break;
                case "b":
                    result = ColumnTypes.Bit;
                    break;
                case "bn":
                    result = ColumnTypes.Binary;
                    break;
                case "c":
                    result = ColumnTypes.Char;
                    break;
                case "dt":
                    result = ColumnTypes.Date;
                    break;
                case "dtm":
                    result = ColumnTypes.DateTime;
                    break;
                case "dtm2":
                    result = ColumnTypes.DateTime2;
                    break;
                case "dtmo":
                    result = ColumnTypes.DateTimeOffset;
                    break;
                case "tx":
                    result = ColumnTypes.Text;
                    break;
                case "ntx":
                    result = ColumnTypes.NText;
                    break;
                case "d":
                    result = ColumnTypes.Decimal;
                    break;
                case "nvc":
                    result = ColumnTypes.NVarChar;
                    break;
                case "f":
                    result = ColumnTypes.Float;
                    break;
                case "m":
                    result = ColumnTypes.Money;
                    break;
                case "nc":
                    result = ColumnTypes.NChar;
                    break;
                case "si":
                    result = ColumnTypes.SmallInt;
                    break;
                case "sm":
                    result = ColumnTypes.SmallMoney;
                    break;
                case "v":
                    result = ColumnTypes.Variant;
                    break;
                case "t":
                    result = ColumnTypes.Time;
                    break;
                case "ts":
                    result = ColumnTypes.TimeStamp;
                    break;
                case "u":
                    result = ColumnTypes.UniqueIdentifier;
                    break;
                case "vb":
                    result = ColumnTypes.VarBinnary;
                    break;
                case "vc":
                    result = ColumnTypes.VarChar;
                    break;
                case "x":
                    result = ColumnTypes.Xml;
                    break;
                default:
                    throw new ApplicationException("unknown type");
            }

            return result;
        }

    }
}
