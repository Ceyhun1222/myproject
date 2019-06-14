using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BRuleManager.Model
{
    public static class ModelFactory
    {
        public static object Create(ModelType type)
        {
            dynamic model = new ExpandoObject();
            model._type = type;
            model.PropertyChangedAction = null;
            model._windowTitle = string.Empty;
            (model as INotifyPropertyChanged).PropertyChanged += ModelFactory_PropertyChanged;

            var initializerType = typeof(ModelFactory).Assembly.GetType($"BRuleManager.Model.{type}Initializer");
            if (initializerType != null)
                model._initializer = Activator.CreateInstance(initializerType, model);

            return model;
        }

        private static void ModelFactory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (sender as IDictionary<string, object>).TryGetValue(e.PropertyName + "Changed", out object prChangedActionObj);

            if (prChangedActionObj != null && prChangedActionObj is Action<object> prChangedAction)
            {
                prChangedAction.Invoke(sender);
            }
        }
    }

    public enum ModelType
    {
        Rules,
        RuleEdit,
        PropertySelector,
        Vocabulary,
        SbvrEdit,
        CheckReport,
        About,
        RulesGroup
    }
}
