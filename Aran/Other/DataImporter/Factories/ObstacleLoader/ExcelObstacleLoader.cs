using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DataImporter.Factories.ObstacleLoader
{
    class ExcelObstacleLoader : IObstacleFileLoader
    {
        private readonly string _fileName;
        private List<Obstacle> _obstacles;
        private readonly SpatialReferenceOperation _spOperation;
        private ILogger _logger;

        /// <exception cref="ArgumentNullException"><paramref name="FileName cannot be empty"/> is <see langword="null"/></exception>
        public ExcelObstacleLoader(string fileName,ILogger logger,SpatialReferenceOperation spOperation)
        {
            _fileName = fileName;
            _logger = logger;
            _spOperation = spOperation;

            if (string.IsNullOrEmpty(_fileName))
                throw new ArgumentNullException($"FileName cannot be empty");
        }

        public List<Obstacle> Obstacles => _obstacles ?? (_obstacles = LoadObstacles(_fileName));

        public List<Obstacle> LoadObstacles(string fileName)
        {
           ValidateFormat(fileName);

            var sheets = new XSSFWorkbook(new FileStream(fileName, FileMode.Open));
            var obstSheet = sheets[0];

            var headerRowObs = obstSheet.GetRow(0);
            var rowsObs = obstSheet.GetRowEnumerator();

            var result = new List<Obstacle>();

            var colCountObs = headerRowObs.LastCellNum;
            rowsObs.MoveNext();
            while (rowsObs.MoveNext())
            {
                IRow row = (XSSFRow)rowsObs.Current;

                if (row == null) continue;
                var obstaclePt = new Obstacle {Name = row.GetCell(0).ToString()};

                if (string.IsNullOrEmpty(obstaclePt.Name) || string.IsNullOrWhiteSpace(obstaclePt.Name))
                    break;
                var type = row.GetCell(1);
                if (!string.IsNullOrEmpty(type?.StringCellValue))
                    obstaclePt.Type = type.ToString();

                obstaclePt.Lat = row.GetCell(2).ToString();
                obstaclePt.Long = row.GetCell(3).ToString();
                obstaclePt.Elev = Convert.ToDouble(row.GetCell(4).ToString());
                if (colCountObs > 6)
                {
                    obstaclePt.CodeGrp = row.GetCell(5).ToString() == "YES";
                    obstaclePt.Lght = row.GetCell(6).ToString() == "YES";
                    obstaclePt.Markings = row.GetCell(7).ToString() == "YES";
                    obstaclePt.RMK = row.GetCell(8).ToString();
                    if (colCountObs > 9)
                    {
                        obstaclePt.GeoType = (ObstacleGeoType)Enum.Parse(typeof(ObstacleGeoType), row.GetCell(9).ToString());
                        double radius;
                        if (row.GetCell(10) != null && double.TryParse(row.GetCell(10).ToString(), out radius))
                            obstaclePt.Radius = radius;
                    }
                }

                obstaclePt.Y =Math.Round(ARANFunctions.DmsStr2Dd(obstaclePt.Lat.Substring(0, obstaclePt.Lat.Length - 1), true),3);
                obstaclePt.X =Math.Round(ARANFunctions.DmsStr2Dd(obstaclePt.Long.Substring(0, obstaclePt.Long.Length - 1), false),3);
                obstaclePt.Geo = new Aran.Geometries.Point(obstaclePt.X, obstaclePt.Y);
                obstaclePt.GeoPrj = _spOperation.ToPrj(obstaclePt.Geo);

                result.Add(obstaclePt);
            }
            return result;
        }

        private void ValidateFormat(string fileName)
        {
            if (string.IsNullOrEmpty(_fileName))
                throw new ArgumentNullException($"FileName cannot be empty");

            if (!File.Exists(_fileName))
                throw new ArgumentException("File is not exist");
            var extension = Path.GetExtension(fileName);
            if (extension != null && !extension.Contains("xls"))
                throw new ArgumentException("File is not in correct format!");
        }

    }
}
