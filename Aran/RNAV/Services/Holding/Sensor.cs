using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Holding
{
	
	public enum Reciever
	{ 
		GNSS = 0,
		DMEDME= 1,
		VORDME = 2,
        VORNDB = 3,
        VORVOR = 4
	}

	public enum FlightPhase
	{ 
	
	}

    public enum PointType
    {
        Create,
        Select
    }

	//public class Sheet
	//{
	//    public Sheet(FlightType flightType,bool[,] cell,string[] column,string[] row)
	//    {
	//        Row = row;
	//        Column = column;
	//        Cell = cell;
	//        FlightType = flightType;
	//        RowCount = row.Length;
	//        ColumnCount = column.Length;
	//    }
				
	//    public Array Row { get; private set; }
	//    public Array Column { get; private set; }
	//    public Array Cell{get;private set;}
	//    public int RowCount { get; private set; }
	//    public int ColumnCount{get;private set;}
	//    public FlightType FlightType { get; private set; }

	//}

	//public class SheetList:IEnumerable
	//{
	//    public SheetList()
	//    {
	//        sheetList = new List<Sheet>();
	//    }

	//    public SheetList(params Sheet[] sheets)
	//    {
	//        sheetList = new List<Sheet>();
	//        foreach (Sheet item in sheets)
	//        {
	//            sheetList.Add(item);								
	//        }
	//    }

	//    public void Add(Sheet sheet)
	//    {
	//        sheetList.Add(sheet);		
	//    }

	//    public FlightType SheetName(int index)
	//    {
	//        return sheetList[index].FlightType;		
	//    }

	//    public int Count
	//    {
	//        get { return sheetList.Count; }
	//    }

	//    public Sheet this[int index]
	//    {
	//        get {return sheetList[index];}
	//    }
				
	//    public List<Sheet> sheetList;
	//    public List<string> name;

	//    #region IEnumerable Members

	//    public IEnumerator GetEnumerator()
	//    {
	//        return sheetList.GetEnumerator();
	//    }

	//    #endregion
	//}
}


