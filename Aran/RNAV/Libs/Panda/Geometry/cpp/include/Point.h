#ifndef POINT_H
#define POINT_H

#include "Geometry.h"

namespace Panda
{
	class Point: public Geometry
	{
		public:
			Point () throw ();
			Point (double x, double y) throw ();

			virtual void assign (const Geometry& geometry) throw (std::bad_cast);
			virtual Geometry* clone () const throw (std::bad_cast);
			virtual void pack (Handle handle) const throw (Registry::Exception);
			virtual void unpack (Handle handle) throw (Registry::Exception);
			virtual int geometryType () const throw ();


			void setCoords(double x, double y) throw ();
			void setX(double x) throw ();
			double getX() const throw ();
			void setY(double y) throw ();
			double getY() const throw ();
			void setZ(double z) throw ();
			double getZ() const throw ();
			void setM(double m) throw ();
			double getM() const throw ();
			void setT(double t) throw ();
			double getT() const throw ();
			bool isEmpty() const throw ();
			void setEmpty() throw ();

		private:
			double _x;
			double _y;
			double _z;
			double _m;
			double _t;
	};
}

#endif
