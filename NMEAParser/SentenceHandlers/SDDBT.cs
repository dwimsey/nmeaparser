using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class SDDBT
	{
		public readonly Double Feet = Double.MinValue;
		public readonly Double Meters = Double.MinValue;
		public readonly Double Fathoms = Double.MinValue;

		internal SDDBT(double feet, double meters, double fathoms)
		{
			this.Feet = feet;
			this.Meters = meters;
			this.Fathoms = fathoms;
		}
	}

	public class SDDBTHandler : BaseSentenceHandler
	{
		internal SDDBTHandler()
		{
			p_Name = "SDDBT";
		}

		/// <summary>
		/// Parses SDDBT sentences into SDDBT objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$SDDBT,{depth1},{depth1 scale},{depth2},{depth2 scale},{depth3},{depth3 scale}*checksum</code>
		/// Sentence Example: <code>$SDDBT,,f,,M,,F*28</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A SDDBT object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double feet = double.MinValue;
			double meters = double.MinValue;
			double fathoms = double.MinValue;
			double depth;
			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					depth = double.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse depth value from string: " + Fields[1].ToString(), "Depth1", ex);
				}
				if(String.IsNullOrEmpty(Fields[2])) {
					throw new ArgumentNullException("DepthScale1", "Depth1 scale field is missing.");
				}
				if(Fields[2].Equals("f")) {
					feet = depth;
				} else if(Fields[2].Equals("M")) {
					meters = depth;
				} else if(Fields[2].Equals("F")) {
					fathoms = depth;
				} else {
					throw new ArgumentException("Could not parse depth scale value from string, unexpected scale: " + Fields[2].ToString(), "DepthScale1");
				}
			}
			if(!String.IsNullOrEmpty(Fields[3])) {
				try {
					depth = double.Parse(Fields[3]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse depth value from string: " + Fields[3].ToString(), "Depth2", ex);
				}
				if(String.IsNullOrEmpty(Fields[4])) {
					throw new ArgumentNullException("DepthScale2", "Depth2 scale field is missing.");
				}
				if(Fields[4].Equals("f")) {
					feet = depth;
				} else if(Fields[4].Equals("M")) {
					meters = depth;
				} else if(Fields[4].Equals("F")) {
					fathoms = depth;
				} else {
					throw new ArgumentException("Could not parse depth scale value from string, unexpected scale: " + Fields[4].ToString(), "DepthScale2");
				}
			}
			if(!String.IsNullOrEmpty(Fields[5])) {
				try {
					depth = double.Parse(Fields[5]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse depth3 value from string: " + Fields[5].ToString(), "Depth3", ex);
				}
				if(String.IsNullOrEmpty(Fields[6])) {
					throw new ArgumentNullException("DepthScale3", "Depth3 scale field is missing.");
				}
				if(Fields[6].Equals("f")) {
					feet = depth;
				} else if(Fields[6].Equals("M")) {
					meters = depth;
				} else if(Fields[6].Equals("F")) {
					fathoms = depth;
				} else {
					throw new ArgumentException("Could not parse depth scale value from string, unexpected scale: " + Fields[6].ToString(), "DepthScale3");
				}
			}
			return (new SDDBT(feet, meters, fathoms));
		}
	}
}
