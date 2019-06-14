
Public Class bglTaxiwayExt
    Public TaxiwayPath As ctTaxiwayPath
    Public TaxiwayPoint() As ctTaxiwayPoint
    Public TaxiwayParking As ctTaxiwayParking
    Public TaxiName As ctTaxiName
    Sub New()

        TaxiwayPath = New ctTaxiwayPath()
        ReDim TaxiwayPoint(1)
        TaxiwayPoint(0) = New ctTaxiwayPoint() : TaxiwayPoint(1) = New ctTaxiwayPoint()
        TaxiwayParking = New ctTaxiwayParking()
        TaxiName = New ctTaxiName()

    End Sub

    Public linkstart As List(Of Long)
    Public linkend As List(Of Long)
End Class
Public Class bglRunwayExt
    Public Runway As ctRunway
    Public Start() As ctStart

    Sub New()
        Runway = New ctRunway()
        ReDim Start(1)
        Start(0) = New ctStart() : Start(1) = New ctStart()
    End Sub
End Class

Public Class bglTaxiwayComplex

    Public TaxiwayComplex As List(Of bglTaxiwayExt)

End Class