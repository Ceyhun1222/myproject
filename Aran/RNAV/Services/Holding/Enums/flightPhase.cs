using System;

namespace Holding
{
    [Flags]
    public enum flightPhase
    {
        Enroute = 1,
        STARUpTo30 = 2,
        STARDownTo30 = 4,
        IFDownTo30 = 8,
        IAFDownTo30 = 16,
        MissAprchDownTo30 = 32,
        MissAprchDownTo15 = 64,
    }
}