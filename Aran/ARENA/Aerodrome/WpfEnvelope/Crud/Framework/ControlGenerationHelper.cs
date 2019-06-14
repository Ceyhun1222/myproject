using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Controls;

using System.Collections;
using Framework.Stuff.Extensions;
using System.Globalization;
using WpfEnvelope.WpfShell.UI.Converter;
using WpfEnvelope.Crud.UserControls;
using System.Linq.Expressions;
using System.Dynamic;
using Framework.Attributes;
using Aerodrome.Features;
using System.Collections.ObjectModel;
using Aerodrome.DataType;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;

namespace WpfEnvelope.Crud.Framework
{
   
    class ControlGenerationHelper
	{
		public ControlGenerationHelper(
			CrudManager crudManager,
			Action<Exception> propertySetExceptionHandler)
		{
			_crudManager = crudManager;
			_propertySetExceptionHandler = propertySetExceptionHandler;
		}

		private readonly CrudManager _crudManager;
		private readonly Action<Exception> _propertySetExceptionHandler;

        public FrameworkElement CreateControl(
            KeyValuePair<PropertyInfo, ICrudPropertyMetadata> crudProperty,
            ref string displayName)
        {
            if (!crudProperty.Value.DisplayInModification)
                return null;

            displayName = crudProperty.Value.GetDisplayName(crudProperty.Key);

            ViewModel vm = _crudManager.NavigationManager.CurrentModel;
            var binding = new Binding()
            {
                Source = vm,
                Path = new PropertyPath("EditedItem." + crudProperty.Key.Name),
                Mode = BindingMode.OneWay, 
            };

            FrameworkElement element;

            // Свойства только для чтения
            if (crudProperty.Key.GetSetMethod(false) == null ||
                (!crudProperty.Value.IsEnabledInEdit && !crudProperty.Value.IsEnabledInNew))
            {
                element = GetReadOnlyElement(binding);
            }
            else if (crudProperty.Key.PropertyType == typeof(bool))
            {
                element = GetBooleanElement(crudProperty.Key, binding, vm);
            }
            else if (crudProperty.Key.PropertyType.IsType(typeof(DateTime), false))
            {
                object source = null;
                if (vm.EditedItem != null)
                    source = crudProperty.Key.GetValue(vm.EditedItem);
                element = GetDateTimeElement(crudProperty.Key, binding, vm, source);
            }
            else if (crudProperty.Key.PropertyType.IsEnum)
            {
                var enumValues = Enum.GetValues(crudProperty.Key.PropertyType);
                element = GetCollectionElement(crudProperty.Key, binding, vm, enumValues);
            }

            else if (crudProperty.Key.PropertyType.IsCollection())
            {

                IEnumerable list = _crudManager.TryGetDataSource(crudProperty.Key.PropertyType.GetGenericArguments().First());
                if (list == null)
                    throw new CrudConfigurationException(
                        "The property " + crudProperty.Key.Name + " of type " + crudProperty.Key.ReflectedType.FullName +
                        " shall be displayed in the modification section, but there is no datasource defined for it.");
               
                List<AM_AbstractFeature> featList =  list.OfType<AM_AbstractFeature>().Where(t => t.Descriptor != null && !t.Descriptor.Equals(string.Empty)).OrderBy(r => r.Descriptor).ToList();               

                element = GetMultiSelectElement(crudProperty.Key, binding, vm, featList);

            }
            else if (crudProperty.Key.PropertyType.Name.Equals(typeof(AM_AbstractFeature).Name))
            {
                List<Type> types = null;
                var allovableTypesAttr = crudProperty.Key.GetCustomAttribute(typeof(AllowableTypesAttribute));
                if (allovableTypesAttr != null)
                {
                    types = ((AllowableTypesAttribute)allovableTypesAttr).AllovableTypes.ToList();
                    
                }
                  

                element = GetRelatedFeatureElement(crudProperty.Key, binding, vm, types);


            }
            else if (ObjectExtensions.GetInheritanceHierarchy(crudProperty.Key.PropertyType).Contains(typeof(AM_AbstractFeature)))// Обычная ссылка: нужно выбрать объект из списка ...
            {
                // get list
                IEnumerable list = _crudManager.TryGetDataSource(crudProperty.Key.PropertyType);
                if (list == null)
                    throw new CrudConfigurationException(
                        "The property " + crudProperty.Key.Name + " of type " + crudProperty.Key.ReflectedType.FullName +
                        " shall be displayed in the modification section, but there is no datasource defined for it.");

                element = GetCollectionElement(crudProperty.Key, binding, vm, list.OfType<AM_AbstractFeature>().Where(t=> t.Descriptor!=null && !t.Descriptor.Equals(string.Empty)).OrderBy(r=>r.Descriptor));
            }
           
            else if (crudProperty.Key.PropertyType.Name == typeof(DataType<Enum>).Name)
            {

                object source = null;
                if (vm.EditedItem != null)
                    source = crudProperty.Key.GetValue(vm.EditedItem);

                element = GetDataTypeElement(crudProperty.Key, binding, vm, source);

            }
            else if(crudProperty.Key.PropertyType.GetInterfaces().Any(t=>t.Name==typeof(IGeometry).Name))
            {
                object source = null;
                if (vm.EditedItem != null)
                    source = crudProperty.Key.GetValue(vm.EditedItem);
                element = GetGeometryElement(crudProperty.Key, binding, vm, source);
            }
            else if (crudProperty.Key.PropertyType.Name == typeof(AM_Nullable<Type>).Name)
            {
                if (crudProperty.Key.PropertyType.GetGenericArguments()[0].IsEnum)
                {
                    object selected = null;
                    if (vm.EditedItem != null)
                        selected = crudProperty.Key.GetValue(vm.EditedItem);

                    //element = GetNullableEnumElement(crudProperty.Key, binding, vm, selected);

                    element = GetNullableEnumControl(crudProperty.Key, binding, vm, selected, crudProperty.Key.PropertyType);

                }
                else if (crudProperty.Key.PropertyType.GetGenericArguments()[0].IsType(typeof(DateTime), false))
                {
                    object source = null;
                    if (vm.EditedItem != null)
                        source = crudProperty.Key.GetValue(vm.EditedItem);
                    //element = GetNullableDateTimeElement(crudProperty.Key, binding, vm);

                    element = GetNullableDateTimeControl(crudProperty.Key, binding, vm, source, crudProperty.Key.PropertyType);
                }
                else if (crudProperty.Key.PropertyType.GetGenericArguments()[0].IsValueType|| crudProperty.Key.PropertyType.GetGenericArguments()[0].IsType(typeof(string), false))
                {
                    //element = GetNullableGenericValueElement(crudProperty.Key, binding, vm);

                    object selected = null;
                    if (vm.EditedItem != null)
                        selected = crudProperty.Key.GetValue(vm.EditedItem);

                    element = GetNullableSimpleControl(crudProperty.Key, binding, vm, selected, crudProperty.Key.PropertyType);
                }
                else
                    element = null;

            }
            else if (!crudProperty.Key.PropertyType.IsReferenceType())
            {
                element = GetGenericValueElement(crudProperty.Key, binding, vm);
            }

            else
                element = null;

            SetElementEnabled(crudProperty, vm, element);

            return element;
        }

       

