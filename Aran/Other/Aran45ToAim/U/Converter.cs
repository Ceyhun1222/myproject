using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using AIXM45_AIM_UTIL;

namespace Aran45ToAixm
{
	public class Converter
	{
		private IFeatureWorkspace _featWS;
		private List<Aran.Aim.Features.Feature> _designatedPointList;

		public Converter ()
		{

		}

		public void Open (IFeatureWorkspace featWS)
		{
			_featWS = featWS;
		}

		public List<string> GetSupportedTables45 ()
		{
			var list = new List<string> ();
			list.Add ("Organisation, OrganisationAuthority");
			list.Add ("AD_HP, AirportHeliport");
			list.Add ("RWY, Runway");
			list.Add ("RwyDirection, RunwayDirection");
			list.Add ("RwyCline");
			list.Add ("VOR");
			list.Add ("DME");
			list.Add ("TACAN");
			list.Add ("IGP, Glidepath");
			list.Add ("ILZ, Localizer");
			list.Add ("NDB");
			list.Add ("DesignatedPoint");
			list.Add ("Obstacle, VerticalStructure");			
			list.Add ("Airspace");
			list.Add ("RouteSegment");

			return list;
		}

		public List<ConvertedObj> Convert (string tableName, ITable table, List<string> errorList)
		{
			switch (tableName)
			{
				case "Airspace":
					return ConvertAirspace (table as IFeatureClass, errorList);
				case "DesignatedPoint":
					var convObjList = ConvertDesignatedPoint (table as IFeatureClass, errorList);
					_designatedPointList = convObjList.Select (co => co.Obj).ToList ();
					return convObjList;
				case "VerticalStructure":
					return ConvertVerticalStructure (table as IFeatureClass, errorList);
				case "RouteSegment":
					return ConvertRoutes (table as IFeatureClass, errorList);
			}

			return new List<ConvertedObj> ();
		}

		
		private List<ConvertedObj> ConvertAirspace (IFeatureClass featClass, List<string> errorList)
		{
			IFeatureCursor cursor = featClass.Search (null, false);
			IFeature esriFeature;
			var list = new List<ConvertedObj> ();

			System.Array enumItems = Enum.GetValues (typeof (AirspaceField));
			int [] fieldIndexes = new int [enumItems.Length];
			
			foreach (object enumItem in enumItems)
			{
				fieldIndexes [(int) enumItem] = featClass.FindField (enumItem.ToString ());
			}

			var valueGetter = new EsriFieldValueGetter<AirspaceField> ();
			valueGetter.FieldIndexes = fieldIndexes;

			while ((esriFeature = cursor.NextFeature ()) != null)
			{
				valueGetter.CurrentRowItem = esriFeature;

				try
				{
					var convObj = Aixm45To51.ToFeature (valueGetter);
					if (convObj.Obj != null)
						list.Add (convObj);
				}
				catch (Exception ex)
				{
					errorList.Add ("Error, ID: " + valueGetter.GetId () + "\r\n" + ex.Message);
				}
			}

			return list;
		}

		private List<ConvertedObj> ConvertDesignatedPoint (IFeatureClass featClass, List<string> errorList)
		{
			IFeatureCursor cursor = featClass.Search (null, false);
			IFeature esriFeature;

			var list = new List<ConvertedObj> ();

			System.Array enumItems = Enum.GetValues (typeof (DesignatedPointField));
			int [] fieldIndexes = new int [enumItems.Length];

			foreach (object enumItem in enumItems)
			{
				fieldIndexes [(int) enumItem] = featClass.FindField (enumItem.ToString ());
			}

			var valueGetter = new EsriFieldValueGetter<DesignatedPointField> ();
			valueGetter.FieldIndexes = fieldIndexes;

			while ((esriFeature = cursor.NextFeature ()) != null)
			{
				valueGetter.CurrentRowItem = esriFeature;

				try
				{
					var convObj = Aixm45To51.ToFeature (valueGetter);
					if (convObj.Obj != null)
						list.Add (convObj);
				}
				catch (Exception ex)
				{
					errorList.Add ("Error, ID: " + valueGetter.GetId () + "\r\n" + ex.Message);
				}
			}

			return list;
		}

		private List<ConvertedObj> ConvertVerticalStructure (IFeatureClass featClass, List<string> errorList)
		{
			IFeatureCursor cursor = featClass.Search (null, false);
			IFeature esriFeature;
			var list = new List<ConvertedObj> ();

			System.Array enumItems = Enum.GetValues (typeof (VerticalStructureField));
			int [] fieldIndexes = new int [enumItems.Length];

			foreach (object enumItem in enumItems)
			{
				fieldIndexes [(int) enumItem] = featClass.FindField (enumItem.ToString ());
			}

			var valueGetter = new EsriFieldValueGetter<VerticalStructureField> ();
			valueGetter.FieldIndexes = fieldIndexes;

            Aixm45To51.UnknowVerticalStructureIndex = 1;

			while ((esriFeature = cursor.NextFeature ()) != null)
			{
				valueGetter.CurrentRowItem = esriFeature;

				try
				{
					var convObj = Aixm45To51.ToFeature (valueGetter);
					if (convObj.Obj != null)
						list.Add (convObj);
				}
				catch (Exception ex)
				{
					errorList.Add ("Error, ID: " + valueGetter.GetId () + "\r\n" + ex.Message);
				}
			}

			return list;
		}

