using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zCharts
{
    public class liteDB
    {
        public string DB { get; set; }
        public liteDB(string DB)
        {
            this.DB = DB;
        }
        public void insert(ScanResult a,string table)
        {
            using (var db = new LiteDatabase(this.DB))
            {
                var col = db.GetCollection<ScanResult>(table);
                var customer = new ScanResult
                {
                     ScanDate = a.ScanDate,
                     SymbolName= a.SymbolName,
                     ScanName = a.ScanName,
                     ScanValue= a.ScanValue,
                     Signal=a.Signal,
                     date=a.date,
                     open=a.open,
                     high=a.high,
                     low=a.low,
                     close=a.close,
                     volume=a.volume
                };
                col.Insert(customer);
                col.EnsureIndex(x => x.SymbolName); 
            }
        }
        public void insert(List<ScanResult> a, string table)
        {
            foreach (ScanResult x in a)
                insert(x, table);
        }
        public List<ScanResult> view(string table)
        {
            using (var db = new LiteDatabase(this.DB))
            {
                if (db != null)
                {
                    var col = db.GetCollection<ScanResult>(table);
                    col.EnsureIndex(x => x.SymbolName);
                    var results = col.Query()
                       .ToList();
                    return results;
                }
                return new List<ScanResult>();
            }
        }
        public void clear(string table)
        {
            using (var db = new LiteDatabase(this.DB))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<ScanResult>(table);

                //clean up to add new result
                //this is only for demonstration purpose
                if (col.Count() > 0)
                    col.DeleteAll();
            }
        }

    }

    public class ScanResult
    {
        public int Id { get; set; }
        public DateTime ScanDate { get; set; }
        public string SymbolName { get; set; }
        public string ScanName { get; set; }
        public double ScanValue { get; set; }
        public string Signal { get; set; }
        public DateTime date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
    }

}
