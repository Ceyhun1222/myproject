Namespace Telerik.Examples.WinControls.PageView.StripView.CustomTheme
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

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Me.enableAnimationCheck = New Telerik.WinControls.UI.RadCheckBox()
            Me.radLabel1 = New Telerik.WinControls.UI.RadLabel()
            Me.animationTypeCombo = New Telerik.WinControls.UI.RadDropDownList()
            Me.shrinkItemsCheck = New Telerik.WinControls.UI.RadCheckBox()
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.settingsPanel.SuspendLayout()
            CType(Me.enableAnimationCheck, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.animationTypeCombo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.shrinkItemsCheck, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'settingsPanel
            '
            Me.settingsPanel.Controls.Add(Me.shrinkItemsCheck)
            Me.settingsPanel.Controls.Add(Me.animationTypeCombo)
            Me.settingsPanel.Controls.Add(Me.radLabel1)
            Me.settingsPanel.Controls.Add(Me.enableAnimationCheck)
            Me.settingsPanel.Location = New System.Drawing.Point(999, 1)
            Me.settingsPanel.Size = New System.Drawing.Size(200, 524)
            Me.settingsPanel.ThemeName = "ControlDefault"
            Me.settingsPanel.Controls.SetChildIndex(Me.enableAnimationCheck, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.radLabel1, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.animationTypeCombo, 0)
            Me.settingsPanel.Controls.SetChildIndex(Me.shrinkItemsCheck, 0)
            '
            'enableAnimationCheck
            '
            Me.enableAnimationCheck.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.enableAnimationCheck.Location = New System.Drawing.Point(10, 38)
            Me.enableAnimationCheck.Name = "enableAnimationCheck"
            Me.enableAnimationCheck.Size = New System.Drawing.Size(139, 18)
            Me.enableAnimationCheck.TabIndex = 1
            Me.enableAnimationCheck.Text = "Enable Scroll Animation"
            '
            'radLabel1
            '
            Me.radLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.radLabel1.Location = New System.Drawing.Point(10, 62)
            Me.radLabel1.Name = "radLabel1"
            Me.radLabel1.Size = New System.Drawing.Size(118, 18)
            Me.radLabel1.TabIndex = 2
            Me.radLabel1.Text = "Scroll Animation Type:"
            '
            'animationTypeCombo
            '
            Me.animationTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.animationTypeCombo.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
            Me.animationTypeCombo.Location = New System.Drawing.Point(10, 84)
            Me.animationTypeCombo.Name = "animationTypeCombo"
            '
            '
            '
            Me.animationTypeCombo.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
            Me.animationTypeCombo.Size = New System.Drawing.Size(180, 20)
            Me.animationTypeCombo.TabIndex = 3
            '
            'shrinkItemsCheck
            '
            Me.shrinkItemsCheck.Anchor = System.Windows.Forms.AnchorStyles.Top
            Me.shrinkItemsCheck.Location = New System.Drawing.Point(10, 122)
            Me.shrinkItemsCheck.Name = "shrinkItemsCheck"
            Me.shrinkItemsCheck.Size = New System.Drawing.Size(82, 18)
            Me.shrinkItemsCheck.TabIndex = 4
            Me.shrinkItemsCheck.Text = "Shrink Items"
            '
            'Form1
            '
            Me.Name = "Form1"
            Me.Size = New System.Drawing.Size(1337, 1022)
            CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
            Me.settingsPanel.ResumeLayout(False)
            Me.settingsPanel.PerformLayout()
            CType(Me.enableAnimationCheck, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radLabel1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.animationTypeCombo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.shrinkItemsCheck, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Private enableAnimationCheck As Telerik.WinControls.UI.RadCheckBox
		Private radLabel1 As Telerik.WinControls.UI.RadLabel
		Private shrinkItemsCheck As Telerik.WinControls.UI.RadCheckBox
		Private animationTypeCombo As Telerik.WinControls.UI.RadDropDownList
	End Class
End Namespace
