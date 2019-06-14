unit ParameterContainerFrame;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms, 
  Dialogs, Parameter, ComCtrls, StdCtrls;
     
type
  TParamContainer = class(TFrame)
    ValueControl: TEdit;
    UnitControl: TLabel;
    ChangerControl: TUpDown;
    P: TParameter;
	DescriptionControl: TLabel;
  public
    destructor Destroy (); override;
  end;

implementation

{$R *.dfm}

{ TParamContainer }

destructor TParamContainer.Destroy;
begin
    inherited;
end;

end.
