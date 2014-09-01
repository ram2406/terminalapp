{*******************************************************}
{                                                       }
{           ������ ������ � ����������������            }
{            �� ��������� CCNET                         }
{                                                       }
{           ����� : ������� �.�. ����� 11.12.2010       }
{            �� ��� �������� info@youfreedom.ru         }
{                                                       }
{*******************************************************}
unit UMain;

interface

uses Windows, Classes, Graphics, Forms, Controls, Menus,
  Dialogs, StdCtrls, Buttons, ExtCtrls, ComCtrls, ImgList, StdActns,
  ActnList, ToolWin,UCashCode, Spin;

type
  TFMain = class(TForm)
    Button1: TButton;
    Button2: TButton;
    SpinEdit1: TSpinEdit;
    Button3: TButton;
    Memo1: TMemo;
    Button4: TButton;
    Button5: TButton;
    GroupBox1: TGroupBox;
    CheckBox1: TCheckBox;
    CheckBox2: TCheckBox;
    CheckBox3: TCheckBox;
    CheckBox4: TCheckBox;
    CheckBox5: TCheckBox;
    CheckBox6: TCheckBox;
    Button6: TButton;
    Button7: TButton;
    Button8: TButton;
    Button9: TButton;
    Button10: TButton;
    Button11: TButton;
    Button12: TButton;
    Button13: TButton;
    GroupBox2: TGroupBox;
    Button14: TButton;
    Edit1: TEdit;
    Edit2: TEdit;
    Label1: TLabel;
    Button15: TButton;
    Label2: TLabel;
    Button16: TButton;
    Label3: TLabel;
    Label4: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
    procedure Button5Click(Sender: TObject);
    procedure Button6Click(Sender: TObject);
    procedure Button7Click(Sender: TObject);
    procedure Button8Click(Sender: TObject);
    procedure Button9Click(Sender: TObject);
    procedure Button10Click(Sender: TObject);
    procedure Button11Click(Sender: TObject);
    procedure Button12Click(Sender: TObject);
    procedure Button13Click(Sender: TObject);
    procedure Button14Click(Sender: TObject);
    procedure Button15Click(Sender: TObject);
    procedure Button16Click(Sender: TObject);
  private
    CashCodeBillValidatorCCNET:TCashCodeBillValidatorCCNET;
    procedure MessagessFormCC(CodeMess:integer;mess:string);
    procedure PolingBillCC(Nominal:word;var CanLoop:boolean);
    procedure EnabledControls(val: boolean);
  public
    FSum:Word;
    procedure RefreshSum();
  end;

var
  FMain: TFMain;

implementation

{$R *.dfm}

uses SysUtils;

procedure TFMain.Button10Click(Sender: TObject);
begin
 CashCodeBillValidatorCCNET.Poll;
end;

procedure TFMain.Button11Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.SendASC;
end;

procedure TFMain.Button12Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.SendNSC;
end;

procedure TFMain.Button13Click(Sender: TObject);
var
  Nominal:TNominal;
begin
  CashCodeBillValidatorCCNET.Reset;
  sleep(1000);
  CashCodeBillValidatorCCNET.Poll;
  Nominal.B10   :=   true;
  CashCodeBillValidatorCCNET.EnableBillTypes(Nominal);

  Nominal.B10   :=   false;
  Nominal.B50   :=   false;
  Nominal.B100  :=   false;
  Nominal.B500  :=   false;
  Nominal.B1000 :=   false;
  Nominal.B5000 :=   false;
  CashCodeBillValidatorCCNET.EnableBillTypes(Nominal);
end;

procedure TFMain.Button14Click(Sender: TObject);
var
  Sum:Word;
  Nominal:TNominal;
begin
  FSum:=0;
  RefreshSum();
  CashCodeBillValidatorCCNET.Reset;

  // ��������� ����������� ������
  Nominal.B10   :=   CheckBox1.Checked;
  Nominal.B50   :=   CheckBox2.Checked;
  Nominal.B100  :=   CheckBox3.Checked;
  Nominal.B500  :=   CheckBox4.Checked;
  Nominal.B1000 :=   CheckBox5.Checked;
  Nominal.B5000 :=   CheckBox6.Checked;

  CashCodeBillValidatorCCNET.EnableBillTypes(Nominal);

  Sum:=CashCodeBillValidatorCCNET.PollingLoop(StrToInt(Edit1.Text),StrToInt(Edit2.Text));
  Memo1.Lines.Add('����� ������� : '+IntToStr(Sum));

  CashCodeBillValidatorCCNET.Reset;
end;

procedure TFMain.Button15Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.CanPollingLoop:=false;
end;

procedure TFMain.Button16Click(Sender: TObject);
begin
  Memo1.Clear;
end;

procedure TFMain.Button1Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.NamberComPort:=SpinEdit1.Value; // ����� �����
  if CashCodeBillValidatorCCNET.OpenComPort then Memo1.Lines.Add('COM ���� ������.');
  EnabledControls(CashCodeBillValidatorCCNET.ComConnected);
end;

procedure TFMain.Button2Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.Reset;
end;

