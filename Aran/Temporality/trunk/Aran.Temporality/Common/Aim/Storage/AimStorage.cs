#region

using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Temporality.Aim.MetaStorage;
using Aran.Temporality.Internal.Abstract.Storage;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.Implementation.Storage.Linq;
using Aran.Temporality.Internal.Util;

#endregion

namespace Aran.Temporality.Aim.Storage
{
    internal class AimStorage : AbstractStorage<Feature>
    {
        public AimStorage(String path) : base(path)
        {
        }

        #region Overrides of Storage<DummyFeature>

        private readonly AbstractEventStorage<Feature> _abstractEventStorage = new AimEventStorage();
        private readonly AbstractStateStorage<Feature> _abstractStateStorage = new AimStateStorage();

        internal override AbstractEventStorage<Feature> AbstractEventStorage
        {
            get { return _abstractEventStorage; }
        }

        internal override AbstractStateStorage<Feature> AbstractStateStorage
        {
            get { return _abstractStateStorage; }
        }


        #endregion
    }
}