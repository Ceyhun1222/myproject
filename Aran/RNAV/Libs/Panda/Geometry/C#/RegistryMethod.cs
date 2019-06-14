using System;
using System.Collections.Generic;
using System.Text;

namespace ARAN.GeometryClasses
{
	public interface RegistryMethod
	{
		int EntryPoint(int privateData, int command, int inout);
	}
}
