unit ARANGlobals;

interface

uses
	UIContract,
	GeometryOperatorsContract,
	ObjectDirectoryContract,
	ConstantsContract,
	Geometry,
	MUILoader,
	ARANClasses;

var
	GPrjSR: TSpatialReference;
	GGeoSR:	TSpatialReference;
	GConst:	TConstants;
	GUI: TUIContract;
	GOrgID: String;
	GObjectDirectory: TObjectDirectory;
	GLicenseRect: TPolygon;
	GMuiLoader: TMUILoader;
	GMuiStrings: TMUIStrings;
	GCurrentUserName: String;
const
	GNullDataValue:     Double = -9999.0;
	GNullRadianValue:   Double = 4.0;

implementation
    uses ARANFunctions;

initialization
	GConst := TConstants.Create;
	GLicenseRect := nil;

finalization
	FreeObject (TObject (GConst));
end.
