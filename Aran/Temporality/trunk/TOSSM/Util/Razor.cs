using Aran.Temporality.Common.Logging;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TOSSM.Util
{
    internal static class Razor
    {
        static readonly bool IsDebug = true;

        public static object RazorTemplates { get; private set; }

        static Razor()
        {
            try
            {
                TemplateServiceConfiguration config = new TemplateServiceConfiguration { Language = Language.CSharp };
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
                LogManager.GetLogger(typeof(Razor)).Error(ex, $@"Error in the {ex.TargetSite?.Name}");
            }
        }

        internal static string Run<T>(string name,T obj)
        {
            try
            {
                Type sectionType = typeof(T);
                string template = GetTemplateContent<T>(name);
                string key = GetKey(template + sectionType.Name);
                return Engine.Razor.RunCompile(template, key, sectionType, obj);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Razor)).Error(ex, $@"Error in the {ex.TargetSite?.Name}");
                return null;
            }
        }

        private static string GetTemplateContent<T>(string name)
        {
            try
            {
                string file = $@"Report/Template/{name}.cshtml";
                var template = File.Exists(file) ? File.ReadAllText(file) : "";

                if (template == "")
                    throw new ArgumentException("System incorrectly detected template file name. File will not be found");

                return template;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Razor)).Error(ex, ($@"Error in the {ex.TargetSite?.Name}"));
                return null;
            }

        }

        private static string GetMd5Hash(string input)
        {
            try
            {
                var md5 = MD5.Create();
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hash = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (byte t in hash)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Razor)).Error(ex, $@"Error in the {ex.TargetSite?.Name}");
                return null;
            }
        }

        private static string GetKey(string fileContent)
        {
            try
            {
                return GetMd5Hash(fileContent);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Razor)).Error(ex, $@"Error in the {ex.TargetSite?.Name}");
                return null;
            }
        }
    }
}
