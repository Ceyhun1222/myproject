using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArenaStatic;
using Novacode;

namespace ChartDataTabulation
{
    public class SIDTabulation
    {
		public void CrateTable ( List<PDM.PDMObject> procList, string filename)
		{
			using ( DocX document = DocX.Create ( filename) )
			{
                if (procList.Count > 0 && procList[0] is PDM.StandardInstrumentDeparture)
                {
                    foreach (PDM.StandardInstrumentDeparture sid in procList)
                    {
                        // Add a Table into the document.
                        Table table = document.AddTable(2, 3);
                        table.Alignment = Alignment.center;
                        table.Rows[0].Cells[0].Paragraphs[0].Append("Fix/point");
                        table.Rows[0].Cells[1].Paragraphs[0].Append("Coordinates");
                        table.Rows[0].Cells[2].Paragraphs[0].Append("ARINC Type");
                        if (sid.Transitions == null)
                            continue;
                        foreach (var transition in sid.Transitions)
                        {
                            if (transition.Legs == null)
                                continue;
                            foreach (var leg in transition.Legs)
                            {
                                if (leg.EndPoint == null)
                                    continue;
                                Row newRow = table.InsertRow();
                                newRow.Cells[0].Paragraphs[0].Append(leg.EndPoint.SegmentPointDesignator);
                                newRow.Cells[1].Paragraphs[0].Append(ArenaStaticProc.LatToDDMMSS(leg.EndPoint.Lat, ANCOR.MapCore.coordtype.DDMMSSN_1) + " " + ArenaStaticProc.LonToDDMMSS(leg.EndPoint.Lon, ANCOR.MapCore.coordtype.DDMMSSN_1));
                                newRow.Cells[2].Paragraphs[0].Append(leg.LegTypeARINC.ToString());
                            }
                        }
                        // Insert a new Paragraph into the document.
                        Paragraph title = document.InsertParagraph();
                        title.AppendLine(sid.Designator);
                        title.AppendLine();
                        title.FontSize(20);
                        title.Alignment = Alignment.center;
                        //title.InsertTableAfterSelf ( table );
                        document.InsertTable(table);
                    }
                }
				document.Save ( );

			}
			Process.Start ( filename);
		}
    }
}
