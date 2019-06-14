using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Threading;
using Aran.Queries.Rnav;
using Aran.Aim.DB;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
namespace Holding
{
	public class DBModule
	{
        public DBModule()
		{
            if (this.HoldingQpi == null)
                HoldingQpi = RNAVSQPIFactory.Create();
           
            var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
            HoldingQpi.Open(dbProvider);

            var terrainDataReaderHandler = GlobalParams.AranEnvironment.CommonData.GetObject("terrainDataReader") as Aran.Queries.TerrainDataReaderEventHandler;
            if (terrainDataReaderHandler != null)
                HoldingQpi.TerrainDataReader += terrainDataReaderHandler;
		}

		public  IRNAVSpecializedQPI HoldingQpi { get; private set; }
	}
		
}
