Namespace Telerik.Examples.WinControls.ChartView.Printing
    Public Class LineSeriesDataModel
        Private m_s1 As IEnumerable(Of LineAreaSeriesData)
        Private m_s2 As IEnumerable(Of LineAreaSeriesData)
        Private m_s3 As IEnumerable(Of LineAreaSeriesData)
        Private m_s4 As IEnumerable(Of LineAreaSeriesData)
        Private m_s5 As IEnumerable(Of LineAreaSeriesData)
        Private m_s6 As IEnumerable(Of LineAreaSeriesData)
        Private m_s7 As IEnumerable(Of LineAreaSeriesData)
        Private m_s8 As IEnumerable(Of LineAreaSeriesData)
        Private m_s9 As IEnumerable(Of LineAreaSeriesData)
        Private m_s10 As IEnumerable(Of LineAreaSeriesData)
        Private m_s11 As IEnumerable(Of LineAreaSeriesData)
        Private m_s12 As IEnumerable(Of LineAreaSeriesData)

        Public ReadOnly Property S1() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s1 Is Nothing Then
                    Me.m_s1 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(30, "May"), _
                        New LineAreaSeriesData(20, "Jun"), _
                        New LineAreaSeriesData(60, "Jul"), _
                        New LineAreaSeriesData(110, "Aug"), _
                        New LineAreaSeriesData(150, "Sep"), _
                        New LineAreaSeriesData(200, "Oct"), _
                        New LineAreaSeriesData(160, "Nov"), _
                        New LineAreaSeriesData(150, "Dec"), _
                        New LineAreaSeriesData(100, "Jan") _
                    }
                End If

                Return Me.m_s1
            End Get
        End Property

        Public ReadOnly Property S2() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s2 Is Nothing Then
                    Me.m_s2 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(70, "May"), _
                        New LineAreaSeriesData(60, "Jun"), _
                        New LineAreaSeriesData(100, "Jul"), _
                        New LineAreaSeriesData(50, "Aug"), _
                        New LineAreaSeriesData(60, "Sep"), _
                        New LineAreaSeriesData(80, "Oct"), _
                        New LineAreaSeriesData(100, "Nov"), _
                        New LineAreaSeriesData(80, "Dec"), _
                        New LineAreaSeriesData(50, "Jan") _
                    }
                End If

                Return Me.m_s2
            End Get
        End Property

        Public ReadOnly Property S3() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s3 Is Nothing Then
                    Me.m_s3 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(150, "May"), _
                        New LineAreaSeriesData(170, "Jun"), _
                        New LineAreaSeriesData(160, "Jul"), _
                        New LineAreaSeriesData(110, "Aug"), _
                        New LineAreaSeriesData(200, "Sep"), _
                        New LineAreaSeriesData(130, "Oct"), _
                        New LineAreaSeriesData(210, "Nov"), _
                        New LineAreaSeriesData(210, "Dec"), _
                        New LineAreaSeriesData(210, "Jan") _
                    }
                End If

                Return Me.m_s3
            End Get
        End Property

        Public ReadOnly Property S4() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s4 Is Nothing Then
                    Me.m_s4 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(190, "May"), _
                        New LineAreaSeriesData(210, "Jun"), _
                        New LineAreaSeriesData(200, "Jul"), _
                        New LineAreaSeriesData(230, "Aug"), _
                        New LineAreaSeriesData(230, "Sep"), _
                        New LineAreaSeriesData(300, "Oct"), _
                        New LineAreaSeriesData(290, "Nov"), _
                        New LineAreaSeriesData(280, "Dec"), _
                        New LineAreaSeriesData(270, "Jan") _
                    }
                End If

                Return Me.m_s4
            End Get
        End Property

        Public ReadOnly Property S5() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s5 Is Nothing Then
                    Me.m_s5 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(280, "May"), _
                        New LineAreaSeriesData(260, "Jun"), _
                        New LineAreaSeriesData(270, "Jul"), _
                        New LineAreaSeriesData(310, "Aug"), _
                        New LineAreaSeriesData(270, "Sep"), _
                        New LineAreaSeriesData(260, "Oct"), _
                        New LineAreaSeriesData(230, "Nov"), _
                        New LineAreaSeriesData(250, "Dec"), _
                        New LineAreaSeriesData(260, "Jan") _
                    }
                End If

                Return Me.m_s5
            End Get
        End Property

        Public ReadOnly Property S6() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s6 Is Nothing Then
                    Me.m_s6 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(330, "May"), _
                        New LineAreaSeriesData(320, "Jun"), _
                        New LineAreaSeriesData(380, "Jul"), _
                        New LineAreaSeriesData(330, "Aug"), _
                        New LineAreaSeriesData(330, "Sep"), _
                        New LineAreaSeriesData(330, "Oct"), _
                        New LineAreaSeriesData(350, "Nov"), _
                        New LineAreaSeriesData(340, "Dec"), _
                        New LineAreaSeriesData(380, "Jan") _
                    }
                End If

                Return Me.m_s6
            End Get
        End Property

        Public ReadOnly Property S7() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s7 Is Nothing Then
                    Me.m_s7 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(420, "May"), _
                        New LineAreaSeriesData(410, "Jun"), _
                        New LineAreaSeriesData(420, "Jul"), _
                        New LineAreaSeriesData(500, "Aug"), _
                        New LineAreaSeriesData(450, "Sep"), _
                        New LineAreaSeriesData(440, "Oct"), _
                        New LineAreaSeriesData(380, "Nov"), _
                        New LineAreaSeriesData(420, "Dec"), _
                        New LineAreaSeriesData(450, "Jan") _
                    }
                End If

                Return Me.m_s7
            End Get
        End Property

        Public ReadOnly Property S8() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s8 Is Nothing Then
                    Me.m_s8 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(450, "May"), _
                        New LineAreaSeriesData(440, "Jun"), _
                        New LineAreaSeriesData(440, "Jul"), _
                        New LineAreaSeriesData(430, "Aug"), _
                        New LineAreaSeriesData(420, "Sep"), _
                        New LineAreaSeriesData(460, "Oct"), _
                        New LineAreaSeriesData(490, "Nov"), _
                        New LineAreaSeriesData(460, "Dec"), _
                        New LineAreaSeriesData(420, "Jan") _
                    }
                End If

                Return Me.m_s8
            End Get
        End Property

        Public ReadOnly Property S9() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s9 Is Nothing Then
                    Me.m_s9 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(240, "May"), _
                        New LineAreaSeriesData(220, "Jun"), _
                        New LineAreaSeriesData(260, "Jul"), _
                        New LineAreaSeriesData(280, "Aug"), _
                        New LineAreaSeriesData(220, "Sep"), _
                        New LineAreaSeriesData(230, "Oct"), _
                        New LineAreaSeriesData(250, "Nov"), _
                        New LineAreaSeriesData(210, "Dec"), _
                        New LineAreaSeriesData(250, "Jan") _
                    }
                End If

                Return Me.m_s9
            End Get
        End Property

        Public ReadOnly Property S10() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s10 Is Nothing Then
                    Me.m_s10 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(250, "May"), _
                        New LineAreaSeriesData(240, "Jun"), _
                        New LineAreaSeriesData(260, "Jul"), _
                        New LineAreaSeriesData(290, "Aug"), _
                        New LineAreaSeriesData(320, "Sep"), _
                        New LineAreaSeriesData(360, "Oct"), _
                        New LineAreaSeriesData(350, "Nov"), _
                        New LineAreaSeriesData(320, "Dec"), _
                        New LineAreaSeriesData(330, "Jan") _
                    }
                End If

                Return Me.m_s10
            End Get
        End Property

        Public ReadOnly Property S11() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s11 Is Nothing Then
                    Me.m_s11 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(130, "May"), _
                        New LineAreaSeriesData(160, "Jun"), _
                        New LineAreaSeriesData(170, "Jul"), _
                        New LineAreaSeriesData(190, "Aug"), _
                        New LineAreaSeriesData(100, "Sep"), _
                        New LineAreaSeriesData(60, "Oct"), _
                        New LineAreaSeriesData(90, "Nov"), _
                        New LineAreaSeriesData(140, "Dec"), _
                        New LineAreaSeriesData(110, "Jan") _
                    }
                End If

                Return Me.m_s11
            End Get
        End Property

        Public ReadOnly Property S12() As IEnumerable(Of LineAreaSeriesData)
            Get
                If Me.m_s12 Is Nothing Then
                    Me.m_s12 = New List(Of LineAreaSeriesData)() From { _
                        New LineAreaSeriesData(330, "May"), _
                        New LineAreaSeriesData(360, "Jun"), _
                        New LineAreaSeriesData(320, "Jul"), _
                        New LineAreaSeriesData(370, "Aug"), _
                        New LineAreaSeriesData(280, "Sep"), _
                        New LineAreaSeriesData(230, "Oct"), _
                        New LineAreaSeriesData(290, "Nov"), _
                        New LineAreaSeriesData(210, "Dec"), _
                        New LineAreaSeriesData(290, "Jan") _
                    }
                End If

                Return Me.m_s12
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

            If index = 8 Then
                Return Me.S9
            End If

            If index = 9 Then
                Return Me.S10
            End If

            If index = 10 Then
                Return Me.S11
            End If

            If index = 11 Then
                Return Me.S12
            End If

            Return Nothing
        End Function

        Public Function GetLegendText(index As Integer) As String
            Select Case index
                Case 0
                    Return "Bills"
                Case 1
                    Return "Car"
                Case 2
                    Return "Entertainment"
                Case 3
                    Return "Food & Drink"
                Case 4
                    Return "Gifts"
                Case 5
                    Return "Groceries"
                Case 6
                    Return "Hobbies"
                Case 7
                    Return "Rent"
                Case 8
                    Return "Savings"
                Case 9
                    Return "Shopping"
                Case 10
                    Return "Travel"
                Case 11
                    Return "Utilities"
                Case Else
                    Return "Missing product ID"
            End Select
        End Function
    End Class
End Namespace
