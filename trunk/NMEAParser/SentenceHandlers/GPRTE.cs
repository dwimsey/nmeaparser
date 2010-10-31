using System;
using System.Collections.Generic;
using System.Text;

namespace NMEAParser.SentenceHandlers
{
	public class GPRTE
	{
		public enum TemperatureScale
		{
			Kelvin,
			Celsius,
			Fahrenheit,
			Unknown
		}

		public readonly Double Temperature = Double.MinValue;
		public readonly TemperatureScale Scale = TemperatureScale.Unknown;

		internal GPRTE(double temp, TemperatureScale s)
		{
			this.Temperature = temp;
			this.Scale = s;
		}
	}

	public class GPRTEHandler : BaseSentenceHandler
	{
		internal GPRTEHandler()
		{
			p_Name = "GPRTE";
		}

		/// <summary>
		/// Parses GPRTE sentences into GPRTE objects.
		/// </summary>
		/// <remarks>
		/// Sentence Format: <code>$GPRTE,{num of sentences},{this sentence index},{},*checksum</code>
		/// Sentence Example: <code>$GPRTE,1,1,c,*37</code>
		/// </remarks>
		/// 
		/// <param name="Fields">Sentence fields for the sentence to be parsed.</param>
		/// <returns>A GPRTE object representing the data in Fields</returns>
		public override object ParseSentence(string[] Fields)
		{
			double temperature = double.MinValue;
			GPRTE.TemperatureScale scale = GPRTE.TemperatureScale.Unknown;

			if(!String.IsNullOrEmpty(Fields[1])) {
				try {
					temperature = double.Parse(Fields[1]);
				} catch(Exception ex) {
					throw new ArgumentException("Could not parse temperature value from string.", "Heading", ex);
				}
			}
			if(!String.IsNullOrEmpty(Fields[2])) {
				if(Fields[2].Equals("C")) {
					scale = GPRTE.TemperatureScale.Celsius;
				} else if(Fields[2].Equals("F")) {
					scale = GPRTE.TemperatureScale.Fahrenheit;
				} else if(Fields[2].Equals("K")) {
					scale = GPRTE.TemperatureScale.Kelvin;
				} else {
					throw new ArgumentException("Could not parse temperature scale value from string, unexpected value: " + Fields[2].ToString(), "Scale");
				}
			}
			return (new GPRTE(temperature, scale));
		}
	}
}
