using AIP.DB;
using AIP.XML;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace AIP.GUI.Classes
{
    internal static class WebGenerate
    {
        /// <summary>
        /// Object collect all messages to send into Main form for the output with the AddOutput method
        /// </summary>
        internal static Queue<BaseLib.Struct.Output> OutputQueue = new Queue<BaseLib.Struct.Output>();

        /// <summary>
        /// Events for command line commands
        /// </summary>
        private static readonly AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
        private static readonly AutoResetEvent errorWaitHandle = new AutoResetEvent(false);

        /// <summary>
        /// Cover Generate authomatic (Razor) or semiauthomatic (Cover Page from DB)
        /// </summary>
        private static bool isCoverAuthomatic = false;

        internal static void Run(eAIPContext db)
        {
            try
            {
                Web_DefaultGenerate("Generating web menu items", "menu");
                Web_DefaultGenerate("Generating AMDT section", "AMDT");
                Web_DefaultGenerate("Generating the list of Supplements", "eSUPs", PathCategory.eSUP);
                Web_DefaultGenerate("Generating the list of AIC", "eAICs", PathCategory.eAIC);
                Web_SearchGenerate();

                if (isCoverAuthomatic == true) Web_AutoCoverGenerate();
                else Web_SemiAutoCoverGenerate(db);

                SendOutput("Website generate completed", Color.Green, 100);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void Web_SemiAutoCoverGenerate(eAIPContext db)
        {
            try
            {
                SendOutput("Generating eAIP Cover page");
                List<AIPPage> pages = GetPages(db);
                // If some pages not yet saved in the DB
                if (pages.Count < 2)
                {
                    var types = Enum.GetValues(typeof(DocType)).OfType<DocType>().Skip(1); // All types except for None
                    foreach (var type in types)
                    {
                        if (pages.All(x => x.DocType != type))
                        {
                            RichTextLib richTextLib = new RichTextLib(PageType.Cover, type, db);
                            richTextLib.SaveFromSource();
                        }
                    }
                    pages = GetPages(db);
                }

                foreach (var page in pages) // must be 2 elements here
                {
                    RichTextLib richTextLib = new RichTextLib(page.PageType, page.DocType, db);
                    string errorText = String.Empty;
                    richTextLib.Generate(page, out errorText);
                    if (!String.IsNullOrEmpty(errorText)) SendOutput(errorText);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private static void Web_AutoCoverGenerate()
        {
            try
            {
                SendOutput("Generating eAIP Cover page");
                ForCover forCover = new ForCover();

                CultureInfo ci = new CultureInfo("en-US");
                var effectiveDate = Lib.CurrentAIP.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant();
                var publicationDate = Lib.CurrentAIP.Publicationdate?.ToString("dd MMM yyyy", ci).ToUpperInvariant();

                forCover.EffectiveDate = effectiveDate;
                forCover.PublicationDate = publicationDate;
                forCover.Amendment = Lib.CurrentAIP.Amendment != null ? Lib.CurrentAIP.Amendment.Number + "/" + Lib.CurrentAIP.Amendment.Year : "";

                string coverOutput = Razor.Run(forCover);

                string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
                string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;

                var coverPath = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant() + "-cover-" + Lib.CurrentAIP.lang + ".html");

                File.WriteAllText(coverPath, coverOutput);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private static void GenerateHTML(string Arguments)
        {
            using (Process p = new Process())
            {
                try
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.FileName = Lib.MakeAIPFile;
                    p.StartInfo.WorkingDirectory = Lib.MakeAIPWorkingDir;

                    p.StartInfo.Arguments = Arguments;
                    p.StartInfo.CreateNoWindow = true;

                    List<string> output = new List<string>();
                    List<string> error = new List<string>();

                    p.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle?.Set();
                        }
                        else
                        {
                            output.Add(e.Data);
                        }
                    };
                    p.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle?.Set();
                        }
                        else
                        {
                            error.Add(e.Data);
                        }
                    };

                    p.Start();

                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    int timeoutSec = 10 * 1000;
                    if (p.WaitForExit(timeoutSec) &&
                        outputWaitHandle.WaitOne(timeoutSec) &&
                        errorWaitHandle.WaitOne(timeoutSec))
                    {
                        // Process completed. Check process.ExitCode here.
                    }
                    else
                    {
                        // Timed out.
                    }
                    p.Close();
                    if (output.Count > 0) SendOutput(output.ToSeparatedValues(Environment.NewLine));
                    if (error.Count > 0) SendOutput(error.ToSeparatedValues(Environment.NewLine), Color.Red, 0);
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowException("Error in the GenerateHTML", ex, true);
                }
            }
        }
        private static void SendOutput(string Message, Color? Color = null, int? Percent = null)
        {
            try
            {
                BaseLib.Struct.Output output = new BaseLib.Struct.Output();
                output.Message = Message;
                output.Color = Color ?? System.Drawing.Color.Black;
                output.Percent = Percent;
                OutputQueue.Enqueue(output);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void Web_DefaultGenerate(string message, string section, PathCategory PathCategory = PathCategory.eAIP)
        {
            try
            {
                SendOutput(message);
                string Arguments = section
                                          + @" -d " + "\"" + Lib.SourceDir + "\""
                                          + @" -t " + "\"" + Lib.TargetDir.WithCategory(PathCategory) + "\""
                                          + @" -l " + Lib.CurrentAIS.lang
                                          + @" -c " + Lib.CurrentAIS.ICAOcountrycode;


                Arguments = Lib.AdditionalParams(Arguments);

                GenerateHTML(Arguments);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void Web_SearchGenerate()
        {
            try
            {
                SendOutput("Generating search data");
                string CurrentDir = Lib.TargetDir.WithCategory(PathCategory.eAIP);

                 string SearchFile = Lib.TargetDir.Replace("{CAT}", $@"EV-records-{Lib.CurrentAIS.lang}.js");

                if (File.Exists(SearchFile))
                {
                    //clear
                    File.WriteAllText(SearchFile, string.Empty);
                }
                else
                {
                    File.Create(SearchFile).Dispose();
                }

                List<SectionName> lst = Lib.SectionByAttribute(SectionParameter.Generate);

                string allUsefulContent = "profiles = new Array(";

                int ident = 1;
                var doc = new HtmlAgilityPack.HtmlDocument()
                {
                    OptionOutputOriginalCase = true,
                    OptionCheckSyntax = false,
                    OptionOutputAsXml = false,
                    OptionFixNestedTags = true,
                    OptionWriteEmptyNodes = true,
                    OptionAutoCloseOnEnd = true
                };
                foreach (SectionName item in lst)
                {
                    string file = Path.Combine(CurrentDir, Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-" + Lib.AIPClassToSection(item.ToString()) + "-" + Lib.CurrentAIS.lang + @".html");

                    if (!File.Exists(file)) continue;

                    string usefulContent = "";
                    if (ident != 1)
                    {
                        usefulContent += "\" , ";
                    }
                    else
                    {
                        ident++;
                    }


                    //string contents = File.ReadAllText(file);

                    doc.LoadHtml(File.ReadAllText(file));

                    string title = doc?.DocumentNode?.SelectSingleNode("//head/title")?.InnerText;
                    var wordEnumerable = doc.DocumentNode?.SelectNodes("//body//text()")
                        ?.Select(x => x.InnerText);
                    string contents = wordEnumerable != null ? string.Join(" ", wordEnumerable) : String.Empty;

                    //Match m = Regex.Match(contents, @"<title>\s*(.+?)\s*</title>");
                    //string title = m.Groups[1].Value;

                    //Match b = Regex.Match(contents, @"</head>\s*(.+?)\s*</body>");
                    //contents = b.Groups[1].Value;

                    //contents = Regex.Replace(contents, "<.*?>", " ");
                    ////contents = Regex.Replace(contents, @"\t|\n|\r", " ");
                    //contents = Regex.Replace(contents, @"[^a-zA-Z0-9 .]+", String.Empty);

                    //var uniqueWords = new HashSet<string>();


                    usefulContent += "\"" + Lib.AIPClassToSection(item.ToString()).ToUpper() + "*" + title?.ToUpper() + " |";

                    if (contents != "")
                    {
                        contents = Regex.Replace(contents, @"[^\p{L}0-9 .]+", String.Empty);
                        var words = new HashSet<string>(contents.Split());
                        var uniqueWords = words.Distinct();
                        foreach (string word in uniqueWords)
                        {
                            usefulContent += word + " ";
                        }
                    }
                    usefulContent += "|file:eAIP/" + Path.GetFileName(file);


                    allUsefulContent += usefulContent;
                }


                allUsefulContent += "\")";
                File.AppendAllText(SearchFile, allUsefulContent);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static List<AIPPage> GetPages(eAIPContext db)
        {
            try
            {
                return db.AIPPage
                        .Where(x => x.eAIPID == Lib.CurrentAIP.id && x.PageType == PageType.Cover)
                        .Include(x => x.AIPPageData)
                        .ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }
}
