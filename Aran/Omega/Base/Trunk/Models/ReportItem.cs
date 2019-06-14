using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Omega.Models
{
    public class ReportItem
    {
        public ReportItem()
        {
            InnerHorizontal = " ";
            Conical = " ";
            OuterHorizontal = " ";
            Strip = " ";
            Transitional = " ";
            Approach = " ";
            InnerApproach = " ";
            InnerTransitional =" ";
            TakeOffClimb = " ";
            BalkedLanding = " ";
            TakeOffFlightPathArea= " ";
            ObstacleElevation = "";
            Area2A = "";
        }

        public long Id { get; set; }
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public string Designator{ get; set; }
        public string ObstacleElevation { get; set; }
        public string InnerHorizontal { get; set; }
        public string Conical { get; set; }
        public string OuterHorizontal { get; set; }
        public string Strip { get; set; }
        public string Area2A { get; set; }
        public string Transitional { get; set; }
        public string Approach { get; set; }
        public string InnerApproach { get; set; }
        public string InnerTransitional { get; set; }
        public string TakeOffClimb { get; set; }
        public string BalkedLanding { get; set; }
        public string TakeOffFlightPathArea { get; set; }
    }

}
