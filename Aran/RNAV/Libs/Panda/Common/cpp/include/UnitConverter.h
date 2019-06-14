#ifndef UNIT_CONVERTER_H
#define UNIT_CONVERTER_H

#include <string>

namespace Panda
{
    class Point;

	class DistanceUnit
	{
		public:	
			enum
			{
				Feet,
				Meter
			};
	};

	class TemperatureUnit
	{
		public:
			enum
			{
				Celsius,
				Fahrenheit
			};
	};

    class UnitConverter
    {
        public:
            UnitConverter();
            ~UnitConverter();

           	void coordinatToMetric (const std::wstring& x, const std::wstring& y, Panda::Point& point);

    		double distanceToMeter (double distance, int distanceUnits);
			double temperatureToMeter (double temperature, int temperatureUnits);

    		void toProject (
                    const std::wstring& goeDatum, 
                    const Panda::Point& ptGeo, 
                    const std::wstring& prjDatum, 
                    Panda::Point& ptPrj);
    };
}    
    
#endif //UNIT_CONVERTER_H
