Option Strict Off
Option Explicit On
Module LogModule
	
	Private LogFileNum As Integer
	
	Function CreateLog(ByRef FormName As String) As String
		Dim I As Integer
		Dim L As Integer
		Dim Pos As Integer
		Dim ProjectPath As String
		Dim FileName As String
		
		On Error GoTo EH
		
		ProjectPath = GetMapFileName()

		Pos = InStrRev(ProjectPath, ".")

		If Pos <> 0 Then
			FileName = Left(ProjectPath, Pos) + "log"
		Else
			FileName = ProjectPath + ".log"
		End If
		
		LogFileNum = FreeFile
		'If fileexist Then
		FileOpen(LogFileNum, FileName, OpenMode.Append, OpenAccess.Write, OpenShare.LockReadWrite)
		
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "Date:     " & CStr(Today) & " ; " & CStr(TimeOfDay))
		PrintLine(LogFileNum, FormName)
		'Print #LogFileNum, "Form:   " + FormName
		
		'MsgBox FileName
		
		CreateLog = FileName
		Exit Function
		
EH: 
		'if
		FileOpen(LogFileNum, FileName, OpenMode.Output, OpenAccess.Write, OpenShare.LockReadWrite)
		Resume Next
		
	End Function
	
	'Function FlushLog() As Boolean
	'Flush
	'Print #LogFileNum, Text
	'End Function
	
	Sub LogStr(ByRef Text As String)
		PrintLine(LogFileNum, Text)
	End Sub
	
	Sub LogLabel(ByRef pLabel As System.Windows.Forms.Label)
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "  Label.Name:       " & pLabel.Name)
		PrintLine(LogFileNum, "    Label.Caption:      " & pLabel.Text)
		PrintLine(LogFileNum, "    Label.Enabled:      " & CStr(pLabel.Enabled))
	End Sub
	
	Sub LogCheckBox(ByRef pCheckBox As System.Windows.Forms.CheckBox)
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "  CheckBox.Name:    " & pCheckBox.Name)
		PrintLine(LogFileNum, "    CheckBox.Caption:   " & pCheckBox.Text)
		PrintLine(LogFileNum, "    CheckBox.Value:     " & CStr(pCheckBox.CheckState))
		PrintLine(LogFileNum, "    CheckBox.Enabled:   " & CStr(pCheckBox.Enabled))
	End Sub
	
	Sub LogOptionButton(ByRef pOptionButton As System.Windows.Forms.RadioButton)
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "  OptionButton.Name:    " & pOptionButton.Name)
		PrintLine(LogFileNum, "    OptionButton.Caption:   " & pOptionButton.Text)
		PrintLine(LogFileNum, "    OptionButton.Value:     " & CStr(pOptionButton.Checked))
		PrintLine(LogFileNum, "    OptionButton.Enabled:   " & CStr(pOptionButton.Enabled))
	End Sub
	
	Sub LogTextBox(ByRef pTextBox As System.Windows.Forms.TextBox)
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "  TextBox.Name:     " & pTextBox.Name)
		PrintLine(LogFileNum, "    TextBox.Text:       " & pTextBox.Text)
		PrintLine(LogFileNum, "    TextBox.Enabled:    " & CStr(pTextBox.Enabled))
		PrintLine(LogFileNum, "    TextBox.Locked:     " & CStr(pTextBox.ReadOnly))
	End Sub
	
	Sub LogCombo(ByRef pComboBox As System.Windows.Forms.ComboBox)
		PrintLine(LogFileNum, "")
		PrintLine(LogFileNum, "  ComboBox.Name:    " & pComboBox.Name)
		PrintLine(LogFileNum, "    ComboBox.Text:      " & pComboBox.Text)
		PrintLine(LogFileNum, "    ComboBox.Enabled:   " & CStr(pComboBox.Enabled))

		'PrintLine(LogFileNum, "    ComboBox.Locked:    " + CStr(pComboBox.Locked))
	End Sub
	
	Function CloseLog() As Boolean
		FileClose(LogFileNum)
		CloseLog = True
	End Function
End Module