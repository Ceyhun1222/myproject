using System.Collections;
using System.Windows;
using System.Windows.Media;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Temporality.CommonUtil.Extender;
using TOSSM.Control;
using TOSSM.View.Document.Editor;
using TOSSM.ViewModel.Document.Single.Editable;
using Aran.Aim.PropertyPrecision;
using Aran.Temporality.CommonUtil.Util;

namespace TOSSM.ViewModel.Document.Editor
{
    internal class SubViewerProvider
    {
        public static HierarchyControl GetSubViewer(HierarchyControl subViewer, ListPropertyModel selectedProperty, HierarchyControl currentViewer, bool? setNullFilter = null)
        {
            if (selectedProperty == null) return null;

            object editedFeature = selectedProperty.Value;

            bool isLink = false;
          
            var root = HierarchyDocViewerModel.GetRootParent(currentViewer).DataContext as FeatureEditorDocViewModel;
            if (root != null && selectedProperty.PropInfo!=null && selectedProperty.PropInfo.IsFeatureReference)
            {
                if (editedFeature is IAbstractFeatureRef)
                {
                    var guid = (editedFeature as IAbstractFeatureRef).Identifier;
                    var featureType = (FeatureType)(editedFeature as IAbstractFeatureRef).FeatureTypeIndex;
                    var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                    editedFeature = state?.Feature;
                    isLink = true;
                }
                else
                {
                    var guid = (editedFeature is FeatureRef)
                                   ? (editedFeature as FeatureRef).Identifier
                                   : (editedFeature as FeatureRefObject).Feature.Identifier;
                    var featureType = selectedProperty.PropInfo.ReferenceFeature;
                    var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                    editedFeature = state != null ? state.Feature : null;
                    isLink = true;
                }
            }
            else if (selectedProperty.PropInfo != null && selectedProperty.PropInfo.PropType.SubClassType == AimSubClassType.Choice && editedFeature is FeatureRef)
            {
                if (root != null)
                {
                    var guid = (editedFeature as FeatureRef).Identifier;
                    if ((int)selectedProperty.ReferenceFeature > 0)
                    {
                        var featureType = selectedProperty.ReferenceFeature;
                        if (featureType > 0)
                        {
                            var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                            if (state != null)
                            {
                                editedFeature = state.Feature;
                                selectedProperty.ReferenceFeature = featureType;
                                isLink = true;
                            }
                        }
                    }

                    if (!isLink)
                    {
                        foreach (var prop in selectedProperty.PropInfo.PropType.Properties)
                        {
                            var featureType = prop.ReferenceFeature;
                            if (featureType > 0)
                            {
                                var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                if (state != null)
                                {
                                    editedFeature = state.Feature;
                                    selectedProperty.ReferenceFeature = featureType;
                                    isLink = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
           



            if (!(subViewer is FeatureSubEditor))
            {
                subViewer = new FeatureSubEditor
                {
                    DataContext = new FeatureSubEditorDocViewModel(),
                };
            }

            var subModel = (FeatureSubEditorDocViewModel)subViewer.DataContext;
            subModel.PropInfo = selectedProperty.PropInfo;

            subModel.IsReadOnly = selectedProperty.IsReadOnly || selectedProperty.IsParentReadOnly;
            if (subModel.IsReadOnly) subModel.IsNotNullFilter = true;

            subModel.Configuration = selectedProperty.ParentModel != null ? 
                selectedProperty.ParentModel.Configuration : null;
           
            subModel.EditedFeature = editedFeature;
            subModel.ParentViewer = currentViewer;

            subModel.IsNotNullFilter = setNullFilter ?? selectedProperty.ParentModel.IsNotNullFilter;


            //set visibility and color
            subViewer.Visibility = (editedFeature == null) ? Visibility.Collapsed : Visibility.Visible;
            subViewer.SubHeaderPanel.Background = currentViewer.SubHeaderPanel.Background;


            if (isLink)
            {
                subModel.IsReadOnly = true;
                subViewer.SubHeaderPanel.Background =
                    new SolidColorBrush(Colors.White.GetRandom().GetPastelShade());
            }


            return subViewer;
        }

        public static HierarchyControl GetSubViewer(HierarchyControl subViewer, ReflectionEditablePropertyModel selectedProperty, HierarchyControl currentViewer, bool? setNullFilter=null)
        {
            if (selectedProperty == null) return null;

            object editedFeature;
            //check for complex
            var oldValue = selectedProperty.Value;
            if (oldValue!=null && oldValue.GetType().Namespace != "System")
            {
                editedFeature = oldValue;
            }
            else
            {
                editedFeature = null;
            }
            


            if (!(subViewer is FeatureSubEditor))
            {
                subViewer = new FeatureSubEditor { DataContext = new FeatureSubEditorDocViewModel() };
            }

            var subModel = (FeatureSubEditorDocViewModel)subViewer.DataContext;

            subModel.IsReadOnly = selectedProperty.IsReadOnly || selectedProperty.IsParentReadOnly;
            if (subModel.IsReadOnly) subModel.IsNotNullFilter = true;

            subModel.EditedFeature = editedFeature;
            subModel.ParentViewer = currentViewer;


            subModel.IsNotNullFilter = setNullFilter ?? selectedProperty.ParentModel.IsNotNullFilter;


            //set visibility and color
            subViewer.Visibility = (editedFeature == null) ? Visibility.Collapsed : Visibility.Visible;
            subViewer.SubHeaderPanel.Background = currentViewer.SubHeaderPanel.Background;

            return subViewer;
        }

        public static HierarchyControl GetSubViewer(HierarchyControl subViewer,
                                                    AimEditablePropertyModel selectedProperty,
                                                    HierarchyControl currentViewer, bool? setNullFilter = null)
        {
            if (selectedProperty == null) return null;

            if (!(subViewer is FeatureSubEditor))
            {
                subViewer = new FeatureSubEditor { DataContext = new FeatureSubEditorDocViewModel() };
            }
            
               
            var subModel = (FeatureSubEditorDocViewModel)subViewer.DataContext;
            subModel.PropInfo = selectedProperty.PropInfo;

            subModel.IsReadOnly = selectedProperty.IsReadOnly || selectedProperty.IsParentReadOnly;
            if (subModel.IsReadOnly) subModel.IsNotNullFilter = true;


            bool isLink = false;

            object editedFeature = null;
            if (selectedProperty.PropInfo == null && selectedProperty.PropertyName!="Feature Type")
            {
                editedFeature = selectedProperty.Value;
            }
            else
            {
                var oldValue = selectedProperty.Value;
                if ((oldValue is IList) ||
                    (oldValue is AimObject && selectedProperty.ValEnumList == null))
                {
                    //AimMetadataUtility
                    if (selectedProperty.PropInfo.IsFeatureReference  && !selectedProperty.PropInfo.IsList)
                    {
                        var root =
                            HierarchyDocViewerModel.GetRootParent(currentViewer).DataContext as FeatureEditorDocViewModel;
                        if (root != null)
                        {
                            if (oldValue is IAbstractFeatureRef)
                            {
                                var guid = (oldValue as IAbstractFeatureRef).Identifier;
                                var featureType = (FeatureType) (oldValue as IAbstractFeatureRef).FeatureTypeIndex;
                                var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                editedFeature = state != null ? state.Feature : null;
                                isLink = true;
                            }
                            else
                            {
                                var guid = (oldValue is FeatureRef)
                                               ? (oldValue as FeatureRef).Identifier
                                               : (oldValue as FeatureRefObject).Feature.Identifier;
                                var featureType = selectedProperty.PropInfo.ReferenceFeature;
                                var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                editedFeature = state != null ? state.Feature : null;
                                isLink = true;
                            }
                        }
                    }
                    else if (selectedProperty.PropInfo.PropType.SubClassType == AimSubClassType.Choice && oldValue is FeatureRef)
                    {
                        var root =
                           HierarchyDocViewerModel.GetRootParent(currentViewer).DataContext as FeatureEditorDocViewModel;
                        if (root != null)
                        {
                            var guid = (oldValue as FeatureRef).Identifier;
                            if (oldValue is IAbstractFeatureRef)
                            {
                                var featureType = (FeatureType)(oldValue as IAbstractFeatureRef).FeatureTypeIndex;
                                var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                editedFeature = state != null ? state.Feature : null;
                                isLink = true;
                            } else if ((int)selectedProperty.ReferenceFeature>0)
                            {
                                var featureType = selectedProperty.ReferenceFeature;
                                if (featureType > 0)
                                {
                                    var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                    if (state != null)
                                    {
                                        editedFeature = state.Feature;
                                        selectedProperty.ReferenceFeature = featureType;
                                        isLink = true;
                                    }
                                }
                            }
                            if (!isLink)
                            {
                                foreach (var prop in selectedProperty.PropInfo.PropType.Properties)
                                {
                                    var featureType = prop.ReferenceFeature;
                                    if (featureType > 0)
                                    {
                                        var state = CommonDataProvider.GetState(featureType, guid, root.AiracDate);
                                        if (state != null)
                                        {
                                            editedFeature = state.Feature;
                                            selectedProperty.ReferenceFeature = featureType;
                                            isLink = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        editedFeature = oldValue;
                    }
                }
            }

           

          
            if (selectedProperty.PropInfo != null && selectedProperty.PropInfo.IsList)
            {
                var itemInstance = AimObjectFactory.Create(selectedProperty.PropInfo.PropType.Index);
                subModel.ListItemType = itemInstance.GetType();
            }

            subModel.Configuration = null;
            if (selectedProperty.PropInfo != null &&
                selectedProperty.ParentModel.Configuration != null &&
                selectedProperty.ParentModel.Configuration.ObjectConfiguration != null &&
                selectedProperty.ParentModel.Configuration.ObjectConfiguration.Properties != null &&
                selectedProperty.ParentModel.Configuration.ObjectConfiguration.Properties.ContainsKey(
                    selectedProperty.PropInfo.Index))
            {
                subModel.Configuration = selectedProperty.ParentModel.Configuration.ObjectConfiguration.
                    Properties[selectedProperty.PropInfo.Index] as ComplexPropertyConfiguration;
            }

            subModel.EditedFeature = editedFeature;
            subModel.ParentViewer = currentViewer;

            subModel.IsNotNullFilter = setNullFilter ?? selectedProperty.ParentModel.IsNotNullFilter;

            subViewer.Visibility = (editedFeature == null) ? Visibility.Collapsed : Visibility.Visible;
            subViewer.SubHeaderPanel.Background = currentViewer.SubHeaderPanel.Background;

            if (isLink)
            {
                subModel.IsReadOnly = true;
                subViewer.SubHeaderPanel.Background =
                    new SolidColorBrush(Colors.White.GetRandom().GetPastelShade());
            }



            return subViewer;
        }




     }
}
