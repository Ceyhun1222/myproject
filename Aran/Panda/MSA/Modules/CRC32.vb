Option Strict Off
Option Explicit On

Module CRC32

	'Test simples
	'480637N                 -> A5A7C704
	'0163411E                -> A1AE5741
	'480637N0163411E         -> A1BA30EE

	Private Const CRC32Poly As UInteger = &H814141ABUI
	Private CRCTable() As UInteger = {&H0UI, &H814141ABUI, &H83C3C2FDUI, &H2828356UI, &H86C6C451UI, &H78785FAUI, &H50506ACUI, &H84444707UI, &H8CCCC909UI, &HD8D88A2UI, &HF0F0BF4UI, &H8E4E4A5FUI, &HA0A0D58UI, &H8B4B4CF3UI, &H89C9CFA5UI, &H8888E0EUI, &H98D8D3B9UI, &H19999212UI, &H1B1B1144UI, &H9A5A50EFUI, &H1E1E17E8UI, &H9F5F5643UI, &H9DDDD515UI, &H1C9C94BEUI, &H14141AB0UI, &H95555B1BUI, &H97D7D84DUI, &H169699E6UI, &H92D2DEE1UI, &H13939F4AUI, &H11111C1CUI, &H90505DB7UI, &HB0F0E6D9UI, &H31B1A772UI, &H33332424UI, &HB272658FUI, &H36362288UI, &HB7776323UI, &HB5F5E075UI, &H34B4A1DEUI, &H3C3C2FD0UI, &HBD7D6E7BUI, &HBFFFED2DUI, &H3EBEAC86UI, &HBAFAEB81UI, &H3BBBAA2AUI, &H3939297CUI, &HB87868D7UI, &H28283560UI, &HA96974CBUI, &HABEBF79DUI, &H2AAAB636UI, &HAEEEF131UI, &H2FAFB09AUI, &H2D2D33CCUI, &HAC6C7267UI, &HA4E4FC69UI, &H25A5BDC2UI, &H27273E94UI, &HA6667F3FUI, &H22223838UI, &HA3637993UI, &HA1E1FAC5UI, &H20A0BB6EUI, &HE0A08C19UI, &H61E1CDB2UI, &H63634EE4UI, &HE2220F4FUI, &H66664848UI, &HE72709E3UI, &HE5A58AB5UI, &H64E4CB1EUI, &H6C6C4510UI, &HED2D04BBUI, &HEFAF87EDUI, &H6EEEC646UI, &HEAAA8141UI, &H6BEBC0EAUI, &H696943BCUI, &HE8280217UI, &H78785FA0UI, &HF9391E0BUI, &HFBBB9D5DUI, &H7AFADCF6UI, &HFEBE9BF1UI, &H7FFFDA5AUI, &H7D7D590CUI, &HFC3C18A7UI, &HF4B496A9UI, &H75F5D702UI, &H77775454UI, &HF63615FFUI, &H727252F8UI, &HF3331353UI, &HF1B19005UI, &H70F0D1AEUI, &H50506AC0UI, &HD1112B6BUI, &HD393A83DUI, &H52D2E996UI, &HD696AE91UI, &H57D7EF3AUI, &H55556C6CUI, &HD4142DC7UI, &HDC9CA3C9UI, &H5DDDE262UI, &H5F5F6134UI, &HDE1E209FUI, &H5A5A6798UI, &HDB1B2633UI, &HD999A565UI, &H58D8E4CEUI, &HC888B979UI, &H49C9F8D2UI, &H4B4B7B84UI, &HCA0A3A2FUI, &H4E4E7D28UI, &HCF0F3C83UI, &HCD8DBFD5UI, &H4CCCFE7EUI, &H44447070UI, &HC50531DBUI, &HC787B28DUI, &H46C6F326UI, &HC282B421UI, &H43C3F58AUI, &H414176DCUI, &HC0003777UI, &H40005999UI, &HC1411832UI, &HC3C39B64UI, &H4282DACFUI, &HC6C69DC8UI, &H4787DC63UI, &H45055F35UI, &HC4441E9EUI, &HCCCC9090UI, &H4D8DD13BUI, &H4F0F526DUI, &HCE4E13C6UI, &H4A0A54C1UI, &HCB4B156AUI, &HC9C9963CUI, &H4888D797UI, &HD8D88A20UI, &H5999CB8BUI, &H5B1B48DDUI, &HDA5A0976UI, &H5E1E4E71UI, &HDF5F0FDAUI, &HDDDD8C8CUI, &H5C9CCD27UI, &H54144329UI, &HD5550282UI, &HD7D781D4UI, &H5696C07FUI, &HD2D28778UI, &H5393C6D3UI, &H51114585UI, &HD050042EUI, &HF0F0BF40UI, &H71B1FEEBUI, &H73337DBDUI, &HF2723C16UI, &H76367B11UI, &HF7773ABAUI, &HF5F5B9ECUI, &H74B4F847UI, &H7C3C7649UI, &HFD7D37E2UI, &HFFFFB4B4UI, &H7EBEF51FUI, &HFAFAB218UI, &H7BBBF3B3UI, &H793970E5UI, &HF878314EUI, &H68286CF9UI, &HE9692D52UI, &HEBEBAE04UI, &H6AAAEFAFUI, &HEEEEA8A8UI, &H6FAFE903UI, &H6D2D6A55UI, &HEC6C2BFEUI, &HE4E4A5F0UI, &H65A5E45BUI, &H6727670DUI, &HE66626A6UI, &H622261A1UI, &HE363200AUI, &HE1E1A35CUI, &H60A0E2F7UI, &HA0A0D580UI, &H21E1942BUI, &H2363177DUI, &HA22256D6UI, &H266611D1UI, &HA727507AUI, &HA5A5D32CUI, &H24E49287UI, &H2C6C1C89UI, &HAD2D5D22UI, &HAFAFDE74UI, &H2EEE9FDFUI, &HAAAAD8D8UI, &H2BEB9973UI, &H29691A25UI, &HA8285B8EUI, &H38780639UI, &HB9394792UI, &HBBBBC4C4UI, &H3AFA856FUI, &HBEBEC268UI, &H3FFF83C3UI, &H3D7D0095UI, &HBC3C413EUI, &HB4B4CF30UI, &H35F58E9BUI, &H37770DCDUI, &HB6364C66UI, &H32720B61UI, &HB3334ACAUI, &HB1B1C99CUI, &H30F08837UI, &H10503359UI, &H911172F2UI, &H9393F1A4UI, &H12D2B00FUI, &H9696F708UI, &H17D7B6A3UI, &H155535F5UI, &H9414745EUI, &H9C9CFA50UI, &H1DDDBBFBUI, &H1F5F38ADUI, &H9E1E7906UI, &H1A5A3E01UI, &H9B1B7FAAUI, &H9999FCFCUI, &H18D8BD57UI, &H8888E0E0UI, &H9C9A14BUI, &HB4B221DUI, &H8A0A63B6UI, &HE4E24B1UI, &H8F0F651AUI, &H8D8DE64CUI, &HCCCA7E7UI, &H44429E9UI, &H85056842UI, &H8787EB14UI, &H6C6AABFUI, &H8282EDB8UI, &H3C3AC13UI, &H1412F45UI, &H80006EEEUI}
	Private CharTab() As Char = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"}

	Function IntToHexStr(ByVal Val_Renamed As ULong) As String
		Dim I As Integer

		IntToHexStr = ""
		For I = 0 To 7
			IntToHexStr = CharTab(Val_Renamed And 15) + IntToHexStr
			Val_Renamed = ((Val_Renamed And &HFFFFFFF0UI) \ 16)
		Next I
	End Function

	Function CalcCRC32(ByVal Str_Renamed As String) As String
		Dim I As Integer
		Dim L As Integer
		Dim CRCValue As ULong
		Dim Index As UInteger
		Dim B As Byte

		L = Len(Str_Renamed)
		CRCValue = 0

		For I = 1 To L
			B = Asc(Mid(Str_Renamed, I, 1))
			Index = ((CRCValue \ 16777216) Xor B) And 255
			CRCValue = CRCValue And &HFFFFFF
			CRCValue = (CRCValue * 256) Xor CRCTable(Index)
		Next I

		Return IntToHexStr(CRCValue)

	End Function
End Module
