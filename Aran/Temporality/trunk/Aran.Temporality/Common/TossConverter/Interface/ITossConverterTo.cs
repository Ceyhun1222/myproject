using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.TossConverter.Interface
{
    public interface ITossConverterTo : ITossWritableRepository
    {
        void AddEventWithGeoProblem(int workPackage, AbstractEvent<AimFeature> abstractEvent, MessageCauseType type);
    }
}
