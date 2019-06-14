//3186801021

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Geometries;
using System.IO;
using System.Windows.Forms;

namespace TestAranGdbProvider
{
	public class GDBProvider
	{
		private GDBProvider _gdbProvider;
		private List<Feature> _featureList;
		private string _parseFolder = @"D:\WORK\Err\Latv\Area3\";

		public GDBProvider()
		{
			_gdbProvider = this;
			_featureList = new List<Feature>();
		}

		public void Open()
		{
			OrganisationAuthority org = CreateOrganisation();
			_gdbProvider.SetFeature(org);
			AirportHeliport ah = CreateAirport(org.Identifier);
			_gdbProvider.SetFeature(ah);
			Runway runway = CreateRunway(ah.Identifier);
			_gdbProvider.SetFeature(runway);
			InsertRunwayGroup(runway.Identifier);

			List<VerticalStructure> vsList = ParseObstacleLayer(_parseFolder + "EVRA_SID-10-03-11.mdb", org.Identifier);
			foreach (var item in vsList)
				_gdbProvider.SetFeature(item);

			InsertComplexVerticalStructure(_parseFolder + "pgon.txt", org.Identifier);

			InsertAirspaceFIR();
			//InsertAirspaceFIR(org.Identifier, "D:\\WORK\\Err\\Latv\\Area3\\01Mar2011.mdb");

			InsertAirspaceTMA1();
			InsertAirspaceTMA2();
			InsertAirspaceP();

			InsertTaxiway(ah.Identifier, _parseFolder + "OODB_Latvia-Nigeria.mdb");
			InsertApron(ah.Identifier, _parseFolder + "OODB_Latvia-Nigeria.mdb");
		}

		public List<OrganisationAuthority> GetOrganisationList()
		{
			List<OrganisationAuthority> list = new List<OrganisationAuthority>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.OrganisationAuthority)
					list.Add((OrganisationAuthority)feature);
			}

