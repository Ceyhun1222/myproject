object AddBranchFrame: TAddBranchFrame
  Left = 0
  Top = 0
  Width = 518
  Height = 334
  TabOrder = 0
  object Label1: TLabel
    Left = 8
    Top = 261
    Width = 45
    Height = 13
    Caption = 'Fly mode:'
  end
  object Label2: TLabel
    Left = -1
    Top = 290
    Width = 114
    Height = 13
    Caption = 'Planned max turn angle:'
  end
  object Label3: TLabel
    Left = 110
    Top = 312
    Width = 4
    Height = 13
    Caption = #176
  end
  object Label4: TLabel
    Left = 171
    Top = 261
    Width = 59
    Height = 13
    Caption = 'WPT Name:'
  end
  object lblPBNTypeIF: TLabel
    Left = 340
    Top = 243
    Width = 52
    Height = 13
    Caption = 'PBN Type:'
  end
  object spbtnInfo: TSpeedButton
    Left = 335
    Top = 174
    Width = 22
    Height = 22
    Glyph.Data = {
      36050000424D3605000000000000360400002800000010000000100000000100
      0800000000000001000000000000000000000001000000010000F0744B00F156
      3100F9847500ED552400AD642D00FBB0A600994A0300F5AC8E00F4604400F193
      6900FB918800A04F0B00A4591B00EA5E2000D65E2700F2A17B00DE561600FDAC
      AB00B26D3B00F1A17900F7796600F1835A00E7A49700FFBBBD00F66F5900F9AD
      9F00E08B7300E2D1BF00B1825000BF997000FFFFFF00FFFFFF00000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      0000000000000000000000000000000000000000000000000000000000000000
      00000000000000000000000000000000000000000000000000001F1F1F1F1F1F
      1F1F1F1F1F1F1F1F1F1F1F1F1F1F1B1C0B04040B1C1B1F1F1F1F1F1F1F1D0C1A
      111111051A0C1D1F1F1F1F1F1D04020A0A1E1E0A0214041D1F1F1F1B0B081814
      021E1E141808010B1B1F1F1C0E010108081E1E08010103101C1F1F0603030101
      081E1E010103030D061F1F0B0D030000001E1E0000000D0D0B1F1F0B0D001515
      151E1E15151500100B1F1F060D090909091E1E090909090D061F1F1C0E131313
      130F131313130F0E1C1F1F1B0C070707071E1E0707070A0C1B1F1F1F1D120519
      191E1E0505111C1D1F1F1F1F1F1D04161717171716121D1F1F1F1F1F1F1F1B1C
      0B04040B1C1B1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F1F}
    OnClick = spbtnInfoClick
  end
  object gBoxDirection: TGroupBox
    Left = 8
    Top = 8
    Width = 252
    Height = 75
    Caption = ' Segment Direction '
    TabOrder = 0
    object labCourseUnit: TLabel
      Left = 183
      Top = 19
      Width = 4
      Height = 13
      Caption = #176
    end
    object labAPtType: TLabel
      Left = 188
      Top = 48
      Width = 47
      Height = 13
      Caption = 'Dsgn Pnt.'
    end
    object rBtnAFromList: TRadioButton
      Tag = 1
      Left = 9
      Top = 46
      Width = 81
      Height = 17
      Caption = 'From WPT'
      TabOrder = 0
      OnClick = rButtonDirectionClick
    end
    object rBtnAzimuth: TRadioButton
      Left = 8
      Top = 17
      Width = 89
      Height = 17
      Caption = 'True course:'
      Checked = True
      TabOrder = 1
      TabStop = True
      OnClick = rButtonDirectionClick
    end
    object cbDirList: TComboBox
      Left = 104
      Top = 44
      Width = 73
      Height = 21
      Style = csDropDownList
      Enabled = False
      ImeName = 'US'
      ItemHeight = 13
      ParentShowHint = False
      ShowHint = True
      TabOrder = 2
      OnChange = cbDirListChange
    end
    object editCourse: TEdit
      Left = 105
      Top = 15
      Width = 57
      Height = 21
      Hint = 'Min: 0 '#176#13#10'Max: 100 '#176
      ImeName = 'US'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 3
      Text = '45'
    end
    object udCourse: TUpDown
      Left = 162
      Top = 13
      Width = 16
      Height = 24
      Hint = 'Min: 0 '#176#13#10'Max: 100 '#176
      ParentShowHint = False
      Position = 45
      ShowHint = True
      TabOrder = 4
    end
  end
  object gBoxPosition: TGroupBox
    Left = 8
    Top = 84
    Width = 253
    Height = 76
    Caption = ' Next WPT Position '
    TabOrder = 1
    object labDistanceUnit: TLabel
      Left = 180
      Top = 22
      Width = 14
      Height = 13
      Caption = 'km'
    end
    object labDPtType: TLabel
      Left = 198
      Top = 50
      Width = 47
      Height = 13
      Caption = 'Dsgn Pnt.'
    end
    object editDistance: TEdit
      Left = 118
      Top = 18
      Width = 57
      Height = 21
      Hint = 'Min: 0 km'#13#10'Max: 100 km'
      ImeName = 'US'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 0
      Text = '0'
    end
    object cbDistList: TComboBox
      Left = 118
      Top = 46
      Width = 71
      Height = 21
      Style = csDropDownList
      Enabled = False
      ImeName = 'US'
      ItemHeight = 13
      ParentShowHint = False
      ShowHint = True
      TabOrder = 1
      OnChange = cbDistListChange
    end
    object rBtnDistance: TRadioButton
      Left = 8
      Top = 12
      Width = 105
      Height = 32
      Caption = 'By distance from current WPT'
      Checked = True
      TabOrder = 2
      TabStop = True
      WordWrap = True
      OnClick = rBtnDistanceClick
    end
    object rBtnDFromList: TRadioButton
      Tag = 1
      Left = 8
      Top = 48
      Width = 81
      Height = 17
      Caption = 'On WPT'
      TabOrder = 3
      OnClick = rBtnDistanceClick
    end
  end
  object gBoxFlight: TGroupBox
    Left = 8
    Top = 167
    Width = 317
    Height = 77
    Caption = ' Flight parameters '
    TabOrder = 2
    object labIAS: TLabel
      Left = 29
      Top = 19
      Width = 20
      Height = 13
      Caption = 'IAS:'
      WordWrap = True
    end
    object labVelocityUnit: TLabel
      Left = 116
      Top = 19
      Width = 25
      Height = 13
      Caption = 'km/h'
    end
    object labAltitude: TLabel
      Left = 11
      Top = 42
      Width = 38
      Height = 26
      Caption = 'WPT Altitude:'
      WordWrap = True
    end
    object labAltitudeUnit: TLabel
      Left = 116
      Top = 49
      Width = 8
      Height = 13
      Caption = 'm'
    end
    object labGradient: TLabel
      Left = 164
      Top = 42
      Width = 43
      Height = 26
      Caption = 'Descent GRD:'
      WordWrap = True
    end
    object labGradientUnit: TLabel
      Left = 274
      Top = 49
      Width = 8
      Height = 13
      Caption = '%'
    end
    object lblTAS: TLabel
      Left = 183
      Top = 19
      Width = 24
      Height = 13
      Caption = 'TAS:'
      WordWrap = True
    end
    object lblTASUnit: TLabel
      Left = 274
      Top = 19
      Width = 25
      Height = 13
      Caption = 'km/h'
    end
    object editVelocity: TEdit
      Left = 54
      Top = 15
      Width = 57
      Height = 21
      Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
      ImeName = 'US'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 0
      Text = '0'
    end
    object editAltitude: TEdit
      Left = 54
      Top = 45
      Width = 57
      Height = 21
      Hint = 'Min: 0 m'#13#10'Max: 100 m'
      ImeName = 'US'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 1
      Text = '0'
    end
    object editGradient: TEdit
      Left = 212
      Top = 45
      Width = 57
      Height = 21
      Hint = 'Min: 0 %'#13#10'Max: 100 %'
      ImeName = 'US'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 2
      Text = '0'
    end
    object edtTAS: TEdit
      Left = 212
      Top = 15
      Width = 57
      Height = 21
      Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
      Color = clBtnFace
      ImeName = 'US'
      ParentShowHint = False
      ReadOnly = True
      ShowHint = False
      TabOrder = 3
      Text = '0'
    end
  end
  object StringGrid1: TStringGrid
    Left = 268
    Top = 15
    Width = 239
    Height = 146
    ColCount = 6
    DefaultColWidth = 44
    DefaultRowHeight = 20
    RowCount = 2
    Options = [goFixedVertLine, goFixedHorzLine, goVertLine, goHorzLine, goRowSelect]
    TabOrder = 3
  end
  object cbFlyMode: TComboBox
    Left = 67
    Top = 257
    Width = 73
    Height = 21
    Style = csDropDownList
    ImeName = 'US'
    ItemHeight = 13
    ItemIndex = 0
    TabOrder = 4
    Text = 'Fly By'
    OnChange = cbFlyModeChange
    Items.Strings = (
      'Fly By'
      'Fly Over')
  end
  object editMaxTurnAngle: TEdit
    Left = 8
    Top = 308
    Width = 97
    Height = 21
    Hint = 'Min: 0 '#176#13#10'Max: 120 '#176
    ImeName = 'US'
    ParentShowHint = False
    ShowHint = True
    TabOrder = 5
    Text = '90'
  end
  object editFIXName: TMaskEdit
    Left = 235
    Top = 257
    Width = 66
    Height = 21
    AutoSize = False
    CharCase = ecUpperCase
    EditMask = '!AAAAA;1; '
    ImeName = 'US'
    MaxLength = 5
    TabOrder = 6
    Text = '    0'
    OnExit = editFIXNameExit
  end
  object spBtnPlus: TBitBtn
    Left = 281
    Top = 302
    Width = 89
    Height = 25
    Caption = 'Add Segment'
    TabOrder = 7
    OnClick = spBtnPlusClick
  end
  object spBtnMinus: TBitBtn
    Left = 374
    Top = 302
    Width = 89
    Height = 25
    Caption = 'Delete Segment'
    Enabled = False
    TabOrder = 8
    OnClick = spBtnMinusClick
  end
  object rgSensorType: TRadioGroup
    Left = 333
    Top = 195
    Width = 175
    Height = 40
    Caption = 'Sensor Type'
    Columns = 2
    ItemIndex = 0
    Items.Strings = (
      'GNSS'
      'DME/DME')
    TabOrder = 9
    OnClick = rgSensorTypeClick
  end
  object cbPBNType: TComboBox
    Left = 432
    Top = 240
    Width = 73
    Height = 19
    Style = csOwnerDrawFixed
    ItemHeight = 13
    TabOrder = 10
    OnChange = cbPBNTypeChange
    OnDrawItem = cbPBNTypeDrawItem
  end
  object cbMoreDME: TCheckBox
    Left = 340
    Top = 265
    Width = 108
    Height = 17
    Caption = 'More than 2 DME'
    TabOrder = 11
    OnClick = cbMoreDMEClick
  end
  object prmCourse: TParameter
    UnitControl = labCourseUnit
    ValueControl = editCourse
    ChangerControl = udCourse
    OnChangeValue = OnCourseChangeValue
    Value = 45.000000000000000000
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDirAngle
    Visible = True
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 208
    Top = 24
  end
  object prmDistance: TParameter
    UnitControl = labDistanceUnit
    ValueControl = editDistance
    OnChangeValue = OnChangeDistance
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDistance
    Visible = False
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 224
    Top = 99
  end
  object prmIAS: TParameter
    DescriptionControl = labIAS
    UnitControl = labVelocityUnit
    ValueControl = editVelocity
    OnChangeValue = prmIASChangeValue
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puHSpeed
    Visible = True
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNone
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 240
    Top = 308
  end
  object prmAltitude: TParameter
    DescriptionControl = labAltitude
    UnitControl = labAltitudeUnit
    ValueControl = editAltitude
    OnChangeValue = prmAltitudeChangeValue
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puAltitude
    Visible = False
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNone
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 272
    Top = 308
  end
  object prmGradient: TParameter
    DescriptionControl = labGradient
    UnitControl = labGradientUnit
    ValueControl = editGradient
    OnChangeValue = prmGradientChangeValue
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puGradient
    Visible = False
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNone
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 304
    Top = 308
  end
  object prmMaxAngle: TParameter
    DescriptionControl = Label2
    UnitControl = Label3
    ValueControl = editMaxTurnAngle
    OnChangeValue = prmMaxAngleChangeValue
    Value = 90.000000000000000000
    MaxValue = 120.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puAngle
    Visible = False
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = True
    Layout = lkVertikalCenterHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNone
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = True
    Left = 144
    Top = 308
  end
  object prmName: TParameter
    DescriptionControl = Label4
    ValueControl = editFIXName
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puNONE
    Visible = False
    ReadOnly = False
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = False
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNone
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 176
    Top = 308
  end
  object prmTAS: TParameter
    DescriptionControl = lblTAS
    UnitControl = lblTASUnit
    ValueControl = edtTAS
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puHSpeed
    Visible = True
    ReadOnly = True
    GenerateEvents = True
    CheckBounds = True
    Enabled = True
    Active = False
    Layout = lkHorisontal
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 208
    Top = 308
  end
end
