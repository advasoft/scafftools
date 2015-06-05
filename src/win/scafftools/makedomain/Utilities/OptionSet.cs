

using scafftools.Utilities;

namespace scafftools.makedomain.Utilities
{

    /// <summary>
    /// Options from command line
    /// </summary>
    public class OptionSet
    {
        /// <summary>
        /// Hiden default constructor to restrict create OptionSet object from outside
        /// </summary>
        public OptionSet()
        {
            
        }

        #region options

        /// <summary>
        /// server type
        /// </summary>
        [Option("s", Required = false)]
        public DbServerTypeEnum ServerType { get; set; }

        /// <summary>
        /// connection string
        /// </summary>
        [Option("c", Required = false)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Overwrite scheme
        /// </summary>
        [Option("f", IsFlag = true)]
        public bool Force { get; set; }

		/// <summary>
		/// json serialized mkdb model path
		/// </summary>
		[Option("m", Required = false)]
		public string ModelPath { get; set; }

		/// <summary>
		/// path for generated script
		/// </summary>
		[Option("o", Required = true)]
		public string OutputPath { get; set; }

		/// <summary>
		/// Programming language type
		/// </summary>
		[Option("l", Required = true)]
		public LanguageTypeEnum LanguageType { get; set; }

		#endregion

	}
}
