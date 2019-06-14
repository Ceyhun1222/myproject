#ifndef GEOMETRY_OPERATORS_CONTRACT
#define GEOMETRY_OPERATORS_CONTRACT
	
//#include <typeinfo>
#include <Panda/Registry/cpp/include/Contract.h>

namespace Panda
{
    class Geometry;
	class Polyline;
	class Point;
	class SpatialReference;

    class GeometryOperatorsContract
    {
		public:
			class Commands
			{
				public:
					enum
					{
						union_ = 1,
						convexHull,	// 2
						cut,		// 3
						intersect,	// 4
						boundary,	// 5
						buffer,		// 6
						difference,	// 7

						getNearestPoint,// 8
						getDistance,	// 9
						
						contains,	// 10
			 			crosses,	// 11
						disjoint,	// 12
						equals,		// 13

						geoTransformations	// 14
					};
			};

        public:
			GeometryOperatorsContract () throw ();
        	~GeometryOperatorsContract () throw ();

            bool isValid () const throw ();
			
			// #region Topological Operators
			const Geometry& unionGeometry (const Geometry& geom1, const Geometry& geom2) throw (Registry::Exception);
        	const Geometry& convexHull (const Geometry& geom) throw (Registry::Exception);
			void cut (const Geometry& geom, const Polyline& cutter, const Geometry*& geomLeft, const Geometry*& geomRight) throw (Registry::Exception);
			const Geometry& intersect (const Geometry& geom1, const Geometry& geom2) throw (Registry::Exception);
			const Geometry& boundary (const Geometry& geom) throw (Registry::Exception);
			const Geometry& buffer(const Geometry& geom, double width) throw (Registry::Exception);
			const Geometry& difference(const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			// #endregion

			// #region Proximity Operators
			const Point& nearestPoint (const Geometry& geom, const Point& point) throw (Registry::Exception, std::bad_cast);
			double distance (const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			// #endregion

			// #region Relational Operators
			bool contains (const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			bool crosses (const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			bool disjoint (const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			bool equals (const Geometry& geom, const Geometry& other) throw (Registry::Exception);
			// #endregion

			const Geometry& geoTransformations(
					const Geometry& geom, 
					const SpatialReference& fromSR, 
					const SpatialReference& toSR);
		
        public:
        	Handle _handle;
    };
    
}

#endif //GEOMETRY_OPERATORS_CONTRACT
