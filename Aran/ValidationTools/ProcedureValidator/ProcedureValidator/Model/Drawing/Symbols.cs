namespace PVT.Drawing.Symbols
{
    public enum PointStyles
    {
        smsCircle,
        smsSquare,
        smsCross,
        smsX,
        smsDiamond
    };

    public enum LineStyles
    {
        slsSolid,
        slsDash,
        slsDot,
        slsDashDot,
        slsDashDotDot,
        slsNull,
        slsInsideFrame
    }

    public enum FillStyles
    {
        sfsSolid,
        sfsNull,
        sfsHollow = sfsNull,
        sfsHorizontal,
        sfsVertical,
        sfsForwardDiagonal,
        sfsBackwardDiagonal,
        sfsCross,
        sfsDiagonalCross
    }
}
