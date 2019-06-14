using System;
using System.Windows;
using System.Windows.Data;

namespace Aran.Temporality.CommonUtil.Extender
{
    public class BindingExtender : FrameworkElement
    {
        #region Source DP
        //We don't know what will be the Source/target type so we keep 'object'.
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(object), typeof(BindingExtender),
          new FrameworkPropertyMetadata
              {
              BindsTwoWayByDefault = true,
              DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
          });
        public Object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        #endregion

        #region Target DP
        //We don't know what will be the Source/target type so we keep 'object'.
        public static readonly DependencyProperty TargetProperty =
          DependencyProperty.Register("Target", typeof(object), typeof(BindingExtender),
          new FrameworkPropertyMetadata
              {
              BindsTwoWayByDefault = true,
              DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
          });
        public Object Target
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name == SourceProperty.Name)
            {
                //no loop wanted
                if (!ReferenceEquals(Source, Target))
                    Target = Source;
            }
            else if (e.Property.Name == TargetProperty.Name)
            {
                //no loop wanted
                if (!ReferenceEquals(Source, Target))
                    Source = Target;
            }
        }
    }
}
