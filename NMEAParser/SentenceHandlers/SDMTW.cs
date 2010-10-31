using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class SDMTW
	{
		public enum TemperatureScale
		{
			Kelvin,
			Celsius,
			Fahrenheit,
			Unknown
		}

		Double Temperature = Double.MinValue;
		TemperatureScale Scale = TemperatureScale.Unknown;

		internal SDMTW(double temp, TemperatureScale s)
		{
			this.Temperature = temp;
			this.Scale = s;
		}
	}

	public class SDMTWHandler : BaseSentenceHandler
	{
		internal SDMTWHandler()
		{
			p_Name = "SDMTW";
		}

		/// <summary>
		/// Parses SDMTW sentences into SDMTW objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$SDMTW,{temperature},{scale}*checksum</code>
		/// Sentence Example: <code>$SDMTW,26.6,C*06</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A SDMTW object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double temperature = double.MinValue;
			SDMTW.TemperatureScale scale = SDMTW.TemperatureScale.Unknown;

			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					temperature = double.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse temperature value from string.", "Heading", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				if(Fields[2].Equals("C")) {
					scale = SDMTW.TemperatureScale.Celsius;
				} else if(Fields[2].Equals("F")) {
					scale = SDMTW.TemperatureScale.Fahrenheit;
				} else if(Fields[2].Equals("K")) {
					scale = SDMTW.TemperatureScale.Kelvin;
				} else {
					throw new ArgumentException("Could not parse temperature scale value from string, unexpected value: " + Fields[2].ToString(), "Scale");
				}
			}
			return (new SDMTW(temperature, scale));
		}
	}
}
