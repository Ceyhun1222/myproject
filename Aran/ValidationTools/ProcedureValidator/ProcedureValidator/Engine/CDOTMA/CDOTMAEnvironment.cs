using System;

namespace PVT.Engine.CDOTMA
{
    public class CDOTMAEnvironment:Environment
    {
        public CDOTMAEnvironment()
        {
            Value = Environments.CDOTMA;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
