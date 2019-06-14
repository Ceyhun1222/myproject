using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Geometries;
using System.Reflection;
using AIXM45_AIM_UTIL;

namespace Aran45ToAixm
{
    internal static class Aixm45To51
    {
        public static ConvertedObj ToFeature<TypeField> (AbsFieldValueGetter<TypeField> valueGetter)
        {
            Type type = typeof (TypeField);
            if (type == typeof (AirspaceField))
                return ToAirspace (valueGetter as AbsFieldValueGetter<AirspaceField>);
            if (type == typeof (VerticalStructureField))
                return ToVerticalStructure (valueGetter as AbsFieldValueGetter<VerticalStructureField>);
            if (type == typeof (DesignatedPointField))
                return ToDesignatedPoint (valueGetter as AbsFieldValueGetter<DesignatedPointField>);
            return null;
        }

        public static ConvertedObj ToAirspace (AbsFieldValueGetter<AirspaceField> valueGetter)
        {
            var aranGeom = valueGetter.GetGeometry ();
            if (aranGeom == null)
                return null;

            MultiPolygon aranPolygon = null;
            if (aranGeom.Type == GeometryType.MultiPolygon)
            {
                aranPolygon = aranGeom as MultiPolygon;
            }
            else if (aranGeom.Type == GeometryType.Polygon)
            {
                aranPolygon = new MultiPolygon ();
                var aranSignlePolygon = aranGeom as Aran.Geometries.Polygon;
				
				if (aranSignlePolygon.ExteriorRing.Count == 0)
					return null;

                aranPolygon.Add (aranSignlePolygon);
            }

			if (aranPolygon == null)
				return null;

			if (!CheckGeometry (aranPolygon))
				return null;

            string s, code, uom;
            object val;

            var airspace = new Airspace ();
            FillTimeSlice (airspace);

            var volume = new AirspaceVolume ();
            volume.HorizontalProjection = new Surface ();
            volume.HorizontalProjection.Geo.Assign (aranPolygon);

            airspace.GeometryComponent.Add (new AirspaceGeometryComponent ());
            airspace.GeometryComponent [0].TheAirspaceVolume = volume;


            airspace.Designator = valueGetter [AirspaceField.R_codeId];
            airspace.LocalType = valueGetter [AirspaceField.txtLocalType];
            airspace.Name = valueGetter [AirspaceField.txtName];


            s = valueGetter [AirspaceField.txtRmk];
            if (s != null)
            {
                var ln = new LinguisticNote ();
                ln.Note = new TextNote ();
                ln.Note.Lang = language.ENG;
                ln.Note.Value = s;

                airspace.Annotation.Add (new Note ());
                airspace.Annotation [0].Purpose = CodeNotePurpose.REMARK;
                airspace.Annotation [0].TranslatedNote.Add (ln);
            }

            #region Enums

            s = valueGetter [AirspaceField.codeClass];
            CodeAirspaceClassification codeAC;
            if (Enum.TryParse<CodeAirspaceClassification> (s, true, out codeAC))
            {
                airspace.Class.Add (new AirspaceLayerClass ());
                airspace.Class [0].Classification = codeAC;
            }

            s = valueGetter [AirspaceField.codeActivity];
            CodeAirspaceActivity codeAA;
            if (Enum.TryParse<CodeAirspaceActivity> (s, true, out codeAA))
            {
                airspace.Activation.Add (new AirspaceActivation ());
                airspace.Activation [0].Activity = codeAA;
            }

            s = valueGetter [AirspaceField.codeMil];
            CodeMilitaryOperations codeMO;
            if (Enum.TryParse<CodeMilitaryOperations> (s, true, out codeMO))
            {
                airspace.ControlType = codeMO;
            }

            s = valueGetter [AirspaceField.R_codeType];
            CodeAirspace codeAir;
            if (Enum.TryParse<CodeAirspace> (s, true, out codeAir))
            {
                airspace.Type = codeAir;
            }

            #endregion

            #region Upper

            code = valueGetter [AirspaceField.codeDistVerUpper];
            val = valueGetter [AirspaceField.valDistVerUpper];
            uom = valueGetter [AirspaceField.uomDistVerUpper];

            CodeVerticalReference? cvr = FieldConverter.ToCodeVerticalReference (code);
            UomDistanceVertical udv;
            if (cvr != null && val != null && Enum.TryParse<UomDistanceVertical> (uom, true, out udv))
            {
                volume.UpperLimit = new ValDistanceVertical (Convert.ToDouble (val), udv);
                volume.UpperLimitReference = cvr;
            }

            #endregion

            #region Lower

            code = valueGetter [AirspaceField.codeDistVerLower];
            val = valueGetter [AirspaceField.valDistVerLower];
            uom = valueGetter [AirspaceField.uomDistVerLower];

            cvr = FieldConverter.ToCodeVerticalReference (code);
            if (cvr != null && val != null && Enum.TryParse<UomDistanceVertical> (uom, true, out udv))
            {
                volume.LowerLimit = new ValDistanceVertical (Convert.ToDouble (val), udv);
                volume.LowerLimitReference = cvr;
            }

            #endregion

            #region Max Limit

            code = valueGetter [AirspaceField.codeDistVerMax];
            val = valueGetter [AirspaceField.valDistVerMax];
            uom = valueGetter [AirspaceField.uomDistVerMax];

            cvr = FieldConverter.ToCodeVerticalReference (code);
            if (cvr != null && val != null && Enum.TryParse<UomDistanceVertical> (uom, true, out udv))
            {
                volume.MaximumLimit = new ValDistanceVertical (Convert.ToDouble (val), udv);
                volume.MaximumLimitReference = cvr;
            }

            #endregion

            #region Min Limit

            code = valueGetter [AirspaceField.codeDistVerMnm];
            val = valueGetter [AirspaceField.valDistVerMnm];
            uom = valueGetter [AirspaceField.uomDistVerMnm];

            cvr = FieldConverter.ToCodeVerticalReference (code);
            if (cvr != null && val != null && Enum.TryParse<UomDistanceVertical> (uom, true, out udv))
            {
                volume.MinimumLimit = new ValDistanceVertical (Convert.ToDouble (val), udv);
                volume.MinimumLimitReference = cvr;
            }

            #endregion

            return new ConvertedObj () { Obj = airspace };;
        }

