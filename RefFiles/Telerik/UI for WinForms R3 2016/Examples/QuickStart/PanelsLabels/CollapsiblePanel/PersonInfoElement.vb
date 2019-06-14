Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports Telerik.WinControls
Imports Telerik.WinControls.Layouts
Imports Telerik.WinControls.UI

Namespace Telerik.Examples.WinControls.PanelsLabels.CollapsiblePanel
    Public Class PersonInfoElement
        Inherits LightVisualElement
        Private mainContainer As New DockLayoutPanel()

        Private m_imageElement As LightVisualElement

        Private m_nameElement As LightVisualElement
        Private m_emailElement As LightVisualElement
        Private m_phoneElement As LightVisualElement
        Private infoStack As StackLayoutPanel

        Public ReadOnly Property ImageElement() As LightVisualElement
            Get
                Return Me.m_imageElement
            End Get
        End Property

        Public ReadOnly Property NameElement() As LightVisualElement
            Get
                Return Me.m_nameElement
            End Get
        End Property

        Public ReadOnly Property EmailElement() As LightVisualElement
            Get
                Return Me.m_emailElement
            End Get
        End Property

        Public ReadOnly Property PhoneElement() As LightVisualElement
            Get
                Return Me.m_phoneElement
            End Get
        End Property

        Protected Overrides Sub InitializeFields()
            MyBase.InitializeFields()

            Me.Shape = New RoundRectShape(5)
            Me.DrawBorder = True
            Me.BorderColor = Color.FromArgb(197, 208, 222)

            Me.Padding = New System.Windows.Forms.Padding(10)
        End Sub

        Protected Overrides Sub CreateChildElements()
            MyBase.CreateChildElements()

            Me.mainContainer = New DockLayoutPanel()
            Me.mainContainer.LastChildFill = True
            Me.Children.Add(Me.mainContainer)

            Me.m_imageElement = New LightVisualElement()
            Me.m_imageElement.DrawBorder = True
            Me.m_imageElement.BorderColor = Color.FromArgb(224, 224, 224)
            Me.mainContainer.Children.Add(m_imageElement)
            DockLayoutPanel.SetDock(Me.m_imageElement, Dock.Left)

            Me.infoStack = New StackLayoutPanel()
            Me.infoStack.Orientation = System.Windows.Forms.Orientation.Vertical
            Me.mainContainer.Children.Add(Me.infoStack)

            Me.m_nameElement = New LightVisualElement()
            Me.m_nameElement.Font = New Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
            Me.m_nameElement.ForeColor = Color.FromArgb(0, 153, 204)
            Me.m_nameElement.Text = "Name"
            Me.infoStack.Children.Add(Me.m_nameElement)

            Me.m_emailElement = New LightVisualElement()
            Me.m_emailElement.Text = "Email@email.com"
            Me.infoStack.Children.Add(Me.m_emailElement)

            Me.m_phoneElement = New LightVisualElement()
            Me.m_phoneElement.Text = "0-381295031"
            Me.infoStack.Children.Add(Me.m_phoneElement)
        End Sub
    End Class
End Namespace
