using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter
{
    class OldLoader
    {

        //private void LoadViciCebsk()
        //{
        //    try
        //    {

        //        var sheets = new XSSFWorkbook(new FileStream(FileName, FileMode.Open));

        //        if (sheets.Count == 2)
        //        {
        //            var cLineSheet = sheets[1];

        //            var dt = new DataTable();
        //            IRow headerRow = cLineSheet.GetRow(0);
        //            IEnumerator rows = cLineSheet.GetRowEnumerator();

        //            int colCount = headerRow.LastCellNum;
        //            int rowCount = cLineSheet.LastRowNum;

        //            bool skipReadingHeaderRow = rows.MoveNext();

        //            // rows.MoveNext();
        //            int i = 0;
        //            while (rows.MoveNext())
        //            {
        //                i++;
        //                IRow row = (XSSFRow)rows.Current;

        //                if (row == null) continue;
        //                var rwyCenterPt = new RwyCenterPoint();
        //                var id = row.GetCell(0).ToString();
        //                if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        //                    break;

        //                if (id.Contains("SZ")) continue;
        //                rwyCenterPt.ID = row.GetCell(0).ToString();
        //                rwyCenterPt.Lat = row.GetCell(1).ToString();
        //                rwyCenterPt.Long = row.GetCell(2).ToString();
        //                rwyCenterPt.Elev = Convert.ToDouble(row.GetCell(3).ToString());
        //                rwyCenterPt.Geoid = Convert.ToDouble(row.GetCell(4).ToString());
        //                //rwyCenterPt.Name = row.GetCell(4).ToString();
        //                //rwyCenterPt.RMK = row.GetCell(5).ToString();

        //                rwyCenterPt.Y = ARANFunctions.DmsStr2Dd(rwyCenterPt.Lat.Substring(0, rwyCenterPt.Lat.Length - 1), true);
        //                rwyCenterPt.X = ARANFunctions.DmsStr2Dd(rwyCenterPt.Long.Substring(0, rwyCenterPt.Long.Length - 1), false);
        //                rwyCenterPt.Geo = new Aran.Geometries.Point(rwyCenterPt.X, rwyCenterPt.Y);

        //                RwyCenterLineList.Add(rwyCenterPt);
        //            }
        //        }

        //        var obstSheet = sheets[0];

        //        var dtObs = new DataTable();
        //        var headerRowObs = obstSheet.GetRow(0);
        //        var rowsObs = obstSheet.GetRowEnumerator();

        //        var tmpObsList = new List<Obstacle>();
        //        var colCountObs = headerRowObs.LastCellNum;
        //        rowsObs.MoveNext();
        //        while (rowsObs.MoveNext())
        //        {
        //            IRow row = (XSSFRow)rowsObs.Current;

        //            if (row == null) continue;
        //            var obstaclePt = new Obstacle();
        //            obstaclePt.Name = row.GetCell(0).ToString();
        //            if (string.IsNullOrEmpty(obstaclePt.Name) || string.IsNullOrWhiteSpace(obstaclePt.Name))
        //                break;
        //            var type = row.GetCell(1);
        //            if (!string.IsNullOrEmpty(type?.StringCellValue))
        //                obstaclePt.Type = type.ToString();

        //            obstaclePt.Lat = row.GetCell(2).ToString();
        //            obstaclePt.Long = row.GetCell(3).ToString();
        //            obstaclePt.Elev = Convert.ToDouble(row.GetCell(4).ToString());
        //            if (colCountObs > 6)
        //            {
        //                obstaclePt.CodeGrp = row.GetCell(5).ToString() == "YES" ? true : false;
        //                obstaclePt.Lght = row.GetCell(6).ToString() == "YES" ? true : false;
        //                obstaclePt.Markings = row.GetCell(7).ToString() == "YES" ? true : false;
        //                obstaclePt.RMK = row.GetCell(8).ToString();
        //                if (colCountObs > 9)
        //                {
        //                    obstaclePt.GeoType = (ObstacleGeoType)Enum.Parse(typeof(ObstacleGeoType), row.GetCell(9).ToString());
        //                    double radius;
        //                    if (row.GetCell(10) != null && double.TryParse(row.GetCell(10).ToString(), out radius))
        //                        obstaclePt.Radius = radius;
        //                }
        //            }


        //            obstaclePt.Y = ARANFunctions.DmsStr2Dd(obstaclePt.Lat.Substring(0, obstaclePt.Lat.Length - 1), true);
        //            obstaclePt.X = ARANFunctions.DmsStr2Dd(obstaclePt.Long.Substring(0, obstaclePt.Long.Length - 1), false);
        //            obstaclePt.Geo = new Aran.Geometries.Point(obstaclePt.X, obstaclePt.Y);

        //            tmpObsList.Add(obstaclePt);
        //        }

        //        GroupObstacles(tmpObsList);
        //        NotifyPropertyChanged("RwyCenterCount");
        //        NotifyPropertyChanged("ObstacleCount");
        //    }

        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}

        //private void LoadMahilou()
        //{
        //    try
        //    {

        //        XSSFWorkbook book1 = new XSSFWorkbook(new FileStream(FileName, FileMode.Open));

        //        var cLineSheet = book1[1];

        //        DataTable dt = new DataTable();
        //        IRow headerRow = cLineSheet.GetRow(0);
        //        IEnumerator rows = cLineSheet.GetRowEnumerator();

        //        int colCount = headerRow.LastCellNum;
        //        int rowCount = cLineSheet.LastRowNum;

        //        for (int i = 0; i < 9; i++)
        //        {
        //            rows.MoveNext();
        //        }

        //        // rows.MoveNext();
        //        while (rows.MoveNext())
        //        {
        //            IRow row = (XSSFRow)rows.Current;

        //            if (row == null) continue;
        //            if (row.GetCell(0).ToString().Contains("CWY"))
        //                continue;
        //            var rwyCenterPt = new RwyCenterPoint();
        //            rwyCenterPt.ID = row.GetCell(0).ToString();
        //            rwyCenterPt.Lat = row.GetCell(1).ToString();
        //            rwyCenterPt.Long = row.GetCell(2).ToString();
        //            rwyCenterPt.Elev = Convert.ToDouble(row.GetCell(3).ToString());
        //            rwyCenterPt.Geoid = Convert.ToDouble(row.GetCell(4).ToString());
        //            //rwyCenterPt.Name = row.GetCell(4).ToString();
        //            //rwyCenterPt.RMK = row.GetCell(5).ToString();

        //            rwyCenterPt.Y = ARANFunctions.DmsStr2Dd(rwyCenterPt.Lat.Substring(0, rwyCenterPt.Lat.Length - 1), true);
        //            rwyCenterPt.X = ARANFunctions.DmsStr2Dd(rwyCenterPt.Long.Substring(0, rwyCenterPt.Long.Length - 1), false);
        //            rwyCenterPt.Geo = new Aran.Geometries.Point(rwyCenterPt.X, rwyCenterPt.Y);

        //            RwyCenterLineList.Add(rwyCenterPt);
        //        }

        //        var obstSheet = book1[0];

        //        dt = new DataTable();
        //        headerRow = obstSheet.GetRow(0);
        //        rows = obstSheet.GetRowEnumerator();

        //        colCount = headerRow.LastCellNum;
        //        rows.MoveNext();

        //        while (rows.MoveNext())
        //        {
        //            IRow row = (XSSFRow)rows.Current;

        //            if (row == null || row.GetCell(0) == null) continue;
        //            var obstaclePt = new Obstacle();
        //            obstaclePt.Name = row.GetCell(0).ToString();
        //            obstaclePt.Type = row.GetCell(1).ToString();
        //            obstaclePt.Lat = row.GetCell(2).ToString();
        //            obstaclePt.Long = row.GetCell(3).ToString();
        //            obstaclePt.Elev = Convert.ToDouble(row.GetCell(4).ToString());
        //            if (colCount > 6)
        //            {
        //                obstaclePt.Lght = row.GetCell(5).ToString() == "YES" ? true : false;
        //                obstaclePt.Markings = row.GetCell(7).ToString() == "YES" ? true : false;

        //            }
        //            obstaclePt.RMK = row.GetCell(colCount - 1).ToString();

        //            obstaclePt.Y = ARANFunctions.DmsStr2Dd(obstaclePt.Lat.Substring(0, obstaclePt.Lat.Length - 1), true);
        //            obstaclePt.X = ARANFunctions.DmsStr2Dd(obstaclePt.Long.Substring(0, obstaclePt.Long.Length - 1), false);
        //            obstaclePt.Geo = new Aran.Geometries.Point(obstaclePt.X, obstaclePt.Y);

        //            ObstacleList.Add(obstaclePt);
        //        }
        //        NotifyPropertyChanged("RwyCenterCount");
        //        NotifyPropertyChanged("ObstacleCount");
        //    }

        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}

        //private void LoadHrodna()
        //{
        //    try
        //    {

        //        XSSFWorkbook book1 = new XSSFWorkbook(new FileStream(FileName, FileMode.Open));

        //        //double y1 = Aran.Panda.Common.ARANFunctions.DmsStr2Dd("533648.46", true);
        //        //double x1 = Aran.Panda.Common.ARANFunctions.DmsStr2Dd("0240311.36", false);
        //        //var pt1 =_spOperation.ToPrj(new Aran.Geometries.Point(x1, y1));

        //        //double y2 = Aran.Panda.Common.ARANFunctions.DmsStr2Dd("533525.69", true);
        //        //double x2 = Aran.Panda.Common.ARANFunctions.DmsStr2Dd("0240315.84", false);
        //        //var pt2 =_spOperation.ToPrj(new Aran.Geometries.Point(x2, y2));

        //        //double dir = Aran.Panda.Common.ARANFunctions.ReturnAngleInRadians(pt1, pt2);

        //        var sheet = book1[0];

        //        DataTable dt = new DataTable();
        //        IRow headerRow = sheet.GetRow(0);
        //        IEnumerator rows = sheet.GetRowEnumerator();

        //        rows.MoveNext();
        //        while (rows.MoveNext())
        //        {
        //            IRow row = (XSSFRow)rows.Current;

        //            if (row == null || row.GetCell(0) == null) continue;
        //            var obstacle = new Obstacle();
        //            obstacle.Name = row.GetCell(0).ToString();
        //            obstacle.Type = row.GetCell(1).ToString();
        //            obstacle.Lat = row.GetCell(2).ToString();
        //            obstacle.Long = row.GetCell(3).ToString();
        //            obstacle.Elev = Convert.ToDouble(row.GetCell(4).ToString().Replace(',', '.'));

        //            obstacle.RMK = row.GetCell(5).ToString();


        //            obstacle.Y = ARANFunctions.DmsStr2Dd(obstacle.Lat.Substring(0, obstacle.Lat.Length - 1), true);
        //            obstacle.X = ARANFunctions.DmsStr2Dd(obstacle.Long.Substring(0, obstacle.Long.Length - 1), false);
        //            obstacle.Geo = new Aran.Geometries.Point(obstacle.X, obstacle.Y);

        //            ObstacleList.Add(obstacle);

        //        }

        //        var cLineSheet = book1[1];

        //        dt = new DataTable();
        //        headerRow = cLineSheet.GetRow(0);
        //        rows = cLineSheet.GetRowEnumerator();

        //        rows.MoveNext();

        //        while (rows.MoveNext())
        //        {
        //            try
        //            {
        //                IRow row = (XSSFRow)rows.Current;

        //                if (row == null || row.GetCell(0) == null) continue;

        //                var id = row.GetCell(0).ToString();
        //                if (id.Contains("SZ")) continue;

        //                var rwyCenterPt = new RwyCenterPoint();
        //                rwyCenterPt.ID = row.GetCell(0).ToString();
        //                rwyCenterPt.Lat = row.GetCell(1).ToString();
        //                rwyCenterPt.Long = row.GetCell(2).ToString();
        //                rwyCenterPt.Elev = Convert.ToDouble(row.GetCell(3).ToString());

        //                rwyCenterPt.Y = ARANFunctions.DmsStr2Dd(rwyCenterPt.Lat.Substring(0, rwyCenterPt.Lat.Length - 1), true);
        //                rwyCenterPt.X = ARANFunctions.DmsStr2Dd(rwyCenterPt.Long.Substring(0, rwyCenterPt.Long.Length - 1), false);
        //                rwyCenterPt.Geo = new Aran.Geometries.Point(rwyCenterPt.X, rwyCenterPt.Y);

        //                RwyCenterLineList.Add(rwyCenterPt);
        //            }
        //            catch (Exception)
        //            {
        //                continue;

        //            }
        //        }
        //        NotifyPropertyChanged("RwyCenterCount");
        //        NotifyPropertyChanged("ObstacleCount");
        //    }

        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
        //}

        //private void LoadHomiel()
        //{
        //    XSSFWorkbook book1 = new XSSFWorkbook(new FileStream(FileName, FileMode.Open));
        //    var obstSheet = book1[0];

        //    var dt = new DataTable();
        //    var headerRow = obstSheet.GetRow(0);
        //    var rows = obstSheet.GetRowEnumerator();

        //    var colCount = headerRow.LastCellNum;
        //    rows.MoveNext();

        //    while (rows.MoveNext())
        //    {
        //        IRow row = (XSSFRow)rows.Current;

        //        if (row == null || row.GetCell(0) == null) continue;
        //        var obstaclePt = new Obstacle();
        //        obstaclePt.Name = row.GetCell(0).ToString();
        //        obstaclePt.Type = row.GetCell(1).ToString();
        //        obstaclePt.Lat = row.GetCell(2).ToString();
        //        obstaclePt.Long = row.GetCell(3).ToString();
        //        obstaclePt.Markings = row.GetCell(4).ToString() == "YES" ? true : false;
        //        obstaclePt.Lght = row.GetCell(5).ToString() == "YES" ? true : false;
        //        obstaclePt.Elev = Convert.ToDouble(row.GetCell(6).ToString());
        //        obstaclePt.RMK = row.GetCell(colCount - 1).ToString();

        //        obstaclePt.Y = ARANFunctions.DmsStr2Dd(obstaclePt.Lat.Substring(0, obstaclePt.Lat.Length - 1), true);
        //        obstaclePt.X = ARANFunctions.DmsStr2Dd(obstaclePt.Long.Substring(0, obstaclePt.Long.Length - 1), false);
        //        obstaclePt.Geo = new Aran.Geometries.Point(obstaclePt.X, obstaclePt.Y);

        //        ObstacleList.Add(obstaclePt);
        //    }

        //    var cLineSheet = book1[1];

        //    dt = new DataTable();
        //    headerRow = cLineSheet.GetRow(0);
        //    rows = cLineSheet.GetRowEnumerator();

        //    colCount = headerRow.LastCellNum;

        //    bool skipReadingHeaderRow = rows.MoveNext();

        //    double y1 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("523152.08", true);
        //    double x1 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("0305956.424", false);
        //    var pt1 = _spOperation.ToPrj(new Aran.Geometries.Point(x1, y1));

        //    double y2 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("523122.594", true);
        //    double x2 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("0310203.826", false);
        //    var pt2 = _spOperation.ToPrj(new Aran.Geometries.Point(x2, y2));

        //    double dir = Aran.PANDA.Common.ARANFunctions.ReturnAngleInRadians(pt2, pt1);

        //    // rows.MoveNext();
        //    while (rows.MoveNext())
        //    {
        //        try
        //        {
        //            IRow row = (XSSFRow)rows.Current;

        //            if (row == null || row.GetCell(0) == null) continue;

        //            var rwyCenterPt = new RwyCenterPoint();
        //            rwyCenterPt.ID = row.GetCell(0).ToString();
        //            rwyCenterPt.X = Convert.ToDouble(row.GetCell(1).ToString());
        //            rwyCenterPt.Elev = Convert.ToDouble(row.GetCell(2).ToString());

        //            var ptPrj = ARANFunctions.LocalToPrj(pt1, dir, rwyCenterPt.X, rwyCenterPt.Y);
        //            if (rwyCenterPt.ID.Contains("POROG28"))
        //                ptPrj = pt2;
        //            var ptGeo = _spOperation.ToGeo(ptPrj);
        //            rwyCenterPt.Geo = ptGeo;

        //            string longd, lat;
        //            ARANFunctions.Dd2DmsStr(ptGeo.X, ptGeo.Y, ".", "E", "N", 1, 2, out longd, out lat);
        //            rwyCenterPt.Lat = lat;
        //            rwyCenterPt.Long = longd;

        //            RwyCenterLineList.Add(rwyCenterPt);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    NotifyPropertyChanged("RwyCenterCount");
        //    NotifyPropertyChanged("ObstacleCount");
        //}

        //private void LoadMinsk()
        //{
        //    XSSFWorkbook book1 = new XSSFWorkbook(new FileStream(FileName, FileMode.Open));
        //    var obstSheet = book1[0];

        //    var dt = new DataTable();
        //    var headerRow = obstSheet.GetRow(0);
        //    var rows = obstSheet.GetRowEnumerator();

        //    var colCount = headerRow.LastCellNum;
        //    rows.MoveNext();

        //    while (rows.MoveNext())
        //    {
        //        IRow row = (XSSFRow)rows.Current;

        //        if (row == null || row.GetCell(0) == null) continue;
        //        var obstaclePt = new Obstacle();
        //        obstaclePt.Name = row.GetCell(0).ToString();
        //        obstaclePt.Type = row.GetCell(1).ToString();
        //        obstaclePt.Lat = row.GetCell(2).ToString();
        //        obstaclePt.Long = row.GetCell(3).ToString();
        //        obstaclePt.Elev = Convert.ToDouble(row.GetCell(4).ToString());
        //        obstaclePt.Lght = row.GetCell(5).ToString() == "YES" ? true : false;
        //        obstaclePt.Markings = row.GetCell(6).ToString() == "YES" ? true : false;
        //        obstaclePt.RMK = row.GetCell(colCount - 1).ToString();

        //        obstaclePt.Y = ARANFunctions.DmsStr2Dd(obstaclePt.Lat.Substring(0, obstaclePt.Lat.Length - 1), true);
        //        obstaclePt.X = ARANFunctions.DmsStr2Dd(obstaclePt.Long.Substring(0, obstaclePt.Long.Length - 1), false);
        //        obstaclePt.Geo = new Aran.Geometries.Point(obstaclePt.X, obstaclePt.Y);

        //        ObstacleList.Add(obstaclePt);
        //    }

        //    var cLineSheet = book1[1];

        //    dt = new DataTable();
        //    headerRow = cLineSheet.GetRow(0);
        //    rows = cLineSheet.GetRowEnumerator();


        //    double y1 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("535341.14", true);
        //    double x1 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("0280044.88", false);
        //    var pt1 = _spOperation.ToPrj(new Aran.Geometries.Point(x1, y1));

        //    double y2 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("535212.60", true);
        //    double x2 = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd("0280256.28", false);
        //    var pt2 = _spOperation.ToPrj(new Aran.Geometries.Point(x2, y2));

        //    double dir = Aran.PANDA.Common.ARANFunctions.ReturnAngleInRadians(pt1, pt2);

        //    for (int i = 0; i < 9; i++)
        //        rows.MoveNext();
        //    // rows.MoveNext();
        //    while (rows.MoveNext())
        //    {
        //        try
        //        {
        //            IRow row = (XSSFRow)rows.Current;

        //            if (row == null || row.GetCell(0) == null) continue;

        //            var x = Convert.ToDouble(row.GetCell(0).ToString());
        //            if (x > 0 || Math.Abs(x) > 3650) continue;
        //            var rwyCenterPt = new RwyCenterPoint();

        //            rwyCenterPt.ID = row.GetCell(2)?.ToString();
        //            rwyCenterPt.X = Convert.ToDouble(row.GetCell(0).ToString());
        //            rwyCenterPt.Elev = Convert.ToDouble(row.GetCell(1).ToString());

        //            var ptPrj = ARANFunctions.LocalToPrj(pt1, dir + Math.PI, rwyCenterPt.X, rwyCenterPt.Y);
        //            var ptGeo = _spOperation.ToGeo(ptPrj);
        //            rwyCenterPt.Geo = ptGeo;

        //            string longd, lat;
        //            ARANFunctions.Dd2DmsStr(ptGeo.X, ptGeo.Y, ".", "E", "N", 1, 2, out longd, out lat);
        //            rwyCenterPt.Lat = lat;
        //            rwyCenterPt.Long = longd;

        //            RwyCenterLineList.Add(rwyCenterPt);
        //        }
        //        catch (Exception)
        //        {
        //            continue;
        //        }
        //    }

        //    NotifyPropertyChanged("RwyCenterCount");
        //    NotifyPropertyChanged("ObstacleCount");
        //}
    }
}
