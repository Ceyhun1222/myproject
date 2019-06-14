using Aran.Aim.Data;
using Aran.Aim.Data.Filters;

namespace Aran.Aim.Data
{
    public static class PgProviderFactory
    {
        public static IDbProvider Create ()
        {
			return new PgDbProvider ( );
        }	
    }
}
