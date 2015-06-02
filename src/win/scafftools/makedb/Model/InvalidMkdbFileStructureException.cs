
namespace scafftools.makedb.Model
{
    using System;

    public class InvalidMkdbFileStructureException : ApplicationException
    {
        public int Line { get; set; }
    }
}
