
namespace scafftools.makedb.Utilities
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
        /// (required) server type
        /// </summary>
        [Option("s", Required = true)]
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
		/// mkdb files path
		/// </summary>
		[Option("p", Required = true)]
		public string SourcePath { get; set; }

		/// <summary>
		/// path for generated script
		/// </summary>
		[Option("o", Required = true)]
		public string OutputPath { get; set; }

		/// <summary>
		/// Create database
		/// </summary>
		[Option("d", IsFlag = true)]
		public bool GenerateDb { get; set; }

		#endregion

	}
}
