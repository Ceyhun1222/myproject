#include <cstdio>
#include "../include/EsriPoint.h"

namespace
{	
	class ComInit
	{
	public:
		ComInit()
		{
			CoInitialize (NULL);
		}
		~ComInit()
		{
			CoUninitialize ();
		}
	};

	ComInit comInit;
}


int main()
{
	Esri::Point* p = Esri::Point::create();
	p->PutCoords(1, 2);

	Esri::Geometry* geom = p->asGeometry();

	printf("GeomType: %d\n", geom->getType());

	return 0;
}