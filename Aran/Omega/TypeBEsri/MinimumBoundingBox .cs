using System;

namespace Aran.Omega.TypeBEsri
{
    public enum ShapeType
    {
        POLYGON,
        POLYLINE
    }

    public enum MinimizationCriterion {

        AREA, PERIMETER
    }

//    public class MinimumBoundingBox : WhiteboxPlugin
//    {

//        private string[] args;

//        public virtual string Name
//        {
//            get { return "MinimumBoundingBox"; }
//        }

//        public string DescriptiveName
//        {
//            get { return "Minimum Bounding Box"; }
//        }

//        public string ToolDescription
//        {
//            get { return "Identfies the minimum bounding box around vector polygons or lines."; }
//        }

//        public string[] Toolbox
//        {
//            get
//            {
//                string[] ret = new string[] {"VectorTools"};
//                return ret;
//            }
//        }

//        private int previousProgress = 0;
//        private string previousProgressLabel = "";

      
     

//        public void run()
//        {
//            amIActive = true;
//            string inputFile;
//            double x, y;
//            int progress;
//            int oldProgress;
//            int i, n;
//            double[][] vertices = null;
//            int numPolys = 0;
//            ShapeType shapeType, outputShapeType = ShapeType.POLYLINE;
//            int[] parts = new int[] {0};
//            double psi = 0;

//            if (args.length <= 0)
//            {
//                showFeedback("Plugin parameters have not been set.");
//                return;
//            }

//            inputFile = args[0];
//            MinimizationCriterion minimizationCriteria = MinimizationCriterion.AREA;
//            if (args[1].ToLower().Contains("peri"))
//            {
//                minimizationCriteria = MinimizationCriterion.PERIMETER;
//            }
//            string outputFile = args[2];
//            if (args[3].ToLower().Contains("true"))
//            {
//                outputShapeType = ShapeType.POLYGON;
//            }
//            else
//            {
//                outputShapeType = ShapeType.POLYLINE;
//            }

//            // check to see that the inputHeader and outputHeader are not null.
//            if ((inputFile == null))
//            {
//                showFeedback("One or more of the input parameters have not been set properly.");
//                return;
//            }

//            try
//            {
//                // set up the input shapefile.
//                ShapeFile input = new ShapeFile(inputFile);
//                shapeType = input.ShapeType;
//                numPolys = input.NumberOfRecords;

//                DBFField[] fields = new DBFField[4];

//                fields[0] = new DBFField();
//                fields[0].Name = "PARENT_ID";
//                fields[0].DataType = DBFField.DBFDataType.NUMERIC;
//                fields[0].FieldLength = 10;
//                fields[0].DecimalCount = 0;

//                fields[1] = new DBFField();
//                fields[1].Name = "SHRT_AXIS";
//                fields[1].DataType = DBFField.DBFDataType.NUMERIC;
//                fields[1].FieldLength = 10;
//                fields[1].DecimalCount = 3;

//                fields[2] = new DBFField();
//                fields[2].Name = "LNG_AXIS";
//                fields[2].DataType = DBFField.DBFDataType.NUMERIC;
//                fields[2].FieldLength = 10;
//                fields[2].DecimalCount = 3;

//                fields[3] = new DBFField();
//                fields[3].Name = "ELONGATION";
//                fields[3].DataType = DBFField.DBFDataType.NUMERIC;
//                fields[3].FieldLength = 10;
//                fields[3].DecimalCount = 3;

//                ShapeFile output = new ShapeFile(outputFile, outputShapeType, fields);

//                MinimumBoundingRectangle mbr = new MinimumBoundingRectangle(minimizationCriteria);
//                int recordNum;
//                if (shapeType == ShapeType.POLYGON || shapeType == ShapeType.POLYLINE)
//                {
//                    oldProgress = -1;
//                    foreach (ShapeFileRecord record in input.records)
//                    {
//                        recordNum = record.RecordNumber;
//                        vertices = record.Geometry.Points;

//                        mbr.Coordinates = vertices;
//                        double[][] points = mbr.GetBoundingBox();

//                        object[] rowData = new object[4];
//                        rowData[0] = (double) recordNum;
//                        rowData[1] = mbr.ShortAxisLength;
//                        rowData[2] = mbr.LongAxisLength;
//                        rowData[3] = mbr.ElongationRatio;

//                        Geometry poly;
//                        if (outputShapeType == ShapeType.POLYLINE)
//                        {
//                            poly = new PolyLine(parts, points);
//                        }
//                        else
//                        {
//                            poly = new Polygon(parts, points);
//                        }
//                        output.addRecord(poly, rowData);

//                        progress = (int) ((recordNum*100.0)/numPolys);
//                        if (progress != oldProgress)
//                        {
//                            updateProgress(progress);
//                            oldProgress = progress;
//                            if (cancelOp)
//                            {
//                                cancelOperation();
//                                return;
//                            }
//                        }
//                    }
//                } // point or multipoint basetype
//                else
//                {

//                    List<double?> pointsX = new List<double?>();
//                    List<double?> pointsY = new List<double?>();

//                    oldProgress = -1;
//                    foreach (ShapeFileRecord record in input.records)
//                    {
//                        recordNum = record.RecordNumber;
//                        vertices = record.Geometry.Points;
//                        int numVertices = vertices.length;

//                        for (i = 0; i < numVertices; i++)
//                        {
//                            pointsX.Add(vertices[i][0]);
//                            pointsY.Add(vertices[i][1]);
//                        }

//                        progress = (int) ((recordNum*100.0)/numPolys);
//                        if (progress != oldProgress)
//                        {
//                            updateProgress(progress);
//                            if (cancelOp)
//                            {
//                                cancelOperation();
//                                return;
//                            }
//                        }
//                        oldProgress = progress;

//                    }

////JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
////ORIGINAL LINE: vertices = new double[pointsX.Count][2];
//                    vertices = RectangularArrays.ReturnRectangularDoubleArray(pointsX.Count, 2);
//                    for (i = 0; i < vertices.length; i++)
//                    {
//                        vertices[i][0] = pointsX[i];
//                        vertices[i][1] = pointsY[i];
//                    }

//                    mbr.Coordinates = vertices;
//                    double[][] points = mbr.BoundingBox;

//                    object[] rowData = new object[4];
//                    rowData[0] = 1.0d;
//                    rowData[1] = mbr.ShortAxisLength;
//                    rowData[2] = mbr.LongAxisLength;
//                    rowData[3] = mbr.ElongationRatio;

//                    Geometry poly;
//                    if (outputShapeType == ShapeType.POLYLINE)
//                    {
//                        poly = new PolyLine(parts, points);
//                    }
//                    else
//                    {
//                        poly = new Polygon(parts, points);
//                    }
//                    output.addRecord(poly, rowData);

//                }
//                output.write();

//                // returning a header file string displays the image.
//                updateProgress("Displaying vector: ", 0);
//                returnData(outputFile);

//            }
//            catch (System.OutOfMemoryException)
//            {
//                myHost.showFeedback("An out-of-memory error has occurred during operation.");
//            }

//        }
//    }
}