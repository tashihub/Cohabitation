using Cohabitation.Models;
using SQLite;

namespace Cohabitation.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>
    {
        SQLiteConnection connection;
        public TransactionRepository() : base()
        {
            connection =
                new SQLiteConnection(Constants.DatabasePath,
                                     Constants.Flags);
            //connection.DropTable<T>(); // テーブル削除
            connection.CreateTable<Transaction>();

        }

        /// <summary>
        /// "25" のような2桁の年を渡すと、"25/01" ～ "25/12" に一致する Transaction を取得する
        /// </summary>
        /// <param name="year">"25"などの2桁の年</param>
        /// <returns>一致した Transaction のリスト</returns>
        public List<Transaction> GetTransactionsByYear(string year)
        {
            try
            {
                 var result = connection.Table<Transaction>()
                    .Where(x => x.Setting.Date.StartsWith(year))
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                return new List<Transaction>();
            }
        }

        public void DeleteItem(Transaction item)
        {
            try
            {
                //connection.Delete(item);
                connection.Delete(item);
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
        }

    }
}
