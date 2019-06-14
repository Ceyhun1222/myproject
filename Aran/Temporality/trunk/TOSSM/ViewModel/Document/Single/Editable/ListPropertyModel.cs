using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using Aran.Aim;
using TOSSM.Converter;
using TOSSM.ViewModel.Document.Editor;

namespace TOSSM.ViewModel.Document.Single.Editable
{
    public class ListPropertyModel : EditableSinglePropertyModel
    {
        public ListPropertyModel(FeatureSubEditorDocViewModel parentModel)
        {
            ParentModel = parentModel;
        }

        private int ListIndex { get; set; }

        #region Overrides of EditableSinglePropertyModel


        public override void UpdateStringValue()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (a, b) =>
                                 {
                                     Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                                     Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                                     StringValue = HumanReadableConverter.ToHuman(Value);
                                 };
            worker.RunWorkerAsync();
        }

        public override string PictureId
        {
            get { return ComplexPicture; }
            set { }
        }

        public void UpdateListIndex(int index)
        {
            //ListIndex = ParentModel.PropertyList.IndexOf(this);
            //if (ListIndex == -1) ListIndex = ParentModel.PropertyList.Count;

            //var list = ParentObject as IList;
            //if (list != null) ListIndex = list.IndexOf(Value);

            ListIndex = index;
            PropertyName = "Item " + (ListIndex + 1);
        }

        private object _listValue;
        public override object Value
        {
            get => _listValue;
            set
            {
                if (!(ParentObject is IList)) return;

                if (_listValue == value)
                {
                    //UpdateListIndex();
                    return;
                }

                if (value==null)
                {
                    //UpdateListIndex();
                    ParentModel.DeleteSelected();
                }
                else
                {
                    _listValue = value;
                    CheckIsNull();

                    //UpdateListIndex();
                    UpdateStringValue();
                }
               
                OnPropertyChanged("Value");
            }
        }

        public AimPropInfo PropInfo { get; set; }

        #endregion
    }
}
