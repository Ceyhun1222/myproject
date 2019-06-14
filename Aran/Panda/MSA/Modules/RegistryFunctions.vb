Option Strict Off
Option Explicit On

Module RegistryFunctions

    'Public Function RegRead(ByVal HKey As Microsoft.Win32.RegistryKey, ByVal Key As String, ByVal ValueName As String, Optional ByVal defaultValue As Object = Nothing) As Object
    '	Dim regKey As Microsoft.Win32.RegistryKey
    '	Try
    '		'regKey = My.Computer.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
    '		'regKey = My.Computer.Registry.LocalMachine.OpenSubKey("Software\RISK", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
    '		'regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\RISK", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
    '		'regKey = HKey.OpenSubKey("SOFTWARE", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
    '		'regKey = regKey.OpenSubKey("RISK", Microsoft.Win32.RegistryKeyPermissionCheck.ReadSubTree)
    '		regKey = HKey.OpenSubKey(Key, Microsoft.Win32.RegistryKeyPermissionCheck.ReadSubTree)
    '		If Not (regKey Is Nothing) Then Return regKey.GetValue(ValueName)
    '	Catch ex As Exception

    '	End Try

    '	Return defaultValue
    'End Function

    'Public Function RegWrite(ByVal HKey As Microsoft.Win32.RegistryKey, ByVal Key As String, ByVal ValueName As String, ByVal Value As Object) As Integer
    '	Dim regKey As Microsoft.Win32.RegistryKey

    '	Try
    '		regKey = HKey.OpenSubKey(Key, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
    '		If regKey Is Nothing Then Return -1

    '		regKey.SetValue(ValueName, Value)
    '		Return 0
    '	Catch ex As Exception

    '	End Try

    '	Return -1
    'End Function
End Module
