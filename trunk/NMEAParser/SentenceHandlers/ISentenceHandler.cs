using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public interface ISentenceHandler
	{
		/// <summary>
		/// Stores the event handlers which are called when HandleSentence or CallHandler is called.
		/// </summary>
		event SentenceHandlerCollection.SentenceRecievedHandler OnSentenceRecieved;

		/// <summary>
		/// Name of the sentence this handler processes.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Returns true if OnSentenceRecieved is not null
		/// </summary>
		bool HasEvents
		{
			get;
		}

		/// <summary>
		/// Processes the sentence fields provided and call the sentence handlers if needed.
		/// </summary>
		/// <param name="sender">The NMEAParser object which is calling this handler.</param>
		/// <param name="Fields">Fields </param>
		/// <param name="EnableEvents">If true any OnSentenceRecieved handlers will be
		/// triggered.  If false, event handlers will be ignored.</param>
		void HandleSentence(NMEAParser sender, string[] Fields, bool EnableEvents);

		/// <summary>
		/// Parses the fields of a sentence and returns an object for the sentence.
		/// </summary>
		/// <param name="Fields">Fields of the sentence to be processed.</param>
		/// <returns>An object containing the data from the parsed sentence.</returns>
		object ParseSentence(string[] Fields);

		/// <summary>
		/// Calls the OnSentenceRecieved event handlers associated with this sentence
		/// with the argument specified in SentenceObject.
		/// </summary>
		/// <param name="sender">NMEAPaser which owns this handler.</param>
		/// <param name="SentenceObject">Object to be passed to the OnSentencesRecieved handlers for this handler.</param>
		/// <returns></returns>
		bool CallHandler(NMEAParser sender, object SentenceObject);
	}
}
