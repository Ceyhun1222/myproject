using System;
using System.Windows;
using System.Windows.Controls;
using Aran.Aim;
using TOSSM.ViewModel.Document.Single.Editable;

namespace TOSSM.ViewModel.Document.Editor
{
    class EditorPropertyTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable HeuristicUnreachableCode
            if (item == null) return null;
            // ReSharper restore HeuristicUnreachableCode
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var element = container as ContentPresenter;
            if (element != null)
            {
                //process readonly
                if (item is EditableSinglePropertyModel)
                {
                    var model = item as EditableSinglePropertyModel;
                    if (model.IsReadOnly || model.IsParentReadOnly)
                    {
                        if (model.Value is Boolean)
                        {
                            return element.FindResource("ReadOnlyBoolItemTemplate") as DataTemplate;
                        }
                        if(item is AimXhtmlEditablePropertyModel)
                            return element.FindResource("ReadOnlyRichTextItemTemplate") as DataTemplate;
                        if (item is AimMultiLineEditablePropertyModel)
                            return element.FindResource("ReadOnlyMultiLineDataItemTemplate") as DataTemplate;
                        return element.FindResource("ReadOnlyDataItemTemplate") as DataTemplate;
                    }
                }

                //process reflection
                if (item is ReflectionEditablePropertyModel)
                {
                    var model = item as ReflectionEditablePropertyModel;

                    if (model.PropertyInfo.Name == "Geo")
                    {
                        return element.FindResource("GeoItemTemplate") as DataTemplate;
                    }

                    var propType = model.PropertyInfo.PropertyType;

                    switch (propType.Name)
                    {
                        case "String":
                            return element.FindResource("StringItemTemplate") as DataTemplate;
                        case "Bool":
                            return element.FindResource("BoolItemTemplate") as DataTemplate;
                        case "Double":
                            return element.FindResource("DoubleItemTemplate") as DataTemplate;
                        case "DateTime":
                            return element.FindResource("DateTimeItemTemplate") as DataTemplate;
                        case "Guid":
                            return element.FindResource("GuidItemTemplate") as DataTemplate;
                    }

                    return element.FindResource("ObjectItemTemplate") as DataTemplate;
                }

                //process list
                if (item is ListPropertyModel)
                {
                    return element.FindResource("ListItemTemplate") as DataTemplate;
                }


                //process aim
                if (item is AimEditablePropertyModel)
                {
                    var model = item as AimEditablePropertyModel;
                    var propType = model.PropInfo.PropType;
                    var subType = propType.SubClassType;



                    if (model.PropInfo.IsFeatureReference)
                    {
                        return element.FindResource("LinkItemTemplate") as DataTemplate;
                    }

                    if (propType.IsAbstract)
                    {
                        return element.FindResource("AbstractItemTemplate") as DataTemplate;
                    }

                    switch (subType)
                    {
                        case AimSubClassType.AbstractFeatureRef:
                        case AimSubClassType.Choice:
                            return element.FindResource("ChoiceItemTemplate") as DataTemplate;
                        case AimSubClassType.Enum:
                            return element.FindResource("EnumItemTemplate") as DataTemplate;
                        case AimSubClassType.None:
                            if (propType.AimObjectType == AimObjectType.Field)
                            {
                                var fieldType = (AimFieldType)propType.Index;
                                switch (fieldType)
                                {
                                    case AimFieldType.SysBool:
                                        return element.FindResource("BoolItemTemplate") as DataTemplate;
                                    case AimFieldType.SysDateTime:
                                        return element.FindResource("DateTimeItemTemplate") as DataTemplate;
                                    case AimFieldType.SysDouble:
                                        return element.FindResource("DoubleItemTemplate") as DataTemplate;
                                    case AimFieldType.SysGuid:
                                        return element.FindResource("GuidItemTemplate") as DataTemplate;
                                    case AimFieldType.SysString:
                                        if (item is AimXhtmlEditablePropertyModel)
                                            return element.FindResource("RichTextItemTemplate") as DataTemplate;
                                        else if(item is AimMultiLineEditablePropertyModel)
                                            return element.FindResource("MultiLineStringItemTemplate") as DataTemplate;
                                        else
                                            return element.FindResource("StringItemTemplate") as DataTemplate;

                                    case AimFieldType.SysInt64:
                                        return element.FindResource("Int64ItemTemplate") as DataTemplate;

                                    case AimFieldType.SysInt32:
                                        return element.FindResource("IntItemTemplate") as DataTemplate;

                                    case AimFieldType.SysUInt32:
                                        return element.FindResource("UIntItemTemplate") as DataTemplate;

                                    case AimFieldType.SysEnum:
                                    case AimFieldType.GeoPoint:
                                    case AimFieldType.GeoPolyline:
                                    case AimFieldType.GeoPolygon:
                                        return element.FindResource("StringItemTemplate") as DataTemplate;
                                }
                            }
                            break;
                        case AimSubClassType.ValClass:
                            return element.FindResource("ValItemTemplate") as DataTemplate;
                    }


                    if (propType.AimObjectType == AimObjectType.Object || propType.AimObjectType == AimObjectType.DataType)
                    {
                        return element.FindResource("ObjectItemTemplate") as DataTemplate;
                    }


                }



                return null;
            }
            return null;
        }
    }
}
