Imports Telerik.WinControls.UI
Imports Telerik.QuickStart.WinControls

Namespace Telerik.Examples.WinControls.Scheduler.CustomAppointmentDialog
	Partial Public Class Form1
		Inherits ExamplesForm
		Private appointmentDialog As CustomEditAppointmentDialog = Nothing

		Public Sub New()
			InitializeComponent()

			Me.radSchedulerDemo.AppointmentFactory = New CustomAppointmentFactory()
		End Sub

		Private Sub radSchedulerDemo_AppointmentEditDialogShowing(ByVal sender As Object, ByVal e As AppointmentEditDialogShowingEventArgs)
            Me.appointmentDialog = New CustomEditAppointmentDialog()
			Me.appointmentDialog.ThemeName = Me.radSchedulerDemo.ThemeName
			e.AppointmentEditDialog = Me.appointmentDialog
		End Sub

		Protected Overrides Sub WireEvents()
			AddHandler radSchedulerDemo.AppointmentEditDialogShowing, AddressOf radSchedulerDemo_AppointmentEditDialogShowing
		End Sub
	End Class
End Namespace
