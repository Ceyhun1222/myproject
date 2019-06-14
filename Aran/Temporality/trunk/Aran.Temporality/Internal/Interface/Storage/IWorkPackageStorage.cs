namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IWorkPackageStorage
    {
        int CreateWorkPackage(int storageId, bool isSafe = false, string description = null);
    }
}
