library Departure;

uses
  SysUtils,
  Classes,
  MainDForm in '..\..\Libs\Panda\Common\Delphi\MainDForm.pas' {MainForm},
  ReportForm in 'ReportForm.pas' {ReportDForm},
  ParameterContainerFrame in 'ParameterContainerFrame.pas' {ParamContainer: TFrame},
  DepartureService in 'DepartureService.pas',
  UIContract in '..\..\Libs\Panda\UIContract\delphi\UIContract.pas',
  Contract in '..\..\Libs\Panda\Registry\delphi\Contract.pas',
  Geometry in '..\..\Libs\Panda\Geometry\delphi\Geometry.pas',
  ObjectDirectoryContract in '..\..\Libs\Panda\ObjectDirectoryContract\delphi\ObjectDirectoryContract.pas',
  AIXMTypes in '..\..\Libs\Panda\AixmTypes\Delphi\AIXMTypes.pas',
  ConstantsContract in '..\..\Libs\Panda\Constants\Delphi\ConstantsContract.pas',
  Common in '..\..\Libs\Panda\Common\Delphi\Common.pas',
  GeometryOperatorsContract in '..\..\Libs\Panda\GeometryOperatorsContract\Delphi\GeometryOperatorsContract.pas',
  CollectionUnit in '..\..\Libs\Panda\AixmTypes\Delphi\CollectionUnit.pas',
  DepartureClasses in 'DepartureClasses.pas',
  ARANClasses in '..\..\Libs\Panda\Common\Delphi\ARANClasses.pas',
  ARANFunctions in '..\..\Libs\Panda\Common\Delphi\ARANFunctions.pas',
  IntervalUnit in '..\..\Libs\Panda\Common\Delphi\IntervalUnit.pas',
  UnitConverter in '..\..\Libs\Panda\Common\Delphi\UnitConverter.pas',
  Parameter in '..\..\Libs\Components\Parameter.pas',
  ARANGlobals in '..\..\Libs\Panda\Common\Delphi\ARANGlobals.pas',
  DepartureGlobals in 'DepartureGlobals.pas',
  ARANMath in '..\..\Libs\Panda\Common\Delphi\ARANMath.pas',
  SettingsContract in '..\..\Libs\Panda\Settings\delphi\SettingsContract.pas',
  FunctionParams in 'FunctionParams.pas',
  FunctionCallManager in '..\..\Libs\Panda\Common\Delphi\FunctionCallManager.pas',
  ReportUnit_ in '..\..\Libs\Panda\Common\Delphi\ReportUnit_.pas',
  en in '..\..\Libs\Panda\Common\Delphi\en.pas',
  CRC32Unit in '..\..\Libs\Panda\Common\Delphi\CRC32Unit.pas',
  SaveDForm in 'SaveDForm.pas' {SaveForm},
  MUILoader in '..\..\Libs\Panda\Common\Delphi\MUILoader.pas';

{$R *.res}

begin
end.
