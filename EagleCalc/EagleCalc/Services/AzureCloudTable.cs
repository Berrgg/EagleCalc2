using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using EagleCalc.Abstractions;
using EagleCalc.Models;

namespace EagleCalc.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        //IMobileServiceSyncTable<T> table;
        IMobileServiceSyncTable<T> table;
        IMobileServiceSyncTable<Product> productTable;
        IMobileServiceSyncTable<EagleBatch> batchTable;

        public AzureCloudTable(MobileServiceClient client)
        {
            table = client.GetSyncTable<T>();
            productTable = client.GetSyncTable<Product>();
            batchTable = client.GetSyncTable<EagleBatch>();
        }

        #region ICloudTable implementation

        public async Task<T> CreateItemAsync(T item)
        {
            await table.InsertAsync(item);
            return item;
        }

        public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);
        }

        public async Task PullAsync()
        {
            string queryName = $"incsync_{typeof(T).Name}";
            await table.PullAsync(queryName, table.CreateQuery());
        }

        public async Task PullAsyncBatch()
        {
          //  string queryName = $"incsync_{typeof(T).Name}";
            await batchTable.PullAsync("allBatches", batchTable.CreateQuery());
        }


        //public async Task<ICollection<T>> ReadAllItemsAsync()
        //{
        //    return await table.ToListAsync();
        //}

        public async Task<ICollection<Product>> ReadProducts(string customer)
        {
            return await productTable.Where(x => x.CustomerName == customer).ToListAsync();
        }

        public async Task<ICollection<EagleBatch>> ReadListOfBatches(DateTimeOffset todatDate, string line, string productCode)
        {
            return await batchTable.Where(x => x.ProductionDate == todatDate && x.Line == line && x.ProductCode == productCode).ToListAsync();
        }

        public async Task<ICollection<EagleBatch>> ReadListOfPallets(string batch)
        {
            return await batchTable.Where(x => x.IdBatch == batch).ToListAsync();
        }

        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            List<T> allItems = new List<T>();

            var pageSize = 50;
            var hasMore = true;

            while (hasMore)
            {
                var pageOfItems = await table.Skip(allItems.Count).Take(pageSize).ToListAsync();
                if (pageOfItems.Count > 0)
                {
                    allItems.AddRange(pageOfItems);
                }
                else
                {
                    hasMore = false;
                }
            }
            return allItems;
        }

        public async Task<T> ReadItemAsync(string id)
        {
            return await table.LookupAsync(id);
        }

        public async Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            return await table.Skip(start).Take(count).ToListAsync();
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }

        public async Task<T> UpsertItemAsync(T item)
        {
            return (item.Id == null) ? await CreateItemAsync(item) : await UpdateItemAsync(item);
        }
        #endregion
    }        

}
