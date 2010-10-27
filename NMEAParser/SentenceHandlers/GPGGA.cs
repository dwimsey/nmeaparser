using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPGGA
	{
		DateTime FixTime = DateTime.MinValue;
		double Latitude = double.MinValue;
		double Longitude = double.MinValue;
		int FixQuality;
		int SatellitesTracked;
		double HDOP;
		double Altitude;
		double HeightOfGeoid;
		double DGPSAge;
		string DGSStationId;

		internal GPGGA(DateTime ftime, double latitude, double longitude, int fqual, int stracked, double hdop, double alt, double hgeiod, double dgpsags, string dgpsid)
		{
			this.FixTime = ftime;
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.FixQuality = fqual;
			this.SatellitesTracked = stracked;
			this.HDOP = hdop;
			this.Altitude = alt;
			this.HeightOfGeoid = hgeiod;
			this.DGPSAge = dgpsags;
			this.DGSStationId = dgpsid;
		}
	}

	public class GPGGAHandler : BaseSentenceHandler
	{
		internal GPGGAHandler()
		{
			p_Name = "GPGGA";
		}

		/// <summary>
		/// Parses GPGGA sentences into GPGGA objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPGGA,{fix taken time},{latitude},{latdir},{longitude},{londir},{fix qaulity},{num sats tracked},{HDOP},{alt meters},{height of geoid(WGS84)},{seconds since last DGPS update},{DGPS station id}*checksum</code>
		/// Sentence Example: <code>$GPGGA,002732,3545.2764,N,07849.2324,W,2,08,1.2,136.1,M,-33.7,M,,*76</code>
		/// </remarks>
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A HCHDG object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			DateTime ftime = DateTime.MinValue;
			double lat = double.MinValue;
			double lon = double.MinValue;
			int fqual = int.MinValue;
			int satstracked = int.MinValue;
			double hdop = double.MinValue;
			double altitude = double.MinValue;
			double hgeoid = double.MinValue;
			double dgpsage = double.MinValue;
			string dgpsstationid = null;

			try {
				ftime = Utils.ParseTime(DateTime.UtcNow, Fields[1]);
			} catch(Exception ex) {
				throw new ArgumentException("Could not parse time value from string.", "FixTime", ex);
			}

			if(!String.IsNullOrEmpty(Fields[2]) && !String.IsNullOrEmpty(Fields[3]) && !String.IsNullOrEmpty(Fields[4]) && !String.IsNullOrEmpty(Fields[5])) {
				lat = Utils.ParseLatLon(Fields[2], Fields[3]);
				lon = Utils.ParseLatLon(Fields[4], Fields[5]);
			}

			if(!String.IsNullOrEmpty(Fields[6])) {
				try {
					fqual = Int32.Parse(Fields[6]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse fix quality value from string.", "FixQaulity", ex);
				}
			}


			if(!String.IsNullOrEmpty(Fields[7])) {
				try {
					satstracked = Int32.Parse(Fields[7]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse satellites tracked value from string.", "SatsTracked", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[8])) {
				try {
					hdop = double.Parse(Fields[8]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse horizontal dilution of precision value from string.", "HDOP", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[9])) {
				try {
					altitude = double.Parse(Fields[9]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse altitude value from string.", "Altitude", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[10])) {
				try {
					altitude = double.Parse(Fields[10]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse height of Geoid value from string.", "HGeoid", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[11])) {
				try {
					dgpsage = double.Parse(Fields[11]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse DGPS last update age value from string.", "DGPSAge", ex);
				}
			}

			if(!String.IsNullOrEmpty(Fields[12])) {
				dgpsstationid = Fields[12];
			}

			return (new GPGGA(ftime, lat, lon, fqual, satstracked, hdop, altitude, hgeoid, dgpsage, dgpsstationid));
		}
	}
}
