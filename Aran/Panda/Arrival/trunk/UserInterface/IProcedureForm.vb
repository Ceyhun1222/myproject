<System.Runtime.InteropServices.ComVisible(False)> Interface IProcedureForm
	Property IsClosing As Boolean
	Sub DialogHook(result As Integer, ByRef NSegment As TraceSegment, Optional NewPDG As Double = 0)

End Interface
