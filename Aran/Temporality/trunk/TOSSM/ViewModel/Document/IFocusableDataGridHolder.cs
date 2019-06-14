using System.Windows;
using System.Windows.Controls;

namespace TOSSM.ViewModel.Document
{
    public interface IFocusableDataGridHolder
    {
        DataGrid FocusableDataGrid { get; }
        Visibility Visibility { get; set; }
    }
}
