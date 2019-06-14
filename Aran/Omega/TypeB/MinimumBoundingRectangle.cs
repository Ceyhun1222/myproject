//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Aran.Omega.TypeB
//{
//    public class MinimumBoundingRectangle
//    {
//        internal double[][] coordinates;
//        internal readonly double rightAngle = Math.PI / 2.0;
//        internal bool minimizeArea = true;
//        internal double boxCentreX;
//        internal double boxCentreY;
//        internal double shortAxis;
//        internal double longAxis;
//        internal double longAxisOrientation;
//        internal double slope;
//        internal bool boxCalculated = false;

//        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
//        //ORIGINAL LINE: public MinimumBoundingRectangle()
//        public MinimumBoundingRectangle()
//        {

//        }

//        //JAVA TO C# CONVERTER WARNING: The following constructor is declared outside of its associated class:
//        //ORIGINAL LINE: public MinimumBoundingRectangle(MinimizationCriterion criterion)
//        public MinimumBoundingRectangle(MinimizationCriterion criterion)
//        {
//            minimizeArea = criterion == MinimizationCriterion.AREA;
//        }

//        public MinimumBoundingRectangle(double[][] coordinates, MinimizationCriterion criterion)
//        {
//            this.coordinates = coordinates;
//            minimizeArea = criterion == MinimizationCriterion.AREA;
//        }

//        public virtual double[][] Coordinates
//        {
//            set
//            {
//                this.coordinates = value;
//                boxCalculated = false;
//            }
//        }

//        public virtual MinimizationCriterion MinimizationCriterion
//        {
//            set { minimizeArea = value == MinimizationCriterion.AREA; }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double[] getBoxCentrePoint() throws Exception
//        public virtual double[] BoxCentrePoint
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }
//                return new double[] { boxCentreX, boxCentreY };
//            }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double getLongAxisLength() throws Exception
//        public virtual double LongAxisLength
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }
//                return longAxis;
//            }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double getShortAxisLength() throws Exception
//        public virtual double ShortAxisLength
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }
//                return shortAxis;
//            }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double getElongationRatio() throws Exception
//        public virtual double ElongationRatio
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }
//                return 1 - shortAxis / longAxis;
//            }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double getLongAxisOrientation() throws Exception
//        public virtual double LongAxisOrientation
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }
//                return 90 + Aran.Panda.Common.ARANMath.RadToDeg(Math.Atan(Math.Tan(-slope)));
//            }
//        }

//        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//        //ORIGINAL LINE: public double getShortAxisOrientation() throws Exception
//        public virtual double ShortAxisOrientation
//        {
//            get
//            {
//                if (!boxCalculated)
//                {
//                    GetBoundingBox();
//                }

//                double orient = LongAxisOrientation;
//                return (orient >= 90) ? orient - 90 : orient + 90;
//            }
//        }

//        public virtual double[,] GetBoundingBox()
//        {
//            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//            //ORIGINAL LINE: double[][] ret = new double[5][2];
//            double[,] ret = new double[5, 2];
//            int numPoints = coordinates.Length;
//            Coordinate[] coords = new Coordinate[numPoints];

//            double east = double.NegativeInfinity;
//            double west = double.PositiveInfinity;
//            double north = double.NegativeInfinity;
//            double south = double.PositiveInfinity;

//            for (int i = 0; i < numPoints; i++)
//            {
//                if (coordinates[i][0] > east)
//                {
//                    east = coordinates[i][0];
//                }
//                if (coordinates[i][0] < west)
//                {
//                    west = coordinates[i][0];
//                }
//                if (coordinates[i][1] > north)
//                {
//                    north = coordinates[i][1];
//                }
//                if (coordinates[i][1] < south)
//                {
//                    south = coordinates[i][1];
//                }
//                coords[i] = new Coordinate(coordinates[i][0], coordinates[i][1]);
//            }


//            double midX = west + (east - west) / 2.0;
//            double midY = south + (north - south) / 2.0;

//          //  ConvexHull hull = new ConvexHull(coords, factory);
//            Coordinate[] hullPoints = new Coordinate[100];// hull.ConvexHull.Coordinates;

