using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fletnix.Models.ViewModels;
using FletnixDatabase.Models;

namespace Fletnix.Models.QueryBuilders
{
    public abstract class AbstractQueryBuilder
    {
        protected readonly Queue<JoinPart> joinQueue = new Queue<JoinPart>();
        protected readonly Queue<WherePart> whereQueue = new Queue<WherePart>();
        protected string order = "";
        private int take = 50;
        private int skip;
        public List<string> Columns { get; set; }
        public string From { get; set; }

        public AbstractQueryBuilder Skip(int i)
        {
            skip = i;
            return this;
        }

        public AbstractQueryBuilder Take(int i)
        {
            take = i;
            return this;
        }

        public List<T> ExecuteQuery<T>(DbContext db)
        {
            var paramQueue = new Queue<string>();
            var query = GetQueryString(paramQueue);
            var sqlQuery = db.Database.SqlQuery<T>(query, paramQueue.ToArray());
            return sqlQuery.ToList();
        }

        private string GetQueryString(Queue<string> paramQueue)
        {
            var columns = ColumnsListToString();
            var sb = new StringBuilder("SELECT ");
            if (take > 0)
            {
                sb.Append("TOP (").Append(take).Append(") ");
            }

            sb.Append(columns);

            if (skip > 0)
            {
                sb.Append("FROM ( ");
                sb.Append("SELECT ");
                sb.Append(columns);
                sb.Append(", row_number() OVER (").Append(order).Append(") AS rowNumber ");
            }
            sb.Append("FROM ");
            sb.Append(From);

            var joins = JoinQueueToString();
            sb.Append(joins);
            var wheres = WhereQueueToString(paramQueue);
            sb.Append(wheres);

            if (skip > 0)
            {
                sb.Append(") as title WHERE title.rowNumber > ").Append(skip);
            }

            return sb.Append(order).ToString();
        }

        private string WhereQueueToString(Queue<string> paramQueue)
        {
            var sb = new StringBuilder();
            var idx = 0;
            foreach (var where in whereQueue)
            {
                sb.Append(@where.ToString(idx));
                paramQueue.Enqueue(@where.Value);
                idx++;
            }
            return sb.ToString();
        }

        private string JoinQueueToString()
        {
            var sb = new StringBuilder();
            foreach (var join in joinQueue)
            {
                sb.Append(@join);
            }
            return sb.ToString();
        }

        private string ColumnsListToString()
        {
            var sb = new StringBuilder();
            var last = Columns.Last();
            foreach (var var in Columns)
            {
                sb.Append(var);
                if (!var.Equals(last))
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" ");
            return sb.ToString();
        }

        public abstract AbstractQueryBuilder Ordering(string column, string ordering);

        public int GetNumTotalResults(DbContext db)
        {
            var tempTake = take;
            var tempSkip = skip;
            var tempColumns = Columns;
            var tempOrder = order;
            order = "";
            Take(0).Skip(0);
            Columns = new List<string>{@" COUNT(1) "};

            var result = ExecuteQuery<int>(db).First();

            Take(tempTake).Skip(tempSkip);
            Columns = tempColumns;
            order =  tempOrder;
            return result;
        }
        
        private static string Trim(String input)
        {
            return Regex.Replace(input, @"\t|\n|\r", "");
        }
    }
}