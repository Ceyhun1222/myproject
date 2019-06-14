using AIP.DB;
using AIP.DB.Entities;
using AIP.XML;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using Aran.Aim;
using NILReason = AIP.XML.NILReason;

namespace AIP.GUI.Classes
{
    internal static class DS2XML
    {

        private static readonly List<string> SectionWithSubsection = Lib
                                                    .SectionByAttribute(SectionParameter.GenerateSubsection)
                                                    .ToStringList();
        internal static eAIPContext db;
        /// <summary>
        /// Build XML file
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="file"></param>
        public static void Build<T>(eAIPContext db, string section, string file, DB.TemporalityEntity entity = null)
            where T : class, new()
        {
            try
            {
                SectionName sectionT = Lib.GetSectionName(section);
                if (entity != null)
                {
                    if (entity is DB.Supplement)
                    {
                        T eSUP = Build_eSUP<eSUP>(db, null, Lib.CurrentAIP, (DB.Supplement)entity);
                        AipXmlWrite(eSUP, file, new Dictionary<string, string>() { { "eAIP-locales", "../locales.xml" } });
                    }
                    else if (entity is DB.Circular)
                    {
                        T eAIC = Build_eAIC<eAIC>(db, null, Lib.CurrentAIP, (DB.Circular)entity);
                        AipXmlWrite(eAIC, file, new Dictionary<string, string>() { { "eAIP-locales", "../locales.xml" } });
                    }
                    return;
                }

                T XML_AIP = Convert<T>(db, section, Lib.CurrentAIP);

                //if (!Globals.IsAMDTPreview || Lib.PrevousAIP == null || section == "GEN02" || section == "GEN04" || section == "GEN06")
                if (!sectionT.HasParameterFlag(SectionParameter.AMDT) || Lib.PrevousAIP == null)
                {
                    AipXmlWrite(XML_AIP, file);
                }
                else
                {
                    AipXmlWrite(XML_AIP, file + ".2");
                    XML_AIP = DS2XML.Convert<T>(db, section, Lib.PrevousAIP);
                    if (XML_AIP == null)
                    {
                        if (File.Exists(file)) File.Delete(file);
                        File.Move(file + ".2", file);
                        return;
                    }

                    AipXmlWrite(XML_AIP, file + ".1");

                    
                    //Lib.CreateAmendment(Lib.CurrentAIP);

                    GroupType gtype = (section.StartsWith("G")) ? GroupType.GEN :
                                      (section.StartsWith("E")) ? GroupType.ENR :
                                      (section.StartsWith("A")) ? GroupType.AD :
                                                                  GroupType.Misc;

                    int? id = Lib.CurrentAIP?.Amendment?.Group?.Where(n => n.Type == gtype).FirstOrDefault()?.id;
                    string AMDT_GROUP_ID = (id != null) ? id.ToString() : "";

                    bool HaveChanges = Lib.GetComparedFile(file + ".1", file + ".2", section, AMDT_GROUP_ID);

                    /*
                    // If have changes and Amendment not yet created
                    if (Lib.CurrentAIP?.Amendment?.AmendmentStatus != AmendmentStatus.Available && HaveChanges)
                    {
                        DB.eAIP aip = db.eAIP.Where(n => n.id == Lib.CurrentAIP.id).Include("Amendment.Group.Description").FirstOrDefault();

                        // Get last previous AIP with Amdt
                        DB.eAIP PreviousAmdtAIP = db.eAIP.Where(n => n.Amendment != null && n.Effectivedate < aip.Effectivedate && n.lang == aip.lang)
                            .OrderByDescending(n => n.Effectivedate)
                            .Include("Amendment.Group.Description")
                            .FirstOrDefault();

                        aip.Amendment.AmendmentStatus = AmendmentStatus.Available;
                        if (PreviousAmdtAIP == null || PreviousAmdtAIP.Amendment == null || PreviousAmdtAIP.Effectivedate.Year != aip.Effectivedate.Year)
                        {
                            aip.Amendment.Number = "001";
                        }
                        else
                        {
                            int number = (PreviousAmdtAIP.Amendment.Number == "") ? 0 : System.Convert.ToInt32(PreviousAmdtAIP.Amendment.Number);
                            number++;
                            aip.Amendment.Number = number.ToString("D3");
                        }

                        db.Entry(aip.Amendment).State = EntityState.Modified;
                        db.SaveChanges();

                        Lib.CurrentAIP = aip;
                    }
                    */
                   

                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the Build", ex, true);
            }
        }

        /// <summary>
        /// Change AIP section name, ex. from ENR31 to ENR-3.1
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private static string GetSectionTextName(string section)
        {
            try
            {
                Match result = new Regex(@"([a-zA-Z]+)(\d+)").Match(section);
                string dotNum = string.Join(".", result?.Groups[2]?.Value?.ToString().ToCharArray());
                string sectionT = result?.Groups[1]?.Value + "-" + dotNum;
                return sectionT;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the GetSectionTextName", ex, true);
                return null;
            }
        }

        private static void ConvertClassToXml<T>(T ais, string xmlFileName, XmlSerializerNamespaces ns, Dictionary<string, string> instructionList = null)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                XmlDocument xmlDocument = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();

                XmlNode docNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDocument.AppendChild(docNode);


                using (MemoryStream stream = new MemoryStream())
                {
                    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    serializer.Serialize(stream, ais, ns);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    XmlDeclaration dec = null;
                    if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        dec = (XmlDeclaration)xmlDocument.FirstChild;
                        dec.Encoding = "UTF-8";
                    }
                    else
                    {
                        dec = xmlDocument.CreateXmlDeclaration("1.0", null, null);
                        dec.Encoding = "UTF-8";
                        xmlDocument.InsertBefore(dec, xmlDocument.DocumentElement);
                    }
                    if (instructionList != null)
                    {
                        foreach (KeyValuePair<string, string> instr in instructionList)
                        {
                            xmlDocument.InsertAfter(xmlDocument.CreateProcessingInstruction(instr.Key, instr.Value), dec);
                        }
                    }

                    xmlDocument.Save(xmlFileName);
                    stream.Close();

                    // temporary check
                    // must be improved
                    File.WriteAllText(xmlFileName, File.ReadAllText(xmlFileName)?.Replace("&gt;", ">").Replace("&lt;", "<"));
                    //

                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the ConvertClassToXML", ex, true);
            }
        }

