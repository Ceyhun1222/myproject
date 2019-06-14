using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran45ToAixm
{
    public enum AirspaceField
    {
        txtLocalType,
        txtName,
        codeClass,
        codeActivity,
        codeMil,
        codeDistVerUpper,
        valDistVerUpper,
        uomDistVerUpper,
        codeDistVerLower,
        valDistVerLower,
        uomDistVerLower,
        codeDistVerMax,
        valDistVerMax,
        uomDistVerMax,
        codeDistVerMnm,
        valDistVerMnm,
        uomDistVerMnm,
        txtRmk,
		R_mid,
        R_codeType,
        R_codeId
    }

    public enum VerticalStructureField
    {
        txtName,
        txtDescrType,
        codeGroup,
        codeLgt,
        valElev,
		valHgt,
        valElevAccuracy,
        uomDistVer,
        valGeoAccuracy,
        uomGeoAccuracy
    }

    public enum DesignatedPointField
    {
        R_codeId,
        codeType
    }

    public enum RouteSegmentField
    {
        codeType,
        codeRnp,
        codeLvl,
        codeIntl,
        codeTypeFltRule,
        codeCiv,
        valDistVerUpper,
        uomDistVerUpper,
        codeDistVerUpper,
        valDistVerLower,
        uomDistVerLower,
        codeDistVerLower,
        valDistVerMnm,
        uomDistVerMnm,
        valWid,
        uomWid,
        codeRepAtcStart,
        codeRepAtcEnd,
        codeRvsmStart,
        codeRvsmEnd,
        codeTypePath,
        valTrueTrack,
        valMagTrack,
        valReversTrueTrack,
        valReversMagTrack,
        valLen,
        uomDist,
        txtRmk,
        R_RteMid,
        R_codeDir,
        R_SignificantPointStacode_Id,
        R_SignificantPointEndcode_Id

    }

    public enum EnrouteRouteField
    {
        R_mid,
        R_txtDesig,
        R_txtLocDesig,
        txtRmk
    }
}
