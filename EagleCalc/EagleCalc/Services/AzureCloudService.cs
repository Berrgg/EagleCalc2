using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;
using EagleCalc.Abstractions;
using EagleCalc.Helpers;

namespace EagleCalc.Services
{
    public class AzureCloudService : ICloudService
    {
        public MobileServiceClient Client { get; set; }
        public AzureCloudService()
        {
            Client = new MobileServiceClient(Locations.AppServiceUrl);
        }

        public Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData
        {
            throw new NotImplementedException();
        }

        #region Offline sync
        async Task InitializeAsync()
        {
            if (Client.SyncContext.IsInitialized)
                return;

            var store = new MobileServiceSQLiteStore("eagledb.db");

        }

        public Task SyncOfflineCacheAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
