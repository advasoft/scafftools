
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public enum SortDirectory
	{
        [EnumMember]
        Asc,
        [EnumMember]
        Desc
	}
}
