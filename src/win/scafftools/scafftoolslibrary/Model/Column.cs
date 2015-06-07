
using System;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
	public class Column
	{
        [DataMember]
		public string Name { get; set; }

        [DataMember]
        public ColumnTypes Type { get; set; }

        [DataMember]
        public int Length { get; set; }

        [DataMember]
        public int IntegerPart { get; set; }

        [DataMember]
        public int FactorialPart { get; set; }

        [DataMember]
        public bool CanBeNull { get; set; }
	}
}
