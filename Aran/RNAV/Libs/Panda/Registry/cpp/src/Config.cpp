#include <cstdio>
#include <vector>
#include "../include/Config.h"

namespace NodeType
{
	enum
	{
		section,
		option
	};
}

namespace ParseError
{
	enum
	{
		sectionExpected,
		identifierExpected,
		sectionOrOptionOrClosingParenthesisExpected,
		openingParenthesisExpected,
		closingParenthesisExpected,
		equalExpected,
		stringExpected,
		hexDigitExpected,
		badIdentifier,
		endOfFile
	};
}

namespace SymbolType
{
	enum
	{
		identifier,
		openingParenthesis,
		closingParenthesis,
		equal,
		string
	};
}


class SectionNode;

class Node
{
	public:
		Node (Int32 type, std::string name, SectionNode* parentNode):
			_type (type),
			_name (name),
			_parentNode (parentNode)
		{
		}

		virtual ~Node ()
		{
		}

		Int32 _type;
		std::string _name;
		SectionNode* _parentNode;
};

class SectionNode: public Node
{
	public:
		SectionNode (std::string name, SectionNode* parentNode):
			Node (NodeType::section, name, parentNode)
		{
		}

		~SectionNode ()
		{
			clear ();
		}

		void clear ()
		{
			for (UInt32 i = 0; i < _nodeList.size (); ++i)
			{
				Node* node = _nodeList [i];
				delete node;
			}
			_nodeList.clear ();
		}
        
		std::vector <Node*> _nodeList;
};

class OptionNode: public Node
{
	public:
		OptionNode (std::string name, std::string value, SectionNode* parentNode):
			Node (NodeType::option, name, parentNode),
			_value (value)
		{
		}

		std::string _value;
};

char Config::getChar ()
{
	char ch;

	if (_prevChar != 0)
	{
		ch = _prevChar;
		_prevChar = 0;
		return ch;
	}

	size_t readNum = fread (&ch, sizeof (char), 1, _configFile);
	if (readNum != sizeof (char))
		throw Int32 (ParseError::endOfFile);

	return ch;
}

void Config::parseComment ()
{
	while (true)
	{
		char ch = getChar ();
		if ((ch == 0x0a) || (ch == 0x0d))
			break;
	}
}

void Config::parseIdentifier (std::string& sym)
{
	while (true)
	{
		char ch = getChar ();
		if (((ch >= 'A') && (ch <= 'Z')) ||
			((ch >= 'a') && (ch <= 'z')) ||
			((ch >= '0') && (ch <= '9')) ||
			(ch == '_'))
			sym += ch;
		else if (ch <= 32)
			break;
		else
			throw Int32 (ParseError::badIdentifier);
	}
}

void Config::parseHex (std::string& sym)
{
	Int32 byte = 0;
	for (Int32 i=0; i < 2; ++i)
	{
		byte <<= 4;
		char ch = getChar ();
		if ((ch >= '0') && (ch <= '9'))
		{
			byte += (ch - '0');
		}
		else if ((ch >= 'A') && (ch <= 'F'))
		{
			byte += (ch - 'A' + 10);
		}
		else if ((ch >= 'a') && (ch <= 'f'))
		{
			byte += (ch - 'a' + 10);
		}
		else
		{
			throw Int32 (ParseError::hexDigitExpected);
		}
	}

	sym += char (byte);
}

void Config::parseEscape (std::string& sym)
{
	char ch = getChar ();
	if (ch == 'x')
		parseHex (sym);
	else
		sym += ch;
}

void Config::parseString (std::string& sym)
{
	while (true)
	{
		char ch = getChar ();
		if (ch < 32)
			continue;
		else if (ch == '\"')
			break;
		else if (ch == '\\')
			parseEscape (sym);
		else
			sym += ch;
	}
}

void Config::parseBinaryString (std::string& sym)
{
	while (true)
	{
		char ch = getChar ();
		if (ch <= 32)
			continue;
		else if (ch == ']')
			break;
		else
		{
			_prevChar = ch;
			parseHex (sym);
		}
	}
}

void Config::parseSymbol (std::string& sym, Int32& type)
{
	sym = "";
	while (true)
	{
		char ch = getChar ();
		switch (ch)
		{
			case '#':
				parseComment ();
				break;

			case '\"':
				type = SymbolType::string;
				parseString (sym);
				return;

			case '{':
				sym = ch;
				type = SymbolType::openingParenthesis;
				return;

			case '}':
				sym = ch;
				type = SymbolType::closingParenthesis;
				return;

			case '=':
				sym = ch;
				type = SymbolType::equal;
				return;

			case '[':
				type = SymbolType::string;
				parseBinaryString (sym);
				return;

			default:
			{
				if (ch <= 32)
					continue;
                    
				if (((ch >= 'A') && (ch <= 'Z')) ||
				   ((ch >= 'a') && (ch <= 'z')) ||
				   (ch == '_'))
				{
					sym = ch;
					type = SymbolType::identifier;
					parseIdentifier (sym);
					return;
				}
			}

		}
	}
}

