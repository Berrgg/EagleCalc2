using EagleCalc.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EagleCalc.Abstractions
{
    public interface ICloudTable<T> where T: TableData
    {
        Task<T> CreateItemAsync(T item);
        Task<T> ReadItemAsync(string id);
        Task<T> UpdateItemAsync(T item);
        Task<T> UpsertItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<ICollection<T>> ReadAllItemsAsync();
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
        Task<ICollection<Product>> ReadProducts(string customer);
        Task<ICollection<EagleBatch>> ReadListOfBatches(DateTimeOffset todatDate, string line, string productCode);
        Task<ICollection<EagleBatch>> ReadListOfPallets(string batchId);
      //  Task PullAsync();
    }
}
