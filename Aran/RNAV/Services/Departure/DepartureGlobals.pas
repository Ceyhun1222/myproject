unit DepartureGlobals;

interface

uses
	Controls,
	Geometry,
	UnitConverter,
	UIContract,
	Parameter,
	ARANMath,
	CollectionUnit,
	Classes,
    ARANClasses,
	ParameterContainerFrame,
	AIXM_TLB;

type
	TDepartureFormType = (dftStraight, dftMultiTurn);

var
	GMagneticVariation:	Double;

	GAixmProcedure: IAIXMProcedure = nil;


procedure DeleteGraphic (var handle: Integer);
function DrawGeometry (geom: TGeometry; symbol: TSymbol): Integer; overload;
function DrawGeometry (geom: TGeometry): Integer; overload;
function DrawGeometry (geom: TGeometry; color: Integer): Integer; overload;
function ConvertUnit (val: Double; direction: TConvrtDirection; unitType: TParameterUnit;
            point: TObject = nil): Double;
function ConvertUnitStr (val: Double; direction: TConvrtDirection; unitType: TParameterUnit;
            roundMode: TRoundTo; point: TObject = nil): string;
function FilterSignificantPoints (sigPointList: TSignificantPointCollection;
            vPoint: Geometry.TPoint; leftDir, rightDir: Double; minDistance: Double = -1): TList;
procedure MoveMultiPoint (multiPoint: TMultiPoint; direction, distance: Double);
function LineAlong (basePoint: Geometry.TPoint; Direction, Distance: Double): TPolyline;
function IsSameSide (side1, side2: TSideDirection): Boolean;
function UnionProtectedArea (pointNearRemovedRing: Geometry.TPoint; area1: TProtectedArea; area2: TProtectedArea): TProtectedArea;
procedure CutProtectedArea (const pointNearRemovedRing: Geometry.TPoint; const area: TProtectedArea; cutterLine: TPolyline;
            resultGeomSide: TSideDirection; var areaLeft, areaRight: TProtectedArea);
procedure RemoveNearestRing (point: Geometry.TPoint; polygon: TPolygon);
procedure GetDMEToleranceParams (isDERPoint: Boolean; elevation: Double;
            isDMEtill_1989: Boolean; countDME: Integer;
            var XTT: Double; var ATT: Double; var semiWidth: Double);

function IsGeometryEmpty (geom: TGeometry): Boolean;
function IsPolylineEmpty (geomCol: TPolyline): Boolean;
function IsPolygonEmpty (geomCol: TPolygon): Boolean;
function IsMultiPointEmpty (geomCol: TMultiPoint): Boolean;

procedure DrawProtectedArea_ForDebug (area: TProtectedArea);


function CreateIString (value: string): IString;
function CreateIBoolean (value: Boolean): IBoolean;
function CreateIInt32 (value: Integer): IInt32;
function CreateIDouble (value: Double): IDouble;
function CreateDefaulIDistance (value: Double): IDistance;
function CreateDefaulIDistanceVertical (value: Double): IDistanceVertical;
function PolylineToIAIXMCurve (aranPolyline: TPolyline): IAIXMCurve;


implementation

uses
	DepartureService,
	DepartureClasses,
	Windows,
	SysUtils,
	Graphics,
	ARANFunctions,
	ARANGlobals,
	Dialogs,
	AIXMTypes;

function DrawGeometry (geom: TGeometry; color: Integer): Integer;
var
	symbol:	TSymbol;
begin
	result := -1;

	case geom.GeometryType of
		gtPoint: 	symbol := TPointSymbol.Create;
		gtPolyline:	symbol := TLineSymbol.Create;
		gtPolygon:  symbol := TFillSymbol.Create;
		else
			exit;
	end;

	if color <> -1 then
		symbol.Color := color;

	result := DrawGeometry (geom, symbol);
	symbol.Free;
end;

function DrawGeometry (geom: TGeometry): Integer;
begin
	result := DrawGeometry (geom, -1);
end;

