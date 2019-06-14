using System;
using Aran.Aim.Features;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace PVT.Model
{
    public class ApproachProcedure : Procedure<InstrumentApproachProcedure>
    {
        

        public string ApproachType { get; }
  
        public ApproachProcedure(InstrumentApproachProcedure proc) : base(proc)
        {
            Type = ProcedureType.InstrumentApproachProcedure;
            if (Original.Landing != null && Original.Landing.Runway.Count > 0)
                RunwayDirection = new RunwayDirection(Engine.Environment.Current.DbProvider.GetRunwayDirection(Original.Landing.Runway[0].Feature.Identifier));
            else RunwayDirection = new RunwayDirection(null);
            ApproachType = Original.ApproachType.ToString();
        }

        public static List<ApproachProcedure> Convert(List<InstrumentApproachProcedure> procs)
        {
            var result = new List<ApproachProcedure>();

            bool error = false;
            foreach (var t in procs)
            {
                try
                {
                    result.Add(new ApproachProcedure(t));
                }
                catch (Exception ex)
                {
                    error = true;
                    Engine.Environment.Current.Logger.Error(ex, $"Error on loading {nameof(ApproachProcedure)}. Uuid: {t.Identifier}, name: {t.Name}");
                }
            }

            if (error)
                MessageBox.Show("There are some error on loading approach procedures. Please, see logs.");

            return result;
        }

        public static List<ApproachProcedure> Load()
        {
            return Convert(_procedures);
        }
    }
}
