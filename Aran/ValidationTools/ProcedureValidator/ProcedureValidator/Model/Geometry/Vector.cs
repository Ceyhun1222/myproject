namespace PVT.Model.Geometry
{
    public class Vector
    {
        public Vector()
        {
            X = 1;
            Y = 1;
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector3D vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public int X{ get; set; }
        public int Y { get; set; }
    }

    public class Vector3D
    {
        public Vector3D()
        {
            X = 1;
            Y = 1;
            Z = 0;
        }

        public Vector3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(Vector vector)
        {
            X = vector.X;
            Y = vector.Y;
            Z = 0;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}
