using System.Threading.Tasks;

namespace EagleCalc.Abstractions
{
    public interface ICloudService
    {
        Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData;
        //    ICloudTable<T> GetTable<T>() where T : TableData;
        Task SyncOfflineCacheAsync();
    }
}
