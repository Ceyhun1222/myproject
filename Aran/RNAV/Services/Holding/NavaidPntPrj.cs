using Aran.Geometries;

namespace Holding
{
    public class NavaidPntPrj
    {
        public NavaidPntPrj ( Point pnt, NavType navType )
        {
            Value = ( pnt.Clone () as Point );
            Type = navType;
        }

        public NavaidPntPrj Clone ( )
        {
            NavaidPntPrj result = new NavaidPntPrj ( ( this.Value.Clone () as Point ), this.Type );
            return result;
        }

        public static bool operator == ( NavaidPntPrj leftNavPntPrj, NavaidPntPrj righNavPntPrj )
        {
            if ( leftNavPntPrj == null || righNavPntPrj == null || ( leftNavPntPrj.Value.X == righNavPntPrj.Value.X && leftNavPntPrj.Value.Y == righNavPntPrj.Value.Y ) )
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            var equalsObj = obj as NavaidPntPrj;
            if (equalsObj == null || (this.Value.X == equalsObj.Value.X && this.Value.Y == equalsObj.Value.Y))
                return true;
            else
                return false;
        }

        public static bool operator != ( NavaidPntPrj leftNavPntPrj, NavaidPntPrj rightNavPntPrj )
        {
            return !( leftNavPntPrj == rightNavPntPrj );
        }

        public Point Value;
        public NavType Type;
    }
}