Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.ADF.CATIDs
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.ArcMap
Imports ESRI.ArcGIS.esriSystem



Public Interface ISigmaCallout
    Property Color() As IColor
    Property LeaderSymbol() As ISimpleLineSymbol
    Property Snap() As Integer
    Property Size() As Double
    Property DME() As Boolean
    Property TopMargine() As Double
    Property LeftMargine() As Double
    Property RightMargine() As Double
    Property BottomMargine() As Double
End Interface

<ProgId(SigmaCallout.GUID), Guid("B29E3524-D937-493A-9615-EB01481CBE65"), ComVisible(True)> Public Class SigmaCallout
    Implements IDisplayName
    Implements ICallout
    Implements ITextBackground
    Implements IPersistVariant
    Implements IQueryGeometry
    Implements IClone
    Implements ISigmaCallout
    'Implements IPropertySupport

#Region "COM Registration Function(s)"

    'Property _LeaderSymbol As SimpleLineSymbol

    Private Property pSpatRefGeo As ISpatialReference

    <ComRegisterFunction(), ComVisible(False)> _
    Private Shared Sub RegisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryRegistration(registerType)

        '
        ' TODO: Add any COM registration code here
        '
    End Sub

    <ComUnregisterFunction(), ComVisible(False)> _
    Private Shared Sub UnregisterFunction(ByVal registerType As Type)
        ' Required for ArcGIS Component Category Registrar support
        ArcGISCategoryUnregistration(registerType)

        '
        ' TODO: Add any COM unregistration code here
        '
    End Sub

#Region "ArcGIS Component Category Registrar generated code"
    ''' <summary>
    ''' Required method for ArcGIS Component Category registration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryRegistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        TextBackground.Register(regKey)

    End Sub
    ''' <summary>
    ''' Required method for ArcGIS Component Category unregistration -
    ''' Do not modify the contents of this method with the code editor.
    ''' </summary>
    Private Shared Sub ArcGISCategoryUnregistration(ByVal registerType As Type)
        Dim regKey As String = String.Format("HKEY_CLASSES_ROOT\CLSID\{{{0}}}", registerType.GUID)
        TextBackground.Unregister(regKey)
    End Sub

#End Region
#End Region

    Public Const GUID As String = "RISK.SigmaCallout"
    Private Const _Version As Integer = 1
    Private Const _DisplayName As String = "Sigma Callout"

    'These members are controlled by properties
    Private _textBox As IEnvelope
    Private _anchorPoint As IPoint
    Private _geometry As IGeometry
    Private _fillSymbol As ISimpleFillSymbol
    Private _textBoxCenterPt As IPoint
    Private _textSym As ITextSymbol
    Private _strLines() As String


    Private _color As IColor
    Private _leaderSymbol As ISimpleLineSymbol
    Private _snap As Integer
    Private _dLeaderTolerance As Double
    Private _dSize As Double
    Private _dme As Boolean


    Private _dTopMargine As Double
    Private _dLeftMargine As Double
    Private _dRightMargine As Double
    Private _dBottomtMargine As Double

    Private Sub Initialize()
        _fillSymbol = New SimpleFillSymbol
        _leaderSymbol = New SimpleLineSymbol

        _dSize = 1  'default size
        _dLeftMargine = 1 'default LeftMargine
        _dRightMargine = 1
        _dTopMargine = 1
        _dBottomtMargine = 1

        Dim pRgbColor As IRgbColor = New RgbColor
        pRgbColor.Red = 255
        pRgbColor.Green = 255
        _color = pRgbColor
        _fillSymbol.Color = _color
        _snap = 0
        _dme = False
        'm_pFillSymbol.Outline.Color = pRgbColor
        'm_pFillSymbol.Outline.Width = 0.5


    End Sub

    Public Sub New()
        MyBase.New()
        Initialize()
    End Sub

    Private Sub Terminate()
        '  Which variables do we really need to dereference here, just the anchorPoint,
        '  or any other globals as well??

        _anchorPoint = Nothing
        _geometry = Nothing

        _textBox = Nothing
        _fillSymbol = Nothing
        _textBoxCenterPt = Nothing
        _leaderSymbol = Nothing
        _textSym = Nothing
        _color = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        Terminate()
    End Sub

