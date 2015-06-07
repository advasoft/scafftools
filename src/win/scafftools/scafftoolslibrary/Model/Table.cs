
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public class Table
	{
		public Table()
		{
			Columns = new List<Column>();
			Keys = new List<Key>();
			Indexes = new List<Index>();
			Uniques = new List<Unique>();
			Relations = new List<Relation>();

		}

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<Column> Columns { get; private set; }
        [DataMember]
        public IList<Key> Keys { get; private set; }
        [DataMember]
        public IList<Index> Indexes { get; private set; }
        [DataMember]
        public IList<Unique> Uniques { get; private set; }
        [DataMember]
        public IList<Relation> Relations { get; private set; }
	}
}
