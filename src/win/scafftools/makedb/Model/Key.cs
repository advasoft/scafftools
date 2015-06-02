﻿
namespace scafftools.makedb.Model
{
	public class Key
	{
		public Column UniqueColumn { get; set; }

		public SortDirectory Sorting { get; set; }

		public bool IsCompound { get; set; }

		public bool AutoGenerated { get; set; }

		public int StartIncrement { get; set; }

		public int IncrementStep { get; set; }
	}
}
