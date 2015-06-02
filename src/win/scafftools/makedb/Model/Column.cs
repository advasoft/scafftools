
namespace scafftools.makedb.Model
{
	public class Column
	{
		public string Name { get; set; }

		public ColumnTypes Type { get; set; }

		public int Length { get; set; }

		public int IntegerPart { get; set; }

		public int FactorialPart { get; set; }

		public bool CanBeNull { get; set; }
	}
}
