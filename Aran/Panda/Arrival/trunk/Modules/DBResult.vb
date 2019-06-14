Option Strict Off
Option Explicit On
Module DBResult
	
	Public Const FIXRoleFAF As Short = 0
	Public Const FIXRoleFAP As Short = 1
	Public Const FIXRoleIF As Short = 2
	Public Const FIXRoleSDF As Short = 3
	Public Const FIXRoleTP As Short = 4
	
	Public Const APP_FIX_LayerName As String = "APP_FIX"
	Public Const Def_TableName As String = "Definitions"
	
	'Public Function ESRIPolyToAIXMPoly(pPrjPolyLine As IPolyline) As AIXMCurve
	'Dim I                       As Long
	'Dim ptTmp                   As IPoint
	'Dim pPointCollection        As IPointCollection
	'
	'Dim pAIXMCurve              As IAIXMCurve
	'Dim pGMLPolyline            As IGMLPolyline
	'Dim pGMLPart                As IGMLPart
	'Dim pGMLPoint               As IGMLPoint
	'
	'    Set pPointCollection = pPrjPolyLine
	'
	'    Set pAIXMCurve = New AIXMCurve
	'    Set pGMLPolyline = New GMLPolyline
	'    Set pGMLPart = New GMLPart
	'
	'    For I = 0 To pPointCollection.PointCount - 1
	'        Set ptTmp = ToGeo(pPointCollection.Point(I))
	'        Set pGMLPoint = New gmlPoint
	'        pGMLPoint.PutCoord ptTmp.X, ptTmp.Y
	'        pGMLPart.Add pGMLPoint
	'    Next I
	'
	'    pGMLPolyline.Add pGMLPart
	'    Set pAIXMCurve.Polyline = pGMLPolyline
	'    Set ESRIPolyToAIXMPoly = pAIXMCurve
	'End Function
	
	Public Function CheckName(ByRef pCheckTable As ESRI.ArcGIS.Geodatabase.ITable, ByRef CheckItem As String) As Boolean
		Dim pQueryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
		Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow
		
		CheckName = False
		
		pQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter
		pQueryFilter.SubFields = "NAME"
		
		pQueryFilter.WhereClause = "NAME ='" & CheckItem & "'"
		pCursor = pCheckTable.Search(pQueryFilter, False)
		pRow = pCursor.NextRow
		CheckName = Not (pRow Is Nothing)
		
		pQueryFilter = Nothing
	End Function
	
	'Sub CheckRepidNameInput(LayerName As String, ItemName As String, _
	''                        FieldName As String, CheckN As Boolean, _
	''                        Optional NavType As String = "", _
	''                        Optional CheckNav As Boolean = False)
	'
	'Dim pFeatureLayer As IFeatureLayer
	'Dim pFeatureClass As IFeatureClass
	'Dim pTable As ITable
	'Dim I As Long
	'Dim N As Long
	'Dim pFieldName As Long
	'Dim pFieldType As Long
	'Dim bFlag As Boolean
	'
	'Dim pFilter As IQueryFilter
	'Dim pCursor As ICursor
	'Dim pRow As IRow
	
	'For I = 0 To pMap.LayerCount - 1
	'    If pMap.Layer(I).Name_2 = LayerName Then
	'        Set pFeatureLayer = pMap.Layer(I)
	'        bFlag = True
	'        Exit For
	'    End If
	'Next I
	
	'If Not bFlag Then
	'    msgbox LayerName + "Theme Not Found"
	'    Return
	'End If
	'
	'Set pFeatureClass = pFeatureLayer.FeatureClass
	'Set pTable = pFeatureClass
	'
	'Set pFilter = New QueryFilter
	'pFilter.WhereClause = pTable.OIDFieldName + ">=0"
	'N = pTable.RowCount(pFilter)
	'
	'Set pCursor = pTable.Search(pFilter, True)
	'pFieldName = pCursor.FindField(FieldName)
	'
	'CheckN = False
	'
	'Set pRow = pCursor.NextRow
	'Do While Not pRow Is Nothing
	'    If (pRow.Value(pFieldName) = ItemName) Then
	'        If CheckNav Then
	'            pFieldType = pCursor.FindField("Type")
	'            If (pRow.Value(pFieldType) = NavType) Then
	'                CheckN = True
	'                msgbox "Объект с данным названием уже существует в базе данных." + _
	''                "Измените " + FieldName + " объекта.", vbExclamation
	'                Set pFilter = Nothing
	'                Return
	'            End If
	'        Else
	'            CheckN = True
	'            msgbox "Объект с данным названием уже существует в базе данных." + _
	''            "Измените " + FieldName + " объекта.", vbExclamation
	'            Set pFilter = Nothing
	'            Return
	'        End If
	'    End If
	'    Set pRow = pCursor.NextRow
	'Loop
	'Set pFilter = Nothing
	'
	'End Sub

	Private Function CreateDefsTable(ByVal featWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.ITable
		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit
		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit

		If featWorkspace Is Nothing Then Return Nothing

		Try
			pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields

			'======================================= Object ID
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			With pFieldEdit
				.Name_2 = "OBJECTID"
				.AliasName_2 = "OBJECTID"
				.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
			End With
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= ADHP_ID
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "ADHP_ID"
			pFieldEdit.AliasName_2 = "ADHP_ID"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 48
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= WPT Name
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "WPT_Name"
			pFieldEdit.AliasName_2 = "WPT_Name"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 32
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= PROC Type
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "PROC_Type"
			pFieldEdit.AliasName_2 = "PROC_Type"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 16
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= PROC NAME
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "PROC_NAME"
			pFieldEdit.AliasName_2 = "PROC_Name"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 32
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= H Min
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "H_Min"
			pFieldEdit.AliasName_2 = "H_Min"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= H Max
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "H_Max"
			pFieldEdit.AliasName_2 = "H_Max"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= H Units
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "H_Units"
			pFieldEdit.AliasName_2 = "H_Units"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 8
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= Homing Nav
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "GuidanceNav"
			pFieldEdit.AliasName_2 = "GuidanceNav"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 32
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= TypeOfHomingNav
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "TypeOfGuidNav"
			pFieldEdit.AliasName_2 = "TypeOfGuidNav"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= Intersect Nav
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "IntersectNav"
			pFieldEdit.AliasName_2 = "IntersectNav"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			pFieldEdit.Length_2 = 32
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'======================================= TypeOfIntersectNav
			pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
			pFieldEdit.Name_2 = "TypeOfIntersectNav"
			pFieldEdit.AliasName_2 = "TypeOfIntersectNav"
			pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
			pFieldEdit.IsNullable_2 = False
			pFieldsEdit.AddField(pFieldEdit)

			'=======================================
			pFields = pFieldsEdit

			Return featWorkspace.CreateTable(Def_TableName, pFields, Nothing, Nothing, "")
		Catch ex As Exception
			MsgBox(CStr(Err.Number) & " :" & Err.Description, , "createDefsTable")
			Return Nothing
		End Try
	End Function

	Private Function CreateAPP_FIXFeatureClass(ByVal pFeatureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit
		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit
		Dim pGeomDef As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit
		Dim ShapeFieldName As String

		On Error GoTo EH

		ShapeFieldName = "Shape"
		' Add the Fields to the class the OID and Shape are compulsory

		pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields
		'======================================= OID
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "OID"
			.AliasName_2 = "Object ID"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
			.Editable_2 = False
			.IsNullable_2 = False
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= SHAPE
		pGeomDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
		With pGeomDef
			.AvgNumPoints_2 = 1
			.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint
			.GridCount_2 = 1
			.GridSize_2(0) = 1000
			.HasM_2 = True
			.HasZ_2 = True
			.SpatialReference_2 = pSpRefShp
		End With

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = ShapeFieldName
			.AliasName_2 = ShapeFieldName
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
			.Editable_2 = True
			.IsNullable_2 = False
			.GeometryDef_2 = pGeomDef
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= ADHP_ID
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "ADHP_ID"
			.AliasName_2 = "ADHP_ID"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 48
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= NAME
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "NAME"
			.AliasName_2 = "NAME"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 32
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= TYPE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TYPE"
			.AliasName_2 = "TYPE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 5
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= LAT
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "LAT"
			.AliasName_2 = "LAT"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 16
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= LON
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "LON"
			.AliasName_2 = "LON"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 16
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= ELEV_M
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "ELEV_M"
			.AliasName_2 = "ELEV_M"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 5
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= PROC_NAME
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "PROC_NAME"
			.AliasName_2 = "PROC_Name"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 32
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= PROC_Type
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "PROC_Type"
			.AliasName_2 = "PROC_Type"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 16
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= Homing_Nav
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "GuidNav"
			.AliasName_2 = "GuidNav"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 32
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= TypeOfHomingNav
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TypeOfGuidNav"
			.AliasName_2 = "TypeOfGuidNav"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 8
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= Intersect Nav
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "InterNav"
			.AliasName_2 = "InterNav"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = True
			.Length_2 = 32
		End With
		pFieldsEdit.AddField(pFieldEdit)

		'======================================= TypeOfIntersectNav
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "TypeOfInterNav"
			.AliasName_2 = "TypeOfInterNav"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
			.Editable_2 = True
			.IsNullable_2 = False
			.Length_2 = 12
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'======================================= INPUTDATE
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		With pFieldEdit
			.Name_2 = "INPUTDATE"
			.AliasName_2 = "INPUTDATE"
			.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate
			.Editable_2 = True
			.IsNullable_2 = False
		End With
		pFieldsEdit.AddField(pFieldEdit)
		'=======================================

		'Dim pCLSID As IUID
		'    Set pCLSID = New uID
		'    pCLSID.Value = "esriGeoDatabase.Feature"

		pFields = pFieldsEdit
		Return pFeatureWorkspace.CreateFeatureClass(APP_FIX_LayerName, pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, ShapeFieldName, "")
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, , "CreateAPP_FIX FeatureClass")
		Return Nothing
	End Function

	Public Function CreateTrackFeatureClass(ByVal featWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pGeomDef As ESRI.ArcGIS.Geodatabase.IGeometryDef
		Dim pGeomDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit

		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit
		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit

		If featWorkspace Is Nothing Then Return Nothing

		On Error GoTo EH

		'' determine the appropriate geometry type corresponding the the feature type
		'    pCLSID.value = "esriGeoDatabase.Feature"

		' establish a fields collection

		pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields

		''
		'' create the geometry field
		''
		pGeomDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
		pGeomDefEdit = pGeomDef

		'' assign the geometry definiton properties.
		With pGeomDefEdit
			.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline
			.GridCount_2 = 1
			.GridSize_2(0) = 10
			.AvgNumPoints_2 = 2
			.HasM_2 = True
			.HasZ_2 = True
			.SpatialReference_2 = pSpRefShp
		End With

		''
		'' create the SHAPE field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "SHAPE"
		pFieldEdit.AliasName_2 = "SHAPE"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
		pFieldEdit.GeometryDef_2 = pGeomDef
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the object id field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "OBJECTID"
		pFieldEdit.AliasName_2 = "OBJECTID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the ID field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "ID"
		pFieldEdit.AliasName_2 = "ID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the SHAPELength field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "SHAPELength"
		pFieldEdit.AliasName_2 = "SHAPELength"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the NAME field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "NAME"
		pFieldEdit.AliasName_2 = "Name"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the DirNav1 field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "DirNav1"
		pFieldEdit.AliasName_2 = "DirNav1"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the TypeOfDirNav1 field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "TypeOfDirNav1"
		pFieldEdit.AliasName_2 = "TypeOfDirNav1"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the DirNav2 field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "DirNav2"
		pFieldEdit.AliasName_2 = "DirNav2"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = True
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the TypeOfDirNav2 field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "TypeOfDirNav2"
		pFieldEdit.AliasName_2 = "TypeOfDirNav2"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = True
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the DistToCOP field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "DistToCOP"
		pFieldEdit.AliasName_2 = "DistToCOP"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = True
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the WPT1Name field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "WPT1Name"
		pFieldEdit.AliasName_2 = "WPT1Name"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the WPT2Name field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "WPT2Name"
		pFieldEdit.AliasName_2 = "WPT2Name"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the hObs field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "hObs"
		pFieldEdit.AliasName_2 = "hObs"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the hNav field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "hNav"
		pFieldEdit.AliasName_2 = "hNav"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the FL_Min field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "FL_Min"
		pFieldEdit.AliasName_2 = "FL_Min"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the hFL_Max field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "hFL_Max"
		pFieldEdit.AliasName_2 = "hFL_Max"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the FL_Max field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "FL_Max"
		pFieldEdit.AliasName_2 = "FL_Max"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the Dir field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "Dir"
		pFieldEdit.AliasName_2 = "Dir"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the TwoWay field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "TwoWay"
		pFieldEdit.AliasName_2 = "IsTwoWay"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)


		' establish the class extension

		pFields = pFieldsEdit
		Return featWorkspace.CreateFeatureClass("Track", pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, "SHAPE", "")
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, , "createTrackFeatureClass")
		Return Nothing
	End Function

	Public Function CreatePrimaryAreaFeatureClass(ByVal featWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pGeomDef As ESRI.ArcGIS.Geodatabase.IGeometryDef
		Dim pGeomDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit

		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit

		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit


		If featWorkspace Is Nothing Then Return Nothing

		On Error GoTo EH


		'   determine the appropriate geometry type corresponding the the feature type
		'   establish a fields collection

		''
		'' create the geometry field
		''
		pGeomDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
		pGeomDefEdit = pGeomDef

		'' assign the geometry definiton properties.
		With pGeomDefEdit
			.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
			.GridCount_2 = 1
			.GridSize_2(0) = 10
			.AvgNumPoints_2 = 2
			.HasM_2 = False
			.HasZ_2 = False
			.SpatialReference_2 = pSpRefShp
		End With

		pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields

		''
		'' create the SHAPE field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "SHAPE"
		pFieldEdit.AliasName_2 = "SHAPE"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
		pFieldEdit.GeometryDef_2 = pGeomDef
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the object id field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "OBJECTID"
		pFieldEdit.AliasName_2 = "OBJECTID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the ID field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "ID"
		pFieldEdit.AliasName_2 = "ID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the NAME field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "NAME"
		pFieldEdit.AliasName_2 = "Name"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		' establish the class extension

		pFields = pFieldsEdit
		Return featWorkspace.CreateFeatureClass("PrimaryArea", pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, "SHAPE", "")
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, , "createPrimaryAreaFeatureClass")
		Return Nothing
	End Function

	Public Function CreateSecondaryAreaFeatureClass(ByVal featWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace) As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pGeomDef As ESRI.ArcGIS.Geodatabase.IGeometryDef
		Dim pGeomDefEdit As ESRI.ArcGIS.Geodatabase.IGeometryDefEdit
		Dim pFields As ESRI.ArcGIS.Geodatabase.IFields
		Dim pFieldsEdit As ESRI.ArcGIS.Geodatabase.IFieldsEdit
		Dim pFieldEdit As ESRI.ArcGIS.Geodatabase.IFieldEdit

		If featWorkspace Is Nothing Then Return Nothing

		On Error GoTo EH

		'' determine the appropriate geometry type corresponding the the feature type

		' establish a fields collection

		pFieldsEdit = New ESRI.ArcGIS.Geodatabase.Fields

		''
		'' create the geometry field
		''
		pGeomDef = New ESRI.ArcGIS.Geodatabase.GeometryDef
		pGeomDefEdit = pGeomDef

		'' assign the geometry definiton properties.
		With pGeomDefEdit
			.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
			.GridCount_2 = 1
			.GridSize_2(0) = 10
			.AvgNumPoints_2 = 2
			.HasM_2 = False
			.HasZ_2 = False
			.SpatialReference_2 = pSpRefShp
		End With

		''
		'' create the SHAPE field
		''
		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "SHAPE"
		pFieldEdit.AliasName_2 = "SHAPE"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry
		pFieldEdit.GeometryDef_2 = pGeomDef
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the object id field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "OBJECTID"
		pFieldEdit.AliasName_2 = "OBJECTID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the ID field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "ID"
		pFieldEdit.AliasName_2 = "ID"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		''
		'' create the NAME field
		''

		pFieldEdit = New ESRI.ArcGIS.Geodatabase.Field
		pFieldEdit.Name_2 = "NAME"
		pFieldEdit.AliasName_2 = "Name"
		pFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
		pFieldEdit.Length_2 = 32
		pFieldEdit.IsNullable_2 = False
		pFieldsEdit.AddField(pFieldEdit)

		' establish the class extension
		pFields = pFieldsEdit

		Return featWorkspace.CreateFeatureClass("SecondaryArea", pFields, Nothing, Nothing, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, "SHAPE", "")
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, , "createSecondaryAreaFeatureClass")
		Return Nothing
	End Function

	Public Function GenerateFIXName(ByVal I As Integer) As String
		Dim strName As String

		'Do
		If (I < 10) Then
			strName = "WPT0" & CStr(I)
		Else
			strName = "WPT" & CStr(I)
		End If
		I = I + 1
		'Loop While CheckFIXName(pFIXTable, strName)
		I = I - 1
		GenerateFIXName = strName
	End Function

	Public Sub AddShapeFile(ByVal pFeatureWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace, ByVal pFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass)
		Dim I As Integer
		Dim sName As String
		Dim bLayerExist As Boolean
		Dim pFeatureLayer As ESRI.ArcGIS.Carto.IFeatureLayer

		sName = pFeatureClass.AliasName
		bLayerExist = False

		For I = 0 To pMap.LayerCount - 1
			If pMap.Layer(I).Name_2 = sName Then
				bLayerExist = True
				Exit For
			End If
		Next I

		If Not bLayerExist Then
			'Create a new FeatureLayer and assign a shapefile to it
			pFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayer
			pFeatureLayer.FeatureClass = pFeatureClass
			pFeatureLayer.Name_2 = sName
			'Add the FeatureLayer to the focus map
			pMap.AddLayer(pFeatureLayer)
		End If
	End Sub

	Public Function OpenResultWorkspace(ByVal bCreateNew As Boolean) As ESRI.ArcGIS.Geodatabase.IWorkspace
		Dim L As Integer
		Dim pos As Integer

		Dim FileName As String
		Dim Location As String
		Dim FileNameForCreate As String
		Dim pFact As ESRI.ArcGIS.Geodatabase.IWorkspaceFactory
		Dim pFeatWs As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace

		OpenResultWorkspace = Nothing
		FileName = GetMapFileName()

		L = Len(FileName)
		pos = InStrRev(FileName, "\")

		If pos <> 0 Then
			Location = Left(FileName, pos)
			FileName = Right(FileName, L - pos)
		Else
			Location = "\"
		End If

		pos = InStrRev(FileName, ".")
		FileNameForCreate = Left(FileName, pos - 1)
		FileName = FileNameForCreate & ".mdb"

		pFact = New ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactory
		On Error Resume Next
		OpenResultWorkspace = pFact.OpenFromFile(Location & FileName, 0)

		On Error GoTo EH
		Dim pWorkspaceName As ESRI.ArcGIS.Geodatabase.IWorkspaceName
		Dim pFeatureLayer As ESRI.ArcGIS.Carto.IFeatureLayer
		If (OpenResultWorkspace Is Nothing) And bCreateNew Then

			pWorkspaceName = pFact.Create(Location, FileNameForCreate, Nothing, 0)
			pFact = pWorkspaceName.WorkspaceFactory
			OpenResultWorkspace = pFact.OpenFromFile(Location & FileName, 0)
		End If

		Dim pFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pTable As ESRI.ArcGIS.Geodatabase.ITable

		If bCreateNew Then
			pFeatWs = OpenResultWorkspace
			On Error Resume Next

			pFeatureClass = Nothing
			pFeatureClass = pFeatWs.OpenFeatureClass("Track")
			If pFeatureClass Is Nothing Then
				pFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayer
				pFeatureLayer.Name_2 = "Track"
				pFeatureLayer.FeatureClass = CreateTrackFeatureClass(OpenResultWorkspace)
				pMap.AddLayer(pFeatureLayer)
				pMap.MoveLayer(pFeatureLayer, pMap.LayerCount - 1)
			End If

			pFeatureClass = Nothing
			pFeatureClass = pFeatWs.OpenFeatureClass("PrimaryArea")
			If pFeatureClass Is Nothing Then
				pFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayer
				pFeatureLayer.Name_2 = "PrimaryArea"
				pFeatureLayer.FeatureClass = CreatePrimaryAreaFeatureClass(OpenResultWorkspace)
				pMap.AddLayer(pFeatureLayer)
				pMap.MoveLayer(pFeatureLayer, pMap.LayerCount - 1)
			End If

			pFeatureClass = Nothing
			pFeatureClass = pFeatWs.OpenFeatureClass("SecondaryArea")
			If pFeatureClass Is Nothing Then
				pFeatureLayer = New ESRI.ArcGIS.Carto.FeatureLayer
				pFeatureLayer.Name_2 = "SecondaryArea"

				pFeatureLayer.FeatureClass = CreateSecondaryAreaFeatureClass(OpenResultWorkspace)
				pMap.AddLayer(pFeatureLayer)
				pMap.MoveLayer(pFeatureLayer, pMap.LayerCount - 1)
			End If

			'        Set pFeatureClass = Nothing
			'        Set pFeatureClass = pFeatWs.OpenFeatureClass("HoldingArea")
			'        If pFeatureClass Is Nothing Then
			'            Set pFeatureLayer = New FeatureLayer
			'            pFeatureLayer.Name_2 = "HoldingArea"
			'            Set pFeatureLayer.FeatureClass = createHoldingAreaFeatureClass(OpenResultWorkspace)
			'            pMap.AddLayer pFeatureLayer
			'            pMap.MoveLayer pFeatureLayer, pMap.LayerCount - 1
			'        End If

			pTable = Nothing
			pTable = pFeatWs.OpenTable("Definitions")
			If pTable Is Nothing Then CreateDefsTable(OpenResultWorkspace)
			On Error GoTo 0
		End If

		Exit Function
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, MsgBoxStyle.Information, "createAccessWorkspace")
	End Function

	Public Function OpenAPP_FIXWorkspace(ByVal bCreateNew As Boolean, ByVal APP_FIXFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass) As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pFeatWs As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pDataset As ESRI.ArcGIS.Geodatabase.IDataset

		'    Set OpenAPP_FIXWorkspace = Nothing
		'    Set pDataset = pAIRFeatureClass
		'    Set pFeatWs = pDataset.Workspace

		pFeatWs = OpenResultWorkspace(bCreateNew) 'pObjectDir.GetWorkspace

		APP_FIXFeatureClass = Nothing
		On Error Resume Next
		APP_FIXFeatureClass = pFeatWs.OpenFeatureClass(APP_FIX_LayerName)
		On Error GoTo EH
		If (APP_FIXFeatureClass Is Nothing) And bCreateNew Then
			APP_FIXFeatureClass = CreateAPP_FIXFeatureClass(pFeatWs)
		End If
		If Not (APP_FIXFeatureClass Is Nothing) Then AddShapeFile(pFeatWs, APP_FIXFeatureClass)

		OpenAPP_FIXWorkspace = pFeatWs
		Exit Function
