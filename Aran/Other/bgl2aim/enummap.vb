Imports Aran.Aim.Enums
Public Module enummap

    Function convert_stSurface(val As stSurface) As Nullable(Of CodeSurfaceComposition)
        If stSurfaceDict Is Nothing Then
            stSurfaceDict = New Dictionary(Of stSurface, CodeSurfaceComposition)()
            stSurfaceDict.Add(stSurface.ASPHALT, CodeSurfaceComposition.ASPH)
            stSurfaceDict.Add(stSurface.BITUMINOUS, CodeSurfaceComposition.BITUM)
        End If

        Dim rv As CodeSurfaceComposition
        If stSurfaceDict.TryGetValue(val, rv) Then
            Return rv
        End If

        Return Nothing

    End Function

    Private stSurfaceDict As Dictionary(Of stSurface, CodeSurfaceComposition)


    
End Module
