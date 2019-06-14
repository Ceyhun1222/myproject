Option Strict Off
Option Explicit On

Module Support
	Sub TextBoxInteger(ByRef Key As Char)
		If Key < " " Then Return
		If (Key < "0") Or (Key > "9") Then
			Key = Chr(0)
		End If
	End Sub

	'Sub TextBoxIntegerWithMinus(ByRef Key As Char, ByVal BoxText As String)
	'	If KeyAscii < 32 Then Return
	'	If ((KeyAscii < Asc("0") Or KeyAscii > Asc("9")) And KeyAscii <> Asc("-")) Then
	'		KeyAscii = 0
	'	ElseIf (KeyAscii = Asc("-")) Then
	'		If (BoxText <> "") Then
	'			KeyAscii = 0
	'		End If
	'	End If
	'End Sub

	Sub TextBoxFloat(ByRef Key As Char, ByVal BoxText As String)
		Dim N As Long
		Dim sDecSep As String

		sDecSep = Mid(CStr(1.1), 2, 1)

		If Key < " " Then Return

		If ((Key < "0") Or (Key > "9")) And (Key <> sDecSep) Then
			Key = Chr(0)
		Else
			If Key = sDecSep Then
				N = InStr(BoxText, sDecSep)
				If (N <> 0) Then Key = Chr(0)
			End If
		End If
	End Sub

	Sub TextBoxFloatWithMinus(ByRef Key As Char, ByVal BoxText As String)
		Dim N As Long
		Dim DecSep As Char

		DecSep = Mid(CStr(1.1), 2, 1)

		If Key < Chr(32) Then Return

		If (Key < "0" Or Key > "9") And Key <> DecSep And Key <> "-" Then
			Key = Chr(0)
		Else
			If Key = DecSep Then
				N = InStr(BoxText, DecSep)
				If N <> 0 Then Key = Chr(0)
			ElseIf Key = "-" Then
				If (BoxText <> "") Then Key = Chr(0)
			End If
		End If
	End Sub

	Sub TextBoxLimitCount(ByRef Key As Char, ByVal BoxText As String, ByVal N As Integer)
		If Key < Chr(32) Then Return
		If (Len(BoxText) > N - 1) Then Key = Chr(0)
	End Sub
End Module