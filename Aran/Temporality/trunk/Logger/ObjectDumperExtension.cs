using Newtonsoft.Json;

namespace Aran.Temporality.Common.Extensions
{
    public static class ObjectDumperExtension
    {
        public static string Dump(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
