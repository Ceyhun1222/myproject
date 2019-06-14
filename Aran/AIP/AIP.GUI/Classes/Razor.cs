using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AIP.BaseLib.Class;
using AIP.GUI.Properties;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace AIP.GUI.Classes
{
    internal static class Razor
    {
        static readonly bool IsDebug = true;
        static Razor()
        {
            try
            {
                TemplateServiceConfiguration config = new TemplateServiceConfiguration {Language = Language.CSharp};
                if (IsDebug)
                    config.Debug = true;
                else
                    config.DisableTempFileLocking = true;
                //config.CachingProvider = new DefaultCachingProvider(t => { });
                //config.EncodedStringFactory = new RawStringFactory(); // Raw string encoding.
                //config.EncodedStringFactory = new HtmlEncodedStringFactory(); // Html encoding.
                IRazorEngineService service = RazorEngineService.Create(config);
                Engine.Razor = service;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        internal static string Run<T>(T obj)
        {
            try
            {
                Type sectionType = typeof(T);
                string template = GetTemplateContent<T>();
                string key = GetKey(template + sectionType.Name);
                return Engine.Razor.RunCompile(template, key, sectionType, obj);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static string GetTemplateContent<T>()
        {
            try
            {
                string template = "";
                string section = "";

                string fullName = typeof(T).FullName;
                section = fullName != null && fullName.Contains("System.Collections") ?
                    fullName.ToStringBetween(".For", ", ") : 
                    typeof(T).Name.Replace("For","");
                

                if (!Settings.Default.UserTemplates)
                {
                    var obj = RazorTemplates.ResourceManager.GetObject(section);
                    if (obj is byte[])
                        template = System.Text.Encoding.UTF8.GetString(obj as byte[]);
                }
                else
                {
                    string file = $@"Templates/AIP/{section}.cshtml";
                    template = File.Exists(file) ? File.ReadAllText(file) : "";
                }
                if (template == "")
                    throw new ArgumentException("System incorrectly detected template file name. File will not be found");

                return template;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        private static string GetKey(string fileContent)
        {
            try
            {
                return Hash.GetMd5(fileContent);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }
}
