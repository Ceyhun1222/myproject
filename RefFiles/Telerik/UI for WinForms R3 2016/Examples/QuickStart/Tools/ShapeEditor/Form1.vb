Imports System.ComponentModel
Imports System.Text
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports System.Reflection
Imports Telerik.WinControls.Tests

Namespace Telerik.Examples.WinControls.Tools.ShapeEditor
    Partial Public Class Form1
        Inherits ExamplesForm
        Private shapedForm As New ShapedForm()

        Public Sub New()
            InitializeComponent()

            AddHandler radButton1.MouseDown, AddressOf radButton1_MouseDown
        End Sub

        Private Sub radButton1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
            Me.radButton1.ButtonElement.IsDefault = False
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)
            InitButtonPreview()
        End Sub

        Private Sub radDropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Data.PositionChangedEventArgs)
            Select Case Me.radDropDownList1.SelectedIndex
                Case 0
                    Me.radButton1.RootElement.Children(0).Shape = customShape1
                    editShapeButton1.Enabled = True
                Case 1
                    Me.radButton1.RootElement.Children(0).Shape = New RoundRectShape()
                    editShapeButton1.Enabled = False
                Case 2
                    Me.radButton1.RootElement.Children(0).Shape = New ChamferedRectShape()
                    editShapeButton1.Enabled = False
                Case 3
                    Me.radButton1.RootElement.Children(0).Shape = New EllipseShape()
                    editShapeButton1.Enabled = False
                Case 4
                    Me.radButton1.RootElement.Children(0).Shape = New DonutShape()
                    editShapeButton1.Enabled = False
                Case 5
                    Me.radButton1.RootElement.Children(0).Shape = New TabOffice12Shape()
                    editShapeButton1.Enabled = False
                Case 6
                    Me.radButton1.RootElement.Children(0).Shape = New TabVsShape()
                    editShapeButton1.Enabled = False
            End Select
        End Sub

        Private Sub editShapeButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If TypeOf Me.radButton1.RootElement.Children(0).Shape Is CustomShape Then
                Dim editor As New CustomShapeEditorForm()
                Me.radButton1.RootElement.Children(0).Shape = editor.EditShape(TryCast(Me.radButton1.RootElement.Children(0).Shape, CustomShape))
            End If
        End Sub

        Private Sub InitButtonPreview()
            customShape1.CreateClosedShape(CreateInitialShape(5, 100, 60))

            Dim item As New RadListDataItem("customShape1")
            item.Value = customShape1
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("RoundRectShape")
            item.Value = GetType(RoundRectShape)
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("ChamferedRectShape")
            item.Value = GetType(ChamferedRectShape)
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("EllipseShape")
            item.Value = GetType(EllipseShape)
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("DonutShape")
            item.Value = GetType(DonutShape)
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("TabOffice12Shape")
            item.Value = GetType(TabOffice12Shape)
            radDropDownList1.Items.Add(item)
            item = New RadListDataItem("TabVsShape")
            item.Value = GetType(TabVsShape)
            radDropDownList1.Items.Add(item)
            radDropDownList1.BindingContext = Me.BindingContext
            radDropDownList1.SelectedIndex = -1
            radDropDownList1.SelectedIndex = 0
        End Sub

        Private Shared Function CreateInitialShape(ByVal vertices As Integer, ByVal radius1 As Double, ByVal radius2 As Double) As List(Of PointF)
            Dim pts As New List(Of PointF)()

            If radius1 = 0 Then
                Return Nothing
            End If

            If radius2 = 0 Then
                Return Nothing
            End If

            For i As Integer = 0 To vertices - 1
                Dim angle1 As Double = ((4.0 * i - vertices) * Math.PI) / (2.0F * vertices)
                Dim angle2 As Double = ((4.0 * i - vertices + 2) * Math.PI) / (2.0F * vertices)
                pts.Add(New PointF(CSng(Math.Cos(angle1) * radius1), CSng(Math.Sin(angle1) * radius1)))
                pts.Add(New PointF(CSng(Math.Cos(angle2) * radius2), CSng(Math.Sin(angle2) * radius2)))
            Next i
            Return pts
        End Function

        Protected Overrides Sub WireEvents()
            AddHandler radDropDownList1.SelectedIndexChanged, AddressOf radDropDownList1_SelectedIndexChanged
            AddHandler editShapeButton1.Click, AddressOf editShapeButton_Click
        End Sub
    End Class
End Namespace