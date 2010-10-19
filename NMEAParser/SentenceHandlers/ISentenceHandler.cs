using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public interface ISentenceHandler
	{
		event SentenceHandlerCollection.SentenceRecievedHandler OnSentenceRecieved;
		string Name
		{
			get;
		}

		bool HasEvents
		{
			get;
		}

		void HandleSentence(NMEAParser sender, string[] Fields, bool EnableEvents);
		object ParseSentence(string[] Fields);
		bool CallHandler(NMEAParser sender, object SentenceObject);
	}
}
