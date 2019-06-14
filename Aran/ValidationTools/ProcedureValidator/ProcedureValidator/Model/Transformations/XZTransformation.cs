using System;
using PVT.Model.Geometry;
using PVT.Model.Plot;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PVT.Model.Transformations
{
    public class XZTransformation : CommonTransformation3D
    {
        public PlotModel<Geometry3D> PlotModel { get; private set; }
        public Point3D Centre { get; }
        public double Angle { get; set; }

        public double Scale { get; set; }

        private Matrix<double> transfromMatrix;
        private Matrix<double> rotationMatrix;
        private Matrix<double> transpositionMatrix;
        private Matrix<double> inverseTranspositionMatrix;

        public XZTransformation(PlotModel<Geometry3D> plotModel, Point3D centre, double angle)
        {
            PlotModel = plotModel;
            Centre = centre;
            Angle = angle;
            CalculateTransformMatrix();
        }

        public void Refresh()
        {
            CalculateTransformMatrix();
        }


        protected override Point2D Transform(Point3D point3D)
        {
            var point = rotationMatrix.Multiply(transfromMatrix.Multiply(ToVector(point3D)));
            return ToProjection(ToPoint(point));
        }

        private void CalculateTransformMatrix()
        {

            transfromMatrix = DenseMatrix.OfArray(new double[,] {
                {1, 0, 0,  -Centre.X },
                {0, 1, 0, -Centre.Y },
                {0, 0, 1, -Centre.Z },
                {0, 0, 0, 1 }
            });
            rotationMatrix = DenseMatrix.OfArray(new double[,] {
                {Math.Cos(-Angle), -Math.Sin(-Angle), 0,  0},
                {Math.Sin(-Angle), Math.Cos(-Angle), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            transpositionMatrix = DenseMatrix.OfArray(new double[,] {
                {1, 0,  0, Centre.X },
                {0, 1,  0, Centre.Y },
                {0, 0, 1, Centre.Z  },
                {0, 0, 0, 1 }
            });

            inverseTranspositionMatrix = transpositionMatrix.Inverse();
        }

        Vector<double> ToVector(Point3D point)
        {
            return Vector<double>.Build.Dense(new double[] { point.X, point.Y, point.Z, 1 });
        }

        Point3D ToPoint(Vector<double> vector)
        {
            return new Point3D(vector[0] / vector[3], vector[1] / vector[3], vector[2] / vector[3]);
        }

        Point2D ToProjection(Point3D point)
        {
            return new Point2D(point.Y, point.Z);
        }

    }
}
