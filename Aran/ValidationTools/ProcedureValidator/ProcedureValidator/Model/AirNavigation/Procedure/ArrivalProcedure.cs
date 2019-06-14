using System;
using Aran.Aim.Features;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace PVT.Model
{
    public class ArrivalProcedure: Procedure<StandardInstrumentArrival>
    {

        public ArrivalProcedure(StandardInstrumentArrival proc) : base(proc)
        {
            Type = ProcedureType.StandardInstrumentArrival;
            Designator = Original.Designator;
            if (Original.Arrival != null && Original.Arrival.Runway.Count > 0)
                RunwayDirection = new RunwayDirection(Engine.Environment.Current.DbProvider.GetRunwayDirection(Original.Arrival.Runway[0].Feature.Identifier));
            else RunwayDirection = new RunwayDirection(null);
        }

        public static ObservableCollection<ArrivalProcedure> Convert(List<StandardInstrumentArrival> procs)
        {
            var result = new ObservableCollection<ArrivalProcedure>();

            bool error = false;
            foreach (var t in procs)
            {
                try
                {
                    result.Add(new ArrivalProcedure(t));
                }
                catch (Exception ex)
                {
                    error = true;
                    Engine.Environment.Current.Logger.Error(ex, $"Error on loading {nameof(ArrivalProcedure)}. Uuid: {t.Identifier}, name: {t.Name}");
                }
            }

            if (error)
                MessageBox.Show("There are some error on loadingm arrival procedures. Please, see logs.");

            return result;
        }

        public static ObservableCollection<ArrivalProcedure> Load()
        {
            return Convert(_procedures);
        }
    }
}
