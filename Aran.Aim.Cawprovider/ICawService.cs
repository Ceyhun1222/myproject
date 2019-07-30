using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;

namespace Aran.Aim.CAWProvider
{
    public interface ICawService
    {
        ConnectionInfo ConnectionInfo { get; set; }

        int BeginTransaction ();

        void Commit (int workPackageId, bool save);

        bool InsertFeature (Feature feature, int? workpackageId);

        Feature[] GetFeature(AbstractRequest query, int? workPackageId);

        List<TFeature> GetFeature<TFeature> (AbstractRequest query) where TFeature : Feature;
    }

    public static class CawProviderFactory
    {
        public static ICawService CreateService (CawProviderType providerType)
        {
            if (providerType == CawProviderType.FileBase)
                return new CawFileService ();
            else
                return new CawService ();
        }
    }

    public enum CawProviderType { FileBase, ComSoft }
}
