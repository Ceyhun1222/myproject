using System;
using Aran.Aim.Features;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using PVT.Engine;

namespace PVT.Model
{
    public class DepartureProcedure : Procedure<StandardInstrumentDeparture>
    {

        public DepartureProcedure(StandardInstrumentDeparture proc) : base(proc)
        {
            Type = ProcedureType.StandardInstrumentDeparture;
            Designator = Original.Designator;
            RunwayDirection = Original.Takeoff != null && Original.Takeoff.Runway.Count > 0
                ? new RunwayDirection(
                    Engine.Environment.Current.DbProvider.GetRunwayDirection(
                        Original.Takeoff.Runway[0].Feature.Identifier))
                : new RunwayDirection(null);
        }

        public static ObservableCollection<DepartureProcedure> Convert(List<StandardInstrumentDeparture> procs)
        {
            var result = new ObservableCollection<DepartureProcedure>();
            bool error = false;
            foreach (var t in procs)
            {
                try
                {
                    result.Add(new DepartureProcedure(t));
                }
                catch (Exception ex)
                {
                    error = true;
                    Engine.Environment.Current.Logger.Error(ex, $"Error on loading {nameof(DepartureProcedure)}. Uuid: {t.Identifier}, designator: {t.Designator}");
                }
            }

            if(error)
                MessageBox.Show("There are some error on loading departure procedures. Please, see logs.");

            return result;
        }

        public static ObservableCollection<DepartureProcedure> Load()
        {
            return Convert(_procedures);
        }
    }
}
