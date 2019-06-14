using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.Aim.Features;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class AppliedGuiValues
	{
		public AppliedGuiValues ( )
		{
			_appliedGuiValues = new GuiValues ( );
			_newGuiValues = new GuiValues ( );
		}

		public void AddAppliedValueChangedHandler ( AppliedValueChangedEventHandler handler )
		{
			_appliedValueChanged += handler;
		}

		public void SetDesignatedPoint ( Point value )
		{
			_newGuiValues.DesignatePoint = value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//_newGUIValues.DesignatePoint = value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );
		}

		public void SetIas ( double value )
		{
			SetValue ( ref _newGuiValues.Ias, value );
		}

		internal void SetProcType ( ProcedureTypeConv procType )
		{
			_procType = procType;
		}

		public void SetTime ( double value )
		{
			_newGuiValues.Time = value;
			_appliedValueChanged ( IsEqualPrevious );
		}

		public void SetAltitude ( double value )
		{
			SetValue ( ref _newGuiValues.Altitude, value );
		}

		public void SetLimitingDistance ( double value )
		{
			SetValue ( ref _newGuiValues.LimDist, value );
		}

		public void SetNominalDistance ( double value )
		{
			SetValue ( ref _newGuiValues.NomDist, value );
		}

		public void SetNavaid ( Navaid value )
		{
			_newGuiValues.Navaid = value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//_newGUIValues.Navaid = value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );
		}

		public void SetIntersectingVor ( VOR value )
		{
			_newGuiValues.IntersectVor = value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//_newGUIValues.IntersectVor = value;
			//if ( IsEqualPrevious != isEqual )
				//_appliedValueChanged ( false );
		}

		public void SetIntersectingVorRadial ( double value )
		{
			SetValue ( ref _newGuiValues.IntersectVorRadial, value );
		}

		public void SetDirection ( double value )
		{
			SetValue ( ref _newGuiValues.Direction, value );
		}

		public void SetSide ( SideDirection value )
		{
			_newGuiValues.Side = value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//_newGUIValues.Side = value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );
		}

		public void SetIsToward ( bool value )
		{
			SetValue ( ref _newGuiValues.IsToward, value );
		}

		public void SetIsWithLimitingRadial ( bool value )
		{
			SetValue (ref _newGuiValues.IsWithLimRadial, value );
		}

		public void SetOverheadDirection ( double value )
		{
			SetValue ( ref _newGuiValues.OverheadDirection, value );
		}

		private void SetValue (ref double property, object value )
		{
			//bool isEqual = IsEqualPrevious;
			//property = (double) value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );

			property = ( double ) value;
			_appliedValueChanged ( IsEqualPrevious );
		}

		private void SetValue ( ref object property, object value )
		{
			property = ( double ) value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//property = ( double ) value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );
		}

		private void SetValue ( ref bool property, object value )
		{
			property = ( bool ) value;
			_appliedValueChanged ( IsEqualPrevious );

			//bool isEqual = IsEqualPrevious;
			//property = (bool) value;
			//if ( IsEqualPrevious != isEqual )
			//    _appliedValueChanged ( false );
		}

		public bool Constructed
		{
			get
			{
				return _constructed;
			}

			set
			{
				_constructed = value;
				if ( _constructed )
				{
					//var keyList = new List<string> ( _appliedValues.Keys );
					//foreach ( var key in keyList )
					//{
					//    _appliedValues [ key ] = false;
					//}
					_appliedGuiValues.Altitude = _newGuiValues.Altitude;
					_appliedGuiValues.DesignatePoint= _newGuiValues.DesignatePoint;
					_appliedGuiValues.Direction = _newGuiValues.Direction;
					_appliedGuiValues.Ias = _newGuiValues.Ias;
					_appliedGuiValues.IntersectVor = _newGuiValues.IntersectVor;
					_appliedGuiValues.IntersectVorRadial = _newGuiValues.IntersectVorRadial;
					_appliedGuiValues.IsToward = _newGuiValues.IsToward;
					_appliedGuiValues.IsWithLimRadial = _newGuiValues.IsWithLimRadial;
					_appliedGuiValues.LimDist = _newGuiValues.LimDist;
					_appliedGuiValues.Navaid = _newGuiValues.Navaid;
					_appliedGuiValues.NomDist = _newGuiValues.NomDist;
					_appliedGuiValues.OverheadDirection = _newGuiValues.OverheadDirection;
					_appliedGuiValues.Side = _newGuiValues.Side;
					_appliedGuiValues.Time = _newGuiValues.Time;					
				}				
			}
		}

		public bool IsEqualPrevious
		{
			get
			{
				bool isEqualDsgPnt = false;
				if ( _procType == ProcedureTypeConv.Vordme )
				{
					if ( _appliedGuiValues.DesignatePoint == null && _newGuiValues.DesignatePoint == null )
						isEqualDsgPnt = true;
					else if ( _appliedGuiValues.DesignatePoint != null && _newGuiValues.DesignatePoint != null )
						if ( _appliedGuiValues.DesignatePoint.X == _newGuiValues.DesignatePoint.X && _appliedGuiValues.DesignatePoint.Y == _newGuiValues.DesignatePoint.Y )
							isEqualDsgPnt = true;

					if ( isEqualDsgPnt && _appliedGuiValues.Altitude == _newGuiValues.Altitude &&
						_appliedGuiValues.Direction == _newGuiValues.Direction &&
						_appliedGuiValues.Ias == _newGuiValues.Ias &&
						_appliedGuiValues.IsToward == _newGuiValues.IsToward &&
						_appliedGuiValues.IsWithLimRadial == _newGuiValues.IsWithLimRadial &&
						_appliedGuiValues.LimDist == _newGuiValues.LimDist &&
						_appliedGuiValues.Navaid == _newGuiValues.Navaid &&
						_appliedGuiValues.NomDist == _newGuiValues.NomDist &&
						_appliedGuiValues.Side == _newGuiValues.Side &&
						_appliedGuiValues.Time == _newGuiValues.Time )
						return true;

				}
				else if ( _procType == ProcedureTypeConv.VorNdb )
				{
					if ( _appliedGuiValues.Altitude == _newGuiValues.Altitude &&
						_appliedGuiValues.OverheadDirection == _newGuiValues.OverheadDirection &&
						_appliedGuiValues.Ias == _newGuiValues.Ias &&
						_appliedGuiValues.Navaid == _newGuiValues.Navaid &&
						_appliedGuiValues.NomDist == _newGuiValues.NomDist &&
						_appliedGuiValues.Side == _newGuiValues.Side &&
						_appliedGuiValues.Time == _newGuiValues.Time )
						return true;
				}
				else if ( _procType == ProcedureTypeConv.Vorvor )
				{
					if ( _appliedGuiValues.DesignatePoint == null && _newGuiValues.DesignatePoint == null )
						isEqualDsgPnt = true;
					else if ( _appliedGuiValues.DesignatePoint != null && _newGuiValues.DesignatePoint != null )
						if ( _appliedGuiValues.DesignatePoint.X == _newGuiValues.DesignatePoint.X && _appliedGuiValues.DesignatePoint.Y == _newGuiValues.DesignatePoint.Y )
							isEqualDsgPnt = true;

					if (isEqualDsgPnt && _appliedGuiValues.Altitude == _newGuiValues.Altitude &&
						_appliedGuiValues.Direction == _newGuiValues.Direction &&
						_appliedGuiValues.Ias == _newGuiValues.Ias &&
						_appliedGuiValues.IntersectVor == _newGuiValues.IntersectVor &&
						_appliedGuiValues.IntersectVorRadial == _newGuiValues.IntersectVorRadial &&
						_appliedGuiValues.IsToward == _newGuiValues.IsToward &&
						_appliedGuiValues.Navaid == _newGuiValues.Navaid &&
						_appliedGuiValues.Side == _newGuiValues.Side &&
						_appliedGuiValues.Time == _newGuiValues.Time )
						return true;
				}
				return false;
				//return ( !_appliedValues.ContainsValue ( true ) && Constructed );
			}
		}

		private AppliedValueChangedEventHandler _appliedValueChanged;
		private GuiValues _appliedGuiValues, _newGuiValues;		
		private bool _constructed;
		private ProcedureTypeConv _procType;
	}

	public struct GuiValues
	{
		public Point DesignatePoint;
		public double Ias;
		public double Altitude;
		public double LimDist;
		public double NomDist;
		public Navaid Navaid;
		public VOR IntersectVor;
		public double Direction;
		public SideDirection Side;
		public bool IsToward;
		public bool IsWithLimRadial;
		public double OverheadDirection;
		public double Time;
		public double IntersectVorRadial;
	}
}