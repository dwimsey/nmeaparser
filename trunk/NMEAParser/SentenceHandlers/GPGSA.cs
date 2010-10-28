using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGSA
	{
		public readonly bool AutoFixMode;
		public readonly int FixMode;
		public readonly string[] PRNs;
		public readonly double PDOP;
		public readonly double HDOP;
		public readonly double VDOP;
		internal GPGSA(bool AutoFixMode, int FixMode, string[] PRNs, double PDOP, double HDOP, double VDOP)
		{
			this.AutoFixMode = AutoFixMode;
			this.FixMode = FixMode;
			this.PRNs = PRNs;
			this.PDOP = PDOP;
			this.HDOP = HDOP;
			this.VDOP = VDOP;
		}
	}

	public class GPGSAHandler : BaseSentenceHandler
	{
		internal GPGSAHandler()
		{
			p_Name = "GPGSA";
		}

		/// <summary>
		/// Parses GPGSA sentences into GPGSA objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGSA</code>
		/// Sentence Example: <code>$GPGSA</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPGSA object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			bool AutoFixMode = false;
			int FixMode = int.MinValue;
			string[] PRNs = new string[12];
			double PDOP = double.MinValue;
			double HDOP = double.MinValue;
			double VDOP = double.MinValue;

			if(!String.IsNullOrEmpty(Fields[1])) {
				switch(Fields[1]) {
					case "M":
						AutoFixMode = false;
						break;
					case "A":
						AutoFixMode = true;
						break;
					default:
						throw new ArgumentOutOfRangeException("AutoFixMode", Fields[1], "AutoFixMode value is unknown");
				}
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				switch(Fields[2]) {
					case "1":
						FixMode = 1;
						break;
					case "2":
						FixMode = 2;
						break;
					case "3":
						FixMode = 3;
						break;
					default:
						throw new ArgumentOutOfRangeException("FixMode", Fields[2], "FixMode value is unknown");
				}
			}

			PRNs[0] = Fields[3];
			PRNs[1] = Fields[4];
			PRNs[2] = Fields[5];
			PRNs[3] = Fields[6];
			PRNs[4] = Fields[7];
			PRNs[5] = Fields[8];
			PRNs[6] = Fields[9];
			PRNs[7] = Fields[10];
			PRNs[8] = Fields[11];
			PRNs[9] = Fields[12];
			PRNs[10] = Fields[13];
			PRNs[11] = Fields[14];

			if(!String.IsNullOrEmpty(Fields[15])) {
				try {
					PDOP = double.Parse(Fields[15]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse PDOP  value from string.", "PDOP", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[16])) {
				try {
					HDOP = double.Parse(Fields[16]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse HDOP  value from string.", "HDOP", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[17])) {
				try {
					string f = Fields[17];
					int i = f.IndexOf("*");
					if(i > -1) {
						f = f.Substring(0, i);
					}
					VDOP = double.Parse(f);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse VDOP  value from string.", "VDOP", ex);
				}
			}

			return (new GPGSA(AutoFixMode, FixMode, PRNs, PDOP, HDOP, VDOP));
		}
	}
}
