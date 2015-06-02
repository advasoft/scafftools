
namespace scafftools.makedb.Model
{
	public class Index
	{
		public Column UniqueColumn { get; set; }

		public SortDirectory Sorting { get; set; }

		public bool IsCompound { get; set; }
	}
}