function DrawGeometry (geom: TGeometry; symbol: TSymbol): Integer;
begin
	result := 0;
	case geom.GeometryType of
		gtPoint: 	result := GUi.DrawPoint (geom.AsPoint, TPointSymbol (symbol));
		gtPolyline:	result := GUi.DrawPolyline (geom.AsPolyline, TLineSymbol (symbol));
		gtPolygon:  result := GUi.DrawPolygon (geom.AsPolygon, TFillSymbol (symbol));
	end;
end;

procedure DeleteGraphic (var handle: Integer);
begin
	if handle = -1 then exit;
	GUi.DeleteGraphic (handle);
	handle := -1;
end;

function ConvertUnit (val: Double; direction: TConvrtDirection; unitType: TParameterUnit; point: TObject = nil): Double;
var
	sign:	Integer;
begin
	if direction = cdToInner then
		sign := 1
	else
		sign := -1;

	result := CConverters [unitType].ConvertFunction (val, direction, point);
	if unitType = puAngle then result := Modulus (result - sign * GMagneticVariation, 360);
end;

function ConvertUnitStr (val: Double; direction: TConvrtDirection; unitType: TParameterUnit; roundMode: TRoundTo; point: TObject = nil): string;
var
	newVal:	Double;
begin
	newVal := ConvertUnit (val, direction, unitType, point);
	result := RoundToStr (newVal, CConverters [unitType].Accuracy, roundMode);
end;

function FilterSignificantPoints (
			sigPointList: TSignificantPointCollection;
			vPoint: Geometry.TPoint;
			leftDir,
			rightDir: Double;
			minDistance: Double = -1): TList;
var
	i:			Integer;
	side1,
	side2:		TSideDirection;
	addItem,
	checkSide,
	checkMinDist,
	checkAnd:	Boolean;
begin
	checkAnd := False;

	if (rightDir = GNullDataValue) or (leftDir = GNullDataValue) then
		checkSide := False
	else
	begin
		checkSide := True;
		checkAnd := (Modulus (rightDir - leftDir, 2*Pi) < Pi);
	end;

	checkMinDist := (minDistance > 0);

	result := TList.Create;

	for i:=0 to sigPointList.Count - 1 do
	begin
		if sigPointList.Item [i].AIXMType = atDME then
			continue;

		addItem := False;
		if not checkSide then
			addItem := True
		else
		begin
			side1 := SideDef (vPoint, leftDir, sigPointList.Item [i].Prj);
			if checkAnd then
			begin
				if side1 = sideLeft then
				begin
					side2 := SideDef (vPoint, rightDir, sigPointList.Item [i].Prj);
					if side2 = sideRight then
						addItem := True;
				end;
			end
			else
			begin
				side2 := SideDef (vPoint, rightDir, sigPointList.Item [i].Prj);
				addItem := ((side1 = sideLeft) or (side2 = sideRight));
			end;
		end;

		if addItem and checkMinDist then
		begin
			addItem := (ReturnDistanceInMeters (vPoint, sigPointList.Item [i].Prj) >= minDistance);
		end;

		if addItem then
			result.Add (Pointer (i));
	end;
end;

procedure MoveMultiPoint (multiPoint: TMultiPoint; direction, distance: Double);
var
    i:          Integer;
    tmpPoint:   Geometry.TPoint;
begin
    for i:=0 to multiPoint.Count - 1 do
    begin
        tmpPoint := PointAlongPlane (multiPoint.Point [i], direction, distance);
        multiPoint.Point [i].Assign (tmpPoint);
        tmpPoint.Free;
    end;
end;

function LineAlong (basePoint: Geometry.TPoint; Direction, Distance: Double): TPolyline;
var
	tmpPoint:	Geometry.TPoint;
	tmpPart:	TPart;
begin
	result := TPolyline.Create;
	tmpPart := TPart.Create;

	tmpPoint := PointAlongPlane (basePoint, Direction, -Distance);
	tmpPart.AddPoint (tmpPoint);
	tmpPoint.Free;

	tmpPoint := PointAlongPlane (basePoint, Direction, Distance);
	tmpPart.AddPoint (tmpPoint);
	tmpPoint.Free;

	result.AddPart (tmpPart);
	tmpPart.Free;
end;

