using Aran.Queries.Rnav;
using Aran.Aim.Data;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class DbModule
	{
        public DbModule(DbProvider dbProvider)
		{
            if (HoldingQpi == null)
                HoldingQpi = RNAVSQPIFactory.Create();
			//_connection = connectionStr;
			//TimeSliceFilter timeSliceFilter = new TimeSliceFilter(DateTime.Now);
			//IDbProvider dbProvider = PgProviderFactory.Create ( );
			//dbProvider.TimeSliceFilter = timeSliceFilter;

			HoldingQpi.Open ( dbProvider );


            var terrainDataReaderHandler = GlobalParams.AranEnvironment.CommonData.GetObject("terrainDataReader") as Queries.TerrainDataReaderEventHandler;
            if (terrainDataReaderHandler != null)
                HoldingQpi.TerrainDataReader += terrainDataReaderHandler;
		}

		//public DBModule():this(Connection)
		//{ 
						
		//}

		//public static string Connection
		//{
		//    get {
		//        if ( _connection == null )
		//            _connection = "Server = 172.30.31.18; Port = 5432; Database = AIM; User Id = aran; Password = aran;";
		//        return _connection; 
		//    }
		//    set { _connection = value; }
		//}

		public  IRNAVSpecializedQPI HoldingQpi { get; }


		
		//private static string _connection;
	}
}
