using System.Collections.Generic;

namespace PVT.Model.Geometry
{
    public class Box
    {
        public Point2D Min { get; }
        public Point2D Max { get; }

        public Box(Point2D min, Point2D max)
        {
            Min = min;
            Max = max;
        }

        public static Box GetBox(List<Box> boxes)
        {
            if (boxes.Count == 0)
                return null;
            if (boxes.Count == 1)
                return boxes[1];
            var box = boxes[0];
            for (var i = 1; i < boxes.Count; i++)
            {
                box = GetBox(box, boxes[i]);
            }

            return box;
        }

        public static Box GetBox(Box box1, Box box2)
        {
            return Point2D.GetBox(new List<Point2D>() {box1.Min, box1.Max , box2.Min , box2.Max});
        }
    }

    public class Box3D
    {
        public Point3D Min { get; private set; }
        public Point3D Max { get; private set; }

        public Box3D(Point3D min, Point3D max)
        {
            Min = min;
            Max = max;
        }
    }
}
