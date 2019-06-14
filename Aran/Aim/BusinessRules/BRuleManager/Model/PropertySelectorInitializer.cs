using Aran.Aim;
using MvvmCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class PropertySelectorInitializer : BaseInitializer
    {
        private static string _rootText = "Features and Objects";
        private string _prevSearchText;

        public PropertySelectorInitializer(object model) : base (model)
        {
            DefaultSize = new System.Windows.Size(600, 600);
            ShowInTaskbar = false;

            _prevSearchText = string.Empty;

            var pathItems = new ObservableCollection<ExpandoObject>();
            pathItems.CollectionChanged += (sender, e) => FillItems();

            _model.PathItems = pathItems;
            _model.PathText = string.Empty;
            _model.RootText = _rootText;
            _model.SelectedItem = null;
            _model.SearchText = _prevSearchText;
            _model.AddRootFeature = new Action<object>(AddRootFeature);

            SetCommands();
            FillItems();
        }

        private void SetCommands()
        {
            _model.NextBackCommand = new RelayCommand(OnNextBackCommand,
                (cp) =>
                {
                    if ("next".Equals(cp))
                        return _model.SelectedItem != null;
                    else
                        return _model.PathItems.Count > 0;
                });

            _model.OkCancelCommand = new RelayCommand(OnOkCancelCommand,
                (cp) =>
                {
                    if ("ok".Equals(cp))
                        return !string.IsNullOrWhiteSpace(_model.PathText);
                    return true;
                });
        }

        private void OnNextBackCommand(object arg)
        {
            _prevSearchText = string.Empty;
            _model.SearchText = _prevSearchText;

            ObservableCollection<ExpandoObject> pathItems = _model.PathItems;

            if ("next".Equals(arg))
            {
                dynamic selectedItem = _model.SelectedItem;
                if (selectedItem != null)
                {
                    if (pathItems.Count == 0)
                        _model.RootText = selectedItem.Name;
                    pathItems.Add(selectedItem);
                }
            }
            else //*** back
            {
                if (pathItems.Count > 0)
                {
                    pathItems.RemoveAt(pathItems.Count - 1);
                    if (pathItems.Count == 0)
                        _model.RootText = _rootText;
                }
            }

            _model.PathText = PathItemsToText(pathItems);
        }

        private void OnOkCancelCommand(object arg)
        {
            if ("ok".Equals(arg))
            {
                var okAction = _model.OkAction as Action<string>;
                okAction?.Invoke(_model.PathText);

                OnCloseRequested();
            }
            else //*** cancel
            {
                OnCloseRequested();
            }
        }

        private string PathItemsToText(IList<ExpandoObject> pathItems)
        {
            var sb = new StringBuilder();

            for (var i = 1; i < pathItems.Count; i++)
            {
                dynamic item = pathItems[i];
                sb.Append((sb.Length == 0 ? "" : ".") + item.Name);

                if (item.ClassInfo.AimObjectType == AimObjectType.Object && !string.IsNullOrEmpty(item.Type))
                    sb.Append("." + item.Type);
            }

            return sb.ToString();
        }

        private void FillItems()
        {
            var items = new List<ExpandoObject>();
            dynamic lastPath = (_model.PathItems as ObservableCollection<ExpandoObject>).LastOrDefault();

            if (lastPath == null)
            {
                foreach (var cInfo in AimTypeItems.ClassInfoList)
                {
                    if (!cInfo.AixmName.StartsWith(_prevSearchText, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    dynamic eo = new ExpandoObject();
                    eo.Name = cInfo.AixmName;
                    eo.Type = cInfo.AimObjectType.ToString();
                    eo.ClassInfo = cInfo;
                    items.Add(eo);
                }
            }
            else
            {
                var cInfo = lastPath.ClassInfo as AimClassInfo;

                foreach(var pInfo in AimTypeItems.GetProperties(cInfo))
                {
                    if (cInfo.AixmName != null && 
                        !cInfo.AixmName.StartsWith(_prevSearchText, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    dynamic eo = new ExpandoObject();

                    if (!string.IsNullOrEmpty(pInfo.AixmName))
                        eo.Name = pInfo.AixmName;
                    else if (cInfo.SubClassType == AimSubClassType.ValClass && pInfo.Name == "Value")
                        eo.Name = "value";

                    var propClassInfo = pInfo.PropType;

                    if (propClassInfo.AimObjectType == AimObjectType.Object ||
                        propClassInfo.SubClassType == AimSubClassType.ValClass)
                    {
                        eo.Type = pInfo.PropType.AixmName;
                    }
                    else
                    {
                        eo.Type = string.Empty;
                    }

                    eo.ClassInfo = pInfo.PropType;
                    items.Add(eo);
                }
            }

            _model.Items = items;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SearchText")
            {
                string s = _model.SearchText;
                if (s != _prevSearchText)
                {
                    _prevSearchText = s;
                    FillItems();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        private void AddRootFeature(object arg)
        {
            var rootFeatureName = arg as string;

            foreach(dynamic item in _model.Items)
            {
                if (item.Name == rootFeatureName)
                {
                    _model.SelectedItem = item;
                    OnNextBackCommand("next");
                    break;
                }
            }
        }
    }
}
