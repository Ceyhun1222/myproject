#region

using System;
using System.IO;
using Aran.Temporality.Common.Logging;
using System.Runtime.CompilerServices;

#endregion

[assembly: InternalsVisibleTo("TemporalityTest")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Aran.Temporality.Internal.Util
{
    internal class FContainerFile<T> where T : class
    {
        public static bool WriteContainer(FContainer<T> container, Stream stream)
        {
            try
            {
                if (container == null) return false;

                var writer = new BinaryWriter(stream);
                //write id
                writer.Write(container.Id.ToByteArray());
                //write Size
                container.Size = container.GetSize();
                writer.Write(container.Size);
                //write Delete
                writer.Write(container.Delete);
                //write Data
                if (!container.Delete && container.Data != null)
                {
                    writer.Write(container.Data);
                }
                return true;
            }
            catch(Exception ex)
            {
                LogManager.GetLogger(typeof(FContainer<T>)).Error(ex, ex.Message);
            }

            return false;
        }

        public static FContainer<T> GetNextContainer(Stream stream)
        {
            try
            {
                var container = new FContainer<T>();

                var reader = new BinaryReader(stream);

                //read id
                container.Id = new Guid(reader.ReadBytes(16));

                //read Size
                container.Size = reader.ReadInt64();

                //read Delete
                container.Delete = reader.ReadBoolean();

                //read Data
                if (!container.Delete)
                {
                    container.Data = reader.ReadBytes((int) (container.Size - container.GetSize()));
                }


                return container;
            }
            catch(Exception ex)
            {
                LogManager.GetLogger(typeof(FContainer<T>)).Error(ex, ex.Message);
            }

            return null;
        }
    }
}