
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class Relation
	{
        [DataMember]
        public Column UniqueColumnKey { get; set; }

        [DataMember]
        public Table OuterTable { get; set; }

        [DataMember]
        public Column OuterColumnKey { get; set; }

        [DataMember]
        public UpdateDeleteAction ActionOnUpdate { get; set; }

        [DataMember]
        public UpdateDeleteAction ActionOnDelete { get; set; }
	}
}
