object ParamContainer: TParamContainer
  Left = 0
  Top = 0
  Width = 154
  Height = 24
  AutoScroll = False
  TabOrder = 0
  object UnitControl: TLabel
    Left = 137
    Top = 5
    Width = 14
    Height = 13
    Caption = 'km'
  end
  object DescriptionControl: TLabel
    Left = 2
    Top = 5
    Width = 56
    Height = 13
    Caption = 'Description:'
  end
  object ValueControl: TEdit
    Left = 62
    Top = 1
    Width = 53
    Height = 21
    TabOrder = 0
  end
  object ChangerControl: TUpDown
    Left = 117
    Top = 1
    Width = 14
    Height = 21
    TabOrder = 1
    Visible = False
  end
  object P: TParameter
    DescriptionControl = DescriptionControl
    UnitControl = UnitControl
    ValueControl = ValueControl
    ChangerControl = ChangerControl
    MaxValue = 100.000000000000000000
    MinString = 'Min'
    MaxString = 'Max'
    ParameterType = puDistance
    Visible = True
    ReadOnly = False
    Active = False
    Layout = lkNONE
    DesciptionLeft = 50
    Space = 5
    RoundMode = rtNear
    Accuracy = 1.000000000000000000
    AccuracyMode = amDefault
    ShowMinMaxHint = False
    Left = 81
    Top = 65534
  end
end
