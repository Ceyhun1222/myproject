using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Converters;
using Aran.Geometries;

namespace Omega.Models
{
    public class Annex15Surfaces
    {

        public const double RWYCat12Width = 75.0;
        public const double RWYCat34Width = 150.0;

        private List<RwyCenterlineClass> _rwyCntlnClassList;
        private RunwayCentrelinePoint _startCntlnPt, _endCntlnPt;
        private Aran.Geometries.Point _startCntPrj, _endCntPrj;
        private SpatialReferenceOperation _spatialRefOper;
        private GeometryOperators _geomOperators;
        private double _direction,_axis;
        private Aran.AranEnvironment.IAranGraphics _ui;
        private ElevationDatum _elevDatum;
        private RwyClass _selectedRwy;
        private double _refH;
        private int _codeNumber;
        private RwyDirClass _rwyDir1,_rwyDir2;

        public Annex15Surfaces(RwyClass selectedRwy, ElevationDatum elevDatum)
        {
            try
            {
                _selectedRwy = selectedRwy;
                _elevDatum = elevDatum;
                _refH = elevDatum.Height;

                _spatialRefOper = GlobalParams.SpatialRefOperation;
                _geomOperators = GlobalParams.GeomOperators;

                _rwyDir1 = selectedRwy.RwyDirClassList[0];
                _rwyDir2 = selectedRwy.RwyDirClassList[1];

                _codeNumber = _selectedRwy.CodeNumber;

                _direction = ARANFunctions.ReturnAngleInRadians(_rwyDir1.CenterLineClassList[0].PtPrj,_rwyDir1.CenterLineClassList[1].PtPrj);
                _axis = _direction + Math.PI;
                _ui = GlobalParams.UI;
                CreateProfileLinePoint();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void CreateProfileLinePoint()
        {
            //Initialize rwy points
            ProfileLinePoint = new List<Point>();
            ProfileLinePoint.Add(_spatialRefOper.ToPrj(_rwyDir2.EndCntlPt.Location.Geo));
            ProfileLinePoint.AddRange(_rwyDir1.CenterLineClassList.Select(cntr => cntr.PtPrj));

            Point rwyDir2OffSet = ARANFunctions.LocalToPrj(ProfileLinePoint[0], _direction, -_rwyDir2.ClearWay, 0);
            rwyDir2OffSet.Z = ConverterToSI.Convert(_rwyDir2.EndCntlPt.Location.Elevation, 0);
            ProfileLinePoint.Insert(0,rwyDir2OffSet);

            Point rwyDir1OffSet = ARANFunctions.LocalToPrj(ProfileLinePoint[ProfileLinePoint.Count - 1], _direction,
                _rwyDir1.ClearWay, 0);
            rwyDir1OffSet.Z = ConverterToSI.Convert(_rwyDir1.EndCntlPt.Location.Elevation, 0);
            ProfileLinePoint.Add(rwyDir1OffSet);
        }
        public List<Aran.Geometries.Point> ProfileLinePoint { get; private set; }

        public Area2 CreateArea2()
        {
            return null;

        }

    }
}
