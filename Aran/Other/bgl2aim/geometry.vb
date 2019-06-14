Module Geometry
    Public Class Triangle
        Public Points(2) As PointF
        Public Sub New(ByVal p0 As PointF, ByVal p1 As PointF, ByVal p2 As PointF)
            Points(0) = p0
            Points(1) = p1
            Points(2) = p2
        End Sub
    End Class

    ' Find the polygon's centroid.
    Public Function FindCentroid(ByVal points() As PointF) As PointF
        ' Add the first point at the end of the array.
        ReDim Preserve points(points.Length)
        points(points.Length - 1) = New PointF(points(0).X, points(0).Y)

        ' Find the centroid.
        Dim X As Single = 0
        Dim Y As Single = 0
        Dim second_factor As Single
        For i As Integer = 0 To points.Length - 2
            second_factor = _
                points(i).X * points(i + 1).Y - _
                points(i + 1).X * points(i).Y
            X += (points(i).X + points(i + 1).X) * second_factor
            Y += (points(i).Y + points(i + 1).Y) * second_factor
        Next i

        ' Divide by 6 times the polygon's area.
        Dim polygon_area As Single = PolygonArea(points)
        X /= (6 * polygon_area)
        Y /= (6 * polygon_area)

        ' If the values are negative, the polygon is
        ' oriented counterclockwise. Reverse the signs.
        If X < 0 Then
            X = -X
            Y = -Y
        End If

        Return New PointF(X, Y)
    End Function

    ' Return True if the point is in the polygon.
    Public Function PointInPolygon(ByVal points() As PointF, ByVal X As Single, ByVal Y As Single) As Boolean
        ' Get the angle between the point and the
        ' first and last vertices.
        Dim max_point As Integer = points.Length - 1
        Dim total_angle As Single = GetAngle( _
            points(max_point).X, points(max_point).Y, _
            X, Y, _
            points(0).X, points(0).Y)

        ' Add the angles from the point
        ' to each other pair of vertices.
        For i As Integer = 0 To max_point - 1
            total_angle += GetAngle( _
                points(i).X, points(i).Y, _
                X, Y, _
                points(i + 1).X, points(i + 1).Y)
        Next i

        ' The total angle should be 2 * PI or -2 * PI if
        ' the point is in the polygon and close to zero
        ' if the point is outside the polygon.
        Return Math.Abs(total_angle) > 0.000001
    End Function

#Region "Orientation Routines"
    ' Return True if the polygon is oriented clockwise.
    Public Function PolygonIsOrientedClockwise(ByVal points() As PointF) As Boolean
        Return SignedPolygonArea(points) < 0
    End Function

    ' If the polygon is oriented counterclockwise,
    ' reverse the order of its points.
    Private Sub OrientPolygonClockwise(ByVal points() As PointF)
        If Not PolygonIsOrientedClockwise(points) Then
            Array.Reverse(points)
        End If
    End Sub
#End Region ' Orientation Routines

#Region "Area Routines"
    ' Return the polygon's area in "square units."
    ' Add the areas of the trapezoids defined by the
    ' polygon's edges dropped to the X-axis. When the
    ' program considers a bottom edge of a polygon, the
    ' calculation gives a negative area so the space
    ' between the polygon and the axis is subtracted,
    ' leaving the polygon's area. This method gives odd
    ' results for non-simple polygons.
    Public Function PolygonArea(ByVal points() As PointF) As Single
        ' Return the absolute value of the signed area.
        ' The signed area is negative if the polyogn is
        ' oriented clockwise.
        Return Math.Abs(SignedPolygonArea(points))
    End Function

    ' Return the polygon's area in "square units."
    ' Add the areas of the trapezoids defined by the
    ' polygon's edges dropped to the X-axis. When the
    ' program considers a bottom edge of a polygon, the
    ' calculation gives a negative area so the space
    ' between the polygon and the axis is subtracted,
    ' leaving the polygon's area. This method gives odd
    ' results for non-simple polygons.
    '
    ' The value will be negative if the polyogn is
    ' oriented clockwise.
    Private Function SignedPolygonArea(ByVal points() As PointF) As Single
        ' Add the first point to the end.
        ReDim Preserve points(points.Length)
        points(points.Length - 1) = points(0)

        ' Get the areas.
        Dim area As Single = 0
        For i As Integer = 0 To points.Length - 2
            area += _
                (points(i + 1).X - points(i).X) * _
                (points(i + 1).Y + points(i).Y) / 2
        Next i

        ' Return the result.
        Return area
    End Function
