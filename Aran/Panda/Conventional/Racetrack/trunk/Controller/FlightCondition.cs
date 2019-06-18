﻿using System;
using System.Collections.Generic;
using System.Linq;
using Finisar.SQLite;
using System.Windows.Forms;

namespace Aran.Panda.Conventional.Racetrack
{

	//#region :>Enums
	//[FlagsAttribute]
	//public enum flightPhase
	//{
	//    Enroute = 1,
	//    STARUpTo30 = 2,
	//    STARDownTo30 = 4,
	//    IAF = 8
	//}

	//public enum flightReciever
	//{
	//    GNSS = 0,
	//    DMEDME = 1,
	//    VORDME = 2,
	//    VORNDB = 3,
	//    VORVOR = 4
	//}


	//public enum PointType
	//{
	//    Create,
	//    Select
	//}
	//#endregion

	//public class FlightCondition
	//{
	//    #region :> Fields

	//    private List<PhaseRecieverCondition> _PhaseRecieversList;
	//    private List<PBNCondition> _PBNConditionList;
	//    private List<GNSSValues> _GNSSValuesList;
	//    private SQLiteConnection _conn;
	//    private SQLiteCommand _command;
	//    private SQLiteDataReader _reader;

	//    #endregion

	//    #region :> Ctor
	//    public FlightCondition (string constsInstallDir )
	//    {
	//        string connString = String.Format ( "Data Source={0};New=False;Version=3", constsInstallDir + @"\FlightCondition.s3db" );

	//        if ( !OpenConnection ( connString ) )
	//        {
	//            MessageBox.Show ( "File path isn't true" );
	//            throw new Exception ( "Cannot open file" );
	//        }
	//        _command = _conn.CreateCommand ( );
	//        LoadPhaseRecieverCondition ( );
	//        LoadPBNCondition ( );
	//        LoadGnssValue ( );
	//        CloseConnection ( );
	//        string a = System.Reflection.Assembly.GetExecutingAssembly ( ).Location;
	//    }
	//    #endregion

	//    #region :>Methods
	//    public List<PBNCondition> GetPBNCondition ( PhaseRecieverCondition phaseReciever )
	//    {
	//        return ( from pbn in _PBNConditionList
	//                 where pbn.PhaseReciever_id == phaseReciever.Id
	//                 select pbn ).ToList<PBNCondition> ( );

	//    }

	//    public GNSSValues GetGNSSValue ( PBNCondition PBN )
	//    {
	//        return ( from GNSS in _GNSSValuesList
	//                 where GNSS.PBNCondition_id == PBN.Id
	//                 select GNSS ).ToList<GNSSValues> ( ) [ 0 ];
	//    }

	//    public List<PhaseRecieverCondition> GetFlightPhases ( flightPhase phases )
	//    {
	//        List<string> phaseNames = new List<string> ( );
	//        for ( int i = 1; i < 8; i += i )
	//        {
	//            if ( ( i & ( int ) phases ) != 0 )
	//                phaseNames.Add ( ( ( flightPhase ) i ).ToString ( ) );
	//        }


	//        var distinctItems = _PhaseRecieversList
	//            .Where ( c => phaseNames.Contains ( c.FlightPhaseName ) )
	//        .GroupBy ( x => x.FlightPhaseName )
	//        .Select ( y => y.First ( ) );

	//        return distinctItems.ToList<PhaseRecieverCondition> ( );
	//    }

	//    public List<PhaseRecieverCondition> GetReciever ( PhaseRecieverCondition phaseReciever )
	//    {
	//        Func<PhaseRecieverCondition, bool> check = s => s.FlightPhaseName == phaseReciever.FlightPhaseName;

	//        List<PhaseRecieverCondition> recieve = _PhaseRecieversList.
	//            Where ( s => check ( s ) )
	//            .Select ( s => s ).ToList<PhaseRecieverCondition> ( );
	//        return recieve;
	//    }

	//    private bool OpenConnection ( string pathFolder )
	//    {
	//        try
	//        {
	//            _conn = new SQLiteConnection ( pathFolder );
	//            _conn.Open ( );
	//            return true;
	//        }
	//        catch ( Exception )
	//        {
	//            try
	//            {
	//                _conn = new SQLiteConnection ( String.Format ( "Data Source={0};New=False;Version=3", "FlightCondition.s3db" ) );
	//                _conn.Open ( );
	//                return true;
	//            }
	//            catch ( Exception )
	//            {
	//                return false;

