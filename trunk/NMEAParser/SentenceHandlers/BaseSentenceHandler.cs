using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class BaseSentenceHandler : ISentenceHandler
	{
		protected string p_Name;
		public string Name
		{
			get
			{
				return (p_Name);
			}
		}

		public void HandleSentence(NMEAParser sender, string[] Fields, bool EnableEvents)
		{
			object o = this.ParseSentence(Fields);
			if(EnableEvents && (this.OnSentenceRecieved != null)) {
				this.OnSentenceRecieved(sender, o);
			}
		}

		public virtual object ParseSentence(string[] Fields)
		{
			return (null);
		}

		public bool CallHandler(NMEAParser sender, object SentenceObject)
		{
			if(this.OnSentenceRecieved != null) {
				this.OnSentenceRecieved(sender, SentenceObject);
				return (true);
			}
			return (false);
		}

		public bool HasEvents
		{
			get
			{
				if(OnSentenceRecieved != null) {
					return (true);
				}
				return (false);
			}
		}

		public event SentenceHandlerCollection.SentenceRecievedHandler OnSentenceRecieved = null;
	}
}
