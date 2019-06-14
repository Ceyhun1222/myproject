Imports System.ComponentModel
Imports System.Reflection
Imports System.IO
Imports Telerik.QuickStart.WinControls
Imports Telerik.WinControls

Namespace Telerik.Examples.WinControls.Forms.AboutBox
	Partial Friend Class RadAboutBox1
		Inherits ExamplesRadForm
		Private executingAssembly As System.Reflection.Assembly

		Public Sub New()
			InitializeComponent()

			'  Initialize the AboutBox to display the product information from the assembly information.
			'  Change assembly information settings for your application through either:
			'  - Project->Properties->Application->Assembly Information
			'  - AssemblyInfo.cs
			Me.Text = String.Format("About {0}", AssemblyTitle)
			Me.radLabelProductName.Text = AssemblyProduct
			Me.radLabelVersion.Text = String.Format("Version {0}", AssemblyVersion)
            Me.radLabelCopyright.Text = AssemblyCopyright.Substring(0, AssemblyCopyright.IndexOf("."))
			Me.radLabelCompanyName.Text = AssemblyCompany
            Me.radTextBoxDescription.Text = "Telerik UI for WinForms includes over 110 UI controls that you can " & "use to easily build unique and visually stunning Line of Business applications. " & "Boasting a well-designed architecture and CAB support, RadControls are perfectly " & "suited for your Enterprise desktop development."
			Me.logoPictureBox.Image = Me.GetAboutBoxImage()

			AddHandler ThemeNameChanged, AddressOf RadAboutBox1_ThemeNameChanged
		End Sub

		Protected Overrides Overloads Sub OnLoad(ByVal e As EventArgs)
			Dim themeName As String = Me.ThemeName

			MyBase.OnLoad(e)

			ThemeResolutionService.ApplyThemeToControlTree(Me, themeName)
		End Sub

		Private Sub RadAboutBox1_ThemeNameChanged(ByVal source As Object, ByVal args As Telerik.WinControls.ThemeNameChangedEventArgs)
			'String themeName = this.ThemeName;
			'this.radLabelProductName.ThemeName = themeName;
			'this.radLabelCompanyName.ThemeName = themeName;
			'this.radLabelCopyright.ThemeName = themeName;
			'this.radLabelVersion.ThemeName = themeName;
			'this.radTextBoxDescription.ThemeName = themeName;
			'this.okRadButton.ThemeName = themeName;
		End Sub

		Private Function GetAboutBoxImage() As Image
			Dim img As Image
			Dim imageStream As Stream
			Dim imageFileName As String = "AboutBox.png"

			If executingAssembly Is Nothing Then
				Me.executingAssembly = System.Reflection.Assembly.GetExecutingAssembly()
			End If

            imageStream = Me.executingAssembly.GetManifestResourceStream(imageFileName)
			img = Bitmap.FromStream(imageStream)

			If img Is Nothing Then
				imageStream = Me.executingAssembly.GetManifestResourceStream(String.Format(imageFileName))
				img = Bitmap.FromStream(imageStream)
			End If

			Return img
		End Function



		#Region "Assembly Attribute Accessors"

		Public ReadOnly Property AssemblyTitle() As String
			Get
				' Get all Title attributes on this assembly
				Dim attributes() As Object = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(AssemblyTitleAttribute), False)
				' If there is at least one Title attribute
				If attributes.Length > 0 Then
					' Select the first one
					Dim titleAttribute As AssemblyTitleAttribute = CType(attributes(0), AssemblyTitleAttribute)
					' If it is not an empty string, return it
					If titleAttribute.Title <> "" Then
						Return titleAttribute.Title
					End If
				End If
				' If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				Return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
			End Get
		End Property

		Public ReadOnly Property AssemblyVersion() As String
			Get
				Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
			End Get
		End Property

		Public ReadOnly Property AssemblyDescription() As String
			Get
				' Get all Description attributes on this assembly
				Dim attributes() As Object = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(AssemblyDescriptionAttribute), False)
				' If there aren't any Description attributes, return an empty string
				If attributes.Length = 0 Then
					Return ""
				End If
				' If there is a Description attribute, return its value
				Return (CType(attributes(0), AssemblyDescriptionAttribute)).Description
			End Get
		End Property

		Public ReadOnly Property AssemblyProduct() As String
			Get
				' Get all Product attributes on this assembly
				Dim attributes() As Object = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(AssemblyProductAttribute), False)
				' If there aren't any Product attributes, return an empty string
				If attributes.Length = 0 Then
					Return ""
				End If
				' If there is a Product attribute, return its value
				Return (CType(attributes(0), AssemblyProductAttribute)).Product
			End Get
		End Property

		Public ReadOnly Property AssemblyCopyright() As String
            Get
                Return "Copyright © 2016 Progress Software Corporation. All rights Reserved."

                ' Get all Copyright attributes on this assembly
                'Dim attributes() As Object = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(AssemblyCopyrightAttribute), False)
                '' If there aren't any Copyright attributes, return an empty string
                'If attributes.Length = 0 Then
                '    Return ""
                'End If
                '' If there is a Copyright attribute, return its value
                'Return (CType(attributes(0), AssemblyCopyrightAttribute)).Copyright
            End Get
		End Property

		Public ReadOnly Property AssemblyCompany() As String
            Get
                Return "Progress"

                '' Get all Company attributes on this assembly
                'Dim attributes() As Object = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(GetType(AssemblyCompanyAttribute), False)
                '' If there aren't any Company attributes, return an empty string
                'If attributes.Length = 0 Then
                '    Return ""
                'End If
                '' If there is a Company attribute, return its value
                'Return (CType(attributes(0), AssemblyCompanyAttribute)).Company
            End Get
		End Property
		#End Region
	End Class
End Namespace
