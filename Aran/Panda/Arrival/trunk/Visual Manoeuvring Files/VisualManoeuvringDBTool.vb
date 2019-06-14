Option Strict Off

Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.Collections.Generic
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports Aran.Converters
Imports Aran.Aim.Features

Friend Class VisualManoeuvringDBTool
    Public Shared fileName As String
    Public Shared featureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
    Public Shared visualReferencePointFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
    Public Shared visualReferenceIndexes() As Integer

    Private Shared Sub OpenFeatureClass(fileNameValue As String)
        If fileName <> fileNameValue Then
            fileName = fileNameValue
        Else
            Return
        End If

        Dim wf As IWorkspaceFactory = New AccessWorkspaceFactory()
        featureWorkspace = CType(wf.OpenFromFile(fileName, 0), IFeatureWorkspace)
    End Sub

    Public Shared Sub AddVisualReferencePoint(data As VisualReferencePoint)
        OpenFeatureClass("D:\ProjectData\VisualM_DB\VisualM_DB.mdb")
        If visualReferencePointFeatureClass Is Nothing Then
            visualReferencePointFeatureClass = featureWorkspace.OpenFeatureClass("VisualReferencePoint")
            '_visRefFeatClass.Search(Nothing, True).NextFeature().Value
            ReDim visualReferenceIndexes(4)
            visualReferenceIndexes(1) = visualReferencePointFeatureClass.FindField("NAME")
            visualReferenceIndexes(2) = visualReferencePointFeatureClass.FindField("TYPE")
            visualReferenceIndexes(3) = visualReferencePointFeatureClass.FindField("DESCRIPTION")
        End If

        Dim feature As IFeature = visualReferencePointFeatureClass.CreateFeature()
        Try
            feature.Shape = data.gShape
            feature.Value(visualReferenceIndexes(1)) = data.Name
            feature.Value(visualReferenceIndexes(2)) = data.Type
            feature.Value(visualReferenceIndexes(3)) = data.Description
            feature.Store()
        Catch ex As Exception
            feature.Delete()
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'Public Shared Function GetVisualReferencePoints_old() As List(Of VisualReferencePoint)
    '    Dim allVisRefPnts As List(Of VisualReferencePoint) = New List(Of VisualReferencePoint)
    '    Dim tempVisRefPnt As VisualReferencePoint
    '    OpenFeatureClass("D:\ProjectData\VisualM_DB\VisualM_DB.mdb")
    '    If visualReferencePointFeatureClass Is Nothing Then
    '        visualReferencePointFeatureClass = featureWorkspace.OpenFeatureClass("VisualReferencePoint")
    '        ReDim visualReferenceIndexes(4)
    '        '_visRefIndexes(0) = _visRefFeatClass.FindField("SHAPE")
    '        visualReferenceIndexes(1) = visualReferencePointFeatureClass.FindField("NAME")
    '        visualReferenceIndexes(2) = visualReferencePointFeatureClass.FindField("TYPE")
    '        visualReferenceIndexes(3) = visualReferencePointFeatureClass.FindField("DESCRIPTION")
    '    End If

    '    Try
    '        Dim featCursor As IFeatureCursor = visualReferencePointFeatureClass.Search(Nothing, False)
    '        Dim feature As IFeature = featCursor.NextFeature()

    '        While feature IsNot Nothing
    '            tempVisRefPnt = New VisualReferencePoint(CType(feature.Shape, Global.ESRI.ArcGIS.Geometry.IPoint),
    '                                            CType(Functions.ToPrj(CType(feature.Shape, IPoint)), Global.ESRI.ArcGIS.Geometry.IPoint),
    '                                            CStr(feature.Value(visualReferenceIndexes(1))), CStr(feature.Value(visualReferenceIndexes(2))),
    '                                            CStr(feature.Value(visualReferenceIndexes(3))))
    '            allVisRefPnts.Add(tempVisRefPnt)
    '            feature = featCursor.NextFeature()
    '        End While
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    '    Return allVisRefPnts
    'End Function

    Public Shared Function GetVisualReferencePoints() As List(Of VisualReferencePoint)
        Dim allVisRefPnts As List(Of VisualReferencePoint) = New List(Of VisualReferencePoint)
        'Dim tempList As List(Of Aim.Features.AeronauticalGroundLight) = New List(Of Aim.Features.AeronauticalGroundLight)
        'For Each elem As Object In pObjectDir.GetFeatureList(Aim.FeatureType.AeronauticalGroundLight)
        '    tempList.Add(CType(elem, Aim.Features.AeronauticalGroundLight))
        'Next
        Dim tempList As List(Of Aim.Features.AeronauticalGroundLight) = CType(pObjectDir.GetFeatureList(Aim.FeatureType.AeronauticalGroundLight), Global.System.Collections.Generic.List(Of Global.Aran.Aim.Features.AeronauticalGroundLight))
        If tempList.Count > 0 Then
            For i As Integer = 0 To tempList.Count - 1
                Dim tempVisRefPnt As VisualReferencePoint = New VisualReferencePoint(ConvertToEsriGeom.FromPoint(tempList(i).Location.Geo), CType(ToPrj(ConvertToEsriGeom.FromPoint(tempList(i).Location.Geo)), Global.ESRI.ArcGIS.Geometry.IPoint), tempList(i).Name, tempList(i).Type.ToString, tempList(i).Annotation(0).TranslatedNote(0).Note.Value)
                allVisRefPnts.Add(tempVisRefPnt)
            Next
        End If
        Return allVisRefPnts
    End Function

    Public Shared Sub AddTrackStepCentreline(data As List(Of TrackStep))
        Dim I As Integer
        Dim tempPntColl As IPointCollection = New Polyline
        For I = 0 To data.Count - 1
            tempPntColl.AddPointCollection(data(I).initialSegmentCentrelinePointCollection)
            tempPntColl.AddPointCollection(data(I).intermediateSegmentCentrelinePointCollection)
            tempPntColl.AddPointCollection(data(I).finalSegmentCentrelinePointCollection)
        Next
        Dim tempPolyline As IPolyline = CType(tempPntColl, IPolyline)
        Functions.DrawPolyLine(tempPolyline, Color.Purple.ToArgb, 2.0, True)

        If data.Count > 0 Then
            OpenFeatureClass("D:\ProjectData\VisualM_DB\VisualM_DB.mdb")
        End If
    End Sub

    Public Shared Sub InsertVisualReferencePoints()
        Try
            Dim result As Boolean = pObjectDir.Commit({Aran.Aim.FeatureType.AeronauticalGroundLight})
            If Not result Then
                MessageBox.Show("False")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Public Shared Sub CreateVisualReferencePoint(name As String, description As String, geoPnt As IPoint)
        pObjectDir.SetRootFeatureType(Aim.FeatureType.AeronauticalGroundLight)
        Dim vrp As AeronauticalGroundLight = pObjectDir.CreateFeature(Of AeronauticalGroundLight)()
        vrp.Location = New ElevatedPoint
        vrp.Location.Geo.X = geoPnt.X
        vrp.Location.Geo.Y = geoPnt.Y
        vrp.Type = Aim.Enums.CodeGroundLighting.ABN
        vrp.Name = name

        Dim note As Note = New Note
        Dim ln As LinguisticNote = New LinguisticNote
        ln.Note = New Aran.Aim.DataTypes.TextNote
        ln.Note.Lang = Aim.Enums.language.ENG
        ln.Note.Value = description

        note.TranslatedNote.Add(ln)

        vrp.Annotation.Add(note)
    End Sub
End Class
