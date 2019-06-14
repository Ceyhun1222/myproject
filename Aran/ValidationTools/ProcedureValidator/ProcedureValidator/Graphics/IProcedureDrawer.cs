using PVT.Model;

namespace PVT.Graphics
{
    public interface IProcedureDrawer:IFeatureDrawer<ProcedureBase>
    {

        void Draw(Transition tansition);
        void Draw(SegmentLeg leg);
        void Draw(SignificantPoint point);
        void Clean(Transition transition);
        void Clean(SegmentLeg leg);
        void Clean(SignificantPoint point);
    }


    public interface IHoldingDrawer : IFeatureDrawer<HoldingPattern>
    {
        void Draw(HoldingPattern pattern, int index );
        void Clean(HoldingPattern pattern, int index);
    }
}
