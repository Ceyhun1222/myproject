#ifndef GEOMETRY_TYPE_H
#define GEOMETRY_TYPE_H

namespace Panda
{
	class GeometryType
	{
		public:
			enum
			{
				Null,		// 0
				Point,		// 1
				MultiPoint,	// 2
				Part,		// 3
				Ring,		// 4
				Polyline,	// 5
				Polygon		// 6
			};
		};
}

#endif
