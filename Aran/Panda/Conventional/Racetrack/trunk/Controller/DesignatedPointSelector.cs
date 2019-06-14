using Aran.Geometries;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Panda.Common;

namespace Aran.Panda.Conventional.Racetrack
{
	public class DesignatedPointSelector
	{
		public DesignatedPointSelector ( DesignatedPointCreater createDsgPnt )
		{
			_createDsgPnt = createDsgPnt;
		}

		#region Add EventHandlers

		public void AddtDsgPntListChangedEvent ( DsgPntListEventHandler OnDsgPntListChanged )
		{
			_dsgPntListChanged += OnDsgPntListChanged;
		}

		public void AddDsgPntChangedEvent ( DsgPntEventHandler OnDsgPntChanged )
		{
			_selectedDsgPntChanged += OnDsgPntChanged;
		}

		#endregion

		#region Set Values

		public void SetRadius ( double radius )
		{
			Radius = radius;
		}

		public void SetDesignatePointList ( )
		{
			if ( _selectednavaid != null )
				DsgntPntList = GlobalParams.Database.HoldingQpi.GetDesignatedPointList ( _selectednavaid.Location.Geo, _radius );
			else
				DsgntPntList = null;
		}

		public void SetDesignatedPoint ( DesignatedPoint dsgPnt )
		{
			SelectedDesignatedPoint = dsgPnt;
		}

		public void SetNavaid ( Navaid navaid, bool DirChanged, bool DistChanged )
		{
			if ( _selectednavaid != navaid )
			{
				_selectednavaid = navaid;
				SetDesignatePointList ( );
			}
			else
			{
				SetDirAndDist ( DirChanged, DistChanged );
			}
		}

		public void SetDmePntPrj ( Point dmePntPrj )
		{
			_selectedDmePntPrj = dmePntPrj;
		}

		private void SetDirAndDist ( bool DirChanged, bool DistChanged )
		{
			if ( DirChanged )
				_createDsgPnt.SetDirection ( ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( _createDsgPnt.VorPntPrj, _selectedDsgPntPrj ), ARANMath.C_2xPI ), true );
			if ( DistChanged )
				_createDsgPnt.SetNominalDistance ( ARANFunctions.ReturnDistanceInMeters ( _createDsgPnt.VorPntPrj, _selectedDsgPntPrj ), true, true );
			if ( _selectedDsgPntChanged != null )
			{
				DsgPntEventArg argDsgPnt = new DsgPntEventArg ( _selectedDsgPntPrj );
				_selectedDsgPntChanged ( null, argDsgPnt );
			}
		}

		#endregion

		#region Properties

		public double Radius
		{
			get
			{
				return _radius;
			}
			private set
			{
				_radius = GlobalParams.UnitConverter.DistanceToInternalUnits ( value );
				SetDesignatePointList ( );
			}
		}

		public List<DesignatedPoint> DsgntPntList
		{
			get
			{
				return _desgntPntList;
			}

			private set
			{
				if ( value == null || value.Count == 0 )
					_desgntPntList = null;
				else
					_desgntPntList = value;
				if ( _dsgPntListChanged != null )
				{
					_dsgPntListChanged ( null, new DsgPntListEventArg ( _desgntPntList ) );
				}
				//if ( _desgntPntList.Count == 0 )
				//{
				//    _createDsgPnt.SetDirection ( 0, true );
				//    _createDsgPnt.SetNominalDistance ( 0, true );
				//    _selectedDsgPnt = null;
				//}
			}
		}

		public DesignatedPoint SelectedDesignatedPoint
		{
			get
			{
				return _selectedDsgPnt;
			}

			private set
			{
				_selectedDsgPnt = value;
				if ( _selectedDsgPnt == null )
				{
					_selectedDsgPntGeo = null;
					_selectedDsgPntPrj = null;
					SetDirAndDist ( false, false );
				}
				else
				{
					_selectedDsgPntGeo = _selectedDsgPnt.Location.Geo;
					_selectedDsgPntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( _selectedDsgPntGeo );
					SetDirAndDist (true, true );
				}
			}
		}

		#endregion

		private DsgPntListEventHandler _dsgPntListChanged;
		private DsgPntEventHandler _selectedDsgPntChanged;

		private double _radius = 0;
		private List<DesignatedPoint> _desgntPntList;
		private DesignatedPoint _selectedDsgPnt;
		private Point _selectedDsgPntGeo, _selectedDsgPntPrj;
		private Navaid _selectednavaid;
		private Point _selectedDmePntPrj;
		private DesignatedPointCreater _createDsgPnt;
	}
}