EH:
		MsgBox(CStr(Err.Number) & " :" & Err.Description, MsgBoxStyle.Information, "CreateAPP_FIXWorkspace")
	End Function

	Public Function GetDefNames(ByVal HaveFAF As Boolean, ByVal HaveFAP As Boolean, ByVal HaveSDF As Boolean, ByVal HaveTP As Boolean, ByVal HaveEOL As Boolean, ByVal ptFAF_FAP As ESRI.ArcGIS.Geometry.IPoint, ByVal PtSDF As ESRI.ArcGIS.Geometry.IPoint, ByVal IFPnt As ESRI.ArcGIS.Geometry.IPoint, ByVal PtEOL As ESRI.ArcGIS.Geometry.IPoint, ByVal MAPtPrj As ESRI.ArcGIS.Geometry.IPoint, ByVal PtSOC As ESRI.ArcGIS.Geometry.IPoint, ByVal TurnFixPnt As ESRI.ArcGIS.Geometry.IPoint, ByRef FAFName As String, ByRef FAPName As String, ByRef SDFName As String, ByRef IFName As String, ByRef MAPtName As String, ByRef SOCName As String, ByRef TPName As String, ByRef EOLName As String) As Boolean
		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IWorkspace
		Dim pAPP_FIXFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass

		Dim iWPTFieldShape As Integer
		Dim iWPTFieldName As Integer
		Dim iWPTFieldType As Integer

		Dim iFAFNameCnt As Integer
		Dim iFAPNameCnt As Integer
		Dim iSDFNameCnt As Integer
		Dim iIFNameCnt As Integer
		Dim iMAPtNameCnt As Integer
		Dim iSOCNameCnt As Integer
		Dim iTPNameCnt As Integer
		Dim iEOLNameCnt As Integer

		Dim iFAFName As Integer
		Dim iFAPName As Integer
		Dim iSDFName As Integer
		Dim iIFName As Integer
		Dim iMAPtName As Integer
		Dim iSOCName As Integer
		Dim iTPName As Integer
		Dim iEOLName As Integer

		Dim pQueryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
		Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		Dim distEps2 As Double
		Dim pTmpPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pGeom As ESRI.ArcGIS.Geometry.IGeometry

		Dim FixName As String
		Dim FixType As String

		Dim dX As Double
		Dim dY As Double

		On Error GoTo ErrorHandler

		GetDefNames = False

		pWorkspace = OpenAPP_FIXWorkspace(True, pAPP_FIXFeatureClass) 'pDataset.Workspace
		If pWorkspace Is Nothing Then Exit Function

		iFAFNameCnt = 1
		iFAPNameCnt = 1
		iSDFNameCnt = 1
		iIFNameCnt = 1
		iMAPtNameCnt = 1
		iSOCNameCnt = 1
		iTPNameCnt = 1
		iEOLNameCnt = 1

		iFAFName = 0
		iFAPName = 0
		iSDFName = 0
		iIFName = 0
		iMAPtName = 0
		iSOCName = 0
		iTPName = 0
		iEOLName = 0

		If HaveFAP Then
			iFAFName = 1
			iEOLName = 1
		Else
			iFAPName = 1
			If HaveFAF Then
				iEOLName = 1
			Else
				iFAFName = 1
				iIFName = 1
			End If
		End If

		If Not HaveSDF Then iSDFName = 1
		If Not HaveTP Then iTPName = 1

		FAFName = "FAF01"
		FAPName = "FAP01"
		SDFName = "SDF01"
		IFName = "IF001"
		MAPtName = "MAP01"
		SOCName = "SOC01"
		TPName = "TP001"
		EOLName = "EOL01"

		iWPTFieldShape = pAPP_FIXFeatureClass.FindField("Shape")
		iWPTFieldName = pAPP_FIXFeatureClass.FindField("Name")
		iWPTFieldType = pAPP_FIXFeatureClass.FindField("Type")
		'=======================================================

		pQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter
		pQueryFilter.SubFields = "SHAPE,NAME,TYPE"

		pQueryFilter.WhereClause = CStr(pAPP_FIXFeatureClass.OIDFieldName + ">=0")
		pCursor = pAPP_FIXFeatureClass.Search(pQueryFilter, False)
		pRow = pCursor.NextRow

		distEps2 = distEps * distEps
		GetDefNames = True

		Do While Not pRow Is Nothing
			pTmpPt = pRow.Value(iWPTFieldShape)
			pGeom = pTmpPt
			pGeom.SpatialReference_2 = pSpRefShp
			pGeom.Project(pSpRefPrj)

			FixName = pRow.Value(iWPTFieldName)
			FixType = pRow.Value(iWPTFieldType)

			If iFAFName = 0 Then
				dX = ptFAF_FAP.X - pTmpPt.X
				dY = ptFAF_FAP.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "FAF") Then
					FAFName = FixName
					iFAFName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf FAFName = FixName Then
					iFAFNameCnt = iFAFNameCnt + 1
					If iFAFNameCnt < 10 Then
						FAFName = "FAF0" & CStr(iFAFNameCnt)
					Else
						FAFName = "FAF" & CStr(iFAFNameCnt)
					End If
				End If
			End If

			If iFAPName = 0 Then
				dX = ptFAF_FAP.X - pTmpPt.X
				dY = ptFAF_FAP.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "FAP") Then
					FAPName = FixName
					iFAPName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf FAPName = FixName Then
					iFAPNameCnt = iFAPNameCnt + 1
					If iFAPNameCnt < 10 Then
						FAPName = "FAP0" & CStr(iFAPNameCnt)
					Else
						FAPName = "FAP" & CStr(iFAPNameCnt)
					End If
				End If
			End If


			If iIFName = 0 Then
				dX = IFPnt.X - pTmpPt.X
				dY = IFPnt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "IF") Then
					IFName = FixName
					iIFName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf IFName = FixName Then
					iIFNameCnt = iIFNameCnt + 1
					If iIFNameCnt < 10 Then
						IFName = "IF00" & CStr(iIFNameCnt)
					ElseIf iIFNameCnt < 100 Then
						IFName = "IF0" & CStr(iIFNameCnt)
					Else
						IFName = "IF" & CStr(iIFNameCnt)
					End If
				End If
			End If

			If iEOLName = 0 Then 'Ипподром / Разворот на ПК
				dX = PtEOL.X - pTmpPt.X
				dY = PtEOL.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "EOL") Then
					EOLName = FixName
					iEOLName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf EOLName = FixName Then
					iEOLNameCnt = iEOLNameCnt + 1
					If iEOLNameCnt < 10 Then
						EOLName = "EOL0" & CStr(iEOLNameCnt)
					Else
						EOLName = "EOL" & CStr(iEOLNameCnt)
					End If
				End If
			End If

			If iSDFName = 0 Then 'Создать КТ снижения
				dX = PtSDF.X - pTmpPt.X
				dY = PtSDF.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "SDF") Then
					SDFName = FixName
					iSDFName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf SDFName = FixName Then
					iSDFNameCnt = iSDFNameCnt + 1
					If iSDFNameCnt < 10 Then
						SDFName = "SDF0" & CStr(iSDFNameCnt)
					Else
						SDFName = "SDF" & CStr(iSDFNameCnt)
					End If
				End If
			End If

			If iMAPtName = 0 Then
				dX = MAPtPrj.X - pTmpPt.X
				dY = MAPtPrj.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "MAPt") Then
					MAPtName = FixName
					iMAPtName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf MAPtName = FixName Then
					iMAPtNameCnt = iMAPtNameCnt + 1
					If iMAPtNameCnt < 10 Then
						MAPtName = "MAP0" & CStr(iMAPtNameCnt)
					Else
						MAPtName = "MAP" & CStr(iMAPtNameCnt)
					End If
				End If
			End If

			If iSOCName = 0 Then
				dX = PtSOC.X - pTmpPt.X
				dY = PtSOC.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "SOC") Then
					SOCName = FixName
					iSOCName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf SOCName = FixName Then
					iSOCNameCnt = iSOCNameCnt + 1
					If iSOCNameCnt < 10 Then
						SOCName = "SOC0" & CStr(iSOCNameCnt)
					Else
						SOCName = "SOC" & CStr(iSOCNameCnt)
					End If
				End If
			End If

			If iTPName = 0 Then
				dX = TurnFixPnt.X - pTmpPt.X
				dY = TurnFixPnt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "TP") Then
					TPName = FixName
					iTPName = 1
					If iFAFName + iFAPName + iSDFName + iIFName + iMAPtName + iSOCName + iTPName + iEOLName = 8 Then Exit Do
				ElseIf TPName = FixName Then
					iTPNameCnt = iTPNameCnt + 1
					If iIFNameCnt < 10 Then
						IFName = "TP00" & CStr(iIFNameCnt)
					ElseIf iIFNameCnt < 100 Then
						IFName = "TP0" & CStr(iIFNameCnt)
					Else
						IFName = "TP" & CStr(iIFNameCnt)
					End If
				End If
			End If

			pRow = pCursor.NextRow
		Loop

		Exit Function

