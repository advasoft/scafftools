
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public enum UpdateDeleteAction
	{
        [EnumMember]
        NoAction,
        [EnumMember]
        Cascade,
        [EnumMember]
        SetNull,
        [EnumMember]
        SetDefault
	}
}
