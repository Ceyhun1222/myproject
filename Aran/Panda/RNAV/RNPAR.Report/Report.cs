using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Report.Models;
using Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph;
using RazorEngine;
using RazorEngine.Templating;

namespace Aran.Panda.RNAV.RNPAR.Report
{
    public class Report
    {
        public string Name { get; set; }

        public List<Paragraph> Paragraphs { get; }

        public Report(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Paragraphs = new List<Paragraph>();
        }

        public string GenerateHtml()
        {
            var templateFile = AppDomain.CurrentDomain.BaseDirectory + "Report\\Template\\RNPARReport.cshtml";
            var template = File.Exists(templateFile)
                ? File.ReadAllText(templateFile)
                : throw new FileNotFoundException("Template file not found", templateFile);

            return Engine.Razor.RunCompile(template, GetMd5Hash(template), typeof(Report), this);
        }

        private static string GetMd5Hash(string template)
        {
            var md5 = MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(template);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