procedure TFMain.Button3Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.CloseComPort;
  EnabledControls(CashCodeBillValidatorCCNET.ComConnected);
end;

procedure TFMain.Button4Click(Sender: TObject);
var
  Namber,Name:string;
begin
  if CashCodeBillValidatorCCNET.Identification(Name,Namber)
  then Memo1.Lines.Add('����� : '+Namber+', �������� : '+Name);
end;

procedure TFMain.Button5Click(Sender: TObject);
var
  Nominal,Security:TNominal;
begin
  if CashCodeBillValidatorCCNET.GetStatus(Nominal,Security) then
  begin
    Memo1.Lines.Add('����������� ������:');
    if Nominal.B10 then Memo1.Lines.Add('10 ������');
    if Nominal.B50 then Memo1.Lines.Add('50 ������');
    if Nominal.B100 then Memo1.Lines.Add('100 ������');
    if Nominal.B500 then Memo1.Lines.Add('500 ������');
    if Nominal.B1000 then Memo1.Lines.Add('1000 ������');
    if Nominal.B5000 then Memo1.Lines.Add('5000 ������');
    Memo1.Lines.Add('��������� �������� ��� �����:');
    if Security.B10 then Memo1.Lines.Add('10 ������');
    if Security.B50 then Memo1.Lines.Add('50 ������');
    if Security.B100 then Memo1.Lines.Add('100 ������');
    if Security.B500 then Memo1.Lines.Add('500 ������');
    if Security.B1000 then Memo1.Lines.Add('1000 ������');
    if Security.B5000 then Memo1.Lines.Add('5000 ������');
  end;
end;

procedure TFMain.Button6Click(Sender: TObject);
var
  Nominal:TNominal;
begin
  Nominal.B10   :=   CheckBox1.Checked;
  Nominal.B50   :=   CheckBox2.Checked;
  Nominal.B100  :=   CheckBox3.Checked;
  Nominal.B500  :=   CheckBox4.Checked;
  Nominal.B1000 :=   CheckBox5.Checked;
  Nominal.B5000 :=   CheckBox6.Checked;

  CashCodeBillValidatorCCNET.EnableBillTypes(Nominal);
end;

procedure TFMain.Button7Click(Sender: TObject);
var
  Nominal:TNominal;
begin
  Nominal.B10   :=   CheckBox1.Checked;
  Nominal.B50   :=   CheckBox2.Checked;
  Nominal.B100  :=   CheckBox3.Checked;
  Nominal.B500  :=   CheckBox4.Checked;
  Nominal.B1000 :=   CheckBox5.Checked;
  Nominal.B5000 :=   CheckBox6.Checked;

  CashCodeBillValidatorCCNET.SetSecurity(Nominal);
end;

procedure TFMain.Button8Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.Stack();
end;

procedure TFMain.Button9Click(Sender: TObject);
begin
  CashCodeBillValidatorCCNET.Return();
end;

procedure TFMain.EnabledControls(val: boolean);
begin
  Button1.Enabled:= not val;
  SpinEdit1.Enabled:= not val;
  Button2.Enabled:= val;
  Button3.Enabled:= val;
  Button4.Enabled:= val;
  Button5.Enabled:= val;
  Button6.Enabled:= val;
  Button7.Enabled:= val;
  Button8.Enabled:= val;
  Button9.Enabled:= val;
  Button10.Enabled:= val;
  Button11.Enabled:= val;
  Button12.Enabled:= val;
  Button13.Enabled:= val;
  Button14.Enabled:= val;
  Button15.Enabled:= val;
  Edit1.Enabled:= val;
  Edit2.Enabled:= val;
  CheckBox1.Enabled:=val;
  CheckBox2.Enabled:=val;
  CheckBox3.Enabled:=val;
  CheckBox4.Enabled:=val;
  CheckBox5.Enabled:=val;
  CheckBox6.Enabled:=val;
end;

procedure TFMain.FormCreate(Sender: TObject);
begin
  CashCodeBillValidatorCCNET:=TCashCodeBillValidatorCCNET.Create;

  //  ��������� �������
  CashCodeBillValidatorCCNET.OnProcessMessage:=MessagessFormCC;
  CashCodeBillValidatorCCNET.OnPolingBill:=PolingBillCC;

  EnabledControls(false);
end;

procedure TFMain.MessagessFormCC(CodeMess: integer; mess: string);
begin
  if (CodeMess>100) and (CodeMess<=199)
  then Memo1.Lines.Add('������ : '+inttostr(CodeMess)+' : '+mess)
  else Memo1.Lines.Add(inttostr(CodeMess)+' : '+mess);
  Application.ProcessMessages; // ���� �� �������� �����
end;

procedure TFMain.PolingBillCC(Nominal: word; var CanLoop: boolean);
begin
  FSum:=FSum+Nominal;
  Memo1.Lines.Add('������� '+intToStr(Nominal)+' ������');
  RefreshSum();
  Application.ProcessMessages; // ���� �� �������� �����
end;

procedure TFMain.RefreshSum;
begin
  Label4.Caption:=IntToStr(FSum);
end;

end.
