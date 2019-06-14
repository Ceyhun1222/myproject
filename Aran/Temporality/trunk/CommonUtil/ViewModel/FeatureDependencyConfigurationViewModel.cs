using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Control;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public enum ConfiguratorMode
    {
        ReadOnly,
        ReadWrite
    }

    public class FeatureDependencyUtil
    {
        public static CompressedFeatureDependencyConfiguration Load(byte[] bytes)
        {
            var m = MemoryUtil.GetMemoryStream(bytes);
            var zip = new DeflateStream(m, CompressionMode.Decompress);
            var unzipped = MemoryUtil.GetMemoryStream();

            while (true)
            {
                var i = zip.ReadByte();
                if (i == -1) break;
                unzipped.WriteByte((byte)i);
            }

            return FormatterUtil.ObjectFromBytes<CompressedFeatureDependencyConfiguration>(unzipped.ToArray());
        }
    }

    public class FeatureDependencyConfigurationViewModel : ViewModelBase, ISelectedItemHolder
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private Visibility _editChangesVisibility=Visibility.Visible;
        public Visibility EditChangesVisibility
        {
            get { return _editChangesVisibility; }
            set
            {
                _editChangesVisibility = value;
                OnPropertyChanged("EditChangesVisibility");
            }
        }

        private Visibility _saveChangesVisibility=Visibility.Collapsed;
        public Visibility SaveChangesVisibility
        {
            get { return _saveChangesVisibility; }
            set
            {
                _saveChangesVisibility = value;
                OnPropertyChanged("SaveChangesVisibility");
            }
        }

        private ConfiguratorMode _mode = ConfiguratorMode.ReadOnly;
        public ConfiguratorMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                OnPropertyChanged("Mode");
                if (Mode==ConfiguratorMode.ReadOnly)
                {
                    SaveChangesVisibility = Visibility.Collapsed;
                    EditChangesVisibility = Visibility.Visible;
                    EditEnabled = false;
                }
                else
                {
                    SaveChangesVisibility = Visibility.Visible;
                    EditChangesVisibility = Visibility.Collapsed;
                    EditEnabled = true;
                }
                if (FirstGeneration.Count>0)
                {
                    ResetMode(FirstGeneration[0]);
                }
            }
        }

        private void ResetMode(FeatureTreeViewItemViewModel model)
        {
            model.EditControlVisibility = Mode==ConfiguratorMode.ReadOnly ? Visibility.Collapsed : Visibility.Visible;

            if (Mode==ConfiguratorMode.ReadOnly)
            {
                var notChecked = model.Children.Cast<FeatureTreeViewItemViewModel>().Where(t => !t.IsChecked).ToList();
                foreach (var notCheckedModel in notChecked)
                {
                    model.Children.Remove(notCheckedModel);
                }
                model.IsExpanded = true;
            }
            
            foreach (FeatureTreeViewItemViewModel subModel in model.Children)
            {
                ResetMode(subModel);
            }
        }

        
        #region Commands

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                    t =>
                    {
                        Mode = ConfiguratorMode.ReadOnly;
                        Deserialize(LastState);
                        LastState = null;
                    },
                    t=>LastState!=null
                    ));
            }
        }

        public byte[] LastState;

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get { return _editCommand??(_editCommand=new RelayCommand(
                t=>
                    {
                        BlockerModel.BlockForAction(() =>
                        {
                            if (FirstGeneration.Count > 0)
                            {
                                LastState = Serialize();
                            }

                            Mode = ConfiguratorMode.ReadWrite;

                            if (FirstGeneration.Count > 0)
                            {
                                Deserialize(LastState);
                            }
                        });
                       

                    },
                    t=>IsEnabled
                )); }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get { return _saveCommand; }
            set
            {
                _saveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }


        private RelayCommand _openFromFileCommand;
        public RelayCommand OpenFromFileCommand
        {
            get
            {
                return _openFromFileCommand ?? (_openFromFileCommand = new RelayCommand(
                t=>
                    {
                        var openFileDialog = new Microsoft.Win32.OpenFileDialog
                        {
                            DefaultExt = ".cfg",
                            Filter = "Config files (.cfg)|*.cfg",
                        };

                        if (openFileDialog.ShowDialog() != true) return;

                        using (var f = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            Mode = ConfiguratorMode.ReadWrite;

                            var bytes = new byte[f.Length];

                            f.Read(bytes, 0, bytes.Length);

                            Deserialize(bytes);
                            f.Close();
                        }
                    },
                    t=>IsEnabled
                    )); }
        }


        private RelayCommand _saveToFileCommand;
        public RelayCommand SaveToFileCommand
        {
            get
            {
                return _saveToFileCommand ?? (_saveToFileCommand = new RelayCommand(
                    t =>
                    {
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                                              {
                                                  FileName = RootFeatureType.ToString(),
                                                  DefaultExt = ".cfg",
                                                  Filter = "Config files (.cfg)|*.cfg",
                                              };

                        if (saveFileDialog.ShowDialog() != true) return;

                        using (var f = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            var bytes = Serialize();
                            f.Write(bytes, 0, bytes.Length);
                            f.Flush();
                            f.Close();
                        }
                    },
                    t => IsEnabled
                  ));
            }
        }

        #endregion

        #region Serialization
        
        private CompressedFeatureDependencyConfiguration[] StrippedGeneration(IEnumerable original)
        {
            return (from FeatureTreeViewItemViewModel model in original
                    where model.IsChecked
                    select new CompressedFeatureDependencyConfiguration
                    {
                        FeatureType = (FeatureType) model.FeatureType,
                        IsDirect = model.IsDirect,
                        MandatoryLinks = model.MandatoryLinks == null || model.MandatoryLinks.Count == 0 ? null : 
                                                model.MandatoryLinks.ToArray(),
                        OptionalLinks = model.OptionalLinks == null || model.OptionalLinks.Count==0 ? null : 
                                                model.OptionalLinks.ToArray(),
                        MandatoryProperties = model.MandatoryProperties == null || model.MandatoryProperties.Count == 0 ? null :
                                                model.MandatoryProperties.ToArray(),
                        OptionalProperties = model.OptionalProperties == null || model.OptionalProperties.Count == 0 ? null :
                                                model.OptionalProperties.ToArray(),
                        Children = model.Children==null || model.Children.Count==0? null : 
                                                StrippedGeneration(model.Children).ToArray()
                    }).ToArray();
        }

        private void Apply(FeatureTreeViewItemViewModel viewModel, CompressedFeatureDependencyConfiguration model)
        {
            viewModel.IsChecked = true;
            viewModel.MandatoryLinks = model.MandatoryLinks==null?null:new HashSet<string>(model.MandatoryLinks);
            viewModel.OptionalLinks = model.OptionalLinks==null?null:new HashSet<string>(model.OptionalLinks);

            viewModel.MandatoryProperties = model.MandatoryProperties == null ? null : new HashSet<string>(model.MandatoryProperties);
            viewModel.OptionalProperties = model.OptionalProperties == null ? null : new HashSet<string>(model.OptionalProperties);

            if (model.Children != null)
            {
                foreach (CompressedFeatureDependencyConfiguration subModel in model.Children)
                {
                    var correspondingSubViewModel = viewModel.Children.Cast<FeatureTreeViewItemViewModel>().
                        Where(t => t.FeatureType == subModel.FeatureType && t.IsDirect == subModel.IsDirect).First();

                    Apply(correspondingSubViewModel, subModel);
                }
            }
           
        }

        private bool _editEnabled;
        public bool EditEnabled
        {
            get { return _editEnabled; }
            set
            {
                _editEnabled = value;
                OnPropertyChanged("EditEnabled");
            }
        }


        public void Deserialize(byte[] bytes)
        {
            var model = FeatureDependencyUtil.Load(bytes);

            RootFeatureType = model.FeatureType;

            Apply(FirstGeneration[0], model);

            ResetMode(FirstGeneration[0]);

            FirstGeneration[0].IsExpanded = true;

            _featureType = FirstGeneration[0].FeatureType;
            OnPropertyChanged("FeatureType");

            Links.Clear();
            Properties.Clear();
        }

        //public void Deserialize(byte[] bytes)
        //{
        //    var decompressor = new DeflateStream(new MemoryStream(bytes),CompressionMode.Decompress);
        //    var memory = new MemoryStream();
        //    decompressor.CopyTo(memory);
        //    var model = FormatterUtil.ObjectFromBytes<CompressedFeatureDependencyConfiguration>(memory.ToArray());

        //    RootFeatureType = model.FeatureType;
        //    Apply(FirstGeneration[0],model);
        //    ResetMode(FirstGeneration[0]);
        //    FirstGeneration[0].IsExpanded = true;
        //    _featureType = FirstGeneration[0].FeatureType;
        //    OnPropertyChanged("FeatureType");

        //    Links.Clear();
        //    Properties.Clear();
        //}

        public byte[] Serialize()
        {
            byte[] zipped;
            FirstGeneration[0].IsChecked = true;

            using (var m = new MemoryStream())
            {
                var compressed = StrippedGeneration(FirstGeneration)[0];
                var bytes = FormatterUtil.ObjectToBytes(compressed);

                var zip = new DeflateStream(m, CompressionMode.Compress);
                zip.Write(bytes, 0, bytes.Length);
                zip.Flush();
                zip.Close();

                zipped = m.ToArray();
            }


            //Deserialize(zipped);

            return zipped;
        }

        #endregion

        #region BlockerModel

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get { return _blockerModel ?? (_blockerModel = new BlockerModel()); }
            set
            {
                _blockerModel = value;
            }
        }
         
        #endregion

        #region FeatureList

        private IList<FeatureType> _featureListFiltered;
        public IList<FeatureType> FeatureListFiltered
        {
            get { return _featureListFiltered ?? (_featureListFiltered = new List<FeatureType>(FeatureList)); }
            set
            {
                _featureListFiltered = value;
                OnPropertyChanged("FeatureListFiltered");
            }
        }

        private string _featureTypeFilter;
        public string FeatureTypeFilter
        {
            get { return _featureTypeFilter; }
            set
            {
                _featureTypeFilter = value;
                OnPropertyChanged("FeatureTypeFilter");


                if (string.IsNullOrEmpty(FeatureTypeFilter))
                {
                    FeatureListFiltered = new List<FeatureType>(FeatureList);
                }
                else
                {
                    var result = new List<FeatureType>();
                    var result2 = new List<FeatureType>();
                    foreach (var featureType in FeatureList)
                    {
                        if (featureType.ToString().ToLower().Contains(FeatureTypeFilter.ToLower()))
                        {
                            result.Add(featureType);
                        }
                        else
                        {
                            result2.Add(featureType);
                        }
                    }

                    //result.AddRange(result2);
                    FeatureListFiltered = result;
                }

            }
        }

        private List<FeatureType> _featureList;
        public List<FeatureType> FeatureList
        {
            get
            {
                if (_featureList == null)
                {
                    _featureList = new List<FeatureType>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _featureList.Add(ft);
                    }
                    _featureList = new List<FeatureType>(_featureList.OrderBy(t => t.ToString()));
                }
                return _featureList;
            }
            set
            {
                _featureList = value;
                OnPropertyChanged("FilteredFeatureList");
            }
        }

        #endregion

        #region Features

        private object _featureType;
        public object FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                if (FeatureType is FeatureType)
                {
                    RootFeatureType = (FeatureType)FeatureType;
                }
                OnPropertyChanged("FeatureType");
            }
        }


        private void ResetUse(FeatureTreeViewItemViewModel model, HashSet<FeatureType> types)
        {
            Debug.Assert(model.FeatureType != null, "model.FeatureType != null");
            var type = (FeatureType)model.FeatureType;
          
            model.Flag = !types.Add(type)?(LightData.IsMandatory&LightData.Missing):LightData.Ok;
           
            foreach (FeatureTreeViewItemViewModel subModel in model.Children)
            {
                if (subModel.IsChecked)
                {
                    ResetUse(subModel, types);
                }
                else 
                {
                    subModel.Flag=types.Contains((FeatureType)subModel.FeatureType)?(LightData.IsMandatory&LightData.Missing):LightData.Ok;
                }
            }
        }

        public void OnNodeCheckedChanged(FeatureTreeViewItemViewModel model)
        {
            var usedTypes = new HashSet<FeatureType>();
            ResetUse(FirstGeneration[0], usedTypes);
          
            model.Children.Clear();

            Debug.Assert(model.FeatureType != null, "model.FeatureType != null");


            if (!model.IsChecked || (model.Flag&LightData.Missing)!=0)
            {
                return;
            }


            foreach (var featureType in RelationUtil.MayRefereFrom((FeatureType)model.FeatureType))
            {
                model.Children.Add(new FeatureTreeViewItemViewModel
                                       {
                                           FeatureType = featureType,
                                           IsDirect = true,
                                           CheckedChanged = OnNodeCheckedChanged,
                                           Parent = model,
                                           SelectedItemHolder = this,
                                           Flag = usedTypes.Contains(featureType)?(LightData.IsMandatory&LightData.Missing):LightData.Ok
                                       });
            }


            foreach (var featureType in RelationUtil.MayRefereTo((FeatureType)model.FeatureType))
            {
                model.Children.Add(new FeatureTreeViewItemViewModel
                                       {
                                           FeatureType = featureType,
                                           IsDirect = false,
                                           CheckedChanged = OnNodeCheckedChanged,
                                           Parent = model,
                                           SelectedItemHolder = this,
                                           Flag = usedTypes.Contains(featureType)?(LightData.IsMandatory&LightData.Missing):LightData.Ok
                                       });
            }
        }

        private FeatureType _rootFeatureType;
        public FeatureType RootFeatureType
        {
            get { return _rootFeatureType; }
            set
            {
                _rootFeatureType = value;
                OnPropertyChanged("RootFeatureType");

                FirstGeneration.Clear();

                FirstGeneration.Add(new FeatureTreeViewItemViewModel
                {
                    FeatureType = RootFeatureType,
                    IsDirect = true,
                    CheckedChanged = OnNodeCheckedChanged,
                    SelectedItemHolder = this
                });

                FirstGeneration[0].IsChecked = true;
            }
        }


        private MtObservableCollection<FeatureTreeViewItemViewModel> _firstGeneration;
        public MtObservableCollection<FeatureTreeViewItemViewModel> FirstGeneration
        {
            get { return _firstGeneration??(_firstGeneration=new MtObservableCollection<FeatureTreeViewItemViewModel>()); }
            set
            {
                _firstGeneration = value;
                OnPropertyChanged("FirstGeneration");
            }
        }

        #endregion

        #region Links

        public PropertyViewModel SelectedLink { get; set; }

        private MtObservableCollection<PropertyViewModel> _links;
        public MtObservableCollection<PropertyViewModel> Links
        {
            get { return _links??(_links=new MtObservableCollection<PropertyViewModel>()); }
        }


        private string _linkHeader="Link path";
        public string LinkHeader
        {
            get { return _linkHeader; }
            set
            {
                _linkHeader = value;
                OnPropertyChanged("LinkHeader");
            }
        }

        #endregion

        #region Properties

        public PropertyViewModel SelectedProperty { get; set; }

        private MtObservableCollection<PropertyViewModel> _properties;
        public MtObservableCollection<PropertyViewModel> Properties
        {
            get { return _properties ?? (_properties = new MtObservableCollection<PropertyViewModel>()); }
        }


        private string _propertyHeader = "Property";
        public string PropertyHeader
        {
            get { return _propertyHeader; }
            set
            {
                _propertyHeader = value;
                OnPropertyChanged("PropertyHeader");
            }
        }

        #endregion

        #region Implementation of ISelectedItemHolder

        private void RecreateLinkList()
        {
            var model = SelectedObject as FeatureTreeViewItemViewModel;
            if (model == null) return;

            Links.Clear();
            if (model.Parent == null)
            {
                LinkHeader = "Link path";
            }
            else
            {
                var parentType = (FeatureType)((FeatureTreeViewItemViewModel)model.Parent).FeatureType;
                var currentType = (FeatureType)model.FeatureType;


                var propertyPaths = model.IsDirect ?
                    RelationUtil.GetConnectionProperty(parentType, currentType) :
                    RelationUtil.GetConnectionProperty(currentType, parentType);

                LinkHeader = model.IsDirect ? "Link path (from " + parentType + " to " + currentType + ")" :
                    "Link path (from " + currentType + " to " + parentType + ")";

                if (string.IsNullOrEmpty(propertyPaths)) return;

                var paths = propertyPaths.Split('\n').ToList();
                if (paths.Count == 0) return;

                foreach (var path in paths)
                {
                    var viewModel = new PropertyViewModel
                    {
                        Name = path,
                        Parent = model,
                        MandatoryChangedAction = OnMandatoryLinkChanged,
                        OptionalChangedAction = OnOptionalLinkChanged,
                    };


                    viewModel.SetIsError(model.MandatoryLinks != null && model.MandatoryLinks.Contains(path));
                    viewModel.SetIsWarning(model.OptionalLinks != null && model.OptionalLinks.Contains(path));


                    if (Mode == ConfiguratorMode.ReadWrite || viewModel.IsError || viewModel.IsWarning)
                    {
                        Links.Add(viewModel);
                    }
                }


            }
        }

        private void RecreatePropertyList()
        {
            var model = SelectedObject as FeatureTreeViewItemViewModel;
            if (model == null) return;

            if (model.Parent == null)
            {
                IsEnabled = true;
            }
            else
            {
                IsEnabled = model.IsChecked;
            }

            PropertyHeader = "Property of " + (FeatureType)model.FeatureType;
            Properties.Clear();
            var list = RelationUtil.GetAllPropertyPath((FeatureType)model.FeatureType);
            foreach (var path in list)
            {
                var viewModel = new PropertyViewModel
                {
                    Name = path,
                    Parent = model,
                    MandatoryChangedAction = OnMandatoryPropertyChanged,
                    OptionalChangedAction = OnOptionalPropertyChanged,
                };

                viewModel.SetIsError(model.MandatoryProperties != null && model.MandatoryProperties.Contains(path));
                viewModel.SetIsWarning(model.OptionalProperties != null && model.OptionalProperties.Contains(path));

                if (Mode == ConfiguratorMode.ReadWrite || viewModel.IsError || viewModel.IsWarning)
                {
                    if (viewModel.Name.IndexOf('\\') == -1 || viewModel.IsError || viewModel.IsWarning)
                    {
                        Properties.Add(viewModel);
                    }
                    else
                    {
                        var i = viewModel.Name.LastIndexOf('\\');
                        var parent = viewModel.Name.Substring(0, i);
                        var parentModel = Properties.Where(t => t.Name == parent).FirstOrDefault();
                        if (parentModel != null && (parentModel.IsError || parentModel.IsWarning))
                        {
                            Properties.Add(viewModel);
                        }
                    }

                }
            }
        }

        private void UpdatePropertyList()
        {
            var model = SelectedObject as FeatureTreeViewItemViewModel;
            if (model == null) return;
            if (Mode != ConfiguratorMode.ReadWrite) return;

            var list = RelationUtil.GetAllPropertyPath((FeatureType) model.FeatureType);
            var propertyNames = Properties.Select(t => t.Name);

            //add checked
            foreach (var path in list.Except(propertyNames))
            {
                var viewModel = new PropertyViewModel
                                    {
                                        Name = path,
                                        Parent = model,
                                        MandatoryChangedAction = OnMandatoryPropertyChanged,
                                        OptionalChangedAction = OnOptionalPropertyChanged,
                                    };

                viewModel.SetIsError(model.MandatoryProperties != null && model.MandatoryProperties.Contains(path));
                viewModel.SetIsWarning(model.OptionalProperties != null && model.OptionalProperties.Contains(path));



                if (viewModel.IsError || viewModel.IsWarning) continue;
                var i = viewModel.Name.LastIndexOf('\\');
                var parent = viewModel.Name.Substring(0, i);
                var parentModel = Properties.Where(t => t.Name == parent).FirstOrDefault();
                if (parentModel == null) continue;
                if (!parentModel.IsError && !parentModel.IsWarning) continue;

                
                Properties.Insert(Properties.IndexOf(parentModel)+1,viewModel);

            }

            //remove unchecked
            var toBeRemoved = new List<PropertyViewModel>();
            foreach (PropertyViewModel viewModel in Properties.Where(t => !t.IsError && !t.IsWarning && t.Name.IndexOf('\\') > -1))
            {
                var i = viewModel.Name.LastIndexOf('\\');
                var parent = viewModel.Name.Substring(0, i);
                var parentModel = Properties.Where(t => t.Name == parent).FirstOrDefault();
                if (parentModel == null || (!parentModel.IsError && !parentModel.IsWarning))
                {
                    toBeRemoved.Add(viewModel);
                }
            }
            foreach (var viewModel in toBeRemoved)
            {
                Properties.Remove(viewModel);
            }
            
        }


        private object _selectedObject;
        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;

                RecreateLinkList();
                RecreatePropertyList();
            }
        }


        private void OnOptionalLinkChanged(PropertyViewModel model)
        {
            if (model.Parent.OptionalLinks == null)
            {
                model.Parent.OptionalLinks = new HashSet<string>();
            }

            if (model.IsWarning)
            {
                model.Parent.OptionalLinks.Add(model.Name);
                model.Parent.IsChecked = true;
                IsEnabled = true;
            }
            else
            {
                model.Parent.OptionalLinks.Remove(model.Name);

                var links = 0;
                links += model.Parent.OptionalLinks == null ? 0 : model.Parent.OptionalLinks.Count;
                links += model.Parent.MandatoryLinks == null ? 0 : model.Parent.MandatoryLinks.Count;

                model.Parent.IsChecked = links > 0;

                IsEnabled = links > 0;
            }
        }

        private void OnMandatoryLinkChanged(PropertyViewModel model)
        {
            if (model.Parent.MandatoryLinks == null)
            {
                model.Parent.MandatoryLinks = new HashSet<string>();
            }
          
            if (model.IsError)
            {
                model.Parent.MandatoryLinks.Add(model.Name);
                model.Parent.IsChecked = true;
                IsEnabled = true;
            }
            else
            {
                model.Parent.MandatoryLinks.Remove(model.Name);
                var links = 0;
                links += model.Parent.OptionalLinks == null ? 0 : model.Parent.OptionalLinks.Count;
                links += model.Parent.MandatoryLinks == null ? 0 : model.Parent.MandatoryLinks.Count;

                model.Parent.IsChecked = links > 0;

                IsEnabled = links > 0;
            }

        }


        private void OnOptionalPropertyChanged(PropertyViewModel model)
        {
            _selectedObject= model.Parent;

            if (model.Parent.OptionalProperties == null)
            {
                model.Parent.OptionalProperties = new HashSet<string>();
            }

            if (model.IsWarning)
            {
                model.Parent.OptionalProperties.Add(model.Name);
                //model.Parent.IsChecked = true;
            }
            else
            {
                model.Parent.OptionalProperties.Remove(model.Name);
            }

            UpdatePropertyList();
        }

        private void OnMandatoryPropertyChanged(PropertyViewModel model)
        {
            _selectedObject = model.Parent;

            if (model.Parent.MandatoryProperties == null)
            {
                model.Parent.MandatoryProperties = new HashSet<string>();
            }

            if (model.IsError)
            {
                model.Parent.MandatoryProperties.Add(model.Name);
                //model.Parent.IsChecked = true;
            }
            else
            {
                model.Parent.MandatoryProperties.Remove(model.Name);
            }

            UpdatePropertyList();
        }
        #endregion

        public void Clear()
        {
            FirstGeneration.Clear();
            Properties.Clear();
            Links.Clear();
        }
    }
}
