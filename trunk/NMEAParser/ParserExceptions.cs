using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser
{
	public class ParserException : Exception
	{
		internal ParserException()
			: base()
		{
		}

		internal ParserException(string msg)
			: base(msg)
		{
		}

		internal ParserException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		internal ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}

	public class ParserEventException : Exception
	{
		internal ParserEventException()
			: base()
		{
		}

		internal ParserEventException(string msg)
			: base(msg)
		{
		}

		internal ParserEventException(string msg, Exception innerException)
			: base(msg, innerException)
		{
		}

		internal ParserEventException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
