unit ReportUnit_;

interface

uses
	Geometry,
	ComCtrls;

type

	TReportFile = class
	private
		FFileName: String;
		FFile: TextFile;
	public
		DerPtPrj: TPoint;
		ThrPtPrj: TPoint;

		constructor Create (Name, ReportTitle: string);
		destructor Destroy; override;

		procedure WriteTextLine (Text: string);
		procedure WriteText (Text: string);
		procedure WriteMessage (message: string = '');
		procedure WriteParam (ParamName, ParamValue: string; ParamUnit: string = '');
		procedure WriteTab (ListView: TListView; TabComment: string = '');
		procedure WriteImage (ImageName: string; ImageComment: string = '');
		procedure WriteImageLink (ImageName: string; ImageComment: string = '');
	end;

implementation

uses
	SysUtils,
	ARANMath,
	UnitConverter,
	Parameter,
	Classes;

const
	ReportFileExt = '.htm';
	FootCoeff = 0.3048;

constructor TReportFile.Create(Name, ReportTitle: string);
begin
	Inherited Create;
	FFileName := Name;

	if (FFileName = '') then
		raise EInOutError.Create('Invalid report file name.');

	if ExtractFileExt(FFileName) = '' then
		FFileName := Name + ReportFileExt;

	AssignFile(FFile, FFileName);
	Rewrite(FFile);

	writeln(FFile, '<html>');
	writeln(FFile, '<head>');
	writeln(FFile, '<title>PANDA-RNAV - ' + ReportTitle + '</title>');
	writeln(FFile, '<style>');
	writeln(FFile, 'body {font-family: Arial, Sans-Serif; font-size:12;}');
	writeln(FFile, 'table {font-family: Arial, Sans-Serif; font-size:12;}');
	writeln(FFile, '</style>');
	writeln(FFile, '</head>');
	writeln(FFile, '<body>');
end;

destructor TReportFile.Destroy;
begin
	writeln (FFile, '</body>');
	writeln (FFile, '</html>');
	Close (FFile);
	Inherited;
end;

procedure TReportFile.WriteText (Text: string);
begin
	writeln(FFile, Text)
end;

procedure TReportFile.WriteTextLine (Text: string);
begin
	writeln(FFile, Text + '<br>');
end;

procedure TReportFile.WriteMessage(message: string = '');
var
	sTmp:	string;
begin
	sTmp := '';
	If (Message <> '') then
		sTmp := '<b>' + Message + '</b>';

	writeln(FFile, sTmp + '<br>');
end;

procedure TReportFile.WriteParam (ParamName, ParamValue: string; ParamUnit: string = '');
var
	sTmp:	string;
begin
	if (ParamName = '') or (ParamValue = '') then
		exit;

	sTmp := '<b>' + ParamName + ':</b> ' + ParamValue;
	if ParamUnit <> '' then
		sTmp := sTmp + ' ' + ParamUnit;

	writeln(FFile, sTmp + '<br>')
end;

procedure TReportFile.WriteTab (ListView: TListView; TabComment: string = '');
const
	alignArray: Array [TAlignment] of string = ('left', 'right', 'center');
var
	colCount,
	rowCount,
	I, J:	Integer;
	itmX:	TListItem;
	colorBegin,
	colorEnd: String;
	alignVal: String;
begin

	if not Assigned(ListView) then exit;
	If ListView.Items.Count = 0 then exit;

	if TabComment <> '' then
	begin
		WriteMessage(TabComment);
		WriteMessage;
	end;

	writeln(FFile, '<table border=''1'' cellspacing=''0'' cellpadding=''1''>');

	colCount := ListView.Columns.Count;
	rowCount := ListView.Items.Count;

	writeln(FFile, '<tr>');
	for I := 0 to colCount - 1 do
		writeln(FFile, '<td align=center><b>' + ListView.Columns.Items[I].Caption + '</b></td>');

	writeln(FFile, '</tr>');

	for I := 0 to rowCount - 1 do
	begin
		itmX := ListView.Items.Item[I];

		colorBegin := '';	//<Font color=FF0000>
		colorEnd := '';		//</Font>
		if Integer (ListView.Items [i].Data) <> -1 then
		begin
			colorBegin := '<Font color=FF0000>';
			colorEnd := '</Font>';
		end;

		writeln(FFile, '<tr>');
		writeln(FFile, '<td>' + colorBegin + itmX.Caption + colorEnd + '</td>');
		for J := 0 to colCount - 2 do
		begin
			alignVal := alignArray [ListView.Columns [j].Alignment];
			writeln(FFile, '<td align=' + alignVal + '>' + colorBegin + itmX.SubItems[J] + colorEnd + '</td>');
		end;

		writeln(FFile, '</tr>')
	end;

	writeln(FFile, '</table>');
	writeln(FFile, '<br><br>')