        private FrameworkElement GetReadOnlyElement(Binding binding)
		{
			TextBlock element = new TextBlock();
            element.MaxWidth = 110;
            element.SetBinding(TextBlock.TextProperty, binding);

			return element;
		}

		private FrameworkElement GetBooleanElement(PropertyInfo pi, Binding binding, ViewModel vm)
		{
			CheckBox element = new CheckBox();
            element.Height = 30;
            element.VerticalContentAlignment = VerticalAlignment.Center;
            element.SetBinding(CheckBox.IsCheckedProperty, binding);
			element.Click += new RoutedEventHandler((s, e) =>
			{
				try
				{
					pi.SetValue(vm.EditedItem, element.IsChecked, null);
				}
				catch (Exception ex)
				{
					element.IsChecked = (bool)pi.GetValue(vm.EditedItem, null);
					_propertySetExceptionHandler(ex);
				}
				finally
				{
					element.SetBinding(CheckBox.IsCheckedProperty, binding);
				}
			});

			return element;
		}

		private FrameworkElement GetDateTimeElement(PropertyInfo pi, Binding binding, ViewModel vm,object source)
		{
            // IMP: DatePicker создает проблемы с обновлением и всем рабочим процессом, привязка не работает должным образом и т. Д.
            // TODO: Смотрите, нужно ли устанавливать только дату или время.
            DatePicker element = new DatePicker();
            element.Width = 120;
            element.Height = 30;
            element.VerticalContentAlignment = VerticalAlignment.Center;
            element.SetBinding(DatePicker.SelectedDateProperty, binding);
            if(source is null || ((DateTime)source).Equals(default(DateTime)))
            element.SelectedDate = DateTime.Now;
            else
                element.SelectedDate = (DateTime)source;
            element.SelectedDateChanged += new EventHandler<System.Windows.Controls.SelectionChangedEventArgs>((s, e) =>
            {
                try
                {
                    pi.SetValue(
                        vm.EditedItem,
                        Convert.ChangeType(element.SelectedDate, pi.PropertyType),
                        null);
                }
                catch (Exception ex)
                {
                    element.SelectedDate = (DateTime)pi.GetValue(vm.EditedItem, null);
                    _propertySetExceptionHandler(ex);
                }
                finally
                {
                    element.SetBinding(DatePicker.SelectedDateProperty, binding);
                }
            });

            //         // WORKAROUND:
            //         TextBox element = new TextBox()
            //{
            //	Width = 120 // TODO: auslagern in eine "ValuePropertyWidth" vom KVGrid
            //};
            //var converter = new DateTimeCultureConverter();
            //converter.UseShortDateString = true;
            //binding.Converter = converter;
            //element.SetBinding(TextBox.TextProperty, binding);
            //element.LostFocus += new RoutedEventHandler((s, e) =>
            //{
            //	try
            //	{
            //		var dateTime = DateTime.Parse(element.Text);
            //		pi.SetValue(vm.EditedItem, dateTime, null);
            //	}
            //	catch (Exception ex)
            //	{
            //		var value = pi.GetValue(vm.EditedItem, null);
            //		if (value == null)
            //			element.Text = string.Empty;
            //		else
            //			element.Text = value.ToString();

            //		_propertySetExceptionHandler(ex);
            //	}
            //	finally
            //	{
            //		// привязка должна быть установлена снова, как кажется
            //      // вручную задаем свойство text, что привязка данных больше не работает.
            //		element.SetBinding(TextBox.TextProperty, binding);
            //	}
            //});

            return element;
		}

