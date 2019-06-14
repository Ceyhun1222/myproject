using Aerodrome.Features;
using Aerodrome.Import;
using Aerodrome.Metadata;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Attributes;
using Framework.Stasy;
using Framework.Stasy.Context;
using Framework.Stasy.Core;
using Framework.Stasy.Helper;
using Framework.Stuff.Extensions;
using HelperDialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfEnvelope.Crud.Framework;
using WpfEnvelope.Crud.UserControls;
using WpfEnvelope.WpfShell.UI.Controls;
using WpfEnvelope.WpfShell.UI.Converter;

namespace WpfEnvelope.Crud.Controls
{
    [TemplatePart(Name = "PART_TaxiwayEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_RunwayEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_ApronEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_SurfLightEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_SurfRoutingEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_ConstructionAreaEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_HelipadEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_HotspotEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_SignageEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_SurvControlPntEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_VerticalStructureEntityPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_WaterEntityPanel", Type = typeof(Panel))]

    [TemplatePart(Name = "PART_SearchTbx", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ClearSearchBtn", Type = typeof(Button))]
    [TemplatePart(Name = "PART_FeatureTypeTabs", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_SearchResult", Type = typeof(StackPanel))]

    [TemplatePart(Name = "PART_BreadcrumpList", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_MasterView", Type = typeof(MasterView))]
    [TemplatePart(Name = "PART_DetailsPanel", Type = typeof(DetailsPanel))]
    [TemplatePart(Name = "PART_ModificationPanel", Type = typeof(ModificationPanel))]
    [StyleTypedProperty(Property = "MasterDefaultTemplate", StyleTargetType = typeof(MasterView))]
    [StyleTypedProperty(Property = "MasterTemplates", StyleTargetType = typeof(MasterView))]
    public class CrudDisplay : TemplatedControl
    {
        static CrudDisplay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CrudDisplay),
                new FrameworkPropertyMetadata(typeof(CrudDisplay)));
 
        }
        
        private Menu _runwayEntityPanel;
        private Menu _apronEntityPanel;
        private Menu _taxiwayEntityPanel;
        private Menu _surfLightEntityPanel;
        private Menu _surfRoutingEntityPanel;
        private Menu _constructionAreaEntityPanel;
        private Menu _helipadEntityPanel;
        private Menu _hotspotEntityPanel;
        private Menu _signageEntityPanel;
        private Menu _survControlPntEntityPanel;
        private Menu _verticalStructureEntityPanel;
        private Menu _waterEntityPanel;
        private Panel _breadcrumpList;
        private MasterView _masterView;
        private DetailsPanel _detailsPanel;
        private ModificationPanel _modificationPanel;

        private List<MenuItem> MenuButtons = new List<MenuItem>();
        private StackPanel _featureTypeTabs;
        private Menu _searchResult;
      
        private TextBox _searchTbx;
        private Button _clearSearchBtn;

        

        private CrudManager _crudManager = new CrudManager();
        public CrudManager CrudManager
        {
            get { return _crudManager; }
        }

        public event UnhandledExceptionEventHandler UnhandledRuntimeException;
        private void OnUnhandledRuntimeException(Exception ex)
        {
            if (UnhandledRuntimeException != null)
                UnhandledRuntimeException(this, new UnhandledExceptionEventArgs(ex, false));
        }

        public event UnhandledExceptionEventHandler PropertySetException;
        private void OnPropertySetException(Exception ex)
        {
            if (PropertySetException != null)
            {
                Exception throwException = ex;
                if (ex is TargetInvocationException)
                    throwException = ex as TargetInvocationException;

                PropertySetException(this, new UnhandledExceptionEventArgs(throwException, false));
            }
        }

        // IMP: Die Changed-Callback abfangen und dann das Master setzen
        #region MasterDefaultTemplate

        public ControlTemplate MasterDefaultTemplate
        {
            get { return (ControlTemplate)GetValue(MasterDefaultTemplateProperty); }
            set { SetValue(MasterDefaultTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MasterDefaultTemplate. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MasterDefaultTemplateProperty =
            DependencyProperty.Register("MasterDefaultTemplate", typeof(ControlTemplate), typeof(CrudDisplay),
            new UIPropertyMetadata(new ControlTemplate(typeof(MasterView))));

        #endregion

        #region MasterTemplates

        public List<TemplateDefinition> MasterTemplateDefinitions
        {
            get { return (List<TemplateDefinition>)GetValue(MasterTemplateDefinitionsProperty); }
            set { SetValue(MasterTemplateDefinitionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MasterTemplates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MasterTemplateDefinitionsProperty =
            DependencyProperty.Register("MasterTemplateDefinitions", typeof(List<TemplateDefinition>), typeof(CrudDisplay),
            new UIPropertyMetadata(new List<TemplateDefinition>()));

        #endregion

        private void Invoke(Action del)
        {
            try
            {
                del();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                //return;
                OnUnhandledRuntimeException(ex);
            }
        }

        private void GenerateMainPage()
        {
            CrudManager.NavigationManager.Navigated += new EventHandler((s, e) =>
                Invoke(() => GenerateSubPage()));

            _modificationPanel.Width = 0.0;
            _modificationPanel.Height = 0.0;

            CreateMainMenu();

            CrudManager.NavigationManager.BreadcrumpListChanged
                += new EventHandler((s, e) => CreateBreadcrumpList());
            CreateBreadcrumpList();

            _searchTbx.TextChanged += searchTbx_TextChanged;

            _clearSearchBtn.Click += clearSearchBtn_Click;

           

        }

        
        private void searchTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!(sender as TextBox).Text.Equals(string.Empty))
            {
                _runwayEntityPanel.Items.Clear();
                _taxiwayEntityPanel.Items.Clear();
                _apronEntityPanel.Items.Clear();
                _surfLightEntityPanel.Items.Clear();
                _surfRoutingEntityPanel.Items.Clear();
                _constructionAreaEntityPanel.Items.Clear();
                _helipadEntityPanel.Items.Clear();
                _hotspotEntityPanel.Items.Clear();
                _signageEntityPanel.Items.Clear();
                _survControlPntEntityPanel.Items.Clear();
                _verticalStructureEntityPanel.Items.Clear();
                _waterEntityPanel.Items.Clear();
               
                _searchResult.Items.Clear();
                _featureTypeTabs.Visibility = Visibility.Collapsed;

                var resultButtons = MenuButtons.Where(b => b.Header.ToString().ToUpper().Contains((sender as TextBox).Text.ToUpper()));

                foreach (var b in resultButtons)
                    _searchResult.Items.Add(b);

                _searchResult.Visibility = Visibility.Visible;
                
            }
            else
            {                
                _searchResult.Items.Clear();
                _featureTypeTabs.Visibility = Visibility.Visible;
                CreateMainMenu();
                _searchResult.Visibility = Visibility.Collapsed;
            }           
        }

        private void clearSearchBtn_Click(object sender, RoutedEventArgs e)
        {            
            _searchTbx.Clear();
        }

       
        //Создает кнопки для каждого feature типа
        private void CreateMainMenu()
        {
            MenuButtons.Clear();
            foreach (var it in CrudManager.RegisteredTypes)
            {

                MenuItem menuItem = new MenuItem()
                {
                    Header = it.Description,
                    //IsCheckable = true,
                    //Padding = new Thickness(5, 4, 3, 4)
                };
                
                menuItem.SetValue(MenuItemExtensions.GroupNameProperty, "MenuItemGroup");

                var mtr = it;               
                menuItem.Click += new RoutedEventHandler((s, e) =>
                {
                    (s as MenuItem).IsChecked = true;
                    Invoke(() => CrudManager.NavigationManager.Navigate(mtr, true));
                });
     
                menuItem.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler((o, t) =>
                  {
                      if (CrudManager.NavigationManager.CurrentModel is null)
                          return;

                      IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
                      if (pMxDoc is null)
                          return;
                      IContentsView contentsView = pMxDoc.CurrentContentsView;

                      IMap pMap = pMxDoc.FocusMap;
                      string currentTypeName = CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type.Name.Substring(3);


                      var featLayer = EsriUtils.getLayerByName(pMap, currentTypeName);
                      if (featLayer is null)
                      {
                          if (currentTypeName.Equals("Runway") || currentTypeName.Equals("RunwayDirection") || currentTypeName.Equals("Taxiway"))
                          {
                              return;
                          }
                          System.Windows.MessageBox.Show("Layer not found", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                          return;
                      }
                          
                      contentsView.SelectedItem = null;
                      contentsView.AddToSelectedItems(featLayer);
                  });

                //кнопки добавить в лист и при поиске найти в Content-ах кнопок.
                MenuButtons.Add(menuItem);

                var categAttr = it.Type.GetCustomAttribute(typeof(CrudFeatureConfigurationAttribute));


                if (categAttr != null)
                {
                    switch (((CrudFeatureConfigurationAttribute)categAttr).FeatureCategory)
                    {
                        case FeatureCategories.TaxiwayFeatureTypes:
                            _taxiwayEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.ApronFeatureTypes:
                            _apronEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.RunwayFeatureTypes:
                            _runwayEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.AerodromeSurfaceLightingFeatureTypes:
                            _surfLightEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.AerodromeSurfaceRoutingNetworkFeatureTypes:
                            _surfRoutingEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.ConstructionAreaFeatureTypes:
                            _constructionAreaEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.HelipadFeatureTypes:
                            _helipadEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.HotspotFeatureTypes:
                            _hotspotEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.SignageFeatureTypes:
                            _signageEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.SurveyControlPointFeatureTypes:
                            _survControlPntEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.VerticalStructureFeatureTypes:
                            _verticalStructureEntityPanel.Items.Add(menuItem);
                            break;
                        case FeatureCategories.WaterFeatureTypes:
                            _waterEntityPanel.Items.Add(menuItem);
                            break;
                        default:
                            _taxiwayEntityPanel.Items.Add(menuItem);
                            break;
                    }
                }
                else
                {
                    _taxiwayEntityPanel.Items.Add(menuItem);
                }
            }
          
        }

        //Navigation на самом верху
        private void CreateBreadcrumpList()
        {
            _breadcrumpList.Children.Clear();

            foreach (var it in CrudManager.NavigationManager.Breadcrump)
            {                
                TextBlock textBlock = new TextBlock();
                textBlock.Text = it.Description;

                _breadcrumpList.Children.Add(textBlock);
            }
        }

        private void GenerateSubPage()
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            _breadcrumpList.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _runwayEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _taxiwayEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _apronEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _verticalStructureEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _surfLightEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _surfRoutingEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _survControlPntEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _constructionAreaEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _helipadEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _hotspotEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _signageEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _waterEntityPanel.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });
            _masterView.SetBinding(MasterView.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });

            _searchResult.SetBinding(ListBox.IsEnabledProperty, new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            });

            // TODO: generate filter

            // Нелегко ли получить доступ к CrudManager.NavigationManager.CurrentModel вместо передачи ViewModel?
            CreateGrid();           
            MasterViewSettings();
            CreateEntityOperations();
            CreateCustomOperations();

        }

        #region Create Methods

        private void CreateGrid()
        {
            var mtr = CrudManager.NavigationManager.CurrentModel.TypeRegistration;
            _masterView.RefreshTemplate(mtr);
            _masterView.EntityList.SetBinding(Selector.SelectedItemProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("CrudManager.NavigationManager.CurrentModel.SelectedItem")

            });
            
        }

        private void MasterViewSettings()
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            _masterView.SetBinding(MasterView.WidthProperty, new Binding()
            {
                Path = new PropertyPath("MasterViewSize"),
                Source = vm
            });
            _masterView.SetBinding(MasterView.HeightProperty, new Binding()
            {
                Path = new PropertyPath("MasterViewSize"),
                Source = vm
            });

            _detailsPanel.SetBinding(DetailsPanel.WidthProperty, new Binding()
            {
                Path = new PropertyPath("DetailsPanelSize"),
                Source = vm
            });
            _detailsPanel.SetBinding(DetailsPanel.HeightProperty, new Binding()
            {
                Path = new PropertyPath("DetailsPanelSize"),
                Source = vm
            });

        }

        //Операции New, Edit, Delete 
        private void CreateEntityOperations()
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            // Remove
            _detailsPanel.Remove = () =>
            {
                Invoke(() =>
                    {

                        MetaTypeRegistration mtr = vm.TypeRegistration;

                        var res = MessageBox.Show("Really delete " + CrudManager.NavigationManager.CurrentModel.TypeRegistration.Description +"(" + ((ListView)_masterView.EntityList).SelectedItems.Count +")"+ " ?", "Delete feature(s)", MessageBoxButton.YesNo,MessageBoxImage.Question);
                        if (res != MessageBoxResult.Yes)
                            return;

                        foreach(var item in ((ListView)_masterView.EntityList).SelectedItems)
                        {
                            mtr.MethodManager.RemoveMethod(mtr.Parent, item);
                        }
                           
                        if (mtr.DataSourceManager.NeedsManualRefresh)
                            _masterView.EntityList.ItemsSource = mtr.DataSourceManager.All;

                        var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                        var deletedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Deleted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                        AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Delete((IEnumerable)deletedList);

                        //Удалить запись из mdb        
                        var idList = deletedList.Select(t => ((AM_AbstractFeature)t).featureID);
                        AerodromeDataCash.ProjectEnvironment.GeoDbProvider.DeleteSelectedRows(CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type, idList);

                        AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
                        AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                    });
            };
            Binding bndRemoveEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("CanRemove")
            };
            _detailsPanel.RemoveButton.SetBinding(Button.IsEnabledProperty, bndRemoveEnabled);

            // New
            _detailsPanel.New = () =>
            {
                Invoke(() =>
                    {
                        ((ListView)_masterView.EntityList).SelectedItems.Clear();
                        vm.EditedItem = vm.TypeRegistration.MethodManager.NewMethod();

                        System.Reflection.PropertyInfo propertyInfo = vm.EditedItem.GetType().GetProperty("RelatedARP");

                        var arp = ((CompositeCollection<AM_AerodromeReferencePoint>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)]).FirstOrDefault();

                        if (propertyInfo != null && arp != null)
                            propertyInfo.SetValue(vm.EditedItem, Convert.ChangeType(arp, propertyInfo.PropertyType), null);

                        //Set idnumber property
                        System.Reflection.PropertyInfo prop = vm.EditedItem.GetType().GetProperty(nameof(AM_AbstractFeature.featureID), BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(vm.EditedItem, Guid.NewGuid().ToString(), null);
                        }

                        CreateModifySection();
                        vm.ChangeMode(UserMode.Add);
                    });
            };
            Binding bndNewEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("CanAdd")
            };
            _detailsPanel.NewButton.SetBinding(Button.IsEnabledProperty, bndNewEnabled);

            // Edit
            _detailsPanel.Edit = () =>
            {
                Invoke(() =>
                {
                    
                    var clone = vm.SelectedItem.ShallowClone(); //Clonable.Clone(vm.SelectedItem);
                    vm.EditedItem = vm.TypeRegistration.MethodManager.EditMethod(
                        clone);
                    CreateModifySection();
                    vm.ChangeMode(UserMode.Edit);

                });
            };

            MultiBinding bndEditEnabled = new MultiBinding();
            bndEditEnabled.Converter = new EditBtnEnableConverter();
            bndEditEnabled.Bindings.Add(new Binding
            {
                Source = vm,
                Path = new PropertyPath("CanEdit")
            });
            bndEditEnabled.Bindings.Add(new Binding
            {
                Source = ((ListView)_masterView.EntityList),
                Path = new PropertyPath("SelectedItems.Count"),
                Converter = new SelectedCountToBoolConverter()
            });
            _detailsPanel.EditButton.SetBinding(Button.IsEnabledProperty, bndEditEnabled);
        }

        //Дополнительные операции для конкретного feature. например: dispatch, close
        private void CreateCustomOperations()
        {

            var vm = CrudManager.NavigationManager.CurrentModel;
            _detailsPanel.OperationsPanel.Children.Clear();

            #region Show on Map

            Uri uri = new Uri("pack://application:,,,/WpfEnvelope;component/Icons/SelectionZoomToSelected32.png");
            BitmapImage bitmap = new BitmapImage(uri);
            
            ImageButtonControl showOnMapBtn = new ImageButtonControl()
            {
                ToolTip = "Show on Map",
                NormalImage = bitmap
            };
          
            showOnMapBtn.Margin = new Thickness(4, 0, 4, 0);
            showOnMapBtn.Click += new RoutedEventHandler((s, e) =>
            {
                Invoke(() =>
                {
                    IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
                    IMap pMap = pMxDoc.FocusMap;

                    if (((ListView)_masterView.EntityList).SelectedItems.Count <= 0)
                        return;

                    var featLayer = EsriUtils.getLayerByName(pMap, ((ListView)_masterView.EntityList).SelectedItems[0].GetType().Name.Substring(3));
                    if (featLayer is null)
                        return;
                    var featClass = ((IFeatureLayer)featLayer).FeatureClass;
                    pMap.ClearSelection();
                    IFeatureSelection featSelect = featLayer as IFeatureSelection;

                    IQueryFilter queryFilter = new QueryFilterClass();
                    List<string> selectedFeatureIdList = new List<string>();

                    int selectedCount = ((ListView)_masterView.EntityList).SelectedItems.Count;
                    int iteration = 0;
                    int step = 250;
                    while (selectedCount / (step + iteration * step) > 0)
                    {
                        selectedFeatureIdList = new List<string>();
                        for (int i = iteration * step; i < step + iteration * step; i++)
                        {

                            AM_AbstractFeature abstractFeat = (AM_AbstractFeature)((ListView)_masterView.EntityList).SelectedItems[i];
                            selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + abstractFeat.featureID + "'");

                        }
                        var resultIdList = string.Join(" OR ", selectedFeatureIdList);
                        queryFilter.WhereClause = resultIdList;

                        featSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);                       
                        iteration += 1;
                    }

                    selectedFeatureIdList = new List<string>();
                    for (int i = iteration * step; i < selectedCount; i++)
                    {
                        AM_AbstractFeature abstractFeat = (AM_AbstractFeature)((ListView)_masterView.EntityList).SelectedItems[i];
                        selectedFeatureIdList.Add(nameof(AM_AbstractFeature.featureID) + " =" + "'" + abstractFeat.featureID + "'");
                    }
                    var result = string.Join(" OR ", selectedFeatureIdList);
                    queryFilter.WhereClause = result;

                    featSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);


                    // Zoom to the selection  
                    ISelectionSet pSelSet = featSelect.SelectionSet;
                    IEnumGeometry pEnumGeom;
                    IEnumGeometryBind pEnumGeomBind;
                    pEnumGeom = new EnumFeatureGeometryClass();
                    pEnumGeomBind = (IEnumGeometryBind)pEnumGeom;
                    pEnumGeomBind.BindGeometrySource(null, pSelSet);

                    IGeometryFactory pGeomFactory = new GeometryEnvironmentClass();
                    IGeometry pGeom = pGeomFactory.CreateGeometryFromEnumerator(pEnumGeom);


                    if (featClass.ShapeType == esriGeometryType.esriGeometryPoint && selectedCount == 1)
                    {

                        ESRI.ArcGIS.Geometry.IEnvelope envelopeCls = pGeom.Envelope;
                        double dim = 1.0;
                        double layerWidth = pGeom.Envelope.Width;
                        double layerHeight = pGeom.Envelope.Height;
                        double layerDim = System.Math.Max(layerWidth, layerHeight) * 0.05;

                        if (layerDim > 0.0)
                            dim = System.Math.Min(1.0, layerDim);

                        double xMin = pGeom.Envelope.XMin;
                        double yMin = pGeom.Envelope.YMin;

                        ESRI.ArcGIS.Geometry.IPoint pointCls = new ESRI.ArcGIS.Geometry.PointClass();
                        pointCls.X = xMin;
                        pointCls.Y = yMin;
                        envelopeCls.Width = dim;
                        envelopeCls.Height = dim;
                        envelopeCls.CenterAt(pointCls);
                        pMxDoc.ActiveView.Extent = envelopeCls;

                    }
                    else
                        pMxDoc.ActiveView.Extent = pGeom.Envelope;

                    //pMxDoc.ActiveView.FocusMap.MapScale = 15000;

                    if (pMxDoc.ActiveView.FocusMap.MapScale < 10000)
                        pMxDoc.ActiveView.FocusMap.MapScale = 10000;

                    pMxDoc.ActiveView.Refresh();
                    //pMxDoc.PageLayout.ZoomToWhole();
                });
            });

            Binding bndShowOnMapBtn = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsItemSelected")
            };
            showOnMapBtn.SetBinding(Button.IsEnabledProperty, bndShowOnMapBtn);
            _detailsPanel.OperationsPanel.Children.Add(showOnMapBtn);

            if (vm.TypeRegistration.Type.Equals(typeof(AM_Runway)) || vm.TypeRegistration.Type.Equals(typeof(AM_Taxiway)) || vm.TypeRegistration.Type.Equals(typeof(AM_RunwayDirection)))
                showOnMapBtn.IsEnabled = false;

            #endregion

            #region Show on Grid

            Uri showOnGridUri = new Uri("pack://application:,,,/WpfEnvelope;component/Icons/showOnGrid.png");
            BitmapImage showOnGridBitmap = new BitmapImage(showOnGridUri);

            ImageButtonControl showOnGridBtn = new ImageButtonControl()
            {
                ToolTip = "Show on Grid",
                NormalImage = showOnGridBitmap
            };
           
            showOnGridBtn.Margin = new Thickness(4, 0, 4, 0);
            showOnGridBtn.Click += new RoutedEventHandler((s, e) =>
            {
                Invoke(() =>
                {
                    ((ListView)_masterView.EntityList).SelectedItems.Clear();

                    IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
                    IMap pMap = pMxDoc.FocusMap;
                    IEnumFeature pEnumFeat = pMap.FeatureSelection as IEnumFeature;

                    IEnumFeatureSetup ftSetup = (IEnumFeatureSetup)pEnumFeat;
                    ftSetup.AllFields = true;
                    pEnumFeat.Reset();

                    IFeature pFeat = pEnumFeat.Next();

                    IFields pFields;                   
                    List<AM_AbstractFeature> selectedFeatures = new List<AM_AbstractFeature>();

                    while (pFeat != null)
                    {
                        Type currentType = CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type;
                        var currentFeatType = AerodromeDataCash.ProjectEnvironment.TableDictionary.Where(t => t.Value.Equals(pFeat.Table)).FirstOrDefault().Key;

                        if(!currentType.Equals(currentFeatType))
                        {
                            pFeat = pEnumFeat.Next();
                            continue;
                        }

                        pFields = pFeat.Fields;
                        int indx = pFields.FindField(nameof(AM_AbstractFeature.featureID));

                        if (indx == -1)
                        {
                            //MessageBox.Show("featureID field not found in selected feature");
                            pFeat = pEnumFeat.Next();
                            continue;
                        }
                        object idValue = pFeat.get_Value(indx);

                        var selectedFeat = ((IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[currentType]).FirstOrDefault(t => ((AM_AbstractFeature)t).featureID.Equals(idValue));
                        if (selectedFeat != null)
                            selectedFeatures.Add((AM_AbstractFeature)selectedFeat);

                        pFeat = pEnumFeat.Next();
                    }

                    foreach (var feat in selectedFeatures)
                        ((ListView)_masterView.EntityList).SelectedItems.Add(feat);

                });
            });

            Binding bndIsInViewModeShowOnGridBtn = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            };
            showOnGridBtn.SetBinding(Button.IsEnabledProperty, bndIsInViewModeShowOnGridBtn);
            _detailsPanel.OperationsPanel.Children.Add(showOnGridBtn);

            #endregion

            #region Apply to selected

            Uri applyToSelectedUri = new Uri("pack://application:,,,/WpfEnvelope;component/Icons/applyToSelected.png");
            BitmapImage applyToSelectedBitmap = new BitmapImage(applyToSelectedUri);

            ImageButtonControl applyToSelectedBtn = new ImageButtonControl()
            {
                ToolTip = "Apply to selected",
                NormalImage = applyToSelectedBitmap
            };
           
            applyToSelectedBtn.Margin = new Thickness(4, 0, 4, 0);
            applyToSelectedBtn.Click += new RoutedEventHandler((s, e) =>
            {
                Invoke(() =>
                {                    
                    vm.EditedItem = vm.TypeRegistration.MethodManager.NewMethod();
                    _modificationPanel.ControlList.Clear();
                    CreateStreamModifySection();
                    vm.ChangeMode(UserMode.Edit);
                    Type currentType = CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type;


                });
            });
            MultiBinding multiBinding = new MultiBinding();            
            Binding bndIsInViewModeApplyBtn = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            };
            Binding bndIsItemSelectedApplyBtn = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsItemSelected")
            };
            Binding bndEditEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("CanEdit")
            };
            Binding bndNewEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("CanAdd")
            };

            multiBinding.Converter = new ApplyToSelectedEnableConverter();
            multiBinding.Bindings.Add(bndIsInViewModeApplyBtn);
            multiBinding.Bindings.Add(bndIsItemSelectedApplyBtn);
            multiBinding.Bindings.Add(bndEditEnabled);
            multiBinding.Bindings.Add(bndNewEnabled);
            applyToSelectedBtn.SetBinding(Button.IsEnabledProperty, multiBinding);
            _detailsPanel.OperationsPanel.Children.Add(applyToSelectedBtn);

            #endregion

            #region Import from layer

            Uri importFromLayerUri = new Uri("pack://application:,,,/WpfEnvelope;component/Icons/import.png");
            BitmapImage importFromLayerBitmap = new BitmapImage(importFromLayerUri);

            ImageButtonControl importFromLayerBtn = new ImageButtonControl()
            {
                ToolTip = "Import from layer",
                NormalImage = importFromLayerBitmap
            };

            importFromLayerBtn.Margin = new Thickness(14, 0, 4, 0);
            importFromLayerBtn.Click += new RoutedEventHandler((s, e) =>
            {
                Invoke(() =>
                {
                    Type currentType = CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type;

                    if (currentType.Name.Substring(3).Equals("Runway") || currentType.Name.Substring(3).Equals("RunwayDirection") || currentType.Name.Substring(3).Equals("Taxiway") || currentType.Name.Substring(3).Equals("AerodromeReferencePoint"))
                    {
                        MessageBox.Show("Operation not available for this type", "Import features", MessageBoxButton.OK,MessageBoxImage.Information);
                        return;
                    }

                    IMxDocument doc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
                    if(doc.CurrentContentsView.Name!="Display" && doc.CurrentContentsView.Name != "Source")
                    {
                        MessageBox.Show("Table of contents must be in Display or Source mode", "Import features", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                                  
                    ILayer selectedLayer = doc.CurrentContentsView.SelectedItem as ILayer;
                   if(selectedLayer is null)
                    {
                        MessageBox.Show("No layer selected or more than one layer selected", "Import features", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    IFeatureClass selectedFeatClass = ((IFeatureLayer)selectedLayer).FeatureClass;

                    IGeoDataset geoDataset = (IGeoDataset)selectedFeatClass;
                    ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = geoDataset.SpatialReference;
                    if (spatialReference?.Name is null)
                    {
                        MessageBox.Show("SpatialReference is null", "Import features", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (spatialReference.Name != "GCS_WGS_1984")
                    {
                        MessageBox.Show("Coordinate system must be: WGS 1984", "Import features", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    var selectedGeomType = selectedFeatClass.ShapeType;
                    

                    ITable currentTable = AerodromeDataCash.ProjectEnvironment.TableDictionary[currentType];
                    IFeatureClass currentFeatClass = (IFeatureClass)currentTable;
                    var currentGeomType = currentFeatClass.ShapeType;
                    if(selectedGeomType!=currentGeomType)
                    {
                        MessageBox.Show("Geometry types do not match", "Import features",MessageBoxButton.OK,MessageBoxImage.Information);
                        return;
                    }
                    
                    IFeatureSelection featSelect = selectedLayer as IFeatureSelection;

                    var selectedFeatures = featSelect.SelectionSet;
                    ICursor selectionFeatCursor;
                    selectedFeatures.Search(null, false, out selectionFeatCursor);

                    int count = 0;
                    var countFeat = selectionFeatCursor.NextRow();
                    while(countFeat!=null)
                    {
                        count++;
                        countFeat = selectionFeatCursor.NextRow();
                    }
                    if(count==0)
                    {
                        MessageBox.Show("No items selected", "Import features", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                   
                    var result = MessageBox.Show("Are you sure you want to import feature(s) from layer " + selectedLayer.Name + " (" + count +")", "Import features", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if(result==MessageBoxResult.Cancel)
                        return; 

                    ImportHelper importHelper = new ImportHelper();
                    selectedFeatures.Search(null, false, out selectionFeatCursor);
                    var selectedFeat = selectionFeatCursor.NextRow();
                    while (selectedFeat != null)
                    {
                        var amObj = importHelper.FeatureToAmFeature(currentType, selectedFeat);
                        importHelper.InsertFeatureToCollectionAndMdb(currentType, amObj);
                        selectedFeat = selectionFeatCursor.NextRow();
                    }

                    MetaTypeRegistration mtr = vm.TypeRegistration;
                    if (mtr.DataSourceManager.NeedsManualRefresh)
                        _masterView.EntityList.ItemsSource = mtr.DataSourceManager.All;

                    MessageScreen messageScreen = new MessageScreen();
                    messageScreen.MessageText = "Succesfully imported";
                    messageScreen.ShowDialog();

                });
            });

            Binding bndIsInViewModeimportFromLayerBtn = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsInViewMode")
            };
            importFromLayerBtn.SetBinding(Button.IsEnabledProperty, bndIsInViewModeimportFromLayerBtn);
            _detailsPanel.OperationsPanel.Children.Add(importFromLayerBtn);

            #endregion

        }

        //Секция для создания и редактирования объекта класса с кнопками OK и Cancel
        private void CreateModifySection()
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            _modificationPanel.SetBinding(DetailsPanel.WidthProperty, new Binding()
            {
                Path = new PropertyPath("ModificationPanelSize"),
                Source = vm
            });
            _modificationPanel.SetBinding(DetailsPanel.HeightProperty, new Binding()
            {
                Path = new PropertyPath("ModificationPanelSize"),
                Source = vm
            });

            _modificationPanel.ClearFields();

            Binding bndModificationEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsUserInputEnabled")
            };
            _modificationPanel.SetBinding(ModificationPanel.IsEnabledProperty,
                bndModificationEnabled);

            ControlGenerationHelper generator = new ControlGenerationHelper(CrudManager, OnPropertySetException);
            foreach (var it in vm.TypeRegistration.CrudProperties)
            {
                string displayName = string.Empty;
                FrameworkElement elementToBeAdded = generator.CreateControl(it, ref displayName);

                elementToBeAdded.Margin = new Thickness(0, 0, 110, 0);

                if (elementToBeAdded != null)
                    _modificationPanel.AddField(displayName, elementToBeAdded, it.Value.PropertyCategory);
                
                // TODO
                //CreatePropertyDetails(vm, grid, ref rowIndex, pi.Name);
            }

            _modificationPanel.Ok = () =>
            {
                Invoke(() => HandleOkCancel(true));
            };
            _modificationPanel.Cancel = () =>
            {
                Invoke(() => HandleOkCancel(false));
            };
        }

        private void CreateStreamModifySection()
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            _modificationPanel.SetBinding(DetailsPanel.WidthProperty, new Binding()
            {
                Path = new PropertyPath("ModificationPanelSize"),
                Source = vm
            });
            _modificationPanel.SetBinding(DetailsPanel.HeightProperty, new Binding()
            {
                Path = new PropertyPath("ModificationPanelSize"),
                Source = vm
            });

            _modificationPanel.ClearFields();

            Binding bndModificationEnabled = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("IsUserInputEnabled")
            };
            _modificationPanel.SetBinding(ModificationPanel.IsEnabledProperty,
                bndModificationEnabled);

            ControlGenerationHelper generator = new ControlGenerationHelper(CrudManager, OnPropertySetException);
            foreach (var it in vm.TypeRegistration.CrudProperties)
            {
                //if (it.Key.GetSetMethod(false) == null ||
                //(!it.Value.IsEnabledInEdit && !it.Value.IsEnabledInNew))
                //    continue;

                if (it.Key.PropertyType.GetInterfaces().FirstOrDefault(i => i.FullName == typeof(IGeometry).FullName) != null)
                    continue;

                StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };
                stack.Margin = new Thickness(0, 0, 80, 0);
                string displayName = string.Empty;
                FrameworkElement elementToBeAdded = generator.CreateControl(it, ref displayName);

                if (it.Key.GetSetMethod(false) == null ||
                (!it.Value.IsEnabledInEdit && !it.Value.IsEnabledInNew))
                {
                    stack.Children.Add(elementToBeAdded);
                    _modificationPanel.AddField(displayName, stack, it.Value.PropertyCategory);
                    continue;
                }

                elementToBeAdded.Name = it.Key.Name;

                //StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };

                var enableCbx = new CheckBox() { VerticalAlignment = VerticalAlignment.Center };
                Binding controlIsEnable = new Binding()
                {
                    Source = enableCbx,
                    Path = new PropertyPath("IsChecked")
                };
                elementToBeAdded.SetBinding(FrameworkElement.IsEnabledProperty, controlIsEnable);
                elementToBeAdded.Margin = new Thickness(0, 0, 5, 0);
                stack.Children.Add(elementToBeAdded);
                stack.Children.Add(enableCbx);
                StreamModifyControl userControl = new StreamModifyControl();
                
                userControl.Content = stack;
                userControl.Name = it.Key.Name;
                userControl.SetBinding(StreamModifyControl.IsSelectedProperty, controlIsEnable);
                if (elementToBeAdded != null)
                    _modificationPanel.AddField(displayName, userControl, it.Value.PropertyCategory);

                // TODO
                //CreatePropertyDetails(vm, grid, ref rowIndex, pi.Name);
            }

            _modificationPanel.Ok = () =>
            {
                //TODO:Здесь можно оптимизировать
                MetaTypeRegistration mtr = vm.TypeRegistration;

                List<System.Reflection.PropertyInfo> changedProperties = new List<System.Reflection.PropertyInfo>();

                foreach (var control in _modificationPanel.ControlList)
                {
                    var streamControl = control as StreamModifyControl;
                    if (streamControl != null && streamControl.IsSelected)
                    {
                        var currentProp = vm.EditedItem.GetType().GetProperty(streamControl.Name);

                        var readonlyProps=vm.EditedItem.GetType().GetProperties().Where(p => p.GetCustomAttribute(typeof(CrudPropertyConfigurationAttribute))!=null && ((CrudPropertyConfigurationAttribute)p.GetCustomAttribute(typeof(CrudPropertyConfigurationAttribute))).SetterPropertyNames!=null && ((CrudPropertyConfigurationAttribute)p.GetCustomAttribute(typeof(CrudPropertyConfigurationAttribute))).SetterPropertyNames.Contains(currentProp.Name));

                        changedProperties.Add(currentProp);
                        if (readonlyProps != null && readonlyProps.Count() > 0)
                            changedProperties.AddRange(readonlyProps);

                        foreach (var feature in ((ListView)_masterView.EntityList).SelectedItems)
                        {
                            mtr.MethodManager.CancelEditMethod(feature);
                            var value = currentProp.GetValue(vm.EditedItem);
                            currentProp.SetValue(feature, value);
                        }
                    }

                }
                //TODO:Чтобы вызвать MarkForUpdate написал такой код так как у correction есть INotifyPropertyChanged
                foreach (var feature in ((ListView)_masterView.EntityList).SelectedItems)
                {
                    var currentProp = vm.EditedItem.GetType().GetProperty(nameof(AM_AbstractFeature.correction));
                    currentProp.SetValue(feature, currentProp.GetValue(feature));
                }
                var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                var updatedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Updated)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Update((IEnumerable)updatedList);

                //Изменить запись в mdb   
                var idList = updatedList.Select(t => ((AM_AbstractFeature)t).featureID);
                AerodromeDataCash.ProjectEnvironment.GeoDbProvider.UpdateSelectedRows(CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type, idList, changedProperties, vm.EditedItem);

                AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
                AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                if (mtr.DataSourceManager.NeedsManualRefresh)
                    _masterView.EntityList.ItemsSource = mtr.DataSourceManager.All;

                ((ListView)_masterView.SelectedEntityList).Items.Refresh();
                
                vm.ChangeMode(UserMode.View);
              
            };
            _modificationPanel.Cancel = () =>
            {
                Invoke(() => HandleOkCancel(false));
            };
        }


        //Обработчик для кнопок Ok и Cancel
        private void HandleOkCancel(bool isOk)
        {
            var vm = CrudManager.NavigationManager.CurrentModel;

            MetaTypeRegistration mtr = vm.TypeRegistration;
            if (vm.IsInAddMode)
            {
                if (isOk)
                {

                    mtr.MethodManager.AddMethod(
                        vm.TypeRegistration.Parent, vm.EditedItem);

                    var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                    var insertedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Inserted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                    //Add to Entity List
                    AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Insert((IEnumerable)insertedList);

                    //Добавить новую запись в mdb.                     
                    foreach (var entity in insertedList)
                    {
                        AerodromeDataCash.ProjectEnvironment.GeoDbProvider.Insert(AerodromeDataCash.ProjectEnvironment.TableDictionary, (AM_AbstractFeature)entity);
                    }

                    AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
                    AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                }
                else
                    mtr.MethodManager.CancelAddMethod(vm.EditedItem);
            }
            else
            {
                if (isOk)
                {                  
                    mtr.MethodManager.CancelEditMethod(vm.EditedItem);
                    GenericExtensions.Copy(vm.EditedItem, vm.SelectedItem);

                    var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                    var updatedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Updated)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                    AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Update((IEnumerable)updatedList);

                    //Set ExtensionData
                    Type currentType = CrudManager.NavigationManager.CurrentModel.TypeRegistration.Type;
                    if(currentType.Name.Equals(typeof(AM_AerodromeReferencePoint).Name) && updatedList.Count()>0)
                    {
                        var arp = updatedList.First() as AM_AerodromeReferencePoint;

                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Organization = arp.Organization;
                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.ADHP = arp.idarpt?.Value;
                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Name = arp.name;
                    }

                    var cnt = updatedList.Count();
                    //Изменить запись в mdb                  
                    foreach (var entity in updatedList)
                    {
                        AerodromeDataCash.ProjectEnvironment.GeoDbProvider.Update(AerodromeDataCash.ProjectEnvironment.TableDictionary, (AM_AbstractFeature)entity);
                    }

                    AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;
                    AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                }

                else
                {                                 
                    //mtr.MethodManager.CancelEditMethod(vm.EditedItem);
                }
            }

            if (mtr.DataSourceManager.NeedsManualRefresh)
                _masterView.EntityList.ItemsSource = mtr.DataSourceManager.All;
           
            ((ListView)_masterView.SelectedEntityList).Items.Refresh();
            
            vm.ChangeMode(UserMode.View);
           
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AssignTemplateControl(ref _taxiwayEntityPanel, new Menu(), "PART_TaxiwayEntityPanel");
            AssignTemplateControl(ref _runwayEntityPanel, new Menu(), "PART_RunwayEntityPanel");
            AssignTemplateControl(ref _apronEntityPanel, new Menu(), "PART_ApronEntityPanel");

            AssignTemplateControl(ref _waterEntityPanel, new Menu(), "PART_WaterEntityPanel");
            AssignTemplateControl(ref _verticalStructureEntityPanel, new Menu(), "PART_VerticalStructureEntityPanel");
            AssignTemplateControl(ref _survControlPntEntityPanel, new Menu(), "PART_SurvControlPntEntityPanel");
            AssignTemplateControl(ref _signageEntityPanel, new Menu(), "PART_SignageEntityPanel");
            AssignTemplateControl(ref _hotspotEntityPanel, new Menu(), "PART_HotspotEntityPanel");
            AssignTemplateControl(ref _helipadEntityPanel, new Menu(), "PART_HelipadEntityPanel");
            AssignTemplateControl(ref _constructionAreaEntityPanel, new Menu(), "PART_ConstructionAreaEntityPanel");
            AssignTemplateControl(ref _surfRoutingEntityPanel, new Menu(), "PART_SurfRoutingEntityPanel");
            AssignTemplateControl(ref _surfLightEntityPanel, new Menu(), "PART_SurfLightEntityPanel");

            AssignTemplateControl(ref _searchTbx, new TextBox(), "PART_SearchTbx");
            AssignTemplateControl(ref _clearSearchBtn, new Button(), "PART_ClearSearchBtn");
            AssignTemplateControl(ref _featureTypeTabs, new StackPanel(), "PART_FeatureTypeTabs");
            AssignTemplateControl(ref _searchResult, new Menu(), "PART_SearchResult");

           

            AssignTemplateControl(ref _breadcrumpList, new WrapPanel(), "PART_BreadcrumpList");
            AssignTemplateControl(ref _detailsPanel, "PART_DetailsPanel");
            AssignTemplateControl(ref _modificationPanel, "PART_ModificationPanel");

            AssignTemplateControl(ref _masterView, new MasterView(), "PART_MasterView");
            _masterView.DefaultTemplate = MasterDefaultTemplate;
            _masterView.EntityTemplateDefinitions = MasterTemplateDefinitions;

            GenerateMainPage();
        }
    }
    
}

       



