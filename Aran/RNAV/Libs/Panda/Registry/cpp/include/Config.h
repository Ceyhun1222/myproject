#ifndef CONFIG_H_
#define CONFIG_H_

#include <list>
#include <string>
#include "Contract.h"

class Service;

class Node;
class SectionNode;
class OptionNode;

class Config
{
    public:
		Config (std::string path);
    	class ServiceInfo
    	{
    		public:
				std::string name;
				std::string path;
				std::string dllName;
				std::string entry;
	    		Int32 priority;
				Int32 scope;
   		};

		const ServiceInfo* findServiceInfo (std::string name);
		const std::string& getPath () const;

	private:
		char getChar ();
		void parseComment ();
		void parseIdentifier (std::string& sym);
		void parseHex (std::string& sym);
		void parseEscape (std::string& sym);
		void parseString (std::string& sym);
		void parseBinaryString (std::string& sym);
		void parseSymbol (std::string& sym, Int32& type);
		void parseOption (SectionNode* parentSection);
		void parseSection (SectionNode* parentSection);
		void parseConfig ();
		void getServiceInfoList ();
		
	private:
		std::string _path;
		std::list <Config::ServiceInfo*> _serviceInfoList;
		FILE* _configFile;
		char _prevChar;
};

#endif
