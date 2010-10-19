using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPRMC
	{
		public readonly double Latitude;
		public readonly double Longitude;
		public readonly double Course;
		public readonly double Speed;
		public readonly bool GPSFix;
		public readonly double MagneticVariation;
		public readonly DateTime FixTimeStamp;

		internal GPRMC(double lat, double lon, double course, double speed, bool isGPSFix, double MagVariation, DateTime UTCTimeStamp)
		{
			this.Latitude = lat;
			this.Longitude = lon;
			this.Course = course;
			this.Speed = speed;
			this.GPSFix = isGPSFix;
			this.MagneticVariation = MagVariation;
			this.FixTimeStamp = UTCTimeStamp;
		}
	}

	public class GPRMCHandler : BaseSentenceHandler
	{
		internal GPRMCHandler()
		{
			p_Name = "GPRMC";
		}

		public override object ParseSentence(string[] Fields)
		{
			bool f;
			double lat = 0.0;
			double lon = 0.0;
			double s;
			double b;
			double m;
			DateTime time;

			if(String.IsNullOrEmpty(Fields[9])) {
				throw new Exception("Missing fix date field for NMEA sentence: " + Fields[0]);
			}
			if(!String.IsNullOrEmpty(Fields[1])) {
				time = NMEAParser.ParseTime(NMEAParser.ParseDate(Fields[9]), Fields[1]);
			} else {
				throw new Exception("Missing fix timestamp field for NMEA sentence: " + Fields[0]);
			}

			if(String.IsNullOrEmpty(Fields[2])) {
				throw new Exception("Missing GPS Fix valid flag for NMEA sentence: " + Fields[0]);
			}
			if("A".Equals(Fields[2])) {
				f = true;
			} else {
				f = false;
			}
			if(String.IsNullOrEmpty(Fields[3]) || String.IsNullOrEmpty(Fields[4]) || String.IsNullOrEmpty(Fields[5]) || String.IsNullOrEmpty(Fields[6])) {
				throw new Exception("Missing latitude and/or longitude field for NMEA sentence: " + Fields[0]);
			}
			lat = NMEAParser.ParseLatLon(Fields[3], Fields[4]);
			lon = NMEAParser.ParseLatLon(Fields[5], Fields[6]);

			if(!String.IsNullOrEmpty(Fields[7])) {
				s = Double.Parse(Fields[7]);
			} else {
				throw new Exception("Missing speed field for NMEA sentence: " + Fields[0]);
			}
			if(!String.IsNullOrEmpty(Fields[8])) {
				b = Double.Parse(Fields[8]);
			} else {
				throw new Exception("Missing course field for NMEA sentence: " + Fields[0]);
			}
			if(!String.IsNullOrEmpty(Fields[10])) {
				m = Double.Parse(Fields[10]);
			} else {
				throw new Exception("Missing magnetic variation field for NMEA sentence: " + Fields[0]);
			}
			return (new GPRMC(lat, lon, b, s, f, m, time));
		}
	}
}
