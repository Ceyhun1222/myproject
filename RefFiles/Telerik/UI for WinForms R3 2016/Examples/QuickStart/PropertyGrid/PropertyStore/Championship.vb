Namespace Telerik.Examples.WinControls.PropertyGrid.PropertyStore
	Public Class Championship
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
		End Property
        Private privateAllowedEngineLayouts As EngineLayout
        Public Property AllowedEngineLayouts() As EngineLayout
            Get
                Return privateAllowedEngineLayouts
            End Get
            Set(ByVal value As EngineLayout)
                privateAllowedEngineLayouts = value
            End Set
        End Property
        Private privateMaximumEngineDisplacement As Integer
        Public Property MaximumEngineDisplacement() As Integer
            Get
                Return privateMaximumEngineDisplacement
            End Get
            Set(ByVal value As Integer)
                privateMaximumEngineDisplacement = value
            End Set
        End Property
        Private privateAllowedFuels As Fuel
        Public Property AllowedFuels() As Fuel
            Get
                Return privateAllowedFuels
            End Get
            Set(ByVal value As Fuel)
                privateAllowedFuels = value
            End Set
        End Property
        Private privateAllowedEngineSupercharged As Boolean
        Public Property AllowedEngineSupercharged() As Boolean
            Get
                Return privateAllowedEngineSupercharged
            End Get
            Set(ByVal value As Boolean)
                privateAllowedEngineSupercharged = value
            End Set
        End Property
        Private privateAllowedTransmissions As Transmission
        Public Property AllowedTransmissions() As Transmission
            Get
                Return privateAllowedTransmissions
            End Get
            Set(ByVal value As Transmission)
                privateAllowedTransmissions = value
            End Set
        End Property
        Private privateAllowedDrives As Drive
        Public Property AllowedDrives() As Drive
            Get
                Return privateAllowedDrives
            End Get
            Set(ByVal value As Drive)
                privateAllowedDrives = value
            End Set
        End Property
        Private privateMinimumNumberOfSeats As Integer
        Public Property MinimumNumberOfSeats() As Integer
            Get
                Return privateMinimumNumberOfSeats
            End Get
            Set(ByVal value As Integer)
                privateMinimumNumberOfSeats = value
            End Set
        End Property
        Private privateMinimumVehicleWeight As Integer
        Public Property MinimumVehicleWeight() As Integer
            Get
                Return privateMinimumVehicleWeight
            End Get
            Set(ByVal value As Integer)
                privateMinimumVehicleWeight = value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal name As String, ByVal allowedLayouts As EngineLayout, ByVal maxDisplacement As Integer, ByVal allowedFuels As Fuel, ByVal allowSupercharged As Boolean, ByVal allowedTransmissions As Transmission, ByVal allowedDrives As Drive, ByVal maxNumberOfSeats As Integer, ByVal minWight As Integer)
            Me.Name = name
            Me.AllowedEngineLayouts = allowedLayouts
            Me.MaximumEngineDisplacement = maxDisplacement
            Me.AllowedFuels = allowedFuels
            Me.AllowedEngineSupercharged = allowSupercharged
            Me.AllowedTransmissions = allowedTransmissions
            Me.AllowedDrives = allowedDrives
            Me.MinimumNumberOfSeats = maxNumberOfSeats
            Me.MinimumVehicleWeight = minWight
        End Sub
	End Class
End Namespace
