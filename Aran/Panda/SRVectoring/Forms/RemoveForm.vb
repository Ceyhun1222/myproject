Option Strict Off
Option Explicit On
Friend Class CRemoveForm
	Inherits System.Windows.Forms.Form

	Public Enum RemoveResult
		Cancel = 0
		Last = 1
		All = 2
	End Enum

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		' Get a handle to a copy of this form's system (window) menu
		Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
		' Add a separator
		AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
		' Add the About menu item
		AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "&About…")
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		MyBase.WndProc(m)

		' Test if the About item was selected from the system menu
		If (m.Msg = WM_SYSCOMMAND) AndAlso (CInt(m.WParam) = SYSMENU_ABOUT_ID) Then
			Dim about As AboutForm = New AboutForm()

			about.ShowDialog(Me)
			about = Nothing
		End If
	End Sub

	Public Result As RemoveResult

	Private Sub bCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles bCancel.Click
		CloseWnd(RemoveResult.Cancel)
	End Sub

	Private Sub bOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles bOK.Click

		If OptionButton1.Checked Then
			CloseWnd(RemoveResult.Last)
		Else
			CloseWnd(RemoveResult.All)
		End If

	End Sub

	Private Sub RemoveForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		Result = RemoveResult.Cancel
	End Sub

	Private Sub CloseWnd(ByVal Res As RemoveResult)
		Result = Res
		Me.Close()
	End Sub
End Class