ErrorHandler:
		GetDefNames = False
	End Function

	Public Function GetFIXName(ByVal pFIXpt As ESRI.ArcGIS.Geometry.IPoint, ByVal FIXRole As Integer, ByVal pAPP_FIXFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass) As String
		Dim iWPTFieldShape As Integer
		Dim iWPTFieldName As Integer
		Dim iWPTFieldType As Integer

		Dim iFAFNameCnt As Integer
		Dim iFAPNameCnt As Integer
		Dim iIFNameCnt As Integer
		Dim iSDFNameCnt As Integer
		Dim iTPNameCnt As Integer

		Dim iFAFName As Integer
		Dim iFAPName As Integer
		Dim iIFName As Integer
		Dim iSDFName As Integer
		Dim iTPName As Integer

		Dim FixName As String
		Dim FixType As String

		Dim pQueryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
		Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		Dim distEps2 As Double
		Dim pTmpPt As ESRI.ArcGIS.Geometry.IPoint
		Dim pGeom As ESRI.ArcGIS.Geometry.IGeometry

		Dim dX As Double
		Dim dY As Double

		Dim NameArray() As String
		Dim TypeNameArray() As String

		On Error GoTo ErrorHandler

		'    Set pWorkspace = OpenAPP_FIXWorkspace(True, pAPP_FIXFeatureClass) 'pDataset.Workspace
		'    If pWorkspace Is Nothing Then Exit Function

		iFAFNameCnt = 1
		iFAPNameCnt = 1
		iIFNameCnt = 1
		iSDFNameCnt = 1
		iTPNameCnt = 1

		iFAFName = 1
		iFAPName = 1
		iIFName = 1
		iSDFName = 1
		iTPName = 1

		NameArray = New String() {"FAF01", "FAF01", "IF001", "SDF01", "TP001"}
		TypeNameArray = New String() {"FAF", "FAP", "IF", "SDF", "TP"}

		GetFIXName = NameArray(FIXRole)

		If FIXRole = FIXRoleFAF Then
			iFAFName = 0
			GetFIXName = "FAF01"
		ElseIf FIXRole = FIXRoleFAP Then
			iFAPName = 0
			GetFIXName = "FAP01"
		ElseIf FIXRole = FIXRoleIF Then
			iIFName = 0
			GetFIXName = "IF001"
		ElseIf FIXRole = FIXRoleSDF Then
			iSDFName = 0
			GetFIXName = "SDF01"
		ElseIf FIXRole = FIXRoleTP Then
			iTPName = 0
			GetFIXName = "TP001"
		End If

		iWPTFieldShape = pAPP_FIXFeatureClass.FindField("Shape")
		iWPTFieldName = pAPP_FIXFeatureClass.FindField("Name")
		iWPTFieldType = pAPP_FIXFeatureClass.FindField("Type")
		'=======================================================

		pQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter
		pQueryFilter.SubFields = "SHAPE,NAME,TYPE"
		pQueryFilter.WhereClause = CStr(pAPP_FIXFeatureClass.OIDFieldName + ">=0")
		pCursor = pAPP_FIXFeatureClass.Search(pQueryFilter, False)
		pRow = pCursor.NextRow

		distEps2 = distEps * distEps

		Do While Not pRow Is Nothing
			pTmpPt = pRow.Value(iWPTFieldShape)
			pGeom = pTmpPt
			pGeom.SpatialReference_2 = pSpRefShp
			pGeom.Project(pSpRefPrj)

			FixName = pRow.Value(iWPTFieldName)
			FixType = pRow.Value(iWPTFieldType)

			If iFAFName = 0 Then
				dX = pFIXpt.X - pTmpPt.X
				dY = pFIXpt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "FAF") Then
					GetFIXName = FixName
					Exit Do
				ElseIf GetFIXName = FixName Then
					iFAFNameCnt = iFAFNameCnt + 1
					If iFAFNameCnt < 10 Then
						GetFIXName = "FAF0" & CStr(iFAFNameCnt)
					Else
						GetFIXName = "FAF" & CStr(iFAFNameCnt)
					End If
				End If
			End If

			If iFAPName = 0 Then
				dX = pFIXpt.X - pTmpPt.X
				dY = pFIXpt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "FAP") Then
					GetFIXName = FixName
					Exit Do
				ElseIf GetFIXName = FixName Then
					iFAPNameCnt = iFAPNameCnt + 1
					If iFAPNameCnt < 10 Then
						GetFIXName = "FAP0" & CStr(iFAPNameCnt)
					Else
						GetFIXName = "FAP" & CStr(iFAPNameCnt)
					End If
				End If
			End If


			If iIFName = 0 Then
				dX = pFIXpt.X - pTmpPt.X
				dY = pFIXpt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "IF") Then
					GetFIXName = FixName
					Exit Do
				ElseIf GetFIXName = FixName Then
					iIFNameCnt = iIFNameCnt + 1
					If iIFNameCnt < 10 Then
						GetFIXName = "IF00" & CStr(iIFNameCnt)
					ElseIf iIFNameCnt < 100 Then
						GetFIXName = "IF0" & CStr(iIFNameCnt)
					Else
						GetFIXName = "IF" & CStr(iIFNameCnt)
					End If
				End If
			End If

			If iSDFName = 0 Then
				dX = pFIXpt.X - pTmpPt.X
				dY = pFIXpt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "SDF") Then
					GetFIXName = FixName
					Exit Do
				ElseIf GetFIXName = FixName Then
					iSDFNameCnt = iSDFNameCnt + 1
					If iSDFNameCnt < 10 Then
						GetFIXName = "SDF0" & CStr(iSDFNameCnt)
					Else
						GetFIXName = "SDF" & CStr(iSDFNameCnt)
					End If
				End If
			End If


			If iTPName = 0 Then
				dX = pFIXpt.X - pTmpPt.X
				dY = pFIXpt.Y - pTmpPt.Y

				If (dX * dX + dY * dY < distEps2) And (FixType = "TP") Then
					GetFIXName = FixName
					Exit Do
				ElseIf GetFIXName = FixName Then
					iTPNameCnt = iTPNameCnt + 1
					If iIFNameCnt < 10 Then
						GetFIXName = "TP00" & CStr(iIFNameCnt)
					ElseIf iIFNameCnt < 100 Then
						GetFIXName = "TP0" & CStr(iIFNameCnt)
					Else
						GetFIXName = "TP" & CStr(iIFNameCnt)
					End If
				End If
			End If

			pRow = pCursor.NextRow
		Loop

		Exit Function

