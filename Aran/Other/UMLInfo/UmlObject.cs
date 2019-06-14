using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseMDL
{
    [System.Diagnostics.DebuggerDisplay ("Type: {Type}; Name: {Name}; PropCount: {Propreties.Count}")]
    public class UmlObject : IUmlListItem, IUmlPropertyValue
    {
        public UmlObject ()
        {
            Type = "";
            Name = "";
            Propreties = new List<UmlProperty> ();
        }

        public string Type { get; set; }
        public string Name { get; set; }
        public List<UmlProperty> Propreties { get; private set; }

        public ListItemType ItemType
        {
            get { return ListItemType.UmlObject; }
        }

        public UmlPropertyType PropertyType
        {
            get { return UmlPropertyType.UmlObject; }
        }
    }

    [System.Diagnostics.DebuggerDisplay ("Name: {Name}; Value: {Value}")]
    public class UmlProperty
    {
        public string Name { get; set; }
        public IUmlPropertyValue Value { get; set; }
    }

    public interface IUmlPropertyValue
    {
        UmlPropertyType PropertyType { get; }
    }

    public class UmlPropertyNull : IUmlPropertyValue
    {
        public UmlPropertyType PropertyType
        {
            get { return UmlPropertyType.Null; }
        }
    }

    public interface IUmlListItem
    {
        ListItemType ItemType { get; }
    }

    public class UmlStringItem : IUmlListItem, IUmlPropertyValue
    {
        public UmlStringItem (string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public ListItemType ItemType
        {
            get { return ListItemType.UmlString; }
        }

        public UmlPropertyType PropertyType
        {
            get { return UmlPropertyType.UmlString; }
        }
    }

    public class UmlList : List<IUmlListItem>, IUmlPropertyValue
    {
        public UmlList ()
        {
            Type = "";
        }

        public string Type { get; set; }

        public UmlPropertyType PropertyType
        {
            get { return UmlPropertyType.UmlList; }
        }
    }

    public enum ListItemType { UmlString, UmlObject }
    public enum UmlPropertyType { Null, UmlString, UmlList, UmlObject }
}
