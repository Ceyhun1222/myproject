object FIXInfoForm: TFIXInfoForm
  Left = 471
  Top = 346
  BorderStyle = bsNone
  Caption = 'FIXInfoForm'
  ClientHeight = 163
  ClientWidth = 430
  Color = clInfoBk
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnDeactivate = FormDeactivate
  PixelsPerInch = 96
  TextHeight = 13
  object labAltitude: TLabel
    Left = 11
    Top = 16
    Width = 66
    Height = 13
    Caption = 'WPT Altitude:'
  end
  object labAltitudeUnit: TLabel
    Left = 171
    Top = 16
    Width = 8
    Height = 13
    Caption = 'm'
  end
  object lblTAS: TLabel
    Left = 11
    Top = 54
    Width = 24
    Height = 13
    Caption = 'TAS:'
    WordWrap = True
  end
  object lblTASUnit: TLabel
    Left = 171
    Top = 54
    Width = 25
    Height = 13
    Caption = 'km/h'
  end
  object lblWindSpeed: TLabel
    Left = 11
    Top = 92
    Width = 57
    Height = 13
    Caption = 'wind speed:'
  end
  object lblWindSpeedUnit: TLabel
    Left = 171
    Top = 92
    Width = 25
    Height = 13
    Caption = 'km/h'
  end
  object lblRTurn: TLabel
    Left = 242
    Top = 54
    Width = 32
    Height = 13
    Caption = 'R turn:'
  end
  object lblRTurnUnit: TLabel
    Left = 398
    Top = 54
    Width = 14
    Height = 13
    Caption = 'km'
  end
  object lblEWindSpiral: TLabel
    Left = 242
    Top = 92
    Width = 63
    Height = 13
    Caption = 'R turn + E90:'
  end
  object lblEWindSpiralUnit: TLabel
    Left = 398
    Top = 92
    Width = 14
    Height = 13
    Caption = 'km'
  end
  object lblLatestTPp: TLabel
    Left = 242
    Top = 16
    Width = 49
    Height = 13
    Caption = 'Latest TP:'
  end
  object lblLatestTPpUnit: TLabel
    Left = 398
    Top = 16
    Width = 14
    Height = 13
    Caption = 'km'
  end
  object lblTurnAngle: TLabel
    Left = 11
    Top = 129
    Width = 55
    Height = 13
    Caption = 'Turn Angle:'
  end
  object lblTurnAngleUnit: TLabel
    Left = 176
    Top = 129
    Width = 4
    Height = 13
    Caption = #176
  end
  object editAltitude: TEdit
    Left = 104
    Top = 12
    Width = 57
    Height = 21
    Hint = 'Min: 0 m'#13#10'Max: 100 m'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 0
    Text = '0'
  end
  object edtTAS: TEdit
    Left = 104
    Top = 50
    Width = 57
    Height = 21
    Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 1
    Text = '0'
  end
  object edtWindSpeed: TEdit
    Left = 104
    Top = 88
    Width = 57
    Height = 21
    Hint = 'Min: 0 m'#13#10'Max: 100 m'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 2
    Text = '0'
  end
  object edtRTurn: TEdit
    Left = 331
    Top = 50
    Width = 57
    Height = 21
    Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 3
    Text = '0'
  end
  object edtEWindSpiral: TEdit
    Left = 331
    Top = 88
    Width = 57
    Height = 21
    Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 4
    Text = '0'
  end
  object edtLatestTPp: TEdit
    Left = 331
    Top = 12
    Width = 57
    Height = 21
    Hint = 'Min: 0 km/h'#13#10'Max: 100 km/h'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 5
    Text = '0'
  end
  object edtTurnAngle: TEdit
    Left = 105
    Top = 125
    Width = 57
    Height = 21
    Hint = 'Min: 0 m'#13#10'Max: 100 m'
    Color = clInfoBk
    ImeName = 'US'
    ParentShowHint = False
    ReadOnly = True
    ShowHint = False
    TabOrder = 6
    Text = '0'
  end
  object prmAltitude: TParameter
    DescriptionControl = labAltitude
    UnitControl = labAltitudeUnit
    ValueControl = editAltitude
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puAltitude
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = False
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 105
    Top = 8
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
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 105
    Top = 46
  end
  object prmWindSpeed: TParameter
    DescriptionControl = lblWindSpeed
    UnitControl = lblWindSpeedUnit
    ValueControl = edtWindSpeed
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puHSpeed
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 105
    Top = 84
  end
  object prmRTurn: TParameter
    UnitControl = lblRTurnUnit
    ValueControl = edtRTurn
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDistance
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 332
    Top = 46
  end
  object prmEWindSpiral: TParameter
    DescriptionControl = lblEWindSpiral
    UnitControl = lblEWindSpiralUnit
    ValueControl = edtEWindSpiral
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDistance
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 332
    Top = 84
  end
  object prmLatestTPp: TParameter
    UnitControl = lblLatestTPpUnit
    ValueControl = edtLatestTPp
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDistance
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 332
    Top = 8
  end
  object prmTurnAngle: TParameter
    DescriptionControl = lblTurnAngle
    UnitControl = lblTurnAngleUnit
    ValueControl = edtTurnAngle
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puAngle
    Visible = True
    ReadOnly = True
    GenerateEvents = False
    CheckBounds = False
    Enabled = True
    Active = True
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 106
    Top = 121
  end
end
