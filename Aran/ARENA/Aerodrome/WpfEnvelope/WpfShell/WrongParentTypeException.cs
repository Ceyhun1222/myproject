using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfEnvelope.WpfShell
{
	[global::System.Serializable]
	public class WrongParentTypeException : ApplicationException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public WrongParentTypeException() { }
		public WrongParentTypeException(string message) : base(message) { }
		public WrongParentTypeException(string message, Exception inner) : base(message, inner) { }
		protected WrongParentTypeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
