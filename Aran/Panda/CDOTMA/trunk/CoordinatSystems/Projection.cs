using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CDOTMA.CoordinatSystems
{
	public enum CoordinateSystemType
	{
		Geographic,		//Geographic Coordinate System
		Projected		//Projected Coordinate Systems
	}

	interface IAssignable
	{
		void Assign(object another);
		void AssignTo(object another);
	}

	public struct Unit : IComparable, ICloneable, IAssignable
	{
		public string Name;
		public double Multiplier;

		public Unit(string name, double multiplier)
		{
			Name = name;
			Multiplier = multiplier;
		}

		public override string ToString()
		{
			return string.Format("UNIT[\"{0}\", {1}]", Name, Multiplier);
		}

		public void Parse(string input)
		{
			//char[] seperators = new char[] { ' ', '\t', ',', ':', '=' };

			char[] seperator = new char[] { '['};
			string[] splitted = input.Split(seperator);

			if (splitted.Length != 2 || splitted[0].ToUpper() != "UNIT")
				throw new Exception("Invalid unit format.");

			seperator[0] = '"';
			splitted = splitted[1].Split(seperator);

			if (splitted.Length != 3 || splitted[0] != "")
				throw new Exception("Invalid unit format.");
			Name = splitted[1];

			seperator[0] = ']';
			splitted = splitted[2].Split(seperator);
			splitted[0] = splitted[0].Replace(',', ' ');

			if (splitted.Length != 2 || !double.TryParse(splitted[0], out Multiplier))
				throw new Exception("Invalid unit format.");
		}

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Unit))
				return -1;
			Unit another = (Unit)obj;

			int result = Name.CompareTo(another.Name);
			if (result != 0)
				return result;

			return Multiplier.CompareTo(another.Multiplier);
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}
	}

	public struct Parameter : IComparable, ICloneable, IAssignable
	{
		public string Name;
		//public Unit unit;
		public double Value;

		public override string ToString()
		{
			return string.Format("PARAMETER[\"{0}\", {1}]", Name, Value);
		}

		public Parameter(string name, double value)
		{
			Name = name;
			Value = value;
		}

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Parameter))
				return -1;
			Parameter another = (Parameter)obj;

			int result = Name.CompareTo(another.Name);
			if (result != 0)
				return result;

			return Value.CompareTo(another.Value);
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}
	}

	public struct Primem : IComparable, ICloneable, IAssignable
	{
		public string Name;
		//public Unit unit;
		public double Value;

		public override string ToString()
		{
			return string.Format("PRIMEM[\"{0}\", {1}]", Name, Value);
		}

		public Primem(string name, double value)
		{
			Name = name;
			Value = value;
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Primem))
				return -1;
			Primem another = (Primem)obj;

			int result = Name.CompareTo(another.Name);
			if (result != 0)
				return result;

			return Value.CompareTo(another.Value);
		}

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}
	}

	public class Spheroid : IComparable, ICloneable, IAssignable
	{
		public Spheroid()
		{
			_name = "WGS_1984";
			_a = 6378137.0;
			_f = 298.257223563;
		}

		public Spheroid(string name, double radius, double invFlat)
		{
			_name = name;
			_a = radius;
			_f = invFlat;
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Spheroid))
				return -1;
			Spheroid another = (Spheroid)obj;

			int result = _name.CompareTo(another._name);
			if (result != 0)
				return result;

			result = _a.CompareTo(another._a);
			if (result != 0)
				return result;

			return _f.CompareTo(another._f);
		}

		public override string ToString()
		{
			return string.Format("SPHEROID[\"{0}\", {1}, {2}]", _name, _a, _f);
		}

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}

		public string Name { get { return _name; } }
		public double SemiMajorAxis { get { return _a; } }
		public double InversFlattening { get { return _f; } }

		string _name;
		double _a;
		double _f;
	}

	public class Datum : IComparable, ICloneable, IAssignable
	{
		public Datum()
		{
			_name = "D_WGS_1984";
			_spheroid = new Spheroid();
		}

		public Datum(string name, Spheroid spheroid)
		{
			_name = name;
			_spheroid = spheroid;
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Datum))
				return -1;
			Datum another = (Datum)obj;

			int result = _name.CompareTo(another._name);
			if (result != 0)
				return result;

			return _spheroid.CompareTo(another._spheroid);
		}

		public override string ToString()
		{
			return string.Format("DATUM[\"{0}\", {1}]", _name, _spheroid);
		}

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}

		public string Name { get { return _name; } }
		public Spheroid spheroid { get { return _spheroid; } }

		string _name;
		Spheroid _spheroid;
	}

	public class CoordinatSystem
	{
		protected CoordinateSystemType _type;
		public CoordinateSystemType CSType { get { return _type; } }

		public void SaveToStream(Stream strm)
		{
			StreamWriter tw = new StreamWriter(strm);
			tw.Write(this.ToString());
			tw.Close();
			tw.Dispose();
		}

		public static CoordinatSystem LoadFromStream(Stream strm)
		{
			return ProjectionLoader.Read(strm);
		}
	}

	public class GeographicCoordinatSystem : CoordinatSystem, IComparable, ICloneable, IAssignable
	{
		public GeographicCoordinatSystem()
		{
			_name = "GCS_WGS_1984";
			_datum = new Datum();

			_primem.Name = "Greenwich";
			_primem.Value = 0.0;

			_unit.Name = "Degree";
			_unit.Multiplier = 0.0174532925199433;
			base._type = CoordinateSystemType.Geographic;
		}

		public GeographicCoordinatSystem(string name, Datum dtm, Primem prmm, Unit units)
		{
			_name = name;
			_datum = dtm;
			_primem = prmm;
			_unit = units;
			base._type = CoordinateSystemType.Geographic;
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is GeographicCoordinatSystem))
				return -1;
			GeographicCoordinatSystem another = (GeographicCoordinatSystem)obj;

			int result = _name.CompareTo(another._name);
			if (result != 0)
				return result;

			result = _datum.CompareTo(another._datum);
			if (result != 0)
				return result;

			result = _primem.CompareTo(another._primem);
			if (result != 0)
				return result;

			return _unit.CompareTo(another._unit);
		}

		public override string ToString()
		{
			return string.Format("GEOGCS[\"{0}\", {1}, {2}, {3}]", _name, _datum, _primem, _unit);
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}

		public string Name { get { return _name; } }
		public Datum datum { get { return _datum; } }
		public Primem primem { get { return _primem; } }
		public Unit unit { get { return _unit; } }

		string _name;			//	GEOGCS PROJCS	"GCS_WGS_1984", "WGS_1984_Transverse_Mercator"
		Datum _datum;
		Primem _primem;			//	PRIMEM["Greenwich",0.0],
		Unit _unit;				//	["Degree",0.0174532925199433]
	}

	public class ProjectedCoordinatSystem : CoordinatSystem, IComparable, ICloneable, IAssignable
	{
		public ProjectedCoordinatSystem()
		{
			_name = "WGS_1984_Transverse_Mercator";
			_projectName = "Transverse_Mercator";
			_coordinatSystem = new GeographicCoordinatSystem();

			base._type = CoordinateSystemType.Projected;

			_unit = new Unit("Meter", 1.0);
			_falseEasting = new Parameter("False_Easting", 500000.0);
			_falseNorthing = new Parameter("False_Northing", 0.0);

			_centralMeridian = new Parameter("Central_Meridian", 0.0);
			_scaleFactor = new Parameter("Scale_Factor", 0.9996);

			_latitudeOfOrigin = new Parameter("Latitude_Of_Origin", 0.0);

			CreateParamList();
		}

		public ProjectedCoordinatSystem(string name, string prjName, GeographicCoordinatSystem baseSystem, Unit unit, double centralMeridian, double scaleFactor = 0.9996, double falseEasting = 500000.0, double falseNorthing = 0.0, double latitudeOfOrigin = 0.0)
		{
			_name = name;
			_projectName = prjName;
			_coordinatSystem = baseSystem;
			base._type = CoordinateSystemType.Projected;

			_unit = unit;																//["Meter",1.0]
			_falseEasting = new Parameter("False_Easting", falseEasting);				//["False_Easting",500000.0],
			_falseNorthing = new Parameter("False_Northing", falseNorthing);			//["False_Northing",-6000000.0],
			_centralMeridian = new Parameter("Central_Meridian", centralMeridian);		//["Central_Meridian",24.0],
			_scaleFactor = new Parameter("Scale_Factor", scaleFactor);					//["Scale_Factor",0.9996],
			_latitudeOfOrigin = new Parameter("Latitude_Of_Origin", latitudeOfOrigin);	//["Latitude_Of_Origin",0.0],
			CreateParamList();
		}

		void CreateParamList()
		{
			_parameters = new List<Parameter> ();
			_parameters.Add(_falseEasting);
			_parameters.Add(_falseNorthing);
			_parameters.Add(_centralMeridian);
			_parameters.Add(_scaleFactor);
			_parameters.Add(_latitudeOfOrigin);
		}

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is ProjectedCoordinatSystem))
				return -1;
			ProjectedCoordinatSystem another = (ProjectedCoordinatSystem)obj;

			int result = _name.CompareTo(another._name);
			if (result != 0)
				return result;

			result = _projectName.CompareTo(another._projectName);
			if (result != 0)
				return result;

			result = _coordinatSystem.CompareTo(another._coordinatSystem);
			if (result != 0)
				return result;

			result = _falseEasting.CompareTo(another._falseEasting);
			if (result != 0)
				return result;

			result = _falseNorthing.CompareTo(another._falseNorthing);
			if (result != 0)
				return result;

			result = _centralMeridian.CompareTo(another._centralMeridian);
			if (result != 0)
				return result;

			result = _scaleFactor.CompareTo(another._scaleFactor);
			if (result != 0)
				return result;

			result = _latitudeOfOrigin.CompareTo(another._latitudeOfOrigin);
			if (result != 0)
				return result;

			return _unit.CompareTo(another._unit);
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public void Assign(object another)
		{
			throw new NotImplementedException();
		}

		public void AssignTo(object another)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return string.Format("PROJCS[\"{0}\", {1}, PROJECTION[\"{2}\"], {3}, {4}, {5}, {6}, {7}, {8}]",
				_name, _coordinatSystem, _projectName, _falseEasting, _falseNorthing,
				_centralMeridian, _scaleFactor, _latitudeOfOrigin, _unit);
		}

		public Unit unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		public double FalseEasting
		{
			get { return _falseEasting.Value; }
			set { _falseEasting.Value = value; }
		}

		public double FalseNorthing
		{
			get { return _falseNorthing.Value; }
			set { _falseNorthing.Value = value; }
		}

		public double CentralMeridian
		{
			get { return _centralMeridian.Value; }
			set { _centralMeridian.Value = value; }
		}

		public double ScaleFactor
		{
			get { return _scaleFactor.Value; }
			set { _scaleFactor.Value = value; }
		}

		public double LatitudeOfOrigin
		{
			get { return _latitudeOfOrigin.Value; }
			set { _latitudeOfOrigin.Value = value; }
		}

		public List<Parameter> Parameters { get { return _parameters; } }
		public string Name { get { return _name; } }
		public string projectionName { get { return _projectName; } }
		public GeographicCoordinatSystem baseCoordinatSystem { get { return _coordinatSystem; } }

		string _name;							//	"WGS_1984_Transverse_Mercator"
		string _projectName;					//	"Transverse_Mercator",
		List<Parameter> _parameters;

		GeographicCoordinatSystem _coordinatSystem;
		Parameter _falseEasting;				//	["False_Easting",500000.0],
		Parameter _falseNorthing;				//	["False_Northing",-6000000.0],
		Parameter _centralMeridian;				//	["Central_Meridian",24.0],
		Parameter _scaleFactor;					//	["Scale_Factor",0.9996],
		Parameter _latitudeOfOrigin;			//	["Latitude_Of_Origin",0.0],
		Unit _unit;								//  ["Meter", 1.0]
	}

}
