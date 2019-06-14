using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms.Design;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Display;

namespace ANCOR.MapCore
{
    public class enuInstalledFontsList : StringConverter
    {

        /// <summary>
        /// Будем предоставлять выбор из списка
        /// </summary>
        public override bool GetStandardValuesSupported(
          ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// ... и только из списка
        /// </summary>
        public override bool GetStandardValuesExclusive(
          ITypeDescriptorContext context)
        {
            // false - можно вводить вручную
            // true - только выбор из списка
            return true;
        }

        /// <summary>
        /// А вот и список
        /// </summary>
        public override StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {

            List<string> Coll = new System.Collections.Generic.List<string>();

            System.Drawing.Text.InstalledFontCollection installedFontCollection = new System.Drawing.Text.InstalledFontCollection();
            FontFamily[] fontFamilies;
            fontFamilies = installedFontCollection.Families;

            for (int i = 0; i <= fontFamilies.Length - 1; i++)
            {
                Coll.Add(fontFamilies[i].Name);
            }

            return new StandardValuesCollection(Coll);
        }
    }

    public class FillStyleEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            // картинки хранятся в ресурсах с именами, соответствующими
            // именам каждого члена перечисления
            string resourcename = ((fillStyle)e.Value).ToString();

            // достаем картинку из ресурсов

            Bitmap FillStyleImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourcename);
            Rectangle destRect = e.Bounds;
            //FillStyleImage.MakeTransparent();

