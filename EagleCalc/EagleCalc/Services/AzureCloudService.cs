using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;
using EagleCalc.Abstractions;
using EagleCalc.Helpers;
using EagleCalc.Models;

namespace EagleCalc.Services
{
    public class AzureCloudService : ICloudService
    {
        public MobileServiceClient Client { get; set; }
        public AzureCloudService()
        {
            Client = new MobileServiceClient(Locations.AppServiceUrl);
        }

        //public async Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        //{
        //    await InitializeAsync();
        //    return new AzureCloudTable<T>(Client);
        //}

        public ICloudTable<T> GetTable<T>() where T : TableData
        {
            return new AzureCloudTable<T>(Client);
        }

        #region Offline sync
        //async Task InitializeAsync()
        //{
        //    if (Client.SyncContext.IsInitialized)
        //        return;

        //    var store = new MobileServiceSQLiteStore("eagledb.db");
        //    store.DefineTable<EagleBatch>();
        //    store.DefineTable<Line>();

        //    await Client.SyncContext.InitializeAsync(store);
        //    await SyncOfflineCacheAsync();
        //}

        //public async Task SyncOfflineCacheAsync()
        //{
        //    await InitializeAsync();

        //    await Client.SyncContext.PushAsync();

        //    var tbl = await GetTableAsync<EagleBatch>();
        //    await tbl.PullAsync();

        //    var tbl2 = await GetTableAsync<Line>();
        //    await tbl.PullAsync();
        //}
        #endregion

    }
}
