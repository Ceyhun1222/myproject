using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMLInfo
{
    public class PropInfo 
        : CommonInfo 
    {
        public PropInfo ()
        {
        }

        public PropInfo (string name, ObjectInfo propType, bool nullable = false, bool isList = false, string documentation = null)
        {
            Name = name;
            PropType = propType;
            Nullable = nullable;
            IsList = isList;
            Documentation = documentation;
        }

        public bool IsAssociation { get; set; }
        public ObjectInfo PropType { get; set; }
        public PropInfoContainment Containment { get; set; }
        public CardinalityInterval Cardinality { get; set; }
        public CardinalityInterval OtherCardinality { get; set; }
        public bool Nullable { get; set; }
        public bool IsList { get; set; }
        public ValueRestriction Restriction { get; set; }
        public object Tag { get; set; }
    }

    public enum PropInfoContainment { VAL, REF, Unspecified }

    public struct CardinalityInterval
    {
        public short Min
        {
            get { return (short) _point.X; }
            set { _point.X = value; }
        }
        
        public short Max
        {
            get { return (short) _point.Y; }
            set { _point.Y = value; }
        }

        public bool IsEmpty
        {
            get { return _point.IsEmpty; }
        }

        public override string ToString ()
        {
            if (IsEmpty)
                return "";

            if (_text != null)
                return _text;

            return base.ToString ();
        }

        public static readonly CardinalityInterval Empty;

        public static CardinalityInterval Parse (string cardText)
        {
            if (cardText == null)
                return Empty;

            CardinalityInterval interval = new CardinalityInterval ();
            interval.Min = short.Parse (cardText [0].ToString ());

            if (cardText [cardText.Length - 1] == '*')
                interval.Max = short.MaxValue;
            else
                interval.Max = short.Parse (cardText [cardText.Length - 1].ToString ());

            interval._text = cardText;

            return interval;
        }

        private string _text;
        private System.Drawing.Point _point;
    }

    public class ValueRestriction
    {
        public ValueRestriction ()
        {
            RestrictionValues = new List<string []> ();
            Min = double.NaN;
            Max = double.NaN;
        }

        public double Min { get; set; }
        public double Max { get; set; }
        public string Patter { get; set; }
        public List<string []> RestrictionValues { get; private set; }

        public static ValueRestriction Parse (List<string []> restrictionValues)
        {
            ValueRestriction vr = new ValueRestriction ();
            vr.RestrictionValues.AddRange (restrictionValues);

            foreach (string [] sa in restrictionValues)
            {
                if (sa [0].IndexOf ("min") >= 0)
                    vr.Min = double.Parse (sa [1]);
                else if (sa [0].IndexOf ("max") >= 0)
                    vr.Max = double.Parse (sa [1]);
                else if (sa [0].IndexOf ("pattern") >= 0)
                    vr.Patter = sa [1];
            }

            return vr;
        }

        public override string ToString ()
        {
            string s = "";
            foreach (string [] rv in RestrictionValues)
            {
                s += rv [0] + " = " + rv [1] + "\r\n";
            }
            return s;
        }
    }
}
