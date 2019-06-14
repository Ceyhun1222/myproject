using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.DataSet.Classes
{
    public enum InterpretationTypes
    {
        // One date 
        [Description("Complete AIP Data Set")]
        [LongDescription("<html><b>Scope:</b>\n" +
                         "1. The TimeSlices with interpretation <b>BASELINE</b> that are active or that become active on that effective date.\n" +
                         "2. The TimeSlices with interpretation <b>TEMPDELTA</b> that have a <b>validity period of three months or longer</b> and that are active or that become active on that effective date.")]
        Snapshot,

        // Date range
        [Description("Complete AIP Data Set + NOTAM For Predefined Period")]
        [LongDescription("<html><b>Scope:</b>\n" +
                         "1. The TimeSlices with interpretation <b>BASELINE</b> that are active or that become active on that effective date.\n" +
                         "2. The TimeSlices with interpretation <b>TEMPDELTA</b> that are active or that become active on that effective date.")]
        AllStatesInRange,

        [Description("NOTAM/SUP For Predefined Period")]
        [LongDescription("<html><b>Scope:</b>\n" +
                         "The TimeSlices with interpretation <b>TEMPDELTA</b> that are active or that become active on that effective date.")]
        TempDeltaStatesInRange,
    }
    //public enum InterpretationTypes
    //{
    //    // One date 
    //    [Description("Baseline")]
    //    Baseline,
    //    [Description("Snapshot")]
    //    Snapshot,

    //    // Date range
    //    [Description("All states in range")]
    //    AllStatesInRange,
    //    [Description("Baseline states in range")]
    //    BaseLineStatesInRange,
    //    [Description("TempDelta states in range")]
    //    TempDeltaStatesInRange,
    //    //[Description("Baseline with Update")]
    //    //BaselineUpdate,
    //}

    public class LongDescription : Attribute
    {
        public string description { get; set; }
        public LongDescription(string _description)
        {
            description = _description;
        }
    }

    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) 
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