function IsSameSide (side1, side2: TSideDirection): Boolean;
begin
	result := (Integer (side1) * Integer(side2) > 0);
end;

function UnionProtectedArea (pointNearRemovedRing: Geometry.TPoint; area1: TProtectedArea; area2: TProtectedArea): TProtectedArea;
var
	resArea:    TProtectedArea;
	geom:       TGeometry;
begin

	resArea := TProtectedArea.Create;

	if (not IsGeometryEmpty (area1.LeftArea)) or (not IsGeometryEmpty (area2.LeftArea)) then
	begin
		geom := GeoOper.unionGeometry (area1.LeftArea, area2.LeftArea);
		if (not IsGeometryEmpty (geom)) and
			((geom.AsPolygon.Count = 1) or
				GeoOper.crosses (area1.LeftArea, area2.LeftArea)) then
		begin
//			RemoveNearestRing (pointNearRemovedRing, geom.AsPolygon);
			resArea.LeftArea.Assign (geom);
		end;
		geom.Free;
	end;

	if (not IsGeometryEmpty (area1.PrimaryArea)) or (not IsGeometryEmpty (area2.PrimaryArea)) then
	begin
		geom := GeoOper.unionGeometry (area1.PrimaryArea, area2.PrimaryArea);
		if not IsGeometryEmpty (geom) then
		begin
            resArea.PrimaryArea.Assign (geom);
        end;
        geom.Free;
	end;

	if (not IsGeometryEmpty (area1.RightArea)) or (not IsGeometryEmpty (area2.RightArea)) then
	begin
		geom := GeoOper.unionGeometry (area1.RightArea, area2.RightArea);
		if (not IsGeometryEmpty (geom)) and
			((geom.AsPolygon.Count = 1) or GeoOper.crosses (area1.RightArea, area2.RightArea)) then
		begin
//			RemoveNearestRing (pointNearRemovedRing, geom.AsPolygon);
			resArea.RightArea.Assign (geom);
		end;
		geom.Free;
	end;

	if (not IsGeometryEmpty (area1.NominalLine)) or (not IsGeometryEmpty (area2.NominalLine)) then
    begin
        geom := GeoOper.unionGeometry (area1.NominalLine, area2.NominalLine);
		if not IsGeometryEmpty (geom) then
            resArea.NominalLine.Assign (geom);
        geom.Free;
    end;

    result := resArea;
end;

function IsGeometryEmpty (geom: TGeometry): Boolean;
begin
    result := True;
    case geom.GeometryType of
        gtNull:     result := True;
        gtPoint:    result := geom.AsPoint.IsEmpty;
        gtMultiPoint,
            gtPart,
            gtRing: result := IsMultiPointEmpty (geom.AsMultiPoint);
        gtPolyline: result := IsPolylineEmpty (geom.AsPolyline);
        gtPolygon:  result := IsPolygonEmpty (geom.AsPolygon);
    end;
end;

function IsPolylineEmpty (geomCol: TPolyline): Boolean;
var
    i:  Integer;
begin
    for i:=0 to geomCol.Count - 1 do
    begin
        if not IsMultiPointEmpty (geomCol.Part [i].AsMultiPoint) then
        begin
            result := False;
            exit;
        end;
    end;
    result := True;
end;

function IsPolygonEmpty (geomCol: TPolygon): Boolean;
var
    i:  Integer;
begin
    for i:=0 to geomCol.Count - 1 do
    begin
        if not IsMultiPointEmpty (geomCol.Ring [i].AsMultiPoint) then
        begin
            result := False;
            exit;
        end;
    end;
    result := True;
end;

function IsMultiPointEmpty (geomCol: TMultiPoint): Boolean;
var
    i:  Integer;
begin
    for i:=0 to geomCol.Count - 1 do
    begin
		if not geomCol.Point [i].IsEmpty then
        begin
            result := False;
            exit;
        end;
    end;
    result := True;
end;

procedure CutProtectedArea (const pointNearRemovedRing: Geometry.TPoint; const area: TProtectedArea; cutterLine: TPolyline;
			resultGeomSide: TSideDirection; var areaLeft, areaRight: TProtectedArea);