        private FrameworkElement GetGenericValueElement(PropertyInfo pi, Binding binding, ViewModel vm)
		{
			TextBox element = new TextBox()
			{
				Width = 120,
                Height=30
			};
            element.VerticalContentAlignment = VerticalAlignment.Center;
            element.SetBinding(TextBox.TextProperty, binding);
			element.LostFocus += new RoutedEventHandler((s, e) =>
			{
				try
				{
					pi.SetValue(
						vm.EditedItem,
						Convert.ChangeType(element.Text, pi.PropertyType),
						null);
				}
				catch (Exception ex)
				{
					var value = pi.GetValue(vm.EditedItem, null);
					if (value == null)
						element.Text = string.Empty;
					else
						element.Text = value.ToString();

					_propertySetExceptionHandler(ex);
				}
				finally
				{
                    // привязка должна быть установлена снова, как кажется
                    // вручную задаем свойство text, что привязка данных больше не работает.
                    element.SetBinding(TextBox.TextProperty, binding);
				}
			});

			return element;
		}

        private FrameworkElement GetCollectionElement(PropertyInfo pi, Binding binding, ViewModel vm, IEnumerable list)
		{
			ComboBox element = new ComboBox()
			{
				ItemsSource = list
			};
            element.Width=120;
            element.Height = 30;
            element.VerticalContentAlignment = VerticalAlignment.Center;
            element.SetBinding(ComboBox.SelectedItemProperty, binding);
			element.SelectionChanged += new SelectionChangedEventHandler((s, e) =>
			{
				try
				{                   
                    pi.SetValue(vm.EditedItem, element.SelectedItem, null);
				}
				catch (Exception ex)
				{
					element.SelectedItem = pi.GetValue(vm.EditedItem, null);
					_propertySetExceptionHandler(ex);
				}
				finally
				{
					element.SetBinding(ComboBox.SelectedItemProperty, binding);
				}

            });

			return element;
		}