		private static bool CheckGeometry (MultiPolygon aranPolygon)
		{
			foreach (Polygon polygon in aranPolygon)
			{
				foreach (Aran.Geometries.Point point in polygon.ExteriorRing)
				{
					if (point.X < 0 || point.X > 90)
						return false;
					if (point.Y < 0 || point.Y > 180)
						return false;
				}
			}

			return true;
		}

        public static int UnknowVerticalStructureIndex = 1;

		public static ConvertedObj ToVerticalStructure (AbsFieldValueGetter<VerticalStructureField> valueGetter)
        {
            var aranPoint = valueGetter.GetGeometry () as Aran.Geometries.Point;
            if (aranPoint == null)
                return null;

            string s;
            double? d;

            var vs = new VerticalStructure ();
            FillTimeSlice (vs);

            vs.Name = valueGetter [VerticalStructureField.txtName];

            if (string.IsNullOrWhiteSpace(vs.Name))
                vs.Name = "OBSTACLE_" + (UnknowVerticalStructureIndex++);

            s = valueGetter [VerticalStructureField.codeGroup];
            if (s != null)
                vs.Group = (s == "Y");

            s = valueGetter [VerticalStructureField.codeLgt];
            if (s != null)
                vs.Lighted = (s == "Y");

            VerticalStructurePart part = new VerticalStructurePart ();
            part.Designator = valueGetter [VerticalStructureField.txtDescrType];

            if (part.Designator != null) {
                if (part.Designator.Length >= 16)
                    part.Designator = part.Designator.Substring(0, 15);
                part.Designator = part.Designator.ToUpper();
            }

            part.HorizontalProjection = new VerticalStructurePartGeometry ();
			s = valueGetter[ VerticalStructureField.uomDistVer ];
			d = valueGetter[ VerticalStructureField.valHgt ];
			UomDistance ud;
			if (d.HasValue && s != null && Enum.TryParse<UomDistance> ( s, true, out ud ) )
			{
				part.VerticalExtent = new ValDistance(d.Value, ud);
			}

			ElevatedPoint ep = new ElevatedPoint ();
            ep.Geo.Assign (aranPoint);
            part.HorizontalProjection.Location = ep;
			UomDistanceVertical udv;
			d = valueGetter [VerticalStructureField.valElev];
            if (d.HasValue && s != null && Enum.TryParse<UomDistanceVertical> (s, true, out udv))
            {
                ep.Elevation = new ValDistanceVertical (d.Value, udv);
            }

            d = valueGetter[VerticalStructureField.valGeoAccuracy];
            s = valueGetter[VerticalStructureField.uomGeoAccuracy];
            if (d.HasValue && s != null && Enum.TryParse<UomDistance>(s, true, out ud))
            {
                ep.HorizontalAccuracy = new ValDistance(d.Value, ud);
            }

            vs.Part.Add (part);

			var convObj = new ConvertedObj () {
				mid = valueGetter.GetMid (),
				Obj = vs,
				CRCInfo = valueGetter.GetCRCInfo ()
			};

			convObj.CRCInfo.Name = vs.Name;

			return convObj;
        }