#Region "IClone"

    Private Sub Assign(ByVal src As IClone) Implements IClone.Assign
        '1. Make sure src is pointing to a valid object.
        If src Is Nothing Then
            Throw New Runtime.InteropServices.COMException("Invalid object.")
        End If

        '2. Verify the type of src.
        If Not (TypeOf src Is RISKCallouts.SigmaCallout) Then
            Throw New Runtime.InteropServices.COMException("Bad object type.")
        End If

        '3. Assign the properties of src to the current instance.
        Dim srcClonable As RISKCallouts.SigmaCallout = CType(src, RISKCallouts.SigmaCallout)

        _anchorPoint = srcClonable.AnchorPoint
        _color = srcClonable.Color
        _leaderSymbol = srcClonable.LeaderSymbol
        _snap = srcClonable.Snap
        _dSize = srcClonable.Size
        _dme = srcClonable.DME
        _dLeaderTolerance = srcClonable.LeaderTolerance
        _dTopMargine = srcClonable.TopMargine
        _dLeftMargine = srcClonable.LeftMargine
        _dRightMargine = srcClonable.RightMargine
        _dBottomtMargine = srcClonable.BottomMargine
    End Sub

    Private Function Clone() As IClone Implements IClone.Clone
        Dim obj As RISKCallouts.SigmaCallout = New RISKCallouts.SigmaCallout()
        obj.Assign(Me)

        Return CType(obj, IClone)
    End Function

    Private Function IsEqual(ByVal other As IClone) As Boolean Implements IClone.IsEqual
        '1. Make sure that "other" is pointing to a valid object.
        If Nothing Is other Then
            Throw New COMException("Invalid object.")
        End If

        '2. Verify the type of "other."
        If Not (TypeOf other Is RISKCallouts.SigmaCallout) Then
            Throw New COMException("Bad object type.")
        End If

        Dim otherClonable As RISKCallouts.SigmaCallout = CType(other, RISKCallouts.SigmaCallout)

        'Test that all the object's properties are the same.

        If otherClonable.Version = _Version AndAlso _
         otherClonable.Snap = _snap AndAlso _
         otherClonable.DME = _dme AndAlso _
         otherClonable.NameString = NameString AndAlso _
         otherClonable.LeaderTolerance = _dLeaderTolerance AndAlso _
         otherClonable.Size = _dSize AndAlso _
         otherClonable.TopMargine = _dTopMargine AndAlso _
         otherClonable.LeftMargine = _dLeftMargine AndAlso _
         _dRightMargine = otherClonable.RightMargine AndAlso _
         _dBottomtMargine = otherClonable.BottomMargine AndAlso _
         CType(otherClonable.AnchorPoint, IClone).IsEqual(CType(_anchorPoint, IClone)) AndAlso _
         CType(otherClonable.LeaderSymbol, IClone).IsEqual(CType(_leaderSymbol, IClone)) AndAlso _
         CType(otherClonable.Color, IClone).IsEqual(CType(_color, IClone)) Then
            Return True
        End If

        Return False
    End Function

    Private Function IsIdentical(ByVal other As IClone) As Boolean Implements IClone.IsIdentical
        If Nothing Is other Then
            Throw New COMException("Invalid object.")
        End If

        'Verify the type of other.
        If Not (TypeOf other Is RISKCallouts.SigmaCallout) Then
            Throw New COMException("Bad object type.")
        End If

        'Test if the other is this.
        If CType(other, RISKCallouts.SigmaCallout) Is Me Then
            Return True
        End If

        Return False
    End Function

#End Region

#Region "ICallout"
    Private Property AnchorPoint() As IPoint Implements ICallout.AnchorPoint


        Get
            Dim pClone As IClone
            If _anchorPoint Is Nothing Then Return Nothing
            'If _anchorPoint Is Nothing Then Exit Property

            'Else, pass a reference to a new cloned anchor point
            pClone = _anchorPoint

            Return pClone.Clone
        End Get

        Set(ByVal Value As IPoint)
            'Check if they are the same and if so, do nothing
            If _anchorPoint Is Value Then Return

            If Value Is Nothing Then
                _anchorPoint = Nothing
                Exit Property
            End If

            If _anchorPoint Is Nothing Then _anchorPoint = New Point

            _anchorPoint.PutCoords(Value.X, Value.Y)

            'Dim p1 As IPoint = New Point ' Point
            'Dim x As Double
            'Dim y As Double
            'Dim pLidLine As IPointCollection = New Polyline

            'x = _anchorPoint.X
            'y = _anchorPoint.Y

            'p1.X = x
            'p1.Y = y

            'Dim pSpatRefFact As ISpatialReferenceFactory2 = New SpatialReferenceEnvironment
            'Dim pGCS As IGeographicCoordinateSystem = pSpatRefFact.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984) 'New ESRI.ArcGIS.Geometry.GeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984)
            'Dim pApplication As IApplication

            'If Value.Y < 90.0 Then
            '    Dim k As Integer
            '    Dim appRot As IAppROT = New AppROT()
            '    For k = 0 To appRot.Count - 1
            '        pApplication = appRot.Item(k)
            '        Console.WriteLine(pApplication.Caption)
            '    Next k

            '    Dim mxDocument As IMxDocument = pApplication.Document
            '    Dim activeView As IActiveView = mxDocument.ActiveView
            '    Dim pMap As IMap = activeView.FocusMap
            '    Dim pSpatRefPrj As ISpatialReference = New ProjectedCoordinateSystem
            '    pSpatRefPrj = pMap.SpatialReference
            '    p1.SpatialReference = pGCS
            '    p1.Project(pSpatRefPrj)
            '    _anchorPoint.PutCoords(p1.X, p1.Y)
            'End If

        End Set

    End Property

    Public Property LeaderTolerance() As Double Implements ICallout.LeaderTolerance
        Get
            Return _dLeaderTolerance
        End Get

        Set(ByVal Value As Double)
            'Distance from point to text.  The number is expected to be in points.
            'If actual distance is less than tolerance, the leader is not drawn.
            _dLeaderTolerance = Value
        End Set
    End Property

#End Region

