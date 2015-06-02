
namespace scafftools.makedb.Model
{
	public class Relation
	{
		public Column UniqueColumnKey { get; set; }

		public Table OuterTable { get; set; }

		public Column OuterColumnKey { get; set; }

		public UpdateDeleteAction ActionOnUpdate { get; set; }

		public UpdateDeleteAction ActionOnDelete { get; set; }
	}
}