            // и отрисовываем
            e.Graphics.DrawImage(FillStyleImage, destRect);



        }
    }

    public class LineStyleEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            // картинки хранятся в ресурсах с именами, соответствующими
            // именам каждого члена перечисления 
            string resourcename = ((lineStyle)e.Value).ToString();

            // достаем картинку из ресурсов

            Bitmap LineStyleImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourcename);
            Rectangle destRect = e.Bounds;
            //FillStyleImage.MakeTransparent();

            // и отрисовываем
            e.Graphics.DrawImage(LineStyleImage, destRect);
        }

    }

    public class LineCalloutStyleEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            // картинки хранятся в ресурсах с именами, соответствующими
            // именам каждого члена перечисления 
            string resourcename = ((lineCalloutStyle)e.Value).ToString();

            // достаем картинку из ресурсов

            Bitmap LineStyleImage = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourcename);
            Rectangle destRect = e.Bounds;
            //FillStyleImage.MakeTransparent();

            // и отрисовываем
            e.Graphics.DrawImage(LineStyleImage, destRect);
        }

    }

    //public class MyColorEdotor : UITypeEditor
    //{

    //    public static Action objectChanged;
    //    public static Action<object, string, object, object> undoChanged;


    //    public static Action FontColorChanged;

    //    public override System.Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, System.Object value)
    //    {

    //        if ((context != null) && (provider != null))
    //        {
    //            AncorColor prevval = CloneColor(value as AncorColor);
    //            IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

    //            if ((svc != null) && (((AncorColor)value) != null))
    //            {
    //                using (System.Windows.Forms.ColorDialog frm = new System.Windows.Forms.ColorDialog())
    //                {

    //                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //                    {
    //                        ((AncorColor)value).Red = frm.Color.R;
    //                        ((AncorColor)value).Green = frm.Color.G;
    //                        ((AncorColor)value).Blue = frm.Color.B;

    //                        if (FontColorChanged!=null) FontColorChanged();

    //                        if (objectChanged != null)
    //                        {
    //                            objectChanged();
    //                        }

    //                        if (undoChanged != null)
    //                        {
    //                            AncorColor nextval = CloneColor(value as AncorColor);
    //                            undoChanged(context.Instance, context.PropertyDescriptor.Name, prevval, nextval);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        return base.EditValue(context, provider, value);
    //    }

    //    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    //    {
    //        if (context != null)
    //            return UITypeEditorEditStyle.Modal;
    //        else
    //            return base.GetEditStyle(context);
    //    }

    //    public override bool GetPaintValueSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }

    //    public override void PaintValue(PaintValueEventArgs e)
    //    {
    //        if (e.Value == null) return;
    //        int R_ = ((AncorColor)e.Value).Red;
    //        int G_ = ((AncorColor)e.Value).Green;
    //        int B_ = ((AncorColor)e.Value).Blue;

    //        Color _Color = Color.FromArgb(R_, G_, B_);
    //        SolidBrush _Brush = new SolidBrush(_Color);

    //        // Create location and size of rectangle.
    //        int x = 1;
    //        int y = 1;
    //        int width = 19;
    //        int height = 12;

    //        // Fill rectangle to screen.

    //        e.Graphics.FillRectangle(_Brush, x, y, width, height);
    //    }

    //    AncorColor CloneColor(AncorColor color)
    //    {
    //        AncorColor newcolor = new AncorColor(color.Red, color.Green, color.Blue);
    //        return newcolor;
    //    }
    //}

    public class SigmaColorEdotor : UITypeEditor
    {

        public static Action objectChanged;
        public static Action<object, string, object, object> undoChanged;


        public static Action FontColorChanged;

        public override System.Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, System.Object value)
        {

            if ((context != null) && (provider != null))
            {
                AncorColor obj = (AncorColor)value;

                IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (svc != null && value != null)
                {
                    IColor _Clr = obj.GetColor();

                    var posMs = System.Windows.Forms.Cursor.Position;

                    ESRI.ArcGIS.esriSystem.tagRECT prect = new ESRI.ArcGIS.esriSystem.tagRECT { bottom = posMs.Y, left = posMs.X, right = 0, top = posMs.Y };
                    IColorPalette clrPal = new ColorPalette();
                    bool flag = clrPal.TrackPopupMenu(prect, _Clr, false, 0);
                    if (flag)
                    {
                        _Clr = clrPal.Color;
                        obj.AncorColor_CMYK = _Clr.CMYK;
                        obj.AncorColor_NullColor = _Clr.NullColor;
                        obj.AncorColor_RGB = _Clr.RGB;
                        obj.AncorColor_Transparency = _Clr.Transparency;
                        obj.AncorColor_UseWindowsDithering = _Clr.UseWindowsDithering;

                        if (FontColorChanged != null) FontColorChanged();

                        if (objectChanged != null)
                        {
                            objectChanged();
                        }

                    }

                    //IColorBrowser colorBrowser = new ColorBrowser();
                    //colorBrowser.Color = _Clr;
                    //bool flag = colorBrowser.DoModal(0);
                    //if (flag)
                    //{
                    //    _Clr = colorBrowser.Color;
                    //    obj.AncorColor_CMYK = _Clr.CMYK;
                    //    obj.AncorColor_NullColor = _Clr.NullColor;
                    //    obj.AncorColor_RGB = _Clr.RGB;
                    //    obj.AncorColor_Transparency = _Clr.Transparency;
                    //    obj.AncorColor_UseWindowsDithering = _Clr.UseWindowsDithering;

                    //    if (FontColorChanged != null) FontColorChanged();

                    //    if (objectChanged != null)
                    //    {
                    //        objectChanged();
                    //    }

                    //}



                }
            }

            return base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null)
                return UITypeEditorEditStyle.Modal;
            else
                return base.GetEditStyle(context);
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value == null) return;

            // Create location and size of rectangle.
            int x = 1;
            int y = 1;
            int width = 19;
            int height = 12;

            if (e.Value is AncorColor)
            {

                RgbColor rgbC = new RgbColorClass();
                rgbC.RGB = ((AncorColor)e.Value).AncorColor_RGB;
                int R_ = rgbC.Red;
                int G_ = rgbC.Green;
                int B_ = rgbC.Blue;

                Color _Color = Color.FromArgb(R_, G_, B_);
                // Fill rectangle to screen.
                SolidBrush _Brush = new SolidBrush(_Color);
                e.Graphics.FillRectangle(_Brush, x, y, width, height);
               
            }
           

           
        }

      
    }


    #region Атрибут для поддержки динамически показываемых свойств

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    class DynamicPropertyFilterAttribute : Attribute
    {
        string _propertyName;

        /// <summary>
        /// Название свойства, от которого будет зависить видимость
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        string _showOn;

        /// <summary>
        /// Значения свойства, от которого зависит видимость 
        /// (через запятую, если несколько), при котором свойство, к
        /// которому применен атрибут, будет видимо. 
        /// </summary>
        public string ShowOn
        {
            get { return _showOn; }
        }

        ShowOnCondition _condition;

        public ShowOnCondition Condition
        {
            get { return _condition; }
        }

        /// <summary>
        /// Конструктор  
        /// </summary>
        /// <param name="propName">Название свойства, от которого будет 
        /// зависеть видимость</param>
        /// <param name="value">Значения свойства (через запятую, если несколько), 
        /// при котором свойство, к которому применен атрибут, будет видимо.</param>
        public DynamicPropertyFilterAttribute(string propertyName, string value, ShowOnCondition showOnCondition)
        {
            _propertyName = propertyName;
            _showOn = value;
            _condition = showOnCondition;
        }
    }

    #endregion

    #region Базовый класс для объектов, поддерживающих динамическое  отображение свойств в PropertyGrid

    public class FilterablePropertyBase : ICustomTypeDescriptor
    {

        protected PropertyDescriptorCollection
          GetFilteredProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc
              = TypeDescriptor.GetProperties(this, attributes, true);

            PropertyDescriptorCollection finalProps =
              new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            foreach (PropertyDescriptor pd in pdc)
            {
                bool include = false;
                bool dynamic = false;


                foreach (Attribute a in pd.Attributes)
                {
                    try
                    {
                        if (a is DynamicPropertyFilterAttribute)
                        {
                            dynamic = true;

                            DynamicPropertyFilterAttribute dpf =
                             (DynamicPropertyFilterAttribute)a;

                            PropertyDescriptor temp = pdc[dpf.PropertyName];

                            if (dpf.Condition == ShowOnCondition.ifEqual)
                            {
                                if (dpf.ShowOn.IndexOf(temp.GetValue(this).ToString()) > -1)
                                    include = true;
                            }
                            else
                            {
                                if (dpf.ShowOn.IndexOf(temp.GetValue(this).ToString()) == -1)
                                    include = true;
                            }
                        }
                    }
                    catch
                    {
                        include = true;
                    }
                }

                if (!dynamic || include)
                    finalProps.Add(pd);
            }

            return finalProps;
        }

        #region ICustomTypeDescriptor Members

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public PropertyDescriptorCollection GetProperties(
          Attribute[] attributes)
        {
            return GetFilteredProperties(attributes);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return GetFilteredProperties(new Attribute[0]);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        #endregion
    }

    #endregion

    #region  Возвращает упорядоченный список свойств

    public class PropertySorter : ExpandableObjectConverter
    {
        #region Методы

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


        public override PropertyDescriptorCollection GetProperties(
          ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc =
              TypeDescriptor.GetProperties(value, attributes);

            ArrayList orderedProperties = new ArrayList();

            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];

                if (attribute != null)
                {
                    // атрибут есть - используем номер п/п из него
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    // атрибута нет – считаем, что 0
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }

            // сортируем по Order-у
            orderedProperties.Sort();

            // формируем список имен свойств
            ArrayList propertyNames = new ArrayList();

            foreach (PropertyOrderPair pop in orderedProperties)
                propertyNames.Add(pop.Name);

            // возвращаем
            return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
        }

        #endregion
    }

    #endregion

    #region PropertyOrder Attribute

    /// <summary>
    /// Атрибут для задания сортировки
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get { return _order; }
        }
    }

    #endregion

    #region PropertyOrderPair

    /// <summary>
    /// Пара имя/номер п/п с сортировкой по номеру
    /// </summary>
    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;

        public string Name
        {
            get { return _name; }
        }

        public PropertyOrderPair(string name, int order)
        {
            _order = order;
            _name = name;
        }

        /// <summary>
        /// Собственно метод сравнения
        /// </summary>
        public int CompareTo(object obj)
        {
            int otherOrder = ((PropertyOrderPair)obj)._order;

            if (otherOrder == _order)
            {
                // если Order одинаковый - сортируем по именам
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
                return -1;

            return 1;
        }
    }

    #endregion

}