#Region "ITextBackground"
    Private Property TextSymbol() As ITextSymbol Implements ITextBackground.TextSymbol  '///////////////////////////////////////////////////////////////////
        Get
            Return _textSym
        End Get

        Set(ByVal Value As ITextSymbol)
            If Value Is Nothing Then
                _textSym = Nothing
                Return
            End If

            _textSym = Value

            Dim pParsSup As ITextParserSupport
            Dim hasTags As Boolean

            pParsSup = _textSym
            pParsSup.TextParser.HasTags(hasTags)
            If Not hasTags Then Return

            _strLines = GetStringRows(_textSym.Text)
        End Set
    End Property

    Private WriteOnly Property TextBox() As IEnvelope Implements ITextBackground.TextBox
        Set(ByVal Value As IEnvelope)
            'If _textBox Is Value Then Exit Property 'same, do nothing
            _textBox = Value

            'When they set the text box, let's create the geometry for out symbol used by Draw
            _textBoxCenterPt = New Point
            _textBoxCenterPt.X = 0.5 * (_textBox.XMin + _textBox.XMax)
            _textBoxCenterPt.Y = 0.5 * (_textBox.YMin + _textBox.YMax)
        End Set
    End Property

    Private Sub QueryBoundary(ByVal hDC As Integer, ByVal transform As ITransformation, ByVal Boundary As IPolygon) Implements ITextBackground.QueryBoundary
        'Forward the call down do the symbol
        'This will populate Boundary with a polygon based on the callout envelope
        If _textSym Is Nothing Then Return

        Dim pSymbol As ISymbol = _leaderSymbol
        Dim geom As IGeometry = CreateGeometry(_textBoxCenterPt, hDC, transform)
        pSymbol.QueryBoundary(hDC, transform, geom.Envelope, Boundary)

        'pSymbol.QueryBoundary(hDC, transform, CreateGeometry(_textBoxCenterPt, pDisplayTransform).Envelope, Boundary)

        Dim pTopoOp As ITopologicalOperator2 = Boundary
        pTopoOp.IsKnownSimple_2 = False
        pTopoOp.Simplify()              'Make sure it's simple

        Dim pDisplayTransform As IDisplayTransformation = transform
        Dim dBufferSize As Double = pDisplayTransform.FromPoints(1)
        Dim pBufferedLine As IPolygon
        Dim pNewBoundary As IPolygon = Boundary

        'Create a polygon buffer around the leader line

        'pTopoOp = CreateLeader(geom, pDisplayTransform) ', pDisplayTransform
        'If Not (pTopoOp Is Nothing) Then
        '    pBufferedLine = pTopoOp.Buffer(dBufferSize)
        '    pTopoOp = pBufferedLine
        '    pTopoOp.IsKnownSimple_2 = False
        '    pTopoOp.Simplify()

        '    'Union the buffered leader with the circle geometry
        '    'to create the final shape that needs refreshing
        '    'pTopoOp = pNewBoundary

        '    pNewBoundary = pTopoOp.Union(pNewBoundary)
        '    pTopoOp = pNewBoundary
        '    pTopoOp.IsKnownSimple_2 = False
        '    pTopoOp.Simplify()
        '    'Boundary = pNewBoundary
        'End If

        'Don't want to pass back a different Boundary reference
        'Set our new geometry into the passed in Boundary reference - performance!
        Dim pClone As IClone = Boundary
        pClone.Assign(pNewBoundary)
    End Sub

    Private Sub Draw(ByVal hDC As Integer, ByVal transform As ITransformation) Implements ITextBackground.Draw




        If _textSym Is Nothing Then Return

        Dim pDisplayTransform As IDisplayTransformation = transform

        _geometry = CreateGeometry(_textBoxCenterPt, hDC, pDisplayTransform)
        If _geometry Is Nothing Then Exit Sub

        '=============================================================================
        Dim pSymbol As ISymbol
        Dim geom As IGeometryCollection = _geometry
        '===================Draw backgraund ==========================================================
        _fillSymbol.Color = _color 'use the color property
        _fillSymbol.Outline = Nothing

        pSymbol = _fillSymbol
        pSymbol.SetupDC(hDC, transform)
        pSymbol.Draw(geom.Geometry(0))
        pSymbol.ResetDC()

        '===================Draw lines ==========================================================
        Dim OutlineColor As IColor
        OutlineColor = New RgbColor
        OutlineColor.RGB = RGB(0, 0, 0)

        ' _leaderSymbol.Color = OutlineColor
        ' _leaderSymbol.Width = 1
        pSymbol = _leaderSymbol
        pSymbol.SetupDC(hDC, transform)

        If Not geom.Geometry(1) Is Nothing Then pSymbol.Draw(geom.Geometry(1))
        If Not geom.Geometry(2) Is Nothing Then pSymbol.Draw(geom.Geometry(2))
        pSymbol.ResetDC()


        Dim pPolyline As IPointCollection = New Polyline
        pPolyline = geom.Geometry(3)

        Dim p1 As IPoint = pPolyline.Point(0) ' Point
        Dim p2 As IPoint = pPolyline.Point(1)
        Dim y As Double
        Dim pLidLine As IPointCollection = New Polyline

        'Dim x As Double
        'Dim y As Double

        'pDisplayTransform.FromMapPoint(_anchorPoint, x, y)
        'p2 = pDisplayTransform.ToMapPoint(x, y)
        y = p2.Y


        Dim pSpatRefFact As ISpatialReferenceFactory2 = New SpatialReferenceEnvironment
        Dim pGCS As IGeographicCoordinateSystem = pSpatRefFact.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984) 'New ESRI.ArcGIS.Geometry.GeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984)
        Dim pApplication As IApplication

        If Math.Abs(y) < 90.0 Then
            Dim i As Integer
            Dim appRot As IAppROT = New AppROT()
            For i = 0 To appRot.Count - 1
                pApplication = appRot.Item(i)
                Console.WriteLine(pApplication.Caption)
            Next i

            'Dim doc As IDocument = ESRI.ArcGIS.ArcMap.Applicationclass()

            'Dim pApp As IApplication
            'pApp = doc.Parent
            ' Dim mxDocument As IMxDocument = TryCast(application.Document, IMxDocument)
            Dim mxDocument As IMxDocument = pApplication.Document
            ' Try the following different options to see how the code works in data or layout view.
            Dim activeView As IActiveView = mxDocument.ActiveView
            'Dim activeView As IActiveView = mxDocument.PageLayout
            'Dim activeView As IActiveView = mxDocument.FocusMap
            Dim pMap As IMap = activeView.FocusMap
            Dim pSpatRefPrj As ISpatialReference = New ProjectedCoordinateSystem
            pSpatRefPrj = pMap.SpatialReference

            'Dim s1 As String
            'Dim s2 As String
            's1 = pGCS.Name
            's2 = pSpatRefPrj.Name
            p2.SpatialReference = pGCS
            p2.Project(pSpatRefPrj)
            pLidLine.AddPoint(p1)
            pLidLine.AddPoint(p2)

            pSymbol.SetupDC(hDC, transform)
            If Not pLidLine Is Nothing Then pSymbol.Draw(pLidLine)
            pSymbol.ResetDC()
        Else
            pSymbol.SetupDC(hDC, transform)
            If Not geom.Geometry(3) Is Nothing Then pSymbol.Draw(geom.Geometry(3))
            pSymbol.ResetDC()
        End If


        'MarkerSymbol
        ' Define the color you want to use.
        Dim rgbColorCls As ESRI.ArcGIS.Display.IRgbColor = New RgbColor
        rgbColorCls.Red = 0
        rgbColorCls.Green = 160
        rgbColorCls.Blue = 255

        ' Define the font you want to use.
        Dim stdFontCls As stdole.IFontDisp = CType(New stdole.StdFont, stdole.IFontDisp)


        stdFontCls.Name = "ESRI Default Marker"
        stdFontCls.Size = 12


        ' Set the character marker symbol's properties.
        Dim characterMarkerSymbolCls As ESRI.ArcGIS.Display.ICharacterMarkerSymbol = New CharacterMarkerSymbol

        With characterMarkerSymbolCls
            'With tSym
            .Angle = _textSym.Angle
            .CharacterIndex = 34
            .Color = rgbColorCls
            .Font = stdFontCls
            .Size = 24
            '.XOffset = 0
            '.YOffset = 0
        End With

        Dim CharSym As ISymbol = New CharacterMarkerSymbol
        CharSym = characterMarkerSymbolCls
        CharSym.SetupDC(hDC, transform)
        If Not geom.Geometry(4) Is Nothing Then CharSym.Draw(geom.Geometry(4))
        CharSym.ResetDC()

        rgbColorCls.Red = 255
        rgbColorCls.Green = 255
        rgbColorCls.Blue = 255

        stdFontCls.Name = "Areal"
        ' stdFontCls.Size = 12

        With characterMarkerSymbolCls
            'With tSym
            .Angle = _textSym.Angle
            .CharacterIndex = 68
            .Color = rgbColorCls
            .Font = stdFontCls
            .Size = 16
            '.XOffset = 0
            '.YOffset = 0
        End With
        CharSym = characterMarkerSymbolCls
        CharSym.SetupDC(hDC, transform)
        If Not geom.Geometry(4) Is Nothing Then CharSym.Draw(geom.Geometry(4))
        CharSym.ResetDC()
    End Sub

   

