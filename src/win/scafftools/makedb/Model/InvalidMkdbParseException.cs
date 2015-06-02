
namespace scafftools.makedb.Model
{
    using System;

    public class InvalidMkdbParseException : ApplicationException
    {
        public InvalidMkdbParseException() { }

        public InvalidMkdbParseException(string message) : base(message) { }

        public InvalidMkdbParseException(string message, Exception innerException) : base(message, innerException) { }

        public int Line { get; set; }
    }
}
