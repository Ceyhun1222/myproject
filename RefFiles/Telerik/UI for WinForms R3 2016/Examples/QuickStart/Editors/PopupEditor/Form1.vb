
Imports System.Linq
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Editors.PopupEditor
    Partial Public Class Form1
        Inherits ExamplesForm
        Public Sub New()
            Me.InitializeComponent()
            Me.radCalendar1.SelectedDate = DateTime.Now
        End Sub

        Private Sub timer1_Tick(sender As Object, e As EventArgs) Handles timer1.Tick
            Me.radPopupEditor1.Text = String.Format("{0} {1}", Me.radCalendar1.FocusedDate.[Date].ToString("yyyy/MM/dd"), DateTime.Now.ToString("HH:mm:ss"))
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
            Me.radPopupEditor1.PopupEditorElement.ShowPopup()
        End Sub
    End Class
End Namespace