#End Region

#Region "IQueryGeometry"
    Private Function GetGeometry(ByVal hDC As Integer, ByVal transform As ITransformation, ByVal drawGeometry As IGeometry) As IGeometry Implements IQueryGeometry.GetGeometry
        Return CreateGeometry(_textBoxCenterPt, hDC, transform)
    End Function

    Private Sub QueryEnvelope(ByVal hDC As Integer, ByVal transform As ITransformation, ByVal drawGeometry As IGeometry, ByVal Envelope As IEnvelope) Implements IQueryGeometry.QueryEnvelope
        Envelope = CreateGeometry(_textBoxCenterPt, hDC, transform).Envelope
    End Sub

#End Region

#Region "IDisplayName"
    Private ReadOnly Property NameString() As String Implements IDisplayName.NameString
        Get
            Return _DisplayName
        End Get
    End Property
#End Region

#Region "IPersistVariant"

    Private ReadOnly Property IPersistVariant_ID() As UID Implements IPersistVariant.ID
        Get
            Dim uid As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UID()
            uid.Value = SigmaCallout.GUID
            Return uid
        End Get
    End Property

    Private Sub IPersistVariant_Load(ByVal Stream As IVariantStream) Implements IPersistVariant.Load
        Dim vers As Integer

        vers = Convert.ToInt32(Stream.Read())
        If (vers <> 1) And (vers <> 2) Then
            Throw New Exception("Failed to read from stream")
        End If

        _anchorPoint = TryCast(Stream.Read(), IPoint)
        _dLeaderTolerance = Convert.ToDouble(Stream.Read())

        _color = TryCast(Stream.Read(), IColor)
        If vers = 1 Then
            _leaderSymbol = TryCast(Stream.Read(), ISimpleLineSymbol)
        End If

        _snap = Convert.ToInt32(Stream.Read())
        _dme = Convert.ToBoolean(Stream.Read())
        _dSize = Convert.ToDouble(Stream.Read())
        _dTopMargine = Convert.ToDouble(Stream.Read())
        _dLeftMargine = Convert.ToDouble(Stream.Read())
        _dRightMargine = Convert.ToDouble(Stream.Read())
        _dBottomtMargine = Convert.ToDouble(Stream.Read())
    End Sub

    Private Sub IPersistVariant_Save(ByVal Stream As IVariantStream) Implements IPersistVariant.Save
        Stream.Write(_Version)
        Stream.Write(_anchorPoint)
        Stream.Write(_dLeaderTolerance)

        Stream.Write(_color)
        Stream.Write(_leaderSymbol)
        Stream.Write(_snap)
        Stream.Write(_dme)
        Stream.Write(_dSize)
        Stream.Write(_dTopMargine)
        Stream.Write(_dLeftMargine)
        Stream.Write(_dRightMargine)
        Stream.Write(_dBottomtMargine)
    End Sub

#End Region

