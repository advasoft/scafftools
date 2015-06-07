

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace scafftools.Model
{
    [DataContract]
    [Serializable]
    public class Db
	{
		public Db()
		{
			Tables = new List<Table>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<Table> Tables { get; private set; }
	}
}
