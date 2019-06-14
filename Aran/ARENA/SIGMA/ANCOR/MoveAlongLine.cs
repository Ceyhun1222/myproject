using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ANCOR
{

    public static class MoverAlongLine
    {
        private static PointF _p1, _p2, _p;
        private static float sinX, cosX;
        private static bool b;
        private static float _delta;
        private static float L;

        public static PointF deltaPointFromLine(PointF p1, PointF p2, float delta, ref PointF p)
        {
            _p = p;
            _delta = delta;

            if (Math.Abs(p1.X - p2.X) >= Math.Abs(p1.Y - p2.Y))
            {
                if (p1.X > p2.X)
                {
                    _p1 = p2;
                    _p2 = p1;
                }
                else { _p1 = p1; _p2 = p2; }

                sinX = (_p2.Y - _p1.Y) / DistanceTwoPoint(_p1, _p2);
                cosX = (_p2.X - _p1.X) / DistanceTwoPoint(_p1, _p2);
                b = false;
                return deltaPoint(ref p);
            }
            else
            {
                if (p1.Y > p2.Y)
                {
                    _p1 = p2;
                    _p2 = p1;
                }
                else { _p1 = p1; _p2 = p2; }

                sinX = (_p2.Y - _p1.Y) / DistanceTwoPoint(_p1, _p2);
                cosX = (_p2.X - _p1.X) / DistanceTwoPoint(_p1, _p2);
                b = true;
                return deltaPoint(ref p);
            }
        }

        private static PointF deltaPoint(ref PointF p)
        {
            if (!b)
            {
                if (_p.X < _p1.X)
                {
                    p.X = _p1.X;
                    p.Y = _p1.Y;
                    return new PointF(_p1.X + _delta * sinX, _p1.Y - _delta * cosX);
                }
                else
                    if (_p.X >= _p1.X & _p.X <= _p2.X)
                    {
                        L = GetL(_p1.X, _p2.X, _p.X);
                        float y = _p1.Y + L * (_p2.Y - _p1.Y);

                        //p.X = _p1.X;
                        p.Y = y;

                        return new PointF(_p.X + _delta * sinX, y - _delta * cosX);
                    }
                    else
                        if (_p.X > _p2.X)
                        {
                            p.X = _p2.X;
                            p.Y = _p2.Y;
                            return new PointF(_p2.X + _delta * sinX, _p2.Y - _delta * cosX);
                        }
            }
            else
            {
                //float c = _delta;
                if (_p1.X > _p2.X) { _delta *= -1; }

                if (_p.Y < _p1.Y)
                {
                    p.X = _p1.X;
                    p.Y = _p1.Y;

                    return new PointF(_p1.X + _delta * sinX, _p1.Y - _delta * cosX);
                }
                else
                    if (_p.Y >= _p1.Y & _p.Y <= _p2.Y)
                    {
                        L = GetL(_p1.Y, _p2.Y, _p.Y);
                        float x = _p1.X + L * (_p2.X - _p1.X);

                        p.X = x;

                        return new PointF(x + _delta * sinX, _p.Y - _delta * cosX);
                    }
                    else
                        if (_p.Y > _p2.Y)
                        {
                            p.X = _p2.X;
                            p.Y = _p2.Y;
                            return new PointF(_p2.X + _delta * sinX, _p2.Y - _delta * cosX);
                        }

            }

            return new PointF();
        }

        private static float DistanceTwoPoint(PointF p1, PointF p2)
        {
            return Convert.ToSingle(Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        private static float GetL(float _xy1, float _xy2, float x)
        {
            float LL;
            LL = (x - _xy1) / (_xy2 - _xy1);
            return LL;
        }


    }
}
