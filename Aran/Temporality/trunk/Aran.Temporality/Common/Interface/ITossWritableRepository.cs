using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.TossConverter;

namespace Aran.Temporality.Common.Interface
{
    public interface ITossWritableRepository
    {
        AimFeature AddEvent(int workPackage, AbstractEvent<AimFeature> abstractEvent, bool check = true);
        bool IsExist();
        void ClearRepository();
    }
}