end;

{
procedure TReportFile.WriteObstData(ObstalceInfoList: TList; Ix: Integer = -1);
const
	HederCnt = 7;
	Headers: Array [0..HederCnt - 1] of string = ( 'Name', 'ID', 'Elevation', 'MOC', 'Req. OCA', 'D', 'Area');
										//'Req PDG', 'Dist', 'Area'
var
	sTmp,
	ColorStr,
	Face,
	EndFace,
	FontStr:		string;
	N, I:			Integer;
	Obstacle:		TObstacleInfo;
begin
	writeln(FFile, '<table border=''1'' cellspacing=''0'' cellpadding=''1''>');

	writeln(FFile, '<tr>');
	for I := 0 to HederCnt - 1 do
		writeln(FFile, '<td><b>' + Headers[I] + '</b></td>');

	writeln(FFile, '</tr>');

//=================================================

	N := ObstalceInfoList.Count;
	for I := 0 to N - 1 do
	begin
		if I = Ix then
		begin
			ColorStr := Chr(34) + 'FF0000' + Chr(34);
			Face := '<b>';
			EndFace := '</b>';
		end
		else
		begin
			ColorStr := Chr(34) + '000000' + Chr(34);
			Face := '';
			EndFace := ''
		end;
		FontStr := '<Font Color=' + ColorStr + '>' + Face;

		Obstacle := TObstacleInfo(ObstalceInfoList.Items[I]);

		writeln(FFile, '<Tr><Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + Obstacle.Name + EndFace + '</Td>');
		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + Obstacle.Host.AixmID + EndFace + '</Td>');

		sTmp := RoundToStr(ConvertAltitude(Obstacle.Elevation, cdToOuter), GAltitudeAccuracy, rtCeil);
		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + sTmp + EndFace + '</Td>');

		sTmp := RoundToStr(ConvertAltitude(Obstacle.MOC, cdToOuter), GAltitudeAccuracy);
		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + sTmp + EndFace + '</Td>');

		sTmp := RoundToStr(ConvertAltitude(Obstacle.ReqH, cdToOuter), GAltitudeAccuracy);
		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + sTmp + EndFace + '</Td>');

		sTmp := FormatFloat('0.000', Obstacle.fTmp);
		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + sTmp + EndFace + '</Td>');

//		fTmp := CConverters[puDistance].ConvertFunction(Obstacle.Dist, cdToOuter, nil);
//		writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + RoundToStr(fTmp, CConverters[puDistance].Accuracy) + EndFace + '</Td>');

		if (Obstacle.Flags and 1) = 1 then
			writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + 'Primary' + EndFace + '</Td>')
		else
			writeln(FFile, '<Td Width=' + Chr(34) + '9%' + Chr(34) + '>' + FontStr + 'Secondary' + EndFace + '</Td>');

		writeln(FFile, '</Tr>');
//		writeln(FFile, EndFace + '</Tr>');
	end;
//=======================================================================================================================
	writeln(FFile, '</table>');
	writeln(FFile, '<br><br>');
end;
}

procedure TReportFile.WriteImage(ImageName: string; ImageComment: string = '');
begin
	if ImageName = '' then exit;

	if ImageComment <> '' then
	begin
		WriteMessage(ImageComment);
		WriteMessage;
	end;

	writeln(FFile, '<img src=''' + ImageName + ''' border=''0''>')
end;

procedure TReportFile.WriteImageLink(ImageName: string; ImageComment: string = '');
begin
	if ImageName = '' then exit;
	writeln(FFile, '<a href=''' + ImageName + '''>' + ImageComment + '</a>')
end;

end.
