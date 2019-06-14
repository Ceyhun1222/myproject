using System.Collections;
using System.Collections.Generic;
using Aran.Aim.Objects;

namespace Aran.Aim.DB
{
    public interface IDataListener
    {
        bool GetObject (AObject aObject, AObjectFilter refInfo);
        AObjectList<T> GetObjects<T> (AObjectFilter refInfo) where T : AObject, new ();
        AObject GetAbstractObject (AbstractType abstractType, AObjectFilter refInfo);
        IList GetAbstractObjects (AbstractType abstractType, AObjectFilter refInfo);
    }
}
