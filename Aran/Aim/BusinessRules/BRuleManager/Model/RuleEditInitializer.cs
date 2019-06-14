using MvvmCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager.Model
{
    class RuleEditInitializer : BaseInitializer
    {
        public RuleEditInitializer(object model) : base (model)
        {
            DefaultSize = new System.Windows.Size(800, 670);
            WindowTitle = "Rule Properties";
            ShowInTaskbar = false;

            _model = model;

            _model.SaveCancelCommand = new RelayCommand(OnSaveCancelCommand);
            _model.Saved = null; //** Func<object, string>
            _model.TextualDescription = string.Empty;
            _model.ErrorText = string.Empty;
        }

        private void OnSaveCancelCommand(object arg)
        {
            if ("save".Equals(arg))
            {
                Func<object, string> saved = _model.Saved;
                var resultText = saved?.Invoke(_model);

                if (resultText == string.Empty)
                {
                    OnCloseRequested();
                }
                else
                {
                    _model.ErrorText = resultText;
                }
            }
            else
            {
                OnCloseRequested();
            }
        }
    }
}
