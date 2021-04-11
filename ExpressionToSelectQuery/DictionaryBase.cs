using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionToSelectQuery
{
    internal static class DictionaryBase
    {
        internal static readonly IDictionary<ExpressionType, string> nodeTypeMappings = new Dictionary<ExpressionType, string>
        {
            {ExpressionType.Add, "+"},
            {ExpressionType.And, "AND"},
            {ExpressionType.AndAlso, "AND"},
            {ExpressionType.Divide, "/"},
            {ExpressionType.Equal, "="},
            {ExpressionType.ExclusiveOr, "^"},
            {ExpressionType.GreaterThan, ">"},
            {ExpressionType.GreaterThanOrEqual, ">="},
            {ExpressionType.LessThan, "<"},
            {ExpressionType.LessThanOrEqual, "<="},
            {ExpressionType.Modulo, "%"},
            {ExpressionType.Multiply, "*"},
            {ExpressionType.Negate, "-"},
            {ExpressionType.Not, "NOT"},
            {ExpressionType.NotEqual, "<>"},
            {ExpressionType.Or, "OR"},
            {ExpressionType.OrElse, "OR"},
            {ExpressionType.Subtract, "-"}
        };

        internal static IDictionary<string, string> tableNameHolder = new Dictionary<string, string>();    
    }
}
