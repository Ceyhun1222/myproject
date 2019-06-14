using Aran.Aim;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ArenaErrorReport.ViewModel
{
    public class ErrorReportViewModel: StateViewModel
    {
        private RelayCommand _saveCommand;
        private RelayCommand _continueCommand;
        private RelayCommand _cancelCommand;
        
        private DeserializedErrorInfo _selectedError;
        private ObservableCollection<DeserializedErrorInfo> _errorSource;
        private MainViewModel _mainVeiewModel;

        public ErrorReportViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _mainVeiewModel = mainViewModel;
        }

        public ErrorReportViewModel(MainViewModel mainViewModel, System.Collections.Generic.List<DeserializedErrorInfo> errorInfoList) : this(mainViewModel)
        {
            ErrorSource =new ObservableCollection<DeserializedErrorInfo>(errorInfoList);
        }

        #region Properties

        public ObservableCollection<DeserializedErrorInfo> ErrorSource
        {
            get => _errorSource;
            set => Set(ref _errorSource, value);
        }
        
        public DeserializedErrorInfo SelectedError
        {
            get => _selectedError;
            set
            {
                Set(ref _selectedError, value);
               
            }
        }     

        #endregion

        #region Commands

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {

                    try
                    {
                        var sfd = new SaveFileDialog();
                        sfd.Filter = "HTML (*.html)|*.html|All Files (*.*)|(*.*)";

                        if (sfd.ShowDialog() != DialogResult.OK)
                            return;

                        var fileName = sfd.FileName;
                        using (var sw = File.CreateText(sfd.FileName))
                        {
                            sw.WriteLine("<!DOCTYPE html>");
                            sw.WriteLine("<html>");
                            sw.WriteLine("<head>");
                            sw.WriteLine("<style>");
                            sw.WriteLine("table, th, td {");
                            sw.WriteLine("    border: 1px solid black;");
                            sw.WriteLine("    border-collapse: collapse;");
                            sw.WriteLine("}");
                            sw.WriteLine("th, td {");
                            sw.WriteLine("    padding: 5px;");
                            sw.WriteLine("}");
                            sw.WriteLine("</style>");
                            sw.WriteLine("</head>");
                            sw.WriteLine("<body>");
                            sw.WriteLine("");
                            sw.WriteLine("<table style=\"width:100%\">");
                            sw.WriteLine("  <tr>");
                            sw.WriteLine("    <th>FeatureType</th>");
                            sw.WriteLine("    <th>Identifier</th>");
                            sw.WriteLine("    <th>Property Name</th>");
                            sw.WriteLine("    <th>Message</th>");
                            sw.WriteLine("    <th>Action</th>");
                            sw.WriteLine("  </tr>");


                            foreach (var item in ErrorSource)
                            {
                                var row = new object[] { item.FeatureType, item.Identifier, item.PropertyName, item.ErrorMessage, item.Action };

                                sw.WriteLine("<tr>");
                                foreach (var cell in row)
                                    sw.WriteLine("<td>" + cell + "</td>");
                                sw.WriteLine("</tr>");
                            }

                            sw.WriteLine("</table>");
                            sw.WriteLine("</body>");
                            sw.WriteLine("</html>");

                            sw.Close();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {

                    }
                   
                }));
            }
        }

        public RelayCommand ContinueCommand
        {
            get
            {
                return _continueCommand ?? (_continueCommand = new RelayCommand(() =>
                {
                    
                    _mainVeiewModel.Close();
                    
                }));
            }
        }


        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() =>
                {
                    _mainVeiewModel.Close();
                    
                }));
            }
        }

        #endregion

        #region From StateViewModel

        public override bool CanNext() => true;

        public override bool CanPrevious() => false;

        protected override void next()
        {
            throw new NotImplementedException();
        }

        protected override void previous()
        {
            throw new NotImplementedException();
        }

        protected override void _destroy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    
}
