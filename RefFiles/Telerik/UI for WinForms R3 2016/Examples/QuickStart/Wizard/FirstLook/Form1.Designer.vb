﻿Namespace Telerik.Examples.WinControls.Wizard.FirstLook
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
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Dim gridViewTextBoxColumn1 As New Telerik.WinControls.UI.GridViewTextBoxColumn()
			Dim gridViewCheckBoxColumn1 As New Telerik.WinControls.UI.GridViewCheckBoxColumn()
			Dim gridViewCheckBoxColumn2 As New Telerik.WinControls.UI.GridViewCheckBoxColumn()
			Me.btnSpy = New Telerik.WinControls.UI.RadButton()
			Me.radWizard1 = New Telerik.WinControls.UI.RadWizard()
			Me.wizardCompletionPage1 = New Telerik.WinControls.UI.WizardCompletionPage()
			Me.panel3 = New Panel()
			Me.radLabelCompletion = New Telerik.WinControls.UI.RadLabel()
			Me.panel1 = New Panel()
			Me.radButtonWelcomeReset = New Telerik.WinControls.UI.RadButton()
			Me.radCheckBoxWelcomeImage = New Telerik.WinControls.UI.RadCheckBox()
			Me.radButtonWelcome = New Telerik.WinControls.UI.RadButton()
			Me.radButtonWelcomeBackColor = New Telerik.WinControls.UI.RadButton()
			Me.radLabelWelcomeImageBackColor = New Telerik.WinControls.UI.RadLabel()
			Me.radDropDownListWelcome = New Telerik.WinControls.UI.RadDropDownList()
			Me.radLabelWelcomeImageLayout = New Telerik.WinControls.UI.RadLabel()
			Me.radButtonWelcomeBrowse = New Telerik.WinControls.UI.RadButton()
			Me.radLabelWelcomeImage = New Telerik.WinControls.UI.RadLabel()
			Me.radLabelWelcomeDescription = New Telerik.WinControls.UI.RadLabel()
			Me.radLabelWelcome = New Telerik.WinControls.UI.RadLabel()
			Me.panel2 = New Panel()
			Me.radDropDownListIconAlignment = New Telerik.WinControls.UI.RadDropDownList()
			Me.radButtonPage1Reset = New Telerik.WinControls.UI.RadButton()
			Me.radLabelPage1IconAlignment = New Telerik.WinControls.UI.RadLabel()
			Me.radLabelPage1Title = New Telerik.WinControls.UI.RadLabel()
			Me.radButtonPage1Icon = New Telerik.WinControls.UI.RadButton()
			Me.radTextBoxPage1Title = New Telerik.WinControls.UI.RadTextBox()
			Me.radLabelPage1Icon = New Telerik.WinControls.UI.RadLabel()
			Me.radCheckBoxPage1Title = New Telerik.WinControls.UI.RadCheckBox()
			Me.radCheckBoxPage1Header = New Telerik.WinControls.UI.RadCheckBox()
			Me.radLabelPage1Header = New Telerik.WinControls.UI.RadLabel()
			Me.radTextBoxPage1Header = New Telerik.WinControls.UI.RadTextBox()
			Me.panel4 = New Panel()
			Me.radButtonPage2Reset = New Telerik.WinControls.UI.RadButton()
			Me.radGridViewPage2 = New Telerik.WinControls.UI.RadGridView()
			Me.panel5 = New Panel()
			Me.radRadioButtonCompletionPage2 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radRadioButtonCompletionPage1 = New Telerik.WinControls.UI.RadRadioButton()
			Me.radLabelPage3 = New Telerik.WinControls.UI.RadLabel()
			Me.panel6 = New Panel()
			Me.radLabelFinal = New Telerik.WinControls.UI.RadLabel()
			Me.wizardWelcomePage1 = New Telerik.WinControls.UI.WizardWelcomePage()
			Me.wizardPage1 = New Telerik.WinControls.UI.WizardPage()
			Me.wizardPage2 = New Telerik.WinControls.UI.WizardPage()
			Me.wizardPage3 = New Telerik.WinControls.UI.WizardPage()
			Me.wizardPage4 = New Telerik.WinControls.UI.WizardPage()
			Me.panel0 = New Panel()
			Me.pictureBox1 = New PictureBox()
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.btnSpy, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radWizard1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.radWizard1.SuspendLayout()
			Me.panel3.SuspendLayout()
			CType(Me.radLabelCompletion, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel1.SuspendLayout()
			CType(Me.radButtonWelcomeReset, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxWelcomeImage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonWelcome, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonWelcomeBackColor, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelWelcomeImageBackColor, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radDropDownListWelcome, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelWelcomeImageLayout, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonWelcomeBrowse, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelWelcomeImage, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelWelcomeDescription, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelWelcome, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel2.SuspendLayout()
			CType(Me.radDropDownListIconAlignment, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPage1Reset, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelPage1IconAlignment, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelPage1Title, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radButtonPage1Icon, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBoxPage1Title, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelPage1Icon, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxPage1Title, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radCheckBoxPage1Header, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelPage1Header, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radTextBoxPage1Header, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel4.SuspendLayout()
			CType(Me.radButtonPage2Reset, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridViewPage2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radGridViewPage2.MasterTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel5.SuspendLayout()
			CType(Me.radRadioButtonCompletionPage2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radRadioButtonCompletionPage1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.radLabelPage3, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel6.SuspendLayout()
			CType(Me.radLabelFinal, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panel0.SuspendLayout()
			CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' settingsPanel
			' 
			Me.settingsPanel.Location = New Point(1076, 1)
			Me.settingsPanel.Size = New Size(911, 1218)

			' 
			' btnSpy
			' 
			Me.btnSpy.Location = New Point(0, 0)
			Me.btnSpy.Name = "btnSpy"
			Me.btnSpy.Padding = New Padding(2, 0, 0, 0)
			' 
			' 
			' 
			Me.btnSpy.RootElement.Padding = New Padding(2, 0, 0, 0)
			Me.btnSpy.Size = New Size(740, 24)
			Me.btnSpy.TabIndex = 0
			Me.btnSpy.Text = "RadControl Spy "
			' 
			' radWizard1
			' 
			Me.radWizard1.CompletionPage = Me.wizardCompletionPage1
			Me.radWizard1.Controls.Add(Me.panel1)
			Me.radWizard1.Controls.Add(Me.panel2)
			Me.radWizard1.Controls.Add(Me.panel3)
			Me.radWizard1.Controls.Add(Me.panel4)
			Me.radWizard1.Controls.Add(Me.panel5)
			Me.radWizard1.Controls.Add(Me.panel6)
			Me.radWizard1.Location = New Point(0, 28)
			Me.radWizard1.Name = "radWizard1"
			Me.radWizard1.PageHeaderIcon = (CType(resources.GetObject("radWizard1.PageHeaderIcon"), Image))
			Me.radWizard1.Pages.Add(Me.wizardWelcomePage1)
			Me.radWizard1.Pages.Add(Me.wizardPage1)
			Me.radWizard1.Pages.Add(Me.wizardPage2)
			Me.radWizard1.Pages.Add(Me.wizardPage3)
			Me.radWizard1.Pages.Add(Me.wizardPage4)
			Me.radWizard1.Pages.Add(Me.wizardCompletionPage1)
			Me.radWizard1.Size = New Size(554, 402)
			Me.radWizard1.TabIndex = 1
			Me.radWizard1.Text = "radWizard1"
			Me.radWizard1.WelcomePage = Me.wizardWelcomePage1
			' 
			' wizardCompletionPage1
			' 
			Me.wizardCompletionPage1.ContentArea = Me.panel3
			Me.wizardCompletionPage1.Name = "wizardCompletionPage1"
			Me.wizardCompletionPage1.Title = "Completion page"
			Me.wizardCompletionPage1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' panel3
			' 
			Me.panel3.BackColor = Color.White
			Me.panel3.Controls.Add(Me.radLabelCompletion)
			Me.panel3.Location = New Point(150, 76)
			Me.panel3.Name = "panel3"
			Me.panel3.Size = New Size(406, 278)
			Me.panel3.TabIndex = 2
			' 
			' radLabelCompletion
			' 
			Me.radLabelCompletion.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radLabelCompletion.AutoSize = False
			Me.radLabelCompletion.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelCompletion.Location = New Point(45, 49)
			Me.radLabelCompletion.Name = "radLabelCompletion"
			Me.radLabelCompletion.Size = New Size(365, 30)
			Me.radLabelCompletion.TabIndex = 8
			Me.radLabelCompletion.Text = "Thank you for your interest in RadWizard control!"
			' 
			' panel1
			' 
			Me.panel1.BackColor = Color.White
			Me.panel1.Controls.Add(Me.radButtonWelcomeReset)
			Me.panel1.Controls.Add(Me.radCheckBoxWelcomeImage)
			Me.panel1.Controls.Add(Me.radButtonWelcome)
			Me.panel1.Controls.Add(Me.radButtonWelcomeBackColor)
			Me.panel1.Controls.Add(Me.radLabelWelcomeImageBackColor)
			Me.panel1.Controls.Add(Me.radDropDownListWelcome)
			Me.panel1.Controls.Add(Me.radLabelWelcomeImageLayout)
			Me.panel1.Controls.Add(Me.radButtonWelcomeBrowse)
			Me.panel1.Controls.Add(Me.radLabelWelcomeImage)
			Me.panel1.Controls.Add(Me.radLabelWelcomeDescription)
			Me.panel1.Controls.Add(Me.radLabelWelcome)
			Me.panel1.Location = New Point(150, 56)
			Me.panel1.Name = "panel1"
			Me.panel1.Size = New Size(404, 298)
			Me.panel1.TabIndex = 0
			' 
			' radButtonWelcomeReset
			' 
			Me.radButtonWelcomeReset.Location = New Point(38, 236)
			Me.radButtonWelcomeReset.Name = "radButtonWelcomeReset"
			Me.radButtonWelcomeReset.Size = New Size(100, 24)
			Me.radButtonWelcomeReset.TabIndex = 23
			Me.radButtonWelcomeReset.Text = "Reset"
			' 
			' radCheckBoxWelcomeImage
			' 
			Me.radCheckBoxWelcomeImage.Location = New Point(38, 105)
			Me.radCheckBoxWelcomeImage.Name = "radCheckBoxWelcomeImage"
			Me.radCheckBoxWelcomeImage.Size = New Size(120, 18)
			Me.radCheckBoxWelcomeImage.TabIndex = 10
			Me.radCheckBoxWelcomeImage.Text = "Use welcome image"
			Me.radCheckBoxWelcomeImage.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radButtonWelcome
			' 
			Me.radButtonWelcome.Location = New Point(263, 141)
			Me.radButtonWelcome.Name = "radButtonWelcome"
			Me.radButtonWelcome.Size = New Size(100, 24)
			Me.radButtonWelcome.TabIndex = 22
			Me.radButtonWelcome.Text = "Set default"
			' 
			' radButtonWelcomeBackColor
			' 
			Me.radButtonWelcomeBackColor.Location = New Point(157, 198)
			Me.radButtonWelcomeBackColor.Name = "radButtonWelcomeBackColor"
			Me.radButtonWelcomeBackColor.Size = New Size(100, 24)
			Me.radButtonWelcomeBackColor.TabIndex = 21
			Me.radButtonWelcomeBackColor.Text = "Pick..."
			' 
			' radLabelWelcomeImageBackColor
			' 
			Me.radLabelWelcomeImageBackColor.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelWelcomeImageBackColor.Location = New Point(38, 198)
			Me.radLabelWelcomeImageBackColor.Name = "radLabelWelcomeImageBackColor"
			Me.radLabelWelcomeImageBackColor.Size = New Size(113, 18)
			Me.radLabelWelcomeImageBackColor.TabIndex = 20
			Me.radLabelWelcomeImageBackColor.Text = "Set background color"
			' 
			' radDropDownListWelcome
			' 
			Me.radDropDownListWelcome.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListWelcome.Location = New Point(157, 171)
			Me.radDropDownListWelcome.Name = "radDropDownListWelcome"
			Me.radDropDownListWelcome.Size = New Size(206, 20)
			Me.radDropDownListWelcome.TabIndex = 19
			' 
			' radLabelWelcomeImageLayout
			' 
			Me.radLabelWelcomeImageLayout.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelWelcomeImageLayout.Location = New Point(38, 171)
			Me.radLabelWelcomeImageLayout.Name = "radLabelWelcomeImageLayout"
			Me.radLabelWelcomeImageLayout.Size = New Size(71, 18)
			Me.radLabelWelcomeImageLayout.TabIndex = 18
			Me.radLabelWelcomeImageLayout.Text = "Image layout"
			' 
			' radButtonWelcomeBrowse
			' 
			Me.radButtonWelcomeBrowse.Location = New Point(157, 141)
			Me.radButtonWelcomeBrowse.Name = "radButtonWelcomeBrowse"
			Me.radButtonWelcomeBrowse.Size = New Size(100, 24)
			Me.radButtonWelcomeBrowse.TabIndex = 17
			Me.radButtonWelcomeBrowse.Text = "Browse..."
			' 
			' radLabelWelcomeImage
			' 
			Me.radLabelWelcomeImage.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelWelcomeImage.Location = New Point(38, 141)
			Me.radLabelWelcomeImage.Name = "radLabelWelcomeImage"
			Me.radLabelWelcomeImage.Size = New Size(56, 18)
			Me.radLabelWelcomeImage.TabIndex = 16
			Me.radLabelWelcomeImage.Text = "Set image"
			' 
			' radLabelWelcomeDescription
			' 
			Me.radLabelWelcomeDescription.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radLabelWelcomeDescription.AutoSize = False
			Me.radLabelWelcomeDescription.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelWelcomeDescription.Location = New Point(38, 44)
			Me.radLabelWelcomeDescription.Name = "radLabelWelcomeDescription"
			Me.radLabelWelcomeDescription.Size = New Size(350, 37)
			Me.radLabelWelcomeDescription.TabIndex = 4
			Me.radLabelWelcomeDescription.Text = "You can preview the customization options step by step using RadWizard control."
			' 
			' radLabelWelcome
			' 
			Me.radLabelWelcome.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radLabelWelcome.AutoSize = False
			Me.radLabelWelcome.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelWelcome.Location = New Point(38, 18)
			Me.radLabelWelcome.Name = "radLabelWelcome"
			Me.radLabelWelcome.Size = New Size(350, 20)
			Me.radLabelWelcome.TabIndex = 0
			Me.radLabelWelcome.Text = "Welcome to RadWizard control preview"
			' 
			' panel2
			' 
			Me.panel2.BackColor = Color.White
			Me.panel2.Controls.Add(Me.radDropDownListIconAlignment)
			Me.panel2.Controls.Add(Me.radButtonPage1Reset)
			Me.panel2.Controls.Add(Me.radLabelPage1IconAlignment)
			Me.panel2.Controls.Add(Me.radLabelPage1Title)
			Me.panel2.Controls.Add(Me.radButtonPage1Icon)
			Me.panel2.Controls.Add(Me.radTextBoxPage1Title)
			Me.panel2.Controls.Add(Me.radLabelPage1Icon)
			Me.panel2.Controls.Add(Me.radCheckBoxPage1Title)
			Me.panel2.Controls.Add(Me.radCheckBoxPage1Header)
			Me.panel2.Controls.Add(Me.radLabelPage1Header)
			Me.panel2.Controls.Add(Me.radTextBoxPage1Header)
			Me.panel2.Location = New Point(0, 81)
			Me.panel2.Name = "panel2"
			Me.panel2.Size = New Size(554, 273)
			Me.panel2.TabIndex = 1
			' 
			' radDropDownListIconAlignment
			' 
			Me.radDropDownListIconAlignment.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList
			Me.radDropDownListIconAlignment.Location = New Point(165, 170)
			Me.radDropDownListIconAlignment.Name = "radDropDownListIconAlignment"
			Me.radDropDownListIconAlignment.Size = New Size(103, 20)
			Me.radDropDownListIconAlignment.TabIndex = 25
			' 
			' radButtonPage1Reset
			' 
			Me.radButtonPage1Reset.Location = New Point(38, 236)
			Me.radButtonPage1Reset.Name = "radButtonPage1Reset"
			Me.radButtonPage1Reset.Size = New Size(100, 24)
			Me.radButtonPage1Reset.TabIndex = 24
			Me.radButtonPage1Reset.Text = "Reset"
			' 
			' radLabelPage1IconAlignment
			' 
			Me.radLabelPage1IconAlignment.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelPage1IconAlignment.Location = New Point(165, 145)
			Me.radLabelPage1IconAlignment.Name = "radLabelPage1IconAlignment"
			Me.radLabelPage1IconAlignment.Size = New Size(81, 18)
			Me.radLabelPage1IconAlignment.TabIndex = 8
			Me.radLabelPage1IconAlignment.Text = "Icon alignment"
			' 
			' radLabelPage1Title
			' 
			Me.radLabelPage1Title.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelPage1Title.Location = New Point(38, 18)
			Me.radLabelPage1Title.Name = "radLabelPage1Title"
			Me.radLabelPage1Title.Size = New Size(27, 18)
			Me.radLabelPage1Title.TabIndex = 0
			Me.radLabelPage1Title.Text = "Title"
			' 
			' radButtonPage1Icon
			' 
			Me.radButtonPage1Icon.Location = New Point(38, 167)
			Me.radButtonPage1Icon.Name = "radButtonPage1Icon"
			Me.radButtonPage1Icon.Size = New Size(100, 24)
			Me.radButtonPage1Icon.TabIndex = 7
			Me.radButtonPage1Icon.Text = "Browse..."
			' 
			' radTextBoxPage1Title
			' 
			Me.radTextBoxPage1Title.Location = New Point(38, 42)
			Me.radTextBoxPage1Title.Name = "radTextBoxPage1Title"
			Me.radTextBoxPage1Title.Size = New Size(271, 20)
			Me.radTextBoxPage1Title.TabIndex = 1
			Me.radTextBoxPage1Title.TabStop = False
			Me.radTextBoxPage1Title.Text = "Page Title"
			' 
			' radLabelPage1Icon
			' 
			Me.radLabelPage1Icon.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelPage1Icon.Location = New Point(38, 145)
			Me.radLabelPage1Icon.Name = "radLabelPage1Icon"
			Me.radLabelPage1Icon.Size = New Size(27, 18)
			Me.radLabelPage1Icon.TabIndex = 6
			Me.radLabelPage1Icon.Text = "Icon"
			' 
			' radCheckBoxPage1Title
			' 
			Me.radCheckBoxPage1Title.Location = New Point(315, 42)
			Me.radCheckBoxPage1Title.Name = "radCheckBoxPage1Title"
			Me.radCheckBoxPage1Title.Size = New Size(53, 18)
			Me.radCheckBoxPage1Title.TabIndex = 2
			Me.radCheckBoxPage1Title.Text = "Visible"
			Me.radCheckBoxPage1Title.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radCheckBoxPage1Header
			' 
			Me.radCheckBoxPage1Header.Location = New Point(315, 100)
			Me.radCheckBoxPage1Header.Name = "radCheckBoxPage1Header"
			Me.radCheckBoxPage1Header.Size = New Size(53, 18)
			Me.radCheckBoxPage1Header.TabIndex = 5
			Me.radCheckBoxPage1Header.Text = "Visible"
			Me.radCheckBoxPage1Header.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radLabelPage1Header
			' 
			Me.radLabelPage1Header.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelPage1Header.Location = New Point(38, 76)
			Me.radLabelPage1Header.Name = "radLabelPage1Header"
			Me.radLabelPage1Header.Size = New Size(90, 18)
			Me.radLabelPage1Header.TabIndex = 3
			Me.radLabelPage1Header.Text = "Page description"
			' 
			' radTextBoxPage1Header
			' 
			Me.radTextBoxPage1Header.Location = New Point(38, 100)
			Me.radTextBoxPage1Header.Name = "radTextBoxPage1Header"
			Me.radTextBoxPage1Header.Size = New Size(271, 20)
			Me.radTextBoxPage1Header.TabIndex = 4
			Me.radTextBoxPage1Header.TabStop = False
			Me.radTextBoxPage1Header.Text = "Short description"
			' 
			' panel4
			' 
			Me.panel4.BackColor = Color.White
			Me.panel4.Controls.Add(Me.radButtonPage2Reset)
			Me.panel4.Controls.Add(Me.radGridViewPage2)
			Me.panel4.Location = New Point(0, 81)
			Me.panel4.Name = "panel4"
			Me.panel4.Size = New Size(554, 273)
			Me.panel4.TabIndex = 3
			' 
			' radButtonPage2Reset
			' 
			Me.radButtonPage2Reset.Location = New Point(38, 236)
			Me.radButtonPage2Reset.Name = "radButtonPage2Reset"
			Me.radButtonPage2Reset.Size = New Size(100, 24)
			Me.radButtonPage2Reset.TabIndex = 25
			Me.radButtonPage2Reset.Text = "Reset"
			' 
			' radGridViewPage2
			' 
			Me.radGridViewPage2.Location = New Point(38, 18)
			' 
			' 
			' 
			Me.radGridViewPage2.MasterTemplate.AllowAddNewRow = False
			Me.radGridViewPage2.MasterTemplate.AllowDeleteRow = False
			Me.radGridViewPage2.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill
			gridViewTextBoxColumn1.HeaderText = "Command button"
			gridViewTextBoxColumn1.Name = "CommandButtonColumn"
			gridViewTextBoxColumn1.ReadOnly = True
			gridViewTextBoxColumn1.Width = 109
			gridViewCheckBoxColumn1.HeaderText = "Visible"
			gridViewCheckBoxColumn1.Name = "VisibleColumn"
			gridViewCheckBoxColumn1.Width = 109
			gridViewCheckBoxColumn2.HeaderText = "Enabled"
			gridViewCheckBoxColumn2.Name = "EnabledColumn"
			gridViewCheckBoxColumn2.Width = 111
			Me.radGridViewPage2.MasterTemplate.Columns.AddRange(New Telerik.WinControls.UI.GridViewDataColumn() { gridViewTextBoxColumn1, gridViewCheckBoxColumn1, gridViewCheckBoxColumn2})
			Me.radGridViewPage2.MasterTemplate.EnableGrouping = False
			Me.radGridViewPage2.Name = "radGridViewPage2"
			Me.radGridViewPage2.Size = New Size(347, 174)
			Me.radGridViewPage2.TabIndex = 5
			Me.radGridViewPage2.Text = "radGridView1"
			' 
			' panel5
			' 
			Me.panel5.BackColor = Color.White
			Me.panel5.Controls.Add(Me.radRadioButtonCompletionPage2)
			Me.panel5.Controls.Add(Me.radRadioButtonCompletionPage1)
			Me.panel5.Controls.Add(Me.radLabelPage3)
			Me.panel5.Location = New Point(0, 81)
			Me.panel5.Name = "panel5"
			Me.panel5.Size = New Size(554, 273)
			Me.panel5.TabIndex = 4
			' 
			' radRadioButtonCompletionPage2
			' 
			Me.radRadioButtonCompletionPage2.Location = New Point(45, 109)
			Me.radRadioButtonCompletionPage2.Name = "radRadioButtonCompletionPage2"
			Me.radRadioButtonCompletionPage2.Size = New Size(313, 18)
			Me.radRadioButtonCompletionPage2.TabIndex = 9
			Me.radRadioButtonCompletionPage2.Text = "Completion page 2 (WizardPage)"
			' 
			' radRadioButtonCompletionPage1
			' 
			Me.radRadioButtonCompletionPage1.Location = New Point(45, 85)
			Me.radRadioButtonCompletionPage1.Name = "radRadioButtonCompletionPage1"
			Me.radRadioButtonCompletionPage1.Size = New Size(313, 18)
			Me.radRadioButtonCompletionPage1.TabIndex = 8
			Me.radRadioButtonCompletionPage1.TabStop = True
			Me.radRadioButtonCompletionPage1.Text = "Completion page 1 (WizardCompletionPage)"
			Me.radRadioButtonCompletionPage1.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
			' 
			' radLabelPage3
			' 
			Me.radLabelPage3.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radLabelPage3.AutoSize = False
			Me.radLabelPage3.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelPage3.Location = New Point(45, 49)
			Me.radLabelPage3.Name = "radLabelPage3"
			Me.radLabelPage3.Size = New Size(490, 30)
			Me.radLabelPage3.TabIndex = 7
			Me.radLabelPage3.Text = "Choose the next page."
			' 
			' panel6
			' 
			Me.panel6.BackColor = Color.White
			Me.panel6.Controls.Add(Me.radLabelFinal)
			Me.panel6.Location = New Point(0, 76)
			Me.panel6.Name = "panel6"
			Me.panel6.Size = New Size(554, 278)
			Me.panel6.TabIndex = 5
			' 
			' radLabelFinal
			' 
			Me.radLabelFinal.Anchor = (CType(((AnchorStyles.Top Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles))
			Me.radLabelFinal.AutoSize = False
			Me.radLabelFinal.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, (CByte(0)))
			Me.radLabelFinal.Location = New Point(45, 49)
			Me.radLabelFinal.Name = "radLabelFinal"
			Me.radLabelFinal.Size = New Size(363, 30)
			Me.radLabelFinal.TabIndex = 9
			Me.radLabelFinal.Text = "Thank you for your interest in RadWizard control!"
			' 
			' wizardWelcomePage1
			' 
			Me.wizardWelcomePage1.ContentArea = Me.panel1
			Me.wizardWelcomePage1.Header = "Welcome page header"
			Me.wizardWelcomePage1.Name = "wizardWelcomePage1"
			Me.wizardWelcomePage1.Title = "RadWizard Control Preview"
			Me.wizardWelcomePage1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' wizardPage1
			' 
			Me.wizardPage1.ContentArea = Me.panel2
			Me.wizardPage1.Header = "Short description"
			Me.wizardPage1.Name = "wizardPage1"
			Me.wizardPage1.Title = "Page Title"
			Me.wizardPage1.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' wizardPage2
			' 
			Me.wizardPage2.ContentArea = Me.panel4
			Me.wizardPage2.Header = "Command area customization"
			Me.wizardPage2.Name = "wizardPage2"
			Me.wizardPage2.Title = "Customization options"
			Me.wizardPage2.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' wizardPage3
			' 
			Me.wizardPage3.ContentArea = Me.panel5
			Me.wizardPage3.CustomizePageHeader = False
			Me.wizardPage3.Header = "Page navigation customization"
			Me.wizardPage3.HeaderVisibility = Telerik.WinControls.ElementVisibility.Visible
			Me.wizardPage3.Name = "wizardPage3"
			Me.wizardPage3.Title = "Customization options"
			Me.wizardPage3.TitleVisibility = Telerik.WinControls.ElementVisibility.Visible
			Me.wizardPage3.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' wizardPage4
			' 
			Me.wizardPage4.ContentArea = Me.panel6
			Me.wizardPage4.Header = "Second option of completion page"
			Me.wizardPage4.Name = "wizardPage4"
			Me.wizardPage4.Title = "Final page"
			Me.wizardPage4.Visibility = Telerik.WinControls.ElementVisibility.Visible
			' 
			' panel0
			' 
			Me.panel0.Controls.Add(Me.pictureBox1)
			Me.panel0.Controls.Add(Me.radWizard1)
			Me.panel0.Location = New Point(0, 0)
			Me.panel0.Name = "panel0"
			Me.panel0.Size = New Size(554, 432)
			Me.panel0.TabIndex = 0
			' 
			' pictureBox1
			' 
			Me.pictureBox1.Image = My.Resources.WizardFirstLookTitle
			Me.pictureBox1.Location = New Point(-1, 0)
			Me.pictureBox1.Name = "pictureBox1"
			Me.pictureBox1.Size = New Size(555, 28)
			Me.pictureBox1.TabIndex = 2
			Me.pictureBox1.TabStop = False
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New SizeF(6F, 13F)
			Me.AutoScaleMode = AutoScaleMode.Font
			Me.Controls.Add(Me.panel0)
			Me.Name = "Form1"
			Me.Size = New Size(1028, 762)
			Me.Controls.SetChildIndex(Me.panel0, 0)
			Me.Controls.SetChildIndex(Me.settingsPanel, 0)
			CType(Me.settingsPanel, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.btnSpy, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radWizard1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.radWizard1.ResumeLayout(False)
			Me.panel3.ResumeLayout(False)
			CType(Me.radLabelCompletion, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel1.ResumeLayout(False)
			Me.panel1.PerformLayout()
			CType(Me.radButtonWelcomeReset, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxWelcomeImage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonWelcome, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonWelcomeBackColor, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelWelcomeImageBackColor, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radDropDownListWelcome, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelWelcomeImageLayout, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonWelcomeBrowse, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelWelcomeImage, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelWelcomeDescription, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelWelcome, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel2.ResumeLayout(False)
			Me.panel2.PerformLayout()
			CType(Me.radDropDownListIconAlignment, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPage1Reset, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelPage1IconAlignment, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelPage1Title, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radButtonPage1Icon, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBoxPage1Title, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelPage1Icon, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxPage1Title, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radCheckBoxPage1Header, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelPage1Header, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radTextBoxPage1Header, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel4.ResumeLayout(False)
			CType(Me.radButtonPage2Reset, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridViewPage2.MasterTemplate, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radGridViewPage2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel5.ResumeLayout(False)
			Me.panel5.PerformLayout()
			CType(Me.radRadioButtonCompletionPage2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radRadioButtonCompletionPage1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.radLabelPage3, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel6.ResumeLayout(False)
			CType(Me.radLabelFinal, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panel0.ResumeLayout(False)
			CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private radWizard1 As Telerik.WinControls.UI.RadWizard
		Private wizardCompletionPage1 As Telerik.WinControls.UI.WizardCompletionPage
		Private panel3 As Panel
		Private panel1 As Panel
		Private panel2 As Panel
		Private wizardWelcomePage1 As Telerik.WinControls.UI.WizardWelcomePage
		Private wizardPage1 As Telerik.WinControls.UI.WizardPage
		Private radLabelWelcome As Telerik.WinControls.UI.RadLabel
		Private radButtonPage1Icon As Telerik.WinControls.UI.RadButton
		Private radLabelPage1Icon As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxPage1Header As Telerik.WinControls.UI.RadCheckBox
		Private radTextBoxPage1Header As Telerik.WinControls.UI.RadTextBox
		Private radLabelPage1Header As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxPage1Title As Telerik.WinControls.UI.RadCheckBox
		Private radTextBoxPage1Title As Telerik.WinControls.UI.RadTextBox
		Private radLabelPage1Title As Telerik.WinControls.UI.RadLabel
		Private radLabelWelcomeDescription As Telerik.WinControls.UI.RadLabel
		Private radLabelPage1IconAlignment As Telerik.WinControls.UI.RadLabel
		Private radCheckBoxWelcomeImage As Telerik.WinControls.UI.RadCheckBox
		Private panel4 As Panel
		Private wizardPage2 As Telerik.WinControls.UI.WizardPage
		Private radGridViewPage2 As Telerik.WinControls.UI.RadGridView
		Private panel5 As Panel
		Private wizardPage3 As Telerik.WinControls.UI.WizardPage
		Private panel0 As Panel
		Private btnSpy As Telerik.WinControls.UI.RadButton
		Private radButtonWelcome As Telerik.WinControls.UI.RadButton
		Private radButtonWelcomeBackColor As Telerik.WinControls.UI.RadButton
		Private radLabelWelcomeImageBackColor As Telerik.WinControls.UI.RadLabel
		Private radDropDownListWelcome As Telerik.WinControls.UI.RadDropDownList
		Private radLabelWelcomeImageLayout As Telerik.WinControls.UI.RadLabel
		Private radButtonWelcomeBrowse As Telerik.WinControls.UI.RadButton
		Private radLabelWelcomeImage As Telerik.WinControls.UI.RadLabel
		Private radButtonWelcomeReset As Telerik.WinControls.UI.RadButton
		Private radButtonPage1Reset As Telerik.WinControls.UI.RadButton
		Private radDropDownListIconAlignment As Telerik.WinControls.UI.RadDropDownList
		Private radButtonPage2Reset As Telerik.WinControls.UI.RadButton
		Private radLabelPage3 As Telerik.WinControls.UI.RadLabel
		Private radLabelCompletion As Telerik.WinControls.UI.RadLabel
		Private panel6 As Panel
		Private wizardPage4 As Telerik.WinControls.UI.WizardPage
		Private radRadioButtonCompletionPage2 As Telerik.WinControls.UI.RadRadioButton
		Private radRadioButtonCompletionPage1 As Telerik.WinControls.UI.RadRadioButton
		Private radLabelFinal As Telerik.WinControls.UI.RadLabel
		Private pictureBox1 As PictureBox
	End Class
End Namespace
