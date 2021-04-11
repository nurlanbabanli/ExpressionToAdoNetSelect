using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionToSelectQuery
{
    internal class ExpressionToSqlSelect
    {
        public string GetSqlSelectString(Expression expression)
        {
            string tableName = "";
            LambdaExpression lambdaExpression;

            if (expression is LambdaExpression)
            {
                lambdaExpression = (LambdaExpression)expression;
                tableName = GetTabelName(lambdaExpression);
            }
            else
            {
                throw new Exception("Invalid expression type");
            }
            string where = "Select *From " + tableName + " Where ";
            SelectExpressionType(lambdaExpression.Body, ref where);

            return where;
        }

        private void SelectExpressionType(Expression expression, ref string where)
        {
            switch (expression)
            {
                case UnaryExpression unary: throw new Exception("Invalid expression type"); break;
                case BinaryExpression binary: BinaryExpression(binary, ref where); break;
                case ConstantExpression constant: throw new Exception("Invalid expression type"); break;
                case MemberExpression member: throw new Exception("Invalid expression type"); break;
                case MethodCallExpression methodCall: throw new Exception("Invalid expression type"); break;
                case InvocationExpression invocation: throw new Exception("Invalid expression type"); break;
            }
        }

        private void BinaryExpression(BinaryExpression expression, ref string where)
        {
            if (expression.Left is BinaryExpression)
            {
                SelectExpressionType(expression.Left, ref where);
            }
            else if (expression.Left is MemberExpression)
            {
                string nodeType = null;
                DictionaryBase.nodeTypeMappings.TryGetValue(expression.NodeType, out nodeType);
                MemberExpression((MemberExpression)expression.Left, ref where, ref nodeType);
            }


            if (expression.Right is BinaryExpression)
            {
                string nodeType = null;
                DictionaryBase.nodeTypeMappings.TryGetValue(expression.NodeType, out nodeType);
                where = where + " " + nodeType + " ";
                SelectExpressionType(expression.Right, ref where);
            }
            else if (expression.Right is ConstantExpression)
            {
                ConstantExpression((ConstantExpression)expression.Right, ref where);
            }
            else if (expression.Right is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)expression.Right;
                if (memberExpression.Expression is ConstantExpression)
                {
                    ConstantExpression constantExpression = (ConstantExpression)memberExpression.Expression;
                    if (memberExpression.Member is FieldInfo fieldInfo)
                    {
                        ConstantExpression(constantExpression, ref where, fieldInfo);
                    }
                }
            }
        }


        private void MemberExpression(MemberExpression expression, ref string where, ref string nodeType)
        {
            where = where + expression.Member.Name + nodeType;
        }

        private void ConstantExpression(ConstantExpression expression, ref string where)
        {
            if (expression.Value.GetType() == typeof(string))
            {
                where = where + "'" + expression.Value + "'";
            }
            else
            {
                where = where + expression.Value;
            }
        }
        private void ConstantExpression(ConstantExpression expression, ref string where, FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType==typeof(string))
            {
                where = where +"'"+ fieldInfo.GetValue(expression.Value)+"'"; 
            }
            else if(fieldInfo.FieldType == typeof(DateTime))
            {
                DateTime dateTime = (DateTime)fieldInfo.GetValue(expression.Value);
                where = where + "'"+dateTime.ToString("yyyy-MM-dd HH:mm:ss")+"'";
            }
            else
            {
                where = where + fieldInfo.GetValue(expression.Value);
            }
        }


        private string GetTabelName(LambdaExpression lambdaExpression)
        {
            string entityTypeName = null;
            string tableName = null;
            if (lambdaExpression.Body is BinaryExpression)
            {
                SelectExpression(lambdaExpression.Body, ref entityTypeName);
            }
            DictionaryBase.tableNameHolder.TryGetValue(entityTypeName, out tableName);
            if (tableName == null)
            {
                throw new Exception("Table name not found");
            }

            return tableName;
        }

        private void SelectExpression(Expression expression, ref string tableName)
        {
            if (expression is BinaryExpression)
            {
                BinaryExpression binaryExpression = (BinaryExpression)expression;
                SelectExpression(binaryExpression.Left, ref tableName);
            }
            else if (expression is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)expression;
                if (memberExpression.Member is PropertyInfo property)
                {
                    tableName = property.DeclaringType.Name;
                }
            }
        }
    }
}
