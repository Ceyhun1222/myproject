using PVT.Model;

namespace PVT.Graphics
{
    public interface IFeatureDrawer<in T> where T : Feature
    {
        bool IsEnabled();
        void Draw(T obj);
        void Clean(T obj);
        void Clean();
    }
}
