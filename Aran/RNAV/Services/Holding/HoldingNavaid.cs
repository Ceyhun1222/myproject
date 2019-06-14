using System;
using System.Text;
using System.Collections;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.PANDA.Common;

namespace Holding
{
    public class HoldingNavaid
    {
        private double _courceNavaidToFix,_distance;
        private int _navHandle;
        
        public HoldingNavaid(Navaid nav,bool check,double distance,double direction)
        {
            this.Checked = check;
            this.Designator = nav.Designator;
            this.NavType = nav.Type;
            this._nav = nav;
            this.DistanceNavaidToFix = distance;
            this.CourseNavaidToFix = direction;
        }

        public HoldingNavaid()
        {

        }

        private bool _checked;

        public bool Checked
        {
            get { return _checked; }
            set 
            {
                _checked = value;

                GlobalParams.UI.SafeDeleteGraphic(_navHandle);

				if (_checked && _nav != null)
					_navHandle = GlobalParams.UI.DrawPointWithText(GlobalParams.SpatialRefOperation.ToPrj(_nav.Location.Geo), _nav.Designator, new Aran.AranEnvironment.Symbols.PointSymbol(Aran.AranEnvironment.Symbols.ePointStyle.smsDiamond, 134, 8));
            }
        }
        
        public string Designator { get; set; }
        public CodeNavaidService? NavType { get; set; }
       
        public double DistanceNavaidToFix
        {
            get { return (double)Math.Round((decimal)Common.ConvertDistance(_distance, roundType.toNearest), Convert.ToInt32(InitHolding.DistancePrecision * 10)); }
            set 
            {
                _distance = value;
            }
        }
        
        public double  CourseNavaidToFix
        {
            get { return Math.Round(ARANMath.RadToDeg(_courceNavaidToFix), 0) ;} 
            set{ _courceNavaidToFix = ARANMath.DegToRad(value);
            }
        }
        
        public Navaid GetNavaid
        {
            get { return _nav; }
        }

        public void Dispose()
        {
            GlobalParams.UI.SafeDeleteGraphic(_navHandle);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HoldingNavaid))
                return false;
            else
            {
                HoldingNavaid p = (HoldingNavaid)obj;
                return (p.Designator == this.Designator &&
                p.NavType == this.NavType);
            }
        }

        public override int GetHashCode()
        {
           return base.GetHashCode();
        }

        private Navaid _nav;
    }
}
