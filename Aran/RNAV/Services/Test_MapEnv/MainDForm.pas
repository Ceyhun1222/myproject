unit MainDForm;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls,
  UIContract,
  Contract,
  Geometry,
  GeometryOperatorsContract,
  ObjectDirectoryContract,
  Common,
  SettingsContract;

type
  TForm1 = class(TForm)
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    Button4: TButton;
    ListBox1: TListBox;
    Button5: TButton;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Edit5: TEdit;
    Edit6: TEdit;
    procedure Button1Click(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);
    procedure Button5Click(Sender: TObject);
  private
    FUI: TUIContract;
    FObjDir: TObjectDirectory;
    FSettings: TARANSettings;
  public
  		destructor Destroy; override;
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

function GetGeoSpatialReference (): TSpatialReference;
begin
  result := TSpatialReference.Create;
	result.name := 'WGS1984';
	result.spatialReferenceType := srtGeographic;
	result.spatialReferenceUnit := sruMeter;
	result.ellipsoid.srGeoType := srgtWGS1984;
	result.ellipsoid.semiMajorAxis := 6378137.0;
	result.ellipsoid.flattening := 1 / 298.25722356300003;
end;

destructor TForm1.Destroy;
begin
  FUI.Free;
  FObjDir.Free;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  Close;
end;

procedure TForm1.FormShow(Sender: TObject);
begin
  FUI := TUIContract.Create ();
  FSettings:=TARANSettings.Create();
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  point: TPoint;
  prjPoint: TPoint;
  mapSR: TSpatialReference;
  geoSR: TSpatialReference;
  geoOper: TGeometryOperators;
begin
  point := TPoint.Create (54.1370916, 24.7283084);

  mapSR := FUI.ViewProjection;

  if mapSR = nil then
  begin
    self.Caption := 'Map Projection is null';
    exit;
  end;

  geoSR := GetGeoSpatialReference ();

  geoOper := TGeometryOperators.Create;
  prjPoint := TPoint (geoOper.GeoTransformations(point, geoSR, mapSR));

  FUI.DrawPoint (prjPoint, 255);
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  sa: TStringArray;
  pandaList: TPandaList;
  i: integer;
  unical:TUnicalName;
begin
  SetLength (sa, 0);

  pandaList := FObjDir.GetAerodromeList (sa);
  unical :=TUnicalName(pandaList.Item[0]);
  ListBox1.Items.Add(unical.Name);
end;

procedure TForm1.Button4Click(Sender: TObject);
//var
  //connInfo: TConnectionInfo;
begin
  FObjDir := TObjectDirectory.Create ();

  {
  connInfo := TConnectionInfo.Create;

  FObjDir.Connect(connInfo);

  connInfo.Free;
  }
end;

procedure TForm1.Button5Click(Sender: TObject);
var
  languageCode :TLanguageCode;
  distanceUnit :THorisontalDistanceUnit;
  elevationUnit :TVerticalDistanceUnit;
  speedUnit :THorisontalSpeedUnit;
  distanceName,elevationName,speedName:String;
  distanceAccuracy,heightAccuracy,speedAccuracy:Double;
begin
  languageCode:= FSettings.GetLanguageCode();
  distanceUnit:=FSettings.GetDistanceUnit();
  distanceName:= GHorisontalDistanceUnitName[distanceUnit];
  elevationUnit:= FSettings.GetElevationUnit();
  elevationName:=GVerticalDistanceUnitName[elevationUnit];
  speedUnit:= FSettings.GetSpeedUnit();
  speedName:= GHorisontalSpeedUnitName[speedUnit];
  distanceAccuracy := FSettings.GetDistanceAccuracy;
  heightAccuracy := FSettings.GetElevationAccuracy;
  speedAccuracy := FSettings.GetSpeedAccuracy;
  edit1.Text:=distanceName;
  Edit2.Text := elevationName;
  Edit3.Text := speedName;
  Edit4.Text := FloatToStr(distanceAccuracy);
  Edit5.Text := FloatToStr(heightAccuracy);
  Edit6.Text := FloatToStr(speedAccuracy);
end;

end.
