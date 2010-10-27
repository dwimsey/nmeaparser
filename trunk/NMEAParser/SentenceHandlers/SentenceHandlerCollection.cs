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

			// Get the type for the sentence handler interface and the assembly we belong to
			Type iSentenceHandlerType = typeof(ISentenceHandler);
			Assembly tAssembly = Assembly.GetExecutingAssembly();
			// Get a list of all the types we have access too
			Type[] ts = tAssembly.GetExportedTypes();
			
			// Loop through the types we found and call the handler loaders
			foreach(Type t in ts) {
				if(!t.IsClass) {
					// This isn't a class so we don't care about it.
					continue;
				}
				if(!iSentenceHandlerType.IsAssignableFrom(t)) {
					// This class doesn't adhere to the ISentenceHandler interface, ignore it.
					continue;
				}
				if(t.Name.Equals("BaseSentenceHandler")) {
					// We ignore this class because its the base class we use to derive from to save ourselves some work.
					continue;
				}

				// Create an instance of the handler class and add it to our list
				ISentenceHandler h = Activator.CreateInstance(t, true) as ISentenceHandler;
				this.SentenceList.Add(h.Name, h);
			}
		}

		/// <summary>
		/// Find the handler for the specified sentence type.
		/// </summary>
		/// <param name="SentenceName">Name of the sentence to find a handler for.</param>
		/// <returns>The ISentenceHandler for the sentence named, or the UnknownSentenceHandler if
		/// the sentence is of an unknown type.</returns>
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
