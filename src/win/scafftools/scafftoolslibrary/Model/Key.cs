﻿
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public class Key
	{
        [DataMember]
        public Column UniqueColumn { get; set; }

        [DataMember]
        public SortDirectory Sorting { get; set; }

        [DataMember]
        public bool IsCompound { get; set; }

        [DataMember]
        public bool AutoGenerated { get; set; }

        [DataMember]
        public int StartIncrement { get; set; }

        [DataMember]
        public int IncrementStep { get; set; }
	}
}