using System.Windows.Controls;
using Microsoft.Win32;
using MvvmCore;
using TOSSM.ViewModel.Document.Slot;

namespace TOSSM.View.Document.Slot
{
    /// <summary>
    /// Interaction logic for SlotValidationOverviewView.xaml
    /// </summary>
    public partial class SlotValidationOverviewView : UserControl
    {
        private RelayCommand _onMenuCommand;

        public RelayCommand OnMenuCommand
        {
            get
            {
                return _onMenuCommand ?? (_onMenuCommand = new RelayCommand(
                    t =>
                    {
                        var model = DataContext as SlotValidationOverviewViewModel;
                        if (model != null && !string.IsNullOrEmpty(model.MoreDetailedMessage))
                        {
                            var saveFileDialog = new SaveFileDialog
                            {
                                FileName = "Debug.txt",
                                DefaultExt = ".txt",
                                Filter = "Text files (.txt)|*.txt",
                            };

                            if (saveFileDialog.ShowDialog() != true) return;

                            System.IO.File.WriteAllText(saveFileDialog.FileName, model.MoreDetailedMessage);
                        }
                    },
                    t =>
                    {
                        var model = DataContext as SlotValidationOverviewViewModel;
                        return (model != null && !string.IsNullOrEmpty(model.MoreDetailedMessage));
                    }));
            }
        }


        public SlotValidationOverviewView()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                var cm = new ContextMenu();
                cm.Items.Add(new MenuItem{Header = "Save debug information to file", Command = OnMenuCommand});
                DataGrid.ContextMenu = cm; 
            };
        }
    }
}
