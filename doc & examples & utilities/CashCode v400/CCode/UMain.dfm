object FMain: TFMain
  Left = 197
  Top = 111
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = 'CCDemo'
  ClientHeight = 443
  ClientWidth = 583
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label3: TLabel
    Left = 16
    Top = 232
    Width = 35
    Height = 13
    Caption = #1057#1091#1084#1084#1072':'
  end
  object Label4: TLabel
    Left = 56
    Top = 224
    Width = 13
    Height = 25
    Caption = '0'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Button1: TButton
    Left = 8
    Top = 8
    Width = 129
    Height = 25
    Caption = #1054#1090#1082#1088#1099#1090#1100' COM'
    TabOrder = 0
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 8
    Top = 71
    Width = 185
    Height = 25
    Caption = #1057#1073#1088#1086#1089
    TabOrder = 1
    OnClick = Button2Click
  end
  object SpinEdit1: TSpinEdit
    Left = 144
    Top = 8
    Width = 49
    Height = 22
    MaxValue = 0
    MinValue = 0
    TabOrder = 2
    Value = 0
  end
  object Button3: TButton
    Left = 8
    Top = 40
    Width = 185
    Height = 25
    Caption = #1047#1072#1082#1088#1099#1090#1100' COM'
    TabOrder = 3
    OnClick = Button3Click
  end
  object Memo1: TMemo
    Left = 0
    Top = 256
    Width = 583
    Height = 187
    Align = alBottom
    ScrollBars = ssVertical
    TabOrder = 4
  end
  object Button4: TButton
    Left = 8
    Top = 102
    Width = 185
    Height = 25
    Caption = #1048#1085#1092#1086#1088#1084#1072#1094#1080#1103' '#1086#1073' '#1091#1089#1090#1088#1086#1081#1089#1090#1074#1077
    TabOrder = 5
    OnClick = Button4Click
  end
  object Button5: TButton
    Left = 8
    Top = 133
    Width = 185
    Height = 25
    Caption = #1057#1090#1072#1090#1091#1089' '#1091#1089#1090#1088#1086#1081#1089#1090#1074#1072
    TabOrder = 6
    OnClick = Button5Click
  end
  object GroupBox1: TGroupBox
    Left = 208
    Top = 8
    Width = 193
    Height = 150
    Caption = #1053#1086#1084#1080#1085#1072#1083
    TabOrder = 7
    object CheckBox1: TCheckBox
      Left = 8
      Top = 15
      Width = 97
      Height = 17
      Caption = '10 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 0
    end
    object CheckBox2: TCheckBox
      Left = 8
      Top = 35
      Width = 97
      Height = 17
      Caption = '50 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 1
    end
    object CheckBox3: TCheckBox
      Left = 8
      Top = 55
      Width = 97
      Height = 17
      Caption = '100 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 2
    end
    object CheckBox4: TCheckBox
      Left = 93
      Top = 15
      Width = 97
      Height = 17
      Caption = '500 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 3
    end
    object CheckBox5: TCheckBox
      Left = 93
      Top = 35
      Width = 97
      Height = 17
      Caption = '1000 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 4
    end
    object CheckBox6: TCheckBox
      Left = 93
      Top = 55
      Width = 97
      Height = 17
      Caption = '5000 '#1088#1091#1073#1083#1077#1081
      Checked = True
      State = cbChecked
      TabOrder = 5
    end
    object Button6: TButton
      Left = 8
      Top = 78
      Width = 173
      Height = 25
      Caption = #1056#1072#1079#1088#1077#1096#1080#1090#1100' '#1087#1088#1080#1085#1080#1084#1072#1090#1100' '#1082#1091#1087#1102#1088#1099
      TabOrder = 6
      OnClick = Button6Click
    end
    object Button7: TButton
      Left = 8
      Top = 112
      Width = 173
      Height = 25
      Caption = #1055#1086#1074#1099#1096#1077#1085#1085#1099#1081' '#1082#1086#1085#1090#1088#1086#1083#1100' '#1076#1083#1103
      TabOrder = 7
      OnClick = Button7Click
    end
  end
  object Button8: TButton
    Left = 416
    Top = 40
    Width = 161
    Height = 25
    Caption = #1055#1088#1080#1085#1103#1090#1100' (Stack)'
    TabOrder = 8
    OnClick = Button8Click
  end
  object Button9: TButton
    Left = 416
    Top = 71
    Width = 161
    Height = 25
    Caption = #1042#1077#1088#1085#1091#1090#1100' (Return)'
    TabOrder = 9
    OnClick = Button9Click
  end
  object Button10: TButton
    Left = 416
    Top = 9
    Width = 161
    Height = 25
    Caption = #1054#1087#1088#1086#1089#1080#1090#1100' (POLL)'
    TabOrder = 10
    OnClick = Button10Click
  end
  object Button11: TButton
    Left = 416
    Top = 102
    Width = 161
    Height = 25
    Caption = 'ASC'
    TabOrder = 11
    OnClick = Button11Click
  end
  object Button12: TButton
    Left = 416
    Top = 133
    Width = 161
    Height = 25
    Caption = 'NSC'
    TabOrder = 12
    OnClick = Button12Click
  end
  object Button13: TButton
    Left = 8
    Top = 164
    Width = 185
    Height = 25
    Caption = #1042#1099#1082#1083#1102#1095#1080#1090#1100
    TabOrder = 13
    OnClick = Button13Click
  end
  object GroupBox2: TGroupBox
    Left = 208
    Top = 164
    Width = 369
    Height = 86
    Caption = #1055#1088#1080#1077#1084
    TabOrder = 14
    object Label1: TLabel
      Left = 140
      Top = 55
      Width = 160
      Height = 13
      Caption = #1046#1076#1072#1090#1100' '#1082#1091#1087#1102#1088#1091' '#1085#1077'  '#1073#1086#1083#1077#1077' ('#1089#1077#1082'.)'
    end
    object Label2: TLabel
      Left = 164
      Top = 23
      Width = 36
      Height = 13
      Caption = #1056#1091#1073#1083#1077#1081
    end
    object Button14: TButton
      Left = 8
      Top = 18
      Width = 96
      Height = 25
      Caption = #1055#1088#1080#1085#1103#1090#1100
      TabOrder = 0
      OnClick = Button14Click
    end
    object Edit1: TEdit
      Left = 110
      Top = 20
      Width = 49
      Height = 21
      TabOrder = 1
      Text = '100'
    end
    object Edit2: TEdit
      Left = 306
      Top = 52
      Width = 49
      Height = 21
      TabOrder = 2
      Text = '20'
    end
    object Button15: TButton
      Left = 8
      Top = 49
      Width = 96
      Height = 25
      Caption = #1055#1088#1077#1088#1074#1072#1090#1100
      TabOrder = 3
      OnClick = Button15Click
    end
  end
  object Button16: TButton
    Left = 8
    Top = 195
    Width = 185
    Height = 25
    Caption = #1054#1095#1080#1089#1090#1080#1090#1100' '#1083#1086#1075
    TabOrder = 15
    OnClick = Button16Click
  end
end
