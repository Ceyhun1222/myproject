using System.Collections.Generic;
using Aran.Panda.Constants;

namespace Holding
{
	public class SpeedCategories
	{
		public SpeedCategories()
		{

			_altitudeIndex = -1;
			_aircraftConstantList = GlobalParams.Constant_G.AircraftCategory;
			_trbValues = new List<double>();
			_initialSegment = new List<Interval>();
			_onroute = new List<Interval>();
		}

		public List<double> TurbuLences
		{
			get { return _trbValues; }
			set { _trbValues = value; }
		}

		public List<Interval> InitialSegment
		{
			get { return _initialSegment; }
			set { _initialSegment = value; }
		}

		public List<Interval> Onroute
		{
			get { return _onroute; }
			set { _onroute = value; }
		}

		public double Altitude
		{
			set
			{
				if (HasChangedIndex(value))
				{
					//fillProperties();
				}
				_altitude = value;
			}
		}

		private bool HasChangedIndex(double value)
		{
			int altindex = 0;
			if ((value >= Common.MinAltitude) & (value < Common.SecondAltitude))
				altindex = 0;
			else if ((value >= Common.SecondAltitude) & (value < Common.ThirdAltitude))
				altindex = 1;
			else if ((value >= Common.ThirdAltitude) & (value <= Common.MaxAltitude))
				altindex = 2;

			if (_altitudeIndex != altindex)
			{
				_altitudeIndex = altindex;
				return true;
			}
			else
			{
				return false;
			}
		}

        //private void fillProperties()
        //{

        //    if (_altitudeIndex == 0)
        //    {
        //        setTurbuLences(_trbValues, _aircraftConstantList[aircraftCategoryData.hldIASupTo4250Turb]);
        //        setInterVals(_initialSegment, _aircraftConstantList[aircraftCategoryData.hldIASupTo4250Min], _aircraftConstantList[aircraftCategoryData.hldIASupTo4250Max]);
        //        setInterVals(_onroute, _aircraftConstantList[aircraftCategoryData.hldIASupTo4250], _aircraftConstantList[aircraftCategoryData.hldIASupTo4250Turb]);

        //    }
        //    else if (_altitudeIndex == 1)
        //    {
        //        setTurbuLences(_trbValues, _aircraftConstantList[aircraftCategoryData.hldIASupTo6100Turb]);
        //        setInterVals(_initialSegment, _aircraftConstantList[aircraftCategoryData.hldIASupTo6100Min], _aircraftConstantList[aircraftCategoryData.hldIASupTo6100Max]);

        //        setInterVals(_onroute, _aircraftConstantList[aircraftCategoryData.hldIASupTo6100], _aircraftConstantList[aircraftCategoryData.hldIASupTo6100Turb]);
        //    }
        //    else if (_altitudeIndex == 2)
        //    {
        //        setTurbuLences(_trbValues, _aircraftConstantList[aircraftCategoryData.hldIASupTo10350Turb]);
        //        setInterVals(_initialSegment, _aircraftConstantList[aircraftCategoryData.hldIASupTo10350Min], _aircraftConstantList[aircraftCategoryData.hldIASupTo10350Max]);
        //        setInterVals(_onroute, _aircraftConstantList[aircraftCategoryData.hldIASupTo10350], _aircraftConstantList[aircraftCategoryData.hldIASupTo10350Turb]);
        //    }
        //}

        //private void setTurbuLences(List<double> trbValues, AircraftCategoryConstant categoriesData)
        //{
        //    trbValues.Clear();
        //    trbValues.Add(categoriesData[aircraftCategory.acA]);
        //    trbValues.Add(categoriesData[aircraftCategory.acB]);
        //    trbValues.Add(categoriesData[aircraftCategory.acC]);
        //    trbValues.Add(categoriesData[aircraftCategory.acD]);
        //    trbValues.Add(categoriesData[aircraftCategory.acE]);
        //}

        //private void setInterVals(List<Interval> listIntervals, AircraftCategoryConstant minCategoriesData, AircraftCategoryConstant maxCategoriesData)
        //{
        //    listIntervals.Clear();
        //    listIntervals.Add(new Interval(minCategoriesData[aircraftCategory.acA], maxCategoriesData[aircraftCategory.acA]));
        //    listIntervals.Add(new Interval(minCategoriesData[aircraftCategory.acB], maxCategoriesData[aircraftCategory.acB]));
        //    listIntervals.Add(new Interval(minCategoriesData[aircraftCategory.acC], maxCategoriesData[aircraftCategory.acC]));
        //    listIntervals.Add(new Interval(minCategoriesData[aircraftCategory.acD], maxCategoriesData[aircraftCategory.acD]));
        //    listIntervals.Add(new Interval(minCategoriesData[aircraftCategory.acE], maxCategoriesData[aircraftCategory.acE]));

        //}

		private List<double> _trbValues;
		private List<Interval> _initialSegment;
		private List<Interval> _onroute;
		private int _altitudeIndex;
		private AircraftCategoryList _aircraftConstantList;

		private double _altitude;
	}

	public class Interval
	{
		public Interval(double left, double right)
		{
			Left = left;
			Right = right;
		}

		public double Left { get; set; }

		public double Right { get; set; }

		public int Tag { get; set; }


	}
}
