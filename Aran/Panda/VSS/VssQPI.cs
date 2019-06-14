using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Aran.Geometries.Operators;
using Aran.PANDA.Common;

namespace Aran.PANDA.Vss
{
    public class VssQPI : CommonQPI
    {
        public List<VerticalStructure> GetVerticalStructureList(MultiPolygon polygon)
        {
            var blo = new BinaryLogicOp();
            blo.Type = BinaryLogicOpType.Or;

            var within = new Within();
            within.Geometry = polygon;
            within.PropertyName = "part.horizontalProjection.location.geo";
            blo.OperationList.Add(new OperationChoice(within));

			within = new Within ( );
			within.Geometry = polygon;
			within.PropertyName = "part.horizontalProjection.linearExtent.geo";
			blo.OperationList.Add ( new OperationChoice ( within ) );

			within = new Within ( );
			within.Geometry = polygon;
			within.PropertyName = "part.horizontalProjection.surfaceExtent.geo";
			blo.OperationList.Add ( new OperationChoice ( within ) );

            var filter = new Filter(new OperationChoice(blo));
			Stopwatch st = new Stopwatch ( );
			st.Start ( );

            var gettingResult = base.DbProvider.GetVersionsOf(
                FeatureType.VerticalStructure,
                TimeSliceInterpretationType.BASELINE,
                default(Guid),
                true,
                null,
                null,
                filter);

			st.Stop ( );			

            if (!gettingResult.IsSucceed)
                throw new Exception(gettingResult.Message);

			var obsList = gettingResult.GetListAs<VerticalStructure> ( );
			GeometryOperators geomOper = new GeometryOperators ( );
			geomOper.CurrentGeometry = Globals.SpatRefOperation.ToPrj<Geometry> ( polygon );
			Geometry pGeomGeo = null, pGeomPrj;
			List<VerticalStructure> filteredObsList = new List<VerticalStructure> ( );
			foreach ( var obstacle in obsList )
			{
				if ( obstacle.Part == null )
					continue;
				foreach ( var obsPart in obstacle.Part )
				{
					if ( obsPart.HorizontalProjection == null )
						continue;
					switch ( obsPart.HorizontalProjection.Choice )
					{
						case VerticalStructurePartGeometryChoice.ElevatedPoint:
							pGeomGeo = obsPart.HorizontalProjection.Location.Geo;
							break;
						case VerticalStructurePartGeometryChoice.ElevatedCurve:
							pGeomGeo = obsPart.HorizontalProjection.LinearExtent.Geo;
							break;
						case VerticalStructurePartGeometryChoice.ElevatedSurface:
							pGeomGeo = obsPart.HorizontalProjection.SurfaceExtent.Geo;
							break;
						default:
							break;
					}
					pGeomPrj = Globals.SpatRefOperation.ToPrj<Geometry> ( pGeomGeo );
					if ( pGeomPrj.IsEmpty )
						continue;
					if ( !geomOper.Disjoint ( pGeomPrj ) )
					{
						filteredObsList.Add ( obstacle );
						break;
					}
				}
			}
			

			var terrainDataList = DoTerrainDataReader(polygon);
            if (terrainDataList == null)
                return gettingResult.GetListAs<VerticalStructure>();

            var result = new List<VerticalStructure>();
            result.AddRange(filteredObsList);
            result.AddRange(terrainDataList);
            return result;
        }

        public Feature GetFeature(FeatureType featureType, Guid identifier)
        {
            var result = DbProvider.GetVersionsOf(featureType,
                TimeSliceInterpretationType.BASELINE,
                identifier,
                true, null, null, null);

            if (!result.IsSucceed) {
                Globals.ShowError(result.Message);
                return null;
            }

            if (result.List == null || result.List.Count == 0)
                return null;

            return (Feature)result.List[0];
        }

        public List<TFeature> GetFeatures<TFeature>(Guid identifier = default(Guid), Filter filter = null) where TFeature : Feature, new()
        {
            var feat = new TFeature();
            var gr = DbProvider.GetVersionsOf(feat.FeatureType, TimeSliceInterpretationType.BASELINE, identifier, true, null, null, filter);
            if (!gr.IsSucceed)
                throw new Exception(gr.Message);
            return gr.GetListAs<TFeature>();
        }

    }
}