ErrorHandler:

	End Function


	'Public Function LoadDef(ByVal ADHP_ID As String, ByRef DefDataArray() As DefData) As Integer
	'	Dim I As Integer
	'	Dim N As Integer

	'	Dim iDefFieldADHP_ID As Integer
	'	Dim iDefFieldWPT_Name As Integer
	'	Dim iDefFieldProc_Type As Integer
	'	Dim iDefFieldProc_Nam As Integer
	'	Dim iDefFieldFL_Min As Integer
	'	Dim iDefFieldFL_Max As Integer
	'	Dim iDefFieldH_Units As Integer
	'	Dim iDefFieldHoming_Nav As Integer
	'	Dim iDefFieldTypeOfHomingNav As Integer
	'	Dim iDefFieldIntersectNav As Integer
	'	Dim iDefFieldTypeOfIntersectNav As Integer

	'	Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
	'	Dim pDefinitionsTable As ESRI.ArcGIS.Geodatabase.ITable
	'	Dim pQueryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
	'	Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor
	'	Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

	'	LoadDef = -1
	'	ReDim DefDataArray(-1)

	'	pWorkspace = OpenResultWorkspace(False)
	'	pDefinitionsTable = pWorkspace.OpenTable(Def_TableName)
	'	If pDefinitionsTable Is Nothing Then Exit Function

	'	pQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter

	'	If ADHP_ID = "*" Then
	'		pQueryFilter.WhereClause = CStr(pDefinitionsTable.OIDFieldName + ">=0")
	'	Else
	'		pQueryFilter.WhereClause = "ADHP_ID='" & ADHP_ID & "'"
	'	End If

	'	N = pDefinitionsTable.RowCount(pQueryFilter) - 1
	'	If N < 0 Then Exit Function

	'	ReDim DefDataArray(N)

	'	'    pQueryFilter.WhereClause = pDefinitionsTable.OIDFieldName + ">=0"
	'	pCursor = pDefinitionsTable.Search(pQueryFilter, False)

	'	iDefFieldADHP_ID = pCursor.FindField("ADHP_ID")
	'	iDefFieldWPT_Name = pCursor.FindField("WPT_Name")
	'	iDefFieldProc_Type = pCursor.FindField("Proc_Type")
	'	iDefFieldProc_Nam = pCursor.FindField("Proc_Name")
	'	iDefFieldFL_Min = pCursor.FindField("H_Min")
	'	iDefFieldFL_Max = pCursor.FindField("H_Max")
	'	iDefFieldH_Units = pCursor.FindField("H_Units")

	'	iDefFieldHoming_Nav = pCursor.FindField("GuidanceNav")
	'	iDefFieldTypeOfHomingNav = pCursor.FindField("TypeOfGuidNav")
	'	iDefFieldIntersectNav = pCursor.FindField("IntersectNav")
	'	iDefFieldTypeOfIntersectNav = pCursor.FindField("TypeOfIntersectNav")

	'	pRow = pCursor.NextRow

	'	I = -1
	'	While Not pRow Is Nothing
	'		I = I + 1

	'		If iDefFieldADHP_ID >= 0 Then
	'			DefDataArray(I).ADHP_ID = pRow.Value(iDefFieldADHP_ID)
	'		End If

	'		DefDataArray(I).WPT_Name = pRow.Value(iDefFieldWPT_Name)
	'		DefDataArray(I).Proc_Name = pRow.Value(iDefFieldProc_Nam)
	'		DefDataArray(I).Proc_Type = pRow.Value(iDefFieldProc_Type)
	'		DefDataArray(I).H_Units = pRow.Value(iDefFieldH_Units)

	'		DefDataArray(I).Homing_Nav = pRow.Value(iDefFieldHoming_Nav)
	'		DefDataArray(I).TypeOfHomingNav = pRow.Value(iDefFieldTypeOfHomingNav)

	'		DefDataArray(I).FL_Min = pRow.Value(iDefFieldFL_Min)
	'		DefDataArray(I).FL_Max = pRow.Value(iDefFieldFL_Max)

	'		DefDataArray(I).IntersectNav = pRow.Value(iDefFieldIntersectNav)
	'		DefDataArray(I).TypeOfIntersectNav = pRow.Value(iDefFieldTypeOfIntersectNav)

	'		pRow = pCursor.NextRow
	'	End While

	'	If I >= 0 Then
	'		ReDim Preserve DefDataArray(I)
	'		LoadDef = I
	'	Else
	'		ReDim DefDataArray(-1)
	'	End If
	'End Function

	Public Sub StoreDef(ByRef DefDataArray() As DefData)
		Dim I As Integer
		Dim N As Integer

		Dim iDefFieldADHP_ID As Integer
		Dim iDefFieldWPT_Name As Integer
		Dim iDefFieldProc_Type As Integer
		Dim iDefFieldProc_Nam As Integer
		Dim iDefFieldFL_Min As Integer
		Dim iDefFieldFL_Max As Integer
		Dim iDefFieldH_Units As Integer
		Dim iDefFieldHoming_Nav As Integer
		Dim iDefFieldTypeOfHomingNav As Integer
		Dim iDefFieldIntersectNav As Integer
		Dim iDefFieldTypeOfIntersectNav As Integer

		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pWorkspaceEdit As ESRI.ArcGIS.Geodatabase.IWorkspaceEdit
		Dim pDefinitionsTable As ESRI.ArcGIS.Geodatabase.ITable
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		On Error GoTo ErrorHandler

		pWorkspace = OpenResultWorkspace(True)
		pDefinitionsTable = pWorkspace.OpenTable(Def_TableName)

		iDefFieldADHP_ID = pDefinitionsTable.FindField("ADHP_ID")
		iDefFieldWPT_Name = pDefinitionsTable.FindField("WPT_Name")
		iDefFieldProc_Type = pDefinitionsTable.FindField("Proc_Type")
		iDefFieldProc_Nam = pDefinitionsTable.FindField("Proc_Name")
		iDefFieldFL_Min = pDefinitionsTable.FindField("H_Min")
		iDefFieldFL_Max = pDefinitionsTable.FindField("H_Max")
		iDefFieldH_Units = pDefinitionsTable.FindField("H_Units")

		iDefFieldHoming_Nav = pDefinitionsTable.FindField("GuidanceNav")
		iDefFieldTypeOfHomingNav = pDefinitionsTable.FindField("TypeOfGuidNav")
		iDefFieldIntersectNav = pDefinitionsTable.FindField("IntersectNav")
		iDefFieldTypeOfIntersectNav = pDefinitionsTable.FindField("TypeOfIntersectNav")
		'=======================================================

		pWorkspaceEdit = pWorkspace
		pWorkspaceEdit.StartEditing(False)
		pWorkspaceEdit.StartEditOperation()

		N = UBound(DefDataArray)

		For I = 0 To N
			pRow = pDefinitionsTable.CreateRow

			If iDefFieldADHP_ID >= 0 Then pRow.Value(iDefFieldADHP_ID) = DefDataArray(I).ADHP_ID

			pRow.Value(iDefFieldWPT_Name) = DefDataArray(I).WPT_Name
			pRow.Value(iDefFieldProc_Nam) = DefDataArray(I).Proc_Name
			pRow.Value(iDefFieldProc_Type) = DefDataArray(I).Proc_Type
			pRow.Value(iDefFieldH_Units) = DefDataArray(I).H_Units

			pRow.Value(iDefFieldHoming_Nav) = DefDataArray(I).Homing_Nav
			pRow.Value(iDefFieldTypeOfHomingNav) = DefDataArray(I).TypeOfHomingNav

			pRow.Value(iDefFieldFL_Min) = DefDataArray(I).FL_Min
			pRow.Value(iDefFieldFL_Max) = DefDataArray(I).FL_Max

			pRow.Value(iDefFieldIntersectNav) = DefDataArray(I).IntersectNav
			pRow.Value(iDefFieldTypeOfIntersectNav) = DefDataArray(I).TypeOfIntersectNav
			pRow.Store()
		Next I

		pWorkspaceEdit.StopEditOperation()
		pWorkspaceEdit.StopEditing(True)

		Return
