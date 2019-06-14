using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.CAWProvider
{
    public enum OutputFormatType
    {
        TextXmlAixm,
        TextXmlMap,
        XmlAixm,
        XmlMap
    }

    public enum SupportedFeatureVersionType
    {
        Aixm51,
        Aixm50,
        Aixm45
    }

    public enum InterpretationType
    {
        BASELINE,
        SNAPSHOT,
        TEMPDELTA,
        PERMDELTA
    }

    public enum TemproalTimesliceChoiceType
    {
        EffectiveDate,
        Period,
        SequenceNumber
    }

    public enum SortOrderType
    {
        DESC,
        ASC
    }

    public enum LogMessageCategoryType
    {
        INFO,
        WARNING,
        ERROR,
        FATAL
    }
}
