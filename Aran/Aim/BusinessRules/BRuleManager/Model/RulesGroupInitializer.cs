using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class RulesGroupInitializer : BaseInitializer
    {
        private IEnumerable<ExpandoObject> _rules;

        public RulesGroupInitializer(object model) : 
            base(model)
        {
            _model.Columns = GetGroups();
            _model.Items = new ObservableCollection<string>();
            _model.SetRules = new Action<IEnumerable<ExpandoObject>>(SetRules);
            _model.ClearSelectedGroup = new Action(OnClearSelectedGroup);
        }

        private void SetRules(IEnumerable<ExpandoObject> rules)
        {
            _rules = rules;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == "SelectedColumn")
            {
                FillGroupItems();
            }
            else if (propertyName == "GroupSelectedItem")
            {
                _model.GroupSelected(_model.SelectedColumn.DataSource, _model.GroupSelectedItem);
            }
        }

        private ObservableCollection<ExpandoObject> GetGroups()
        {
            var list = new ObservableCollection<ExpandoObject>();

            dynamic item = new ExpandoObject();
            item.Header = "";
            item.DataSource = "";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Name";
            item.DataSource = "Name";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Profile";
            item.DataSource = "Profile";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Active";
            item.DataSource = "IsActive";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Source";
            item.DataSource = "Source";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Category";
            item.DataSource = "Category";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Tags";
            item.DataSource = "Tags";
            list.Add(item);

            item = new ExpandoObject();
            item.Header = "Custom";
            item.DataSource = "Custom";
            list.Add(item);



            return list;
        }

        private void FillGroupItems()
        {
            dynamic colItem = _model.SelectedColumn;
            if (colItem == null || colItem.DataSource == "")
            {
                _model.Items = new ObservableCollection<string>();
                return;
            }

            var isTag = colItem.DataSource == "Tags";

            var hashSet = new HashSet<string>();

            foreach (IDictionary<string, object> rule in _rules)
            {
                if (rule.TryGetValue(colItem.DataSource, out object propValue) && propValue != null)
                {
                    var propText = propValue.ToString();

                    if (isTag)
                    {
                        var strArr = propText.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        foreach (var tag in strArr)
                            hashSet.Add(tag.Trim());
                    }
                    else
                    {
                        hashSet.Add(propText);
                    }
                }
            }

            _model.Items = new ObservableCollection<string>(hashSet);
        }

        private void OnClearSelectedGroup()
        {
            foreach(dynamic item in _model.Columns)
            {
                if (item.Header == "")
                {
                    _model.SelectedColumn = item;
                    break;
                }
            }
        }
    }

}
