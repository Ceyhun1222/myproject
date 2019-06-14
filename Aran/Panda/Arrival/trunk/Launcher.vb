Public Class Launcher

    Dim PrecisionFrm As CPrecisionFrm

    Public Function InitCommands() As Boolean
        Return InitCommand() > 0
    End Function


	Public Sub PrecArrival_Click(sender As Object, e As EventArgs)
		If CurrCmd = 2 Then Return
		CurrCmd = 0

		Try
			If InitCommands() Then
				If PrecisionFrm Is Nothing OrElse PrecisionFrm.IsDisposed Then
					PrecisionFrm = New CPrecisionFrm
				End If

				PrecisionFrm.Show(s_Win32Window)
				PrecisionFrm.ComboBox0001.Focus()
				CurrCmd = 2
			End If
		Catch
			Throw New Exception("Form initialization error.")
		End Try
	End Sub
End Class
