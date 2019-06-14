object FIXParameterForm: TFIXParameterForm
  Left = 512
  Top = 162
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsToolWindow
  Caption = 'FIX Location'
  ClientHeight = 357
  ClientWidth = 438
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  OnDeactivate = FormDeactivate
  PixelsPerInch = 96
  TextHeight = 13
  object gBoxDirection: TGroupBox
    Left = 8
    Top = 8
    Width = 417
    Height = 94
    Caption = ' Segment Direction '
    TabOrder = 0
    object labCourseUnit: TLabel
      Left = 163
      Top = 53
      Width = 4
      Height = 13
      Caption = #176
    end
    object labCourse: TLabel
      Left = 16
      Top = 55
      Width = 36
      Height = 13
      Caption = 'Course:'
    end
    object labAPtType: TLabel
      Left = 321
      Top = 56
      Width = 76
      Height = 13
      Caption = 'Significant Type'
    end
    object rBtnAFromList: TRadioButton
      Tag = 1
      Left = 176
      Top = 24
      Width = 81
      Height = 17
      Caption = 'From List'
      TabOrder = 0
      OnClick = rButtonDirectionClick
    end
    object rBtnAzimuth: TRadioButton
      Left = 16
      Top = 24
      Width = 89
      Height = 17
      Caption = 'By azimuth'
      Checked = True
      TabOrder = 1
      TabStop = True
      OnClick = rButtonDirectionClick
    end
    object cbDirList: TComboBox
      Left = 228
      Top = 51
      Width = 85
      Height = 21
      Style = csDropDownList
      ItemHeight = 13
      ParentShowHint = False
      ShowHint = True
      TabOrder = 2
      OnChange = cbDirListChange
    end
    object editCourse: TEdit
      Left = 84
      Top = 51
      Width = 57
      Height = 21
      TabOrder = 3
      Text = '0'
      OnExit = editCourseExit
    end
    object udCourse: TUpDown
      Left = 140
      Top = 49
      Width = 16
      Height = 24
      TabOrder = 4
      OnClick = udCourseClick
    end
  end
  object gBoxPosition: TGroupBox
    Left = 8
    Top = 107
    Width = 417
    Height = 100
    Caption = ' FIX Position '
    TabOrder = 1
    object labDistance: TLabel
      Left = 8
      Top = 57
      Width = 76
      Height = 26
      Caption = 'Distance from end of segment:'
      WordWrap = True
    end
    object labDistanceUnit: TLabel
      Left = 184
      Top = 64
      Width = 14
      Height = 13
      Caption = 'km'
    end
    object labDPtType: TLabel
      Left = 318
      Top = 63
      Width = 76
      Height = 13
      Caption = 'Significant Type'
    end
    object editDistance: TEdit
      Left = 96
      Top = 60
      Width = 81
      Height = 21
      TabOrder = 0
      Text = '0'
      OnExit = editDistanceExit
    end
    object cbDistList: TComboBox
      Left = 225
      Top = 60
      Width = 85
      Height = 21
      Style = csDropDownList
      ItemHeight = 13
      ParentShowHint = False
      ShowHint = True
      TabOrder = 1
      OnChange = cbDistListChange
    end
    object rBtnDistance: TRadioButton
      Left = 16
      Top = 24
      Width = 89
      Height = 17
      Caption = 'By distance'
      Checked = True
      TabOrder = 2
      TabStop = True
      OnClick = rBtnDistanceClick
    end
    object rBtnDFromList: TRadioButton
      Tag = 1
      Left = 176
      Top = 24
      Width = 81
      Height = 17
      Caption = 'From List'
      TabOrder = 3
      OnClick = rBtnDistanceClick
    end
  end
  object Button1: TButton
    Left = 232
    Top = 320
    Width = 75
    Height = 25
    Caption = '&OK'
    Default = True
    ModalResult = 1
    TabOrder = 2
  end
  object Button2: TButton
    Left = 328
    Top = 320
    Width = 75
    Height = 25
    Cancel = True
    Caption = '&Cancel'
    ModalResult = 2
    TabOrder = 3
  end
  object gBoxFlight: TGroupBox
    Left = 8
    Top = 212
    Width = 417
    Height = 93
    Caption = ' Flight parameters '
    TabOrder = 4
    object labIAS: TLabel
      Left = 12
      Top = 27
      Width = 65
      Height = 13
      Caption = 'Segment IAS:'
      WordWrap = True
    end
    object labVelocityUnit: TLabel
      Left = 174
      Top = 27
      Width = 25
      Height = 13
      Caption = 'km/h'
    end
    object labAltitude: TLabel
      Left = 15
      Top = 54
      Width = 57
      Height = 13
      Caption = 'FIX Altitude:'
    end
    object labAltitudeUnit: TLabel
      Left = 175
      Top = 53
      Width = 8
      Height = 13
      Caption = 'm'
    end
    object labGradient: TLabel
      Left = 232
      Top = 26
      Width = 43
      Height = 13
      Caption = 'Gradient:'
    end
    object labGradientUnit: TLabel
      Left = 356
      Top = 26
      Width = 8
      Height = 13
      Caption = '%'
    end
    object editVelocity: TEdit
      Left = 94
      Top = 23
      Width = 73
      Height = 21
      TabOrder = 0
      Text = '530'
      OnExit = editVelocityExit
    end
    object editAltitude: TEdit
      Left = 93
      Top = 52
      Width = 73
      Height = 21
      TabOrder = 1
      Text = '0'
      OnExit = editAltitudeExit
    end
    object editGradient: TEdit
      Left = 280
      Top = 22
      Width = 65
      Height = 21
      TabOrder = 2
      Text = '3.3'
      OnExit = editGradientExit
    end
  end
end
