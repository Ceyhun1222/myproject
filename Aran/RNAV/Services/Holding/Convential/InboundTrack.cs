using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARAN.Common;
using ARAN.GeometryClasses;

namespace Holding.Convential
{
    public class InboundTrack
    {
        public void AddDirectionChangedEvent ( EventHandlerDirection OnDirChanged )
        {
            _directionChanged += OnDirChanged;
        }

        public void SetDirection ( double direction, bool isCalculated )
        {
            if ( isCalculated )
                _direction = direction;
            else
                _direction = ARANMath.Modulus ( ARANFunctions.AztToDirection ( _vorPntGeo, ARANFunctions.DegToRad ( ( double ) direction ), GlobalParams.SpatialRefOperation.GeoSp, GlobalParams.SpatialRefOperation.PrjSp ), 2*ARANMath.C_PI );
            if ( _directionChanged != null )
            {
                EventArgDirection argDir = new EventArgDirection ( _direction, ARANFunctions.RadToDeg ( ARANFunctions.DirToAzimuth ( _vorPntPrj, _direction, GlobalParams.SpatialRefOperation.PrjSp, GlobalParams.SpatialRefOperation.GeoSp ) ) );
                _directionChanged ( null, argDir );
            }
        }

        public double Direction
        {
            get
            {
                return _direction;
            }
        }

        public void SetVorPntPrj (Point vorPntPrj, Point vorPntGeo )
        {
            _vorPntPrj = vorPntPrj;
            _vorPntGeo = vorPntGeo;
        }

        private EventHandlerDirection _directionChanged;
        private Point _vorPntPrj, _vorPntGeo;
        private double _direction;
    }
}
