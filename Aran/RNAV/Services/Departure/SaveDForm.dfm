object SaveForm: TSaveForm
  Left = 408
  Top = 197
  BorderIcons = [biSystemMenu]
  BorderStyle = bsDialog
  Caption = 'Save Procedure'
  ClientHeight = 455
  ClientWidth = 426
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnClose = FormClose
  OnCreate = FormCreate
  DesignSize = (
    426
    455)
  PixelsPerInch = 96
  TextHeight = 13
  object bCancel: TButton
    Left = 342
    Top = 418
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = '&Cancel'
    TabOrder = 0
    OnClick = bCancelClick
  end
  object bOK: TButton
    Left = 262
    Top = 418
    Width = 75
    Height = 25
    Anchors = [akRight, akBottom]
    Caption = '&OK'
    TabOrder = 1
    OnClick = bOKClick
  end
  object grbProc: TGroupBox
    Left = 5
    Top = 4
    Width = 231
    Height = 101
    Caption = 'Procedure'
    TabOrder = 2
    object labProcName: TLabel
      Left = 7
      Top = 28
      Width = 31
      Height = 13
      Caption = 'Name:'
    end
    object labProcNote: TLabel
      Left = 9
      Top = 48
      Width = 26
      Height = 13
      Caption = 'Note:'
    end
    object editProcReadOnlyName: TEdit
      Left = 40
      Top = 24
      Width = 139
      Height = 21
      Color = clBtnFace
      ReadOnly = True
      TabOrder = 0
    end
    object editProcedureName: TEdit
      Left = 180
      Top = 24
      Width = 39
      Height = 21
      CharCase = ecUpperCase
      MaxLength = 2
      TabOrder = 1
    end
    object editProcNote: TMemo
      Left = 40
      Top = 50
      Width = 180
      Height = 44
      MaxLength = 250
      TabOrder = 2
    end
  end
  object grbTransition: TGroupBox
    Left = 242
    Top = 5
    Width = 173
    Height = 100
    Caption = 'Runway Transition'
    TabOrder = 3
    object labTransitionName: TLabel
      Left = 8
      Top = 25
      Width = 56
      Height = 13
      Caption = 'Description:'
    end
    object editTransitionName: TEdit
      Left = 8
      Top = 45
      Width = 157
      Height = 21
      MaxLength = 250
      TabOrder = 0
    end
  end
  object grbSegmentLeg: TGroupBox
    Left = 5
    Top = 111
    Width = 410
    Height = 290
    Caption = 'Segment Leg'
    TabOrder = 4
    object labSegmentLegList: TLabel
      Left = 9
      Top = 20
      Width = 40
      Height = 13
      Caption = 'Leg List:'
    end
    object labSegmentLegNote: TLabel
      Left = 8
      Top = 217
      Width = 26
      Height = 13
      Caption = 'Note:'
    end
    object lbSegmentLeg: TListBox
      Left = 8
      Top = 36
      Width = 153
      Height = 93
      ItemHeight = 13
      TabOrder = 0
      OnClick = lbSegmentLegClick
    end
    inline pcUpperAlt: TParamContainer
      Left = 6
      Top = 162
      Width = 159
      Height = 27
      AutoScroll = False
      TabOrder = 1
      inherited UnitControl: TLabel
        Left = 138
        Top = 6
        Width = 8
        Caption = 'm'
      end
      inherited DescriptionControl: TLabel
        Width = 70
        Caption = 'Upper Altitude:'
      end
      inherited ChangerControl: TUpDown [2]
        Left = 93
        Top = 2
        Hint = 'Min: 120 m'#13#10'Max: 10000 m'
        Min = 120
        Max = 10360
        ParentShowHint = False
        Position = 360
        ShowHint = True
        Visible = True
      end
      inherited ValueControl: TEdit [3]
        Left = 84
        Top = 2
        Hint = 'Min: 120 m'#13#10'Max: 10000 m'
        ParentShowHint = False
        ShowHint = True
      end
      inherited P: TParameter
        OnChangeValue = pcUpperAltPChangeValue
        MinValue = 120.000000000000000000
        MaxValue = 10000.000000000000000000
        ParameterType = puAltitude
        Active = True
        ShowMinMaxHint = True
        Left = 90
        Top = 65533
      end
    end
    inline pcLowerAlt: TParamContainer
      Left = 6
      Top = 134
      Width = 159
      Height = 27
      AutoScroll = False
      TabOrder = 2
      inherited UnitControl: TLabel
        Left = 138
        Top = 6
        Width = 8
        Caption = 'm'
      end
      inherited DescriptionControl: TLabel
        Width = 70
        Caption = 'Lower Altitude:'
      end
      inherited ChangerControl: TUpDown [2]
        Left = 93
        Top = 2
        Min = 120
        Max = 10360
        ParentShowHint = False
        Position = 120
        ShowHint = True
      end
      inherited ValueControl: TEdit [3]
        Left = 84
        Top = 2
        Color = clBtnFace
        ParentShowHint = False
        ReadOnly = True
        ShowHint = True
      end
      inherited P: TParameter
        MinValue = 120.000000000000000000
        MaxValue = 10000.000000000000000000
        MinString = ''
        MaxString = ''
        ParameterType = puAltitude
        ReadOnly = True
        ShowMinMaxHint = True
        Left = 91
        Top = 65533
      end
    end
    object grbDesignatedPoint: TGroupBox
      Left = 177
      Top = 15
      Width = 220
      Height = 266
      Caption = 'Designated Point'
      TabOrder = 3
      object labDesignatedPointName: TLabel
        Left = 8
        Top = 73
        Width = 31
        Height = 13
        Caption = 'Name:'
      end
      object labDesPointREPATC: TLabel
        Left = 8
        Top = 102
        Width = 59
        Height = 13
        Caption = 'Report ATC:'
      end
      object labDesPointMagVarDate: TLabel
        Left = 9
        Top = 161
        Width = 48
        Height = 26
        Caption = 'Mag. var. date:'
        WordWrap = True
      end
      object labDesPointNote: TLabel
        Left = 9
        Top = 215
        Width = 26
        Height = 13
        Caption = 'Note:'
      end
      object labDesPointDesignator: TLabel
        Left = 6
        Top = 24
        Width = 54
        Height = 13
        Caption = 'Designator:'
      end
      object labDesPointPosition: TLabel
        Left = 9
        Top = 50
        Width = 70
        Height = 13
        Caption = 'Lat: y   Long: x'
      end
      object editDesignatedPointName: TEdit
        Left = 72
        Top = 73
        Width = 140
        Height = 21
        MaxLength = 250
        TabOrder = 0
        OnChange = editDesignatedPointNameChange
      end
      object cbDesPointREPATC: TComboBox
        Left = 72
        Top = 101
        Width = 97
        Height = 21
        Style = csDropDownList
        ItemHeight = 13
        ItemIndex = 0
        TabOrder = 1
        Text = 'Compulsory'
        OnSelect = cbDesPointREPATCSelect
        Items.Strings = (
          'Compulsory'
          'On request'
          'No report')
      end
      inline pcDesPointMagVar: TParamContainer
        Left = 6
        Top = 127
        Width = 133
        Height = 29
        AutoScroll = False
        TabOrder = 2
        inherited UnitControl: TLabel
          Left = 89
          Visible = False
        end
        inherited DescriptionControl: TLabel
          Left = 3
          Top = 1
          Width = 52
          Height = 26
          AutoSize = False
          Caption = 'Magnetic variation:'
          WordWrap = True
        end
        inherited ChangerControl: TUpDown [2]
          Left = 100
          Top = 5
          Height = 19
          Hint = 'Min: -90 km'#13#10'Max: 90 km'
          Min = -90
          Max = 450
        end
        inherited ValueControl: TEdit [3]
          Left = 66
          Top = 4
          Hint = 'Min: -90 km'#13#10'Max: 90 km'
        end
        inherited P: TParameter
          OnChangeValue = pcDesPointMagVarPChangeValue
          MinValue = -90.000000000000000000
          MaxValue = 90.000000000000000000
          ParameterType = puNONE
          Active = True
          Top = 2
        end
      end
      object dtpDesPointMagVar: TDateTimePicker
        Left = 71
        Top = 164
        Width = 100
        Height = 21
        Date = 39960.816043645840000000
        Time = 39960.816043645840000000
        TabOrder = 3
        OnChange = dtpDesPointMagVarChange
      end
      object chbDesPointOnShore: TCheckBox
        Left = 70
        Top = 192
        Width = 73
        Height = 17
        Caption = 'On Shore'
        TabOrder = 4
        OnClick = chbDesPointOnShoreClick
      end
      object editDesPointDesignator: TEdit
        Left = 72
        Top = 22
        Width = 140
        Height = 21
        MaxLength = 250
        TabOrder = 5
        OnChange = editDesPointDesignatorChange
      end
      object editDesPointNote: TMemo
        Left = 70
        Top = 215
        Width = 140
        Height = 44
        MaxLength = 250
        TabOrder = 6
        OnChange = editDesPointNoteChange
      end
    end
    object editSegmentLegNote: TMemo
      Left = 8
      Top = 235
      Width = 160
      Height = 44
      MaxLength = 250
      TabOrder = 4
      OnChange = editSegmentLegNoteChange
    end
  end
end
