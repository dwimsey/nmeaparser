using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	class UnknownSentenceData
	{
		public readonly string SentenceName;
		public readonly string[] Fields;
		internal UnknownSentenceData(string[] Fields)
		{
			this.SentenceName = Fields[0].Substring(1);
			this.Fields = Fields;
		}
	}

	public class UnknownSentenceHandler : BaseSentenceHandler
	{
		internal UnknownSentenceHandler()
		{
			p_Name = "Unknown";
		}

		public override object ParseSentence(string[] Fields)
		{
			return(new UnknownSentenceData(Fields));
		}
	}

}
