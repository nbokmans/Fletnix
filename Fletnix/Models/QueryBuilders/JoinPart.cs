using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Fletnix.Models.QueryBuilders
{
    public class JoinPart
    {
        protected string column;
        private readonly string leftTable;
        private readonly string rightTable;
        private const string JoinType = " INNER ";
        public JoinPart(string leftTable, string rightTable, string column)
        {
            this.leftTable = leftTable;
            this.rightTable = rightTable;
           this.column = column;
        }

        public override string ToString()
        {
            return String.Format("{0} JOIN [{2}] [{3}] ON [{1}].[{4}] = [{3}].[{4}]", JoinType, leftTable.ToLower(), rightTable, rightTable.ToLower(), column);
        }
    }
}