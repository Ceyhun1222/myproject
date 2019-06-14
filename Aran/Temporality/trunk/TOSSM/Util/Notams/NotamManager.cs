using System;
using Aran.Temporality.Common.Entity;
using TOSSM.Util.Notams.Operations;

namespace TOSSM.Util.Notams
{
    public class NotamManager
    {
        public static INotamOperation GetOperation(Notam notam, Notam refNotam = null)
        {
            if(notam.Code23.Equals("RA"))
                return new AirspaceReservationOperation();
            if (notam.Code23.Equals("RR"))
                return new RestrictedAreaOperation();
            if (notam.Code23.Equals("RM"))
                return new MilitaryOperationArea();
            if (notam.Code23.Equals("RD"))
                return new DangerAreaOperation();
            if (notam.Code23.Equals("RT"))
            {
                if(refNotam != null)
                {
                    if (NotamUtils.GetCoordinates(refNotam).Count > 0)
                        return new TemporaryAirspaceOperation();
                    return new ExistingTemporaryAirspaceOperation();
                }

                if(NotamUtils.GetCoordinates(notam).Count > 0 )
                    return new TemporaryAirspaceOperation();
                return new ExistingTemporaryAirspaceOperation();
            }

            if (notam.Code23.Equals("WE"))
            {
                if (refNotam != null)
                {
                    if (NotamUtils.GetCoordinates(refNotam).Count > 0)
                        return new WarningAirspaceOperation();
                    return new ExistingWarningAirspaceOperation();
                }

                if (NotamUtils.GetCoordinates(notam).Count > 0)
                    return new WarningAirspaceOperation();
                return new ExistingWarningAirspaceOperation();
            }

            if (notam.Code23.Equals("WB"))
                return new AerobaticsOperation();
            if (notam.Code23.Equals("WP"))
                return new ParachuteJumpingOperation();

            throw new NotImplementedException($"Q-Code 23 {notam.Code23} is not supported");
        }
    }
}
