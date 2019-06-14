using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.DMECoverage
{
	class DMEListSerialiser
	{
		[Flags]
		private enum DMEFields
		{
			None = 0,
			CallSign = 1,
			X = 2,
			Y = 4,
			Z = 8
		}

		private string _fileName;
		private List<NavaidType> _dmeList;
		int _line;

		public DMEListSerialiser(string fileName)
		{
			_fileName = fileName;
		}

		public int Count { get; set; }

		public NavaidType this[int i]
		{
			get { return _dmeList[i]; }
			set { _dmeList[i] = value; }
		}

		private string[] getInputLine(StreamReader stReader)
		{
			if (stReader.EndOfStream)
				return new string[0];

			string[] result;

			do
			{
				result = stReader.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				_line++;
			}
			while (result.Length == 0 && !stReader.EndOfStream);

			if (result.Length == 1)
			{
				int p = result[0].IndexOf('=');
				if (p > 0)
				{
					System.Array.Resize<string>(ref result, result.Length + 2);
					string sTmp = result[0];
					result[0] = sTmp.Substring(0, p);
					result[1] = "=";
					result[2] = sTmp.Substring(p + 1, sTmp.Length - p - 1);
				}
			}

			return result;
		}

		internal void Read(List<NavaidType> extraDMEList)
		{
			if (!File.Exists(_fileName))
				return;

			using (var stReader = new StreamReader(_fileName))
			{
				//var cultureInfo = new CultureInfo("en-US");

				bool error = false;
				_line = 0;

				string[] input = getInputLine(stReader);

				while (!stReader.EndOfStream)
				{
					if (input.Length != 2 || input[0] != "DME")
					{
						MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					}

					string CallSign = input[1];
					
					DMEFields loadedFields = DMEFields.CallSign;

					double x = 0, y = 0, z = 0, fTmp;

					while (!stReader.EndOfStream)
					{
						input = getInputLine(stReader);

						if (input.Length != 3 || input[1] != "=")
						{
							if ((loadedFields & DMEFields.X) == DMEFields.X && (loadedFields & DMEFields.Y) == DMEFields.Y && (loadedFields & DMEFields.Z) == DMEFields.Z)
								break;

							MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}\n\rAssgnment expected!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
							error = true;
							break;
						}

						if(!double.TryParse(input[2], out fTmp))
						{
							MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}\n\rNumber expected!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
							error = true;
							break;
						}

						if (input[0].ToLower() == "x")
						{
							if ((loadedFields & DMEFields.X) == DMEFields.X)
							{
								MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}\n\rMultiple definition of 'X' field!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
								error = true;
								break;
							}

							x = fTmp;
							loadedFields |= DMEFields.X;
						}
						else if (input[0].ToLower() == "y")
						{
							if ((loadedFields & DMEFields.Y) == DMEFields.Y)
							{
								MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}\n\rMultiple definition of 'Y' field!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
								error = true;
								break;
							}

							y = fTmp;
							loadedFields |= DMEFields.Y;
						}
						else if (input[0].ToLower() == "z")
						{
							if ((loadedFields & DMEFields.Z) == DMEFields.Z)
							{
								MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}\n\rMultiple definition of 'Z' field!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
								error = true;
								break;
							}

							z = fTmp;
							loadedFields |= DMEFields.Z;
						}
						else
						{
							MessageBox.Show(string.Format("Invalid file: {0}.\n\rUnknown symbol at line {1}\n\rIgnored!", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}

					if (error)
						break;

					if ((loadedFields & DMEFields.X) != DMEFields.X || (loadedFields & DMEFields.Y) != DMEFields.Y)
					{
						MessageBox.Show(string.Format("Corrupted file: {0}.\n\rError at line {1}", _fileName, _line), null, MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					}

					NavaidType currDME = new NavaidType();

					currDME.CallSign = CallSign; 
					currDME.TypeCode = eNavaidType.DME;
					currDME.pPtGeo = new Point(x, y);
					currDME.pPtGeo.Z = z;
					currDME.Identifier = Guid.NewGuid();
					currDME.pPtPrj = GlobalVars.pspatialReferenceOperation.ToPrj<Point>(currDME.pPtGeo);
					//if ((loadedFields & DMEFields.Z) == DMEFields.Z)
					//	currDME.MagVar = MagVar;

					extraDMEList.Add(currDME);
				}
			}

			_dmeList = extraDMEList;
		}

		internal void Save(List<NavaidType> extraDMEList)
		{
			_dmeList = extraDMEList;

			using (var stWriter = new StreamWriter(_fileName))
			{
				//var cultureInfo = new CultureInfo("en-US");

				foreach (NavaidType currDME in extraDMEList)
				{
					stWriter.WriteLine(string.Format("DME {0}", currDME.CallSign));
					stWriter.WriteLine(string.Format("X = {0}", currDME.pPtGeo.X));
					stWriter.WriteLine(string.Format("Y = {0}", currDME.pPtGeo.Y));
					stWriter.WriteLine(string.Format("Z = {0}", currDME.pPtGeo.Z));
					stWriter.WriteLine();
				}
			}
		}
	}
}