#Region "IPropertySupport Members"

    'Public Function Applies(ByVal pUnk As Object) As Boolean Implements ESRI.ArcGIS.esriSystem.IPropertySupport.Applies
    '	Dim c As IColor = TryCast(pUnk, IColor)
    '	Dim sigmLineCll As ISigmaLineCallout = TryCast(pUnk, ISigmaLineCallout)
    '	If Not Nothing Is c OrElse Not Nothing Is sigmLineCll Then
    '		Return True
    '	End If

    '	Return False
    'End Function

    'Public Function Apply(ByVal newObject As Object) As Object Implements ESRI.ArcGIS.esriSystem.IPropertySupport.Apply
    '	Dim oldObject As Object = Nothing

    '	Dim c As IColor = TryCast(newObject, IColor)
    '	If Not Nothing Is c Then
    '		oldObject = (CType(Me, IPropertySupport)).Current(newObject)
    '		CType(Me, IMarkerSymbol).Color = c

    '	End If

    '	Dim sigmLineCll As ISigmaLineCallout = TryCast(newObject, ISigmaLineCallout)
    '	oldObject = (CType(Me, IPropertySupport)).Current(newObject)
    '	Dim clonee As IClone = CType(newObject, IClone)
    '	CType(Me, IClone).Assign(clonee)

    '	Return oldObject
    'End Function

    'Public Function CanApply(ByVal pUnk As Object) As Boolean Implements ESRI.ArcGIS.esriSystem.IPropertySupport.CanApply
    '	Return (CType(Me, IPropertySupport)).Applies(pUnk)
    'End Function

    'Public ReadOnly Property Current(ByVal pUnk As Object) As Object Implements ESRI.ArcGIS.esriSystem.IPropertySupport.Current
    '	Get
    '		Dim c As IColor = TryCast(pUnk, IColor)
    '		If Not Nothing Is c Then
    '			Dim currentColor As IColor = (CType(Me, IMarkerSymbol)).Color
    '			Return CObj(currentColor)
    '		End If

    '		Dim sigmLineCll As ISigmaLineCallout = TryCast(pUnk, ISigmaLineCallout)

    '		Dim clonee As IClone = (CType(Me, IClone)).Clone()
    '		Return CObj(clonee)
    '	End Get
    'End Property

#End Region

#Region "Public Members on default interface"

    Public ReadOnly Property Version() As Integer
        Get
            Return _Version
        End Get
    End Property

    Public Property Color() As IColor Implements ISigmaCallout.Color
        Get
            Return _color
        End Get

        Set(ByVal Value As IColor)
            _color = Value
        End Set
    End Property
    Public Property LeaderSymbol() As ISimpleLineSymbol Implements ISigmaCallout.LeaderSymbol
        Get
            Return _leaderSymbol
        End Get

        Set(ByVal Value As ISimpleLineSymbol)
            _leaderSymbol = Value
        End Set
    End Property
    Public Property Snap() As Integer Implements ISigmaCallout.Snap
        Get
            Return _snap
        End Get

        Set(ByVal Value As Integer)
            _snap = Value
        End Set
    End Property
    Public Property DME() As Boolean Implements ISigmaCallout.DME
        Get
            Return _dme
        End Get

        Set(ByVal Value As Boolean)
            _dme = Value
        End Set
    End Property
    Public Property Size() As Double Implements ISigmaCallout.Size
        Get
            Return _dSize
        End Get

        Set(ByVal Value As Double)
            _dSize = Value
        End Set
    End Property

    Public Property TopMargine() As Double Implements ISigmaCallout.TopMargine
        Get
            Return _dTopMargine
        End Get

        Set(ByVal Value As Double)
            _dTopMargine = Value
        End Set
    End Property

    Public Property LeftMargine() As Double Implements ISigmaCallout.LeftMargine
        Get
            Return _dLeftMargine
        End Get

        Set(ByVal Value As Double)
            _dLeftMargine = Value
        End Set
    End Property

    Public Property RightMargine() As Double Implements ISigmaCallout.RightMargine
        Get
            Return _dRightMargine
        End Get

        Set(ByVal Value As Double)
            _dRightMargine = Value
        End Set
    End Property

    Public Property BottomMargine() As Double Implements ISigmaCallout.BottomMargine
        Get
            Return _dBottomtMargine
        End Get

        Set(ByVal Value As Double)
            _dBottomtMargine = Value
        End Set
    End Property

#End Region

