using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser
{
	public class Utils
	{
		internal static DateTime ParseTime(DateTime day, string val)
		{
			return (new DateTime(day.Year, day.Month, day.Day, Int32.Parse(val.Substring(0, 2)), Int32.Parse(val.Substring(2, 2)), Int32.Parse(val.Substring(4, 2))));
		}

		internal static DateTime ParseDate(string val)
		{
			return (new DateTime(int.Parse(val.Substring(4, 2)), int.Parse(val.Substring(2, 2)), int.Parse(val.Substring(0, 2))));
		}

		internal static double ParseBearing(string val, string dir)
		{
			double r;
			try {
				r = double.Parse(val);
			} catch(Exception ex) {
				throw new ArgumentException("Could not parse numerical value from string for direction.", ex);
			}
			if(dir == "W") {
				return (-1.0 * double.Parse(val));
			} else if(dir == "E") {
				return (double.Parse(val));
			}
			throw new Exception("Could not parse direction value from string for direction: Expect E (East) or W (West)");
		}

		internal static double ParseLatLon(string val, string dir)
		{
			try {
				switch(dir) {
					case "N":
						return (double.Parse(val.Substring(0, 2)) + (double.Parse(val.Substring(2)) / 60.0));
					case "E":
						return (double.Parse(val.Substring(0, 3)) + (double.Parse(val.Substring(3)) / 60.0));
					case "W":
						return (-1.0 * (double.Parse(val.Substring(0, 3)) + (double.Parse(val.Substring(3)) / 60.0)));
					case "S":
						return (-1.0 * (double.Parse(val.Substring(0, 2)) + (double.Parse(val.Substring(2)) / 60.0)));
				}
			} catch(Exception ex) {
				throw new ArgumentException("Could not parse numerical value from string for coordinate", ex);
			}
			throw new Exception("Could not parse direction value from string for coordinate: Expect N (North), E (East), W (West), or S (South)");
		}
	}
}
