using PVT.Model.Geometry;
using System;

namespace PVT.Model.Transformations
{
    public interface ITransformation<T> where T : Geometry.Geometry
    {
        Geometry2D Transform(T geom);
    }

    public class TransformationException : Exception
    {
        public TransformationException(string message) : base(message)
        {

        }
    }
}
