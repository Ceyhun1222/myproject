using Aran.Geometries;
using System.Collections.Generic;
using PVT.Drawing.Symbols;

namespace PVT.Engine.Graphics
{
    public interface IGraphics
    {
        void Init();
        int Draw(MultiPolygon multipolygon, int color, int width);
        int Draw(MultiPolygon multipolygon, LineStyles style, int color, int width);
        int Draw(MultiLineString multiline, int color, int width);
        int Draw(MultiLineString multiline, LineStyles style, int color, int width);
        int Draw(Point point, int color, int size);
        int Draw(Point point, PointStyles style , int color, int size);
        int Draw(Point point, string text, int color, int size);
        int Draw(Point point, string text, PointStyles style, int color, int size);
        void Delete(int handler);
        void Delete(List<int> handlers);
        void SetLayoutText(List<string> text);
        void CreateLayout();
        void DeleteLayout();
    }
}