void Config::parseOption (SectionNode* parentSection)
{
	Int32 type;
	std::string symbol;

	parseSymbol (symbol, type);
	if (type != SymbolType::identifier)
		throw Int32 (ParseError::identifierExpected);

	std::string name = symbol;
	parseSymbol (symbol, type);
	if (type != SymbolType::equal)
		throw Int32 (ParseError::equalExpected);

	parseSymbol (symbol, type);
	if (type != SymbolType::string)
		throw Int32 (ParseError::stringExpected);

	OptionNode* option = new OptionNode (name, symbol, parentSection);
	parentSection->_nodeList.push_back (option);
}

void Config::parseSection (SectionNode* parentSection)
{
	Int32 type;
	std::string symbol;

	parseSymbol (symbol, type);
	if (type != SymbolType::identifier)
		throw Int32 (ParseError::identifierExpected);

	std::string name = symbol;
	parseSymbol (symbol, type);
	if (type != SymbolType::openingParenthesis)
		throw Int32 (ParseError::openingParenthesisExpected);

	SectionNode* section = new SectionNode (name, parentSection);
	if (parentSection != 0)
		parentSection->_nodeList.push_back (section);

	while (true)
	{
		parseSymbol (symbol, type);
		if (type == SymbolType::closingParenthesis)
			break;       
		if (symbol == "section")
			parseSection (section);
		else if (symbol == "option")
			parseOption (section);
		else
			throw Int32 (ParseError::sectionOrOptionOrClosingParenthesisExpected);
	}
}

static SectionNode _rootNode ("", 0);

void Config::parseConfig ()
{
	Int32 type;
	std::string sym;

	parseSymbol (sym, type);
	if (sym != "section")
		throw Int32 (ParseError::sectionExpected);

	_rootNode.clear ();
	parseSection (&_rootNode);
}

void Config::getServiceInfoList ()
{
	std::string path = _path + "/config.ini";
	_configFile = fopen (path.c_str (), "rb");
	if (_configFile == 0) return;
	try
	{
		parseConfig ();
	}
	catch (Int32 error)
	{
		printf ("%d\n", error);
	}
	fclose (_configFile);

	if (_rootNode._nodeList.size () == 0) return;
	Node* node = _rootNode._nodeList [0];
	if (node->_type != NodeType::section) return;
	if (node->_name != "Registry") return;

	SectionNode* registryNode = dynamic_cast <SectionNode*> (node);
	if (registryNode->_nodeList.size () == 0) return;

	node = registryNode->_nodeList [0];
	if (node->_type != NodeType::section) return;
	if (node->_name != "Services") return;

	SectionNode* servicesNode = dynamic_cast <SectionNode*> (node);
	UInt32 size = servicesNode->_nodeList.size (); 
	for (UInt32 i=0; i < size; ++i)
	{
		Node* node = servicesNode->_nodeList [i];
		if (node->_type != NodeType::section) continue;

		Config::ServiceInfo* serviceInfo = new Config::ServiceInfo ();
		serviceInfo->name = node->_name;
		serviceInfo->dllName = node->_name;
		serviceInfo->entry = node->_name + "_entryPoint";
		serviceInfo->path = _path;
		serviceInfo->priority = 0;
		_serviceInfoList.push_back (serviceInfo);

		{
			SectionNode* serviceNode = dynamic_cast <SectionNode*> (node);
			for (UInt32 i=0; i<serviceNode->_nodeList.size(); ++i)
			{
				Node* node = serviceNode->_nodeList [i];
				if (node->_type != NodeType::option) continue;

				OptionNode* option = dynamic_cast <OptionNode*> (node);
				if (option->_name == "entryPoint")
				{
					serviceInfo->entry = option->_value;
				}
				else if (option->_name == "dllName")
				{
					serviceInfo->dllName = option->_value;
				}
				else if (option->_name == "priority")
				{
					serviceInfo->priority = atoi (option->_value.c_str ());
				}
				else if (option->_name == "dllPath")
				{
					serviceInfo->path = option->_value;
				}
			}
		}
	}
}

Config::Config (std::string path):
	_path (path),
	_prevChar (0)
{
	getServiceInfoList ();
}

const Config::ServiceInfo* Config::findServiceInfo (std::string className)
{
	std::list <Config::ServiceInfo*>::iterator i;
	for (i = _serviceInfoList.begin (); i != _serviceInfoList.end (); ++i)
	{
		const Config::ServiceInfo* serviceInfo = *i;
		if (serviceInfo->name == className)
		{
			return serviceInfo;
		}
	}

	return 0;
}

const std::string& Config::getPath () const
{
	return _path;
}