	//            }
	//        }

	//    }
	//    private void CloseConnection ( )
	//    {
	//        _conn.Close ( );
	//    }

	//    private void LoadPhaseRecieverCondition ( )
	//    {
	//        _PhaseRecieversList = new List<PhaseRecieverCondition> ( );
	//        _command.CommandText = "select Id,FlightPhase_Name,Reciever_name,UserFlightPhaseName from PhaseRecieverCondition";
	//        _reader = _command.ExecuteReader ( );
	//        while ( _reader.Read ( ) )
	//        {
	//            _PhaseRecieversList.Add ( new PhaseRecieverCondition
	//            {
	//                Id = Convert.ToInt32 ( _reader [ 0 ] ),
	//                FlightPhaseName = _reader [ 1 ].ToString ( ),
	//                RecieverName = _reader [ 2 ].ToString ( ),
	//                UserFlightPhaseName = _reader [ 3 ].ToString ( )
	//            } );
	//        }
	//        _reader.Close ( );
	//    }

	//    private void LoadPBNCondition ( )
	//    {
	//        _PBNConditionList = new List<PBNCondition> ( );
	//        _command.CommandText = "select id,phasereciever_id,pbn_name from PBNCondition";
	//        _reader = _command.ExecuteReader ( );
	//        while ( _reader.Read ( ) )
	//        {
	//            _PBNConditionList.Add ( new PBNCondition
	//            {
	//                Id = Convert.ToInt32 ( _reader [ "Id" ] ),
	//                PhaseReciever_id = Convert.ToInt32 ( _reader [ "PhaseReciever_ID" ] ),
	//                PBNName = _reader [ "PBN_Name" ].ToString ( )

	//            } );
	//        }
	//        _reader.Close ( );
	//    }

	//    private void LoadGnssValue ( )
	//    {
	//        _GNSSValuesList = new List<GNSSValues> ( );
	//        _command.CommandText = "select id,pbncondition_id,defined_id,comment,att,xtt" +
	//        ",unit,multiplier  from GNSSValues";
	//        _reader = _command.ExecuteReader ( );
	//        while ( _reader.Read ( ) )
	//        {

	//            _GNSSValuesList.Add ( new GNSSValues
	//            {
	//                Id = Convert.ToInt32 ( _reader [ "Id" ] ),
	//                PBNCondition_id = Convert.ToInt32 ( _reader [ 1 ] ),
	//                Defined_in = _reader [ 2 ].ToString ( ),
	//                Comment = _reader [ 3 ].ToString ( ),
	//                ATT = Convert.ToDouble ( _reader [ 4 ] ) * Convert.ToDouble ( _reader [ 7 ] ),
	//                XTT = Convert.ToDouble ( _reader [ 5 ] ) * Convert.ToDouble ( _reader [ 7 ] )
	//            } );

	//        }
	//        _reader.Close ( );

	//    }
	//    #endregion
	//}

	//public class PhaseRecieverCondition
	//{
	//    public int Id
	//    {
	//        get;
	//        set;
	//    }
	//    public string FlightPhaseName
	//    {
	//        get;
	//        set;
	//    }
	//    public string RecieverName
	//    {
	//        get;
	//        set;
	//    }
	//    public string UserFlightPhaseName
	//    {
	//        get;
	//        set;
	//    }
	//    public string UserReciverName
	//    {
	//        get;
	//        set;
	//    }
	//}

	//public class PBNCondition
	//{
	//    public int Id
	//    {
	//        get;
	//        set;
	//    }
	//    public int PhaseReciever_id
	//    {
	//        get;
	//        set;
	//    }
	//    public string PBNName
	//    {
	//        get;
	//        set;
	//    }
	//}

	//public class GNSSValues
	//{
	//    public int Id
	//    {
	//        get;
	//        set;
	//    }
	//    public int PBNCondition_id
	//    {
	//        get;
	//        set;
	//    }
	//    public string Defined_in
	//    {
	//        get;
	//        set;
	//    }
	//    public string Comment
	//    {
	//        get;
	//        set;
	//    }
	//    public double ATT
	//    {
	//        get;
	//        set;
	//    }
	//    public double XTT
	//    {
	//        get;
	//        set;
	//    }
	//}
}