#End Region ' Area Routines

    ' Return True if the polygon is convex.
    Public Function PolygonIsConvex(ByVal points() As PointF) As Boolean
        ' For each set of three adjacent points A, B, C,
        ' find the dot product AB · BC. If the sign of
        ' all the dot products is the same, the angles
        ' are all positive or negative (depending on the
        ' order in which we visit them) so the polygon
        ' is convex.

        Dim got_negative As Boolean = False
        Dim got_positive As Boolean = False
        Dim max_point As Integer = points.Length - 1
        Dim B, C As Integer
        For A As Integer = 0 To max_point
            B = A + 1
            If B > max_point Then B = 0
            C = B + 1
            If C > max_point Then C = 0

            Dim cross_product As Single = _
                CrossProductLength( _
                    points(A).X, points(A).Y, _
                    points(B).X, points(B).Y, _
                    points(C).X, points(C).Y)
            If cross_product < 0 Then
                got_negative = True
            ElseIf cross_product > 0 Then
                got_positive = True
            End If
            If got_negative And got_positive Then Return False
        Next A

        ' If we got this far, the polygon is convex.
        Return True
    End Function

#Region "Cross and Dot Products"
    ' Return the cross product AB x BC.
    ' The cross product is a vector perpendicular to AB
    ' and BC having length |AB| * |BC| * Sin(theta) and
    ' with direction given by the right-hand rule.
    ' For two vectors in the X-Y plane, the result is a
    ' vector with X and Y components 0 so the Z component
    ' gives the vector's length and direction.
    Public Function CrossProductLength( _
        ByVal Ax As Single, ByVal Ay As Single, _
        ByVal Bx As Single, ByVal By As Single, _
        ByVal Cx As Single, ByVal Cy As Single _
      ) As Single
        ' Get the vectors' coordinates.
        Dim BAx As Single = Ax - Bx
        Dim BAy As Single = Ay - By
        Dim BCx As Single = Cx - Bx
        Dim BCy As Single = Cy - By

        ' Calculate the Z coordinate of the cross product.
        Return BAx * BCy - BAy * BCx
    End Function

    ' Return the dot product AB · BC.
    ' Note that AB · BC = |AB| * |BC| * Cos(theta).
    Private Function DotProduct( _
        ByVal Ax As Single, ByVal Ay As Single, _
        ByVal Bx As Single, ByVal By As Single, _
        ByVal Cx As Single, ByVal Cy As Single _
      ) As Single
        ' Get the vectors' coordinates.
        Dim BAx As Single = Ax - Bx
        Dim BAy As Single = Ay - By
        Dim BCx As Single = Cx - Bx
        Dim BCy As Single = Cy - By

        ' Calculate the dot product.
        Return BAx * BCx + BAy * BCy
    End Function
#End Region ' Cross and Dot Products

    ' Return the angle ABC.
    ' Return a value between PI and -PI.
    ' Note that the value is the opposite of what you might
    ' expect because Y coordinates increase downward.
    Public Function GetAngle(ByVal Ax As Single, ByVal Ay As Single, ByVal Bx As Single, ByVal By As Single, ByVal Cx As Single, ByVal Cy As Single) As Single
        Dim dot_product As Single
        Dim cross_product As Single

        ' Get the dot product and cross product.
        dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy)
        cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy)

        ' Calculate the angle.
        Return Math.Atan2(cross_product, dot_product)
    End Function

