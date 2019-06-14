using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XHTML_WPF.Classes
{
    public static class Parser
    {
        private static int cnt = 0;
        private static HtmlDocument doc = new HtmlDocument();

        public static string Export(string html)
        {
            try
            {
                if (string.IsNullOrEmpty(html)) return String.Empty;

                html = PreProcessHTML(html);

                var tables = doc.DocumentNode.SelectNodes("//table");

                if (tables != null)
                {

                    foreach (var item in tables)
                    {
                        string tableStyle = item.Attributes["style"].Value;
                        var match = Regex.Match(tableStyle, "width:(.*?);");

                        string customTable = "<table align=\"center\" style=\"width:100%;\">";
                        if (match.Success)
                        {
                            customTable = "<table align=\"center\" style=\"width:" + match.Groups[1].Value + ";\">";
                        }
                        string thead = "";
                        string tbody = "";
                        var rows = item.SelectNodes("tr[td[contains(@style, 'background-color: #AAAACC;')]]");
                        var numbering_rows = item.SelectNodes("tr[td[contains(@style, 'background-color: #DDDDDD;')]]");

                        if (rows != null || numbering_rows != null)
                        {
                            thead += "<thead>";
                            if (rows != null)
                            {
                                foreach (var row in rows)
                                {

                                    thead += "<tr class=\"Table-row-type-1\">";
                                    var cells = row.SelectNodes("td");
                                    if (cells != null)
                                    {
                                        foreach (var cell in cells)
                                        {
                                            string cellColSpan = "";
                                            string cellRowSpan = "";
                                            if (cell.Attributes.Contains(@"colspan"))
                                            {
                                                cellColSpan = cell.Attributes["colspan"].Value;

                                            }

                                            if (cell.Attributes.Contains(@"rowspan"))
                                            {
                                                cellRowSpan = cell.Attributes["rowspan"].Value;

                                            }

                                            if (cell.Attributes.Contains(@"style"))
                                            {
                                                string cellStyle = cell.Attributes["style"].Value;
                                                var match_cell = Regex.Match(cellStyle, "width:(.*?);");

                                                if (cellStyle.Contains("background-color: #AAAACC;"))
                                                {
                                                    thead += "<th " + (match_cell.Success ? "style=\"width:" + match_cell.Groups[1].Value + ";\"" : "") + " " + (!string.IsNullOrEmpty(cellColSpan) ? "colspan =\"" + cellColSpan + "\"" : "") + " " + (!string.IsNullOrEmpty(cellRowSpan) ? "rowspan=\"" + cellRowSpan + "\"" : "") + " class=\"bleft btop bright bbottom\">" + cell.InnerHtml + "</th>";
                                                }

                                            }
                                        }
                                    }
                                    thead += "</tr>";
                                }
                            }

                            if (numbering_rows != null)
                            {
                                foreach (var row in numbering_rows)
                                {
                                    thead += "<tr class=\"Table-row-type-2\">";
                                    var cells = row.SelectNodes("td");
                                    if (cells != null)
                                    {
                                        foreach (var cell in cells)
                                        {
                                            string cellColSpan = "";
                                            string cellRowSpan = "";
                                            if (cell.Attributes.Contains(@"colspan"))
                                            {
                                                cellColSpan = cell.Attributes["colspan"].Value;

                                            }

                                            if (cell.Attributes.Contains(@"rowspan"))
                                            {
                                                cellRowSpan = cell.Attributes["rowspan"].Value;

                                            }

                                            if (cell.Attributes.Contains(@"style"))
                                            {
                                                string cellStyle = cell.Attributes["style"].Value;
                                                var match_cell = Regex.Match(cellStyle, "width:(.*?);");

                                                if (cellStyle.Contains("background-color: #DDDDDD;"))
                                                {
                                                    thead += "<th " + (match_cell.Success ? "style=\"width:" + match_cell.Groups[1].Value + ";\"" : "") + " " + (!string.IsNullOrEmpty(cellColSpan) ? "colspan=\"" + cellColSpan + "\"" : "") + " " + (!string.IsNullOrEmpty(cellRowSpan) ? "rowspan=\"" + cellRowSpan + "\"" : "") + " class=\"bleft btop bright bbottom\">" + cell.InnerHtml + "</th>";
                                                }
                                            }
                                        }
                                    }
                                    thead += "</tr>";
                                }
                            }
                            thead += "</thead>";
                            customTable += thead;
                        }

                        var tbody_rows = item.SelectNodes("tr[td[not(contains(@style, 'background-color: #AAAACC;')) and not(contains(@style, 'background-color: #DDDDDD;'))]]");

                        if (tbody_rows != null)
                        {
                            tbody += "<tbody>";
                            foreach (var row in tbody_rows)
                            {
                                tbody += "<tr>";
                                var cells = row.SelectNodes("td");
                                if (cells != null)
                                {
                                    foreach (var cell in cells)
                                    {
                                        string cellColSpan = "";
                                        string cellRowSpan = "";
                                        if (cell.Attributes.Contains(@"colspan"))
                                        {
                                            cellColSpan = cell.Attributes["colspan"].Value;

                                        }

                                        if (cell.Attributes.Contains(@"rowspan"))
                                        {
                                            cellRowSpan = cell.Attributes["rowspan"].Value;

                                        }

                                        string cellStyle = cell.Attributes["style"].Value;
                                        var match_cell = Regex.Match(cellStyle, "width:(.*?);");
                                        
                                        if (match_cell.Success)
                                        {
                                            tbody += "<td style=\"width:" + match_cell.Groups[1].Value + ";\"" + (!string.IsNullOrEmpty(cellColSpan) ? "colspan=\"" + cellColSpan + "\"" : "") + " " + (!string.IsNullOrEmpty(cellRowSpan) ? "rowspan=\"" + cellRowSpan + "\"" : "") + " class=\"bleft btop bright bbottom\">" + cell.InnerHtml + "</td>";

                                        }
                                        else
                                        {
                                            tbody += "<td " + (!string.IsNullOrEmpty(cellColSpan) ? "colspan=\"" + cellColSpan + "\"" : "") + " " + (!string.IsNullOrEmpty(cellRowSpan) ? "rowspan=\"" + cellRowSpan + "\"" : "") + " class=\"bleft btop bright bbottom\" style=\"width: 216px;\">" + cell.InnerHtml + "</td>";

                                        }
                                    }
                                }
                                tbody += "</tr>";
                            }
                            tbody += "</tbody>";
                            customTable += tbody;
                        }
                        customTable += "</table>";
                        html = html.Replace(item.OuterHtml, customTable);
                    }
                }

                var listsUL = doc.DocumentNode.SelectNodes("//ul");

                if (listsUL != null)
                {
                    foreach (var list in listsUL)
                    {
                        html = listParser(list, html);
                    }

                }

                var listsOL = doc.DocumentNode.SelectNodes("//ol");

                if (listsOL != null)
                {
                    foreach (var list in listsOL)
                    {
                        html = listParser(list, html);
                    }

                }

                var paragraphs = doc.DocumentNode.SelectNodes("//p");

                if (paragraphs != null)
                {

                    foreach (var item in paragraphs)
                    {
                        var paragraphSpans = item.SelectNodes("span");
                        string pStyle = item.Attributes["style"].Value;
                        if (paragraphSpans != null)
                        {
                            foreach (var paragraphSpan in paragraphSpans)
                            {
                                string style = paragraphSpan.Attributes["style"].Value;
                                if (style.Contains("font-size: 26.6666666666667px;") && style.Contains("color: #800000;"))
                                {
                                    html = html.Replace(item.OuterHtml, "<h4 class=\"Title\">" + paragraphSpan.InnerHtml + "</h4>");
                                }
                                else if (style.Contains("font-size: 21.3333333333333px;") && style.Contains("color: #800000;"))
                                {
                                    html = html.Replace(item.OuterHtml, "<h5 class=\"Sub-title\">" + paragraphSpan.InnerHtml + "</h5>");
                                }
                                else
                                {
                                    if (pStyle.Contains("text-align:") || 
                                        pStyle.Contains("margin-left:"))
                                    {
                                        string styleInject = pStyle.Contains("text-align:")? "text-align: " + pStyle.ToStringBetween("text-align:", ";") + ";" : "";
                                        styleInject += (pStyle.Contains("margin-left:") && !pStyle.Contains("margin-left: 0px;")) ? "margin-left: " + pStyle.ToStringBetween("margin-left:", ";") + ";" : "";
                                        html = html.Replace(item.OuterHtml, $@"<p style=""{styleInject}"">" + item.InnerHtml + "</p>");
                                    }
                                    else
                                        html = html.Replace(item.OuterHtml, "<p>" + item.InnerHtml + "</p>");
                                }

                                if (style.Contains("font-size: 16px;"))
                                {
                                    if (paragraphSpan.InnerHtml.Contains("<strong>"))
                                    {
                                        html = html.Replace(paragraphSpan.OuterHtml, paragraphSpan.InnerHtml);

                                    }
                                    if (paragraphSpan.InnerHtml.Contains("<em>"))
                                    {
                                        html = html.Replace(paragraphSpan.OuterHtml, paragraphSpan.InnerHtml);

                                    }
                                    else
                                    {
                                        html = html.Replace(paragraphSpan.OuterHtml, paragraphSpan.InnerHtml);
                                    }
                                }
                            }
                        }
                    }
                }


                var spans = doc.DocumentNode.SelectNodes("//span");

                if (spans != null)
                {
                    foreach (var item in spans)
                    {
                        html = html.Replace(item.OuterHtml, item.InnerHtml);
                    }
                }

                doc.LoadHtml(html);
                var paragraphs2 = doc.DocumentNode.SelectNodes("//p");

                if (paragraphs2 != null)
                {
                    foreach (var item in paragraphs2)
                    {
                        if (item.InnerHtml.Trim() == String.Empty)
                        {
                            html = html.Replace(item.OuterHtml, @"<br />");
                        }
                    }
                }

                html = PostProcessHTML(html);
                return html;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static string PreProcessHTML(string html)
        {
            try
            {
                doc.LoadHtml(html);
                FixNodes(doc.DocumentNode);
                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static string PostProcessHTML(string html)
        {
            try
            {
                doc.LoadHtml(html);
                DeFixNodes(doc.DocumentNode);
                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static void FixNodes(HtmlNode node)
        {
            try
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    HtmlNode n = node.ChildNodes[i];
                    if (n.Name != "#text" && !n.Attributes.Contains("id"))
                    {
                        n.Attributes.Add("id", "id" + cnt++);
                        FixNodes(n);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void DeFixNodes(HtmlNode node)
        {
            try
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    HtmlNode n = node.ChildNodes[i];
                    if (n.NodeType == HtmlNodeType.Element)
                    {
                        if (n.Attributes.Contains("id") && n.Attributes["id"].Value.StartsWith("id"))
                        {
                            n.Attributes.Remove("id");
                        }
                        DeFixNodes(n);
                    }
                    else
                    {
                        Console.WriteLine(n.InnerHtml);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static string listParser(HtmlNode list, string html)
        {
            try
            {
                var nestedListsUL = list.SelectNodes("ul");
                if (nestedListsUL != null)
                {

                    foreach (var nestedList in nestedListsUL)
                    {
                        if (nestedList.PreviousSibling != null && nestedList.PreviousSibling.Name.Equals("li"))
                        {
                            html = html.Replace(nestedList.OuterHtml, "");
                            string item = "<li>" + nestedList.PreviousSibling.InnerHtml + " " + nestedList.OuterHtml + "</li>";
                            html = html.Replace(nestedList.PreviousSibling.OuterHtml, item);
                        }
                    }
                }

                var nestedListsOL = list.SelectNodes("ol");
                if (nestedListsOL != null)
                {

                    foreach (var nestedList in nestedListsOL)
                    {
                        if (nestedList.PreviousSibling != null && nestedList.PreviousSibling.Name.Equals("li"))
                        {
                            html = html.Replace(nestedList.OuterHtml, "");
                            string item = "<li>" + nestedList.PreviousSibling.InnerHtml + " " + nestedList.OuterHtml + "</li>";
                            html = html.Replace(nestedList.PreviousSibling.OuterHtml, item);
                        }
                    }
                }

                var listItems = list.SelectNodes("li");

                if (listItems != null)
                {

                    foreach (var li in listItems)
                    {
                        html = html.Replace(li.OuterHtml, "<li>" + li.InnerHtml + "</li>");
                    }
                }
                return html;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return html;
            }
        }

    }
}
