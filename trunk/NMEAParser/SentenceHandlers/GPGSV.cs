using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGSV
	{
		public readonly int TotalLines;
		public readonly int ThisLine;
		public readonly int SatellitesInView;
		public readonly GPGSVHandler.Satellite[] Satellites;

		internal GPGSV(int total_lines, int this_line, int sats_in_view, GPGSVHandler.Satellite[] sats)
		{
			this.TotalLines = total_lines;
			this.ThisLine = this_line;
			this.SatellitesInView = sats_in_view;
			this.Satellites = sats;
		}
	}

	public class GPGSVHandler : BaseSentenceHandler
	{
		internal GPGSVHandler()
		{
			p_Name = "GPGSV";
		}

		/// <summary>
		/// Parses GPGSV sentences into GPGSV objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGSV,(total lines),(this line),(sats in view),(prn #0),(elevation0),(azimuth0),(signal/noise0),(prn #1),(elevation1),(azimuth1),(signal/noise1),(prn #2),(elevation2),(azimuth2),(signal/noise2),(prn #3),(elevation3),(azimuth3),(signal/noise3)*checksum</code>
		/// Sentence Example: <code>$GPGSV,3,1,12,10,35,045,00,26,22,161,00,15,17,182,00*45</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPGSV object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			int tlines = int.MinValue;
			int cline_num = int.MinValue;
			int sats_in_view = int.MinValue;
			Satellite[] sats = {null, null, null, null};

			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					tlines = int.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse total line count value from string.", "TotalLines", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[2])) {
				try {
					cline_num = int.Parse(Fields[2]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse current line number value from string.", "ThisLine", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[3])) {
				try {
					cline_num = int.Parse(Fields[3]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse satellites in view value from string.", "SatellitesInView", ex);
				}
			}

			int c = 0;
			for(int offset = 4; (offset+3) < Fields.Length; offset += 4) {
				try {
					if((!String.IsNullOrEmpty(Fields[offset])) && (!String.IsNullOrEmpty(Fields[offset+1])) && (!String.IsNullOrEmpty(Fields[offset+2]))) {
						sats[c++] = Satellite.Parse(Fields[offset], Fields[offset+1], Fields[offset+2], Fields[offset+3]);
					}
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse satellites information from strings.", "SatellitesInView", ex);
				}
			}				
			return (new GPGSV(tlines, cline_num, sats_in_view, sats));
		}

		public class Satellite
		{
			public readonly string PRN;
			public readonly double Elevation;
			public readonly double Azimuth;
			public readonly double SNR;

			internal Satellite(string PRN, double Elevation, double Azimuth, double SNR)
			{
				this.PRN = PRN;
				this.Elevation = Elevation;
				this.Azimuth = Azimuth;
				this.SNR = SNR;
			}

			internal static Satellite Parse(string prn, string elevation, string azimuth, string snr)
			{
				if(prn == null) {
					prn = "";
				}
				double e = double.MinValue;
				double a = double.MinValue;
				double s = double.MinValue;
				if(!String.IsNullOrEmpty(elevation)) {
					try {
						e = double.Parse(elevation);
					} catch(Exception ex) {
						throw new ArgumentException("Could not parse elevation value from string.", "elevation", ex);
					}
				}
				if(!String.IsNullOrEmpty(azimuth)) {
					try {
						a = double.Parse(azimuth);
					} catch(Exception ex) {
						throw new ArgumentException("Could not parse azimuth value from string.", "azimuth", ex);
					}
				}
				if(!String.IsNullOrEmpty(snr)) {
					try {
						s = double.Parse(snr);
					} catch(Exception ex) {
						throw new ArgumentException("Could not parse signal to noise ratio value from string.", "snr", ex);
					}
				} else {
					// we set to MinValue for null input values as this means not tracking
					s = double.MinValue;
				}

				return (new Satellite(prn, e, a, s));
			}
		}
	}
}
