
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.Scheduler.Grouping
	Public Partial Class Form1
		Inherits ExamplesForm
		Public Sub New()
			InitializeComponent()
		End Sub

		Protected Overrides Sub OnLoad(e As EventArgs)
			MyBase.OnLoad(e)

			Dim baseDate As DateTime = DateTime.Today
			Dim start As DateTime() = New DateTime() {baseDate.AddHours(14.0), baseDate.AddDays(1.0).AddHours(9.0), baseDate.AddDays(2.0).AddHours(13.0)}
			Dim [end] As DateTime() = New DateTime() {baseDate.AddHours(16.0), baseDate.AddDays(1.0).AddHours(15.0), baseDate.AddDays(2.0).AddHours(17.0)}
			Dim summaries As String() = New String() {"Mr. Brown", "Mr. White", "Mrs. Green"}
			Dim descriptions As String() = New String() {"", "", ""}
			Dim locations As String() = New String() {"City", "Out of town", "Service Center"}
			Dim backgrounds As AppointmentBackground() = New AppointmentBackground() {AppointmentBackground.Business, AppointmentBackground.MustAttend, AppointmentBackground.Personal}

			Me.radSchedulerDemo.Appointments.BeginUpdate()
			Dim appointment As Appointment = Nothing

			appointment = New Appointment(baseDate.AddHours(11.0), baseDate.AddHours(12.0), "Wash the car", "", "Garage")
			appointment.RecurrenceRule = New DailyRecurrenceRule(baseDate.AddHours(11.0), 2)
			Me.radSchedulerDemo.Appointments.Add(appointment)
			Me.radSchedulerDemo.Appointments.EndUpdate()

			Dim colors As Color() = New Color() {Color.LightBlue, Color.LightGreen, Color.LightYellow, Color.Red, Color.Orange, Color.Pink, _
				Color.Purple, Color.Peru, Color.PowderBlue}

			Dim names As String() = New String() {"Alan Smith", "Anne Dodsworth", "Boyan Mastoni", "Richard Duncan", "Maria Shnaider"}

			For i As Integer = 0 To names.Length - 1
				Dim resource As New Resource()
				resource.Id = New EventId(i)
				resource.Name = names(i)
				resource.Color = colors(i)

				resource.Image = Me.imageList1.Images(i)
				Me.radSchedulerDemo.Resources.Add(resource)
			Next

			Me.radSchedulerDemo.GetDayView().ResourcesPerView = 2
			Me.radSchedulerDemo.GroupType = GroupType.Resource
			Dim headerElement As SchedulerDayViewGroupedByResourceElement = TryCast(Me.radSchedulerDemo.SchedulerElement.ViewElement, SchedulerDayViewGroupedByResourceElement)
			headerElement.ResourceHeaderHeight = 135

			For i As Integer = 0 To summaries.Length - 1
				appointment = New Appointment(start(i), [end](i), summaries(i), descriptions(i), locations(i))
				appointment.ResourceId = Me.radSchedulerDemo.Resources(0).Id
				appointment.BackgroundId = CInt(backgrounds(i))
				Me.radSchedulerDemo.Appointments.Add(appointment)
			Next

			Dim dayView As SchedulerDayViewGroupedByResourceElement = TryCast(Me.radSchedulerDemo.SchedulerElement.ViewElement, SchedulerDayViewGroupedByResourceElement)
			dayView.ScrollToWorkHours()

			Me.radSchedulerDemo.SchedulerElement.SetResourceHeaderAngleTransform(SchedulerViewType.Timeline, 0)

            Me.radSchedulerNavigator1.AssociatedScheduler = Me.radSchedulerDemo
            Me.radSchedulerDemo.SelectionBehavior.SelectDateRange(DateTime.Now.[Date].AddHours(7), DateTime.Now.[Date].AddHours(8), Me.radSchedulerDemo.Resources(0).Id)

		End Sub

		Private Sub radSchedulerDemo_MouseDown(sender As Object, e As MouseEventArgs)
			If Me.isSwitchedMonthToWeek Then
				InvalidateScheduler()
				Me.isSwitchedMonthToWeek = False
			End If
		End Sub

		Private Sub radSchedulerDemo_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
			InvalidateScheduler()
		End Sub

		Private Sub radSchedulerDemo_ActiveViewChanged(sender As Object, e As SchedulerViewChangedEventArgs)
			If e.OldView.ViewType <> e.NewView.ViewType Then
				Select Case e.NewView.ViewType
					Case SchedulerViewType.Month
						Me.radSchedulerDemo.GetMonthView().WeekCount = 5
						Exit Select
					Case SchedulerViewType.Day
						Me.radSchedulerDemo.GetDayView().DayCount = 1
						Exit Select
				End Select
			End If

			If e.OldView.ViewType = SchedulerViewType.Month AndAlso e.NewView.ViewType = SchedulerViewType.Week Then
				Me.isSwitchedMonthToWeek = True
			End If

			InvalidateScheduler()
		End Sub

		Private isSwitchedMonthToWeek As Boolean = False

		Private Sub InvalidateScheduler()
			Dim dayView As SchedulerDayViewGroupedByResourceElement = TryCast(Me.radSchedulerDemo.SchedulerElement.ViewElement, SchedulerDayViewGroupedByResourceElement)
			Dim monthView As SchedulerMonthViewGroupedByResourceElement = TryCast(Me.radSchedulerDemo.SchedulerElement.ViewElement, SchedulerMonthViewGroupedByResourceElement)
			Dim timelineElement As TimelineGroupingByResourcesElement = TryCast(Me.radSchedulerDemo.SchedulerElement.ViewElement, TimelineGroupingByResourcesElement)

			If dayView Is Nothing AndAlso monthView Is Nothing AndAlso timelineElement Is Nothing Then
				Return
			End If

			Dim headerHeight As Integer = 135

			If dayView IsNot Nothing Then
				dayView.ResourceHeaderHeight = headerHeight
				dayView.InvalidateMeasure(True)
			ElseIf monthView IsNot Nothing Then
				monthView.ResourceHeaderHeight = headerHeight
			ElseIf timelineElement IsNot Nothing Then
				timelineElement.ResourceHeaderWidth = headerHeight
			End If

			Me.radSchedulerDemo.PerformLayout()
		End Sub

		Private Sub radButton1_Click(sender As Object, e As EventArgs)
			Me.radSchedulerDemo.GetDayView().DayCount = 1
			Me.radSchedulerDemo.GetDayView().StartDate = Me.radSchedulerDemo.GetDayView().StartDate.AddDays(1)
		End Sub

        Protected Overrides Sub WireEvents()
            AddHandler Me.radSchedulerDemo.ActiveViewChanged, AddressOf radSchedulerDemo_ActiveViewChanged
            AddHandler Me.radSchedulerDemo.PropertyChanged, AddressOf radSchedulerDemo_PropertyChanged
            AddHandler Me.radSchedulerDemo.MouseDown, AddressOf radSchedulerDemo_MouseDown
            AddHandler Me.radSchedulerDemo.CellSelectionChanged, AddressOf Me.radSchedulerDemo_CellSelectionChanged
            AddHandler Me.radTrackBar1.ValueChanged, AddressOf Me.radTrackBar1_ValueChanged
        End Sub

		Private suspendTrackBar As Boolean = False
		Private Sub radTrackBar1_ValueChanged(sender As Object, e As EventArgs)
			If radSchedulerDemo.SelectionBehavior.CurrentCellElement IsNot Nothing Then
				Dim viewGroupedByResource As SchedulerViewGroupedByResourceElementBase = TryCast(radSchedulerDemo.ViewElement, SchedulerViewGroupedByResourceElementBase)
				Dim resourceId As Integer = GetResourceIndex(viewGroupedByResource)

				If resourceId >= 0 AndAlso Not suspendTrackBar Then
					viewGroupedByResource.SetResourceSize(resourceId, Me.radTrackBar1.Value)
				End If
			End If
		End Sub

		Private Function GetResourceIndex(viewGroupedByResource As SchedulerViewGroupedByResourceElementBase) As Integer
			Dim cellResource As IResource = radSchedulerDemo.SelectionBehavior.CurrentCellElement.View.GetResource()
			Dim cellResourceIndex As Integer = radSchedulerDemo.Resources.IndexOf(cellResource)

			Dim resourceId As Integer = cellResourceIndex - viewGroupedByResource.ResourceStartIndex
			Return resourceId
		End Function

		Private Sub radSchedulerDemo_CellSelectionChanged(sender As Object, e As EventArgs)
			suspendTrackBar = True
			Dim viewGroupedByResource As SchedulerViewGroupedByResourceElementBase = TryCast(radSchedulerDemo.ViewElement, SchedulerViewGroupedByResourceElementBase)
			Dim resourceId As Integer = GetResourceIndex(viewGroupedByResource)
			Me.radTrackBar1.Value = viewGroupedByResource.GetResourceSize(resourceId)
			suspendTrackBar = False
		End Sub

		Private Sub radTrackBar1_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs)
			e.ToolTipText = "Resize current resource"
		End Sub
	End Class
End Namespace