//            int numHullPoints = hullPoints.Length;
//            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//            //ORIGINAL LINE: double[][] verticesRotated = new double[numHullPoints][2];
//            double[,] verticesRotated = new double[numHullPoints, 2];
//            double[] newBoundingBox = new double[4];
//            double[] axes = new double[2];
//            axes[0] = 9999999;
//            axes[1] = 9999999;
//            double x, y;
//            slope = 0;
//            boxCentreX = 0;
//            boxCentreY = 0;
//            // Rotate the hull points to align with the orientation of each side in order.
//            for (int m = 0; m < numHullPoints - 1; m++)
//            {
//                double xDiff = hullPoints[m + 1].X - hullPoints[m].X;
//                double yDiff = hullPoints[m + 1].Y - hullPoints[m].Y;
//                double psi = -Math.Atan2(yDiff, xDiff);
//                // Rotate each edge cell in the array by m degrees.
//                for (int n = 0; n < numHullPoints; n++)
//                {
//                    x = hullPoints[n].X - midX;
//                    y = hullPoints[n].Y - midY;
//                    verticesRotated[n,0] = (x * Math.Cos(psi)) - (y * Math.Sin(psi));
//                    verticesRotated[n,1] = (x * Math.Sin(psi)) + (y * Math.Cos(psi));
//                }
//                // calculate the minimum area bounding box in this coordinate 
//                // system and see if it is less
//                newBoundingBox[0] = double.MaxValue; // west
//                newBoundingBox[1] = double.Epsilon; // east
//                newBoundingBox[2] = double.MaxValue; // north
//                newBoundingBox[3] = double.Epsilon; // south
//                for (int n = 0; n < numHullPoints; n++)
//                {
//                    x = verticesRotated[n,0];
//                    y = verticesRotated[n,1];
//                    if (x < newBoundingBox[0])
//                    {
//                        newBoundingBox[0] = x;
//                    }
//                    if (x > newBoundingBox[1])
//                    {
//                        newBoundingBox[1] = x;
//                    }
//                    if (y < newBoundingBox[2])
//                    {
//                        newBoundingBox[2] = y;
//                    }
//                    if (y > newBoundingBox[3])
//                    {
//                        newBoundingBox[3] = y;
//                    }
//                }
//                double newXAxis = Math.Abs(newBoundingBox[1] - newBoundingBox[0]);
//                double newYAxis = Math.Abs(newBoundingBox[3] - newBoundingBox[2]);
//                double newValue = minimizeArea ? newXAxis * newYAxis : (newXAxis + newYAxis) * 2;
//                double currentValue = minimizeArea ? axes[0] * axes[1] : (axes[0] + axes[1]) * 2;
//                if (newValue < currentValue) // minimize the metric of the bounding box.
//                {
//                    axes[0] = newXAxis;
//                    axes[1] = newYAxis;

//                    if (axes[0] > axes[1])
//                    {
//                        slope = -psi;
//                    }
//                    else
//                    {
//                        slope = -(rightAngle + psi);
//                    }
//                    x = newBoundingBox[0] + newXAxis / 2;
//                    y = newBoundingBox[2] + newYAxis / 2;
//                    boxCentreX = midX + (x * Math.Cos(-psi)) - (y * Math.Sin(-psi));
//                    boxCentreY = midY + (x * Math.Sin(-psi)) + (y * Math.Cos(-psi));
//                }
//            }
//            longAxis = Math.Max(axes[0], axes[1]);
//            shortAxis = Math.Min(axes[0], axes[1]);

//            //JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//            //ORIGINAL LINE: double[][] axesEndPoints = new double[4][2];
//            double[,] axesEndPoints = new double[4,2];
//            axesEndPoints[0,0] = boxCentreX + longAxis / 2.0 * Math.Cos(slope);
//            axesEndPoints[0,1] = boxCentreY + longAxis / 2.0 * Math.Sin(slope);
//            axesEndPoints[1,0] = boxCentreX - longAxis / 2.0 * Math.Cos(slope);
//            axesEndPoints[1,1] = boxCentreY - longAxis / 2.0 * Math.Sin(slope);
//            axesEndPoints[2,0] = boxCentreX + shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            axesEndPoints[2,1] = boxCentreY + shortAxis / 2.0 * Math.Sin(rightAngle + slope);
//            axesEndPoints[3,0] = boxCentreX - shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            axesEndPoints[3,1] = boxCentreY - shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            ret[0,0] = axesEndPoints[0,0] + shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            ret[0,1] = axesEndPoints[0,1] + shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            ret[1,0] = axesEndPoints[0,0] - shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            ret[1,1] = axesEndPoints[0,1] - shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            ret[2,0] = axesEndPoints[1,0] - shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            ret[2,1] = axesEndPoints[1,1] - shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            ret[3,0] = axesEndPoints[1,0] + shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            ret[3,1] = axesEndPoints[1,1] + shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            ret[4,0] = axesEndPoints[0,0] + shortAxis / 2.0 * Math.Cos(rightAngle + slope);
//            ret[4,1] = axesEndPoints[0,1] + shortAxis / 2.0 * Math.Sin(rightAngle + slope);

//            boxCalculated = true;
//            return ret;
//        }
//    }

//    internal static partial class RectangularArrays
//    {
//        internal static double[][] ReturnRectangularDoubleArray(int size1, int size2)
//        {
//            double[][] array = null;
//            if (size1 > -1)
//            {
//                array = new double[size1][];
//                if (size2 > -1)
//                {
//                    for (int index = 0; index < size1; index++)
//                    {
//                        array[index] = new double[size2];
//                    }
//                }
//            }

//            return array;
//        }
//    }

//    internal class Coordinate
//    {
//        public Coordinate(double x, double y)
//        {
//            // TODO: Complete member initialization
//            this.X = x;
//            this.Y = y;
//        }
//        public double X { get; set; }
//        public double  Y { get; set; }
//    }
//}