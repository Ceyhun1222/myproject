using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Models;

namespace ChartManagerWeb.Helper
{
    public static class Extensions
    {
        public static Chart4View ConvertTo(this Chart chart)
        {
            var result = new Chart4View
            {
                Id = chart.Id,
                CreatedAt = chart.CreatedAt,
                CreatedBy = chart.CreatedBy.FirstName + " " + chart.CreatedBy.LastName,
                Name = chart.Name,
                Type = chart.Type.ToString(),
                Version = chart.Version,
                Effective = "From: " + chart.BeginEffectiveDate.ToShortDateString() + "; To:  ",
                Locked = chart.IsLocked,
                Airport = chart.Airport,
                Organization = chart.Organization,
                RunwayDirection = chart.RunwayDirection
            };
            if (chart.EndEffectiveDate.HasValue)
                result.Effective += chart.EndEffectiveDate.Value.ToShortDateString();
            if (chart.IsLocked)
                result.LockedBy = chart.LockedBy.FirstName + " " + chart.LockedBy.LastName;
            return result;
        }
    }
}