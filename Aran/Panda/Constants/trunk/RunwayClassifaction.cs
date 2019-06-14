using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.PANDA.Common;
using System.Data.SQLite;

namespace Aran.PANDA.Constants
{
    public enum RunwayClassificationType
    { 
        NonInstrument,
        NonPrecisionApproach,
        PrecisionApproach
    }

    public enum SurfaceType
    {
        CONICAL,
        InnerHorizontal,
        InnerApproach,
        Approach,
        Transitional,
        InnerTransitional,
        BalkedLanding,
        TakeOffClimb,
        OuterHorizontal,
        Strip,
        TakeOffFlihtPathArea,
        Area2A
        
    }
    public enum EtodSurfaceType 
    {
        None,
        Area1,
        Area2A,
        Area2B,
        Area2C,
        Area2D,
        Area3,
        Area4,
        Area2
    }

    public enum DimensionType 
    {
        Slope,
        Height,
        Radius,
        Width,
        DistanceFromThreshold,
        Length,
        LengthOfInnerEdge,
        Divergence,
        DistanceFromRunwayEnd,
        FinalWidth,
        FirstSectionLength,
        FirstSectionSlope,
        SecondSectionLength,
        SecondSectionSlope,
        HorizontalSectoinLength,
        HorizontalSectionTotalLength
    }

    public enum CategoryNumber { 
        One =0,
        Two =1,
        Three =2,
        TwoThree = 3,
        None =4
    }

    [Serializable]
    public class RunwayConstansList
    {
        private readonly string[] SurfaceNames = new string[]{
            "CONICAL","INNER HORIZONTAL","INNER APPROACH (OFZ)",
            "APPROACH","TRANSITIONAL","INNER TRANSITIONAL (OFZ)",
            "BALKED LANDING (OFZ)","TAKE-OFF CLIMB","OUTER HORIZONTAL","STRIP","TAKEOFFLIGHTPATHAREA","AREA2A"
        };

        private readonly string[] DimensionNames = new string[]{
            "Slope","Height","Radius","Width","Distance from threshold",
            "Length","Length of inner edge","Divergence(each side)","Distance from runway end",
            "Final width","First section(Length)","First section(Slope)","Second section(Length)",
            "Second section(Slope)","Horizontal section(Length)","Horizontal section(Total Length)"
        };


        #region :> Fields
        private SQLiteConnection _conn;
        private SQLiteCommand _command;
        private SQLiteDataReader _reader;
        private EnumArray<string, SurfaceType> surfaceEnumArray;
        #endregion

        #region :>Ctor
        public RunwayConstansList(string installDir)
        {
            surfaceEnumArray = new EnumArray<string, SurfaceType>();
            int i = -1;
            foreach (SurfaceType item in Enum.GetValues(typeof(SurfaceType)))
            {
                i++;
                surfaceEnumArray[item] = SurfaceNames[i];
            }

            List = new List<RunwayConstants>();
            string connString = String.Format("Data Source={0};New=False;Version=3", installDir + @"\ApproachRunway.s3db");
            if (!OpenConnection(connString))
            {
                MessageBox.Show("File path isn't true");
                throw new Exception("Cannot open file");
            }
        }
        #endregion

        public RunwayConstants this[SurfaceType surfaceType, DimensionType dimensionType] 
        {
            get
            {
                foreach (RunwayConstants rwyConstants in this.List)
                {
                    if (rwyConstants.Surface == surfaceType && rwyConstants.Dimension == dimensionType)
                        return rwyConstants;
                }
                return null;
            }
        }

        public List<RunwayConstants> List { get;private set; }

        #region :>Methods
        
