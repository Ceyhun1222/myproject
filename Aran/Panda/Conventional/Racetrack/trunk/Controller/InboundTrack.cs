using Aran.Geometries;
using Aran.Panda.Common;

namespace Aran.Panda.Conventional.Racetrack
{
	//public class InboundTrack
	//{
	//    public void AddDirectionChangedEvent ( DirectionEventHandler OnDirChanged )
	//    {
	//        _directionChanged += OnDirChanged;
	//    }

	//    public void SetDirection ( double direction, bool isCalculated, bool raiseEvent = true )
	//    {
	//        if ( isCalculated )
	//            _direction = direction;
	//        else
	//            _direction = ARANMath.Modulus ( ARANFunctions.AztToDirection ( _vorPntGeo, /*ARANMath.RadToDeg ( */direction, GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj ), ARANMath.C_2xPI );
	//        if ( _directionChanged != null && raiseEvent )
	//        {
	//            DirectionEventArg argDir = new DirectionEventArg ( _direction, ARANFunctions.DirToAzimuth ( _vorPntPrj, _direction, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo ) );
	//            _directionChanged ( null, argDir );
	//        }
	//    }

	//    public double Direction
	//    {
	//        get
	//        {
	//            return _direction;
	//        }
	//    }

	//    public void SetVorPntPrj (Point vorPntPrj, Point vorPntGeo )
	//    {
	//        _vorPntPrj = vorPntPrj;
	//        _vorPntGeo = vorPntGeo;
	//    }

	//    private DirectionEventHandler _directionChanged;
	//    private Point _vorPntPrj, _vorPntGeo;
	//    private double _direction;
	//}
}