var
	resGeomSideVal: Integer;
	geomLeft,
	geomRight:      TGeometry;
  forDebug: Boolean;
begin
	areaLeft := nil;
	areaRight := nil;

	resGeomSideVal := Integer (resultGeomSide);

	if resGeomSideVal <= 0 then
		areaLeft := TProtectedArea.Create;
	if resGeomSideVal >= 0 then
		areaRight := TProtectedArea.Create;

	if not IsGeometryEmpty (area.LeftArea) then
	begin

		forDebug := false;
		if forDebug then
		begin
		  DrawGeometry(area.LeftArea);
		  DrawGeometry(cutterLine);
		end;

		GeoOper.cut (area.LeftArea, cutterLine, geomLeft, geomRight);

		if resGeomSideVal <= 0 then
		begin
			if geomLeft.GeometryType <> gtNull then
			begin
//				RemoveNearestRing (pointNearRemovedRing, geomLeft.AsPolygon);
				areaLeft.LeftArea.Assign (geomLeft);
			end;
		end;

		if resGeomSideVal >= 0 then
		begin
			if geomRight.GeometryType <> gtNull then
			begin
//				RemoveNearestRing (pointNearRemovedRing, geomRight.AsPolygon);
				areaRight.LeftArea.Assign (geomRight);
            end;
	    end;
	    geomLeft.Free;
	    geomRight.Free;
    end;



    if not IsGeometryEmpty (area.PrimaryArea) then
    begin
	    GeoOper.cut (area.PrimaryArea, cutterLine, geomLeft, geomRight);
	    if resGeomSideVal <= 0 then
	    begin
	        if geomLeft.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomLeft.AsPolygon);
	            areaLeft.PrimaryArea.Assign (geomLeft);
            end;
	    end;
	    if resGeomSideVal >= 0 then
	    begin
	        if geomRight.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomRight.AsPolygon);
	            areaRight.PrimaryArea.Assign (geomRight);
            end;
	    end;
	    geomLeft.Free;
	    geomRight.Free;
    end;

    if not IsGeometryEmpty (area.RightArea) then
    begin
	    GeoOper.cut (area.RightArea, cutterLine, geomLeft, geomRight);
	    if resGeomSideVal <= 0 then
	    begin
	        if geomLeft.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomLeft.AsPolygon);
	            areaLeft.RightArea.Assign (geomLeft);
            end;
	    end;
	    if resGeomSideVal >= 0 then
	    begin
	        if geomRight.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomRight.AsPolygon);
	            areaRight.RightArea.Assign (geomRight);
            end;
	    end;
	    geomLeft.Free;
	    geomRight.Free;
    end;

    if not IsGeometryEmpty (area.NominalLine) then
    begin

//      DrawGeometry(area.NominalLine);
//      DrawGeometry(cutterLine);
//      DrawGeometry(geomLeft);
//      DrawGeometry(geomRight);

	    GeoOper.Cut (area.NominalLine, cutterLine, geomLeft, geomRight);
	    if resGeomSideVal <= 0 then
	    begin
	        if geomLeft.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomLeft.AsPolygon);
	            areaLeft.NominalLine.Assign (geomLeft);
            end;
	    end;
	    if resGeomSideVal >= 0 then
	    begin
	        if geomRight.GeometryType <> gtNull then
            begin
//				RemoveNearestRing (pointNearRemovedRing, geomRight.AsPolygon);
	            areaRight.NominalLine.Assign (geomRight);
            end;
	    end;
	    geomLeft.Free;
	    geomRight.Free;
    end;
end;

procedure RemoveNearestRing (point: Geometry.TPoint; polygon: TPolygon);
var
	d0, d1:		Double;
	tmpPolygon:	TPolygon;
begin
	while polygon.Count > 1 do
	begin
		tmpPolygon := TPolygon.Create;
		tmpPolygon.AddRing (polygon.Ring [0]);
		d0 := GeoOper.getDistance (point, tmpPolygon);
		tmpPolygon.Ring [0].Assign (polygon.Ring [1]);
		d1 := GeoOper.getDistance (point, tmpPolygon);
		if d0 < d1 then
			polygon.RemoveAt (0)
        else
			polygon.RemoveAt (1);
    end;
