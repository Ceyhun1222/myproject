using System;

namespace Aran
{
    public interface IAranCloneable
    {
        AranObject Clone ( );
        void Assign ( AranObject source );
    }
}
