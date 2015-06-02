
using System.Collections.Generic;

namespace scafftools.makedb.Model
{
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

		public string Name { get; set; }

		public IList<Column> Columns { get; private set; }
		public IList<Key> Keys { get; private set; }
		public IList<Index> Indexes { get; private set; }
		public IList<Unique> Uniques { get; private set; }
		public IList<Relation> Relations { get; private set; }
	}
}