#Region "Private implementation details"

    Private Function CreateGeometry(pPoint As IPoint, ByVal hDC As Integer, ByVal transform As ITransformation) As IGeometry

        Dim pDisplayTransformation As IDisplayTransformation = transform

        Dim units As esriUnits = pDisplayTransformation.Units
        Dim scasle As Double
        Dim scasleRatio As Double

        If units <> esriUnits.esriUnknownUnits Then
            scasle = pDisplayTransformation.ReferenceScale
            scasleRatio = pDisplayTransformation.ScaleRatio
        Else
            scasleRatio = 1.0
        End If

        Dim n As Integer
        Dim xsize0 As Double
        Dim ysize0 As Double
        Dim xsizeN As Double
        Dim ysizeN As Double

        '_textSym.GetTextSize(hDC, transform, _strLines(0), xsize0, ysize0)
        '_textSym.GetTextSize(hDC, transform, _strLines(n), xsizeN, ysizeN)
        '_textSym.GetTextSize(hDC, transform, _strLines(0), xsize0, ysize0)
        '_textSym.GetTextSize(hDC, transform, _strLines(UBound(_strLines)), xsizeN, ysizeN)

        Dim Wmax As Double
        Dim Hsum As Double
        Dim Wi As Double
        Dim Hi As Double

        Dim W0 As Double
        Dim H0 As Double
        Dim i As Integer
        Dim textSym As ITextSymbol = CType(_textSym, IClone).Clone

        Wmax = 0
        Hsum = 0

        _strLines = GetStringRows(_textSym.Text)
        n = UBound(_strLines)


        Dim sKey As String = "QWERTYUIOPASDFGHJKLZXCVBNM"

        For i = 0 To n
            textSym.Text = _strLines(i)
            Dim pTextParserSupport As ITextParserSupport = textSym
            Dim pTextParser As ITextParser = pTextParserSupport.TextParser
            pTextParser.Text = textSym.Text
            pTextParser.TextSymbol = textSym

            Dim bHasTags As Boolean
            pTextParser.HasTags(bHasTags)

            If bHasTags Then
                Wi = 0
                Hi = 0

                pTextParser.Reset()
                pTextParser.TextSymbol.Text = sKey  '_strLines(i)
                pTextParser.Next()

                Do While pTextParser.TextSymbol.Text <> sKey    '_strLines(i)
                    pTextParser.TextSymbol.GetTextSize(hDC, transform, pTextParser.TextSymbol.Text, W0, H0)
                    pTextParser.TextSymbol.Text = sKey          '_strLines(i)
                    Wi += W0
                    Hi = Math.Max(Hi, H0)
                    pTextParser.Next()
                Loop
            Else
                _textSym.GetTextSize(hDC, transform, _strLines(i), Wi, Hi)
            End If

            If Wi > Wmax Then Wmax = Wi
            Hsum = Hsum + Hi

            If i = 0 Then
                xsize0 = Wi
                ysize0 = Hi
            End If

            If i = n Then
                xsizeN = Wi
                ysizeN = Hi
            End If
        Next

        Dim fTextSym As IFormattedTextSymbol
        fTextSym = _textSym

        Hsum = Hsum + fTextSym.Leading * i


        Dim yTop As Double = pDisplayTransformation.FromPoints(ysize0)
        Dim yBot As Double = pDisplayTransformation.FromPoints(ysizeN)
        Hsum = pDisplayTransformation.FromPoints(Hsum)
        Wmax = pDisplayTransformation.FromPoints(Wmax)

        'Create polygon =====================================
        Dim fSize As Double = pDisplayTransformation.FromPoints(_dSize)
        Dim textAngle As Double = _textSym.Angle + 180.0
        Dim BasePoint As IPoint

        BasePoint = PointAlongPlane(_textBoxCenterPt, textAngle - 90.0, 0.5 * (Hsum - yTop))

        Dim pt1 As IPoint = PointAlongPlane(BasePoint, textAngle, 0.5 * Wmax + fSize)


        Dim pt2 As IPoint
        Dim pt3 As IPoint
        Dim pt4 As IPoint

        If _dme Then
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, Hsum - 0.5 * (yBot + yTop))
        Else
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, Hsum - 0.5 * yBot)
        End If

        pt3 = PointAlongPlane(pt2, textAngle + 180.0, Wmax + 2 * fSize)
        pt4 = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * Wmax + fSize)

        Dim pPoly As IPointCollection = New Polygon

        pPoly.AddPoint(pt1)
        pPoly.AddPoint(pt2)
        pPoly.AddPoint(pt3)
        pPoly.AddPoint(pt4)
        pPoly.AddPoint(pt1)

        Dim pTopoOp As ITopologicalOperator
        pTopoOp = pPoly
        pTopoOp.Simplify()
        '========================================================
        Dim LidPt As IPoint = New Point

        Dim pOutLineL As IPolyline = CreateOutlineLeft(xsize0, ysize0, xsizeN, ysizeN, Wmax, Hsum, fSize, pDisplayTransformation)
        Dim pOutLineR As IPolyline = CreateOutlineRight(xsize0, ysize0, xsizeN, ysizeN, Wmax, Hsum, fSize, pDisplayTransformation)


        Dim LiderLine As IPolyline = New Polyline
        Dim ptColl As IPointCollection = New Polyline

        Dim LiderPt As IPoint = New Point
        Select Case _snap
            Case 1
                LiderPt = pt1
            Case 4
                LiderPt = CreatePoint((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2)
            Case 7
                LiderPt = pt2
            Case 3
                LiderPt = pt4
            Case 6
                LiderPt = CreatePoint((pt3.X + pt4.X) / 2, (pt3.Y + pt4.Y) / 2)
            Case 9
                LiderPt = pt3
            Case 2
                LiderPt = CreatePoint((pt1.X + pt4.X) / 2, (pt1.Y + pt4.Y) / 2 + yTop / 2)
            Case 8
                If _dme Then
                    LiderPt = CreatePoint((pt2.X + pt3.X) / 2, (pt2.Y + pt3.Y) / 2 - yBot / 2)
                Else
                    LiderPt = CreatePoint((pt2.X + pt3.X) / 2, (pt2.Y + pt3.Y) / 2)
                End If

            Case 5
                Dim p1 As IPoint = New Point ' Point
                Dim x As Double
                Dim y As Double
                Dim pLidLine As IPointCollection = New Polyline

                x = _anchorPoint.X
                y = _anchorPoint.Y
                
                p1.X = x
                p1.Y = y

                Dim pSpatRefFact As ISpatialReferenceFactory2 = New SpatialReferenceEnvironment
                Dim pGCS As IGeographicCoordinateSystem = pSpatRefFact.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984) 'New ESRI.ArcGIS.Geometry.GeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984)
                Dim pApplication As IApplication

                If y < 90.0 Then
                    Dim k As Integer
                    Dim appRot As IAppROT = New AppROT()
                    For k = 0 To appRot.Count - 1
                        pApplication = appRot.Item(k)
                        Console.WriteLine(pApplication.Caption)
                    Next k

                    Dim mxDocument As IMxDocument = pApplication.Document
                    Dim activeView As IActiveView = mxDocument.ActiveView
                    Dim pMap As IMap = activeView.FocusMap
                    Dim pSpatRefPrj As ISpatialReference = New ProjectedCoordinateSystem
                    pSpatRefPrj = pMap.SpatialReference
                    p1.SpatialReference = pGCS
                    p1.Project(pSpatRefPrj)
                End If

                Dim ptTmp As IPointCollection
                Dim pLine As IPointCollection = New Polyline
                pLine.AddPoint(p1)
                pLine.AddPoint(_textBoxCenterPt)

                ptTmp = pTopoOp.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension)
                If ptTmp.PointCount > 0 Then
                    LiderPt = ptTmp.Point(0)
                End If
            Case 0
                Dim p1 As IPoint = New Point ' Point
                Dim x As Double
                Dim y As Double
                Dim pLidLine As IPointCollection = New Polyline

                x = _anchorPoint.X
                y = _anchorPoint.Y

                p1.X = x
                p1.Y = y

                Dim pSpatRefFact As ISpatialReferenceFactory2 = New SpatialReferenceEnvironment
                Dim pGCS As IGeographicCoordinateSystem = pSpatRefFact.CreateGeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984) 'New ESRI.ArcGIS.Geometry.GeographicCoordinateSystem(esriSRGeoCSType.esriSRGeoCS_WGS1984)
                Dim pApplication As IApplication

                If y < 90.0 Then
                    Dim k As Integer
                    Dim appRot As IAppROT = New AppROT()
                    For k = 0 To appRot.Count - 1
                        pApplication = appRot.Item(k)
                        Console.WriteLine(pApplication.Caption)
                    Next k

                    Dim mxDocument As IMxDocument = pApplication.Document
                    Dim activeView As IActiveView = mxDocument.ActiveView
                    Dim pMap As IMap = activeView.FocusMap
                    Dim pSpatRefPrj As ISpatialReference = New ProjectedCoordinateSystem
                    pSpatRefPrj = pMap.SpatialReference
                    p1.SpatialReference = pGCS
                    p1.Project(pSpatRefPrj)
                End If
                Dim pProxy As IProximityOperator
                Dim pt As IPoint

                pProxy = pPoly
                pt = pProxy.ReturnNearestPoint(p1, esriSegmentExtension.esriNoExtension)
                LiderPt = pt
        End Select


        Dim GeoBag As IGeometryCollection = New GeometryBag()
        GeoBag.AddGeometry(pPoly)
        GeoBag.AddGeometry(pOutLineL)
        GeoBag.AddGeometry(pOutLineR)


        ptColl.AddPoint(LiderPt)

        ptColl.AddPoint(_anchorPoint)

        Dim GapLine As ILine = New Line
        Dim dir As Double
        Dim Gap As Double = pDisplayTransformation.FromPoints(_dTopMargine)

        GapLine.FromPoint = _anchorPoint
        GapLine.ToPoint = LiderPt

        If pTopoOp.Intersect(LiderPt, esriGeometryDimension.esriGeometry0Dimension) Is Nothing Then
            dir = GapLine.Angle
            Dim GapPt As IPoint = CreatePoint(_anchorPoint.X + Gap * Math.Cos(dir), _anchorPoint.Y + Gap * Math.Sin(dir))
            If Math.Sqrt(Gap * Math.Cos(dir) * Gap * Math.Cos(dir) + Gap * Math.Sin(dir) * Gap * Math.Sin(dir)) < GapLine.Length Then
                ptColl.RemovePoints(1, 1)
                ptColl.AddPoint(GapPt)
            End If
        End If
        GeoBag.AddGeometry(ptColl)

        Dim AirClass As IPoint = New Point
        AirClass = CreatePoint((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2)
        'Dim pTrans2D As ITransform2D
        'pTrans2D = AirClass
        'pTrans2D.Rotate(AirClass, textAngle + 180)

        GeoBag.AddGeometry(AirClass)


        Return GeoBag
    End Function



    Public Function CreateOutlineLeft(ByVal xTop As Double, ByVal yTop As Double, ByVal xBot As Double, ByVal yBot As Double, ByVal W As Double, ByVal H As Double, ByVal dW As Double, ByVal pDisplayTransformation As IDisplayTransformation) As IPolyline

        Dim textAngle As Double = _textSym.Angle + 180.0
        Dim BasePoint As IPoint
        BasePoint = New Point

        xTop = pDisplayTransformation.FromPoints(xTop)
        yTop = pDisplayTransformation.FromPoints(yTop)

        xBot = pDisplayTransformation.FromPoints(xBot)
        yBot = pDisplayTransformation.FromPoints(yBot)

        'W = pDisplayTransformation.FromPoints(W)
        'H = pDisplayTransformation.FromPoints(H)
        BasePoint = PointAlongPlane(_textBoxCenterPt, _textSym.Angle + 90.0, 0.5 * (H - yTop))

        Dim lMarg As Double = pDisplayTransformation.FromPoints(_dLeftMargine)
        Dim fSize As Double = pDisplayTransformation.FromPoints(_dSize)

        Dim pt0 As IPoint = PointAlongPlane(BasePoint, textAngle, 0.5 * xTop + lMarg)
        Dim pt1 As IPoint = PointAlongPlane(BasePoint, textAngle, 0.5 * W + dW)
        Dim pt2 As IPoint
        Dim pt3 As IPoint

        If _dme Then
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, H - 0.5 * (yBot + yTop))
            pt3 = PointAlongPlane(pt2, textAngle + 180.0, 0.5 * (W - xBot) + dW - lMarg)
        Else
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, H - 0.5 * yBot)
            pt3 = PointAlongPlane(pt2, textAngle + 180.0, W)
        End If



        Dim pLine As IPointCollection = New Polyline

        pLine.AddPoint(pt0)
        pLine.AddPoint(pt1)
        pLine.AddPoint(pt2)
        pLine.AddPoint(pt3)


        Return pLine
    End Function

    Public Function CreateOutlineRight(ByVal xTop As Double, ByVal yTop As Double, ByVal xBot As Double, ByVal yBot As Double, ByVal W As Double, ByVal H As Double, ByVal dW As Double, ByVal pDisplayTransformation As IDisplayTransformation) As IPolyline

        Dim textAngle As Double = _textSym.Angle + 180.0
        Dim BasePoint As IPoint
        BasePoint = New Point

        xTop = pDisplayTransformation.FromPoints(xTop)
        yTop = pDisplayTransformation.FromPoints(yTop)

        xBot = pDisplayTransformation.FromPoints(xBot)
        yBot = pDisplayTransformation.FromPoints(yBot)

        'W = pDisplayTransformation.FromPoints(W)
        'H = pDisplayTransformation.FromPoints(H)
        BasePoint = PointAlongPlane(_textBoxCenterPt, _textSym.Angle + 90.0, 0.5 * (H - yTop))

        Dim rMarg As Double = pDisplayTransformation.FromPoints(_dRightMargine)
        Dim fSize As Double = pDisplayTransformation.FromPoints(_dSize)

        Dim pt0 As IPoint = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * xTop + rMarg)
        Dim pt1 As IPoint = PointAlongPlane(BasePoint, textAngle + 180.0, 0.5 * W + dW)
        Dim pt2 As IPoint
        Dim pt3 As IPoint

        If _dme Then
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, H - 0.5 * (yBot + yTop))
            pt3 = PointAlongPlane(pt2, textAngle, 0.5 * (W - xBot) + dW - rMarg)
        Else
            pt2 = PointAlongPlane(pt1, textAngle + 90.0, H - 0.5 * yBot)
            pt3 = PointAlongPlane(pt2, textAngle, W)
        End If



        Dim pLine As IPointCollection = New Polyline

        pLine.AddPoint(pt0)
        pLine.AddPoint(pt1)
        pLine.AddPoint(pt2)
        pLine.AddPoint(pt3)


        Return pLine
    End Function

    Public Function CreateLeader(geom As IGeometry, ByVal pDisplayTransformation As IDisplayTransformation) As IPolyline    ', ByVal pDisplayTransformation As IDisplayTransformation
        If _anchorPoint Is Nothing Or _textBoxCenterPt Is Nothing Then Return Nothing

        'Dim ptExanchorPoint As IPoint = CreatePoint(pDisplayTransformation.TransformPointsFF.ToPoints(_anchorPoint.X), pDisplayTransformation.ToPoints(_anchorPoint.Y))
        'Dim ptExanchorPoint As IPoint = CreatePoint(pDisplayTransformation.FromPoints(_anchorPoint.X), pDisplayTransformation.FromPoints(_anchorPoint.Y))

        Dim geomColl As IGeometryCollection = geom
        Dim pLine As IPointCollection = New Polyline
        ' pLine.AddPoint(_anchorPoint)
        ' pLine.AddPoint(_textBoxCenterPt)

        Dim pLineCut As IPointCollection = New Polyline
        ' pDisplayTransformation.FromMapPoint(_anchorPoint, _anchorPoint.X, _anchorPoint.Y)
        pLineCut.AddPoint(_anchorPoint)

        Dim fSize As Double = pDisplayTransformation.FromPoints(_dSize)

        'Select Case _snap
        '    Case 0
        '        Dim pProxy As IProximityOperator
        '        Dim pt As IPoint

        '        pProxy = geomColl.Geometry(0)
        '        pt = pProxy.ReturnNearestPoint(_anchorPoint, esriSegmentExtension.esriExtendEmbedded)
        '        pLineCut.AddPoint(pt)
        '    Case 5
        '        Dim pTopo As ITopologicalOperator
        '        Dim ptTmp As IPointCollection

        '        pTopo = pLine
        '        ptTmp = pTopo.Intersect(geomColl.Geometry(0), esriGeometryDimension.esriGeometry0Dimension)
        '        If ptTmp.PointCount > 0 Then
        '            pLineCut.AddPoint(ptTmp.Point(0))
        '        End If
        '    Case 1
        '        'pLineCut.AddPoint(CreatePoint(_textBox.XMax + fSize, _textBox.YMax - 0.5 * yTop))

        'End Select

        'If _snap <> 0 Then
        '    Dim pTopo As ITopologicalOperator
        '    Dim ptTmp As IPointCollection

        '    pTopo = pLine
        '    ptTmp = pTopo.Intersect(geomColl.Geometry(0), esriGeometryDimension.esriGeometry0Dimension)
        '    If ptTmp.PointCount > 0 Then
        '        pLineCut.AddPoint(ptTmp.Point(0))
        '    End If
        'Else
        '    Dim pProxy As IProximityOperator
        '    Dim pt As IPoint

        '    pProxy = geomColl.Geometry(0)
        '    pt = pProxy.ReturnNearestPoint(_anchorPoint, esriSegmentExtension.esriExtendEmbedded)
        '    pLineCut.AddPoint(pt)
        'End If

        Dim cLine As ILine = New Line
        cLine.FromPoint = pLineCut.Point(0)
        cLine.ToPoint = pLineCut.Point(0)
        Dim ang As Double
        ang = cLine.Angle


        Return pLineCut
    End Function

    Private Sub CreateBoundaryOfEnvelope(ByVal pEnvelope As IEnvelope, ByRef pPolycurve As IPolycurve)
        '  Check we have valid parameters.
        '  pPolycurve must be initialized as either a Polygon or Polyline, and must be empty.
        If pEnvelope Is Nothing Or pPolycurve Is Nothing Then Exit Sub
        pPolycurve.SetEmpty()

        '  Build the boundary of the Envelope, pEnvelope.
        Dim pGeomColl As IGeometryCollection
        Dim pSegColl As ISegmentCollection
        Dim pLine As ILine

        If TypeOf pPolycurve Is IPolygon Then
            pSegColl = New Ring
        ElseIf TypeOf pPolycurve Is IPolyline Then
            pSegColl = New Path
        Else
            Exit Sub
        End If

        pLine = CreateLine(pEnvelope.UpperLeft, pEnvelope.UpperRight)
        pSegColl.AddSegment(pLine)

        pLine = CreateLine(pEnvelope.UpperRight, pEnvelope.LowerRight)
        pSegColl.AddSegment(pLine)

        pLine = CreateLine(pEnvelope.LowerRight, pEnvelope.LowerLeft)
        pSegColl.AddSegment(pLine)

        pLine = CreateLine(pEnvelope.LowerLeft, pEnvelope.UpperLeft)
        pSegColl.AddSegment(pLine)

        pGeomColl = pPolycurve
        pGeomColl.AddGeometry(pSegColl)
    End Sub

#End Region



End Class