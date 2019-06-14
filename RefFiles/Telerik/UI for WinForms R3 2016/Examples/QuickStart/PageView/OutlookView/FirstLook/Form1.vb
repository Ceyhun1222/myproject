Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.PageView.OutlookView.FirstLook
	Partial Public Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()

            Me.radPageViewPage1.Image = My.Resources.OutlookViewNotes
            Me.radPageViewPage2.Image = My.Resources.OutlookViewTasks
            Me.radPageViewPage3.Image = My.Resources.OutlookViewContacts
            Me.radPageViewPage4.Image = My.Resources.OutlookViewCalendar
            Me.radPageViewPage6.Image = My.Resources.OutlookViewMail
		End Sub

		Protected Overrides Sub WireEvents()
		End Sub
	End Class
End Namespace
