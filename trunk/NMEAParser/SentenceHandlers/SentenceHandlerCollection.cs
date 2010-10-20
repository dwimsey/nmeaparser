using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NMEAParser.SentenceHandlers
{
	public class SentenceHandlerCollection
	{
		private readonly System.Collections.Generic.Dictionary<string, ISentenceHandler> SentenceList;
		public delegate void SentenceRecievedHandler(NMEAParser sender, object SentenceObject);

		internal SentenceHandlerCollection()
		{
			SentenceList = new Dictionary<string, ISentenceHandler>();

			Type iSentenceHandlerType = typeof(ISentenceHandler);
			Assembly tAssembly = Assembly.GetExecutingAssembly();
			Type[] ts = tAssembly.GetExportedTypes();
			foreach(Type t in ts) {
				if(!t.IsClass) {
					continue;
				}
				if(!iSentenceHandlerType.IsAssignableFrom(t)) {
					continue;
				}
				if(t.Name.Equals("BaseSentenceHandler")) {
					// We ignore this class because its the base class we use to derive from to save ourselves some work.
					continue;
				}

				// Create an instance of the handler class and add it to our list
				ISentenceHandler h = Activator.CreateInstance(t) as ISentenceHandler;
				this.SentenceList.Add(h.Name, h);
			}
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
