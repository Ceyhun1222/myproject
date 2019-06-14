using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ObstacleManagementSystem.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }
    }


    //public enum MobileOperator
    //{
    //    [Display(Name = "050")]
    //    Sim = 50,
    //    [Display(Name = "051")]
    //    Sim1 = 51,
    //    [Display(Name = "055")]
    //    Cin = 55,
    //    [Display(Name = "070")]
    //    Nar = 70,
    //    [Display(Name = "077")]
    //    Nar1 = 77
    //}
}
