using System.Collections.Generic;
using Aran.Geometries;
using PVT.Drawing.Symbols;

namespace PVT.Engine.Common.Graphics
{
    abstract class CommonGraphics: Engine.Graphics.IGraphics
    {
        public bool IsInit { get; protected set; }        
        public abstract void Init();
        public abstract int Draw(MultiPolygon multipolygon, int color, int width);
        public abstract int Draw(MultiPolygon multipolygon, LineStyles style, int color, int width);
        public abstract int Draw(MultiLineString multiline, int color, int width);
        public abstract int Draw(MultiLineString multiline, LineStyles style, int color, int width);
        public abstract int Draw(Point point, int color, int size);
        public abstract int Draw(Point point, PointStyles style, int color, int size);
        public abstract int Draw(Point point, string text, int color, int size);
        public abstract int Draw(Point point, string text, PointStyles style, int color, int size);

        public abstract void Delete(int handler);
        public  void Delete(List<int> handlers)
        {
            for (var i = 0; i < handlers.Count; i++)
            {
                Delete(handlers[i]);
            }
        }
        public abstract void CreateLayout();
        public abstract void DeleteLayout();
        public abstract void SetLayoutText(List<string> text);
    }
}
