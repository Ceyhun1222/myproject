Imports System.IO
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions

Imports Aran
Imports Aran.Aim
Imports Aran.Aim.Objects
Imports Aran.Aim.Enums
Imports Aran.Aim.DataTypes
Imports Aran.Aim.Features
Imports Aran.Aim.GmlParser
Imports Aran.Panda.Common



Public Class bgl2aixm
    Public Structure flowgeoattr

        Public res As Integer

        Public lat0 As Double
        Public lon0 As Double
        Public lat1 As Double
        Public lon1 As Double
        Public azm_d As Double
        Public azm_i As Double

        Public angdef As Double
        Public angle As Double

        Public way_offset As Double
        Public radius_offset As Double

        'radius_offset = taxi_taxi_offset / Math.Tan(deflection_angle / 2)
    End Structure

    Private geoattr As flowgeoattr
    Private bglFSData As New ctFSData()
    Private xmlfile_input
    Private inifile As String = Application.StartupPath.ToString & "\" & Application.ProductName & ".cfg"

    Private Sub bgl2aixm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Dim inifile As String = Application.StartupPath.ToString & "\" & Application.ProductName & ".cfg"
        If System.IO.File.Exists(inifile) = True And inifile.Length > 0 Then

            Dim obj As New System.IO.StreamReader(inifile)
            xmlfile_input = obj.ReadLine()
            xmlfile_txt.Text = xmlfile_input
            obj.Close()
        End If

        xmlload_btn.Focus()

        lvwBooks.Items.Clear()

        filllist()


    End Sub
    ' The column currently used for sorting.
    Private m_SortingColumn As ColumnHeader
    Private Sub lvwBooks_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwBooks.ColumnClick
        ' Get the new sorting column.
        Dim new_sorting_column As ColumnHeader = _
            lvwBooks.Columns(e.Column)

        ' Figure out the new sorting order.
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If

        ' Create a comparer.
        lvwBooks.ListViewItemSorter = New ListViewComparer(e.Column, sort_order)

        ' Sort.
        lvwBooks.Sort()
    End Sub
    Sub filllist()

        lvwBooks.Items.Clear()
        lvwBooks.CheckBoxes = True
        ' Add data rows.
        'ListViewMakeRow(lvwBooks, "            ", "", "0-471-24268-3", "http://www.vb-helper.com/vba.jpg", "395", "1998")
        

        ' Make the ListView column headers.
        ListViewMakeColumnHeaders(lvwBooks, _
            "Name", HorizontalAlignment.Left, _
            "Quantity", HorizontalAlignment.Center, _
            "Description", HorizontalAlignment.Right)

        ' Size the columns.
        For i As Integer = 0 To lvwBooks.Columns.Count - 1
            lvwBooks.Columns(i).Width = -2
        Next i

        ' Make the form big enough to show the ListView.
        'Dim item_rect As Rectangle =         lvwBooks.GetItemRect(lvwBooks.Columns.Count - 1)
        'Me.SetClientSizeCore(item_rect.Left + item_rect.Width + 50, item_rect.Top + item_rect.Height + 75)

    End Sub


    ' Make a ListView row.
    Private Sub ListViewMakeRow(ByVal lvw As ListView, ByVal item_title As String, ByVal chk As Boolean, ByVal ParamArray subitem_titles() As String)
        ' Make the item.

        Dim new_item As ListViewItem = lvw.Items.Add(item_title)
        new_item.Checked = chk 'True

        ' Make the sub-items.
        For i As Integer = subitem_titles.GetLowerBound(0) To subitem_titles.GetUpperBound(0)
            new_item.SubItems.Add(subitem_titles(i))
        Next i

    End Sub


    ' Make the ListView's column headers.
    ' The ParamArray entries should alternate between
    ' strings and HorizontalAlignment values.
    Private Sub ListViewMakeColumnHeaders(ByVal lvw As ListView, ByVal ParamArray header_info() As Object)
        ' Remove any existing headers.
        lvw.Columns.Clear()

        ' Make the column headers.
        For i As Integer = header_info.GetLowerBound(0) To header_info.GetUpperBound(0) Step 2
            lvw.Columns.Add( _
                DirectCast(header_info(i), String), _
                -1, _
                DirectCast(header_info(i + 1), HorizontalAlignment))
        Next i
    End Sub

    Private Sub xmlfile_btn_Click(sender As Object, e As EventArgs) Handles xmlfile_btn.Click



        OpenFileDialog.Title = "Open File Dialog"
        'OpenFileDialog.InitialDirectory = "C:\"
        OpenFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
        OpenFileDialog.FilterIndex = 1
        OpenFileDialog.RestoreDirectory = True
        Dim result As DialogResult = OpenFileDialog.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            xmlfile_input = OpenFileDialog.FileName
            xmlfile_txt.Text = xmlfile_input
            Dim obj As New System.IO.StreamWriter(inifile)
            obj.WriteLine(xmlfile_input)
            obj.Close()
        End If




    End Sub


    Private Sub start_btn_Click(sender As Object, e As EventArgs) Handles start_btn.Click
        If Not System.IO.File.Exists(xmlfile_input) = True Then Exit Sub

        Try
            Dim x As New XmlSerializer(bglFSData.GetType)

            Dim objStreamReader As New StreamReader(xmlfile_input.ToString)

            bglFSData = x.Deserialize(objStreamReader)
            objStreamReader.Close()

        Catch ex As Exception
            Console.WriteLine(ex.Message) ' MessageBox.Show(ex.Message)
            Console.WriteLine(ex.StackTrace) 'MessageBox.Show("Stack Trace: " & vbCrLf & ex.StackTrace)
            Exit Sub
        End Try

        'Finally
        Console.Write(bglFSData.version)

        Dim ts = New TimeSlice
        ts.SequenceNumber = 1
        ts.CorrectionNumber = 0
        ts.ValidTime = New TimePeriod(Date.Now)
        ts.FeatureLifetime = ts.ValidTime


        '== start airport part
        'For i = 0 To bglFSData.Airport.Length - 1

        '    adhplist_cmb.Items.Add(bglFSData.Airport()(i).ident)

        'Next
        'adhplist_cmb.SelectedIndex = 0

        Dim bglAirport As New ctAirport()
        bglAirport = bglFSData.Airport()(adhplist_cmb.SelectedIndex)

        Dim pAirportHeliport As AirportHeliport
        pAirportHeliport = New AirportHeliport()
        pAirportHeliport.TimeSlice = ts
        pAirportHeliport.Identifier = Guid.NewGuid
        pAirportHeliport.Name = bglAirport.name.ToUpper

        pAirportHeliport.FieldElevation = trdv(bglAirport.alt)

        pAirportHeliport.MagneticVariation = bglAirport.magvar
        pAirportHeliport.MagneticVariationChange = bglAirport.magvarSpecified
        pAirportHeliport.Designator = bglAirport.ident


        Dim coord As Aran.Geometries.Point
        coord = New Aran.Geometries.Point(CDbl(Regex.Replace(bglAirport.lon, "[A-Z]", String.Empty)), CDbl(Regex.Replace(bglAirport.lat, "[A-Z]", String.Empty)), CDbl(Regex.Replace(bglAirport.alt, "[A-Z]", String.Empty)))
        pAirportHeliport.ARP = New ElevatedPoint
        pAirportHeliport.ARP.Geo.Assign(coord)

        '== end airport part

        '== start runway part
        Dim bglRunwayExt() As bglRunwayExt

        For i = LBound(bglAirport.Runway()) To UBound(bglAirport.Runway())
            ReDim Preserve bglRunwayExt(i)
            bglRunwayExt(i) = New bglRunwayExt
            bglRunwayExt(i).Runway = bglAirport.Runway()(i)

            Dim list As List(Of ctStart) = bglAirport.Start().Where(Function(item) item.number = bglRunwayExt(i).Runway.number And item.designator = bglRunwayExt(i).Runway.designator).ToList()
            'bglRunwayExt(i).Start(0) = New ctStart
            bglRunwayExt(i).Start(0) = list(0) 'temp

            If bglRunwayExt(i).Start(0).number < 19 + 10 Then
                bglRunwayExt(i).Start(1).number = bglRunwayExt(i).Start(0).number + 18
                'bglRunwayExt(i).Runway.designator = (bglRunwayExt(i).Start(0).number - 10).ToString + "-" + (bglRunwayExt(i).Start(1).number - 10).ToString
            Else
                bglRunwayExt(i).Start(1).number = bglRunwayExt(i).Start(0).number - 18
                'bglRunwayExt(i).Runway.designator = (bglRunwayExt(i).Start(1).number - 10).ToString + "-" + (bglRunwayExt(i).Start(0).number - 10).ToString
            End If

            Select Case bglRunwayExt(i).Start(0).designator
                Case stDesignator.LEFT : bglRunwayExt(i).Start(1).designator = stDesignator.RIGHT
                Case stDesignator.RIGHT : bglRunwayExt(i).Start(1).designator = stDesignator.LEFT
                Case stDesignator.L : bglRunwayExt(i).Start(1).designator = stDesignator.R
                Case stDesignator.R : bglRunwayExt(i).Start(1).designator = stDesignator.L
                Case Else : bglRunwayExt(i).Start(1).designator = bglRunwayExt(i).Start(0).designator
            End Select

            list = bglAirport.Start().Where(Function(item) item.number = bglRunwayExt(i).Start(1).number And item.designator = bglRunwayExt(i).Start(1).designator).ToList()
            bglRunwayExt(i).Start(1) = list(0)


            'Dim list3 As List(Of Integer) = bglAirport.Start().Where(Function(item) item.number = bglRunwayExt(i).Start(1).number And item.designator = bglRunwayExt(i).Start(1).designator).ToList()
        Next


        Dim pRunway() As Runway
        Dim pRunwayElement() As RunwayElement
        Dim pRunwayDirection(,) As RunwayDirection
        Dim pRunwayCentrelinePoint(,) As RunwayCentrelinePoint

        For i = LBound(bglRunwayExt) To UBound(bglRunwayExt)
            ReDim Preserve pRunway(i)
            pRunway(i) = New Runway
            pRunway(i).TimeSlice = ts
            pRunway(i).Identifier = Guid.NewGuid
            pRunway(i).AssociatedAirportHeliport = New FeatureRef()
            pRunway(i).AssociatedAirportHeliport.Identifier = pAirportHeliport.Identifier
            pRunway(i).AssociatedAirportHeliport.FeatureType = FeatureType.AirportHeliport
            pRunway(i).NominalLength = trd(bglRunwayExt(i).Runway.length)
            pRunway(i).NominalWidth = trd(bglRunwayExt(i).Runway.width)
            pRunway(i).Designator = (bglRunwayExt(i).Start(0).number - 10).ToString + "-" + (bglRunwayExt(i).Start(1).number - 10).ToString

            ReDim Preserve pRunwayElement(i)
            pRunwayElement(i) = New RunwayElement
            pRunwayElement(i).TimeSlice = ts
            pRunwayElement(i).Identifier = Guid.NewGuid
            pRunwayElement(i).AssociatedRunway.Add(New FeatureRefObject(pRunway(i).Identifier))
            pRunwayElement(i).Length = trd(bglRunwayExt(i).Runway.length)
            pRunwayElement(i).Width = trd(bglRunwayExt(i).Runway.width)
            pRunwayElement(i).Extent = New ElevatedSurface
            pRunwayElement(i).Extent.Geo.Assign(runway_geometry(bglRunwayExt(i).Runway.lon, bglRunwayExt(i).Runway.lat, _
                                                                bglRunwayExt(i).Runway.heading, pRunwayElement(i).Length.Value, pRunwayElement(i).Width.Value))


            ReDim Preserve pRunwayDirection(1, i)
            pRunwayDirection(0, i) = New RunwayDirection
            pRunwayDirection(0, i).TimeSlice = ts
            pRunwayDirection(0, i).Identifier = Guid.NewGuid
            pRunwayDirection(0, i).UsedRunway = New FeatureRef()
            pRunwayDirection(0, i).UsedRunway.Identifier = pRunway(i).Identifier
            pRunwayDirection(0, i).UsedRunway.FeatureType = FeatureType.Runway
            pRunwayDirection(0, i).TrueBearing = bglRunwayExt(i).Runway.heading

            pRunwayDirection(1, i) = New RunwayDirection
            pRunwayDirection(1, i).TimeSlice = ts
            pRunwayDirection(1, i).Identifier = Guid.NewGuid
            pRunwayDirection(1, i).UsedRunway = New FeatureRef()
            pRunwayDirection(1, i).UsedRunway.Identifier = pRunway(i).Identifier
            pRunwayDirection(1, i).UsedRunway.FeatureType = FeatureType.Runway
            pRunwayDirection(1, i).TrueBearing = bglRunwayExt(i).Runway.heading + 180

            ReDim Preserve pRunwayCentrelinePoint(5, i)
            pRunwayCentrelinePoint(0, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(0, i).TimeSlice = ts
            pRunwayCentrelinePoint(0, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(0, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(0, i).OnRunway.Identifier = pRunwayDirection(0, i).Identifier
            pRunwayCentrelinePoint(0, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            coord = New Aran.Geometries.Point(CDbl(trd(bglRunwayExt(i).Start(0).lon).Value), CDbl(trd(bglRunwayExt(i).Start(0).lat).Value), CDbl(trdv(bglRunwayExt(i).Start(0).alt).Value))
            pRunwayCentrelinePoint(0, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(0, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(0, i).Role = CodeRunwayPointRole.START


            pRunwayCentrelinePoint(1, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(1, i).TimeSlice = ts
            pRunwayCentrelinePoint(1, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(1, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(1, i).OnRunway.Identifier = pRunwayDirection(0, i).Identifier
            pRunwayCentrelinePoint(1, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            pRunwayCentrelinePoint(1, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(1, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(1, i).Role = CodeRunwayPointRole.THR

            pRunwayCentrelinePoint(2, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(2, i).TimeSlice = ts
            pRunwayCentrelinePoint(2, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(2, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(2, i).OnRunway.Identifier = pRunwayDirection(0, i).Identifier
            pRunwayCentrelinePoint(2, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            coord = New Aran.Geometries.Point(CDbl(trd(bglRunwayExt(i).Start(1).lon).Value), CDbl(trd(bglRunwayExt(i).Start(1).lat).Value), CDbl(trdv(bglRunwayExt(i).Start(1).alt).Value))
            pRunwayCentrelinePoint(2, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(2, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(2, i).Role = CodeRunwayPointRole.END


            pRunwayCentrelinePoint(3, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(3, i).TimeSlice = ts
            pRunwayCentrelinePoint(3, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(3, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(3, i).OnRunway.Identifier = pRunwayDirection(1, i).Identifier
            pRunwayCentrelinePoint(3, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            coord = New Aran.Geometries.Point(CDbl(trd(bglRunwayExt(i).Start(1).lon).Value), CDbl(trd(bglRunwayExt(i).Start(1).lat).Value), CDbl(trdv(bglRunwayExt(i).Start(1).alt).Value))
            pRunwayCentrelinePoint(3, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(3, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(3, i).Role = CodeRunwayPointRole.START

            pRunwayCentrelinePoint(4, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(4, i).TimeSlice = ts
            pRunwayCentrelinePoint(4, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(4, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(4, i).OnRunway.Identifier = pRunwayDirection(1, i).Identifier
            pRunwayCentrelinePoint(4, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            pRunwayCentrelinePoint(4, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(4, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(4, i).Role = CodeRunwayPointRole.THR

            pRunwayCentrelinePoint(5, i) = New RunwayCentrelinePoint
            pRunwayCentrelinePoint(5, i).TimeSlice = ts
            pRunwayCentrelinePoint(5, i).Identifier = Guid.NewGuid
            pRunwayCentrelinePoint(5, i).OnRunway = New FeatureRef()
            pRunwayCentrelinePoint(5, i).OnRunway.Identifier = pRunwayDirection(1, i).Identifier
            pRunwayCentrelinePoint(5, i).OnRunway.FeatureType = FeatureType.RunwayDirection
            coord = New Aran.Geometries.Point(CDbl(trd(bglRunwayExt(i).Start(0).lon).Value), CDbl(trd(bglRunwayExt(i).Start(0).lat).Value), CDbl(trdv(bglRunwayExt(i).Start(0).alt).Value))
            pRunwayCentrelinePoint(5, i).Location = New ElevatedPoint
            pRunwayCentrelinePoint(5, i).Location.Geo.Assign(coord)
            pRunwayCentrelinePoint(5, i).Role = CodeRunwayPointRole.END

        Next

        Dim bglTaxiwayParking() As ctTaxiwayParking
        Dim pAircraftStand() As AircraftStand
        Dim dictAircraftStand As New Dictionary(Of String, Guid)

        bglTaxiwayParking = bglAirport.TaxiwayParking

        Dim pntArray() As Double
        For i = 0 To bglTaxiwayParking.Length - 1
            ReDim Preserve pAircraftStand(i)
            pAircraftStand(i) = New AircraftStand
            pAircraftStand(i).TimeSlice = ts
            pAircraftStand(i).Identifier = Guid.NewGuid
            pAircraftStand(i).Location = New ElevatedPoint
            coord = New Aran.Geometries.Point(CDbl(trd(bglTaxiwayParking(i).lon).Value), CDbl(trd(bglTaxiwayParking(i).lat).Value), CDbl(0.0))
            pAircraftStand(i).Location.Geo.Assign(coord)
            dictAircraftStand.Add(bglTaxiwayParking(i).index, pAircraftStand(i).Identifier)

            For j = 0 To 359
                Dim azmt As Double = j, resX As Double, resY As Double
                AranMathFunctions.PointAlongGeodesic(pAircraftStand(i).Location.Geo.X, pAircraftStand(i).Location.Geo.Y, trd(bglTaxiwayParking(i).radius).Value, azmt, resX, resY)
                ReDim Preserve pntArray(j * 2 + 1)
                pntArray(j * 2) = resX
                pntArray(j * 2 + 1) = resY
            Next
            pAircraftStand(i).Extent = New ElevatedSurface
            pAircraftStand(i).Extent.Geo.Assign(create_polygon(pntArray))
        Next



        Dim bglTaxiwayPoint() As ctTaxiwayPoint
        Dim pTaxiHoldingPosition() As TaxiHoldingPosition
        Dim dictTaxiwayPoint As New Dictionary(Of String, Guid)

        bglTaxiwayPoint = bglAirport.TaxiwayPoint().Where(Function(item) item.type = stTaxiPointType.HOLD_SHORT).ToArray()

        For i = 0 To bglTaxiwayPoint.Length - 1
            ReDim Preserve pTaxiHoldingPosition(i)
            pTaxiHoldingPosition(i) = New TaxiHoldingPosition
            pTaxiHoldingPosition(i).TimeSlice = ts
            pTaxiHoldingPosition(i).Identifier = Guid.NewGuid
            pTaxiHoldingPosition(i).Location = New ElevatedPoint
            coord = New Aran.Geometries.Point(CDbl(trd(bglTaxiwayPoint(i).lon).Value), CDbl(trd(bglTaxiwayPoint(i).lat).Value), CDbl(0.0))
            pTaxiHoldingPosition(i).Location.Geo.Assign(coord)
            dictTaxiwayPoint.Add(bglTaxiwayPoint(i).index, pTaxiHoldingPosition(i).Identifier)

            Dim jmin As Long
            For j = 0 To pRunwayElement.Length - 1
                Dim distval As Double, near_x As Double, near_y As Double
                Dim distvalmin As Double = 100000
                distval = DistToSegment(pTaxiHoldingPosition(i).Location.Geo.X, pTaxiHoldingPosition(i).Location.Geo.Y, _
                              pRunwayElement(j).Extent.Geo.Item(0).ExteriorRing.Item(0).X, pRunwayElement(j).Extent.Geo.Item(0).ExteriorRing.Item(0).Y, _
                              pRunwayElement(j).Extent.Geo.Item(0).ExteriorRing.Item(2).X, pRunwayElement(j).Extent.Geo.Item(0).ExteriorRing.Item(2).Y, _
                              near_x, near_y)
                If distval < distvalmin Then distvalmin = distval : jmin = j
            Next

            'pTaxiHoldingPosition(i).AssociatedGuidanceLine.Identifier
            'pTaxiHoldingPosition(i).ProtectedRunway.


        Next




        '== end runway part

        '== start taxy part

        Dim bglTaxiwayExt() As bglTaxiwayExt

        Dim pTaxiway() As Taxiway
        Dim pTaxiwayElement() As TaxiwayElement
        Dim pTaxiwayMarking() As TaxiwayMarking
        Dim pGuidanceLine() As GuidanceLine

        Dim bglTaxiway As ctTaxiwayPath
        'Dim bglTaxiwayPoint() As ctTaxiwayPoint
        Dim bglTaxiName() As ctTaxiName
        'Dim bglTaxiwayParking() As ctTaxiwayParking

        Dim idx As Long
        Dim start_bglTaxiwayPoint As ctTaxiwayPoint
        Dim end_bglTaxiwayPoint As ctTaxiwayPoint

        'bglTaxiwayPoint = bglAirport.TaxiwayPoint
        bglTaxiName = bglAirport.TaxiName
        bglTaxiwayParking = bglAirport.TaxiwayParking

        For i = LBound(bglAirport.TaxiwayPath()) To UBound(bglAirport.TaxiwayPath())

            ReDim Preserve bglTaxiwayExt(i)
            bglTaxiwayExt(i) = New bglTaxiwayExt
            bglTaxiwayExt(i).TaxiwayPath = bglAirport.TaxiwayPath(i)

            bglTaxiwayExt(i).TaxiwayPoint(0) = bglAirport.TaxiwayPoint().Where(Function(item) item.index = bglTaxiwayExt(i).TaxiwayPath.start).ToList()(0)
            bglTaxiwayExt(i).TaxiwayPoint(1) = bglAirport.TaxiwayPoint().Where(Function(item) item.index = bglTaxiwayExt(i).TaxiwayPath.end).ToList().FirstOrDefault
            bglTaxiwayExt(i).TaxiwayParking = bglAirport.TaxiwayParking().Where(Function(item) item.index = bglTaxiwayExt(i).TaxiwayPath.end).ToList().FirstOrDefault
            bglTaxiwayExt(i).TaxiName = bglAirport.TaxiName().Where(Function(item) item.index = bglTaxiwayExt(i).TaxiwayPath.name).FirstOrDefault()


        Next

        For i = 0 To bglTaxiwayExt.Length - 1
            bglTaxiwayExt(i).linkend = New List(Of Long)
            bglTaxiwayExt(i).linkstart = New List(Of Long)
            For j = 0 To bglTaxiwayExt.Length - 1
                If bglTaxiwayExt(i).TaxiwayPath.end = bglTaxiwayExt(j).TaxiwayPath.start Then bglTaxiwayExt(i).linkend.Add(j)
                If bglTaxiwayExt(i).TaxiwayPath.start = bglTaxiwayExt(j).TaxiwayPath.end Then bglTaxiwayExt(i).linkstart.Add(j)
            Next
        Next

        For i = 0 To bglTaxiwayExt.Length - 1
            For j = 0 To bglTaxiwayExt(i).linkend.Count - 1
                Debug.Print(GetAngle(bglTaxiwayExt(i).TaxiwayPoint(0).lon, bglTaxiwayExt(i).TaxiwayPoint(0).lat, _
                                     bglTaxiwayExt(i).TaxiwayPoint(1).lon, bglTaxiwayExt(i).TaxiwayPoint(1).lat, _
                                     bglTaxiwayExt(bglTaxiwayExt(i).linkend.Item(j)).TaxiwayPoint(1).lon, bglTaxiwayExt(bglTaxiwayExt(i).linkend.Item(j)).TaxiwayPoint(1).lat) _
                            )
            Next

            For j = 0 To bglTaxiwayExt(i).linkstart.Count - 1
                Debug.Print(GetAngle(bglTaxiwayExt(i).TaxiwayPoint(0).lon, bglTaxiwayExt(i).TaxiwayPoint(0).lat, _
                                     bglTaxiwayExt(i).TaxiwayPoint(1).lon, bglTaxiwayExt(i).TaxiwayPoint(1).lat, _
                                     bglTaxiwayExt(bglTaxiwayExt(i).linkstart.Item(j)).TaxiwayPoint(1).lon, bglTaxiwayExt(bglTaxiwayExt(i).linkstart.Item(j)).TaxiwayPoint(1).lat) _
                            )
            Next
        Next





        Dim list1 As List(Of bglTaxiwayExt)
        bglTaxiName = bglAirport.TaxiName

        Dim bglTaxiwayComplex() As bglTaxiwayComplex

        Dim k As Long = 0


        For i = 0 To bglTaxiName.Length - 1

            ReDim Preserve bglTaxiwayComplex(k)
            list1 = bglTaxiwayExt.Where(Function(item)
                                            If item.TaxiName Is Nothing Then Return False
                                            Return item.TaxiName.name = bglTaxiName(i).name
                                        End Function).ToList()

            bglTaxiwayComplex(k) = New bglTaxiwayComplex
            bglTaxiwayComplex(k).TaxiwayComplex = list1

            k += 1
        Next

        k = UBound(bglTaxiwayComplex) + 1



        For i = 0 To bglRunwayExt.Length - 1
            ReDim Preserve bglTaxiwayComplex(k)
            list1 = bglTaxiwayExt.Where(Function(item)
                                            If item.TaxiwayPath Is Nothing Then Return False
                                            Return item.TaxiwayPath.type = stTaxiwayPathType.RUNWAY And item.TaxiwayPath.number = bglRunwayExt(i).Runway.number
                                        End Function).ToList()


            bglTaxiwayComplex(k) = New bglTaxiwayComplex
            bglTaxiwayComplex(k).TaxiwayComplex = list1

            k += 1
        Next



        k = UBound(bglTaxiwayComplex) + 1
        ReDim Preserve bglTaxiwayComplex(k)
        list1 = bglTaxiwayExt.Where(Function(item)
                                        If item.TaxiwayPath Is Nothing Then Return False
                                        Return item.TaxiwayPath.type = stTaxiwayPathType.PARKING
                                    End Function).ToList()


        bglTaxiwayComplex(k) = New bglTaxiwayComplex
        bglTaxiwayComplex(k).TaxiwayComplex = list1


        k = UBound(bglTaxiwayComplex) + 1
        ReDim Preserve bglTaxiwayComplex(k)
        list1 = bglTaxiwayExt.Where(Function(item)
                                        If item.TaxiwayPath Is Nothing Then Return False
                                        Return item.TaxiwayPath.type = stTaxiwayPathType.PATH
                                    End Function).ToList()


        bglTaxiwayComplex(k) = New bglTaxiwayComplex
        bglTaxiwayComplex(k).TaxiwayComplex = list1


        k = 0
        For i = 0 To bglTaxiwayComplex.Length - 1

            ReDim Preserve pTaxiway(i)
            pTaxiway(i) = New Taxiway
            pTaxiway(i).TimeSlice = ts
            pTaxiway(i).Identifier = Guid.NewGuid
            pTaxiway(i).AssociatedAirportHeliport = New FeatureRef()
            pTaxiway(i).AssociatedAirportHeliport.Identifier = pAirportHeliport.Identifier
            pTaxiway(i).AssociatedAirportHeliport.FeatureType = FeatureType.AirportHeliport

            For j = 0 To bglTaxiwayComplex(i).TaxiwayComplex.Count - 1
                ReDim Preserve pTaxiwayElement(k)
                pTaxiwayElement(k) = New TaxiwayElement
                pTaxiwayElement(k).TimeSlice = ts
                pTaxiwayElement(k).Identifier = Guid.NewGuid
                pTaxiwayElement(k).AssociatedTaxiway = New FeatureRef()
                pTaxiwayElement(k).AssociatedTaxiway.Identifier = pTaxiway(i).Identifier
                pTaxiwayElement(k).AssociatedTaxiway.FeatureType = FeatureType.Taxiway

                pTaxiwayElement(k).Width = trd(bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPath.width)
                pTaxiwayElement(k).Extent = New ElevatedSurface

                geoattr.res = NativeMethods.ReturnGeodesicAzimuth(bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lat, _
                                                                  bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lat, _
                                                                  geoattr.azm_d, geoattr.azm_i)
                geoattr.way_offset = 5

                geoattr.res = NativeMethods.PointAlongGeodesic(bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lat, _
                                                                geoattr.way_offset, geoattr.azm_i, geoattr.lon0, geoattr.lat0)




                If bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPath.type = stTaxiwayPathType.PARKING Then

                    pTaxiwayElement(k).Extent.Geo.Assign(taxiway_geometry(bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lat, _
                                                                          bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayParking.lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayParking.lat, pTaxiwayElement(j).Width.Value))
                Else
                    pTaxiwayElement(k).Extent.Geo.Assign(taxiway_geometry(bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lat, _
                                                                          bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lat, pTaxiwayElement(j).Width.Value))
                End If

                ReDim Preserve pGuidanceLine(k)
                pGuidanceLine(k) = New GuidanceLine
                pGuidanceLine(k).TimeSlice = ts
                pGuidanceLine(k).Identifier = Guid.NewGuid
                pGuidanceLine(k).ConnectedTaxiway.Add(New FeatureRefObject(pTaxiway(i).Identifier))
                pGuidanceLine(k).Extent = New ElevatedCurve

                If bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPath.type = stTaxiwayPathType.PARKING Then

                    pGuidanceLine(k).Extent.Geo.Assign(create_polyline({bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lat, _
                                                                          bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayParking.lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayParking.lat}))
                Else
                    pGuidanceLine(k).Extent.Geo.Assign(create_polyline({bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(0).lat, _
                                                                          bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lon, bglTaxiwayComplex(i).TaxiwayComplex.Item(j).TaxiwayPoint(1).lat}))
                End If

                k = UBound(pTaxiwayElement) + 1

            Next


        Next



        Dim m_Points() As PointF = {}

        Dim bglApron() As ctApron
        bglApron = bglAirport.Aprons()
        Dim pAprone() As Apron
        Dim pAproneElement() As ApronElement
        Dim bglVertexLL As ctVertexLL
        For i = 0 To bglApron.Length - 1
            ReDim Preserve pAprone(i)
            pAprone(i) = New Apron
            pAprone(i).TimeSlice = ts
            pAprone(i).Identifier = Guid.NewGuid
            pAprone(i).AssociatedAirportHeliport = New FeatureRef
            pAprone(i).AssociatedAirportHeliport.Identifier = pAirportHeliport.Identifier
            pAprone(i).AssociatedAirportHeliport.FeatureType = FeatureType.AirportHeliport

            ReDim Preserve pAproneElement(i)
            pAproneElement(i) = New ApronElement
            pAproneElement(i).TimeSlice = ts
            pAproneElement(i).Identifier = Guid.NewGuid
            pAproneElement(i).AssociatedApron = New FeatureRef
            pAproneElement(i).AssociatedApron.Identifier = pAprone(i).Identifier
            pAproneElement(i).AssociatedApron.FeatureType = FeatureType.Apron

            k = 0
            ReDim pntArray(k)

            Dim k_m As Long = 0
            ReDim m_Points(k_m)

            For Each bglVertexLL In bglApron(i).Vertex
                ReDim Preserve pntArray(k + 1)
                pntArray(k) = bglVertexLL.lon
                pntArray(k + 1) = bglVertexLL.lat
                k = k + 2

                ReDim Preserve m_Points(k_m)
                m_Points(k_m).X = bglVertexLL.lon
                m_Points(k_m).Y = bglVertexLL.lat
                k_m += 1
            Next

            pAproneElement(i).Extent = New ElevatedSurface
            pAproneElement(i).Extent.Geo.Assign(create_polygon(pntArray))

            For k = 0 To pAircraftStand.Length - 1
                If PointInPolygon(m_Points, pAircraftStand(k).Location.Geo.X, pAircraftStand(k).Location.Geo.Y) = True Then
                    pAircraftStand(k).ApronLocation = New FeatureRef
                    pAircraftStand(k).ApronLocation.Identifier = pAproneElement(i).Identifier
                    pAircraftStand(k).ApronLocation.FeatureType = FeatureType.ApronElement

                End If
            Next


        Next


        'Dim dbPro As Aran.Aim.Data.DbProvider = Aran.Aim.Data.DbProviderFactory.Create("Aran.Aim.Data.PgDbProviderComplex.dll")
        'dbPro.Open("server=localhost; port=5432; user id=aran; password=airnav2012; database=simf")
        'If Not dbPro.Login("administrator", Aran.Aim.Data.DbUtility.GetMd5Hash("aim_administrator")) Then MsgBox("unssucsefilful connection") : Exit Sub

        Dim dbPro As Aran.Aim.Data.DbProvider = Aran.Aim.Data.DbProviderFactory.Create("Aran.Temporality.Provider")
        dbPro.CallSpecialMethod("SetApplicationEnvironment", Nothing)
        dbPro.Open(Nothing)


        Dim ir As Aim.Data.InsertingResult

        ir = dbPro.Insert(pAirportHeliport)
        If Not ir.IsSucceed Then MsgBox("not IsSucceed") : Exit Sub

        For Each item In pRunway
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pRunwayElement
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pTaxiway
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pTaxiwayElement
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        'For Each item In pTaxiwayMarking
        '    ir = dbPro.Insert(item)
        '    If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        'Next

        For Each item In pGuidanceLine
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pTaxiHoldingPosition
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pAprone
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pAproneElement
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        For Each item In pAircraftStand
            ir = dbPro.Insert(item)
            If Not ir.IsSucceed Then MsgBox(item.Identifier.ToString & "not IsSucceed") : Exit Sub
        Next

        status_line.Text = "finish"

    End Sub

    Function runway_geometry(x As Double, y As Double, ang As Double, l As Double, w As Double) As Aran.Geometries.MultiPolygon

        Dim x1 As Double, y1 As Double, x2 As Double, y2 As Double, _
            x3 As Double, y3 As Double, x4 As Double, y4 As Double
        'Dim x_(3) As Double : Dim y_(3) As Double

        ang = -deg2rad(ang)

        x1 = +w / 2.0 : y1 = +l / 2.0
        x2 = -w / 2.0 : y2 = +l / 2.0
        x3 = -w / 2.0 : y3 = -l / 2.0
        x4 = +w / 2.0 : y4 = -l / 2.0

        Dim xy_utm() As Double, zn_utm As Double, x_utm As Double, y_utm As Double
        xy_utm = wgs2utm(y, x, x)
        zn_utm = xy_utm(0)
        y_utm = xy_utm(1)
        x_utm = xy_utm(2)

        Dim xy_rotate() As Double, xy_move() As Double
        xy_rotate = rotate(x1, y1, ang)
        x1 = xy_rotate(0) : y1 = xy_rotate(1)
        xy_rotate = rotate(x2, y2, ang)
        x2 = xy_rotate(0) : y2 = xy_rotate(1)
        xy_rotate = rotate(x3, y3, ang)
        x3 = xy_rotate(0) : y3 = xy_rotate(1)
        xy_rotate = rotate(x4, y4, ang)
        x4 = xy_rotate(0) : y4 = xy_rotate(1)

        xy_move = Me.move(x1, y1, x_utm, y_utm)
        x1 = xy_move(0) : y1 = xy_move(1)
        xy_move = Me.move(x2, y2, x_utm, y_utm)
        x2 = xy_move(0) : y2 = xy_move(1)
        xy_move = Me.move(x3, y3, x_utm, y_utm)
        x3 = xy_move(0) : y3 = xy_move(1)
        xy_move = Me.move(x4, y4, x_utm, y_utm)
        x4 = xy_move(0) : y4 = xy_move(1)

        'zn_utm=x
        Dim xy_latlon() As Double
        xy_latlon = utm2wgs(y, zn_utm, y1, x1, x)
        x1 = xy_latlon(1) : y1 = xy_latlon(0)
        xy_latlon = utm2wgs(y, zn_utm, y2, x2, x)
        x2 = xy_latlon(1) : y2 = xy_latlon(0)
        xy_latlon = utm2wgs(y, zn_utm, y3, x3, x)
        x3 = xy_latlon(1) : y3 = xy_latlon(0)
        xy_latlon = utm2wgs(y, zn_utm, y4, x4, x)
        x4 = xy_latlon(1) : y4 = xy_latlon(0)

        'Dim coordinates() As Double = 
        Return create_polygon({x1, y1, x2, y2, x3, y3, x4, y4})

    End Function
    Function taxiway_geometry(x1 As Double, y1 As Double, x2 As Double, y2 As Double, w As Double) As Aran.Geometries.MultiPolygon

        w = w / 2.0

        Dim x_cen As Double, y_cen As Double, ang As Double
        x_cen = (CDbl(x1) + CDbl(x2)) / 2.0
        y_cen = (CDbl(y1) + CDbl(y2)) / 2.0

        Dim xy_utm() As Double, zn_utm As Double, _
            x_utm1 As Double, y_utm1 As Double, _
            x_utm2 As Double, y_utm2 As Double

        xy_utm = wgs2utm(y1, x1, x_cen)
        zn_utm = xy_utm(0)
        y_utm1 = xy_utm(1)
        x_utm1 = xy_utm(2)


        xy_utm = wgs2utm(y2, x2, x_cen)
        zn_utm = xy_utm(0)
        y_utm2 = xy_utm(1)
        x_utm2 = xy_utm(2)

        ang = Math.Atan((y_utm2 - y_utm1) / (x_utm2 - x_utm1))

        Dim x_n1 As Double, y_n1 As Double, x_n2 As Double, y_n2 As Double, _
            x_n3 As Double, y_n3 As Double, x_n4 As Double, y_n4 As Double

        Dim xy_rotate() As Double, xy_move() As Double
        xy_rotate = rotate(w, 0, ang + deg2rad(90))
        x_n1 = xy_rotate(0) : y_n1 = xy_rotate(1)
        xy_move = move(x_n1, y_n1, x_utm1, y_utm1)
        x_n1 = xy_move(0) : y_n1 = xy_move(1)

        xy_rotate = rotate(w, 0, ang - deg2rad(90))
        x_n2 = xy_rotate(0) : y_n2 = xy_rotate(1)
        xy_move = move(x_n2, y_n2, x_utm1, y_utm1)
        x_n2 = xy_move(0) : y_n2 = xy_move(1)

        xy_rotate = rotate(w, 0, ang - deg2rad(90))
        x_n3 = xy_rotate(0) : y_n3 = xy_rotate(1)
        xy_move = move(x_n3, y_n3, x_utm2, y_utm2)
        x_n3 = xy_move(0) : y_n3 = xy_move(1)

        xy_rotate = rotate(w, 0, ang + deg2rad(90))
        x_n4 = xy_rotate(0) : y_n4 = xy_rotate(1)
        xy_move = move(x_n4, y_n4, x_utm2, y_utm2)
        x_n4 = xy_move(0) : y_n4 = xy_move(1)


        'zn_utm=x
        Dim xy_latlon
        xy_latlon = utm2wgs(y_cen, zn_utm, y_n1, x_n1, x_cen)
        x_n1 = xy_latlon(1) : y_n1 = xy_latlon(0)
        xy_latlon = utm2wgs(y_cen, zn_utm, y_n2, x_n2, x_cen)
        x_n2 = xy_latlon(1) : y_n2 = xy_latlon(0)

        xy_latlon = utm2wgs(y_cen, zn_utm, y_n3, x_n3, x_cen)
        x_n3 = xy_latlon(1) : y_n3 = xy_latlon(0)
        xy_latlon = utm2wgs(y_cen, zn_utm, y_n4, x_n4, x_cen)
        x_n4 = xy_latlon(1) : y_n4 = xy_latlon(0)

        Return create_polygon({x_n1, y_n1, x_n2, y_n2, x_n3, y_n3, x_n4, y_n4})

    End Function


    Private Function create_polyline(ByVal coordinates() As Double) As Aran.Geometries.MultiLineString
        Dim valarr() As Double
        valarr = coordinates

        Dim pnt As Aran.Geometries.Point
        Dim pline As New Aran.Geometries.LineString()


        For i = LBound(valarr) To UBound(valarr) Step 2
            'Console.WriteLine(valarr(i))
            pnt = New Aran.Geometries.Point(valarr(i), valarr(i + 1))
            pline.Add(pnt)
        Next
        Dim mltplygon = New Aran.Geometries.MultiLineString()
        mltplygon.Add(pline)
        Return mltplygon

    End Function
    Private Function create_polygon(ByVal coordinates() As Double) As Aran.Geometries.MultiPolygon

        Dim valarr() As Double
        valarr = coordinates

        Dim pnt As Aran.Geometries.Point
        Dim plygon As New Aran.Geometries.Polygon()
        plygon.ExteriorRing = New Geometries.Ring()

        For i = LBound(valarr) To UBound(valarr) Step 2
            'Console.WriteLine(valarr(i))
            pnt = New Aran.Geometries.Point(valarr(i), valarr(i + 1))
            plygon.ExteriorRing.Add(pnt.Clone)
        Next
        Dim mltplygon = New Aran.Geometries.MultiPolygon()
        mltplygon.Add(plygon)
        Return mltplygon

    End Function
    Private Function rotate(x As Double, y As Double, ang As Double) As Double()
        Dim xr As Double, yr As Double
        xr = CDbl(x) * Math.Cos(ang) - CDbl(y) * Math.Sin(ang)
        yr = CDbl(x) * Math.Sin(ang) + CDbl(y) * Math.Cos(ang)
        Return New Double() {xr, yr}
    End Function

    Private Shadows Function move(x As Double, y As Double, dx As Double, dy As Double)
        Dim xm As Double, ym As Double
        xm = CDbl(x) + CDbl(dx)
        ym = CDbl(y) + CDbl(dy)
        Return New Double() {xm, ym}
    End Function


    Private Function deg2rad(ang As Double) As Double
        deg2rad = ang / 180 * Math.PI
    End Function

    Private Function wgs2utm(lat As Double, lon As Double, c_m As Double) As Double()
        ' LATLON to UTM
        'convert latitude, longitude into UTM projection
        'return is  UTM = zone & "|" & north & "|" & east
        'http://en.wikipedia.org/wiki/Universal_Transverse_Mercator_coordinate_system

        Dim a As Double, b As Double, ff As Double, finv As Double, rm As Double, _
            k0 As Double, EE As Double, e1sq As Double, NN As Double

        a = 6378137
        b = 6356752.314
        ff = 0.003352811
        finv = 298.2572236
        rm = 6367435.68
        k0 = 0.9996
        EE = 0.081819191
        e1sq = 0.006739497
        NN = 0.00167922

        Dim a0 As Double, b0 As Double, c0 As Double, d0 As Double, e0 As Double

        a0 = a * (1 - NN + (5 * NN * NN / 4) * (1 - NN) + (81 * NN ^ 4 / 64) * (1 - NN))
        b0 = (3 * a * NN / 2) * (1 - NN - (7 * NN * NN / 8) * (1 - NN) + 55 * NN ^ 4 / 64)
        c0 = (15 * a * NN * NN / 16) * (1 - NN + (3 * NN * NN / 4) * (1 - NN))
        d0 = (35 * a * NN ^ 3 / 48) * (1 - NN + 11 * NN * NN / 16)
        e0 = (315 * a * NN ^ 4 / 51) * (1 - NN)


        Dim n As Double, o As Double, p As Double, q As Double, r As Double

        n = 31 + Int(lon / 6)
        If IsNumeric(c_m) = True Then
            o = c_m
        Else
            o = 6 * n - 183
        End If

        p = (lon - o) * Math.PI / 180
        'p = (LON-LON) * math.pi / 180
        q = lat * Math.PI / 180

        r = lon * Math.PI / 180


        Dim s As Double, t As Double, v As Double

        s = a * (1 - EE * EE) / ((1 - (EE * Math.Sin(q)) ^ 2) ^ (3 / 2))
        t = a / ((1 - (EE * Math.Sin(q)) ^ 2) ^ (1 / 2))
        v = a0 * q - b0 * Math.Sin(2 * q) + c0 * Math.Sin(4 * q) - d0 * Math.Sin(6 * q) + e0 * Math.Sin(8 * q)


        Dim x As Double, y As Double, z As Double, AA As Double, AB As Double, AC As Double, AD As Double

        x = v * k0

        y = t * Math.Sin(q) * Math.Cos(q) / 2
        z = ((t * Math.Sin(q) * Math.Cos(q) ^ 3) / 24) * (5 - Math.Tan(q) ^ 2 + 9 * e1sq * Math.Cos(q) ^ 2 + 4 * e1sq ^ 2 * Math.Cos(q) ^ 4) * k0
        AA = t * Math.Cos(q) * k0
        AB = (Math.Cos(q)) ^ 3 * (t / 6) * (1 - Math.Tan(q) ^ 2 + e1sq * Math.Cos(q) ^ 2) * k0

        AC = ((p) ^ 6 * t * Math.Sin(q) * Math.Cos(q) ^ 5 / 720) * (61 - 58 * Math.Tan(q) ^ 2 + Math.Tan(q) ^ 4 + 270 * e1sq * Math.Cos(q) ^ 2 - 330 * e1sq * Math.Sin(q) ^ 2) * k0
        AD = (x + y * p * p + z * p ^ 4)

        Dim east As Double, north As Double, zone As Double

        If AD < 0 Then north = 10000000 + AD Else north = AD


        east = 500000 + (AA * p + AB * p ^ 3)
        zone = n


        Return New Double() {zone, north, east}


    End Function


    Private Function utm2wgs(ne As Double, zone As Double, NN As Double, EE As Double, c_m As Double) As Double()

        ' UTM to LATLON
        ' ne is "N" or "S"
        ' zone
        ' NN - NORTHING
        ' EE - EASTING
        ' return     KALK = LAT & "|" & LON
        Dim k0 As Double, b As Double, e As Double, a As Double, e1sq As Double

        k0 = 0.9996
        b = 6356752.314
        a = 6378137
        e = 0.081819191
        e1sq = 0.006739497


        Dim E1 As Double, C1 As Double, C2 As Double, C3 As Double, C4 As Double

        E1 = (1 - (1 - e * e) ^ (1 / 2)) / (1 + (1 - e * e) ^ (1 / 2))
        C1 = 3 * E1 / 2 - 27 * E1 ^ 3 / 32
        C2 = 21 * E1 ^ 2 / 16 - 55 * E1 ^ 4 / 32
        C3 = 151 * E1 ^ 3 / 96
        C4 = 1097 * E1 ^ 4 / 512



        Dim H As Double, i As Double, j As Double, K As Double

        If Math.Sign(ne) = 1 Then H = NN Else H = 10000000 - NN


        i = 500000 - EE
        j = NN / k0


        K = j / (a * (1 - e ^ 2 / 4 - 3 * e ^ 4 / 64 - 5 * e ^ 6 / 256))

        Dim L As Double, M As Double, n As Double, o As Double, p As Double

        Dim q As Double, r As Double, s As Double, t As Double, U As Double, v As Double, W As Double

        Dim x As Double, y As Double, z As Double

        Dim AA As Double, AB As Double, AC As Double


        L = K + C1 * Math.Sin(2 * K) + C2 * Math.Sin(4 * K) + C3 * Math.Sin(6 * K) + C4 * Math.Sin(8 * K)
        M = e1sq * Math.Cos(L) ^ 2
        n = Math.Tan(L) ^ 2
        o = a / (1 - (e * Math.Sin(L)) ^ 2) ^ (1 / 2)
        p = a * (1 - e * e) / (1 - (e * Math.Sin(L)) ^ 2) ^ (3 / 2)

        q = i / (o * k0)
        r = o * Math.Tan(L) / p
        s = q * q / 2
        t = (5 + 3 * n + 10 * M - 4 * M * M - 9 * e1sq) * q ^ 4 / 24
        U = (61 + 90 * n + 298 * M + 45 * n * n - 252 * e1sq - 3 * M * M) * q ^ 6 / 720

        v = q
        W = (1 + 2 * n + M) * q ^ 3 / 6

        x = (5 - 2 * M + 28 * n - 3 * M ^ 2 + 8 * e1sq + 24 * n ^ 2) * q ^ 5 / 120

        y = (v - W + x) / Math.Cos(L)

        If IsNumeric(c_m) = True Then z = c_m Else z = 6 * zone - 183

        'z=zone

        AA = 180 * (L - r * (s + t + U)) / Math.PI
        AB = 0
        If Math.Sign(ne) = 1 Then
            AB = AA
        Else
            AB = -AA
        End If
        AC = z - y * 180 / Math.PI
        'KALK = AB & "|" & AC
        Return New Double() {AB, AC}
    End Function

    Private Function trd(ByVal value As String) As ValDistance
        Return New ValDistance(CDbl(Replace(value, "M", "")), UomDistance.M)
    End Function

    Private Function trdv(ByVal value As String) As ValDistanceVertical
        Return New ValDistanceVertical(CDbl(Replace(value, "M", "")), UomDistanceVertical.M)
    End Function

    Private Sub LineShape1_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub xmlload_btn_Click(sender As Object, e As EventArgs) Handles xmlload_btn.Click

        If Not System.IO.File.Exists(xmlfile_input) = True Then Exit Sub

        Try
            Dim x As New XmlSerializer(bglFSData.GetType)

            Dim objStreamReader As New StreamReader(xmlfile_input.ToString)

            bglFSData = x.Deserialize(objStreamReader)
            objStreamReader.Close()

        Catch ex As Exception
            Console.WriteLine(ex.Message) ' MessageBox.Show(ex.Message)
            Console.WriteLine(ex.StackTrace) 'MessageBox.Show("Stack Trace: " & vbCrLf & ex.StackTrace)
            Exit Sub
        End Try

        Dim i As Integer


        For i = 0 To bglFSData.Airport.Length - 1

            adhplist_cmb.Items.Add(bglFSData.Airport()(i).ident)

        Next

        adhplist_cmb.SelectedIndex = 0

        Dim bglAirport As New ctAirport()
        bglAirport = bglFSData.Airport()(adhplist_cmb.SelectedIndex)

        lvwBooks.Items.Clear()
        ListViewMakeRow(lvwBooks, "Services", False, If(bglAirport.Services Is Nothing, "0", CStr(bglAirport.Services.Count)))
        ListViewMakeRow(lvwBooks, "Tower", False, If(bglAirport.Tower Is Nothing, "0", CStr(bglAirport.Tower.Count)))
        ListViewMakeRow(lvwBooks, "Runway", True, If(bglAirport.Runway Is Nothing, "0", CStr(bglAirport.Runway.Count)))
        ListViewMakeRow(lvwBooks, "Com", False, If(bglAirport.Com Is Nothing, "0", CStr(bglAirport.Com.Count)))
        ListViewMakeRow(lvwBooks, "TaxiwayPoint", True, If(bglAirport.TaxiwayPoint Is Nothing, "0", CStr(bglAirport.TaxiwayPoint.Count)))
        ListViewMakeRow(lvwBooks, "TaxiwayParking", True, If(bglAirport.TaxiwayParking Is Nothing, "0", CStr(bglAirport.TaxiwayParking.Count)))
        ListViewMakeRow(lvwBooks, "TaxiName", True, If(bglAirport.TaxiName Is Nothing, "0", CStr(bglAirport.TaxiName.Count)))
        ListViewMakeRow(lvwBooks, "TaxiwayPath", True, If(bglAirport.TaxiwayPath Is Nothing, "0", CStr(bglAirport.TaxiwayPath.Count)))
        ListViewMakeRow(lvwBooks, "Jetway", False, If(bglAirport.Jetway Is Nothing, "0", CStr(bglAirport.Jetway.Count)))
        ListViewMakeRow(lvwBooks, "Aprons", True, If(bglAirport.Aprons Is Nothing, "0", CStr(bglAirport.Aprons.Count)))
        ListViewMakeRow(lvwBooks, "ApronEdgeLights", False, If(bglAirport.ApronEdgeLights Is Nothing, "0", CStr(bglAirport.ApronEdgeLights.Count)))
        ListViewMakeRow(lvwBooks, "BoundaryFence", False, If(bglAirport.BoundaryFence Is Nothing, "0", CStr(bglAirport.BoundaryFence.Count)))
        ListViewMakeRow(lvwBooks, "Approach", False, If(bglAirport.Approach Is Nothing, "0", CStr(bglAirport.Approach.Count)))
        ListViewMakeRow(lvwBooks, "Waypoint", False, If(bglAirport.Waypoint Is Nothing, "0", CStr(bglAirport.Waypoint.Count)))
        ListViewMakeRow(lvwBooks, "Ndb", False, If(bglAirport.Ndb Is Nothing, "0", CStr(bglAirport.Ndb.Count)))

        Dim bglRunway As ctRunway
        Dim k As Integer = 0
        For i = 0 To bglAirport.Runway.Count - 1
            bglRunway = bglAirport.Runway(i)

            If Not bglRunway.Ils Is Nothing Then
                For j As Integer = 0 To bglRunway.Ils.Length - 1
                    k += 1
                Next
            End If

        Next


        ListViewMakeRow(lvwBooks, "Ils", False, k)

        For i = 0 To lvwBooks.Columns.Count - 1
            lvwBooks.Columns(i).Width = -2
        Next i
    End Sub

    Private Sub lvwBooks_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lvwBooks.ItemSelectionChanged

        If e.IsSelected Then e.Item.Selected = False 'And e.Item.Text = "Ndb"

    End Sub
End Class

' Implements a comparer for ListView columns.
Class ListViewComparer
    Implements IComparer

    Private m_ColumnNumber As Integer
    Private m_SortOrder As SortOrder

    Public Sub New(ByVal column_number As Integer, ByVal sort_order As SortOrder)
        m_ColumnNumber = column_number
        m_SortOrder = sort_order
    End Sub

    ' Compare the items in the appropriate column
    ' for objects x and y.
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim item_x As ListViewItem = DirectCast(x, ListViewItem)
        Dim item_y As ListViewItem = DirectCast(y, ListViewItem)

        ' Get the sub-item values.
        Dim string_x As String
        If item_x.SubItems.Count <= m_ColumnNumber Then
            string_x = ""
        Else
            string_x = item_x.SubItems(m_ColumnNumber).Text
        End If

        Dim string_y As String
        If item_y.SubItems.Count <= m_ColumnNumber Then
            string_y = ""
        Else
            string_y = item_y.SubItems(m_ColumnNumber).Text
        End If

        ' Compare them.
        If m_SortOrder = SortOrder.Ascending Then
            If IsNumeric(string_x) And IsNumeric(string_y) Then
                Return Val(string_x).CompareTo(Val(string_y))
            Else
                Return String.Compare(string_x, string_y)
            End If
        Else
            If IsNumeric(string_x) And IsNumeric(string_y) Then
                Return Val(string_y).CompareTo(Val(string_x))
            Else
                Return String.Compare(string_y, string_x)
            End If
        End If
    End Function
End Class