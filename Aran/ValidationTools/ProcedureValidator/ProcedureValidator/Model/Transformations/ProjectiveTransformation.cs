using PVT.Model.Geometry;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PVT.Model.Transformations
{
    public class ProjectiveTransformation : CommonTransformation2D
    {

        protected Matrix<double> transfromMatrix;

        public ProjectiveTransformation(double a11, double a12, double a13, 
            double a21, double a22, double a23, 
            double a31, double a32, double a33)
        {
            transfromMatrix = DenseMatrix.OfArray(new double[,] {
                {a11, a12, a13, },
                {a21, a22, a23,},
                {a31, a32, a33 }
            });
        }


        protected override Point2D Transform(Point2D point)
        {
            return ToPoint(transfromMatrix.Multiply(ToVector(point)));
        }


        private Vector<double> ToVector(Point2D point)
        {
            return Vector<double>.Build.Dense(new double[] { point.X, point.Y, 1});
        }

        private Point2D ToPoint(Vector<double> vector)
        {
            return new Point2D(vector[0] / vector[2], vector[1] / vector[2]);
        }
    }
}
