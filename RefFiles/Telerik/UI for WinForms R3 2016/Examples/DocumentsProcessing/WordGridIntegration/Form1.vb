Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Telerik.WinControls.Data
Imports Telerik.WinControls.UI
Imports Telerik.Windows.Documents.Common.FormatProviders
Imports Telerik.Windows.Documents.Flow.FormatProviders.Docx
Imports Telerik.Windows.Documents.Flow.FormatProviders.Rtf
Imports Telerik.Windows.Documents.Flow.FormatProviders.Txt
Imports Telerik.Windows.Documents.Flow.Model
Imports Telerik.Windows.Documents.Flow.Model.Styles
Imports Telerik.Windows.Documents.Spreadsheet.Model

Namespace WordGridIntegration
    Partial Public Class Form1
        Inherits RadForm
        Private Const WidthOfIndentColumns As Integer = 20

        Private Shared ReadOnly DefaultHeaderRowColor As Color = Color.FromArgb(255, 127, 127, 127)
        Private Shared ReadOnly DefaultGroupHeaderRowColor As Color = Color.FromArgb(255, 216, 216, 216)
        Private Shared ReadOnly DefaultDataRowColor As Color = Color.FromArgb(255, 251, 247, 255)

        Private m_products As List(Of Product)
        Private m_headerRowColor As Color
        Private m_dataRowColor As Color
        Private m_groupHeaderRowColor As Color
        Private m_exportFormats As String()
        Private m_selectedExportFormat As String
        Private m_repeatHeaderRowOnEveryPage As Boolean = True

        Public Sub New()
            InitializeComponent()

            If Program.themeName <> "" Then
                'set the example theme to the same theme QSF uses
                Telerik.WinControls.ThemeResolutionService.ApplicationThemeName = Program.themeName
            End If
        End Sub

        Public Property Products() As List(Of Product)
            Get
                Return Me.m_products
            End Get
            Set(value As List(Of Product))
                If Not Me.m_products Is value Then
                    Me.m_products = value
                End If
            End Set
        End Property


        Public Property HeaderRowColor() As Color
            Get
                Return Me.m_headerRowColor
            End Get
            Set(value As Color)
                If Me.m_headerRowColor <> value Then
                    Me.m_headerRowColor = value
                End If
            End Set
        End Property

        Public Property DataRowColor() As Color
            Get
                Return Me.m_dataRowColor
            End Get
            Set(value As Color)
                If Me.m_dataRowColor <> value Then
                    Me.m_dataRowColor = value
                End If
            End Set
        End Property

        Public Property GroupHeaderRowColor() As Color
            Get
                Return Me.m_groupHeaderRowColor
            End Get
            Set(value As Color)
                If Me.m_groupHeaderRowColor <> value Then
                    Me.m_groupHeaderRowColor = value
                End If
            End Set
        End Property

        Public ReadOnly Property ExportFormats() As IEnumerable(Of String)
            Get
                If m_exportFormats Is Nothing Then
                    m_exportFormats = New String() {"Docx", "Rtf", "Txt"}
                End If

                Return m_exportFormats
            End Get
        End Property

        Public Property SelectedExportFormat() As String
            Get
                Return m_selectedExportFormat
            End Get
            Set(value As String)
                If Not Object.Equals(m_selectedExportFormat, value) Then
                    m_selectedExportFormat = value
                End If
            End Set
        End Property

        Public Property RepeatHeaderRowOnEveryPage() As Boolean
            Get
                Return Me.m_repeatHeaderRowOnEveryPage
            End Get
            Set(value As Boolean)
                If Me.m_repeatHeaderRowOnEveryPage <> value Then
                    Me.m_repeatHeaderRowOnEveryPage = value
                End If
            End Set
        End Property

        Private Sub Form1_Load(sender As Object, e As EventArgs)
            Me.Products = New Products().GetData(100).ToList()

            Me.SelectedExportFormat = Me.ExportFormats.First()

            Me.HeaderRowColor = DefaultHeaderRowColor
            Me.DataRowColor = DefaultDataRowColor
            Me.GroupHeaderRowColor = DefaultGroupHeaderRowColor

            Me.radGridView1.DataSource = Me.Products
            Me.radGridView1.BestFitColumns()
            Me.radGridView1.Columns("UnitPrice").FormatString = "{0:C}"
            Me.radGridView1.Columns("Date").DataType = GetType(DateTime)
            Me.radGridView1.Columns("Date").FormatString = "{0:d}"
            Me.radGridView1.Columns.Remove(Me.radGridView1.Columns("SubTotal"))
            Me.radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill

            Me.radGridView1.AutoExpandGroups = True
            Dim descriptor As New GroupDescriptor()
            descriptor.GroupNames.Add("UnitPrice", ListSortDirection.Ascending)
            Me.radGridView1.GroupDescriptors.Add(descriptor)


            headerRowColorBox.Value = DefaultHeaderRowColor
            groupHeaderColorBox.Value = DefaultGroupHeaderRowColor
            dataRowColorBox.Value = DefaultDataRowColor
            Me.exportFormatDropDownList.DataSource = ExportFormats
        End Sub

        Private Sub exportButton_Click(sender As Object, e As EventArgs)
            Dim document As RadFlowDocument = Me.CreateDocument(radGridView1)

            Dim selectedFromat As String = Me.SelectedExportFormat
            SaveDocument(document, selectedFromat)
        End Sub
        Public Shared Sub SaveDocument(document As RadFlowDocument, selectedFromat As String)
            Dim formatProvider As IFormatProvider(Of RadFlowDocument) = Nothing
            Select Case selectedFromat
                Case "Docx"
                    formatProvider = New DocxFormatProvider()
                    Exit Select
                Case "Rtf"
                    formatProvider = New RtfFormatProvider()
                    Exit Select
                Case "Txt"
                    formatProvider = New TxtFormatProvider()
                    Exit Select
            End Select
            If formatProvider Is Nothing Then
                Return
            End If

            Dim dialog As New SaveFileDialog()
            dialog.Filter = [String].Format("{0} files|*{1}|All files (*.*)|*.*", selectedFromat, formatProvider.SupportedExtensions.First())
            dialog.FilterIndex = 1
            dialog.FileName = "WordGridIntegration"

            If dialog.ShowDialog() = DialogResult.OK Then
                Using stream = dialog.OpenFile()
                    formatProvider.Export(document, stream)
                End Using
            End If
        End Sub
        Private Function CreateDocument(grid As RadGridView) As RadFlowDocument
            Dim document As New RadFlowDocument()
            Dim table As Table = document.Sections.AddSection().Blocks.AddTable()
            document.StyleRepository.AddBuiltInStyle(BuiltInStyleNames.TableGridStyleId)
            table.StyleId = BuiltInStyleNames.TableGridStyleId

            ' where c.IsVisible
            Dim columns As IList(Of GridViewColumn) = (From c In grid.Columns.OfType(Of GridViewColumn)() Order By c.Index Select c).ToList()
            Dim indentColumns As Integer = grid.GroupDescriptors.Count
            Dim rowIndex As Integer = 0

            If grid.ShowColumnHeaders Then
                Dim headerRow As TableRow = table.Rows.AddTableRow()
                headerRow.RepeatOnEveryPage = Me.RepeatHeaderRowOnEveryPage
                Dim headerBackground As ThemableColor = Me.ConvertColor(Me.headerRowColorBox.Value)

                If grid.GroupDescriptors.Count > 0 Then
                    Me.AddIndentCell(headerRow, indentColumns, headerBackground)
                End If

                For i As Integer = 0 To columns.Count - 1
                    Dim cell As TableCell = headerRow.Cells.AddTableCell()
                    cell.Shading.BackgroundColor = headerBackground
                    cell.PreferredWidth = New TableWidthUnit(columns(i).Width)
                    Dim headerRun As Run = cell.Blocks.AddParagraph().Inlines.AddRun(columns(i).Name)
                    headerRun.FontWeight = System.Windows.FontWeights.Bold
                Next
            End If

            If grid.Groups.Count > 0 Then
                For Each group As DataGroup In grid.Groups
                    rowIndex = Me.AddGroupRow(table, rowIndex, group.Level, group, columns)
                    Me.AddDataRows(table, rowIndex, group.Level + 1, group, columns)
                Next
            Else
                Me.AddDataRows(table, rowIndex, 0, grid.Rows, columns)
            End If

            document.Sections.First().Blocks.AddParagraph()
            Return document
        End Function

        Private Sub AddDataRows(table As Table, startRowIndex As Integer, indentColumnsstartColumnIndex As Integer, rows As IEnumerable(Of Telerik.WinControls.UI.GridViewRowInfo), columns As IList(Of GridViewColumn))
            Dim background As ThemableColor = ConvertColor(Me.dataRowColorBox.Value)
            For Each row In rows
                Dim tableRows As TableRow = table.Rows.AddTableRow()
                If indentColumnsstartColumnIndex > 0 Then
                    Me.AddIndentCell(tableRows, indentColumnsstartColumnIndex, background)
                End If

                For columnIndex As Integer = 0 To columns.Count - 1
                    Dim cell As TableCell = tableRows.Cells.AddTableCell()
                    Me.AddCellValue(cell, row.Cells(columnIndex).Value)
                    cell.Shading.BackgroundColor = background
                    cell.PreferredWidth = New TableWidthUnit(columns(columnIndex).Width)
                Next
            Next row


        End Sub

        Private Function AddGroupRow(table As Table, rowIndex As Integer, numberOfIndentCells As Integer, group As DataGroup, columns As IList(Of GridViewColumn)) As Integer
            Dim level As Integer = Me.GetGroupLevel(group)
            Dim row As TableRow = table.Rows.AddTableRow()
            If level > 0 Then
                Me.AddIndentCell(row, level, ConvertColor(Me.dataRowColorBox.Value))
            End If
            Dim cell As TableCell = row.Cells.AddTableCell()
            cell.Shading.BackgroundColor = ConvertColor(Me.groupHeaderColorBox.Value)
            cell.ColumnSpan = columns.Count + (If(radGridView1.GroupDescriptors.Count > 0, 1, 0)) - (If(level > 0, 1, 0))
            Me.AddCellValue(cell, group.GroupRow.HeaderText)
            Return rowIndex
        End Function

        Private Sub AddCellValue(cell As TableCell, value As Object)
            Dim stringValue As String = If(value IsNot Nothing, value.ToString(), String.Empty)
            cell.Blocks.AddParagraph().Inlines.AddRun(stringValue)
        End Sub

        Private Sub AddIndentCell(row As TableRow, indentColumns As Integer, background As ThemableColor)
            Dim indentCell As TableCell = row.Cells.AddTableCell()
            indentCell.PreferredWidth = New TableWidthUnit(indentColumns * WidthOfIndentColumns)
            indentCell.Shading.BackgroundColor = background
            indentCell.Blocks.AddParagraph()
        End Sub

        Private Function GetGroupLevel(group As DataGroup) As Integer
            Return group.Level
        End Function

        Private Function ConvertColor(color As Color) As ThemableColor
            Return ThemableColor.FromArgb(color.A, color.R, color.G, color.B)
        End Function

        Private Sub radCheckBox1_CheckStateChanged(sender As Object, e As EventArgs)
            Me.RepeatHeaderRowOnEveryPage = Me.radCheckBox1.Checked
        End Sub

        Private Sub exportFormatDropDownList_SelectedIndexChanged(sender As Object, e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Me.SelectedExportFormat = exportFormatDropDownList.Items(e.Position).Text
        End Sub
    End Class
End Namespace