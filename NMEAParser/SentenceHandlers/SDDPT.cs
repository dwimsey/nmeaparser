using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class SDDPT
	{
		double Depth = double.MinValue;
		double KeelOffset = double.MinValue;
		double MaxDepth = double.MinValue;

		internal SDDPT(double depth, double keeloffset, double maxdepth)
		{
			this.Depth = depth;
			this.KeelOffset = keeloffset;
			this.MaxDepth = maxdepth;
		}
	}

	public class SDDPTHandler : BaseSentenceHandler
	{
		internal SDDPTHandler()
		{
			p_Name = "SDDPT";
		}

		/// <summary>
		/// Parses SDDPT sentences into SDDPT objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$SDDPT,{depth meters},{keel offset meters}*checksum</code>
		/// Sentence Example: <code>$SDDPT,3.0,0.0*54</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A SDDPT object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double depth = double.MinValue;
			double keeloffset = double.MinValue;
			double maxdepth = double.MinValue;

			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					depth = double.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse depth value from string: " + Fields[1].ToString(), "Depth", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				try {
					keeloffset = double.Parse(Fields[2]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse keel offset value from string: " + Fields[2].ToString(), "KeelOffset", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[3])) {
				try {
					maxdepth = double.Parse(Fields[3]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse maximum value from string: " + Fields[3].ToString(), "MaxDepth", ex);
				}
			}
			return (new SDDPT(depth, keeloffset, maxdepth));
		}
	}
}
