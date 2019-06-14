Namespace Telerik.Examples.WinControls.ChartView.Appearance
	Public Class LineSeriesDataModel
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
                    Me.m_s1 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(30, "May"), New LineAreaSeriesData(20, "Jun"), New LineAreaSeriesData(60, "Jul"), New LineAreaSeriesData(110, "Aug"), New LineAreaSeriesData(150, "Sep"), New LineAreaSeriesData(200, "Oct"), New LineAreaSeriesData(160, "Nov"), New LineAreaSeriesData(150, "Dec"), New LineAreaSeriesData(100, "Jan")})
                End If

                Return Me.m_s1
            End Get
        End Property

        Public ReadOnly Property S2() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s2 Is Nothing Then
                    Me.m_s2 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(70, "May"), New LineAreaSeriesData(60, "Jun"), New LineAreaSeriesData(100, "Jul"), New LineAreaSeriesData(50, "Aug"), New LineAreaSeriesData(60, "Sep"), New LineAreaSeriesData(80, "Oct"), New LineAreaSeriesData(100, "Nov"), New LineAreaSeriesData(80, "Dec"), New LineAreaSeriesData(50, "Jan")})
                End If

                Return Me.m_s2
            End Get
        End Property

        Public ReadOnly Property S3() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s3 Is Nothing Then
                    Me.m_s3 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(150, "May"), New LineAreaSeriesData(170, "Jun"), New LineAreaSeriesData(160, "Jul"), New LineAreaSeriesData(110, "Aug"), New LineAreaSeriesData(200, "Sep"), New LineAreaSeriesData(130, "Oct"), New LineAreaSeriesData(210, "Nov"), New LineAreaSeriesData(210, "Dec"), New LineAreaSeriesData(210, "Jan")})
                End If

                Return Me.m_s3
            End Get
        End Property

        Public ReadOnly Property S4() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s4 Is Nothing Then
                    Me.m_s4 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(190, "May"), New LineAreaSeriesData(210, "Jun"), New LineAreaSeriesData(200, "Jul"), New LineAreaSeriesData(230, "Aug"), New LineAreaSeriesData(230, "Sep"), New LineAreaSeriesData(300, "Oct"), New LineAreaSeriesData(290, "Nov"), New LineAreaSeriesData(280, "Dec"), New LineAreaSeriesData(270, "Jan")})
                End If

                Return Me.m_s4
            End Get
        End Property

        Public ReadOnly Property S5() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s5 Is Nothing Then
                    Me.m_s5 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(280, "May"), New LineAreaSeriesData(260, "Jun"), New LineAreaSeriesData(270, "Jul"), New LineAreaSeriesData(310, "Aug"), New LineAreaSeriesData(270, "Sep"), New LineAreaSeriesData(260, "Oct"), New LineAreaSeriesData(230, "Nov"), New LineAreaSeriesData(250, "Dec"), New LineAreaSeriesData(260, "Jan")})
                End If

                Return Me.m_s5
            End Get
        End Property

        Public ReadOnly Property S6() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s6 Is Nothing Then
                    Me.m_s6 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(330, "May"), New LineAreaSeriesData(320, "Jun"), New LineAreaSeriesData(380, "Jul"), New LineAreaSeriesData(330, "Aug"), New LineAreaSeriesData(330, "Sep"), New LineAreaSeriesData(330, "Oct"), New LineAreaSeriesData(350, "Nov"), New LineAreaSeriesData(340, "Dec"), New LineAreaSeriesData(380, "Jan")})
                End If

                Return Me.m_s6
            End Get
        End Property

        Public ReadOnly Property S7() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s7 Is Nothing Then
                    Me.m_s7 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(420, "May"), New LineAreaSeriesData(410, "Jun"), New LineAreaSeriesData(420, "Jul"), New LineAreaSeriesData(500, "Aug"), New LineAreaSeriesData(450, "Sep"), New LineAreaSeriesData(440, "Oct"), New LineAreaSeriesData(380, "Nov"), New LineAreaSeriesData(420, "Dec"), New LineAreaSeriesData(450, "Jan")})
                End If

                Return Me.m_s7
            End Get
        End Property

        Public ReadOnly Property S8() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s8 Is Nothing Then
                    Me.m_s8 = New List(Of LineAreaSeriesData)(New LineAreaSeriesData() {New LineAreaSeriesData(450, "May"), New LineAreaSeriesData(440, "Jun"), New LineAreaSeriesData(440, "Jul"), New LineAreaSeriesData(430, "Aug"), New LineAreaSeriesData(420, "Sep"), New LineAreaSeriesData(460, "Oct"), New LineAreaSeriesData(490, "Nov"), New LineAreaSeriesData(460, "Dec"), New LineAreaSeriesData(420, "Jan")})
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
