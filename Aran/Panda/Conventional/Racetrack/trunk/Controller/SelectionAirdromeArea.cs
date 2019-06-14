using System.Collections.Generic;
using Aran.Aim.Features;
using System;

namespace Aran.Panda.Conventional.Racetrack
{
    public class SelectionAirdromeArea
    {
		public string AirportName
		{
			get;
			set;
		}

		private NavaidEventHandler _selectedNavaidChanged;

		private List<NavaidPntPrj> _selectedNavPnts;
		private FixFacilities _navaids;

        public SelectionAirdromeArea ( FixFacilities navaidList )
        {
            _navaids = navaidList;
			_selectedNavPnts = new List<NavaidPntPrj> ( );
            _navaids.AddNavaidChangedEvent ( OnSelectedNavaidChanged );
        }

        #region Add EventHandlers

        public void AddSelectedNavaidChangedEvent ( NavaidEventHandler value )
        {
            _selectedNavaidChanged += value;
        }

        #endregion

        #region Set Values

        public void SetRadius ( double radius )
        {
			_navaids.SetRadius ( GlobalParams.UnitConverter.DistanceToInternalUnits ( radius ) );
        }

        #endregion

        #region Properties 

        public List<NavaidPntPrj> SelectedNavaidsPntPrj
        {
            get
            {
                return _selectedNavPnts;
            }
        }

        #endregion

        #region Events

        private void OnSelectedNavaidChanged ( object sender, NavaidEventArg argNav )
        {
			_selectedNavPnts.Clear ( );
			foreach ( NavaidPntPrj navPntPrj in argNav.NavaidPntPrjList )
				_selectedNavPnts.Add ( navPntPrj );
			if ( _selectedNavaidChanged != null )
				_selectedNavaidChanged ( sender, argNav );
        }

        #endregion

        public void GetAerodrome ( )
        {
			var selectedOrganisation = GlobalParams.Database.HoldingQpi.GetOrganisation ( GlobalParams.Settings.Organization );
			List<AirportHeliport> arpList = GlobalParams.Database.HoldingQpi.GetAdhpListDesignatorNotNull ( selectedOrganisation.Identifier );
			if ( arpList.Count == 0 )
				throw new Exception ( "AirportHeliport not found in database !" );
			AirportName = arpList[ 0 ].Name;
			Geometries.Point _arpPnt = arpList[ 0 ].ARP.Geo;
			_navaids.SetArp ( _arpPnt.X, _arpPnt.Y );
			_navaids.SetRadius ( GlobalParams.Settings.Radius );
        }	
	}
}
