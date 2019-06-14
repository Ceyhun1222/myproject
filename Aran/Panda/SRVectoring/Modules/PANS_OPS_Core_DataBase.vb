Option Strict Off
Option Explicit On
Module PANS_OPS_Core_DataBase
	
	Structure PANS_OPS_Core_Data
		Dim Name As String
		Dim Value As Double
		Dim Multiplier As Double
		Dim Unit As String
	End Structure
	
	'Public ErrorCode As Integer
	
	Public depWS As PANS_OPS_Core_Data
	Public arVisualWS As PANS_OPS_Core_Data
	Public fAirportISAtC As PANS_OPS_Core_Data
	
	Function InitModule() As Boolean
		Dim I As Integer
		Dim N As Integer
		Dim iParam_Name As Integer
		Dim iValue As Integer
		Dim iUnit As Integer
		Dim iMultiplier As Integer
		
		Dim Value As Double
		Dim Multiplier As Double
		Dim ParName As String
		Dim Unit As String

		Dim pTable As ESRI.ArcGIS.Geodatabase.ITable
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		On Error GoTo EH
		OpenTableFromFile(pTable, ConstDir + "\", "PANSCORE")
		
        iParam_Name = pTable.FindField("NAME")
        iValue = pTable.FindField("VALUE")
        iUnit = pTable.FindField("UNIT")
        iMultiplier = pTable.FindField("MULTIPLIER")
		
		N = pTable.RowCount(Nothing)
		
		For I = 0 To N - 1
			pRow = pTable.GetRow(I)
			ParName = pRow.Value(iParam_Name)
			Unit = pRow.Value(iUnit)
			Multiplier = pRow.Value(iMultiplier)
            Value = pRow.Value(iValue) * Multiplier

			If (Unit = "rad") Or (Unit = "°") Then Value = System.Math.Round(RadToDeg(Value), 2)

			Select Case ParName
				Case "depWS"
					depWS.Value = Value
					depWS.Name = ParName
					depWS.Unit = Unit
					depWS.Multiplier = Multiplier
				Case "arVisualWS"
					arVisualWS.Value = Value
					arVisualWS.Name = ParName
					arVisualWS.Unit = Unit
					arVisualWS.Multiplier = Multiplier
				Case "fAirportISAtC"
					fAirportISAtC.Value = Value
					fAirportISAtC.Name = ParName
					fAirportISAtC.Unit = Unit
					fAirportISAtC.Multiplier = Multiplier
			End Select
		Next I
		
		Return True
EH:
		MessageBox.Show(Err.Number + "  " + Err.Description)
		'ErrorCode = Err.Number
		Return False
	End Function
End Module