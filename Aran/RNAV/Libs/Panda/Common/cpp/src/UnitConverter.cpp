#include "../include/UnitConverter.h"

namespace Panda
{
    
    UnitConverter::UnitConverter ()
    {
    }

    UnitConverter::~UnitConverter()
    {
    }

    void UnitConverter::coordinatToMetric (const std::wstring& x, const std::wstring& y, Panda::Point& point)
    {
    }

    double UnitConverter::distanceToMeter (double distance, int distanceUnit)
    {
        return 0;
    }

    void UnitConverter::toProject (
            const std::wstring& goeDatum, 
            const Panda::Point& ptGeo, 
            const std::wstring& prjDatum, 
            Panda::Point& ptPrj)
    {
    }

	double UnitConverter::temperatureToMeter (double temperature, int temperatureUnits)
	{
		return 0;
	}
}