			return list;
		}

		public List<AirportHeliport> GetAirportList(Guid organisationIdentifier)
		{
			List<AirportHeliport> list = new List<AirportHeliport>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
					list.Add((AirportHeliport)feature);
			}

			return list;
		}

		public List<Airspace> GetAirspaceList(Guid organisationIdentifier)
		{
			List<Airspace> list = new List<Airspace>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.Airspace)
				{
					list.Add((Airspace)feature);
				}
			}

			return list;
		}

		public List<Runway> GetRunwayList(Guid airportIdentifier)
		{
			List<Runway> list = new List<Runway>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.Runway)
					list.Add((Runway)feature);
			}

			return list;
		}

		public List<RunwayDirection> GetRunwayDirectionList(Guid runwayIdentifier)
		{
			List<RunwayDirection> list = new List<RunwayDirection>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.RunwayDirection)
					list.Add((RunwayDirection)feature);
			}

			return list;
		}

		public List<RunwayCentrelinePoint> GetRunwayCentrelinePointList(Guid rwyDirIdentifier)
		{
			List<RunwayCentrelinePoint> list = new List<RunwayCentrelinePoint>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint)
				{
					RunwayCentrelinePoint rcp = (RunwayCentrelinePoint)feature;
					if (rcp.OnRunway.Identifier == rwyDirIdentifier)
						list.Add(rcp);
				}
			}

			return list;
		}

		public List<RunwayProtectArea> GetRunwayProtectAreaList(Guid rwyDirIdentifier)
		{
			List<RunwayProtectArea> list = new List<RunwayProtectArea>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.RunwayProtectArea)
				{
					RunwayProtectArea rpa = (RunwayProtectArea)feature;
					if (rpa.ProtectedRunwayDirection.Identifier == rwyDirIdentifier)
						list.Add(rpa);
				}
			}

			return list;
		}

		public List<VerticalStructure> GetVerticalStructureList(Guid organisationIdentifier)
		{
			List<VerticalStructure> list = new List<VerticalStructure>();
			//int k;

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.VerticalStructure)
				{
					VerticalStructure vs = (VerticalStructure)feature;
					List<FeatureRefObject> orgList = vs.HostedOrganisation;

					if (orgList.Where(fro =>
							fro.Feature != null &&
							fro.Feature.Identifier == organisationIdentifier).Count() > 0)
					{
						list.Add(vs);
					}
				}
			}

			return list;
		}

		public List<Taxiway> GetTaxiwayList(Guid airportIdentifier)
		{
			List<Taxiway> list = new List<Taxiway>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.Taxiway
					&& ((Taxiway)feature).AssociatedAirportHeliport.Identifier == airportIdentifier)
				{
					list.Add(((Taxiway)feature));
				}
			}

			return list;
		}

		public List<TaxiwayElement> GetTaxiwayElementList(Guid taxiwayIdentifier)
		{
			List<TaxiwayElement> list = new List<TaxiwayElement>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.TaxiwayElement
					&& ((TaxiwayElement)feature).AssociatedTaxiway.Identifier == taxiwayIdentifier)
				{
					list.Add(((TaxiwayElement)feature));
				}
			}

			return list;
		}

		public List<Apron> GetApronList(Guid airportIdentifier)
		{
			List<Apron> list = new List<Apron>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.Apron
					&& ((Apron)feature).AssociatedAirportHeliport.Identifier == airportIdentifier)
				{
					list.Add(((Apron)feature));
				}
			}

			return list;
		}

		public List<ApronElement> GetApronElementList(Guid apronIdentifier)
		{
			List<ApronElement> list = new List<ApronElement>();

			foreach (Feature feature in _featureList)
			{
				if (feature.FeatureType == Aran.Aim.FeatureType.ApronElement
					&& ((ApronElement)feature).AssociatedApron.Identifier == apronIdentifier)
				{
					list.Add(((ApronElement)feature));
				}
			}

			return list;
		}

		private OrganisationAuthority CreateOrganisation()
		{
			OrganisationAuthority org = new OrganisationAuthority();
			org.Identifier = Guid.NewGuid();
			org.Designator = "EV";
			org.Name = "LATVIA";
			return org;
		}

		private AirportHeliport CreateAirport(Guid orgIdentifier)
		{
			AirportHeliport ah = new AirportHeliport();
			ah.Identifier = Guid.NewGuid();
			ah.ResponsibleOrganisation = new AirportHeliportResponsibilityOrganisation();
			ah.ResponsibleOrganisation.TheOrganisationAuthority = new FeatureRef(orgIdentifier);

			ah.Designator = "EVRA";
			ah.Name = "RIGA";
			ah.FieldElevation = new ValDistanceVertical(34, UomDistanceVertical.FT);
			ah.Type = CodeAirportHeliport.AD;

			ah.ARP = GetElevatedPoint(23.9712, 56.923663889, 34, UomDistanceVertical.FT);
			return ah;
		}

		private Runway CreateRunway(Guid airportIdentifier)
		{
			Runway runway = new Runway();
			runway.Identifier = Guid.NewGuid();
			runway.AssociatedAirportHeliport = new FeatureRef(airportIdentifier);

			runway.Designator = "18/36";
			runway.Type = CodeRunway.RWY;
			runway.NominalLength = new ValDistance(3200, UomDistance.M);
			return runway;
		}

		private void InsertRunwayGroup(Guid runwayIdentifier)
		{
			#region RunwayDirection => 18

			RunwayDirection rwyDir18 = new RunwayDirection();
			rwyDir18.Identifier = Guid.NewGuid();
			rwyDir18.UsedRunway = new FeatureRef(runwayIdentifier);
			rwyDir18.Designator = "18";
			_gdbProvider.SetFeature(rwyDir18);

			RunwayCentrelinePoint rcp = null;

			#region RunwayCentrelinePoint => START

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.START;
			rcp.Location = GetElevatedPoint(23.973075, 56.935069444, 31, UomDistanceVertical.FT);

			RunwayDeclaredDistance rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.ASDA;
			RunwayDeclaredDistanceValue rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.TORA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.TODA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.LDA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			_gdbProvider.SetFeature(rcp);

			#endregion

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.THR;
			rcp.Location = GetElevatedPoint(23.973075, 56.935069444, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.972338889, 56.930561111, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.971605556, 56.926125, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.9712, 56.923663889, 34, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.971125, 56.923205556, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.970675, 56.920480556, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.970236111, 56.917819444, 32, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir18.Identifier);
			rcp.Role = CodeRunwayPointRole.END;
			rcp.Location = GetElevatedPoint(23.968355556, 56.90645, 36, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			#endregion

			#region RunwayDirection => 36

			RunwayDirection rwyDir36 = new RunwayDirection();
			rwyDir36.Identifier = Guid.NewGuid();
			rwyDir36.UsedRunway = new FeatureRef(runwayIdentifier);
			rwyDir36.Designator = "36";

			_gdbProvider.SetFeature(rwyDir36);

			#region RunwayCentrelinePoint => START

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.START;
			rcp.Location = GetElevatedPoint(23.968355556, 56.90645, 36, UomDistanceVertical.FT);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.ASDA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.TORA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.TODA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			rdd = new RunwayDeclaredDistance();
			rdd.Type = CodeDeclaredDistance.LDA;
			rddValue = new RunwayDeclaredDistanceValue();
			rddValue.Distance = new ValDistance(3200, UomDistance.M);
			rdd.DeclaredValue.Add(rddValue);
			rcp.AssociatedDeclaredDistance.Add(rdd);

			_gdbProvider.SetFeature(rcp);

			#endregion

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.THR;
			rcp.Location = GetElevatedPoint(23.968355556, 56.90645, 36, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.970236111, 56.917819444, 32, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.970675, 56.920480556, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.971125, 56.923205556, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.9712, 56.923663889, 34, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.971605556, 56.926125, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.MID;
			rcp.Location = GetElevatedPoint(23.972338889, 56.930561111, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			rcp = new RunwayCentrelinePoint();
			rcp.Identifier = Guid.NewGuid();
			rcp.OnRunway = new FeatureRef(rwyDir36.Identifier);
			rcp.Role = CodeRunwayPointRole.END;
			rcp.Location = GetElevatedPoint(23.973075, 56.935069444, 31, UomDistanceVertical.FT);
			_gdbProvider.SetFeature(rcp);

			#endregion

			InsertRunwayProtectArea(rwyDir18.Identifier);
			InsertRunwayProtectArea(rwyDir36.Identifier);
		}

		private void InsertRunwayProtectArea(Guid rwyDirIdentifier)
		{
			RunwayProtectArea rpa = new RunwayProtectArea();
			rpa.Identifier = Guid.NewGuid();
			rpa.ProtectedRunwayDirection = new FeatureRef(rwyDirIdentifier);

			rpa.Type = CodeRunwayProtectionArea.CWY;
			rpa.Length = new ValDistance(0, UomDistance.M);

			_gdbProvider.SetFeature(rpa);
		}

		private ElevatedPoint GetElevatedPoint(double x, double y, double z, UomDistanceVertical zUom)
		{
			ElevatedPoint ep = new ElevatedPoint();
			ep.Geo.SetCoords(x, y);
			ep.Elevation = new ValDistanceVertical(z, zUom);
			return ep;
		}

		//private void InsertAirspaceFIR(Guid airportIdentifier, string fileName)
		//{
		//    ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wsFact = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory();
		//    var ws = wsFact.OpenFromFile(fileName, 0);
		//    var featWs = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)ws;
		//    var featClass = featWs.OpenFeatureClass("Airspace");
		//    var featCursor = featClass.Search(null, false);

		//    int i_Name = featClass.FindField("txtName");
		//    int i_Type = featClass.FindField("Type");

		//    string constStr = "FIR";

		//    ESRI.ArcGIS.Geodatabase.IFeature feature;
		//    object tmpValue;
		//    while ((feature = featCursor.NextFeature()) != null)
		//    {
		//        ESRI.ArcGIS.Geometry.IGeometry geom = feature.Shape;
		//        string name = null;
		//        string type = null;

		//        if (geom.GeometryType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
		//            continue;

		//        tmpValue = feature.get_Value(i_Name);

		//        if (tmpValue != DBNull.Value)
		//        {
		//            name = tmpValue.ToString();
		//            if (name.EndsWith(constStr))
		//            {
		//                name = name.Substring(0, name.Length - constStr.Length);
		//            }
		//        }

		//        if (string.IsNullOrEmpty(name))
		//            continue;
		//        if(name !=  "RIGA")
		//            continue;

		//        //tmpValue = feature.get_Value(i_Type);
		//        //if (tmpValue != DBNull.Value)
		//        //{
		//        //    type = tmpValue.ToString();
		//        //}

		//        Airspace airspace = new Airspace();
		//        airspace.Identifier = Guid.NewGuid();
		//        airspace.Designator = "RIGA FIR";
		//        airspace.Name = "Riga FIR (Test)";
		//        airspace.Type = CodeAirspace.FIR;

		//        AirspaceGeometryComponent agc = new AirspaceGeometryComponent();
		//        agc.TheAirspaceVolume = new AirspaceVolume();

		//        //agc.TheAirspaceVolume.HorizontalProjection = (Surface)ParseGeomText(_parseFolder + "AirspacesTMA1.txt");

		//        airspace.GeometryComponent.Add(agc);

		//        _gdbProvider.SetFeature(airspace);
		//    }

			//========================================
		//}

		private void InsertAirspaceFIR()
		{
			Airspace airspace = new Airspace();
			airspace.Identifier = Guid.NewGuid();
			airspace.Designator = "RIGA FIR";
			airspace.Name = "Riga FIR (Test)";
			airspace.Type = CodeAirspace.FIR;

			AirspaceGeometryComponent agc = new AirspaceGeometryComponent();
			agc.TheAirspaceVolume = new AirspaceVolume();
			agc.TheAirspaceVolume.HorizontalProjection = (Surface)ParseGeomText(_parseFolder + "AirspacesFIR.txt");

			airspace.GeometryComponent.Add(agc);

			_gdbProvider.SetFeature(airspace);
		}

		private void InsertAirspaceTMA1()
		{
			Airspace airspace = new Airspace();
			airspace.Identifier = Guid.NewGuid();
			airspace.Designator = "RIGA TMA";
			airspace.Name = "Riga TMA (Test)";
			airspace.Type = CodeAirspace.TMA;

			AirspaceGeometryComponent agc = new AirspaceGeometryComponent();
			agc.TheAirspaceVolume = new AirspaceVolume();
			agc.TheAirspaceVolume.HorizontalProjection = (Surface)ParseGeomText(_parseFolder + "AirspacesTMA1.txt");

			airspace.GeometryComponent.Add(agc);

			_gdbProvider.SetFeature(airspace);
		}

		private void InsertAirspaceTMA2()
		{
			Airspace airspace = new Airspace();
			airspace.Identifier = Guid.NewGuid();
			airspace.Designator = "LIEPA TMA";
			airspace.Name = "Liepa TMA (Test)";
			airspace.Type = CodeAirspace.TMA;

			AirspaceGeometryComponent agc = new AirspaceGeometryComponent();
			agc.TheAirspaceVolume = new AirspaceVolume();
			agc.TheAirspaceVolume.HorizontalProjection = (Surface)ParseGeomText(_parseFolder + "AirspacesTMA2.txt");

			airspace.GeometryComponent.Add(agc);
			_gdbProvider.SetFeature(airspace);
		}

		private void InsertAirspaceP()
		{
			Airspace airspace = new Airspace();
			airspace.Identifier = Guid.NewGuid();
			airspace.Designator = "RIGA P";
			airspace.Name = "Riga P (Test)";
			airspace.Type = CodeAirspace.P;

			AirspaceGeometryComponent agc = new AirspaceGeometryComponent();
			agc.TheAirspaceVolume = new AirspaceVolume();
			agc.TheAirspaceVolume.HorizontalProjection = (Surface)ParseGeomText(_parseFolder + "AirspacesP.txt");

			airspace.GeometryComponent.Add(agc);
			_gdbProvider.SetFeature(airspace);
		}

		private AObject ParseGeomText(string fileName)
		{
			System.IO.FileStream fs = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(fs);

			string line = sr.ReadLine();
			if (line == null)
				return null;

			string geomType = line;

			line = sr.ReadLine();
			if (line == null)
				return null;

			List<Aran.Geometries.Point> pointList = new List<Aran.Geometries.Point>();
			while ((line = sr.ReadLine()) != null)
			{
				if (line == "END")
					break;
				string[] sa = line.Split(' ');
				double x = double.Parse(sa[1]);
				double y = double.Parse(sa[2]);
				pointList.Add(new Aran.Geometries.Point(x, y));
			}
			fs.Close();

			if (geomType == "Polygon")
			{
				Surface surface = new Surface();
				MultiPolygon mp = surface.Geo;
				Aran.Geometries.Polygon polygon = new Aran.Geometries.Polygon();
				mp.Add(polygon);

				foreach (Aran.Geometries.Point pt in pointList)
					polygon.ExteriorRing.Add(pt);

				return surface;
			}
			else if (geomType == "Polyline")
			{
				Curve curve = new Curve();
				MultiLineString mls = curve.Geo;
				LineString lineStr = new LineString();
				mls.Add(lineStr);

				foreach (Aran.Geometries.Point pt in pointList)
					lineStr.Add(pt);

				return curve;
			}

			return null; ;
		}

		private List<VerticalStructure> ParseObstacleLayer(string fileName, Guid organisationIdentifier)
		{
			ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wsFact = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory();
			ESRI.ArcGIS.Geodatabase.IWorkspace ws = wsFact.OpenFromFile(fileName, 0);
			ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featWs = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)ws;
			ESRI.ArcGIS.Geodatabase.IFeatureClass featClass = featWs.OpenFeatureClass("Obstacle");
			ESRI.ArcGIS.Geodatabase.IFeatureCursor featCursor = featClass.Search(null, false);

			int i_txtName = featClass.FindField("txtName");
			int i_txtDescrType = featClass.FindField("txtDescrType");
			int i_codeGroup = featClass.FindField("codeGroup");
			int i_codeLgt = featClass.FindField("codeLgt");
			int i_valElev = featClass.FindField("valElev");
			int i_valHgt = featClass.FindField("valHgt");
			int i_valElevAccuracy = featClass.FindField("valElevAccuracy");
			int i_valGeoAccuracy = featClass.FindField("valGeoAccuracy");

			int i_UomDistanceVertical = featClass.FindField("uomDistVer");
			int i_UomGeoAccuracy = featClass.FindField("uomGeoAccuracy");

			List<VerticalStructure> vsList = new List<VerticalStructure>();

			ESRI.ArcGIS.Geodatabase.IFeature feature;
			object tmpValue;
			while ((feature = featCursor.NextFeature()) != null)
			{
				UomDistance uomGeoAccuracy;
				UomDistance uomDist;
				UomDistanceVertical uomDistVer;

				VerticalStructure vs = new VerticalStructure();
				vs.Identifier = Guid.NewGuid();
				vs.Name = feature.get_Value(i_txtName).ToString();

				vs.Type = ConvertTo_CodeVerticalStructure(feature.get_Value(i_txtDescrType).ToString());
				vs.Group = (feature.get_Value(i_codeGroup).ToString() == "Y");
				vs.Lighted = (feature.get_Value(i_codeLgt).ToString() == "Y");

				ESRI.ArcGIS.Geometry.IPoint obsPoint = feature.Shape as ESRI.ArcGIS.Geometry.IPoint;
				VerticalStructurePart vsPart = new VerticalStructurePart();
				vsPart.HorizontalProjection = new VerticalStructurePartGeometry();

				tmpValue = feature.get_Value(i_UomDistanceVertical);
				uomDist = UomDistance.FT;
				uomDistVer = UomDistanceVertical.FT;

				if (tmpValue != System.DBNull.Value)
				{
					if (tmpValue.ToString() == "M")
					{
						uomDist = UomDistance.M;
						uomDistVer = UomDistanceVertical.M;
					}
				}

				tmpValue = feature.get_Value(i_UomGeoAccuracy);
				uomGeoAccuracy = UomDistance.M;
				if (tmpValue != System.DBNull.Value)
				{
					if (tmpValue.ToString() == "KM")
						uomGeoAccuracy = UomDistance.KM;
					else if (tmpValue.ToString() == "NM")
						uomGeoAccuracy = UomDistance.NM;
					else if (tmpValue.ToString() == "FT")
						uomGeoAccuracy = UomDistance.FT;
				}

				double z = Convert.ToDouble(feature.get_Value(i_valElev));
				vsPart.HorizontalProjection.Location = GetElevatedPoint(obsPoint.X, obsPoint.Y, z, uomDistVer);

				tmpValue = feature.get_Value(i_valHgt);
				if (tmpValue != System.DBNull.Value)
					vsPart.VerticalExtent = new ValDistance((double)tmpValue, uomDist);

				tmpValue = feature.get_Value(i_valElevAccuracy);
				if (tmpValue != System.DBNull.Value)
				{
					vsPart.HorizontalProjection.Location.VerticalAccuracy = new ValDistance((double)tmpValue, uomDist);
				}

				tmpValue = feature.get_Value(i_valGeoAccuracy);
				if (tmpValue != System.DBNull.Value)
				{
					vsPart.HorizontalProjection.Location.HorizontalAccuracy = new ValDistance((double)tmpValue, uomGeoAccuracy);
				}

				vs.Part.Add(vsPart);

				FeatureRefObject featRefObj = new FeatureRefObject();
				featRefObj.Feature = new FeatureRef(organisationIdentifier);
				vs.HostedOrganisation.Add(featRefObj);

				vsList.Add(vs);
			}

			return vsList;
		}

		private CodeVerticalStructure? ConvertTo_CodeVerticalStructure(string txtDescrType)
		{
			txtDescrType = txtDescrType.ToLower();

			if (txtDescrType == "antenna")
				return CodeVerticalStructure.ANTENNA;

			if (txtDescrType == "building")
				return CodeVerticalStructure.BUILDING;

			if (txtDescrType == "chimney")
				return null;

			if (txtDescrType == "crane")
				return CodeVerticalStructure.CRANE;

			if (txtDescrType == "mast")
				return CodeVerticalStructure.POLE;

			if (txtDescrType.StartsWith("tower"))
				return CodeVerticalStructure.TOWER;

			if (txtDescrType.StartsWith("tree"))
				return CodeVerticalStructure.TREE;

			if (txtDescrType.StartsWith("Wind"))
				return CodeVerticalStructure.WINDMILL;

			return null;
		}

		private void SetFeature(Feature feature)
		{
			_featureList.Add(feature);
		}

		private void InsertTaxiway(Guid airportIdentifier, string fileName)
		{
			ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wsFact = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory();
			var ws = wsFact.OpenFromFile(fileName, 0);
			var featWs = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)ws;
			var featClass = featWs.OpenFeatureClass("TaxiElement");
			var featCursor = featClass.Search(null, false);

			int i_Name = featClass.FindField("Name");
			int i_Type = featClass.FindField("Type");

			string constStr = "EVRA TaxiElement ";

			ESRI.ArcGIS.Geodatabase.IFeature feature;
			object tmpValue;
			while ((feature = featCursor.NextFeature()) != null)
			{
				ESRI.ArcGIS.Geometry.IGeometry geom = feature.Shape;
				string taxiwayName = null;
				string type = null;

				if (geom.GeometryType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
					continue;

				tmpValue = feature.get_Value(i_Name);
				if (tmpValue != DBNull.Value)
				{
					taxiwayName = tmpValue.ToString();
					if (taxiwayName.StartsWith(constStr))
					{
						taxiwayName = taxiwayName.Substring(constStr.Length);
					}
				}

				if (string.IsNullOrEmpty(taxiwayName))
					continue;

				tmpValue = feature.get_Value(i_Type);
				if (tmpValue != DBNull.Value)
				{
					type = tmpValue.ToString();
				}

				Taxiway taxiway = new Taxiway();
				taxiway.Identifier = Guid.NewGuid();
				taxiway.Designator = taxiwayName;
				taxiway.AssociatedAirportHeliport = new FeatureRef(airportIdentifier);

				_gdbProvider.SetFeature(taxiway);

				TaxiwayElement te = new TaxiwayElement();
				te.Identifier = Guid.NewGuid();
				te.AssociatedTaxiway = new FeatureRef(taxiway.Identifier);
				if (!string.IsNullOrEmpty(type))
				{
					CodeTaxiwayElement tmpTaxiwayElementType;
					try
					{
						tmpTaxiwayElementType = (CodeTaxiwayElement)Enum.Parse(typeof(CodeTaxiwayElement), tmpValue.ToString());
						te.Type = tmpTaxiwayElementType;
					}
					catch (ArgumentException)
					{
					}

					//if (Enum.TryParse<CodeTaxiwayElement>(tmpValue.ToString(), true, out tmpTaxiwayElementType))
					//{
					//    te.Type = tmpTaxiwayElementType;
					//}
				}

				MultiPolygon mpolygon = (MultiPolygon)Aran.Converters.ConvertFromEsriGeom.ToPolygonGeo((ESRI.ArcGIS.Geometry.IPolygon)geom);
				if (mpolygon != null)
				{
					te.Extent = new ElevatedSurface();
					te.Extent.Geo.Assign((MultiPolygon)mpolygon);
					te.Extent.Elevation = new ValDistanceVertical(33, UomDistanceVertical.FT);
				}

				_gdbProvider.SetFeature(te);
			}
		}

		private void InsertApron(Guid airportIdentifier, string fileName)
		{
			ESRI.ArcGIS.Geodatabase.IWorkspaceFactory wsFact = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory();
			var ws = wsFact.OpenFromFile(fileName, 0);
			var featWs = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)ws;
			var featClass = featWs.OpenFeatureClass("ApronElement");
			var featCursor = featClass.Search(null, false);

			int i_Name = featClass.FindField("Name");
			int i_Type = featClass.FindField("Type");

			string constStr = "AprElement-";

			ESRI.ArcGIS.Geodatabase.IFeature feature;
			object tmpValue;
			while ((feature = featCursor.NextFeature()) != null)
			{
				ESRI.ArcGIS.Geometry.IGeometry geom = feature.Shape;
				string name = null;
				string type = null;

				if (geom.GeometryType != ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
					continue;

				tmpValue = feature.get_Value(i_Name);
				if (tmpValue != DBNull.Value)
				{
					name = tmpValue.ToString();
					if (name.StartsWith(constStr))
					{
						name = name.Substring(constStr.Length);
					}
				}

				if (string.IsNullOrEmpty(name))
					continue;

				tmpValue = feature.get_Value(i_Type);
				if (tmpValue != DBNull.Value)
				{
					type = tmpValue.ToString();
				}

				Apron apron = new Apron();
				apron.Identifier = Guid.NewGuid();
				apron.Name = name;
				apron.AssociatedAirportHeliport = new FeatureRef(airportIdentifier);
				_gdbProvider.SetFeature(apron);

				ApronElement element = new ApronElement();
				element.Identifier = Guid.NewGuid();
				element.AssociatedApron = new FeatureRef(apron.Identifier);

				if (!string.IsNullOrEmpty(type))
				{
					CodeApronElement tmpElementType = (CodeApronElement)0;
					bool assigned = true;

					switch (tmpValue.ToString())
					{
						case "CARGO":
							tmpElementType = CodeApronElement.CARGO;
							break;
						case "FUEL":
							tmpElementType = CodeApronElement.FUEL;
							break;
						case "HARDSTAND":
							tmpElementType = CodeApronElement.HARDSTAND;
							break;
						case "LOADING":
							tmpElementType = CodeApronElement.LOADING;
							break;
						case "MAINT":
							tmpElementType = CodeApronElement.MAINT;
							break;
						case "MILITARY":
							tmpElementType = CodeApronElement.MILITARY;
							break;
						case "NORMAL":
							tmpElementType = CodeApronElement.NORMAL;
							break;
						case "PARKING":
							tmpElementType = CodeApronElement.PARKING;
							break;
						case "RAMP":
							tmpElementType = CodeApronElement.RAMP;
							break;
						case "STAIRS":
							tmpElementType = CodeApronElement.STAIRS;
							break;
						case "TAXILANE":
							tmpElementType = CodeApronElement.TAXILANE;
							break;
						case "TEMPORARY":
							tmpElementType = CodeApronElement.TEMPORARY;
							break;
						case "TURNAROUND":
							tmpElementType = CodeApronElement.TURNAROUND;
							break;
						default:
							assigned = false;
							break;
					}

					if (assigned)
						element.Type = tmpElementType;
					//if (Enum.TryParse<CodeApronElement>(tmpValue.ToString(), true, out tmpElementType))
					//{
					//    element.Type = tmpElementType;
					//}
				}

				MultiPolygon mpolygon = (MultiPolygon)Aran.Converters.ConvertFromEsriGeom.ToPolygonGeo((ESRI.ArcGIS.Geometry.IPolygon)geom);
				if (mpolygon != null)
				{
					element.Extent = new ElevatedSurface();
					element.Extent.Geo.Assign((MultiPolygon)mpolygon);
					element.Extent.Elevation = new ValDistanceVertical(33, UomDistanceVertical.FT);
				}

				_gdbProvider.SetFeature(element);
			}
		}

		private void InsertComplexVerticalStructure(string fileName, Guid orgIdentifier)
		{
			DataGridView dgv = new DataGridView();

			#region Fill DataGridView from File

			dgv.Rows.Clear();

			FileStream fs = new FileStream(fileName, FileMode.Open);
			StreamReader sr = new StreamReader(fs);

			bool isColumnsLoad = false;

			string line = null;
			while ((line = sr.ReadLine()) != null)
			{
				string[] sa = line.Split('\t');

				if (!isColumnsLoad)
				{
					isColumnsLoad = true;
					for (int i = 1; i <= sa.Length; i++)
					{
						DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
						col.Name = "Column " + i;
						dgv.Columns.Add(col);
					}
				}

				dgv.Rows.Add(sa);
			}

			sr.Close();
			fs.Close();

			#endregion

			#region Convert

			int x_index = 3;
			int y_index = 4;
			int elev_idnex = 5;
			int partName_index = 7;
			int partGeomType_index = 9;
			int partGeomIndex_index = 11;

			dgv.Sort(new ObsInfoComparer(partName_index, partGeomIndex_index));

			List<ObsInfo> obsInfoList = new List<ObsInfo>();

			int[] reqFields = new int[] { x_index, y_index, elev_idnex, partName_index, partGeomType_index, partGeomIndex_index };

			for (int i = 0; i < dgv.Rows.Count; i++)
			{
				DataGridViewCellCollection cells = dgv.Rows[i].Cells;

				bool isContinue = false;
				for (int j = 0; j < reqFields.Length; j++)
				{
					if (cells[reqFields[j]].Value == null)
					{
						isContinue = true;
						break;
					}
				}
				if (isContinue)
					continue;

				double x = Convert.ToDouble(cells[x_index].Value);
				double y = Convert.ToDouble(cells[y_index].Value);
				double elev = Convert.ToDouble(cells[elev_idnex].Value);
				string name = cells[partName_index].Value.ToString();
				string geomType = cells[partGeomType_index].Value.ToString();
				int orderIndex = Convert.ToInt32(cells[partGeomIndex_index].Value);

				ObsInfo obsInfo = GetObsInfo(name, obsInfoList);

				if (obsInfo == null)
				{
					obsInfo = new ObsInfo();
					obsInfo.Name = name;
					obsInfo.GeomType = ToGeometryType(geomType);
					obsInfo.HorAccuracy = 3.0;	//3.0;
					obsInfo.VertAccuracy = 0.15;

					obsInfoList.Add(obsInfo);
				}

				Aran.Geometries.Point pt = new Aran.Geometries.Point(x, y, elev);
				pt.M = orderIndex;
				obsInfo.Coordinates.Add(pt);
			}

			VerticalStructure vs = new VerticalStructure();
			vs.Identifier = Guid.NewGuid();
			vs.Name = "Complex VS";

			var orgFro = new Aran.Aim.Objects.FeatureRefObject();
			orgFro.Feature = new FeatureRef(orgIdentifier);
			vs.HostedOrganisation.Add(orgFro);

			foreach (ObsInfo obsInfo in obsInfoList)
			{
				VerticalStructurePart vsPart = ToVerticalStructurePart(obsInfo);
				vs.Part.Add(vsPart);
			}

			_gdbProvider.SetFeature(vs);

			#endregion
		}

		private ObsInfo GetObsInfo(string name, List<ObsInfo> obsInfoList)
		{
			foreach (ObsInfo obsInfo in obsInfoList)
			{
				if (obsInfo.Name == name)
					return obsInfo;
			}
			return null;
		}

		private VerticalStructurePart ToVerticalStructurePart(ObsInfo obsInfo)
		{
			obsInfo.Coordinates.Sort(new ObsInfoCoordinateComparer());

			VerticalStructurePart vsPart = new VerticalStructurePart();
			vsPart.Designator = obsInfo.Name;
			vsPart.HorizontalProjection = new VerticalStructurePartGeometry();

			if (obsInfo.GeomType == GeometryType.Point)
			{
				if (obsInfo.Coordinates.Count > 0)
				{
					ElevatedPoint ep = new ElevatedPoint();
					ep.Elevation = new ValDistanceVertical(obsInfo.Elevation, UomDistanceVertical.M);
					ep.HorizontalAccuracy = new ValDistance(obsInfo.HorAccuracy, UomDistance.M);
					double x = obsInfo.Coordinates[0].X;
					double y = obsInfo.Coordinates[0].Y;
					ep.Geo.SetCoords(x, y);

					vsPart.HorizontalProjection.Location = ep;
				}
			}
			else if (obsInfo.GeomType == GeometryType.MultiLineString)
			{
				if (obsInfo.Coordinates.Count > 1)
				{
					ElevatedCurve ec = new ElevatedCurve();
					ec.Elevation = new ValDistanceVertical(obsInfo.Elevation, UomDistanceVertical.M);
					ec.HorizontalAccuracy = new ValDistance(obsInfo.HorAccuracy, UomDistance.M);

					LineString lineString = new LineString();

					foreach (Aran.Geometries.Point pt in obsInfo.Coordinates)
					{
						lineString.Add(new Aran.Geometries.Point(pt.X, pt.Y));
					}

					ec.Geo.Add(lineString);
					vsPart.HorizontalProjection.LinearExtent = ec;
				}
			}
			else if (obsInfo.GeomType == GeometryType.MultiPolygon)
			{
				if (obsInfo.Coordinates.Count > 1)
				{
					ElevatedSurface es = new ElevatedSurface();
					es.Elevation = new ValDistanceVertical(obsInfo.Elevation, UomDistanceVertical.M);
					es.HorizontalAccuracy = new ValDistance(obsInfo.HorAccuracy, UomDistance.M);
					Polygon polygon = new Polygon();

					foreach (Aran.Geometries.Point pt in obsInfo.Coordinates)
					{
						polygon.ExteriorRing.Add(new Aran.Geometries.Point(pt.X, pt.Y));
					}

					es.Geo.Add(polygon);
					vsPart.HorizontalProjection.SurfaceExtent = es;
				}
			}

			return vsPart;
		}

		private GeometryType ToGeometryType(string geomType)
		{
			geomType = geomType.ToLower();

			if (geomType == "point")
				return GeometryType.Point;

			if (geomType == "polyline")
				return GeometryType.MultiLineString;

			if (geomType == "polygon")
				return GeometryType.MultiPolygon;

			return GeometryType.Null;
		}
	}

	internal class ObsInfo
	{
		public ObsInfo()
		{
			Coordinates = new List<Aran.Geometries.Point>();
		}

		public string Name;

		public double Elevation
		{
			get
			{
				if (Coordinates.Count == 0)
					return double.NaN;

				double maxZ = Coordinates[0].Z;

				for (int i = 1; i < Coordinates.Count; i++)
				{
					if (maxZ < Coordinates[i].Z)
						maxZ = Coordinates[i].Z;
				}

				return maxZ;
			}
		}

		public GeometryType GeomType;
		public double HorAccuracy;
		public double VertAccuracy;

		public List<Aran.Geometries.Point> Coordinates { get; private set; }
	}

	internal class ObsInfoComparer : System.Collections.IComparer
	{
		public int Sort1Index;
		public int Sort2Index;

		public ObsInfoComparer(int sort1Index, int sort2Index)
		{
			Sort1Index = sort1Index;
			Sort2Index = sort2Index;
		}

		public int Compare(object x, object y)
		{
			DataGridViewRow dgvr1 = (DataGridViewRow)x;
			DataGridViewRow dgvr2 = (DataGridViewRow)y;

			int c1 = string.Compare((string)dgvr1.Cells[Sort1Index].Value,
				(string)dgvr2.Cells[Sort1Index].Value);

			if (c1 != 0)
				return c1;

			if (dgvr1.Cells[Sort2Index].Value == null)
				return -1;

			if (dgvr2.Cells[Sort2Index].Value == null)
				return 1;

			return Convert.ToInt32(dgvr1.Cells[Sort2Index].Value) -
				Convert.ToInt32(dgvr2.Cells[Sort2Index].Value);
		}
	}

	internal class ObsInfoCoordinateComparer : IComparer<Aran.Geometries.Point>
	{
		public int Compare(Aran.Geometries.Point x, Aran.Geometries.Point y)
		{
			return (int)(x.M - y.M);
		}
	}
}
