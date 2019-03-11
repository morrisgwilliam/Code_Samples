using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioWebApi.Data.Structured
{
    public class NVarCharTable : IEnumerable<SqlDataRecord>
    {
        private IEnumerable<string> _items;

        public NVarCharTable(IEnumerable<string> items)
        {
            _items = items;
        }

        private static SqlDataRecord GetRecord()
        {
            return new SqlDataRecord(
                new SqlMetaData[] { new SqlMetaData("Data", System.Data.SqlDbType.NVarChar, -1) });
        }
        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            if (_items != null)
            {
                foreach (string item in _items)
                {
                    var rec = GetRecord();
                    rec.SetString(0, item);

                    yield return rec;
                }
            }
            yield break;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}