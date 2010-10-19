﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using NMEAParser.SentenceHandlers;

namespace NMEAParser
{
	public class NMEAParser
	{
		public enum ParserMode
		{
			Serial,
			LogFile,
			Generated
		}

		public SentenceHandlerCollection Sentences;
		public TimeSpan SerialPortActivityTimeout;
		public int SerialPortActivityRetryCount;

		public string ConnectionString
		{
			get
			{
				switch(p_ParserMode) {
					case ParserMode.Serial:
						return (p_SerialPortPortName);
					case ParserMode.LogFile:
						throw new NotImplementedException("Not yet implemented");
					case ParserMode.Generated:
						throw new NotImplementedException("Not yet implemented");
					default:
						throw new ArgumentOutOfRangeException("Mode", p_ParserMode, "Unexpected ParserMode.");
				}
			}
			set
			{
				switch(p_ParserMode) {
					case ParserMode.Serial:
						{
							string[] spns = System.IO.Ports.SerialPort.GetPortNames();
							if(!"AUTO".Equals(value)) {
								foreach(string pn in spns) {
									if(pn == null) {
										continue;
									}

									if(pn.Equals(value)) {
										p_SerialPortPortName = value;
										break;
									}
								}
								if(!p_SerialPortPortName.Equals(value)) {
									throw new System.IO.FileNotFoundException("Could not find specified COM port.", "value");
								}
							} else {
								p_SerialPortPortName = value;
							}
						}
						break;
					case ParserMode.LogFile:
						throw new NotImplementedException("Not yet implemented");
					case ParserMode.Generated:
						throw new NotImplementedException("Not yet implemented");
					default:
						throw new ArgumentOutOfRangeException("Mode", p_ParserMode, "Unexpected ParserMode");
				}
			}
		}

		private ParserMode p_ParserMode;
		public ParserMode Mode
		{
			get
			{
				return (this.p_ParserMode);
			}
			set
			{
				this.p_ParserMode = value;
			}
		}

		private string p_DeviceMake;
		public string DeviceMake
		{
			get
			{
				return (this.p_DeviceMake);
			}
			set
			{
				this.p_DeviceMake = value;
			}
		}

		private string p_DeviceModel;
		public string DeviceModel
		{
			get
			{
				return (this.p_DeviceModel);
			}
			set
			{
				this.p_DeviceModel = value;
				SerialApplyDeviceConfiguration();
			}
		}

		private string p_NMEAVersion;
		public string NMEAVersion
		{
			get
			{
				return (p_NMEAVersion);
			}
		}

		#region Serial port state variables

		private string p_SerialPortPortName;
		private int p_SerialPortBaudRate;
		private int p_SerialPortDataBits;
		private int p_SerialPortParity;
		private int p_SerialPortStopBits;
		private byte p_SerialPortParityReplacementByte;
		private SerialPort p_SerialPort;

		#endregion Serial port state variables

		#region Serial port access methods

		bool SerialCheckPortForNMEADevice(string portName)
		{
			string junk;
			SerialPort tp = new SerialPort(portName, this.p_SerialPort.BaudRate, this.p_SerialPort.Parity, this.p_SerialPort.DataBits, this.p_SerialPort.StopBits);
			try {
				tp.Open();
				if(tp.IsOpen) {
					try {
						try {
							tp.ReadTimeout = 1000;
							//						this.SerialPortActivityTimeout.Milliseconds;
							for(int i = 0; i < this.SerialPortActivityRetryCount; i++) {
								try {
									while(true) {
										junk = tp.ReadLine();
										try {
											ParseNMEA0183Sentence(junk, false);
											return (true);
										} catch(Exception ex) {
											int ii = 0;
											ii++;
										}
									}
								} catch(TimeoutException) {
									continue;
								}
							}
							// didn't get a NMEA string, abort
							return (false);
						} finally {
							tp.Close();
						}
					} finally {
						if(tp.IsOpen) {
							tp.Close();
						}
					}
				} else {
					return (false);
				}
//			} catch(Exception ex) {
			} catch(Exception) {
				return (false);
			} finally {
				tp.Dispose();
			}
		}

