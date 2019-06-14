using ESRI.ArcGIS.Geometry;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartTypeA.Utils
{
    class RwyDepictionUtil
    {
        public static IPolygon GetRunwayElementWithTaxiway()
        {
            try
            {
                var taxiwayElementList = GlobalParams.RModel.SelectedAirport.TaxiwayList.SelectMany<Taxiway, TaxiwayElement>(taxiway => taxiway.TaxiWayElementsList).
                    Where<TaxiwayElement>(taxElement => taxElement.TaxiwayElementType == CodeTaxiwayElementType.INTERSECTION || taxElement.TaxiwayElementType == CodeTaxiwayElementType.NORMAL).ToList();

                var rwyElementList = GlobalParams.RModel.SelectedRunway.RunwayElementsList.
                    Where(elem => elem.RunwayElementType == CodeRunwayElementType.INTERSECTION || elem.RunwayElementType == CodeRunwayElementType.NORMAL).ToList();

                var normal = CalculateTaxiwayElement(taxiwayElementList, rwyElementList);

                var taxiwayShoulderElementList = GlobalParams.RModel.SelectedAirport.TaxiwayList.SelectMany<Taxiway, TaxiwayElement>(taxiway => taxiway.TaxiWayElementsList).
                    Where<TaxiwayElement>(taxElement => taxElement.TaxiwayElementType == CodeTaxiwayElementType.SHOULDER).
                    ToList<TaxiwayElement>();

                var rwyShoulderElementList = GlobalParams.RModel.SelectedRunway.RunwayElementsList.
                    Where(elem => elem.RunwayElementType == CodeRunwayElementType.SHOULDER).ToList();

                var shoulder = CalculateTaxiwayElement(taxiwayShoulderElementList, rwyShoulderElementList);

                var taxiwayResult = EsriFunctions.Union(normal, shoulder);
                return taxiwayResult as IPolygon;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static ESRI.ArcGIS.Geometry.IPolygon CalculateTaxiwayElement(List<TaxiwayElement> taxiwayElementList,
            List<RunwayElement> rwyElementList)
        {
            var allTaxiwayIntersectingList = new List<ESRI.ArcGIS.Geometry.IGeometry>();

            if (taxiwayElementList != null && taxiwayElementList.Count > 0)
            {
                bool isIntersect = false;
                for (int i = 1; i < taxiwayElementList.Count; i++)
                {
                    taxiwayElementList[i].RebuildGeo();
                    if (!isIntersect)
                    {
                        taxiwayElementList[i - 1].RebuildGeo();
                        var prjGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(taxiwayElementList[i - 1].Geo);
                        allTaxiwayIntersectingList.Add(prjGeo);
                    }

                    isIntersect = false;
                    for (int j = 0; j < allTaxiwayIntersectingList.Count; j++)
                    {
                        if (taxiwayElementList[i].Geo == null) continue;

                        var tmpGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(taxiwayElementList[i].Geo);
                        if (EsriFunctions.ReturnDistanceAsMetr(tmpGeo, allTaxiwayIntersectingList[j]) < 5)
                        {
                            isIntersect = true;
                            allTaxiwayIntersectingList[j] = EsriFunctions.Union(tmpGeo, allTaxiwayIntersectingList[j]);
                        }
                    }
                }
            }

            ESRI.ArcGIS.Geometry.IPolygon normResultGeo = new ESRI.ArcGIS.Geometry.PolygonClass();

            for (int i = 0; i < rwyElementList.Count; i++)
            {
                var rwyElement = rwyElementList[i];
                rwyElement.RebuildGeo();
                if (rwyElement.Geo == null) continue;

                var elementGeo = GlobalParams.SpatialRefOperation.ToEsriPrj(rwyElement.Geo);
                if (i == 0)
                    normResultGeo = elementGeo as IPolygon;

                foreach (var taxiwayInterGeo in allTaxiwayIntersectingList)
                {
                    if (EsriFunctions.ReturnDistanceAsMetr(taxiwayInterGeo, elementGeo) < 5)
                    {
                        var tmpGeo = EsriFunctions.Union(taxiwayInterGeo, elementGeo);
                        normResultGeo = (ESRI.ArcGIS.Geometry.IPolygon) EsriFunctions.Union(normResultGeo, tmpGeo);
                    }
                }

            }
            return normResultGeo;
        }
    }
}