        private FrameworkElement GetRelatedFeatureElement(PropertyInfo pi, Binding binding, ViewModel vm, List<Type> list)
        {
            RelatedFeatureControl element = new RelatedFeatureControl()
            {
                FeatureTypes = list,
                
                //set selected value
                
            };
            element.Width = 120;
            element.Height = 64;
            element.SetBinding(RelatedFeatureControl.SelectedFeatureProperty, binding);
            if(element.SelectedFeature!=null)
            {
                element.typesCombobox.SelectedItem = element.SelectedFeature.GetType();
                var featuresByType = AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[element.SelectedFeature.GetType()];
                element.featuresCombobox.ItemsSource = featuresByType;
            }            

            element.typesCombobox.SelectionChanged += new SelectionChangedEventHandler((s, e) =>
              {                  
                  var selectedType = element.typesCombobox.SelectedItem as Type;
                  var featuresByType = AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[selectedType];
                  element.featuresCombobox.ItemsSource = (featuresByType as IEnumerable).OfType<AM_AbstractFeature>().Where(t=>t.Descriptor!=null && !t.Descriptor.Equals(string.Empty)).OrderBy(r=>r.Descriptor);
              });
            
            
            //element.typesCombobox.ItemsSource = list;

            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    if (element.SelectedFeature == null)
                    {
                        pi.SetValue(vm.EditedItem, null, null);
                        return;
                    }

                    pi.SetValue(vm.EditedItem, element.SelectedFeature, null);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Aerodrome",MessageBoxButton.OK,MessageBoxImage.Error);
                }

            });
            return element;
        }

        private FrameworkElement GetNullableEnumControl(PropertyInfo pi, Binding binding, ViewModel vm, object source, Type propType)
        {
            NullableEnumControl element = new NullableEnumControl();
            element.Width = 120;
            element.Height = 30;
            element.PropertyType = propType;
           
            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    var instanceProps = pi.PropertyType.GetProperties();
                    if (element.Value == null)
                    {
                        //Здесь присвоить NilReason
                        var instance = Activator.CreateInstance(pi.PropertyType);
                        instanceProps[0].SetValue(instance, element.nilReasonCbx.SelectedItem);                      
                        pi.SetValue(vm.EditedItem, instance, null);
                        return;
                    }
                   
                    var inst = Activator.CreateInstance(pi.PropertyType);                  
                    instanceProps[1].SetValue(inst, element.Value);
                    instanceProps[0].SetValue(inst, null);
                    pi.SetValue(vm.EditedItem, inst, null);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
            //if(source!=null)
                element.Source = source;
            
            return element;
        }

        private FrameworkElement GetNullableDateTimeControl(PropertyInfo pi, Binding binding, ViewModel vm, object source, Type propType)
        {

            NullableDateTimeControl element = new NullableDateTimeControl();
            element.Width = 120;
            element.Height = 30;
            element.PropertyType = propType;

            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    var instanceProps = pi.PropertyType.GetProperties();
                    if (element.Value == null)
                    {
                        //Здесь присвоить NilReason
                        var instance = Activator.CreateInstance(pi.PropertyType);
                        
                        instanceProps[0].SetValue(instance, element.nilReasonCbx.SelectedItem);
                        pi.SetValue(vm.EditedItem, instance, null);
                        return;
                    }

                    var inst = Activator.CreateInstance(pi.PropertyType);
                    instanceProps[1].SetValue(inst, element.Value);
                    instanceProps[0].SetValue(inst, null);
                    pi.SetValue(vm.EditedItem, inst, null);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
            //if (source != null)
                element.Source = source;

            return element;
        }

        private FrameworkElement GetNullableSimpleControl(PropertyInfo pi, Binding binding, ViewModel vm, object source, Type propType)
        {

            NullableSimpleControl element = new NullableSimpleControl();
            element.Width = 120;
            element.Height = 30;
            element.PropertyType = propType;

            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    var instanceProps = pi.PropertyType.GetProperties();
                    if (element.Value == null)
                    {
                        //Здесь присвоить NilReason
                        var instance = Activator.CreateInstance(pi.PropertyType);
                        instanceProps[0].SetValue(instance, element.nilReasonCbx.SelectedItem);
                        pi.SetValue(vm.EditedItem, instance, null);
                        return;
                    }
                    
                     var inst = Activator.CreateInstance(pi.PropertyType);

                    try
                    {
                        
                        instanceProps[1].SetValue(inst, Convert.ChangeType(element.Value, instanceProps[1].PropertyType));
                        instanceProps[0].SetValue(inst, null);
                        pi.SetValue(vm.EditedItem, inst, null);
                    }
                    catch (Exception ex)
                    {

                        var value = pi.GetValue(vm.EditedItem, null);
                        if (value == null)
                            element.valueTbx.Text = string.Empty;
                        else
                            element.valueTbx.Text = value.ToString();

                        _propertySetExceptionHandler(ex);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
            //if (source != null)
                element.Source = source;

            return element;
        }

        private FrameworkElement GetMultiSelectElement(PropertyInfo pi, Binding binding, ViewModel vm, List<AM_AbstractFeature> featList)
        {

            MultiSelectComboBox element = new MultiSelectComboBox()
            {
                ItemsSource = featList,

            };
            element.Height = 30;
            binding.Converter = new ListPropertyConverter();

            element.SetBinding(MultiSelectComboBox.SelectedItemsProperty, binding);

            element.CheckBoxClicked += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    Type itemType = pi.PropertyType.GetGenericArguments()[0];

                    MethodInfo castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(itemType);
                    object castedSelectedItems = castMethod.Invoke(null, new object[] { element.SelectedItems });

                    MethodInfo toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(itemType);
                    var resultSelectedItems = toListMethod.Invoke(null, new object[] { castedSelectedItems });

                    pi.SetValue(vm.EditedItem, resultSelectedItems, null);

                }
                catch (Exception ex)
                {
                    element.MultiSelectCombo.SelectedItem = pi.GetValue(vm.EditedItem, null);
                    _propertySetExceptionHandler(ex);
                }

            });

            return element;
        }

        private FrameworkElement GetDataTypeElement(PropertyInfo pi, Binding binding, ViewModel vm, object source)
        {

            DataTypeControl element = new DataTypeControl();
            element.Width = 120;
            element.Height = 30;
            var instanceProps = pi.PropertyType.GetProperties();

                    
           
            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    if (element.Selected == null)
                    {
                        pi.SetValue(vm.EditedItem, null, null);
                        return;
                    }

                    var attr = pi.PropertyType.GetGenericArguments();

                    var inst = Activator.CreateInstance(pi.PropertyType);
                    var props = pi.PropertyType.GetProperties();
                    props[0].SetValue(inst, element.Selected.Value);
                    props[1].SetValue(inst, element.Selected.Uom);

                    pi.SetValue(vm.EditedItem, inst, null);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        
            });
            element.ComboSource = pi.PropertyType.GetGenericArguments()[0].GetEnumValues();

            if (source != null)
            {
                DataType<Enum> convertedSource = new DataType<Enum>()
                {
                    Uom = (Enum)instanceProps[1].GetValue(source),
                    Value = (double)instanceProps[0].GetValue(source)
                };
                element.Source = convertedSource;
            }

            return element;
        }

        private FrameworkElement GetGeometryElement(PropertyInfo pi, Binding binding, ViewModel vm,object source)
        {
            GeometryControl element = new GeometryControl();

            element.EditedItem = (AM_AbstractFeature)vm.EditedItem;

            if (source!=null && !((IGeometry)source).IsEmpty)
            {

                element.Value = (IGeometry)source;
                element.hasValueCheckbox.IsChecked = true;
            }
            
            //Оставить только один из них: Source или Selected
            element.ValueChanged += new SelectionChangedEventHandler((s, e) =>
            {
            try
            {
                if (element.Value == null)
                    {
                        pi.SetValue(vm.EditedItem, null, null);
                        return;
                    }

                    pi.SetValue(vm.EditedItem, element.Value, null);
                }
                catch (Exception ex)
                {
                    throw;
                }

            });
            
           

            return element;
        }

        private FrameworkElement GetNullableDateTimeElement(PropertyInfo pi, Binding binding, ViewModel vm)
        {
            DatePicker element = new DatePicker();
            element.Width = 120;
            element.SelectedDate = null;
            element.SelectedDateChanged += new EventHandler<System.Windows.Controls.SelectionChangedEventArgs>((s, e) =>
            {
                try
                {
                    var inst = Activator.CreateInstance(pi.PropertyType);
                    var props = pi.PropertyType.GetProperties();
                    props[1].SetValue(inst, element.SelectedDate);
                    pi.SetValue(vm.EditedItem, inst, null);
                }
                catch (Exception ex)
                {
                    element.SelectedDate = (DateTime)pi.GetValue(vm.EditedItem, null);
                    _propertySetExceptionHandler(ex);
                }
            });

            var instanceProps = pi.PropertyType.GetProperties();
            var source = pi.GetValue(vm.EditedItem);
            if (source != null && (DateTime)instanceProps[1].GetValue(source) != default(DateTime))
                element.SelectedDate = (DateTime)instanceProps[1].GetValue(source);

            return element;
        }

        private FrameworkElement GetNullableGenericValueElement(PropertyInfo pi, Binding binding, ViewModel vm)
        {
            TextBox element = new TextBox()
            {
                // TODO: auslagern in eine "ValuePropertyWidth" vom KVGrid
                Width = 120,              
            };

            element.LostFocus += new RoutedEventHandler((s, e) =>
            {
                try
                {
                    var inst = Activator.CreateInstance(pi.PropertyType);
                    var props = pi.PropertyType.GetProperties();
                    props[1].SetValue(inst, Convert.ChangeType(element.Text, props[1].PropertyType));

                    pi.SetValue(vm.EditedItem, inst, null);
                }
                catch (Exception ex)
                {
                    var value = pi.GetValue(vm.EditedItem, null);
                    if (value == null)
                        element.Text = string.Empty;
                    else
                        element.Text = value.ToString();

                    _propertySetExceptionHandler(ex);
                }
            });
            var instanceProps = pi.PropertyType.GetProperties();

            var source = pi.GetValue(vm.EditedItem);
            if (source != null)
            {
                var val = instanceProps[1].GetValue(source);
                if (val != null)
                    element.Text = val.ToString();
            }


            return element;
        }

        private FrameworkElement GetNullableEnumElement(PropertyInfo pi, Binding binding, ViewModel vm, object source)
        {
            ComboBox element = new ComboBox()
            {
                ItemsSource = pi.PropertyType.GetGenericArguments()[0].GetEnumValues()
            };
            element.Width = 120;
            element.SelectionChanged += new SelectionChangedEventHandler((s, e) =>
            {
                try
                {
                    var inst = Activator.CreateInstance(pi.PropertyType);
                    var props = pi.PropertyType.GetProperties();
                    props[1].SetValue(inst, element.SelectedItem);

                    pi.SetValue(vm.EditedItem, inst, null);
                }
                catch (Exception ex)
                {
                    element.SelectedItem = pi.GetValue(vm.EditedItem, null);
                    _propertySetExceptionHandler(ex);
                }
            });

            var instanceProps = pi.PropertyType.GetProperties();
            if (source != null)
                element.SelectedItem = (Enum)instanceProps[1].GetValue(source);

            return element;
        }

        private void SetElementEnabled(KeyValuePair<PropertyInfo, ICrudPropertyMetadata> crudProperty,
            ViewModel vm, FrameworkElement element)
        {
            element.IsEnabled = false;

            if (crudProperty.Value.IsEnabledInEdit && crudProperty.Value.IsEnabledInNew)
            {
                element.SetBinding(FrameworkElement.IsEnabledProperty, new Binding()
                {
                    Source = vm,
                    Path = new PropertyPath("IsUserInputEnabled")
                });
            }
            else if (crudProperty.Value.IsEnabledInEdit)
            {
                element.SetBinding(FrameworkElement.IsEnabledProperty, new Binding()
                {
                    Source = vm,
                    Path = new PropertyPath("IsInEditMode")
                });
            }
            else if (crudProperty.Value.IsEnabledInNew)
            {
                element.SetBinding(FrameworkElement.IsEnabledProperty, new Binding()
                {
                    Source = vm,
                    Path = new PropertyPath("IsInAddMode")
                });
            }
        }
	}
    
}
