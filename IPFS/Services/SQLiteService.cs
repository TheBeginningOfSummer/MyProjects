using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IPFS.Services
{
    public class SQLiteService
    {
        public readonly string DatabaseName;
        public readonly string DatabasePath;
        private SQLiteAsyncConnection? sqlconnection;
        public SQLiteAsyncConnection SQLConnection => sqlconnection ??= new SQLiteAsyncConnection(DatabasePath);

        public SQLiteService(string databaseName = "data.db", string databasePath = "")
        {
            this.DatabaseName = databaseName;
            if (databasePath == "")
                DatabasePath = databaseName;
            else
                DatabasePath = Path.Combine(databasePath, databaseName);
        }

        public async void InitializeTableAsync<T>() where T : new()
        {
            try
            {
                await SQLConnection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                if (e.Message == $"no such table: {typeof(T).ToString().Split('.').LastOrDefault()}")
                    await SQLConnection.CreateTableAsync<T>();
            }
        }

        public async Task InsertOrUpdateAsync<T>(Expression<Func<T, bool>> predicate, T storedData) where T : ISQLData, new()
        {
            T data = await SQLConnection.FindAsync(predicate);
            if (data == null)
            {
                await SQLConnection.InsertAsync(storedData);
            }
            else
            {
                storedData.Id = data.Id;
                await SQLConnection.UpdateAsync(storedData);
            }
        }
    }

    public interface ISQLData
    {
        int Id { get; set; }
    }
}
