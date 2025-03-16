using Cohabitation.Abstractions;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Linq.Expressions;
using static SQLite.SQLite3;

namespace Cohabitation.Repositories
{
    public class BaseRepository<T> :
         IBaseRepository<T> where T : TableData, new()
    {
        SQLiteConnection connection;
        public string StatusMessage { get; set; }

        public BaseRepository()
        {
            connection =
                 new SQLiteConnection(Constants.DatabasePath,
                 Constants.Flags);
            //connection.DropTable<T>(); // テーブル削除
            connection.CreateTable<T>();
        }

        public List<T> GetAllData()
        {
            return connection.Table<T>().ToList();
        }

        public void DeleteItem(T item)
        {
            try
            {
                //connection.Delete(item);
                connection.Delete(item, true);
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
        }

        public void DeleteAllItems()
        {
            try
            {
                connection.DeleteAll<T>(); // テーブルの全データを削除
                StatusMessage = "All records deleted successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }


        public void Dispose()
        {
            connection.Close();
        }

        public T GetItem(int id)
        {
            try
            {
                return
                     connection.Table<T>()
                     .FirstOrDefault(x => x.ID == id);
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }




        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return connection.Table<T>()
                     .Where(predicate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }

        public List<T> GetItems()
        {
            try
            {
                return connection.Table<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }

        public List<T> GetItems(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return connection.Table<T>().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }

        public void SaveItem(T item)
        {
            int result = 0;
            try
            {
                if (item.ID != 0)
                {
                    result =
                         connection.Update(item);
                    StatusMessage =
                         $"{result} row(s) updated";
                }
                else
                {
                    result = connection.Insert(item);
                    StatusMessage =
                         $"{result} row(s) added";
                }

            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
        }




        public void SaveItemWithChildren(T item, bool recursive = false)
        {
            connection.InsertWithChildren(item, recursive);
        }

        public List<T> GetItemsWithChildren()
        {
            try
            {
                return connection.GetAllWithChildren<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }


        public void TestDeleteAllItem()
        {
            try
            {
                connection.Execute($"DELETE FROM {typeof(T).Name};"); // 全データ削除
                connection.Execute("DELETE FROM sqlite_sequence WHERE name = ?", typeof(T).Name); // IDリセット
                connection.Execute("VACUUM;"); // 最適化
                StatusMessage = "All records deleted and ID reset to 1.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
    }
}