		private void SerialApplyDeviceConfiguration()
		{
			int b;
			int bits;
			int p;
			int sb;
			byte c;

			try {
				switch(p_DeviceMake) {
					case "Generic": {
							string[] parts;
							try {
								parts = p_DeviceModel.Split('@');
							} catch(Exception ex) {
								throw new Exception("Could not parse Generic device make string: " + p_DeviceMake, ex);
							}

							try {
								// Data bits per packet
								try {
									bits = Int32.Parse(parts[0].Substring(0, 1));
								} catch(Exception ix) {
									throw new Exception("", ix);
								}
								if(bits>9) {
									throw new ArgumentOutOfRangeException("Data bit count is out of range: " + parts[0].Substring(0, 1) +" > 8");
								}
								if(bits<5) {
									throw new ArgumentOutOfRangeException("Data bit count is out of range: " + parts[0].Substring(0, 1) +" < 5");
								}

								// Parity bits per packet
								try {
									switch(parts[0].Substring(1, 1).ToLower()) {
										case "n":
											p = 0;
											break;
										case "o":
											p = 1;
											break;
										case "e":
											p = 2;
											break;
										case "s":
											p = 3;
											break;
										case "m":
											p = 4;
											break;
										default:
											throw new ArgumentOutOfRangeException("Unexpected value for parity of Generic DeviceMake: " + parts[0].Substring(1, 1));
									}
								} catch(Exception ix) {
									throw new Exception("Could not parse parity value of Generic DeviceMake: " + parts[0].Substring(1, 1), ix);
								}

								// Stop bits per packet
								try {
									switch(parts[0].Substring(2)) {
										case "0":
											sb = 0;
											break;
										case "1":
											sb = 1;
											break;
										case "1.5":
											sb = 3;
											break;
										case "2":
											sb = 2;
											break;
										default:
											throw new ArgumentOutOfRangeException("StopBits is out of range: " + parts[0].Substring(2));
									}
								} catch(Exception ix) {
									throw new Exception("", ix);
								}

								// Parity recovered byte value
								c = 0xFF;
							} catch(Exception ex) {
								throw new Exception("Could not parse port settings from from Generic DeviceMake: " + p_DeviceMake, ex);
							}

							// Baud Rate
							try {
								b = Int32.Parse(parts[1]);
							} catch {
								throw new Exception("Could not parse baud rate from Generic DeviceMake: " + p_DeviceMake);
							}

							p_SerialPortBaudRate = b;
							p_SerialPortDataBits = bits;
							p_SerialPortParity = p;
							p_SerialPortStopBits = sb;
							p_SerialPortParityReplacementByte = c;
						}
						break;
					case "Garmin":
						switch(p_DeviceModel) {
							case "Intelliducer":
								p_SerialPortBaudRate = 4800;
								p_SerialPortDataBits = 8;
								p_SerialPortParity = 0;
								p_SerialPortStopBits = 1;
								p_SerialPortParityReplacementByte = 0xFF;
								break;
							default:
								throw new Exception("Unknown Garmin model specified: " + p_DeviceModel);
						}
						break;
					default:
						throw new Exception("Unknown hardware make specified: " + p_DeviceMake);
				}
			} catch(Exception ex) {
				throw new Exception("Unexpected exception setting", ex);
			}
		}

		public void SerialConnect()
		{
			if(p_SerialPort != null) {
				Disconnect();
			}

			p_SerialPort = new SerialPort();
			p_SerialPort.BaudRate = p_SerialPortBaudRate;
			p_SerialPort.DataBits = p_SerialPortDataBits;
			switch(p_SerialPortParity) {
				case 0:
					p_SerialPort.Parity = Parity.None;
					break;
				case 1:
					p_SerialPort.Parity = Parity.Odd;
					break;
				case 2:
					p_SerialPort.Parity = Parity.Even;
					break;
				case 3:
					p_SerialPort.Parity = Parity.Space;
					break;
				case 4:
					p_SerialPort.Parity = Parity.Mark;
					break;
				default:
					throw new Exception("Unexpected value for serial port Parity: " + p_SerialPortParity.ToString());
			}
			switch(p_SerialPortStopBits) {
				case 0:
					p_SerialPort.StopBits = StopBits.None;
					break;
				case 1:
					p_SerialPort.StopBits = StopBits.One;
					break;
				case 2:
					p_SerialPort.StopBits = StopBits.Two;
					break;
				case 3:
					// this really means 1.5
					p_SerialPort.StopBits = StopBits.OnePointFive;
					break;
				default:
					throw new Exception("Unexpected value for serial port stop bits: " + p_SerialPortStopBits.ToString());
			}
			p_SerialPort.ParityReplace = p_SerialPortParityReplacementByte;
			if("AUTO".Equals(p_SerialPortPortName)) {
				string[] spns = System.IO.Ports.SerialPort.GetPortNames();
				foreach(string pn in spns) {
					if(pn == null) {
						continue;
					}
					if(this.SerialCheckPortForNMEADevice(pn)) {
						p_SerialPort.PortName = pn;
						break;
					}
				}
			} else {
				p_SerialPort.PortName = p_SerialPortPortName;
			}


			p_SerialPort.DataReceived += SerialDataReceived;
			p_SerialPort.Open();
		}

		public void SerialDisconnect()
		{
			if(p_SerialPort != null) {
				p_SerialPort.DataReceived -= SerialDataReceived;
				try {
					if(p_SerialPort.IsOpen) {
						p_SerialPort.Close();
					}
				} catch(Exception ex) {
					throw new Exception("Exception closing serial port: " + p_SerialPort.PortName, ex);
				}
				p_SerialPort.Dispose();
				p_SerialPort = null;
			}
		}

		private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if(!p_SerialPort.IsOpen) {
				return;
			}

