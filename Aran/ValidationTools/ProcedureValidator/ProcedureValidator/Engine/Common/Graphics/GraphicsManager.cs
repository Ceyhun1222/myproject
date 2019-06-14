namespace PVT.Engine.Graphics
{
    class GraphicsManager
    {
        private static IGraphics _graphics;
        public static IGraphics Graphics {
            get {
                if(_graphics == null)
                {
                    if (Environment.Current == null)
                        return null;

                    if (Environment.Current.Value == Environments.IAIM)
                        _graphics = new IAIM.Graphics.Graphics();
                    if (Environment.Current.Value == Environments.CDOTMA)
                        _graphics = new CDOTMA.Graphics.Graphics();
                }
                return _graphics;
            }
        }
    }
}
