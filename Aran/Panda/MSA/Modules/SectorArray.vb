
Public Class SectorList
	Inherits ArrayList

	'i	-----------				00x00
	'j	===========

	'i	-----------				00x01
	'j	=============

	'i	-----------				00x02
	'j	========


	'i	-----------				01x00		3
	'j	  =========

	'i	-----------				01x01		4
	'j	  ===========

	'i	-----------				01x02		5
	'j	   =====



	'i	  ---------				02x00		6
	'j	===========

	'i	  ---------				02x01		7
	'j	=============

	'i	  ---------				02x02		8
	'j	========


	'i				--------	02x02		9
	'j	========

	'i	---------				02x02		10
	'j				========

	Protected Const InsertionThreshold As Integer = 16

	Public Overrides Sub Sort()
		If Me.Count > 1 Then Me.Sort(0, Me.Count - 1)
	End Sub

	Public Overloads Sub Sort(iLo As Integer, iHi As Integer)
		Dim Lo As Integer
		Dim Hi As Integer
		Dim Lu As Integer
		Dim Ha As Integer
		Dim i As Integer
		Dim j As Integer
		Dim m As Integer
		Dim Mid As Integer

		Dim x As MSASectorType
		Lo = iLo

		Do
			If (iHi - iLo < InsertionThreshold) Then

				For i = iLo + 1 To iHi

					x = CType(Me(i), MSASectorType)

					Lu = iLo
					Ha = i - 1
					While (Lu <= Ha)

						m = (Lu + Ha) >> 1

						If (x.FromAngle >= CType(Me(m), MSASectorType).FromAngle) Then
							Lu = m + 1
						Else
							Ha = m - 1
						End If
					End While

					For j = i - 1 To Lu Step -1
						Me(j + 1) = Me(j)
					Next
					Me(Lu) = x
				Next
				Return
			End If

			Hi = iHi
			Mid = CType(Me((Lo + Hi) >> 1), MSASectorType).FromAngle	'Pivot

			''if (((Edge)base[Lo]).X > Mid && ((Edge)base[Lo]).X < ((Edge)base[Hi]).X)
			''    Mid = ((Edge)base[Lo]).X;
			''else if (((Edge)base[Hi]).X > Mid && ((Edge)base[Lo]).X < ((Edge)base[Lo]).X)
			''    Mid = ((Edge)base[Hi]).X;
			''double Mid = selectPivot(Lo, Hi);

			Do
				While CType(Me(Lo), MSASectorType).FromAngle < Mid
					Lo += 1
				End While

				While CType(Me(Hi), MSASectorType).FromAngle > Mid
					Hi -= 1
				End While

				If Lo <= Hi Then
					Dim T As MSASectorType
					T = CType(Me(Lo), MSASectorType)

					Me(Lo) = Me(Hi)
					Me(Hi) = T
					Lo += 1
					Hi -= 1
				End If

			Loop While (Lo < Hi)

			If Hi > iLo Then Sort(iLo, Hi)
			iLo = Lo
		Loop While (Lo < iHi)
	End Sub

	Public Sub AddOrderedSector(item As MSASectorType)
		Dim i As Integer

		If Me.Count = 0 Then
			Me.Add(item)
			Return
		End If

		If item.FromAngle >= CType(Me(Count - 1), MSASectorType).FromAngle Then
			Me.Add(item)
			Return
		End If

		i = 0
		While (i < Count) And item.FromAngle >= CType(Me(i), MSASectorType).FromAngle
			i += 1
		End While

		Insert(i, item)
	End Sub

	Function SectorsCombination(ByRef SectorI As MSASectorType, ByRef SectorJ As MSASectorType) As Integer
		If SectorI.FromAngle = SectorJ.FromAngle Then
			If SectorI.ToAngle = SectorJ.ToAngle Then Return 0
			If AngleInSectorExc(SectorI.ToAngle, SectorJ.FromAngle, SectorJ.ToAngle) Then Return 1
			Return 2
		End If

		If AngleInSectorExc(SectorJ.FromAngle, SectorI.FromAngle, SectorI.ToAngle) Then
			If SectorI.ToAngle = SectorJ.ToAngle Then Return 3
			If AngleInSectorExc(SectorI.ToAngle, SectorJ.FromAngle, SectorJ.ToAngle) Then Return 4
			Return 5
		End If

		If AngleInSectorExc(SectorI.FromAngle, SectorJ.FromAngle, SectorJ.ToAngle) Then
			If SectorI.ToAngle = SectorJ.ToAngle Then Return 6
			If AngleInSectorExc(SectorI.ToAngle, SectorJ.FromAngle, SectorJ.ToAngle) Then Return 7
			Return 8
		End If

		Return 9
	End Function

	Public Sub AddSector(newItem As MSASectorType, Optional startAt As Integer = 0)
		Dim curri As Integer
		Dim combiCase As Integer
		Dim SectorI As MSASectorType
		Dim tmpSector As MSASectorType

		If Me.Count = 0 Then
			Me.Add(newItem)
			Return
		End If

		curri = startAt	'0

		Do
			SectorI = CType(Me(curri), MSASectorType)
			combiCase = SectorsCombination(SectorI, newItem)
			curri += 1
		Loop While (curri < Count) And combiCase > 8

		If (combiCase > 8) Then
			AddOrderedSector(newItem)
			Return
		End If

		curri -= 1

		Select Case combiCase
			Case 0
				'i	-----------				00x00
				'j	===========
				If newItem.DominicObstacle.Height > SectorI.DominicObstacle.Height Then Me(curri) = newItem
			Case 1
				'i	-----------				00x01
				'j	=============
				If newItem.DominicObstacle.Height < SectorI.DominicObstacle.Height Then
					newItem.FromAngle = SectorI.ToAngle
				Else
					RemoveAt(curri)
					curri -= 1
					If curri < 0 Then curri = 0
				End If
				AddSector(newItem, curri)
			Case 2
				'i	-----------				00x02
				'j	========
				If newItem.DominicObstacle.Height > SectorI.DominicObstacle.Height Then
					Me(curri) = newItem

					SectorI.FromAngle = newItem.ToAngle
					Me.Insert(curri + 1, SectorI) 'AddOrderedSector(SectorI)		'AddSector(SectorI, curri)
				End If
			Case 3
				'i	-----------				01x00		3
				'j	  =========
				If newItem.DominicObstacle.Height > SectorI.DominicObstacle.Height Then
					SectorI.ToAngle = newItem.FromAngle
					Me(curri) = SectorI
					Me.Insert(curri + 1, newItem) 'AddOrderedSector(newItem)		'AddSector(newItem, curri)
				End If
			Case 4
				'i	-----------				01x01		4
				'j	  ===========
				If newItem.DominicObstacle.Height < SectorI.DominicObstacle.Height Then
					newItem.FromAngle = SectorI.ToAngle
				Else
					SectorI.ToAngle = newItem.FromAngle
					Me(curri) = SectorI
					curri += 1
				End If
				AddSector(newItem, curri)
			Case 5
				'i	-----------				01x02		5
				'j	   =====
				If newItem.DominicObstacle.Height > SectorI.DominicObstacle.Height Then
					tmpSector = SectorI
					tmpSector.FromAngle = newItem.ToAngle

					SectorI.ToAngle = newItem.FromAngle
					Me(curri) = SectorI
					Me.Insert(curri + 1, newItem) 'AddSector(newItem, curri)
					Me.Insert(curri + 2, tmpSector)	'AddSector(tmpSector, curri)
				End If

			Case 6
				'i	  ---------				02x00		6
				'j	===========
				If newItem.DominicObstacle.Height > SectorI.DominicObstacle.Height Then
					Me(curri) = newItem	'RemoveAt(i)		'replace with new item
				Else
					newItem.ToAngle = SectorI.FromAngle
					Me.Insert(curri, newItem) 'AddOrderedSector(newItem)
					'AddSector(newItem, curri)
				End If
			Case 7
				'i	  ---------				02x01		7
				'j	=============
				If newItem.DominicObstacle.Height >= SectorI.DominicObstacle.Height Then
					RemoveAt(curri)
					curri -= 1
					If curri < 0 Then curri = 0
				Else
					tmpSector = newItem
					tmpSector.ToAngle = SectorI.FromAngle
					newItem.FromAngle = SectorI.ToAngle
					Me.Insert(curri, tmpSector)	'AddOrderedSector(tmpSector)
					'AddSector(tmpSector, curri)
					'curri += 1
				End If
				AddSector(newItem, curri)
			Case 8
				'i	  ---------				02x02		8
				'j	========
				If newItem.DominicObstacle.Height >= SectorI.DominicObstacle.Height Then
					SectorI.FromAngle = newItem.ToAngle
					Me(curri) = newItem
					Me.Insert(curri + 1, SectorI)	'AddSector(SectorI, curri)
				Else
					newItem.ToAngle = SectorI.FromAngle
					'AddOrderedSector(newItem)
					Me.Insert(curri, newItem)  'AddSector(newItem, curri)
				End If
		End Select
	End Sub

End Class
