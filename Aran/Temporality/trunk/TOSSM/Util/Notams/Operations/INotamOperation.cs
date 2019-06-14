using Aran.Aim;
using Aran.Temporality.Common.Entity;
using TOSSM.ViewModel.Document.Editor;

namespace TOSSM.Util.Notams
{
    public interface INotamOperation
    {
        NotamFeatureEditorViewModel GetViewModel(Notam notam, int workpackage = 0);
        void Prepare(NotamFeatureEditorViewModel notamViewModel);
        void Save(NotamFeatureEditorViewModel notam);
        FeatureType GetFeatureType(Notam notam, int workpackage = 0);
    }
}
