using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPVTG
	{
		public enum GPSModeType {
			Active,
			Differential,
			Estimated,
			NotValid,
			Unavailable
		}
		public readonly double MagneticCourse = double.MinValue;
		public readonly double TrueCourse = double.MinValue;
		public readonly double GroundSpeedKnots = double.MinValue;
		public readonly double GroundSpeedKilometers = double.MinValue;
		public readonly GPSModeType Mode;

		internal GPVTG(double mcourse, double tcourse, double knots, double kilometers, GPSModeType mode)
		{
			this.MagneticCourse = mcourse;
			this.TrueCourse = tcourse;
			this.GroundSpeedKnots = knots;
			this.GroundSpeedKilometers = kilometers;
			this.Mode = mode;
		}
	}

	public class GPVTGHandler : BaseSentenceHandler
	{
		internal GPVTGHandler()
		{
			p_Name = "GPVTG";
		}

		/// <summary>
		/// Parses GPVTG sentences into GPVTG objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPVTG,{track},{True/Magnetic},{track2},{True/Magnetic},{distance},{Speed scale},{distance},{Speed scale},*checksum</code>
		/// Sentence Example: <code>$GPVTG,186.3,T,195.0,M,0.0,N,0.0,K,D*27</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPVTG object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double knots = double.MinValue;
			double kilometers = double.MinValue;
			double tcourse = double.MinValue;
			double mcourse = double.MinValue;
			GPVTG.GPSModeType mode = GPVTG.GPSModeType.Unavailable;
			double tmp;

			string vstr;
			string sstr;
			vstr = Fields[1];
			sstr = Fields[2];
			if(!String.IsNullOrEmpty(vstr)) {
				try {
					tmp = double.Parse(vstr);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse first course value from string: " + vstr, "Course1", ex);
				}
			} else {
				tmp = double.MinValue;
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				if(Fields[2].Equals("T")) {
					tcourse = tmp;
				} else if(Fields[2].Equals("M")) {
					mcourse = tmp;
				} else {
					throw new ArgumentException("Could not parse first course scale value from string, unexpected value: " + sstr, "CourseScale1");
				}
			} else {
				throw new ArgumentNullException("CourseScale1", "No scale provided for first course scale.");
			}

			vstr = Fields[3];
			sstr = Fields[4];
			if(!String.IsNullOrEmpty(vstr)) {
				try {
					tmp = double.Parse(vstr);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse second course value from string: " + vstr, "Course2", ex);
				}
			} else {
				tmp = double.MinValue;
			}
			if(!String.IsNullOrEmpty(sstr)) {
				if(sstr.Equals("T")) {
					tcourse = tmp;
				} else if(sstr.Equals("M")) {
					mcourse = tmp;
				} else {
					throw new ArgumentException("Could not parse second course scale value from string, unexpected value: " + sstr, "CourseScale2");
				}
			} else {
				throw new ArgumentNullException("CourseScale2", "No scale provided for second course scale.");
			}

			vstr = Fields[5];
			sstr = Fields[6];
			if(!String.IsNullOrEmpty(vstr)) {
				try {
					tmp = double.Parse(vstr);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse first speed value from string: " + vstr, "Speed1", ex);
				}
			} else {
				tmp = double.MinValue;
			}
			if(!String.IsNullOrEmpty(sstr)) {
				if(sstr.Equals("N")) {
					knots = tmp;
				} else if(sstr.Equals("K")) {
					kilometers = tmp;
				} else {
					throw new ArgumentException("Could not parse first speed scale value from string, unexpected value: " + sstr, "SpeedScale1");
				}
			} else {
				throw new ArgumentNullException("SpeedScale1", "No scale provided for first speed scale.");
			}

			vstr = Fields[7];
			sstr = Fields[8];
			if(!String.IsNullOrEmpty(vstr)) {
				try {
					tmp = double.Parse(vstr);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse second speed value from string: " + vstr, "Speed2", ex);
				}
			} else {
				tmp = double.MinValue;
			}
			if(!String.IsNullOrEmpty(sstr)) {
				if(sstr.Equals("N")) {
					knots = tmp;
				} else if(sstr.Equals("K")) {
					kilometers = tmp;
				} else {
					throw new ArgumentException("Could not parse second speed scale value from string, unexpected value: " + sstr, "SpeedScale2");
				}
			} else {
				throw new ArgumentNullException("SpeedScale2", "No scale provided for second speed scale.");
			}

			// field 9 only exists in NMEA 0183 >= v3.0
			if(!String.IsNullOrEmpty(Fields[9])) {
				if(Fields[9].Equals("A")) {
					mode = GPVTG.GPSModeType.Active;
				} else if(Fields[9].Equals("D")) {
					mode = GPVTG.GPSModeType.Differential;
				} else if(Fields[9].Equals("E")) {
					mode = GPVTG.GPSModeType.Estimated;
				} else if(Fields[9].Equals("N")) {
					mode = GPVTG.GPSModeType.NotValid;
				} else {
					throw new ArgumentOutOfRangeException("GPSMode", "Unexpected GPVTG mode: " + Fields[9]);
				}
			} else {
				mode = GPVTG.GPSModeType.Unavailable;
			}

			return (new GPVTG(mcourse, tcourse, knots, kilometers, mode));
		}
	}
}
