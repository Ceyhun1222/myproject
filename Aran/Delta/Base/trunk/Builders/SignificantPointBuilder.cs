using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Features;
using Aran.Delta.Builders;
using Aran.Delta.Enums;
using Aran.Delta.Model;
using Unity;
using Unity.Interception.Utilities;

namespace Aran.Delta.Builders
{
    public class SignificantPointBuilder:ISignificantPointBuilder
    {
        private readonly List<IPointModel> _ptModelList;
        private readonly IUnityContainer _container;
        private readonly IDBModule _dbModule;

        public SignificantPointBuilder(IUnityContainer container)
        {
            _dbModule = container.Resolve<IDBModule>();
            _ptModelList = new List<IPointModel>();
            _container = container;
        }

        public void BuildDesignatedPoint()
        {
            _dbModule.DesignatedPointList?.Where(dp => dp.Location != null)
                .ForEach(designatedPoint => _ptModelList.Add(GetPtModel(designatedPoint, PointChoiceType.DesignatedPoint)));
        }

        public void BuildNavaids()
        {
            _dbModule.NavaidList?.Where(nav => nav.Location != null)
                .ForEach(designatedPoint => _ptModelList.Add(GetPtModel(designatedPoint, PointChoiceType.Navaid)));
        }

        public void BuildRunwayCenterlinePoints()
        {
            _dbModule.RunwayCenterlineList?.Where(cnt => cnt.Location != null)
                .ForEach(cnt => _ptModelList.Add(GetPtModel(cnt, PointChoiceType.RunwayCenterlinePoint)));
        }

        private IPointModel GetPtModel(dynamic significantObject,PointChoiceType choiceType)
        {
            var ptModel = _container.Resolve<IPointModel>();
            ptModel.Name = significantObject.Designator;
            ptModel.Geo = significantObject.Location.Geo;
            ptModel.Identifier = significantObject.Identifier;
            ptModel.ObjectType = choiceType;
            ptModel.Feat = significantObject;
            return ptModel;

        }

        public IList<IPointModel> GetSegmentPoints()
        {
            return _ptModelList?.OrderBy(ptModel => ptModel.Name).ToList();
        }
    }
}
