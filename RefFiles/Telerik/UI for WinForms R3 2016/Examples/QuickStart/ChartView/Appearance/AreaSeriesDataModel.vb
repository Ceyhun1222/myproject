Namespace Telerik.Examples.WinControls.ChartView.Appearance
	Public Class AreaSeriesDataModel
        Private m_s1 As IEnumerable(Of LineAreaSeriesData)
        Private m_s2 As IEnumerable(Of LineAreaSeriesData)
        Private m_s3 As IEnumerable(Of LineAreaSeriesData)
        Private m_s4 As IEnumerable(Of LineAreaSeriesData)
        Private m_s5 As IEnumerable(Of LineAreaSeriesData)
        Private m_s6 As IEnumerable(Of LineAreaSeriesData)
        Private m_s7 As IEnumerable(Of LineAreaSeriesData)
        Private m_s8 As IEnumerable(Of LineAreaSeriesData)

        Public ReadOnly Property S1() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s1 Is Nothing Then
                    Me.m_s1 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(50, "May"), New LineAreaSeriesData(40, "Jun"), New LineAreaSeriesData(80, "Jul"), New LineAreaSeriesData(30, "Aug"), New LineAreaSeriesData(50, "Sep"), New LineAreaSeriesData(60, "Oct"), New LineAreaSeriesData(90, "Nov"), New LineAreaSeriesData(60, "Dec"), New LineAreaSeriesData(50, "Jan")})
                End If

                Return Me.m_s1
            End Get
        End Property

        Public ReadOnly Property S2() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s2 Is Nothing Then
                    Me.m_s2 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(150, "May"), New LineAreaSeriesData(90, "Jun"), New LineAreaSeriesData(140, "Jul"), New LineAreaSeriesData(140, "Aug"), New LineAreaSeriesData(160, "Sep"), New LineAreaSeriesData(130, "Oct"), New LineAreaSeriesData(100, "Nov"), New LineAreaSeriesData(140, "Dec"), New LineAreaSeriesData(130, "Jan")})
                End If

                Return Me.m_s2
            End Get
        End Property

        Public ReadOnly Property S3() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s3 Is Nothing Then
                    Me.m_s3 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(180, "May"), New LineAreaSeriesData(200, "Jun"), New LineAreaSeriesData(190, "Jul"), New LineAreaSeriesData(170, "Aug"), New LineAreaSeriesData(220, "Sep"), New LineAreaSeriesData(180, "Oct"), New LineAreaSeriesData(200, "Nov"), New LineAreaSeriesData(150, "Dec"), New LineAreaSeriesData(160, "Jan")})
                End If

                Return Me.m_s3
            End Get
        End Property

        Public ReadOnly Property S4() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s4 Is Nothing Then
                    Me.m_s4 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(210, "May"), New LineAreaSeriesData(230, "Jun"), New LineAreaSeriesData(220, "Jul"), New LineAreaSeriesData(270, "Aug"), New LineAreaSeriesData(270, "Sep"), New LineAreaSeriesData(250, "Oct"), New LineAreaSeriesData(230, "Nov"), New LineAreaSeriesData(230, "Dec"), New LineAreaSeriesData(200, "Jan")})
                End If

                Return Me.m_s4
            End Get
        End Property

        Public ReadOnly Property S5() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s5 Is Nothing Then
                    Me.m_s5 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(310, "May"), New LineAreaSeriesData(300, "Jun"), New LineAreaSeriesData(320, "Jul"), New LineAreaSeriesData(350, "Aug"), New LineAreaSeriesData(320, "Sep"), New LineAreaSeriesData(320, "Oct"), New LineAreaSeriesData(300, "Nov"), New LineAreaSeriesData(330, "Dec"), New LineAreaSeriesData(310, "Jan")})
                End If

                Return Me.m_s5
            End Get
        End Property

        Public ReadOnly Property S6() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s6 Is Nothing Then
                    Me.m_s6 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(360, "May"), New LineAreaSeriesData(420, "Jun"), New LineAreaSeriesData(370, "Jul"), New LineAreaSeriesData(370, "Aug"), New LineAreaSeriesData(370, "Sep"), New LineAreaSeriesData(360, "Oct"), New LineAreaSeriesData(350, "Nov"), New LineAreaSeriesData(400, "Dec"), New LineAreaSeriesData(380, "Jan")})
                End If

                Return Me.m_s6
            End Get
        End Property

        Public ReadOnly Property S7() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s7 Is Nothing Then
                    Me.m_s7 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(450, "May"), New LineAreaSeriesData(440, "Jun"), New LineAreaSeriesData(450, "Jul"), New LineAreaSeriesData(440, "Aug"), New LineAreaSeriesData(430, "Sep"), New LineAreaSeriesData(380, "Oct"), New LineAreaSeriesData(400, "Nov"), New LineAreaSeriesData(420, "Dec"), New LineAreaSeriesData(430, "Jan")})
                End If

                Return Me.m_s7
            End Get
        End Property

        Public ReadOnly Property S8() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s8 Is Nothing Then
                    Me.m_s8 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(480, "May"), New LineAreaSeriesData(470, "Jun"), New LineAreaSeriesData(470, "Jul"), New LineAreaSeriesData(500, "Aug"), New LineAreaSeriesData(480, "Sep"), New LineAreaSeriesData(480, "Oct"), New LineAreaSeriesData(430, "Nov"), New LineAreaSeriesData(450, "Dec"), New LineAreaSeriesData(480, "Jan")})
                End If

                Return Me.m_s8
            End Get
        End Property

		Public Function GetData(ByVal index As Integer) As IEnumerable(Of LineAreaSeriesData)
			If index = 0 Then
				Return Me.S1
			End If

			If index = 1 Then
				Return Me.S2
			End If

			If index = 2 Then
				Return Me.S3
			End If

			If index = 3 Then
				Return Me.S4
			End If

			If index = 4 Then
				Return Me.S5
			End If

			If index = 5 Then
				Return Me.S6
			End If

			If index = 6 Then
				Return Me.S7
			End If

			If index = 7 Then
				Return Me.S8
			End If

			Return Nothing
		End Function
	End Class
End Namespace
