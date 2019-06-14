Option Strict Off
Option Explicit On
Module RegistryFunctions

    'Public Function RegRead(ByVal HKey As Microsoft.Win32.RegistryKey, ByVal Key As String, ByVal ValueName As String, Optional ByVal defaultValue As Object = Nothing) As Object
    '	Dim regKey As Microsoft.Win32.RegistryKey
    '	Try
    '		regKey = HKey.OpenSubKey(Key, False)
    '		If Not (regKey Is Nothing) Then Return regKey.GetValue(ValueName)
    '	Catch ex As Exception

    '	End Try

    '	Return defaultValue
    'End Function

	' is not passed, 'rvString' is the default.				Optional ByVal ValueType As RegObj.RegValueType = RegObj.RegValueType.rvString
    'Public Function RegWrite(ByVal HKey As Microsoft.Win32.RegistryKey, ByVal Key As String, ByVal ValueName As String, ByVal Value As Object) As Integer
    '	Dim regKey As Microsoft.Win32.RegistryKey

    '	Try
    '		regKey = HKey.OpenSubKey(Key, True)
    '		If regKey Is Nothing Then regKey = HKey.CreateSubKey(Key)
    '		If regKey Is Nothing Then Return -1

    '		regKey.SetValue(ValueName, Value)
    '		Return 0
    '	Catch ex As Exception

    '	End Try

    '	Return -1
    'End Function

	' Delete a sub key or value name
	' ------------------------------
    'Public Function RegDelete(ByVal HKey As Microsoft.Win32.RegistryKey, ByVal SubKey As String, Optional ByVal ValueName As String = Nothing) As Integer
    '	Try
    '		If IsNothing(ValueName) Then
    '			' Remove the sub key and all its sub values
    '			HKey.DeleteSubKey(SubKey, True)
    '		Else
    '			' Remove the value name only
    '			Dim regKey As Microsoft.Win32.RegistryKey
    '			regKey = HKey.OpenSubKey(SubKey, True)
    '			regKey.DeleteValue(ValueName, True)
    '		End If

    '		Return 0
    '	Catch ex As Exception

    '	End Try

    '	Return -1
    'End Function
End Module