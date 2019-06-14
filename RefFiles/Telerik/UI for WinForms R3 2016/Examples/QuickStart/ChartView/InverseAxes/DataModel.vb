Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data

Namespace Telerik.Examples.WinControls.ChartView.InverseAxes
	Public Class DataModel
		Public Function GetRawData() As DataSet
			Dim result As DataSet = New DataSet()

			Dim tableDryHoles As DataTable = New DataTable("DryHoles")
			Dim tableNaturalGas As DataTable = New DataTable("NaturalGas")
			Dim tableCrudeOil As DataTable = New DataTable("CrudeOil")

			tableDryHoles.Columns.Add("Time", GetType(DateTime))
			tableDryHoles.Columns.Add("Depth", GetType(Double))

			tableNaturalGas.Columns.Add("Time", GetType(DateTime))
			tableNaturalGas.Columns.Add("Depth", GetType(Double))

			tableCrudeOil.Columns.Add("Time", GetType(DateTime))
			tableCrudeOil.Columns.Add("Depth", GetType(Double))

			For Each data As WellDrillData In Me.GetDryHolesDrillData()
				tableDryHoles.Rows.Add(data.Date, data.Depth)
			Next data

			For Each data As WellDrillData In Me.GetNaturalGasDrillData()
				tableNaturalGas.Rows.Add(data.Date, data.Depth)
			Next data

			For Each data As WellDrillData In Me.GetCrudeOilDrillData()
				tableCrudeOil.Rows.Add(data.Date, data.Depth)
			Next data

			result.Tables.Add(tableDryHoles)
			result.Tables.Add(tableNaturalGas)
			result.Tables.Add(tableCrudeOil)

			Return result
		End Function

		Public Function GetAggregatedData() As DataTable
			Dim result As DataTable = New DataTable()

			result.Columns.Add("Name", GetType(String))
			result.Columns.Add("AverageDepth", GetType(Double))
			result.Columns.Add("MaximumDepth", GetType(Double))

			result.Rows.Add("Dry Holes", Average(Me.GetDryHolesDrillData()), Maximum(Me.GetDryHolesDrillData()))
			result.Rows.Add("Natural Gas", Average(Me.GetNaturalGasDrillData()), Maximum(Me.GetNaturalGasDrillData()))
			result.Rows.Add("Crude Oil", Average(Me.GetCrudeOilDrillData()), Maximum(Me.GetCrudeOilDrillData()))

			Return result
		End Function

		Private Function GetDryHolesDrillData() As List(Of WellDrillData)
            Dim result As New List(Of WellDrillData)()

            result.Add(New WellDrillData(New DateTime(1980, 6, 30), 4214.0))
            result.Add(New WellDrillData(New DateTime(1981, 6, 30), 4226.0))
            result.Add(New WellDrillData(New DateTime(1982, 6, 30), 4184.0))
            result.Add(New WellDrillData(New DateTime(1983, 6, 30), 3974.0))
            result.Add(New WellDrillData(New DateTime(1984, 6, 30), 4205.0))
            result.Add(New WellDrillData(New DateTime(1985, 6, 30), 4306.0))
            result.Add(New WellDrillData(New DateTime(1986, 6, 30), 4236.0))
            result.Add(New WellDrillData(New DateTime(1987, 6, 30), 4390.0))
            result.Add(New WellDrillData(New DateTime(1988, 6, 30), 4704.0))
            result.Add(New WellDrillData(New DateTime(1989, 6, 30), 4684.0))
            result.Add(New WellDrillData(New DateTime(1990, 6, 30), 4755.0))
            result.Add(New WellDrillData(New DateTime(1991, 6, 30), 4629.0))
            result.Add(New WellDrillData(New DateTime(1992, 6, 30), 4733.0))
            result.Add(New WellDrillData(New DateTime(1993, 6, 30), 4704.0))
            result.Add(New WellDrillData(New DateTime(1994, 6, 30), 5125.0))
            result.Add(New WellDrillData(New DateTime(1995, 6, 30), 5204.0))
            result.Add(New WellDrillData(New DateTime(1996, 6, 30), 5371.0))
            result.Add(New WellDrillData(New DateTime(1997, 6, 30), 5405.0))
            result.Add(New WellDrillData(New DateTime(1998, 6, 30), 5607.0))
            result.Add(New WellDrillData(New DateTime(1999, 6, 30), 5481.0))
            result.Add(New WellDrillData(New DateTime(2000, 6, 30), 5326.0))
            result.Add(New WellDrillData(New DateTime(2001, 6, 30), 5187.0))
            result.Add(New WellDrillData(New DateTime(2002, 6, 30), 5096.0))
            result.Add(New WellDrillData(New DateTime(2003, 6, 30), 5224.0))
            result.Add(New WellDrillData(New DateTime(2004, 6, 30), 5311.0))
            result.Add(New WellDrillData(New DateTime(2005, 6, 30), 4935.0))
            result.Add(New WellDrillData(New DateTime(2006, 6, 30), 4987.0))
            result.Add(New WellDrillData(New DateTime(2007, 6, 30), 5243.0))
            result.Add(New WellDrillData(New DateTime(2008, 6, 30), 5220.0))

            Return result
		End Function

		Private Function GetNaturalGasDrillData() As List(Of WellDrillData)
            Dim result As New List(Of WellDrillData)()

            result.Add(New WellDrillData(New DateTime(1980, 6, 30), 6604.0))
            result.Add(New WellDrillData(New DateTime(1981, 6, 30), 6772.0))
            result.Add(New WellDrillData(New DateTime(1982, 6, 30), 6921.0))
            result.Add(New WellDrillData(New DateTime(1983, 6, 30), 6395.0))
            result.Add(New WellDrillData(New DateTime(1984, 6, 30), 6502.0))
            result.Add(New WellDrillData(New DateTime(1985, 6, 30), 6787.0))
            result.Add(New WellDrillData(New DateTime(1986, 6, 30), 6777.0))
            result.Add(New WellDrillData(New DateTime(1987, 6, 30), 6698.0))
            result.Add(New WellDrillData(New DateTime(1988, 6, 30), 6683.0))
            result.Add(New WellDrillData(New DateTime(1989, 6, 30), 6606.0))
            result.Add(New WellDrillData(New DateTime(1990, 6, 30), 7100.0))
            result.Add(New WellDrillData(New DateTime(1991, 6, 30), 7122.0))
            result.Add(New WellDrillData(New DateTime(1992, 6, 30), 6907.0))
            result.Add(New WellDrillData(New DateTime(1993, 6, 30), 6482.0))
            result.Add(New WellDrillData(New DateTime(1994, 6, 30), 6564.0))
            result.Add(New WellDrillData(New DateTime(1995, 6, 30), 6264.0))
            result.Add(New WellDrillData(New DateTime(1996, 6, 30), 6773.0))
            result.Add(New WellDrillData(New DateTime(1997, 6, 30), 7188.0))
            result.Add(New WellDrillData(New DateTime(1998, 6, 30), 7230.0))
            result.Add(New WellDrillData(New DateTime(1999, 6, 30), 7015.0))
            result.Add(New WellDrillData(New DateTime(2000, 6, 30), 7347.0))
            result.Add(New WellDrillData(New DateTime(2001, 6, 30), 6990.0))
            result.Add(New WellDrillData(New DateTime(2002, 6, 30), 6796.0))
            result.Add(New WellDrillData(New DateTime(2003, 6, 30), 6589.0))
            result.Add(New WellDrillData(New DateTime(2004, 6, 30), 5948.0))
            result.Add(New WellDrillData(New DateTime(2005, 6, 30), 5732.0))
            result.Add(New WellDrillData(New DateTime(2006, 6, 30), 5770.0))
            result.Add(New WellDrillData(New DateTime(2007, 6, 30), 5901.0))
            result.Add(New WellDrillData(New DateTime(2008, 6, 30), 5899.0))

            Return result
        End Function

		Private Function GetCrudeOilDrillData() As List(Of WellDrillData)
            Dim result As New List(Of WellDrillData)()

            result.Add(New WellDrillData(New DateTime(1980, 6, 30), 3691.0))
            result.Add(New WellDrillData(New DateTime(1981, 6, 30), 3799.0))
            result.Add(New WellDrillData(New DateTime(1982, 6, 30), 3681.0))
            result.Add(New WellDrillData(New DateTime(1983, 6, 30), 3577.0))
            result.Add(New WellDrillData(New DateTime(1984, 6, 30), 3695.0))
            result.Add(New WellDrillData(New DateTime(1985, 6, 30), 3808.0))
            result.Add(New WellDrillData(New DateTime(1986, 6, 30), 3875.0))
            result.Add(New WellDrillData(New DateTime(1987, 6, 30), 3972.0))
            result.Add(New WellDrillData(New DateTime(1988, 6, 30), 4171.0))
            result.Add(New WellDrillData(New DateTime(1989, 6, 30), 4116.0))
            result.Add(New WellDrillData(New DateTime(1990, 6, 30), 4326.0))
            result.Add(New WellDrillData(New DateTime(1991, 6, 30), 4434.0))
            result.Add(New WellDrillData(New DateTime(1992, 6, 30), 4877.0))
            result.Add(New WellDrillData(New DateTime(1993, 6, 30), 4986.0))
            result.Add(New WellDrillData(New DateTime(1994, 6, 30), 5278.0))
            result.Add(New WellDrillData(New DateTime(1995, 6, 30), 4998.0))
            result.Add(New WellDrillData(New DateTime(1996, 6, 30), 4735.0))
            result.Add(New WellDrillData(New DateTime(1997, 6, 30), 4944.0))
            result.Add(New WellDrillData(New DateTime(1998, 6, 30), 4941.0))
            result.Add(New WellDrillData(New DateTime(1999, 6, 30), 4507.0))
            result.Add(New WellDrillData(New DateTime(2000, 6, 30), 4493.0))
            result.Add(New WellDrillData(New DateTime(2001, 6, 30), 4791.0))
            result.Add(New WellDrillData(New DateTime(2002, 6, 30), 4496.0))
            result.Add(New WellDrillData(New DateTime(2003, 6, 30), 4684.0))
            result.Add(New WellDrillData(New DateTime(2004, 6, 30), 4675.0))
            result.Add(New WellDrillData(New DateTime(2005, 6, 30), 4669.0))
            result.Add(New WellDrillData(New DateTime(2006, 6, 30), 4706.0))
            result.Add(New WellDrillData(New DateTime(2007, 6, 30), 4945.0))
            result.Add(New WellDrillData(New DateTime(2008, 6, 30), 4938.0))

            Return result
		End Function

		Private Function Average(ByVal wellData As List(Of WellDrillData)) As Double
			Dim sum As Double = 0R

			For Each well As WellDrillData In wellData
				sum += well.Depth
			Next well

			Return (sum / wellData.Count)
		End Function

		Private Function Maximum(ByVal wellData As List(Of WellDrillData)) As Double
			Dim max As Double = Double.MinValue

			For Each well As WellDrillData In wellData
				If max < well.Depth Then
					max = well.Depth
				End If
			Next well

			Return max
		End Function
	End Class
End Namespace
