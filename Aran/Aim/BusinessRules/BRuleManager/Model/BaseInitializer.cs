using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace BRuleManager.Model
{
    class BaseInitializer
    {
        protected dynamic _model;

        protected BaseInitializer(object model)
        {
            _model = model;
            DefaultSize = Size.Empty;
            ShowInTaskbar = true;

            (_model as INotifyPropertyChanged).PropertyChanged += ((sender, e) => OnPropertyChanged(e.PropertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
        }

        protected Size DefaultSize
        {
            get => _model._defaultSize;
            set => _model._defaultSize = value;
        }

        protected string WindowTitle
        {
            get => _model._windowTitle;
            set => _model._windowTitle = value;
        }

        protected bool WindowIsLoading
        {
            get => _model._windowIsLoading;
            set => _model._windowIsLoading = value;
        }

        protected bool ShowInTaskbar
        {
            get => _model._showInTaskbar;
            set => _model._showInTaskbar = value;
        }

        protected void OnCloseRequested()
        {
            var closeAction = _model.CloseRequested as Action<object>;
            closeAction?.Invoke(_model);
        }
    }
}
