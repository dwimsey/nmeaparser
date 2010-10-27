using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class SDMWT
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

		internal SDMWT(double temp, TemperatureScale s)
		{
			this.Temperature = temp;
			this.Scale = s;
		}
	}

	public class SDMWTHandler : BaseSentenceHandler
	{
		internal SDMWTHandler()
		{
			p_Name = "SDMWT";
		}

		/// <summary>
		/// Parses SDMWT sentences into SDMWT objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$SDMWT,{temperature},{scale}*checksum</code>
		/// Sentence Example: <code>$SDMTW,26.6,C*06</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A SDMWT object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double temperature = double.MinValue;
			SDMWT.TemperatureScale scale = SDMWT.TemperatureScale.Unknown;

			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					temperature = double.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse temperature value from string.", "Heading", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				if(Fields[2].Equals("C")) {
					scale = SDMWT.TemperatureScale.Celsius;
				} else if(Fields[2].Equals("F")) {
					scale = SDMWT.TemperatureScale.Fahrenheit;
				} else if(Fields[2].Equals("K")) {
					scale = SDMWT.TemperatureScale.Kelvin;
				} else {
					throw new ArgumentException("Could not parse temperature scale value from string, unexpected value: " + Fields[2].ToString(), "Scale");
				}
			}
			return (new SDMWT(temperature, scale));
		}
	}
}