			string sentence;
			while(p_SerialPort.BytesToRead > 5) {
				// this means there is enough data there to be a complete sentence so take a look
				// we should probably do some sort of timeout check here
				sentence = p_SerialPort.ReadLine();	//this may block breifly but thats okay, we're in our own thread
				if(p_EnableRAWLogfile && p_RAWLogfile != null) {
					p_RAWLogfile.WriteLine(sentence);
					p_RAWLogfile.Flush();
				}
				try {
					ParseNMEA0183Sentence(sentence, true);
				} catch {
					int i = 0;
					i++;
					// while we want to ignore invalid NMEA sentences, we don't want them to take the whole
					// application down due to some fuzz on a serial cable or unexpected output from a device
				}
			}
		}

		#endregion Serial port access methods

		public NMEAParser()
		{
			Sentences = new SentenceHandlerCollection();
			SerialPortActivityTimeout = new TimeSpan(0, 0, 0, 0, 1000);
			SerialPortActivityRetryCount = 1;
			p_DeviceMake = "Generic";
			p_DeviceModel = "4800-8N1";
			p_NMEAVersion = "1.5";
			p_RAWLogfile = null;
			p_EnableRAWLogfile = false;
		}

		private System.IO.TextWriter p_RAWLogfile;
		private bool p_EnableRAWLogfile;
		public bool EnableRAWLogfile
		{
			get
			{
				return (p_EnableRAWLogfile);
			}
			set
			{
				if(p_EnableRAWLogfile == value) {
					return;
				}

				p_EnableRAWLogfile = value;
				if(value) {
					if(p_RAWLogfile == null) {
						DateTime n = DateTime.Now;
						string fname = "C:\\NMEA-" + n.Year.ToString() + n.Month.ToString() + n.Day.ToString() + n.Hour.ToString() + n.Minute.ToString() + n.Second.ToString() + ".log";
						p_RAWLogfile = System.IO.File.CreateText(fname);
					}
				} else {
					if(p_RAWLogfile != null) {
						p_RAWLogfile.Close();
						p_RAWLogfile = null;
					}
				}
			}
		}

		public void Connect()
		{
			if(p_SerialPort != null) {
				Disconnect();
			}

			EnableRAWLogfile = true;
			switch(p_ParserMode) {
				case ParserMode.Serial:
					SerialConnect();
					break;
				case ParserMode.LogFile:
				case ParserMode.Generated:
					throw new NotImplementedException("This mode is not yet implemented: " + p_ParserMode.ToString());
				default:
					throw new ArgumentOutOfRangeException("Mode", p_ParserMode, "Unexpected parser mode: " + p_ParserMode.ToString());
			}
		}

		public void Disconnect()
		{
			if(p_SerialPort != null) {
				if(p_SerialPort.IsOpen) {
					p_SerialPort.Close();
				}
				SerialDisconnect();
			}
			EnableRAWLogfile = false;
		}

		private bool VerifyNMEA0183CheckSum(string NMEA0183Sentence)
		{
			int chksum;

			if(NMEA0183Sentence.Length < 5) {
				// bad sentence
				return (false);
			}
			NMEA0183Sentence = NMEA0183Sentence.Trim();

			// verify we have a checksum on the end
			if(NMEA0183Sentence[NMEA0183Sentence.Length-3] != '*') {
				return (false);
			}

			char c;
			chksum = 0;
			bool keepGoing = true;
			for(int i = 0; i < (NMEA0183Sentence.Length-3); i++) {
				c = NMEA0183Sentence[i];
				switch(c) {
					case '*':		// this is the end of data
						if(i != (NMEA0183Sentence.Length-3)) {
							throw new Exception("Found sentence checksum seperator too early: * was found at " + i.ToString() + " instead of expected location at " + (NMEA0183Sentence.Length - 3).ToString() + ".");
						}
						keepGoing = false;
						break;
					case '$':		// skip this character
						break;
					default:
						if(i == 0) {
							chksum = Convert.ToByte(c);
						} else {
							chksum ^= Convert.ToByte(c);
						}
						break;
				}
				if(!keepGoing) {
					break;
				}
			}

			if(!NMEA0183Sentence.Substring(NMEA0183Sentence.Length-2, 2).Equals(chksum.ToString("X2"))) {
				return (false);
			}
			return (true);
		}

		private void ParseNMEA0183Sentence(string NMEA0183Sentence, bool EnableEvents)
		{
			if(!NMEA0183Sentence.StartsWith("$")) {
				throw new ParserException("Sentence does not begin with $.");
			}

			if(!VerifyNMEA0183CheckSum(NMEA0183Sentence)) {
				// checksum is invalid, ignore this line
				throw new ParserException("Checksum verification failed.");
			}

			string[] fields = NMEA0183Sentence.Split(',');
			if(fields.Length < 2) {
				// doesn't look like a NMEA line!
				throw new ParserException("Sentence does not contain enough fields");
			}

			string NMEACmd = "Error";
			try {
				NMEACmd = fields[0].Substring(1);
				ISentenceHandler handler = Sentences[NMEACmd];
				handler.HandleSentence(this, fields, true);
			} catch {
				throw new Exception("Error parsing NMEA sentence: " + fields[0]);
			}
		}
	}
}
