using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ANCOR.MapCore
{
    public enum verticalAlignment { Top = 1, Center = 2, Baselene = 3, Bottom = 4 };
    public enum horizontalAlignment { Left = 1, Right = 2, Center = 3, Full = 4 };
    public enum textPosition { Normal = 1, Superscript = 2, Subscript = 3 };
    public enum textCase { Normal = 1, AllCaps = 2, SmallCaps = 3, Lowercase =4 };
    public enum fillStyle
    {
        // Summary:
        //     Solid fill.
        fSSolid = 0,
        //
        // Summary:
        //     Empty fill.
        fSNull = 1,
        //
        // Summary:
        //     Hollow fill (same as fSNull).
        fSHollow = 2,
        //
        // Summary:
        //     Horizontal hatch fill ------.
        fSHorizontal = 3,
        //
        // Summary:
        //     Vertical hatch fill ||||||.
        fSVertical = 4,
        //
        // Summary:
        //     45-degree downward, left-to-right hatch fill \\\.
        fSForwardDiagonal = 5,
        //
        // Summary:
        //     45-degree upward, left-to-right hatch fill //////.
        fSBackwardDiagonal = 6,
        //
        // Summary:
        //     Horizontal and vertical crosshatch ++++++.
        fSCross = 7,
        //
        // Summary:
        //     45-degree crosshatch xxxxxx.
        fSDiagonalCross = 8
    }
    public enum hemiSphere { EasternHemisphere = 1, WesternHemisphere = 2 };
    public enum lineStyle
    {
        // Summary:
        //     The line is solid.
        lsSolid = 0,
        //
        // Summary:
        //     The line is dashed -------.
        lsDash = 1,
        //
        // Summary:
        //     The line is dotted .......
        lsDot = 2,
        //
        // Summary:
        //     The line has alternating dashes and dots _._._._.
        lsDashDot = 3,
        //
        // Summary:
        //     The line has alternating dashes and double dots _.._.._.
        lsDashDotDot = 4,
        //
        // Summary:
        //     The line is invisible.
        lsNull = 5,
        //
        // Summary:
        //     The line will fit into it's bounding rectangle, if any.
        lsInsideFrame = 6
    }
    public enum routeDesignatorPosition { Top = 1, Bottom = 2 };
    public enum routeSegmentDirection { Forward = 1, Backward = 2, Both = 3 };
    public enum lineCalloutStyle
    {
        // Summary:
        //     The line callout leader is a single line originating from the base or top
        //     of the accent bar.
        CSBase = 0,
        //
        // Summary:
        //     The line callout leader is a single line originating from the midpoint of
        //     the accent bar.
        CSMidpoint = 1,
        //
        // Summary:
        //     The line callout leader is a 3-point line originating from the midpoint of
        //     the accent bar.
        CSThreePoint = 2,
        //
        // Summary:
        //     The line callout leader is a 4-point line originating from the midpoint of
        //     the accent bar.
        CSFourPoint = 3,
        //
        // Summary:
        //     The line callout underlines the text.
        CSUnderline = 4,
        //
        // Summary:
        //     A user defined line callout style.
        //CSCustom = 5,
        //
        // Summary:
        //     The line callout leader is curved (clockwise) from the anchor point to the
        //     text.
        CSCircularCW = 6,
        //
        // Summary:
        //     The line callout leader is curved (counter-clockwise) from the anchor point
        //     to the text.
        CSCircularCCW = 7,
        //
        // Summary:
        //     Non-Standart R.A.
        CSFree =8,
    }
    public enum arrowPosition { Start = 0, End = 1 };
    public enum distanceUOM { Kilometers = 1,  NauticalMiles= 2 , Feets = 3, Metres = 4};
    public enum scaleBarPos
    {
        // Summary:
        //     Above bar and labels.
        ScaleBarAbove = 0,
        //
        // Summary:
        //     Before labels.
        ScaleBarBeforeLabels = 1,
        //
        // Summary:
        //     After labels.
        ScaleBarAfterLabels = 2,
        //
        // Summary:
        //     Before bar.
        ScaleBarBeforeBar = 3,
        //
        // Summary:
        //     After bar.
        ScaleBarAfterBar = 4,
        //
        // Summary:
        //     Below bar and labels.
        ScaleBarBelow = 5,
    }
    public enum ShowOnCondition { ifEqual = 0, ifNotEqual = -1 };
    public enum coordtype
    {
        DDMMSSN_1 = 0,
        NDDMMSS_1 = 1,
        DDMMSS_2 = 2,
        NDDMMSS_2 = 3,
        DDMMSS_SSN_2 = 4,
        NDDMMSS_SS_3 = 5,
        NDDMMSS_SS_2 = 6,
        DDMMSS_SS_2 = 7,
        DDMMSS_SN_2 = 8,
        NDDMMSS_S_3 = 9,
        NDDMMSS_S_2 = 10,
        DDMMSS_S_2 = 11,
        OTHER = 12,
    }
    public enum enuChartTypes
    {
        Aerodrome_Heliport =0,
        Aerodrome_Ground_Movement =1,
        Aircraft_Parking_Docking =2,
        Standard_Instrument_Approach =3,
        Standard_Instrument_Arrival =4,
        Standard_Instrument_Departure =5,
        Standard_Instrument_Departure_Portrait=6,
        World_Aeronautical=7,
        Aeronautical=8,
        AirNavigation_small_scale=9,
        Enroute=10,
        Area=11,
        Aerodrome_Obstacle_type_A=12,
        Aerodrome_Obstacle_type_B=13,
        Visual_Approach =14,
        Precision_Approach =15,
        Radar_Minimumum_Altitude=16
    }
    public enum ChartCategory
    {
        TERMINAL,
        ROUTE,
        AERODROME,
        NAVIGATION,
        OBSTACLE,
        AREA
    }
    public enum GeometryType
    {
        GeometryNull = 0,
        GeometryPoint = 1,
        //GeometryMultipoint = 2,
        GeometryPolyline = 3,
        GeometryPolygon = 4,
        GeometryEnvelope = 5,
        //GeometryPath = 6,
        //GeometryAny = 7,
        //GeometryMultiPatch = 9,
        //GeometryRing = 11,
        GeometryLine = 13,
        //GeometryCircularArc = 14,
        //GeometryBezier3Curve = 15,
        //GeometryEllipticArc = 16,
        //GeometryBag = 17,
        //GeometryTriangleStrip = 18,
        //GeometryTriangleFan = 19,
        //GeometryRay = 20,
        //GeometrySphere = 21,
        //GeometryTriangles = 22,
    }
    public enum rowPosition { above =1, under = 2, after=3, before = 4}

    public enum sigmaCalloutLeaderLineSnap 
    {
        nearest = 0, 
        leftTop = 1, 
        centerTop =2, 
        rightTop =3, 
        leftCenter =4, 
        centerCenter =5, 
        rightCenter =6, 
        leftBottom =7, 
        centerBottom =8, 
        rightBottom =9
    }

    public enum sigmaCalloutAccentbarPosition
    {
        left =0 ,
        right = 1
    }

    public enum SigmaIlsStyle
    {
        profileStyle1 = 1,
        profileStyle2 = 2,
        profileStyle3 = 3,
        profileStyle4 = 4,
    }

    public enum SigmaIlsAnchorPoint
    {
        LOC =1,
        GP = 2
    }

    public enum Scroll
    {
        FillColor = 0,
        FrameColor = 1
    }


}