ErrorHandler:

	End Sub

	Public Function LoadAPP_FIX(ByVal ADHP_ID As String, ByRef WPT_APPArray() As APP_FIXType) As Integer
		Dim I As Integer
		Dim N As Integer

		Dim iWPTFieldShape As Integer
		Dim iWPTFieldADHP_ID As Integer
		Dim iWPTFieldName As Integer
		Dim iWPTFieldType As Integer
		Dim iWPTFieldLat As Integer
		Dim iWPTFieldLon As Integer
		Dim iWPTFieldELEV_M As Integer
		Dim iWPTFieldPROC_Type As Integer
		Dim iWPTFieldPROC_NAME As Integer
		Dim iWPTFieldGuidNav As Integer
		Dim iWPTFieldTypeOfGuidNav As Integer
		Dim iWPTFieldInterNav As Integer
		Dim iWPTFieldTypeOfInterNav As Integer
		Dim iWPTFieldInputDate As Integer

		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pAPP_FIXFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass
		Dim pAPP_FIXTable As ESRI.ArcGIS.Geodatabase.ITable

		Dim pQueryFilter As ESRI.ArcGIS.Geodatabase.IQueryFilter
		Dim pCursor As ESRI.ArcGIS.Geodatabase.ICursor
		Dim pRow As ESRI.ArcGIS.Geodatabase.IRow

		LoadAPP_FIX = -1
		ReDim WPT_APPArray(-1)

		pWorkspace = OpenAPP_FIXWorkspace(False, pAPP_FIXFeatureClass)
		If pAPP_FIXFeatureClass Is Nothing Then Exit Function

		pAPP_FIXTable = pAPP_FIXFeatureClass
		pQueryFilter = New ESRI.ArcGIS.Geodatabase.QueryFilter

		If ADHP_ID = "*" Then
			pQueryFilter.WhereClause = CStr(pAPP_FIXTable.OIDFieldName + ">=0")
		Else
			pQueryFilter.WhereClause = "ADHP_ID='" & ADHP_ID & "'"
		End If

		N = pAPP_FIXTable.RowCount(pQueryFilter) - 1
		If N < 0 Then Exit Function

		ReDim WPT_APPArray(N)

		'    pQueryFilter.WhereClause = pAPP_FIXTable.OIDFieldName + ">=0"
		pCursor = pAPP_FIXTable.Search(pQueryFilter, False)

		iWPTFieldShape = pCursor.FindField("Shape")
		iWPTFieldADHP_ID = pCursor.FindField("ADHP_ID")
		iWPTFieldName = pCursor.FindField("Name")
		iWPTFieldType = pCursor.FindField("Type")
		iWPTFieldLat = pCursor.FindField("Lat")
		iWPTFieldLon = pCursor.FindField("Lon")
		iWPTFieldELEV_M = pCursor.FindField("ELEV_M")
		iWPTFieldPROC_Type = pCursor.FindField("PROC_Type")
		iWPTFieldPROC_NAME = pCursor.FindField("PROC_NAME")
		iWPTFieldGuidNav = pCursor.FindField("GuidNav")
		iWPTFieldTypeOfGuidNav = pCursor.FindField("TypeOfGuidNav")
		If iWPTFieldTypeOfGuidNav < 0 Then iWPTFieldTypeOfGuidNav = pCursor.FindField("TypeOfGuid")

		iWPTFieldInterNav = pCursor.FindField("InterNav")
		iWPTFieldTypeOfInterNav = pCursor.FindField("TypeOfInterNav")
		If iWPTFieldTypeOfInterNav < 0 Then iWPTFieldTypeOfInterNav = pCursor.FindField("TypeOfInte")
		iWPTFieldInputDate = pCursor.FindField("InputDate")


		pRow = pCursor.NextRow
		Dim X As Double
		Dim Y As Double
		Dim pX As Double
		Dim pY As Double

		I = -1
		While Not pRow Is Nothing
			I = I + 1

			WPT_APPArray(I).pPtGeo = pRow.Value(iWPTFieldShape)
			WPT_APPArray(I).pPtPrj = ToPrj(WPT_APPArray(I).pPtGeo)

			X = WPT_APPArray(I).pPtGeo.X
			Y = WPT_APPArray(I).pPtGeo.Y

			pX = WPT_APPArray(I).pPtPrj.X
			pY = WPT_APPArray(I).pPtPrj.Y

			If iWPTFieldADHP_ID >= 0 Then WPT_APPArray(I).ADHP_ID = pRow.Value(iWPTFieldADHP_ID)

			WPT_APPArray(I).WPT_Name = pRow.Value(iWPTFieldName)
			WPT_APPArray(I).WPT_Type = pRow.Value(iWPTFieldType)
			WPT_APPArray(I).Lat = pRow.Value(iWPTFieldLat)
			WPT_APPArray(I).Lon = pRow.Value(iWPTFieldLon)
			WPT_APPArray(I).Proc_Name = pRow.Value(iWPTFieldPROC_NAME)
			WPT_APPArray(I).Proc_Type = pRow.Value(iWPTFieldPROC_Type)
			WPT_APPArray(I).Homing_Nav = pRow.Value(iWPTFieldGuidNav)
			WPT_APPArray(I).TypeOfHomingNav = pRow.Value(iWPTFieldTypeOfGuidNav)
			WPT_APPArray(I).IntersectNav = pRow.Value(iWPTFieldInterNav)
			WPT_APPArray(I).TypeOfIntersectNav = pRow.Value(iWPTFieldTypeOfInterNav)

			pRow = pCursor.NextRow
		End While

		If I >= 0 Then
			ReDim Preserve WPT_APPArray(I)
			LoadAPP_FIX = I
		Else
			ReDim WPT_APPArray(-1)
		End If
	End Function

	Public Sub StoreAPP_FIX(ByRef WPT_APPArray() As APP_FIXType)
		Dim I As Integer
		Dim N As Integer

		Dim iWPTFieldShape As Integer
		Dim iWPTFieldADHP_ID As Integer
		Dim iWPTFieldName As Integer
		Dim iWPTFieldType As Integer
		Dim iWPTFieldLat As Integer
		Dim iWPTFieldLon As Integer
		Dim iWPTFieldELEV_M As Integer
		Dim iWPTFieldPROC_Type As Integer
		Dim iWPTFieldPROC_NAME As Integer
		Dim iWPTFieldGuidNav As Integer
		Dim iWPTFieldTypeOfGuidNav As Integer
		Dim iWPTFieldInterNav As Integer
		Dim iWPTFieldTypeOfInterNav As Integer
		Dim iWPTFieldInputDate As Integer

		Dim pWorkspace As ESRI.ArcGIS.Geodatabase.IFeatureWorkspace
		Dim pAPP_FIXFeatureClass As ESRI.ArcGIS.Geodatabase.IFeatureClass

		Dim pFIXFeat As ESRI.ArcGIS.Geodatabase.IFeature
		Dim pWPT_WorkspaceEdit As ESRI.ArcGIS.Geodatabase.IWorkspaceEdit

		pWorkspace = OpenAPP_FIXWorkspace(True, pAPP_FIXFeatureClass) 'pDataset.Workspace
		If pAPP_FIXFeatureClass Is Nothing Then Return

		iWPTFieldShape = pAPP_FIXFeatureClass.FindField("Shape")
		iWPTFieldADHP_ID = pAPP_FIXFeatureClass.FindField("ADHP_ID")
		iWPTFieldName = pAPP_FIXFeatureClass.FindField("Name")
		iWPTFieldType = pAPP_FIXFeatureClass.FindField("Type")
		iWPTFieldLat = pAPP_FIXFeatureClass.FindField("Lat")
		iWPTFieldLon = pAPP_FIXFeatureClass.FindField("Lon")
		iWPTFieldELEV_M = pAPP_FIXFeatureClass.FindField("ELEV_M")
		iWPTFieldPROC_Type = pAPP_FIXFeatureClass.FindField("PROC_Type")
		iWPTFieldPROC_NAME = pAPP_FIXFeatureClass.FindField("PROC_NAME")
		iWPTFieldGuidNav = pAPP_FIXFeatureClass.FindField("GuidNav")
		iWPTFieldTypeOfGuidNav = pAPP_FIXFeatureClass.FindField("TypeOfGuidNav")
		If iWPTFieldTypeOfGuidNav < 0 Then iWPTFieldTypeOfGuidNav = pAPP_FIXFeatureClass.FindField("TypeOfGuid")

		iWPTFieldInterNav = pAPP_FIXFeatureClass.FindField("InterNav")
		iWPTFieldTypeOfInterNav = pAPP_FIXFeatureClass.FindField("TypeOfInterNav")
		If iWPTFieldTypeOfInterNav < 0 Then iWPTFieldTypeOfInterNav = pAPP_FIXFeatureClass.FindField("TypeOfInte")
		iWPTFieldInputDate = pAPP_FIXFeatureClass.FindField("InputDate")
		'=======================================================

		pWPT_WorkspaceEdit = pWorkspace
		pWPT_WorkspaceEdit.StartEditing(False)
		pWPT_WorkspaceEdit.StartEditOperation()

		N = UBound(WPT_APPArray)

		For I = 0 To N
			If Not CheckName(pAPP_FIXFeatureClass, WPT_APPArray(I).WPT_Name) Then
				pFIXFeat = pAPP_FIXFeatureClass.CreateFeature

				pFIXFeat.Value(iWPTFieldShape) = WPT_APPArray(I).pPtGeo
				pFIXFeat.Value(iWPTFieldADHP_ID) = WPT_APPArray(I).ADHP_ID
				pFIXFeat.Value(iWPTFieldName) = WPT_APPArray(I).WPT_Name
				pFIXFeat.Value(iWPTFieldType) = WPT_APPArray(I).WPT_Type
				pFIXFeat.Value(iWPTFieldLat) = WPT_APPArray(I).Lat
				pFIXFeat.Value(iWPTFieldLon) = WPT_APPArray(I).Lon
				pFIXFeat.Value(iWPTFieldELEV_M) = WPT_APPArray(I).pPtGeo.Z
				pFIXFeat.Value(iWPTFieldPROC_NAME) = WPT_APPArray(I).Proc_Name
				pFIXFeat.Value(iWPTFieldPROC_Type) = WPT_APPArray(I).Proc_Type
				pFIXFeat.Value(iWPTFieldGuidNav) = WPT_APPArray(I).Homing_Nav
				pFIXFeat.Value(iWPTFieldTypeOfGuidNav) = WPT_APPArray(I).TypeOfHomingNav
				pFIXFeat.Value(iWPTFieldInterNav) = WPT_APPArray(I).IntersectNav
				pFIXFeat.Value(iWPTFieldTypeOfInterNav) = WPT_APPArray(I).TypeOfIntersectNav
				pFIXFeat.Value(iWPTFieldInputDate) = Now
				pFIXFeat.Store()
			End If
		Next I

		pWPT_WorkspaceEdit.StopEditOperation()
		pWPT_WorkspaceEdit.StopEditing(True)
	End Sub

End Module