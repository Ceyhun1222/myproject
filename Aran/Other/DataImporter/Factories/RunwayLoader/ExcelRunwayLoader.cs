using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DataImporter.Factories.RunwayLoader
{
    class ExcelRunwayLoader : IRunwayFileLoader
    {
        private string _fileName;
        private ILogger _logger;
        private SpatialReferenceOperation _spOperation;
        private List<RwyCenterPoint> _centerPoints;

        public ExcelRunwayLoader(string fileName, ILogger logger,SpatialReferenceOperation spOperation)
        {
            _fileName = fileName;
            _logger = logger;
            _spOperation = spOperation;

            if (string.IsNullOrEmpty(_fileName))
                throw new ArgumentNullException($"FileName cannot be empty");
        }

        private List<RwyCenterPoint> LoadRwyCenterPoints()
        {
            XSSFWorkbook book1 = new XSSFWorkbook(new FileStream(_fileName, FileMode.Open));

            var cLineSheet = book1[0];


            IEnumerator rows = cLineSheet.GetRowEnumerator();

            var cntPtList = new List<RwyCenterPoint>();

            //Head row
            
            rows.MoveNext();
            while (rows.MoveNext())
            {
                IRow row = (XSSFRow)rows.Current;

                if (row == null) continue;
                try
                {
                    var rwyCenterPt = new RwyCenterPoint
                    {
                        ID = row.GetCell(0).ToString(),
                        Lat = row.GetCell(1).ToString(),
                        Long = row.GetCell(2).ToString(),
                        Elev = Convert.ToDouble(row.GetCell(3).ToString()),
                        Geoid = Convert.ToDouble(row.GetCell(4).ToString())
                    };

                    rwyCenterPt.Y = ARANFunctions.DmsStr2Dd(rwyCenterPt.Lat.Substring(0, rwyCenterPt.Lat.Length - 1), true);
                    rwyCenterPt.X = ARANFunctions.DmsStr2Dd(rwyCenterPt.Long.Substring(0, rwyCenterPt.Long.Length - 1), false);
                    rwyCenterPt.Geo = new Aran.Geometries.Point(rwyCenterPt.X, rwyCenterPt.Y);
                    rwyCenterPt.GeoPrj = _spOperation.ToPrj(rwyCenterPt.Geo);

                    cntPtList.Add(rwyCenterPt);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                    //We should add logs
                    continue;
                }
            }
            return cntPtList;
        }

        public List<RwyCenterPoint> RwyCenterPoints => _centerPoints ?? (_centerPoints = LoadRwyCenterPoints());
    }
}
