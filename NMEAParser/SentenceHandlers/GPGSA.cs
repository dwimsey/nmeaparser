using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGSA
	{
		internal GPGSA()
		{
		}
	}

	public class GPGSAHandler : BaseSentenceHandler
	{
		internal GPGSAHandler()
		{
			p_Name = "GPGSA";
		}

		/// <summary>
		/// Parses GPGSA sentences into GPGSA objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGSA</code>
		/// Sentence Example: <code>$GPGSA</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPGSA object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			return (new GPGSA());
		}
	}
}
