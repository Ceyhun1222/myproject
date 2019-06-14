namespace PVT.Engine.Common.Geometry
{
    class GeometryManager
    {
        private static IGeometry _geometry;
        public static IGeometry Geometry
        {
            get
            {
                if (_geometry == null)
                {
                    if (Environment.Current == null)
                        return null;

                    if (Environment.Current.Value == Environments.IAIM)
                        _geometry = new IAIM.Geometry.Geometry();
                    if (Environment.Current.Value == Environments.CDOTMA)
                        _geometry = new CDOTMA.Geometry.Geometry();
                }
                return _geometry;
            }
        }
    }
}
