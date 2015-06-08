using System;

namespace Fletnix.Models.QueryBuilders
{
    public class WherePart
    {
        private readonly string column;
        private readonly string value;
        private readonly string whereOperator;
        private readonly string dataType;

        public string Value { get; private set; }

        public WherePart(string column, string value, string whereOperator, string dataType)
        {
            Value = value;
            this.column = column;
            this.whereOperator = whereOperator;
            this.dataType = dataType;
        }

        private string GetOperand(int idx)
        {
            var operand = "";

            if (dataType.ToLower() == "varchar")
            {
                if (whereOperator == "LIKE")
                    operand = String.Format(" '%'+{0}+'%' ", "@p" + idx);
                else
                    operand = String.Format(" {0} ", "@p" + idx);
            }
            else if (dataType.ToLower() == "number")
            {
                operand = String.Format(" {0} ", "@p" + idx);
            }
            else if (dataType.ToLower() == "date")
            {
                operand = String.Format(" CONVERT (datetime, {0} , 112) ", "@p" + idx);
            }
            return operand;
        }

        public override string ToString()
        {
            throw new ArgumentException("You must provide an paramIdx when calling to string.");
            return base.ToString();
        }

        public string ToString(int idx)
        {
            var whereAnd = idx == 0 ? "WHERE" : "AND";
            return String.Format(" {0} [{1}] {2} {3}",whereAnd, column, whereOperator, GetOperand(idx));
        }
    }
}