using Cohabitation.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohabitation.Repositories
{
    public class SettingRepository : BaseRepository<Setting>
    {
        SQLiteConnection connection;

        public SettingRepository() :base()
        {
            connection =
            new SQLiteConnection(Constants.DatabasePath,
            Constants.Flags);
            //connection.DropTable<T>(); // テーブル削除
            connection.CreateTable<Setting>();
        }

        public Setting GetItemByDateTime(string currentDate)
        {
            try
            {
                return connection.Table<Setting>()
                    .FirstOrDefault(x => x.Date == currentDate);
            }
            catch (Exception ex)
            {
                StatusMessage =
                     $"Error: {ex.Message}";
            }
            return null;
        }

        public string SaveItemByDate(Setting saveItem,Setting prevItem)
        {
            try
            {
                var result = 0;

                var table = connection.Table<Setting>()
                    .FirstOrDefault(x => x.Date == saveItem.Date);
                if (table != null)
                {
                    //もし現在高に途中で変更が加えられていた場合バージョンをあげる
                    //計算結果はバージョンが最新のものだけを取得する
                    if (saveItem != prevItem)
                    {
                        saveItem.Version++;
                    }
                    result =
                         connection.Insert(saveItem);
                    return StatusMessage =
                         $"{saveItem.Date}の設定を更新しました！";
                }
                else
                {
                    result = connection.Insert(saveItem);
                    return StatusMessage =
                         $"{saveItem.Date}の設定に成功しました！";
                }

            }
            catch (Exception ex)
            {
                return StatusMessage =
                     $"Error: {saveItem.Date}の設定に失敗しました！";
            }
        }
    }
}
