using SQLite;

namespace Cohabitation.Abstractions
{
    public class TableData
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        public int Version { get; set; }
        /// <summary>
        /// yyyy/MM
        /// </summary>
        public string Date { get; set; }
     }
}
