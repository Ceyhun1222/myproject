using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aran.Geometries;
using Newtonsoft.Json.Linq;

namespace Aran.Panda.VisualManoeuvring
{
    public class VM_VisualFeature
    {
        public Point Location
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        public Point pShape
        {
            get;
            set;
        }
        public Point gShape
        {
            get;
            set;
        }
        public VM_VisualFeature()
        {
        }

        public JObject ToJson()
        {
            return new JObject(
                new JProperty("name", Name),
                new JProperty("description", Description),
                new JProperty("point", new JObject(
                    new JProperty("x", pShape.X),
                    new JProperty("y", pShape.Y)))
            );
        }

        public void FromJson(JObject jo)
        {
            Name = jo["name"].Value<String>();
            Description = jo["description"].Value<String>();
            var pointJO = jo["point"].Value<JObject>();
            pShape = new Point(pointJO["x"].Value<double>(), pointJO["y"].Value<double>());
        }        
    }
}
