Imports System.Data

Namespace Telerik.Examples.WinControls.ChartView.ChartTypes.Bubble
    Public NotInheritable Class DataModel

        Private Sub New()
        End Sub

        Public Shared Function GetData() As DataSet
            Dim result As New DataSet()

            Dim tableEurope As New DataTable("Europe")
            Dim tableNorthAmerica As New DataTable("NorthAmerica")
            Dim tableMiddleEast As New DataTable("MiddleEast")

            tableEurope.Columns.Add("Country", GetType(String))
            tableEurope.Columns.Add("LifeExpectancy", GetType(Double))
            tableEurope.Columns.Add("FertilityRate", GetType(Double))
            tableEurope.Columns.Add("Population", GetType(Integer))

            tableNorthAmerica.Columns.Add("Country", GetType(String))
            tableNorthAmerica.Columns.Add("LifeExpectancy", GetType(Double))
            tableNorthAmerica.Columns.Add("FertilityRate", GetType(Double))
            tableNorthAmerica.Columns.Add("Population", GetType(Integer))

            tableMiddleEast.Columns.Add("Country", GetType(String))
            tableMiddleEast.Columns.Add("LifeExpectancy", GetType(Double))
            tableMiddleEast.Columns.Add("FertilityRate", GetType(Double))
            tableMiddleEast.Columns.Add("Population", GetType(Integer))

            tableEurope.Rows.Add("Russia", 68.6, 1.54, 141850000)
            tableEurope.Rows.Add("Danmark", 78.6, 1.84, 5523095)
            tableEurope.Rows.Add("Great Britain", 80.05, 2.0, 61801570)
            tableEurope.Rows.Add("Germany", 79.84, 1.36, 81902307)

            tableNorthAmerica.Rows.Add("USA", 78.09, 2.05, 307007000)
            tableNorthAmerica.Rows.Add("Canada", 80.66, 1.67, 33739900)

            tableMiddleEast.Rows.Add("Iraq", 68.09, 4.77, 31090763)
            tableMiddleEast.Rows.Add("Egypt", 72.73, 2.78, 79716203)
            tableMiddleEast.Rows.Add("Iran", 72.49, 1.7, 73137148)
            tableMiddleEast.Rows.Add("Israel", 81.55, 2.96, 7485600)

            result.Tables.Add(tableEurope)
            result.Tables.Add(tableNorthAmerica)
            result.Tables.Add(tableMiddleEast)

            Return result
        End Function
    End Class
End Namespace