using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class SentenceHandlerCollection
	{
		private readonly System.Collections.Generic.Dictionary<string, ISentenceHandler> SentenceList;
		public delegate void SentenceRecievedHandler(NMEAParser sender, object SentenceObject);

		internal SentenceHandlerCollection()
		{
			SentenceList = new Dictionary<string, ISentenceHandler>();
			SentenceList.Add("GPRMC", new GPRMCHandler());
			SentenceList.Add("HCHDG", new GPRMCHandler());
			SentenceList.Add("unknown", new UnknownSentenceHandler());
		}

		public ISentenceHandler this[string SentenceName]    // Indexer declaration
		{
			get
			{
				if(this.SentenceList.ContainsKey(SentenceName)) {
					return (this.SentenceList[SentenceName]);
				} else {
					return (this.SentenceList["unknown"]);
				}
			}
		}
	}
}
