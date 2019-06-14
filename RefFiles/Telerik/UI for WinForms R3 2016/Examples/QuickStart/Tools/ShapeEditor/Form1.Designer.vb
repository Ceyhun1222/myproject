Namespace Telerik.Examples.WinControls.Tools.ShapeEditor
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.radButton1 = New Telerik.WinControls.UI.RadButton()
            Me.radDropDownList1 = New Telerik.WinControls.UI.RadDropDownList()
            Me.editShapeButton1 = New Telerik.WinControls.UI.RadButton()
            Me.customShape1 = New Telerik.WinControls.CustomShape(Me.components)
            Me.customShape2 = New Telerik.WinControls.CustomShape(Me.components)
            Me.customShape3 = New Telerik.WinControls.CustomShape(Me.components)
            Me.radPanel2 = New Telerik.WinControls.UI.RadPanel()
            Me.roundRectShape1 = New Telerik.WinControls.RoundRectShape(Me.components)
            Me.radGroupShape = New Telerik.WinControls.UI.RadGroupBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.editShapeButton1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radPanel2.SuspendLayout()
            CType(Me.radGroupShape, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.radGroupShape.SuspendLayout()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.radGroupShape)
            Me.settingsPanel.Location = New System.Drawing.Point(1023, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 735)
            Me.settingsPanel.ThemeName = "ControlDefault"
            Me.settingsPanel.Controls.SetChildIndex(Me.radGroupShape, 0)
            '
            'radButton1
            '
            Me.radButton1.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.radButton1.RootElement.BackColor = System.Drawing.Color.Transparent
            Me.radButton1.Location = New System.Drawing.Point(137, 50)
            Me.radButton1.Name = "radButton1"
            Me.radButton1.Size = New System.Drawing.Size(130, 114)
            Me.radButton1.TabIndex = 0
            Me.radButton1.Text = "Button"
            '
            'radDropDownList1
            '
            Me.radDropDownList1.AllowShowFocusCues = False
            Me.radDropDownList1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radDropDownList1.DropDownSizingMode = CType((Telerik.WinControls.UI.SizingMode.RightBottom Or Telerik.WinControls.UI.SizingMode.UpDown), Telerik.WinControls.UI.SizingMode)
            Me.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.radDropDownList1.ForeColor = System.Drawing.Color.Black
            Me.radDropDownList1.Location = New System.Drawing.Point(5, 36)
            Me.radDropDownList1.MaxDropDownItems = 6
            Me.radDropDownList1.Name = "radDropDownList1"
            '
            '
            '
            Me.radDropDownList1.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.radDropDownList1.Size = New System.Drawing.Size(170, 20)
            Me.radDropDownList1.TabIndex = 6
            '
            'editShapeButton1
            '
            Me.editShapeButton1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.editShapeButton1.Location = New System.Drawing.Point(5, 65)
            Me.editShapeButton1.Name = "editShapeButton1"
            Me.editShapeButton1.Size = New System.Drawing.Size(170, 23)
            Me.editShapeButton1.TabIndex = 10
            Me.editShapeButton1.Text = "Edit Shape"
            '
            'customShape1
            '
            Me.customShape1.AsString = "0,0,0,0:"
            '
            'customShape2
            '
            Me.customShape2.AsString = "0,0,224,128:0,0,False,0,0,0,0,0:224,0,False,0,0,0,0,0:192,96,False,0,0,0,0,0:32,1" & _
        "28,False,0,0,0,0,0:"
            '
            'customShape3
            '
            Me.customShape3.AsString = "0,0,192,128:0,32,True,64,0,128,0,0:192,0,False,0,0,0,0,0:192,96,True,128,128,64,1" & _
        "28,0:0,128,False,0,0,0,0,0:"
            '
            'radPanel2
            '
            Me.radPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left Or AnchorStyles.Top
            Me.radPanel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(248, Byte), Integer))
            Me.radPanel2.Controls.Add(Me.radButton1)
            Me.radPanel2.ForeColor = System.Drawing.Color.Black
            Me.radPanel2.Name = "radPanel2"
            Me.radPanel2.Size = New System.Drawing.Size(404, 215)
            Me.radPanel2.TabIndex = 19
            CType(Me.radPanel2.GetChildAt(0), Telerik.WinControls.UI.RadPanelElement).Shape = Me.roundRectShape1
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor2 = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(255, Byte), Integer))
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor3 = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor4 = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).NumberOfColors = 4
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).GradientPercentage = 0.3!
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).GradientPercentage2 = 0.01!
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).BackColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(255, Byte), Integer))
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).GradientAngle = 0.0!
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).ForeColor = System.Drawing.Color.FromArgb(CType(CType(162, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(254, Byte), Integer))
            CType(Me.radPanel2.GetChildAt(0).GetChildAt(1), Telerik.WinControls.Primitives.BorderPrimitive).BackColor = System.Drawing.Color.FromArgb(CType(CType(162, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(254, Byte), Integer))
            '
            'roundRectShape1
            '
            Me.roundRectShape1.Radius = 10
            '
            'radGroupShape
            '
            Me.radGroupShape.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
            Me.radGroupShape.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radGroupShape.Controls.Add(Me.radDropDownList1)
            Me.radGroupShape.Controls.Add(Me.editShapeButton1)
            Me.radGroupShape.FooterText = ""
            Me.radGroupShape.HeaderText = "Shape Options"
            Me.radGroupShape.Location = New System.Drawing.Point(10, 3)
            Me.radGroupShape.Name = "radGroupShape"
            Me.radGroupShape.Padding = New System.Windows.Forms.Padding(10, 20, 10, 10)
            Me.radGroupShape.Size = New System.Drawing.Size(180, 116)
            Me.radGroupShape.TabIndex = 0
            Me.radGroupShape.Text = "Shape Options"
            '
            'Form1
            '
            Me.BackColor = System.Drawing.Color.Transparent
            Me.Controls.Add(Me.radPanel2)
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1405, 535)
            Me.Controls.SetChildIndex(Me.themePanel, 0)
            Me.Controls.SetChildIndex(Me.radPanel2, 0)
            Me.Controls.SetChildIndex(Me.settingsPanel, 0)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.themePanel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radButton1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radDropDownList1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.editShapeButton1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radPanel2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radPanel2.ResumeLayout(False)
            CType(Me.radGroupShape, System.ComponentModel.ISupportInitialize).EndInit()
            Me.radGroupShape.ResumeLayout(False)
            Me.radGroupShape.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radButton1 As Telerik.WinControls.UI.RadButton
        Private radDropDownList1 As Telerik.WinControls.UI.RadDropDownList
        Private editShapeButton1 As Telerik.WinControls.UI.RadButton
        Private customShape1 As Telerik.WinControls.CustomShape
        Private customShape2 As Telerik.WinControls.CustomShape
        Private customShape3 As Telerik.WinControls.CustomShape
        WithEvents radPanel2 As Telerik.WinControls.UI.RadPanel
        Private roundRectShape1 As Telerik.WinControls.RoundRectShape
        Private radGroupShape As Telerik.WinControls.UI.RadGroupBox
    End Class
End Namespace