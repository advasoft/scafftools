

using System;
using System.Collections.Generic;

namespace scafftools.Model
{
    [Serializable]
    public class Db
	{
		public Db()
		{
			Tables = new List<Table>();
        }

		public string Name { get; set; }

		public IList<Table> Tables { get; private set; }
	}
}