        public void Load()
        {
            try
            {
                if (List.Count > 0)
                    return;
                _command = _conn.CreateCommand();
                _command.CommandText = "select id,surface,dimension,unit,col1,col2,col3,col4,col5,col6,col7,col8,col9,col10,comment,orders from approachrunways";
                _reader = _command.ExecuteReader();
                while (_reader.Read())
                {
                    RunwayConstants rwyConstant = new RunwayConstants();
                    rwyConstant.Id = Convert.ToInt32(_reader[0]);
                    rwyConstant.SurfaceName = _reader[1].ToString();
                    rwyConstant.Surface = (SurfaceType)Array.FindLastIndex(SurfaceNames, surface => surface == _reader[1].ToString());
                    rwyConstant.Dimension = (DimensionType)Array.FindIndex(DimensionNames, dimension => dimension == _reader[2].ToString());
                    rwyConstant.Unit = _reader[3].ToString();
                    rwyConstant.Order = Convert.ToInt32(_reader[15]);
                    if (rwyConstant.Surface == SurfaceType.TakeOffClimb)
                    {
                        CategoryValue tmpVal = new CategoryValue();
                        tmpVal[0] = Convert.ToDouble(_reader[4]);
                        tmpVal[1] = Convert.ToDouble(_reader[5]);
                        tmpVal[2] = Convert.ToDouble(_reader[6]);
                        tmpVal[3] = Convert.ToDouble(_reader[7]);
                        rwyConstant.NonIstrument.CategoryValues.Add(tmpVal);
                        rwyConstant.NonPrecisionApprocah.CategoryValues.Add(tmpVal);
                        rwyConstant.PrecisionApproach.CategoryValues.Add(tmpVal);
                    }
                    else
                    {
                        rwyConstant.NonIstrument.Type = RunwayClassificationType.NonInstrument;
                        CategoryValue tmpVal = new CategoryValue();
                        if (_reader[4] != DBNull.Value)
                            tmpVal[0] = Convert.ToDouble(_reader[4]);

                        if (_reader[5] != DBNull.Value)
                            tmpVal[1] = Convert.ToDouble(_reader[5]);
                        if (_reader[6] != DBNull.Value)
                            tmpVal[2] = Convert.ToDouble(_reader[6]);
                        if (_reader[7] != DBNull.Value)
                            tmpVal[3] = Convert.ToDouble(_reader[7]);
                        rwyConstant.NonIstrument.CategoryValues.Add(tmpVal);

                        rwyConstant.NonPrecisionApprocah.Type = RunwayClassificationType.NonPrecisionApproach;
                        tmpVal = new CategoryValue();
                        if (_reader[8] != DBNull.Value)
                            tmpVal[0] = Convert.ToDouble(_reader[8]);
                        if (_reader[8] != DBNull.Value)
                            tmpVal[1] = Convert.ToDouble(_reader[8]);
                        if (_reader[9] != DBNull.Value)
                            tmpVal[2] = Convert.ToDouble(_reader[9]);
                        if (_reader[10] != DBNull.Value)
                            tmpVal[3] = Convert.ToDouble(_reader[10]);
                        rwyConstant.NonPrecisionApprocah.Type = RunwayClassificationType.NonPrecisionApproach;
                        rwyConstant.NonPrecisionApprocah.CategoryValues.Add(tmpVal);

                        rwyConstant.PrecisionApproach.Type = RunwayClassificationType.PrecisionApproach;
                        CategoryValue tmpCat1 = new CategoryValue(CategoryNumber.One);
                        if (_reader[11] != DBNull.Value)
                        {
                            tmpCat1[0] = Convert.ToDouble(_reader[11]);
                            tmpCat1[1] = Convert.ToDouble(_reader[11]);
                        }
                        if (_reader[12] != DBNull.Value)
                        {
                            tmpCat1[2] = Convert.ToDouble(_reader[12]);
                            tmpCat1[3] = Convert.ToDouble(_reader[12]);
                        }

                        CategoryValue tmpCat2 = new CategoryValue(CategoryNumber.Two);
                        if (_reader[13] != DBNull.Value)
                        {
                            tmpCat2[0] = 0;
                            tmpCat2[1] = 0;
                            tmpCat2[2] = Convert.ToDouble(_reader[13]);
                            tmpCat2[3] = Convert.ToDouble(_reader[13]);
                        }

                        CategoryValue tmpCat3 = new CategoryValue(CategoryNumber.Three);
                        if (_reader[13] != DBNull.Value)
                        {
                            tmpCat3[0] = 0;
                            tmpCat3[1] = 0;
                            tmpCat3[2] = Convert.ToDouble(_reader[13]);
                            tmpCat3[3] = Convert.ToDouble(_reader[13]);
                        }
                        rwyConstant.PrecisionApproach.CategoryValues.Add(tmpCat1);
                        rwyConstant.PrecisionApproach.CategoryValues.Add(tmpCat2);
                        rwyConstant.PrecisionApproach.CategoryValues.Add(tmpCat3);
                    }
                    List.Add(rwyConstant);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error with loading RunwayConstants");
            }
        }

        private bool OpenConnection(string pathFolder)
        {
            try
            {
                _conn = new SQLiteConnection(pathFolder);
                _conn.Open();
                return true;
            }
            catch (Exception)
            {
                try
                {
                    _conn = new SQLiteConnection(String.Format("Data Source={0};New=False;Version=3", "ApproachRunway.s3db"));
                    _conn.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }
            }
        }
   
        private void CloseConnection()
        {
            _conn.Close();
        }
        #endregion
    }