        /// <summary>
        /// Convert Class to XML file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Class"></param>
        /// <param name="xmlFileName"></param>
        private static void AipXmlWrite<T>(T Class, string xmlFileName, Dictionary<string, string> instructionList = null)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("e", "http://www.eurocontrol.int/xmlns/AIM/eAIP");
                ns.Add("x", "http://www.w3.org/1999/xhtml");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");
                ConvertClassToXml(Class, xmlFileName, ns, instructionList);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the AIPXMLWrite", ex, true);
            }
        }

        /// <summary>
        /// Convert Dataset section into XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Convert<T>(eAIPContext db, string section, DB.eAIP eAIP) where T : class, new()
        {
            try
            {
                MethodInfo BuildSectionMethod;
                bool UseCommonSubsectionMethod = false;

                if (SectionWithSubsection.Contains(section))
                {
                    Type AIPType = Type.GetType("AIP.XML." + section + ",AIP.XML");
                    MethodInfo method = typeof(AIP.GUI.Classes.DS2XML).GetMethod($@"Build_SectionWithSubsection");
                    BuildSectionMethod = method?.MakeGenericMethod(AIPType);
                    UseCommonSubsectionMethod = true;
                }
                else if (section.Contains(".")) // AD2 or AD3
                {
                    string className = section.Contains("2.") ? "Aerodrome" : "Heliport";
                    Type AIPType = Type.GetType("AIP.XML." + className + ",AIP.XML");
                    MethodInfo method = typeof(AIP.GUI.Classes.DS2XML).GetMethod($@"Build_AirportHeliport");
                    BuildSectionMethod = method?.MakeGenericMethod(AIPType);
                    string icaoName = section.GetAfterOrEmpty();
                    DB.AirportHeliport airHel = db.AirportHeliport.AsNoTracking().FirstOrDefault(x => x.LocationIndicatorICAO == icaoName);
                    if (airHel == null) return null;

                    List<SectionName> sectionList = section.Contains("2.")
                        ? new List<SectionName>() { DB.SectionName.AD21, DB.SectionName.AD22, DB.SectionName.AD23, DB.SectionName.AD24, DB.SectionName.AD25, DB.SectionName.AD26, DB.SectionName.AD27, DB.SectionName.AD28, DB.SectionName.AD29, DB.SectionName.AD210, DB.SectionName.AD211, DB.SectionName.AD212, DB.SectionName.AD213, DB.SectionName.AD214, DB.SectionName.AD215, DB.SectionName.AD216, DB.SectionName.AD217, DB.SectionName.AD218, DB.SectionName.AD219, DB.SectionName.AD220, DB.SectionName.AD221, DB.SectionName.AD222, DB.SectionName.AD223, DB.SectionName.AD224 }
                        : new List<SectionName>() { DB.SectionName.AD31, DB.SectionName.AD32, DB.SectionName.AD33, DB.SectionName.AD34, DB.SectionName.AD35, DB.SectionName.AD36, DB.SectionName.AD37, DB.SectionName.AD38, DB.SectionName.AD39, DB.SectionName.AD310, DB.SectionName.AD311, DB.SectionName.AD312, DB.SectionName.AD313, DB.SectionName.AD314, DB.SectionName.AD315, DB.SectionName.AD316, DB.SectionName.AD317, DB.SectionName.AD318, DB.SectionName.AD319, DB.SectionName.AD320, DB.SectionName.AD321, DB.SectionName.AD322, DB.SectionName.AD323 };

                    List<DB.AIPSection> Dataset_AIP_List = db.AIPSection.OfType<DB.AIPSection>().AsNoTracking().Where(n => n.eAIPID == eAIP.id && n.SectionStatus == SectionStatusEnum.Filled && sectionList.Contains(n.SectionName) && n.AirportHeliportID == airHel.id).Include(n => n.Children).Include(n => n.AirportHeliport).OrderBy(x => x.SectionName).ToList();

                    return BuildSectionMethod?.Invoke(null, new object[] { Dataset_AIP_List, section, eAIP });
                }
                else
                {
                    BuildSectionMethod = typeof(AIP.GUI.Classes.DS2XML).GetMethod($@"Build_{section}");
                }

                if (BuildSectionMethod == null)
                {
                    ErrorLog.ShowMessage($@"No such method Build_{ section} found");
                    return null;
                }

                SectionName SectionName = Lib.GetSectionName(section);
                DB.AIPSection Dataset_AIP = db.AIPSection.OfType<AIPSection>().AsNoTracking().Where(n => n.eAIPID == eAIP.id && n.SectionStatus == SectionStatusEnum.Filled && n.SectionName == SectionName).Include(n => n.Children).FirstOrDefault();
                if (Dataset_AIP == null && SectionName.HasParameterFlag(SectionParameter.AIXM_Source)) return null;

                if (UseCommonSubsectionMethod)
                    return BuildSectionMethod.Invoke(null, new object[] { Dataset_AIP, section, eAIP });
                else if (!SectionName.HasParameterFlag(SectionParameter.AIXM_Source))
                    return BuildSectionMethod.Invoke(null, new object[] { db, Dataset_AIP, section, eAIP });
                else
                    return BuildSectionMethod.Invoke(null, new object[] { Dataset_AIP, section, eAIP });

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static T SetValue<T>(string value) where T : class, new()
        {
            try
            {
                if (value == null) return null;
                T newprop = new T();
                PropertyInfo propInfo = newprop.GetType().GetProperty("Items");
                propInfo.SetValue(newprop, new object[] { value });
                return newprop;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static object[] AddSubsectionText(string text, DB.eAIP eAIP, string section = null)
        {
            try
            {
                return new object[] { new div() { Items = new object[] {
                    Lib.ConvertToAIPXhtml(text, eAIP, section)
                } } };
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static XML.NIL Nil(XML.NILReason NILReason = XML.NILReason.Notavailable)
        {
            try
            {
                if (Properties.Settings.Default.NilSections)
                {
                    return
                        new XML.NIL()
                        {
                            Reason = NILReason,
                            @class = "Body"
                        };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static XML.NIL[] NilArray(XML.NILReason NILReason = XML.NILReason.Notavailable)
        {
            try
            {
                if (Properties.Settings.Default.NilSections)
                {
                    return new XML.NIL[]
                    {
                        Nil(NILReason)
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        #region Build Sections for preview        

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_SectionWithSubsection<T>(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP) where T : new()
        {
            try
            {
                dynamic XML_AIP = new T();
                var sections = Dataset_AIP?.Children.OrderBy(n => n.OrderNumber).ToList();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "PREFACE";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";

                if (!sections?.Any() == true)
                {
                    XML_AIP.NIL = NilArray();
                }
                else
                {
                    List<XML.Subsection> SubsectionList = new List<XML.Subsection>();
                    if (sections != null)
                        foreach (DB.Subsection item in sections)
                        {
                            XML.Subsection ent = new XML.Subsection();
                            //ent.Title = SetValue<XML.Title>(item.Title);
                            ent.Title = ProcessTitle(item.Title);
                            ent.Items = AddSubsectionText(item.Content, eAIP, section);
                            SubsectionList.Add(ent);
                        }
                    XML_AIP.Subsection = SubsectionList.ToArray();
                }

                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        private static XML.Title ProcessTitle(string XhtmlTitle)
        {
            try
            {
                if (XhtmlTitle?.StartsWith("<span") == true) // Numbering set
                {
                    Regex regexRule = new Regex("<span (.*)>(.*)</span>(.*)");
                    var regex = regexRule.Match(XhtmlTitle);
                    string number = regex.Groups[2].ToString();
                    XML.span spanValue = new XML.span {@class = "Numbering", Items = new object[]{ regex.Groups[2].ToString() } };
                    XML.Title title = new Title(){ Items = new object[] { spanValue, regex.Groups[3].ToString() } };
                    return title;
                }
                else
                    return SetValue<XML.Title>(XhtmlTitle);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static dynamic Build_AirportHeliport<T>(List<DB.AIPSection> Dataset_AIP_List, string section, DB.eAIP eAIP) where T : new()
        {
            try
            {
                AIPSection Dataset_AIP = null;
                dynamic XML_AIP = new T();
                bool IsAerodrome = section.StartsWith("AD2") ? true : false;
                //Aerodrome XML_AIP = new Aerodrome();

                DB.AirportHeliport airHel = Dataset_AIP_List?.FirstOrDefault()?.AirportHeliport;
                if (airHel == null) return null;

                XML_AIP.id = IsAerodrome ? section.Replace("AD2.", "AD-2.") : section.Replace("AD3.", "AD-3.");
                XML_AIP.Title = new Title();
                XML_AIP.Ref = $@"GEN24-{section.GetAfterOrEmpty()}";
                string title = airHel.LocationIndicatorICAO + " - " + airHel.Name;
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";

                if (IsAerodrome)
                {
                    AD21 adx1 = new AD21();
                    adx1.Title = SetValue<XML.Title>(Lib.GetText("AD21"));
                    adx1.Remarks = airHel.LocationIndicatorICAO + " - " + airHel.Name;
                    adx1.id = $@"{airHel.LocationIndicatorICAO}-AD-2.1";
                    XML_AIP.AD21 = adx1;
                }
                else
                {
                    AD31 adx1 = new AD31();
                    adx1.Title = SetValue<XML.Title>(Lib.GetText("AD31"));
                    adx1.Remarks = airHel.LocationIndicatorICAO + " - " + airHel.Name;
                    adx1.id = $@"{airHel.LocationIndicatorICAO}-AD-3.1";
                    XML_AIP.AD31 = adx1;
                }

                int maxNum = IsAerodrome ? 24 : 23;
                if (Lib.TempAdNum < maxNum) maxNum = Lib.TempAdNum; // temporary to test first X sections
                for (int i = 0; i <= maxNum; i++) // For AirportHeliport - each from AD21 to AD224, from AD31 to AD323
                {
                    GenAd(Dataset_AIP_List, IsAerodrome ? "AD2" + i : "AD3" + i, eAIP, airHel, ref XML_AIP);
                }

                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_eSUP<T>(eAIPContext db, string section, DB.eAIP eAIP, DB.Supplement sup) where T : new()
        {
            try
            {
                eSUP xmlSup = new eSUP();
                //xmlSup.Title = new Title(){Items = new object[]{ $@"AIP Supplement for REPUBLIC OF LATVIA {sup.Number}/{sup.Year}" }}; 
                //SetValue<XML.Title>($@"AIP Supplement for REPUBLIC OF LATVIA {sup.Number}/{sup.Year}");
                Guid oaGuid = db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier);
                string contactName = db.GetDBConfiguration<string>(Cfg.ContactName);

                OrganisationAuthority oa = Globals
                    .GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .OfType<OrganisationAuthority>()
                    .FirstOrDefault(x => x.Identifier == db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier));
                ContactInformation contact = oa?.Contact
                    .Where(y => y.Name == contactName)
                    .ToList()
                    .FirstOrDefault();
                if (contact != null)
                {
                    xmlSup.ICAOcountrycode = Lib.CurrentAIP.ICAOcountrycode;
                    xmlSup.State = Lib.CurrentAIP.State;
                    xmlSup.Effectivedate = sup.EffectivedateFrom.ToString("yyyy-MM-dd");
                    xmlSup.Publishingorganisation = Lib.CurrentAIP.Publishingorganisation;
                    xmlSup.Cancel = sup.EffectivedateTo?.ToString("yyyy-MM-dd");
                    xmlSup.Publicationdate = sup.Publicationdate.ToString("yyyy-MM-dd");
                    xmlSup.Number = sup.Number;
                    xmlSup.Year = sup.Year;
                    xmlSup.lang = Lib.CurrentAIP.lang;
                    xmlSup.Version = sup.Version.ToString();
                    xmlSup.Type = Lib.IsAIRAC(sup.EffectivedateFrom) ? eSUPType.AIRAC : eSUPType.NonAIRAC;
                    xmlSup.@class = "Body";

                    int cnt = 0;
                    xmlSup.Address = new Address[]
                    {
                        new Address()
                        {
                            Addresspart = new Addresspart[]
                            {
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Phone,
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Voice }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Fax, @class = "Body",
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Facsimile }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Email, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x=>x.Network == CodeTelecomNetwork.INTERNET)?.eMail }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.AFS, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.AFTN)?.Linkage }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.URL, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.INTERNET)?.Linkage }}
                            }
                        },
                        new Address()
                            {
                                Addresspart = new Addresspart[]
                                {
                                    new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Post,
                                        Items = new object[]
                                        {
                                            oa.Name, new br(),
                                            contact.Name, new br(),
                                            contact.Address.FirstOrDefault()?.DeliveryPoint + ", " +
                                            contact.Address.FirstOrDefault()?.City,
                                            contact.Address.FirstOrDefault()?.AdministrativeArea + ", " +
                                            contact.Address.FirstOrDefault()?.PostalCode + ", " +
                                            contact.Address.FirstOrDefault()?.Country
                                        }}
                                }
                            }
                    };

                    //xmlSup.Title = new Title() { Items = new object[] { sup.Title } };
                    xmlSup.SUPsection = new SUPsection[]
                    {
                            new SUPsection()
                            {
                                Title = new Title(){Items = new object[]{sup.Title}},
                                Items = new object[] { new div() { Items = new object[] {
                                    Lib.ConvertToAIPXhtml(XML.Parser.Export(sup.Description), Lib.CurrentAIP)
                                    }
                                    }
                                }

                            }
                    };
                }


                return xmlSup;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_eAIC<T>(eAIPContext db, string section, DB.eAIP eAIP, DB.Circular sup) where T : new()
        {
            try
            {
                eAIC xmlSup = new eAIC();
                //xmlSup.Title = new Title() { Items = new object[] { $@"AIC for REPUBLIC OF LATVIA {sup.Series} {sup.Number}/{sup.Year}" } };
                //SetValue<XML.Title>($@"AIP Supplement for REPUBLIC OF LATVIA {sup.Number}/{sup.Year}");
                Guid oaGuid = db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier);
                string contactName = db.GetDBConfiguration<string>(Cfg.ContactName);

                OrganisationAuthority oa = Globals
                    .GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .OfType<OrganisationAuthority>()
                    .FirstOrDefault(x => x.Identifier == db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier));
                ContactInformation contact = oa?.Contact
                    .Where(y => y.Name == contactName)
                    .ToList()
                    .FirstOrDefault();
                if (contact != null)
                {
                    xmlSup.ICAOcountrycode = Lib.CurrentAIP.ICAOcountrycode;
                    xmlSup.State = Lib.CurrentAIP.State;
                    xmlSup.Effectivedate = sup.EffectivedateFrom.ToString("yyyy-MM-dd");
                    xmlSup.Publishingorganisation = Lib.CurrentAIP.Publishingorganisation;
                    xmlSup.Cancel = sup.EffectivedateTo?.ToString("yyyy-MM-dd");
                    xmlSup.Publicationdate = sup.Publicationdate.ToString("yyyy-MM-dd");
                    xmlSup.Number = sup.Number;
                    xmlSup.Year = sup.Year;
                    xmlSup.lang = Lib.CurrentAIP.lang;
                    xmlSup.Version = sup.Version.ToString();
                    xmlSup.Series = sup.Series;
                    xmlSup.@class = "Body";

                    int cnt = 0;
                    xmlSup.Address = new Address[]
                    {
                        new Address()
                        {
                            Addresspart = new Addresspart[]
                            {
                                new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.Phone,
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Voice }},
                                new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.Fax, @class = "Body",
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Facsimile }},
                                new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.Email, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x=>x.Network == CodeTelecomNetwork.INTERNET)?.eMail }},
                                new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.AFS, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.AFTN)?.Linkage }},
                                new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.URL, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.INTERNET)?.Linkage }}
                            }
                        },
                        new Address()
                            {
                                Addresspart = new Addresspart[]
                                {
                                    new Addresspart(){ id = $@"eAIC{sup.id}{cnt++}", Type = AddresspartType.Post,
                                        Items = new object[]
                                        {
                                            oa.Name, new br(),
                                            contact.Name, new br(),
                                            contact.Address.FirstOrDefault()?.DeliveryPoint + ", " +
                                            contact.Address.FirstOrDefault()?.City,
                                            contact.Address.FirstOrDefault()?.AdministrativeArea + ", " +
                                            contact.Address.FirstOrDefault()?.PostalCode + ", " +
                                            contact.Address.FirstOrDefault()?.Country
                                        }

                                    }
                                }
                            }
                    };

                    //xmlSup.Title = new Title() { Items = new object[] { sup.Title } };
                    xmlSup.Subsection = new XML.Subsection[]
                    {
                            new XML.Subsection()
                            {
                                Title = new Title(){Items = new object[]{sup.Title}},
                                Items = new object[] { new div() { Items = new object[] {
                                    Lib.ConvertToAIPXhtml(XML.Parser.Export(sup.Description), Lib.CurrentAIP)
                                    }
                                    }
                                }

                            }
                    };
                }

                return xmlSup;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }
        public static void GenAd<T>(List<DB.AIPSection> Dataset_AIP_List, string section, DB.eAIP eAIP, DB.AirportHeliport airHel, ref T XML_AIP)
        {
            try
            {
                SectionName sn = Lib.GetSectionName(section);
                AIPSection datasetAip = Dataset_AIP_List.FirstOrDefault(x => x.SectionName == sn);
                if (datasetAip != null)
                {
                    Type type = Type.GetType("AIP.XML." + section + ",AIP.XML");
                    if (type != null)
                    {
                        object ah = Activator.CreateInstance(type);
                        PropertyInfo prop = type.GetProperty("Title");
                        prop?.SetValue(ah, SetValue<XML.Title>(Lib.GetText(section).ToUpperInvariant()), null);
                        prop = type.GetProperty("id");
                        string idSec = section.Replace("AD2", "AD-2.").Replace("AD3", "AD-3.");
                        string idVal = $@"{airHel.LocationIndicatorICAO}-{idSec}";
                        prop?.SetValue(ah, idVal, null);

                        List<XML.Subsection> subsectionList = new List<XML.Subsection>();
                        var subList = datasetAip.Children.OfType<DB.Subsection>().OrderBy(n => n.OrderNumber).ToList();
                        if (subList.Count > 0)
                        {
                            foreach (DB.Subsection item in subList)
                            {
                                XML.Subsection ent = new XML.Subsection();
                                ent.Title = SetValue<XML.Title>(item.Title);
                                if (item.Content != null)
                                    ent.Items = AddSubsectionText(item.Content, eAIP, section);
                                subsectionList.Add(ent);
                            }
                            prop = type.GetProperty("Subsection");
                            prop?.SetValue(ah, subsectionList.ToArray(), null);
                        }
                        else if (section != "AD21" && section != "AD31")
                        {
                            prop = type.GetProperty("NIL");
                            prop?.SetValue(ah, NilArray(), null);
                            return; // remove it to show section in the AD 0.6
                        }
                        

                        XML_AIP?
                            .GetType()
                            .GetProperties()?
                            .Where(x => x.Name == section)
                            .FirstOrDefault()?
                            .SetValue(XML_AIP, ah);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>

        public static dynamic Build_GEN02(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.GEN02 XML_AIP = new XML.GEN02();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "RECORD OF AIP AMENDMENTS";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                //db.Database.Log = Console.WriteLine;
                var AIP_List = db.eAIP.AsNoTracking().Where(n => n.Amendment != null
                && n.Amendment.AmendmentStatus == AmendmentStatus.Available
                && n.lang == eAIP.lang
                && n.Effectivedate <= eAIP.Effectivedate)
                .Include(n => n.Amendment.Group)
                .OrderBy(n => n.Effectivedate).ToList();
                List<XML.Amendment> amdt_list = new List<XML.Amendment>();
                if (AIP_List.Count == 0)
                {
                    XML_AIP.Items = new object[] { new NIL() { Reason = XML.NILReason.Notavailable } };
                }
                else
                {
                    var last = AIP_List.Last();
                    foreach (DB.eAIP caip in AIP_List)
                    {
                        XML.Amendment amdt = new XML.Amendment();
                        //if (caip.id == eAIP.id) // full list
                        //{
                            amdt.id = $@"DBAUTOAMDTID{caip.Amendment.id}";
                            amdt.Type = (caip.Amendment.Type == DB.AmendmentType.AIRAC) ? XML.AmendmentType.AIRAC : XML.AmendmentType.NonAIRAC;
                            amdt.Number = caip.Amendment.Number;
                            amdt.Year = caip.Amendment.Year;
                            amdt.Publicationdate = caip.Publicationdate?.ToString("yyyy-MM-dd");
                            amdt.Effectivedate = caip.Effectivedate.ToString("yyyy-MM-dd");
                            if (caip.Equals(last)){
                                amdt.Inserted = true;
                            }
                            List<XML.Group> grp_list = new List<XML.Group>();
                            XML.Group grp = new XML.Group();
                            //grp.id = $@"DBAUTOGROUPID{caip.Amendment.Group?.Where(n => n.Type == GroupType.Misc)?.FirstOrDefault().id}";
                            //grp.title = "Miscellaneous";
                            //grp.Description = new XML.Description()
                            //{
                            //    Title = new Title() { Updated = "No", Items = new object[] { "Miscellaneous" } },
                            //    Abstract = new Abstract() { id = "" }
                            //};
                            //grp_list.Add(grp);

                            grp = new XML.Group();
                            grp.id = $@"DBAUTOGROUPID{caip.Amendment.Group?.Where(n => n.Type == GroupType.GEN)?.FirstOrDefault().id}";
                            grp.title = "GEN";
                            grp.Description = new XML.Description()
                            {
                                Title = new Title() { Updated = "No", Items = new object[] { "GEN" } },
                                Abstract = new Abstract() { id = "" }
                            };
                            grp_list.Add(grp);

                            grp = new XML.Group();
                            grp.id = $@"DBAUTOGROUPID{caip.Amendment.Group?.Where(n => n.Type == GroupType.ENR)?.FirstOrDefault().id}";
                            grp.title = "ENR";
                            grp.Description = new XML.Description()
                            {
                                Title = new Title() { Updated = "No", Items = new object[] { "ENR" } },
                                Abstract = new Abstract() { id = "" }
                            };
                            grp_list.Add(grp);

                            grp = new XML.Group();
                            grp.id = $@"DBAUTOGROUPID{caip.Amendment.Group?.Where(n => n.Type == GroupType.AD)?.FirstOrDefault().id}";
                            grp.title = "AD";
                            grp.Description = new XML.Description()
                            {
                                Title = new Title() { Updated = "No", Items = new object[] { "AD" } },
                                Abstract = new Abstract() { id = "" }
                            };
                            grp_list.Add(grp);

                            amdt.Group = grp_list.ToArray();
                        //}
                        //else
                        //{
                        //    amdt.id = $@"DBAUTOAMDTID{caip.Amendment.id}";
                        //    amdt.Type = (caip.Amendment.Type == DB.AmendmentType.AIRAC) ? XML.AmendmentType.AIRAC : XML.AmendmentType.NonAIRAC;
                        //    amdt.Number = caip.Amendment.Number;
                        //    amdt.Year = caip.Amendment.Year;
                        //    amdt.Publicationdate = caip.Publicationdate?.ToString("yyyy-MM-dd");
                        //    amdt.Effectivedate = caip.Effectivedate.ToString("yyyy-MM-dd");
                        //}
                        amdt_list.Add(amdt);
                    }

                    XML_AIP.Items = amdt_list.ToArray();
                }
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_GEN03(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.GEN03 XML_AIP = new XML.GEN03();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "RECORD OF AIP SUPPLEMENTS";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                //db.Database.Log = Console.WriteLine;
                int langId = Lib.GetLangIdByValue(eAIP.lang) ?? 0;
                var queryS = db.Supplement
                    .Where(x => x.IsCanceled == false && x.LanguageReferenceId == langId)?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();
                List<XML.Supplement> sup_list = new List<XML.Supplement>();
                if (queryS.Count == 0)
                {
                    XML_AIP.Items = new object[] { new NIL() { Reason = XML.NILReason.Notavailable } };
                }
                else
                {

                    foreach (var sup in queryS)
                    {
                        XML.Supplement xmlSup = new XML.Supplement();
                        xmlSup.id = $@"SUP-{sup.id}";
                        xmlSup.Year = sup.Year;
                        xmlSup.Number = sup.Number;
                        xmlSup.Effectivedate = sup.EffectivedateFrom.ToString("yyyy-MM-dd");
                        xmlSup.Cancel = sup.EffectivedateTo?.ToString("yyyy-MM-dd");
                        xmlSup.Title = new Title() { Items = new[] { sup.Title } };
                        xmlSup.Affects = new Affects() { Items = new[] { sup.Affects } };
                        xmlSup.href = $@"../eSUP/EV-eSUP-{sup.Year}-{sup.Number}-{eAIP.lang}.xml";
                        sup_list.Add(xmlSup);
                    }

                    XML_AIP.Items = sup_list.ToArray();
                }
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_GEN04(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.GEN04 XML_AIP = new XML.GEN04();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "CHECKLIST OF AIP PAGES";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new NIL() { Reason = NILReason.Electronic };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_GEN06(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.GEN06 XML_AIP = new XML.GEN06();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "TABLE OF CONTENTS TO PART 1";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Generated = new Generated();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_ENR01(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR01 XML_AIP = new XML.ENR01();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new[] {new NIL() {Reason = NILReason.Notapplicable}};
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_ENR02(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR02 XML_AIP = new XML.ENR02();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Items = new[] { new NIL() { Reason = NILReason.Notapplicable } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_ENR03(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR03 XML_AIP = new XML.ENR03();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Items = new[] { new NIL() { Reason = NILReason.Notapplicable } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_ENR04(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR04 XML_AIP = new XML.ENR04();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new NIL() { Reason = NILReason.Notapplicable };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_ENR05(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR05 XML_AIP = new XML.ENR05();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new NIL() { Reason = NILReason.Notapplicable };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_ENR06(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR06 XML_AIP = new XML.ENR06();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "TABLE OF CONTENTS TO PART 2";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Generated = new Generated();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_AD06(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD06 XML_AIP = new XML.AD06();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "TABLE OF CONTENTS TO PART 3";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Generated = new Generated();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_GEN22(DB.AIPSection Dataset_AIP, string section, DB.eAIP caip)
        {
            try
            {
                XML.GEN22 XML_AIP = new XML.GEN22();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "ABBREVIATIONS USED IN AIS PUBLICATIONS";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";

                List<XML.div> DivList = new List<XML.div>();
                if (Dataset_AIP != null)
                    foreach (DB.Subsection item in Dataset_AIP.Children.OrderBy(n => n.OrderNumber).ToList())
                    {
                        XML.div ent = new XML.div();
                        ent.Items = AddSubsectionText(item.Content, caip, section);
                        DivList.Add(ent);
                    }


                int langId = Lib.GetLangIdByValue(caip.lang) ?? 0;
                if (langId == 0) return null;
                List<DB.Abbreviation> abbrevList = new List<DB.Abbreviation>();

                using (eAIPContext db = new eAIPContext())
                {
                    abbrevList = db.Abbreviation
                    .AsNoTracking()
                    .Where(x => x.IsCanceled != true
                                && (x.LanguageReferenceId == null || x.LanguageReferenceId == langId)
                                && (caip.Effectivedate >= x.EffectivedateFrom && (x.EffectivedateTo == null || caip.Effectivedate <= x.EffectivedateTo))
                    )
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .OrderBy(x => x.Ident)
                    .ThenBy(x => x.Identifier)
                    .ToList();

                    if (abbrevList.Any())
                    {
                        List<XML.Abbreviationdescription> XML_Abbrev_List = new List<Abbreviationdescription>();
                        foreach (DB.Abbreviation abbrev in abbrevList)
                        {
                            XML_Abbrev_List.Add(
                                new Abbreviationdescription
                                {
                                    Abbreviationdetails = new Abbreviationdetails
                                    {
                                        Items = new object[]
                                    {
                                        $@"<span>{abbrev.Details}</span>"
                                    }
                                    }
                                    ,
                                    Abbreviationident = new Abbreviationident
                                    {
                                        Text = new string[]
                                        {
                                            abbrev.Ident
                                        }
                                    },
                                    id = abbrev.IdKey
                                }
                                );
                        }
                        XML_AIP.Abbreviationdescription = XML_Abbrev_List.ToArray();
                    }
                }

                XML_AIP.Items = DivList.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_GEN24(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.GEN24 XML_AIP = new XML.GEN24();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "LOCATION INDICATORS";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML.Locationtable lt = new XML.Locationtable();
                List<XML.Locationdefinition> ldList = new List<XML.Locationdefinition>();

                if (Dataset_AIP != null)
                    foreach (DB.LocationTable item in Dataset_AIP?.Children?.OfType<LocationTable>())
                    {
                        int cnt = 0;
                        if (item.LocationDefinition.Count == 0) // error
                        {
                            break;
                        }
                        //XML_AIP.Locationtable = new List<Locationtable>().ToArray();
                        foreach (DB.LocationDefinition ld in item.LocationDefinition)
                        {
                            XML.Locationdefinition aipLd = new Locationdefinition();
                            aipLd.AFS = (ld.LocationDefinitionAFS == YesNo.Yes)
                                ? LocationdefinitionAFS.Yes
                                : LocationdefinitionAFS.No;
                            aipLd.Locationident = new Locationident() {Items = new object[] {ld.LocationIdent}};
                            aipLd.id = $@"GEN24-{ld.LocationIdent}";
                            aipLd.Type = ld.LocationDefinitionType == LocationDefinitionType.ICAO
                                ? LocationdefinitionType.ICAO
                                : LocationdefinitionType.Nonstandard;
                            aipLd.Locationname = new Locationname() {Items = new object[] {ld.LocationName}};
                            ldList.Add(aipLd);
                        }
                    }
                lt.Locationdefinition = ldList.ToArray();
                XML_AIP.Locationtable = new Locationtable[] { lt };

                XML_AIP.Items = new[] { new p() { Items = new object[] { "Location indicators marked with an asterisk (*) cannot be used in the address component for AFS messages." } } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR31(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR31 XML_AIP = new XML.ENR31();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Number = Lib.AIPClassToSection(section);
                XML_AIP.Toc = ENR31Toc.Yes;
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "Lower ATS Routes";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                List<object> rt_items = new List<object>();
                AIP.XML.Route rt;
                AIP.XML.Significantpointreference spr;
                AIP.XML.Routesegment rs;
                List<XML.Route> lst = new List<XML.Route>();
                var rtList = Dataset_AIP?.Children.OrderBy(n => n.id).ToList();
                if (rtList.Count > 0)
                {
                    foreach (DB.Route route in rtList)
                    {
                        rt_items = new List<object>();
                        rt = new AIP.XML.Route();
                        rt.id = $@"RTE-{route.Routedesignator}";
                        rt.Updated = RouteUpdated.No;
                        rt.Routedesignator = new Routedesignator() { Items = new string[] { route.Routedesignator } };
                        rt.RouteRNP = new RouteRNP()
                        {
                            Items = new string[] { route.RouteRNP },
                            Updated = RouteRNPUpdated.No
                        };
                        rt.Routesegmentusage = new Routesegmentusage[]
                        {
                            new Routesegmentusage
                            {
                                id = "RSU-" + route.Routedesignator + "-1",
                                Updated = RoutesegmentusageUpdated.No,
                                Items = new string[] {route.Routesegmentusage}
                            }
                        };


                        foreach (DB.SubClass item in route.Children.OrderBy(n => n.OrderNumber).ToList())
                        {
                            if (item is DB.Significantpointreference)
                            {
                                spr = new XML.Significantpointreference();
                                spr.Ref = ((DB.Significantpointreference)item).Ref;
                                spr.Updated = SignificantpointreferenceUpdated.No;
                                spr.SignificantpointATC = new XML.SignificantpointATC
                                {
                                    Items = new string[] { ((DB.Significantpointreference)item).SignificantpointATC }
                                };
                                rt_items.Add(spr);
                            }
                            else if (item is DB.Routesegment)
                            {
                                rs = new XML.Routesegment();
                                rs.Updated = RoutesegmentUpdated.No;
                                rs.Routesegmentmagtrack = new Routesegmentmagtrack
                                {
                                    Updated = RoutesegmentmagtrackUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentmagtrack }
                                };
                                rs.Routesegmentreversemagtrack = new Routesegmentreversemagtrack
                                {
                                    Updated = RoutesegmentreversemagtrackUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentreversemagtrack }
                                };
                                rs.Routesegmentlength = new Routesegmentlength
                                {
                                    Updated = RoutesegmentlengthUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentlength }
                                };

                                rs.Routesegmentupper = new Routesegmentupper
                                {
                                    Updated = RoutesegmentupperUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentupper }
                                };
                                rs.Routesegmentlower = new Routesegmentlower
                                {
                                    Updated = RoutesegmentlowerUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentlower }
                                };
                                rs.Routesegmentminimum = new Routesegmentminimum
                                {
                                    Updated = RoutesegmentminimumUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentminimum }
                                };
                                rs.Routesegmentwidth = new Routesegmentwidth
                                {
                                    Updated = RoutesegmentwidthUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentwidth }
                                };
                                rs.RoutesegmentATC = new RoutesegmentATC
                                {
                                    Updated = RoutesegmentATCUpdated.No,
                                    Items = new string[] { (item as DB.Routesegment).RoutesegmentATC }
                                };
                                List<XML.Routesegmentusagereference> tmp = new List<XML.Routesegmentusagereference>();
                                if ((item as DB.Routesegment).Routesegmentusagereference != null)
                                {
                                    foreach (DB.Routesegmentusagereference rsur in (item as DB.Routesegment)
                                        .Routesegmentusagereference)
                                    {
                                        tmp.Add(
                                            new XML.Routesegmentusagereference
                                            {
                                                Ref = rsur.Ref,
                                                Updated = RoutesegmentusagereferenceUpdated.No,
                                                Routesegmentusageleveltype = new Routesegmentusageleveltype
                                                {
                                                    Items = new string[] { rsur.Routesegmentusageleveltype }
                                                },
                                                Routesegmentusagedirection = new Routesegmentusagedirection
                                                {
                                                    Items = new string[] { rsur.Routesegmentusagedirection }
                                                }
                                            }
                                        );
                                    }
                                }

                                rs.Routesegmentusagereference = tmp.ToArray();
                                rs.Routesegmentairspaceclass = new Routesegmentairspaceclass()
                                {
                                    Items = new string[] { (item as DB.Routesegment).Routesegmentairspaceclass }
                                };
                                rt_items.Add(rs);
                            }
                        }
                        // Temporary fix to list SP and RS, need to find something better
                        if (rt_items.Count > 0)
                        {

                        }
                        rt.Items = rt_items.ToArray();

                        lst.Add(rt);
                    }
                }
                else
                {
                    XML_AIP.NIL = NilArray();
                }
                XML_AIP.Items = lst.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR32(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR32 XML_AIP = new XML.ENR32();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Number = Lib.AIPClassToSection(section);
                XML_AIP.Toc = ENR32Toc.Yes;
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "Upper ATS Routes";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                List<object> rt_items = new List<object>();
                AIP.XML.Route rt;
                AIP.XML.Significantpointreference spr;
                AIP.XML.Routesegment rs;
                List<XML.Route> lst = new List<XML.Route>();
                var rtList = Dataset_AIP?.Children.OrderBy(n => n.id).ToList();
                if (rtList.Count > 0)
                {
                    foreach (DB.Route route in rtList)
                    {
                        rt_items = new List<object>();
                        rt = new AIP.XML.Route();
                        rt.id = $@"RTE-{route.Routedesignator}";
                        rt.Updated = RouteUpdated.No;
                        rt.Routedesignator = new Routedesignator() { Items = new string[] { route.Routedesignator } };
                        rt.RouteRNP = new RouteRNP() { Items = new string[] { route.RouteRNP }, Updated = RouteRNPUpdated.No };
                        rt.Routesegmentusage = new Routesegmentusage[] { new Routesegmentusage { id = "RSU-" + route.Routedesignator + "-1", Updated = RoutesegmentusageUpdated.No, Items = new string[] { route.Routesegmentusage } } };


                        foreach (DB.SubClass item in route.Children.OrderBy(n => n.OrderNumber).ToList())
                        {
                            if (item is DB.Significantpointreference)
                            {
                                spr = new XML.Significantpointreference();
                                spr.Ref = ((DB.Significantpointreference)item).Ref;
                                spr.Updated = SignificantpointreferenceUpdated.No;
                                spr.SignificantpointATC = new XML.SignificantpointATC { Items = new string[] { ((DB.Significantpointreference)item).SignificantpointATC } };
                                rt_items.Add(spr);
                            }
                            else if (item is DB.Routesegment)
                            {
                                rs = new XML.Routesegment();
                                rs.Updated = RoutesegmentUpdated.No;
                                rs.Routesegmentmagtrack = new Routesegmentmagtrack { Updated = RoutesegmentmagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentmagtrack } };
                                rs.Routesegmentreversemagtrack = new Routesegmentreversemagtrack { Updated = RoutesegmentreversemagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentreversemagtrack } };
                                rs.Routesegmentlength = new Routesegmentlength { Updated = RoutesegmentlengthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlength } };

                                rs.Routesegmentupper = new Routesegmentupper { Updated = RoutesegmentupperUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentupper } };
                                rs.Routesegmentlower = new Routesegmentlower { Updated = RoutesegmentlowerUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlower } };
                                rs.Routesegmentminimum = new Routesegmentminimum { Updated = RoutesegmentminimumUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentminimum } };
                                rs.Routesegmentwidth = new Routesegmentwidth { Updated = RoutesegmentwidthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentwidth } };
                                rs.RoutesegmentATC = new RoutesegmentATC { Updated = RoutesegmentATCUpdated.No, Items = new string[] { (item as DB.Routesegment).RoutesegmentATC } };
                                List<XML.Routesegmentusagereference> tmp = new List<XML.Routesegmentusagereference>();
                                if ((item as DB.Routesegment).Routesegmentusagereference != null)
                                {
                                    foreach (DB.Routesegmentusagereference rsur in (item as DB.Routesegment).Routesegmentusagereference)
                                    {
                                        tmp.Add(
                                            new XML.Routesegmentusagereference
                                            {
                                                Ref = rsur.Ref,
                                                Updated = RoutesegmentusagereferenceUpdated.No,
                                                Routesegmentusageleveltype = new Routesegmentusageleveltype
                                                {
                                                    Items = new string[] { rsur.Routesegmentusageleveltype }
                                                },
                                                Routesegmentusagedirection = new Routesegmentusagedirection
                                                {
                                                    Items = new string[] { rsur.Routesegmentusagedirection }
                                                }
                                            }
                                            );
                                    }
                                }

                                rs.Routesegmentusagereference = tmp.ToArray();
                                rs.Routesegmentairspaceclass = new Routesegmentairspaceclass() { Items = new string[] { (item as DB.Routesegment).Routesegmentairspaceclass } };
                                rt_items.Add(rs);
                            }
                        }
                        // Temporary fix to list SP and RS, need to find something better
                        if (rt_items.Count > 0)
                        {

                        }
                        rt.Items = rt_items.ToArray();

                        lst.Add(rt);
                    }
                }
                else
                {
                    XML_AIP.NIL = NilArray();
                }
                XML_AIP.Items = lst.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR33(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR33 XML_AIP = new XML.ENR33();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Number = Lib.AIPClassToSection(section);
                XML_AIP.Toc = ENR33Toc.Yes;
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "AREA NAVIGATION  ROUTES ";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                List<object> rt_items = new List<object>();
                AIP.XML.Route rt;
                AIP.XML.Significantpointreference spr;
                AIP.XML.Routesegment rs;
                List<XML.Route> lst = new List<XML.Route>();
                var rtList = Dataset_AIP?.Children.OrderBy(n => n.id).ToList();
                if (rtList.Count > 0)
                {
                    foreach (DB.Route route in rtList)
                    {
                        rt_items = new List<object>();
                        rt = new AIP.XML.Route();
                        rt.id = $@"RTE-{route.Routedesignator}";
                        rt.Updated = RouteUpdated.No;
                        rt.Routedesignator = new Routedesignator() { Items = new string[] { route.Routedesignator } };
                        rt.RouteRNP = new RouteRNP() { Items = new string[] { route.RouteRNP }, Updated = RouteRNPUpdated.No };
                        rt.Routesegmentusage = new Routesegmentusage[] { new Routesegmentusage { id = "RSU-" + route.Routedesignator + "-1", Updated = RoutesegmentusageUpdated.No, Items = new string[] { route.Routesegmentusage } } };


                        foreach (DB.SubClass item in route.Children.OrderBy(n => n.OrderNumber).ToList())
                        {
                            if (item is DB.Significantpointreference)
                            {
                                spr = new XML.Significantpointreference();
                                spr.Ref = ((DB.Significantpointreference)item).Ref;
                                spr.Updated = SignificantpointreferenceUpdated.No;
                                spr.SignificantpointATC = new XML.SignificantpointATC { Items = new string[] { ((DB.Significantpointreference)item).SignificantpointATC } };
                                rt_items.Add(spr);
                            }
                            else if (item is DB.Routesegment)
                            {
                                rs = new XML.Routesegment();
                                rs.Updated = RoutesegmentUpdated.No;
                                rs.Routesegmentmagtrack = new Routesegmentmagtrack { Updated = RoutesegmentmagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentmagtrack } };
                                rs.Routesegmentreversemagtrack = new Routesegmentreversemagtrack { Updated = RoutesegmentreversemagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentreversemagtrack } };
                                rs.Routesegmentlength = new Routesegmentlength { Updated = RoutesegmentlengthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlength } };

                                rs.Routesegmentupper = new Routesegmentupper { Updated = RoutesegmentupperUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentupper } };
                                rs.Routesegmentlower = new Routesegmentlower { Updated = RoutesegmentlowerUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlower } };
                                rs.Routesegmentminimum = new Routesegmentminimum { Updated = RoutesegmentminimumUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentminimum } };
                                rs.Routesegmentwidth = new Routesegmentwidth { Updated = RoutesegmentwidthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentwidth } };
                                rs.RoutesegmentATC = new RoutesegmentATC { Updated = RoutesegmentATCUpdated.No, Items = new string[] { (item as DB.Routesegment).RoutesegmentATC } };
                                List<XML.Routesegmentusagereference> tmp = new List<XML.Routesegmentusagereference>();
                                if ((item as DB.Routesegment).Routesegmentusagereference != null)
                                {
                                    foreach (DB.Routesegmentusagereference rsur in (item as DB.Routesegment).Routesegmentusagereference)
                                    {
                                        tmp.Add(
                                            new XML.Routesegmentusagereference
                                            {
                                                Ref = rsur.Ref,
                                                Updated = RoutesegmentusagereferenceUpdated.No,
                                                Routesegmentusageleveltype = new Routesegmentusageleveltype
                                                {
                                                    Items = new string[] { rsur.Routesegmentusageleveltype }
                                                },
                                                Routesegmentusagedirection = new Routesegmentusagedirection
                                                {
                                                    Items = new string[] { rsur.Routesegmentusagedirection }
                                                }
                                            }
                                            );
                                    }
                                }

                                rs.Routesegmentusagereference = tmp.ToArray();
                                rs.Routesegmentairspaceclass = new Routesegmentairspaceclass() { Items = new string[] { (item as DB.Routesegment).Routesegmentairspaceclass } };
                                rt_items.Add(rs);
                            }
                        }
                        // Temporary fix to list SP and RS, need to find something better
                        if (rt_items.Count > 0)
                        {

                        }
                        rt.Items = rt_items.ToArray();

                        lst.Add(rt);
                    }
                }
                else
                {
                    XML_AIP.NIL = NilArray();
                }
                XML_AIP.Items = lst.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_ENR34(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR34 XML_AIP = new XML.ENR34();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Number = Lib.AIPClassToSection(section);
                XML_AIP.Toc = ENR34Toc.Yes;
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "HELICOPTER ROUTES ";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                List<object> rt_items = new List<object>();
                AIP.XML.Route rt;
                AIP.XML.Significantpointreference spr;
                AIP.XML.Routesegment rs;
                List<XML.Route> lst = new List<XML.Route>();
                var rtList = Dataset_AIP?.Children.OrderBy(n => n.id).ToList();
                if (rtList.Count > 0)
                {
                    foreach (DB.Route route in rtList)
                    {
                        rt_items = new List<object>();
                        rt = new AIP.XML.Route();
                        rt.id = $@"RTE-{route.Routedesignator}";
                        rt.Updated = RouteUpdated.No;
                        rt.Routedesignator = new Routedesignator() { Items = new string[] { route.Routedesignator } };
                        rt.RouteRNP = new RouteRNP() { Items = new string[] { route.RouteRNP }, Updated = RouteRNPUpdated.No };
                        rt.Routesegmentusage = new Routesegmentusage[] { new Routesegmentusage { id = "RSU-" + route.Routedesignator + "-1", Updated = RoutesegmentusageUpdated.No, Items = new string[] { route.Routesegmentusage } } };


                        foreach (DB.SubClass item in route.Children.OrderBy(n => n.OrderNumber).ToList())
                        {
                            if (item is DB.Significantpointreference)
                            {
                                spr = new XML.Significantpointreference();
                                spr.Ref = ((DB.Significantpointreference)item).Ref;
                                spr.Updated = SignificantpointreferenceUpdated.No;
                                spr.SignificantpointATC = new XML.SignificantpointATC { Items = new string[] { ((DB.Significantpointreference)item).SignificantpointATC } };
                                rt_items.Add(spr);
                            }
                            else if (item is DB.Routesegment)
                            {
                                rs = new XML.Routesegment();
                                rs.Updated = RoutesegmentUpdated.No;
                                rs.Routesegmentmagtrack = new Routesegmentmagtrack { Updated = RoutesegmentmagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentmagtrack } };
                                rs.Routesegmentreversemagtrack = new Routesegmentreversemagtrack { Updated = RoutesegmentreversemagtrackUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentreversemagtrack } };
                                rs.Routesegmentlength = new Routesegmentlength { Updated = RoutesegmentlengthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlength } };

                                rs.Routesegmentupper = new Routesegmentupper { Updated = RoutesegmentupperUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentupper } };
                                rs.Routesegmentlower = new Routesegmentlower { Updated = RoutesegmentlowerUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentlower } };
                                rs.Routesegmentminimum = new Routesegmentminimum { Updated = RoutesegmentminimumUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentminimum } };
                                rs.Routesegmentwidth = new Routesegmentwidth { Updated = RoutesegmentwidthUpdated.No, Items = new string[] { (item as DB.Routesegment).Routesegmentwidth } };
                                rs.RoutesegmentATC = new RoutesegmentATC { Updated = RoutesegmentATCUpdated.No, Items = new string[] { (item as DB.Routesegment).RoutesegmentATC } };
                                List<XML.Routesegmentusagereference> tmp = new List<XML.Routesegmentusagereference>();
                                if ((item as DB.Routesegment).Routesegmentusagereference != null)
                                {
                                    foreach (DB.Routesegmentusagereference rsur in (item as DB.Routesegment).Routesegmentusagereference)
                                    {
                                        tmp.Add(
                                            new XML.Routesegmentusagereference
                                            {
                                                Ref = rsur.Ref,
                                                Updated = RoutesegmentusagereferenceUpdated.No,
                                                Routesegmentusageleveltype = new Routesegmentusageleveltype
                                                {
                                                    Items = new string[] { rsur.Routesegmentusageleveltype }
                                                },
                                                Routesegmentusagedirection = new Routesegmentusagedirection
                                                {
                                                    Items = new string[] { rsur.Routesegmentusagedirection }
                                                }
                                            }
                                            );
                                    }
                                }

                                rs.Routesegmentusagereference = tmp.ToArray();
                                rs.Routesegmentairspaceclass = new Routesegmentairspaceclass() { Items = new string[] { (item as DB.Routesegment).Routesegmentairspaceclass } };
                                rt_items.Add(rs);
                            }
                        }
                        // Temporary fix to list SP and RS, need to find something better
                        if (rt_items.Count > 0)
                        {

                        }
                        rt.Items = rt_items.ToArray();

                        lst.Add(rt);
                    }
                }
                else
                {
                    XML_AIP.NIL = NilArray();
                }
                XML_AIP.Items = lst.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        ///// <summary>
        ///// Build specified AIP section
        ///// </summary>
        ///// <param name="db"></param>
        ///// <param name="section"></param>
        ///// <param name="eAIP"></param>
        ///// <returns></returns>
        //public static dynamic Build_ENR35(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        //{
        //    try
        //    {
        //        ENR35 XML_AIP = new ENR35();

        //        XML_AIP.id = Lib.AIPClassToSection(section);
        //        XML_AIP.Title = new Title();
        //        string title = Dataset_AIP.Title?.ToUpperInvariant().ToString() ?? "OTHER ROUTES";
        //        XML_AIP.Title.Items = new string[] { title };
        //        XML_AIP.Title.Updated = "No";

        //        List<XML.Subsection> SubsectionList = new List<XML.Subsection>();
        //        foreach (DB.Subsection item in Dataset_AIP.Children.OrderBy(n => n.OrderNumber).ToList())
        //        {
        //            XML.Subsection ent = new XML.Subsection();
        //            ent.Title = SetValue<XML.Title>(item.Title);
        //            ent.Items = AddSubsectionText(item.Content);
        //            SubsectionList.Add(ent);
        //        }
        //        XML_AIP.Items = SubsectionList.ToArray();
        //        return XML_AIP;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR36(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR36 XML_AIP = new XML.ENR36();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Number = Lib.AIPClassToSection(section);
                XML_AIP.Toc = ENR36Toc.Yes;
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "En-route holding";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";


                thead thead = new thead()
                {
                    @class = "colsep-0 rowsep-0",
                    valign = theadValign.bottom,
                    Updated = theadUpdated.No
                    ,
                    tr = new tr[] {
                        new tr()
                        { @class = "rowsep-1", Updated = trUpdated.No, Items = new object[]
                            {
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "HLDG ID/FIX/WPT Coordinates" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "INBD TR (˚MAG)" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "Direction of PTN" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "MAX IAS (KT)" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "MNM-MAX HLDG LVL FL/FT (MSL)" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "TIME (MIN) or OUBD" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "Controlling unit and Frequency" } }
                            }
                        },
                        new tr()
                        { @class = "rowsep-1", Updated = trUpdated.No, Items = new object[]
                            {
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "1" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "2" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "3" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "4" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "5" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "6" } },
                                new th() { rowspan = "1", colspan = "1", Updated = thUpdated.No, Items = new object[] { "7" } }
                            }
                        }
                    }
                };


                tbody tbody2 = new tbody() { @class = "colsep-0 rowsep-0", valign = tbodyValign.top, Updated = tbodyUpdated.No };
                List<tr> trlist = new List<tr>();


                if (Dataset_AIP != null)
                    foreach (DB.Sec36Table item in Dataset_AIP?.Children.OfType<Sec36Table>().OrderBy(n => n.Name))
                    {
                        int cnt = 0;
                        if (item.Sec36Table2.Count == 0) // error
                        {
                            break;
                        }
                        foreach (DB.Sec36Table2 item2 in item.Sec36Table2)
                        {
                            cnt++;
                            tr tr = new tr() {@class = "rowsep-0", Updated = trUpdated.No};
                            td td2 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {item2.inboundCourse}
                            };
                            td td3 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {item2.turnDirection}
                            };
                            td td4 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {item2.speedLimit}
                            };
                            td td5 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {item2.lowerLimit + " - " + item2.upperLimit}
                            };
                            td td6 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {item2.duration}
                            };
                            td td7 = new td()
                            {
                                rowspan = "1",
                                colspan = "1",
                                Updated = tdUpdated.No,
                                Items = new object[] {" "}
                            };
                            if (cnt == 1)
                            {
                                td td1 = new td()
                                {
                                    rowspan = item.rowspan.ToString(),
                                    @class = "rowsep-1",
                                    colspan = "1",
                                    Updated = tdUpdated.No,
                                    Items = new object[]
                                    {
                                        new p() {Updated = pUpdated.No, Items = new object[] {item.Name}},
                                        new p() {Updated = pUpdated.No, Items = new object[] {item.Ident}},
                                        new p() {Updated = pUpdated.No, Items = new object[] {item.Y + " " + item.X}},
                                    }
                                };
                                tr.Items = new object[] {td1, td2, td3, td4, td5, td6, td7};
                            }
                            else
                            {
                                tr.Items = new object[] {td2, td3, td4, td5, td6, td7};
                            }
                            trlist.Add(tr);
                        }
                    }
                tbody2.tr = trlist.ToArray();
                table table = new table()
                {
                    frame = tableFrame.border,
                    Updated = tableUpdated.No,
                    Items = new object[]
                {   new col() { @class = "colwidth7"},
                    new col() { @class = "colwidth4"},
                    new col() { @class = "colwidth4"},
                    new col() { @class = "colwidth4"},
                    new col() { @class = "colwidth6"},
                    new col() { @class = "colwidth4"},
                    new col() { @class = "colwidth7"},
                    thead,
                    tbody2
                }
                };
                XML_AIP.Items = new object[] { table };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR41(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR41 XML_AIP = new XML.ENR41();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "RADIO NAVIGATION AIDS - EN-ROUTE";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";

                XML_AIP.Navaidtable = new Navaidtable();
                List<XML.Navaid> lst = new List<XML.Navaid>();
                if (Dataset_AIP != null)
                    foreach (DB.Navaid item in Dataset_AIP?.Children.OfType<DB.Navaid>())
                    {
                        XML.Navaid ent = new XML.Navaid();
                        ent.Navaiddeclination = SetValue<Navaiddeclination>(item.Navaiddeclination);
                        ent.Navaidelevation = SetValue<Navaidelevation>(item.Navaidelevation);
                        ent.Navaidfrequency = new Navaidfrequency() {Items = new object[] {item.Navaidfrequency}};
                        ent.Navaidhours = new Navaidhours() {Items = new object[] {item.Navaidhours}};
                        ent.Navaidmagneticvariation =
                            new Navaidmagneticvariation() {Items = new object[] {item.Navaidmagneticvariation}};
                        ent.Navaidname = new Navaidname() {Items = new object[] {item.Navaidname}};
                        ent.Navaidtype = new Navaidtype() {Items = new object[] {item.Navaidtype}};

                        ent.Navaidident = new XML.Navaidident() {Items = new object[] {item.Navaidident}};
                        ent.id = "NAV-" + item.Navaidident;
                        ent.Latitude = new XML.Latitude() {Items = new object[] {item.Latitude}};
                        ent.Longitude = new XML.Longitude() {Items = new object[] {item.Longitude}};
                        ent.Listed = (item.NavaidListed == YesNo.Yes) ? NavaidListed.Yes : NavaidListed.No;
                        if (item.Navaidremarks?.Count() > 0)
                        {
                            ent.Navaidremarks = new Navaidremarks() {Items = item.Navaidremarks};
                        }
                        else
                        {
                            ent.Navaidremarks = new Navaidremarks() {Items = new string[] {" "}};
                        }
                        lst.Add(ent);
                    }
                XML_AIP.Navaidtable.Navaid = lst.ToArray();

                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        /// <summary>
        /// Build specified AIP section
        /// </summary>
        /// <param name="db"></param>
        /// <param name="section"></param>
        /// <param name="eAIP"></param>
        /// <returns></returns>
        public static dynamic Build_ENR44(DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.ENR44 XML_AIP = new XML.ENR44();

                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section) ?? "Name-code designations for significant points";
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";

                XML_AIP.Designatedpointtable = new Designatedpointtable();
                List<XML.Designatedpoint> lst = new List<XML.Designatedpoint>();
                if (Dataset_AIP != null)
                {
                    var dpLst = Dataset_AIP.Children.OfType<DB.Designatedpoint>()
                        .OrderBy(n => n.Designatedpointident).ToList();
                    foreach (DB.Designatedpoint item in dpLst)
                    {
                        XML.Designatedpoint ent = new XML.Designatedpoint();
                        ent.Designatedpointident =
                            new XML.Designatedpointident() {Items = new object[] {item.Designatedpointident}};
                        ent.id = "SP-" + item.Designatedpointident;
                        ent.Latitude = new XML.Latitude() {Items = new object[] {item.Latitude}};
                        ent.Longitude = new XML.Longitude() {Items = new object[] {item.Longitude}};
                        ent.Listed = (item.DesignatedpointListed == YesNo.Yes)
                            ? DesignatedpointListed.Yes
                            : DesignatedpointListed.No;
                        ent.SIDSTAR = (item.SIDSTAR != "")
                            ? new List<SIDSTAR>()
                            {
                                new SIDSTAR()
                                {
                                    Items = new object[] {item.SIDSTAR}
                                }
                            }.ToArray()
                            : null;
                        lst.Add(ent);
                    }
                }

                XML_AIP.Designatedpointtable.Designatedpoint = lst.ToArray();
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_AD01(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD01 XML_AIP = new XML.AD01();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new[] { new NIL() { Reason = NILReason.Notapplicable } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }

        public static dynamic Build_AD02(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD02 XML_AIP = new XML.AD02();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Items = new[] { new NIL() { Reason = NILReason.Notapplicable } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_AD03(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD03 XML_AIP = new XML.AD03();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.Items = new[] { new NIL() { Reason = NILReason.Notapplicable } };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_AD04(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD04 XML_AIP = new XML.AD04();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new NIL() { Reason = NILReason.Notapplicable };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }


        public static dynamic Build_AD05(eAIPContext db, DB.AIPSection Dataset_AIP, string section, DB.eAIP eAIP)
        {
            try
            {
                XML.AD05 XML_AIP = new XML.AD05();
                XML_AIP.id = Lib.AIPClassToSection(section);
                XML_AIP.Title = new Title();
                string title = Dataset_AIP?.Title?.ToUpperInvariant().ToString() ?? Tpl.MenuText(section);
                XML_AIP.Title.Items = new string[] { title };
                XML_AIP.Title.Updated = "No";
                XML_AIP.NIL = new NIL() { Reason = NILReason.Notapplicable };
                return XML_AIP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite}", ex, true);
                return null;
            }
        }
        #endregion
    }
}
