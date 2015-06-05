
using System;

namespace scafftools.Model
{
    [Serializable]
    public class Unique
	{
		public Column UniqueColumn { get; set; }

		public SortDirectory Sorting { get; set; }

		public bool IsCompound { get; set; }
	}
}
