using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class HCHDG
	{
		public readonly Double Heading;
		public readonly Double Deviation;
		public readonly Double Variation;
		internal HCHDG(double heading, double deviation, double variation)
		{
			this.Heading = heading;
			this.Deviation = deviation;
			this.Variation = variation;
		}
	}

	public class HCHDGHandler : BaseSentenceHandler
	{
		internal HCHDGHandler()
		{
			p_Name = "HCHDG";
		}

		/// <summary>
		/// Parses HCHDG sentences into HCHDG objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$HCHDG,{heading},{deviation},{ddir},{variation},{vdir}*checksum</code>
		/// Sentence Example: <code>$HCHDG,278.4,,,8.7,W*3D</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A HCHDG object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double heading = 0.0;
			double deviation = 0.0;
			double variation = 0.0;

			if(String.IsNullOrEmpty(Fields[1])) {
				throw new Exception("Missing heading field for NMEA sentence: " + Fields[0]);
			}
			try {
				heading = Double.Parse(Fields[1]);
			} catch(Exception ex) {
				throw new ArgumentException("Could not parse heading value from string.", "Heading", ex);
			}

			if(!String.IsNullOrEmpty(Fields[2]) && !String.IsNullOrEmpty(Fields[3])) {
				deviation = Utils.ParseBearing(Fields[2], Fields[3]);
			}
			if(!String.IsNullOrEmpty(Fields[4]) && !String.IsNullOrEmpty(Fields[5])) {
				// try to parse the magnetic variation values
				variation = Utils.ParseBearing(Fields[4], Fields[5]);
			}

			return (new HCHDG(heading, deviation, variation));
		}
	}
}
