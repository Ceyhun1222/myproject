﻿Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports Telerik.WinControls.UI
Imports Telerik.Windows.Documents.Spreadsheet.FormatProviders
Imports Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx
Imports Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv
Imports Telerik.Windows.Documents.Spreadsheet.Model
Imports Telerik.Windows.Documents.Spreadsheet.Utilities

Namespace SpreadGeneration
    Partial Public Class Form1
        Inherits RadForm
        Private products As New Products()
        Private m_exportFormats As IEnumerable(Of String)

        Private Shared ReadOnly IndexColumnName As Integer = 0
        Private Shared ReadOnly IndexColumnQuantity As Integer = 1
        Private Shared ReadOnly IndexColumnUnitPrice As Integer = 2
        Private Shared ReadOnly IndexColumnSubTotal As Integer = 3

        Private Shared ReadOnly InvoiceBackground As New Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(System.Windows.Media.Color.FromArgb(255, 44, 62, 80))
        Private Shared ReadOnly InvoiceHeaderForeground As New Telerik.Windows.Documents.Spreadsheet.Model.ThemableColor(System.Windows.Media.Color.FromArgb(255, 255, 255, 255))
        Private Shared ReadOnly EnUSCultureAccountFormatString As String = "_($* #,##0.00_);_($* (#,##0.00);_($* ""-""??_);_(@_)"

        Public Sub New()
            InitializeComponent()

            WorkbookFormatProvidersManager.RegisterFormatProvider(New XlsxFormatProvider())

            If Program.themeName <> "" Then
                'set the example theme to the same theme QSF uses
                Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Program.themeName
            End If
        End Sub

        Private Sub Form1_Load(sender As Object, e As EventArgs)
            Me.exportFormatDropDownList.DataSource = ExportFormats
            Me.exportFormatDropDownList.SelectedIndex = 0
            Me.radGridView1.AutoGenerateColumns = False
            Me.radGridView1.AllowEditRow = False
            Me.radGridView1.AllowDeleteRow = False
            Me.radGridView1.AllowAddNewRow = False
            Me.radGridView1.ShowRowHeaderColumn = False
            Me.radGridView1.DataSource = Me.products.GetData(20)
            Me.radGridView1.BestFitColumns()
            Me.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill
            Me.radGridView1.CurrentRow = Nothing
            AddHandler Me.radGridView1.SelectionChanging, AddressOf SelectionChanging

            Me.CalculateTotal()
        End Sub

        Private Sub SelectionChanging(sender As Object, e As GridViewSelectionCancelEventArgs)
            e.Cancel = True
        End Sub

        Private Sub CalculateTotal()
            Dim sum As Decimal = 0
            For Each row In Me.radGridView1.Rows
                sum = sum + Decimal.Parse(row.Cells(3).Value.ToString())
            Next row

            Me.totalSumLabel.Text = String.Format("{0:C}", sum)
        End Sub

        Public ReadOnly Property ExportFormats() As IEnumerable(Of String)
            Get
                If Me.m_exportFormats Is Nothing Then
                    Me.m_exportFormats = New String() {"Xlsx", "Csv", "Txt"}
                End If

                Return Me.m_exportFormats
            End Get
        End Property

        Private Sub exportButton_Click(sender As Object, e As EventArgs)
            Dim workbook As Workbook = Me.CreateWorkbook(Me.radGridView1)
            SaveDocument(workbook, Me.exportFormatDropDownList.Text)
        End Sub


        Private Function CreateWorkbook(gridView As RadGridView) As Workbook
            Dim workbook As New Workbook()
            workbook.Sheets.Add(SheetType.Worksheet)

            Dim worksheet As Worksheet = workbook.ActiveWorksheet

            Me.PrepareInvoiceDocument(worksheet, gridView.RowCount)

            Dim currentRow As Integer = IndexColumnName + 1
            For Each product In gridView.Rows
                worksheet.Cells(currentRow, IndexColumnName).SetValue(product.Cells(IndexColumnName).Value.ToString())
                worksheet.Cells(currentRow, IndexColumnQuantity).SetValue(product.Cells(IndexColumnQuantity).Value.ToString())
                worksheet.Cells(currentRow, IndexColumnUnitPrice).SetValue(product.Cells(IndexColumnUnitPrice).Value.ToString())
                worksheet.Cells(currentRow, IndexColumnSubTotal).SetValue(product.Cells(IndexColumnSubTotal).Value.ToString())

                currentRow += 1
            Next product

            Return workbook
        End Function

        Private Sub PrepareInvoiceDocument(worksheet As Worksheet, itemsCount As Integer)
            Dim lastItemIndexRow As Integer = IndexColumnName + itemsCount

            Dim firstUsedCellIndex As New CellIndex(0, 0)
            Dim lastUsedCellIndex As New CellIndex(lastItemIndexRow + 1, IndexColumnSubTotal)
            Dim border As New CellBorder(CellBorderStyle.DashDot, InvoiceBackground)
            worksheet.Cells(firstUsedCellIndex, lastUsedCellIndex).SetBorders(New CellBorders(border, border, border, border, Nothing, Nothing, _
                Nothing, Nothing))

            worksheet.Cells(firstUsedCellIndex).SetValue("INVOICE")
            worksheet.Cells(firstUsedCellIndex).SetFontSize(20)

            worksheet.Columns(0).SetWidth(New ColumnWidth(300, True))
            worksheet.Columns(IndexColumnUnitPrice).SetWidth(New ColumnWidth(120, True))
            worksheet.Columns(IndexColumnSubTotal).SetWidth(New ColumnWidth(120, True))

            worksheet.Cells(IndexColumnName, 0).SetValue("Item")
            worksheet.Cells(IndexColumnName, IndexColumnQuantity).SetValue("QTY")
            worksheet.Cells(IndexColumnName, IndexColumnUnitPrice).SetValue("Unit Price")
            worksheet.Cells(IndexColumnName, IndexColumnSubTotal).SetValue("SubTotal")

            worksheet.Cells(IndexColumnName, 0, IndexColumnName, IndexColumnSubTotal).SetFill(New GradientFill(GradientType.Horizontal, InvoiceBackground, InvoiceBackground))
            worksheet.Cells(IndexColumnName, 0, IndexColumnName, IndexColumnSubTotal).SetForeColor(InvoiceHeaderForeground)
            worksheet.Cells(IndexColumnName, IndexColumnUnitPrice, lastItemIndexRow, IndexColumnSubTotal).SetFormat(New CellValueFormat(EnUSCultureAccountFormatString))

            worksheet.Cells(lastItemIndexRow + 1, 2).SetValue("TOTAL: ")
            worksheet.Cells(lastItemIndexRow + 1, 3).SetFormat(New CellValueFormat(EnUSCultureAccountFormatString))

            Dim subTotalColumnCellRange As String = NameConverter.ConvertCellRangeToName(New CellIndex(IndexColumnName + 1, IndexColumnSubTotal), New CellIndex(lastItemIndexRow, IndexColumnSubTotal))

            worksheet.Cells(lastItemIndexRow + 1, IndexColumnSubTotal).SetValue(String.Format("=SUM({0})", subTotalColumnCellRange))

            worksheet.Cells(lastItemIndexRow + 1, IndexColumnUnitPrice, lastItemIndexRow + 1, IndexColumnSubTotal).SetFontSize(20)
        End Sub

        Public Shared Sub SaveDocument(workbook As Workbook, selectedFormat As String)
            Dim formatProvider As IWorkbookFormatProvider = GetFormatProvider(selectedFormat)

            If formatProvider Is Nothing Then
                Return
            End If

            Dim dialog As New SaveFileDialog()
            dialog.Filter = [String].Format("{0} files|*{1}|All files (*.*)|*.*", selectedFormat, formatProvider.SupportedExtensions.First())
            dialog.FileName = "SpreadDocumentsGeneration"

            If dialog.ShowDialog() = DialogResult.OK Then
                Try
                    Using stream = dialog.OpenFile()
                        formatProvider.Export(workbook, stream)
                    End Using
                Catch ex As IOException
                    MessageBox.Show(ex.Message, "Error")
                End Try
            End If
        End Sub

        Private Shared Function GetFormatProvider(extension As String) As IWorkbookFormatProvider
            Dim formatProvider As IWorkbookFormatProvider
            Select Case extension
                Case "Xlsx"
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("XlsxFormatProvider")
                    Exit Select
                Case "Csv"
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("CsvFormatProvider")
                    TryCast(formatProvider, CsvFormatProvider).Settings.HasHeaderRow = True
                    Exit Select
                Case "Txt"
                    formatProvider = WorkbookFormatProvidersManager.GetProviderByName("TxtFormatProvider")
                    Exit Select
                Case Else
                    formatProvider = Nothing
                    Exit Select
            End Select

            Return formatProvider
        End Function

    End Class
End Namespace