    [Serializable]
    public class RunwayConstants
    {
        private List<double> _values; 
        public RunwayConstants()
        {
            _values = new List<double>();
            NonIstrument = new RunwayClassification(RunwayClassificationType.NonInstrument);
            NonPrecisionApprocah = new RunwayClassification(RunwayClassificationType.NonPrecisionApproach);
            PrecisionApproach = new RunwayClassification(RunwayClassificationType.PrecisionApproach);
        }

        public double this[int index]
        {
            get { return _values[index]; }
            set
            {
                if (index < _values.Count)
                    _values[index] = value;
                else
                    _values.Add(value);
            }
        }

        public double GetValue(RunwayClassificationType classifationType, CategoryNumber catNumber, int codeNumber)
        {
            double result = 0;
            if (classifationType == RunwayClassificationType.NonInstrument)
            {
                if (this.NonIstrument.CategoryValues.Count > (int)catNumber)
                    result = this.NonIstrument.CategoryValues[(int)catNumber][codeNumber - 1];
            }
            else if (classifationType == RunwayClassificationType.NonPrecisionApproach)
            {
                if (this.NonPrecisionApprocah.CategoryValues.Count > (int)catNumber)
                    result = this.NonPrecisionApprocah.CategoryValues[(int)catNumber][codeNumber - 1];
            }
            else
            {
                if (this.PrecisionApproach.CategoryValues.Count > (int)catNumber)
                    result = this.PrecisionApproach.CategoryValues[(int)catNumber][codeNumber - 1];
            }
            return result;
        
        }

        public int Id { get; set; }
        public string SurfaceName { get; set; }
        public SurfaceType Surface { get; set; }
        public DimensionType Dimension { get; set; }
        public string Unit { get; set; }
        public RunwayClassification NonIstrument { get; set; }
        public RunwayClassification NonPrecisionApprocah { get; set; }
        public RunwayClassification PrecisionApproach { get; set; }
        public int Order { get; set; }
        
    }

    [Serializable]
    public class RunwayClassification 
    {
        public RunwayClassification(RunwayClassificationType classificationType)
        {
            CategoryValues = new List<CategoryValue>();
            Type = classificationType;
        }

        public RunwayClassificationType Type { get; set; }
        public List<CategoryValue> CategoryValues { get; set; }
    }

    [Serializable]
    public class CategoryValue {
        
        public CategoryValue()
        {
            CatNumber = CategoryNumber.None;
            _values = new List<double>();
            for (int i = 0; i < 4; i++)
            {
                _values.Add(0);
            }
        }
        public CategoryValue(CategoryNumber number)
        {
            CatNumber = number;
            _values = new List<double>();
            for (int i = 0; i < 4; i++)
            {
                _values.Add(0);
            }
        }
        
        public double this[int index]
        {
            get
            {   if (index<_values.Count) 
                    return _values[index];
                
                return 0;
            }
            set 
            {
                if (index < _values.Count)
                    _values[index] = value;
                else
                    _values.Add(value);
            }
        }

        public CategoryNumber CatNumber { get; set; }

        public int Count { get { return _values.Count; } }
        private List<double> _values;
    }
}
