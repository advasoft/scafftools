
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class Unique
	{
        [DataMember]
        public Column UniqueColumn { get; set; }

        [DataMember]
        public SortDirectory Sorting { get; set; }

        [DataMember]
        public bool IsCompound { get; set; }
	}
}