#Region "Triangulation"
    ' Find the indexes of three points that form an ear.
    Private Sub FindEar(ByRef points() As PointF, ByRef A As Integer, ByRef B As Integer, ByRef C As Integer)
        Dim max_point As Integer = points.Length - 1

        For A = 0 To max_point
            B = A + 1
            If B > max_point Then B = 0
            C = B + 1
            If C > max_point Then C = 0

            If FormsEar(points, A, B, C) Then Exit Sub
        Next A

        ' We should never get here because there should
        ' always be at least two ears.
        Stop
    End Sub

    ' Return True if the three points form an ear.
    Private Function FormsEar(ByVal points() As PointF, ByVal A As Integer, ByVal B As Integer, ByVal C As Integer) As Boolean
        ' See if the angle ABC is concave.
        If GetAngle( _
            points(A).X, points(A).Y, _
            points(B).X, points(B).Y, _
            points(C).X, points(C).Y) > 0 _
        Then
            ' This is a concave corner so the triangle
            ' cannot be an ear.
            Return False
        End If

        ' Make the triangle A, B, C.
        Dim test_points() As PointF = _
            {points(A), points(B), points(C)}

        ' Check the other points to see 
        ' if they lie in triangle A, B, C.
        For i As Integer = 0 To points.Length - 1
            If (i <> A) AndAlso (i <> B) AndAlso (i <> C) Then
                With points(i)
                    If PointInPolygon(test_points, .X, .Y) Then
                        ' This point is in the triangle so
                        ' this is not an ear.
                        Return False
                    End If
                End With
            End If
        Next i

        ' This is an ear.
        Return True
    End Function

    ' Remove an ear from the polygon.
    Private Sub RemoveEar(ByRef points() As PointF, ByRef triangles() As Triangle)
        ' Find an ear.
        Dim A, B, C As Integer
        FindEar(points, A, B, C)

        ' Create a new triangle for the ear.
        ReDim Preserve triangles(triangles.Length)
        triangles(triangles.Length - 1) = _
            New Triangle(points(A), points(B), points(C))

        ' Remove the ear from the polygon.
        RemovePointFromArray(points, B)
    End Sub

    ' Remove point target from the array.
    Private Sub RemovePointFromArray(ByRef points() As PointF, ByVal target As Integer)
        Array.Copy(points, target + 1, points, target, points.Length - target - 1)
        ReDim Preserve points(points.Length - 2)
    End Sub

    ' Traingulate the polygon.
    '
    ' For a nice, detailed explanation of this method,
    ' see Ian Garton's Web page:
    ' http://www-cgrl.cs.mcgill.ca/~godfried/teaching/cg-projects/97/Ian/cutting_ears.html
    Public Function Triangulate(ByVal points() As PointF) As Triangle()
        ' Copy the points into a new array,
        ' making room for two more.
        Dim copy_points(points.Length - 1) As PointF
        Array.Copy(points, copy_points, points.Length)

        ' Orient the polygon clockwise.
        OrientPolygonClockwise(copy_points)

        ' Make room for the triangles.
        Dim triangles() As Triangle = {}

        ' While the copy of the polygon has more than
        ' three points, remove an ear.
        Do While copy_points.Length > 3
            ' Remove an ear from the polygon.
            RemoveEar(copy_points, triangles)
        Loop

        ' Copy the last three points into their own triangle.
        ReDim Preserve triangles(triangles.Length)
        triangles(triangles.Length - 1) = _
            New Triangle(copy_points(0), copy_points(1), copy_points(2))

        Return triangles
    End Function
#End Region ' Triangulation


#Region "Calculate the distance between the point and the segment."
    Public Function DistToSegment(ByVal px As Single, ByVal py As Single, ByVal X1 As Single, ByVal Y1 As Single, ByVal X2 As Single, ByVal Y2 As Single, ByRef near_x As Single, ByRef near_y As Single) As Single
        Dim dx As Single
        Dim dy As Single
        Dim t As Single

        dx = X2 - X1
        dy = Y2 - Y1
        If dx = 0 And dy = 0 Then
            ' It's a point not a line segment.
            dx = px - X1
            dy = py - Y1
            near_x = X1
            near_y = Y1
            DistToSegment = Math.Sqrt(dx * dx + dy * dy)
            Exit Function
        End If

        ' Calculate the t that minimizes the distance.
        t = ((px - X1) * dx + (py - Y1) * dy) / (dx * dx + dy * dy)

        ' See if this represents one of the segment's
        ' end points or a point in the middle.
        If t < 0 Then
            dx = px - X1
            dy = py - Y1
            near_x = X1
            near_y = Y1
        ElseIf t > 1 Then
            dx = px - X2
            dy = py - Y2
            near_x = X2
            near_y = Y2
        Else
            near_x = X1 + t * dx
            near_y = Y1 + t * dy
            dx = px - near_x
            dy = py - near_y
        End If

        DistToSegment = Math.Sqrt(dx * dx + dy * dy)
    End Function
#End Region ' Calculate the distance between the point and the segment.
End Module