		private List<ConvertedObj> ConvertRoutes (IFeatureClass featClass, List<string> errorList)
		{
			if (_designatedPointList == null)
				_designatedPointList = new List<Aran.Aim.Features.Feature> ();

			IFeatureCursor cursor = featClass.Search (null, false);
			IFeature esriFeature;

			System.Array enumItems = Enum.GetValues (typeof (RouteSegmentField));
			int [] fieldIndexes = new int [enumItems.Length];

			foreach (object enumItem in enumItems)
			{
				fieldIndexes [(int) enumItem] = featClass.FindField (enumItem.ToString ());
			}

			var valueGetter = new EsriFieldValueGetter<RouteSegmentField> ();
			valueGetter.FieldIndexes = fieldIndexes;

			var rsTagList = new List<RouteSegmentTag> ();

			while ((esriFeature = cursor.NextFeature ()) != null)
			{
				valueGetter.CurrentRowItem = esriFeature;

				try
				{
					var rsTag = Aixm45To51.ToRouteSegment (valueGetter as AbsFieldValueGetter<RouteSegmentField>);
					rsTagList.Add (rsTag);
				}
				catch (Exception ex)
				{
					errorList.Add ("Error, ID: " + valueGetter.GetId () + "\r\n" + ex.Message);
				}
			}

			foreach (var rsTag in rsTagList)
			{
				var rs = rsTag.RouteSegment as Aran.Aim.Features.RouteSegment;

				var guid = GetDesignatedPointByDesignator (rsTag.StartPointCodeId);
				if (guid != Guid.Empty)
				{
					rs.Start = new Aran.Aim.Features.EnRouteSegmentPoint ();
					rs.Start.PointChoice = new Aran.Aim.Features.SignificantPoint ();
					rs.Start.PointChoice.FixDesignatedPoint = new Aran.Aim.DataTypes.FeatureRef (guid);
				}

				guid = GetDesignatedPointByDesignator (rsTag.EndPointCodeId);
				if (guid != Guid.Empty)
				{
					rs.End = new Aran.Aim.Features.EnRouteSegmentPoint ();
					rs.End.PointChoice = new Aran.Aim.Features.SignificantPoint ();
					rs.End.PointChoice.FixDesignatedPoint = new Aran.Aim.DataTypes.FeatureRef (guid);
				}
			}

			return PostRouteSegmentConvert (rsTagList, errorList);
		}

		private List <ConvertedObj> PostRouteSegmentConvert (List<RouteSegmentTag> rsTagList, List<string> errorList)
		{
			try
			{
				ITable table = _featWS.OpenTable ("EnrouteRoute");
				ICursor cursor = table.Search (null, false);
				IRow row;

				var routeDict = new Dictionary<string, Aran.Aim.Features.Route> ();

				while ((row = cursor.NextRow ()) != null)
				{
					try
					{
						var route = new Aran.Aim.Features.Route ();
						Aixm45To51.FillTimeSlice (route);

						string mid = row.get_Value (row.Fields.FindField ("R_mid")).ToString ();
						route.Name = row.get_Value (row.Fields.FindField ("R_txtDesig")).ToString ();
						route.LocationDesignator = row.get_Value (row.Fields.FindField ("R_txtLocDesig")).ToString ();

						routeDict.Add (mid, route);
					}
					catch (Exception ex)
					{
						errorList.Add ("Error, ID: " + row.OID + "\r\n" + ex.Message);
					}
				}

				var routeFeatList = new List<Aran.Aim.Features.Feature> ();
				var routeSegFeatList = new List<Aran.Aim.Features.Feature> ();

				foreach (var rsTag in rsTagList)
				{
					Aran.Aim.Features.Route route;
					if (routeDict.TryGetValue (rsTag.RouteTag.Mid, out route))
					{
						route.InternationalUse = rsTag.RouteTag.InternationalUse;
						route.FlightRule = rsTag.RouteTag.FlightRule;
						route.MilitaryUse = rsTag.RouteTag.MilitaryUse;

						(rsTag.RouteSegment as Aran.Aim.Features.RouteSegment).RouteFormed = 
							new Aran.Aim.DataTypes.FeatureRef (route.Identifier);

                        if (!routeFeatList.Contains(route))
                            routeFeatList.Add(route);
						routeSegFeatList.Add (rsTag.RouteSegment);
					}
				}

				var convObjList = new List<ConvertedObj> ();

				foreach (var item in routeFeatList)
					convObjList.Add (new ConvertedObj () { Obj = item });

				foreach (var item in routeSegFeatList)
					convObjList.Add (new ConvertedObj () { Obj = item });

				return convObjList;
			}
			catch (Exception ex)
			{
				errorList.Add (ex.Message);
			}

			return null;
		}

		private Guid GetDesignatedPointByDesignator (string designator)
		{
			if (_designatedPointList != null)
			{
				foreach (Aran.Aim.Features.DesignatedPoint dp in _designatedPointList)
				{
					if (dp.Designator == designator)
						return dp.Identifier;
				}
			}
			
			return default (Guid);
		}
	}
}
