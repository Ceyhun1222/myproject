using MathNet.Numerics.LinearAlgebra;

namespace PVT.Model.Transformations
{
    public class ScaleTransformation : ProjectiveTransformation
    {
        private double _scaleX;
        public double ScaleX
        {
            get
            {
                return _scaleX;
            }
            set
            {
                _scaleX = value;
                transfromMatrix.SetDiagonal(Vector<double>.Build.Dense(new double[] { _scaleX, _scaleY, 1 }));
            }
        }
        private double _scaleY;
        public double ScaleY
        {
            get
            {
                return _scaleY;
            }
            set
            {
                _scaleY = value;
                transfromMatrix.SetDiagonal(Vector<double>.Build.Dense(new double[] { _scaleX, _scaleY, 1 }));
            }
        }

        public ScaleTransformation(double scaleX, double scaleY) : base(scaleX, 0, 0, 0, scaleY, 0, 0, 0, 1)
        {
            _scaleX = scaleX;
            _scaleY = scaleY;
        }
    }
}
