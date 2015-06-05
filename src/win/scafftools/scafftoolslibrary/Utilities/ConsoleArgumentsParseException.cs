
namespace scafftools.Utilities
{
	using System;

	public class ConsoleArgumentsParseException : ApplicationException
	{
		public ConsoleArgumentsParseException()
		{

		}

		public ConsoleArgumentsParseException(string message) : base(message)
		{
		}

		public ConsoleArgumentsParseException(string message, Exception innerException) : base(message, innerException)
		{

		}
    }
}
