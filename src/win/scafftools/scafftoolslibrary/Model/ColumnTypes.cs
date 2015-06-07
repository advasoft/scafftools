
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public enum ColumnTypes
	{
        [EnumMember]
        Int,
        [EnumMember]
        BigInt,
        [EnumMember]
        SmallInt,
        [EnumMember]
        Numeric,
        [EnumMember]
        Bit,
        [EnumMember]
        Binary,
        [EnumMember]
        Char,
        [EnumMember]
        Date,
        [EnumMember]
        DateTime,
        [EnumMember]
        DateTime2,
        [EnumMember]
		DateTimeOffset,
        [EnumMember]
        Text,
        [EnumMember]
        NText,
        [EnumMember]
        Decimal,
        [EnumMember]
        Float,
        [EnumMember]
        Money,
        [EnumMember]
        SmallMoney,
        [EnumMember]
        NVarChar,
        [EnumMember]
        NChar,
        [EnumMember]
        Variant,
        [EnumMember]
        Time,
        [EnumMember]
        TimeStamp,
        [EnumMember]
        UniqueIdentifier,
        [EnumMember]
        VarBinnary,
        [EnumMember]
        VarChar,
        [EnumMember]
        Xml
	}
}
