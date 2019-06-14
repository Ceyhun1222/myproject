Imports System.Text
Imports System.ComponentModel

Namespace Telerik.Examples.WinControls.ChartView.LiveData
	Friend Class LiveDataModel
		Implements INotifyPropertyChanged

        Public _data As BindingList(Of ChartBusinessObject)
        Public _data2 As BindingList(Of ChartBusinessObject)
        Private _fps As String
        Private _messagesPerSecond As String
        Private _messagesPerMinute As String
        Private tickCountSecond As Integer
        Private tickCountMinute As Integer
        Private timer As Timer
        Private lastDate As Date
        Private random As New Random()
        Private tmpDateTime As DateTime = DateTime.MinValue
        Private count As Integer = 0


        Public Sub New()
            Me.timer = New Timer()
            Me.timer.Interval = 1
            AddHandler Me.timer.Tick, AddressOf OnTimer

            Me.FillData()
            Me.FillData2()
            Me.MessagesPerSecond = Me.random.Next(800, 1800).ToString("#,#")
            Me.MessagesPerMinute = Me.random.Next(50000, 55000).ToString("#,#")
        End Sub

        Public Property MessagesPerSecond() As String
            Get
                Return Me._messagesPerSecond
            End Get
            Set(ByVal value As String)
                If Me._messagesPerSecond <> value Then
                    Me._messagesPerSecond = value
                    Me.OnPropertyChanged("MessagesPerSecond")
                End If
            End Set
        End Property

        Public Property FPS As String
            Get
                Return Me._fps
            End Get
            Set(ByVal value As String)
                If Me._fps <> value Then
                    Me._fps = value
                    Me.OnPropertyChanged("FPS")
                End If
            End Set
        End Property

        Public Property MessagesPerMinute As String
            Get
                Return Me._messagesPerMinute
            End Get
            Set(ByVal value As String)
                If Me._messagesPerMinute <> value Then
                    Me._messagesPerMinute = value
                    Me.OnPropertyChanged("MessagesPerMinute")
                End If
            End Set
        End Property

        Public Property Data() As BindingList(Of ChartBusinessObject)
            Get
                Return Me._data
            End Get
            Set(ByVal value As BindingList(Of ChartBusinessObject))
                If Me._data IsNot value Then
                    Me._data = value
                    Me.OnPropertyChanged("Data")
                End If
            End Set
        End Property

        Public Property Data2() As BindingList(Of ChartBusinessObject)
            Get
                Return Me._data2
            End Get
            Set(ByVal value As BindingList(Of ChartBusinessObject))
                If Me._data2 IsNot value Then
                    Me._data2 = value
                    Me.OnPropertyChanged("Data2")
                End If
            End Set
        End Property

		Public Sub StartTimer()
			Me.timer.Start()
		End Sub

		Public Sub StopTimer()
			Me.timer.Stop()
		End Sub

		Public Sub UpdateTimer(ByVal interval As Double)
			Me.timer.Interval = CInt(Fix(interval))
		End Sub

		Private Sub FillData()
			Dim collection As New BindingList(Of ChartBusinessObject)()
			Me.lastDate = Date.Now

            For i As Integer = 0 To 50
                Me.lastDate = Me.lastDate.AddMilliseconds(200)
                collection.Add(Me.CreateBusinessObject())
            Next i

			Me.Data = collection
		End Sub

		Private Sub FillData2()
			Dim collection As New BindingList(Of ChartBusinessObject)()
			Dim [date] As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day)

			For i As Integer = 0 To 23
				collection.Add(Me.CreateBusinessObject2([date].AddHours(i)))
			Next i

			Me.Data2 = collection
		End Sub

        Private Sub OnTimer(ByVal sender As Object, ByVal e As EventArgs)

            Dim now As DateTime = DateTime.Now
            If (now - tmpDateTime).TotalSeconds > 1 Then
                tmpDateTime = now
                If count <> 0 Then
                    Me.FPS = (count / 1).ToString()
                End If

                count = 0
            Else
                count += 1
            End If

            Me.lastDate = Me.lastDate.AddMilliseconds(200)
            Me.Data.RemoveAt(0)
            Me.Data.Add(Me.CreateBusinessObject())
            Me.UpdateMetrics()
        End Sub

		Private Sub UpdateMetrics()
			Me.tickCountSecond += 1
			Me.tickCountMinute += 1

			If Me.tickCountSecond = 5 Then
				Me.tickCountSecond = 0
                Me.MessagesPerSecond = Me.random.Next(800, 1800).ToString("#,#")
			End If

            If Me.tickCountMinute = 100 Then
                Me.tickCountMinute = 0
                Me.MessagesPerMinute = Me.random.Next(50000, 55000).ToString("#,#")
            End If
		End Sub

		Private Function CreateBusinessObject() As ChartBusinessObject
			Dim obj As New ChartBusinessObject()

			obj.Value = Me.random.Next(800, 1800)
			obj.Category = Me.lastDate

			Return obj
		End Function

		Private Function CreateBusinessObject2(ByVal [date] As Date) As ChartBusinessObject
			Dim obj As New ChartBusinessObject()

			obj.Value = Me.random.Next(3300, 3800)
			obj.Category = [date]

			Return obj
		End Function

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
			If Me.PropertyChangedEvent IsNot Nothing Then
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
End Namespace
