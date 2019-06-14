namespace PVT.Engine.Common.Database
{
    class DbManager
    {
        private static IDbProvider _dbProvider;
        public static IDbProvider DbProvider
        {
            get
            {
                if (_dbProvider == null)
                {
                    if (Environment.Current == null)
                        return null;

                    if (Environment.Current.Value == Environments.IAIM)
                        _dbProvider = new IAIM.Database.DbProvider();
                    if (Environment.Current.Value == Environments.CDOTMA)
                        _dbProvider = new CDOTMA.Database.DbProvider();
                }
                return _dbProvider;
            }
        }

    }
}
