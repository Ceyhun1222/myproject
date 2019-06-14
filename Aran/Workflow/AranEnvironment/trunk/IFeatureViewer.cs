using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;

namespace Aran.AranEnvironment
{
    public interface IFeatureViewer
    {
        bool SetFeature (List<Aran.Aim.Features.Feature> featureIndex, int rootIndex);
    }
}
