using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Temporality.Common.Exceptions;

namespace Aran.Temporality.Internal.Service.Validation
{
    internal class ValidationBase
    {
        protected AimTemporalityService AimTemporalityService;

        public ValidationBase(AimTemporalityService aimTemporalityService)
        {
            AimTemporalityService = aimTemporalityService;
        }

        protected void Error(string message)
        {
            throw new OperationException(message);
        }
    }

}
