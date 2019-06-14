namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IStorageStorage : ICrudStorage<Common.Entity.Storage>
    {
        Common.Entity.Storage GetStorageByName(string storageName);
    }
}
