using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AIP.DB;

namespace AIP.GUI
{
    /// <summary>
    /// Static class for cshtml templates
    /// Contains common properties, require for template
    /// To use value in the template use @AIP.GUI.Tpl.PropertyName
    /// or declare @using AIP.GUI and then use @Tpl.PropertyName
    /// Only public properties are visible, using internal property return error
    /// 
    /// </summary>
    public static class Tpl
    {
        public static string Language;

        public static string Text(string templateName)
        {
            try
            {
                if (string.IsNullOrEmpty(templateName) || !Lib.TplLang?.ContainsKey(templateName) == true) return "";

                return Lib.TplLang[templateName];
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string MenuText(string templateName)
        {
            try
            {
                if (string.IsNullOrEmpty(templateName) ||
                    !Lib.MenuLang?.ContainsKey(templateName) == true)
                    return "";
                Regex withoutCategory = new Regex(@"(GEN|ENR|AD) [0-9]\.?[0-9]{0,2}? ");
                return withoutCategory.Replace(Lib.MenuLang?[templateName] ?? "", "");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static string MenuFullText(string templateName)
        {
            try
            {
                if (string.IsNullOrEmpty(templateName) ||
                    !Lib.MenuLang?.ContainsKey(templateName) == true)
                    return "";

                return Lib.MenuLang?[templateName];
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        /// <summary>
        /// Return text from menu text strings without Section name
        /// For ex: "GEN 0.1 Preface" return "Preface"
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static string CleanMenuText(string templateName)
        {
            try
            {
                string menuText = MenuText(templateName);
                return Regex.Replace(menuText, @"((GEN|ENR|AD)+[0-9.\s]{1,})", "");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }
        
    }
}