end;

procedure DrawProtectedArea_ForDebug (area: TProtectedArea);
var
	fillSymbol:	TFillSymbol;
begin
	fillSymbol := TFillSymbol.Create;
	fillSymbol.Style := sfsHorizontal;
	fillSymbol.Color := RGB (0, 50, 200);
	fillSymbol.Outline.Color := RGB (0, 50, 200);

	if not IsGeometryEmpty (area.PrimaryArea) then
		DrawGeometry (area.PrimaryArea, fillSymbol);


	fillSymbol.Color := RGB (200, 50, 0);
	fillSymbol.Outline.Color := RGB (200, 50, 0);

	if not IsGeometryEmpty (area.LeftArea) then
		DrawGeometry (area.LeftArea, fillSymbol);
	if not IsGeometryEmpty (area.RightArea) then
		DrawGeometry (area.RightArea, fillSymbol);

	fillSymbol.Free;
end;

procedure GetDMEToleranceParams (
            isDERPoint: Boolean;
            elevation: Double;
            isDMEtill_1989: Boolean;
            countDME: Integer;
            var XTT: Double;
            var ATT: Double;
            var semiWidth: Double);
const
    ST = 460.0;
var
    FTT,
    DTT:    Double;
begin
    if isDERPoint then
        FTT := 190.0
    else
        FTT := 930.0;


    if isDMEtill_1989 then
    begin
        if Elevation < 0 then
            Elevation := 0;
        DTT := 4.11 * Sqrt (Elevation) * 12.5 + 463.0;
    end
    else
    begin
        DTT := 930.0;
    end;

    if countDME = 2 then
        DTT := DTT * 1.29;

    XTT := Sqrt (Sqr (DTT) + Sqr (FTT) + Sqr (ST));
    ATT := Sqrt (Sqr (DTT) + Sqr (ST));
    semiWidth := 1.5 * XTT + 930.0;
    if semiWidth < 1852.0 then
        semiWidth := 1852.0;
end;

function CreateIString (value: string): IString;
begin
	result := CoStringClass.Create () as IString;
	result.Value := value;
end;

function CreateIBoolean (value: Boolean): IBoolean;
begin
	result := CoBooleanClass.Create () as IBoolean;
	result.Value := value;
end;

function CreateIInt32 (value: Integer): IInt32;
begin
	result := CoInt32Class.Create () as IInt32;
	result.Value := value;
end;

function CreateIDouble (value: Double): IDouble;
begin
	result := CoDoubleClass.Create () as IDouble;
	result.Value := value;
end;

function CreateDefaulIDistance (value: Double): IDistance;
begin
	result := CoDistance.Create () as IDistance;
	result.value := FloatToStr (value);
	result.uom := UomDistance_M;
end;

function CreateDefaulIDistanceVertical (value: Double): IDistanceVertical;
begin
	result := CoDistanceVertical.Create () as IDistanceVertical;
	result.value := FloatToStr (value);
	result.uom := UomDistanceVertical_M;
end;

function PolylineToIAIXMCurve (aranPolyline: TPolyline): IAIXMCurve;
var
	i, j: Integer;
	gmlPart: IGMLPart;
	gmlPoint: IGMLPoint;
	aranPoint: Geometry.TPoint;
begin
	result := CoAIXMCurve.Create () as IAIXMCurve;
	result.Polyline := CoGMLPolyline.Create () as IGMLPolyline;
	for i := 0 to aranPolyline.Count - 1 do
	begin
		gmlPart := CoGMLPart.Create () as IGMLPart;
		for j := 0 to aranPolyline.Part [i].Count - 1 do
		begin
			aranPoint := PrjToGeo (aranPolyline.Part [i].Point [j]).AsPoint;
			gmlPoint := CoGMLPoint.Create () as IGMLPoint;
			gmlPoint.PutCoord (aranPoint.X, aranPoint.Y);
			gmlPart.Add (gmlPoint);
		end;
		result.Polyline.Add (gmlPart);
	end;
end;

end.
