using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGLL
	{
		public readonly Double Latitude;
		public readonly Double Longitude;
		public readonly DateTime FixTimestamp;
		public readonly bool FixActive;

		internal GPGLL(double latitude, double longitude, DateTime fixts, bool fixactive)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.FixTimestamp = fixts;
			this.FixActive = fixactive;
		}
	}

	public class GPGLLHandler : BaseSentenceHandler
	{
		internal GPGLLHandler()
		{
			p_Name = "GPGLL";
		}

		/// <summary>
		/// Parses GPGLL sentences into GPGLL objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGLL,{latitude},{latdir},{longitude},{londir},{time of fix},{fix active/void}*checksum</code>
		/// Sentence Example: <code>$GPGLL,3545.2764,N,07849.2324,W,002732,A,D*51</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A HCHDG object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double lat = double.MinValue;
			double lon = double.MinValue;
			DateTime ftime = DateTime.MinValue;
			bool factive = false;
			if(!String.IsNullOrEmpty(Fields[1]) && !String.IsNullOrEmpty(Fields[2]) && !String.IsNullOrEmpty(Fields[3]) && !String.IsNullOrEmpty(Fields[4])) {
				lat = Utils.ParseLatLon(Fields[1], Fields[2]);
				lon = Utils.ParseLatLon(Fields[3], Fields[4]);
			}

			if(!String.IsNullOrEmpty(Fields[5])) {
				ftime = Utils.ParseTime(DateTime.UtcNow, Fields[5]);
			}

			if(!String.IsNullOrEmpty(Fields[6])) {
				if(Fields[6].Equals("A")) {
					factive = true;
				} else if(Fields[6].Equals("V")) {
					factive = false;
				}
			}
			return (new GPGLL(lat, lon, ftime, factive));
		}
	}
}