        public static ConvertedObj ToDesignatedPoint (AbsFieldValueGetter<DesignatedPointField> valueGetter)
        {
            var aranPoint = valueGetter.GetGeometry () as Aran.Geometries.Point;
            if (aranPoint == null)
                return null;

            var dp = new DesignatedPoint ();
            FillTimeSlice (dp);

            dp.Designator = valueGetter [DesignatedPointField.R_codeId];
            dp.Type = FieldConverter.ToCodeDesignatedPoint (valueGetter [DesignatedPointField.codeType]);

            dp.Location = new AixmPoint ();
            dp.Location.Geo.Assign (aranPoint);

			var convObj = new ConvertedObj () {
				mid = valueGetter.GetMid (),
				Obj = dp,
				CRCInfo = valueGetter.GetCRCInfo (),
			};

			convObj.CRCInfo.Name = dp.Designator;

			return convObj;
        }

        public static RouteSegmentTag ToRouteSegment (AbsFieldValueGetter<RouteSegmentField> valueGetter)
        {
			var rs = new RouteSegment ();
			var rsTag = new RouteSegmentTag ();
			rsTag.RouteSegment = rs;

            string s;

            Global.CurrentFuncTagDict.Add (rs, rsTag);

            FillTimeSlice (rs);

            rs.NavigationType = FieldConverter.ToSameText<CodeRouteNavigation> (valueGetter [RouteSegmentField.codeType]);
            s = valueGetter [RouteSegmentField.codeRnp];
            if (!string.IsNullOrWhiteSpace (s))
                rs.RequiredNavigationPerformance =  double.Parse (s);
            rs.Level = FieldConverter.ToCodeLevel (valueGetter [RouteSegmentField.codeLvl]);
			rsTag.RouteTag.InternationalUse = FieldConverter.ToCodeRouteOrigin (valueGetter [RouteSegmentField.codeIntl]);
			rsTag.RouteTag.FlightRule = FieldConverter.ToCodeFlightRule (valueGetter [RouteSegmentField.codeTypeFltRule]);
			rsTag.RouteTag.MilitaryUse = FieldConverter.ToCodeMilitaryStatus (valueGetter [RouteSegmentField.codeCiv]);
            rs.UpperLimit = FieldConverter.ToValDistanceVertical (valueGetter.GetValues (RouteSegmentField.valDistVerUpper, RouteSegmentField.uomDistVerUpper));
            rs.UpperLimitReference = FieldConverter.ToSameText<CodeVerticalReference> (valueGetter [RouteSegmentField.codeDistVerUpper]);
            rs.LowerLimit = FieldConverter.ToValDistanceVertical (valueGetter.GetValues (RouteSegmentField.valDistVerLower, RouteSegmentField.uomDistVerLower));
            rs.LowerLimitReference = FieldConverter.ToSameText<CodeVerticalReference> (valueGetter [RouteSegmentField.codeDistVerLower]);
            rs.MinimumEnrouteAltitude = FieldConverter.ToValDistanceVertical (valueGetter.GetValues (RouteSegmentField.valDistVerMnm, RouteSegmentField.uomDistVerMnm));
            rs.WidthLeft = FieldConverter.ToValDistance (valueGetter.GetValues (RouteSegmentField.valWid, RouteSegmentField.uomWid));
            rs.WidthRight = rs.WidthLeft;

            rs.Start = new EnRouteSegmentPoint ();
            rs.Start.ReportingATC = FieldConverter.ToCodeATCReporting (valueGetter [RouteSegmentField.codeRepAtcStart]);
            rs.Start.RoleRVSM = FieldConverter.ToCodeATCReporting (valueGetter [RouteSegmentField.codeRvsmStart]);

            rs.End = new EnRouteSegmentPoint ();
            rs.End.ReportingATC = FieldConverter.ToCodeATCReporting (valueGetter [RouteSegmentField.codeRepAtcEnd]);
            rs.End.RoleRVSM = FieldConverter.ToCodeATCReporting (valueGetter [RouteSegmentField.codeRvsmEnd]);

            rs.PathType = FieldConverter.ToSameText<CodeRouteSegmentPath> (valueGetter [RouteSegmentField.codeTypePath]);
            rs.TrueTrack = valueGetter [RouteSegmentField.valTrueTrack];
            rs.MagneticTrack = valueGetter [RouteSegmentField.valMagTrack];
            rs.ReverseTrueTrack = valueGetter [RouteSegmentField.valReversTrueTrack];
            rs.ReverseMagneticTrack = valueGetter [RouteSegmentField.valReversMagTrack];
            rs.Length = FieldConverter.ToValDistance (valueGetter.GetValues (RouteSegmentField.valLen, RouteSegmentField.uomDist));

            s = valueGetter [RouteSegmentField.txtRmk];
            if (!string.IsNullOrWhiteSpace (s))
                rs.Annotation.Add (GetNote (s));

            var ra = new RouteAvailability ();
            ra.Direction  = FieldConverter.ToCodeDirection (valueGetter [RouteSegmentField.R_codeDir]);
            rs.Availability.Add (ra);

            rsTag.RouteTag.Mid = valueGetter [RouteSegmentField.R_RteMid];
            rsTag.StartPointCodeId = valueGetter [RouteSegmentField.R_SignificantPointStacode_Id];
            rsTag.EndPointCodeId = valueGetter [RouteSegmentField.R_SignificantPointEndcode_Id];

            var aranGeom = valueGetter.GetGeometry ();
            var mls = new MultiLineString ();

            if (aranGeom.Type == GeometryType.MultiLineString)
            {
                mls = aranGeom as MultiLineString;
            }
            else if (aranGeom.Type == GeometryType.LineString)
            {
                mls = new MultiLineString ();
                mls.Add (aranGeom as LineString);
            }

            rs.CurveExtent = new Curve ();
            rs.CurveExtent.Geo.Assign (mls);

            return rsTag;
        }

        
        public static void FillTimeSlice (Feature feature)
        {
            feature.Identifier = Guid.NewGuid ();
            feature.TimeSlice = new TimeSlice ();
            feature.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
            feature.TimeSlice.ValidTime = new TimePeriod (DateTime.Now);
            feature.TimeSlice.FeatureLifetime = feature.TimeSlice.ValidTime;
        }

        private static Note GetNote (string text)
        {
            var note = new Note ();
            note.Purpose = CodeNotePurpose.REMARK;
            var ln = new LinguisticNote ();
            ln.Note = new TextNote ();
            ln.Note.Lang = language.ENG;
            ln.Note.Value = text;
            note.TranslatedNote.Add (ln);

            return note;
        }

    }

    public class RouteSegmentTag
    {
		public RouteSegmentTag ()
		{
			RouteTag = new RouteTag ();
		}

        public string StartPointCodeId;
        public string EndPointCodeId;
		public Feature RouteSegment;
		public RouteTag RouteTag { get; private set; }

		//public Route Route;
		//public string RouteMid;
    }

	public class RouteTag
	{
		public string Mid;
		public CodeRouteOrigin? InternationalUse;
		public CodeFlightRule? FlightRule;
		public CodeMilitaryStatus? MilitaryUse;
	}
}
