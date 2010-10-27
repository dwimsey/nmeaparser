using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGSV
	{
		internal GPGSV()
		{
		}
	}

	public class GPGSVHandler : BaseSentenceHandler
	{
		internal GPGSVHandler()
		{
			p_Name = "GPGSV";
		}

		/// <summary>
		/// Parses GPGSV sentences into GPGSV objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGSV</code>
		/// Sentence Example: <code>$GPGSV</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPGSV object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			return (new GPGSV());
		}
	}
}
