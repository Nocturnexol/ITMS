using System;
using System.Linq.Expressions;
using System.Text;

namespace Bitshare.Common
{
    internal class ExpressionVisitor<T> where T : class, new()
    {
        /// <summary>
        /// 拆分表达式树
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string BinaryExpressionHandler(Expression left, Expression right, ExpressionType type)
        {
            StringBuilder sb = new StringBuilder();
            
            string needParKey = "=,>,<,>=,<=,<>";
            string leftPar = RouteExpressionHandler(left);
            string typeStr = ExpressionTypeCast(type);
            var isRight = needParKey.IndexOf(typeStr) > -1;
            string rightPar = RouteExpressionHandler(right, isRight);

            if (!string.IsNullOrEmpty(rightPar))
            {
                string appendLeft = leftPar;
                sb.Append("(");
                sb.Append(appendLeft);//字段名称

                if (rightPar.ToUpper() == "NULL")
                {
                    if (typeStr == "=")
                        rightPar = " IS NULL ";
                    else if (typeStr == "<>")
                        rightPar = " IS NOT NULL ";
                }
                else
                {
                    sb.Append(typeStr);
                }
                sb.Append(rightPar);
                sb.Append(")");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isRight"></param>
        /// <returns></returns>
        public string RouteExpressionHandler(Expression exp, bool isRight = false)
        {
            if (exp is BinaryExpression)
            {
                BinaryExpression be = (BinaryExpression)exp;
                //重新拆分树,形成递归
                return BinaryExpressionHandler(be.Left, be.Right, be.NodeType);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression mExp = (MemberExpression)exp;
                if (isRight)//按表达式右边值
                {
                    var obj = Expression.Lambda(mExp).Compile().DynamicInvoke();
                    if (obj is Enum)
                    {
                        obj = (int)obj;
                    }
                    return obj + "";
                }
                return mExp.Member.Name;
            }
            else if (exp is NewArrayExpression)
            {
                #region 数组
                NewArrayExpression naExp = (NewArrayExpression)exp;
                StringBuilder sb = new StringBuilder();
                foreach (Expression expression in naExp.Expressions)
                {
                    sb.AppendFormat(",{0}", RouteExpressionHandler(expression));
                }
                return sb.Length == 0 ? "" : sb.Remove(0, 1).ToString();
                #endregion
            }
            else if (exp is MethodCallExpression)
            {
                if (isRight)
                {
                    var val = Expression.Lambda(exp).Compile().DynamicInvoke();
                    if (val is DateTime)
                    {
                        val = "";
                    }
                    return val + "";
                }
                //在这里解析方法
                throw new Exception("暂不支持");
            }
            else if (exp is ConstantExpression)
            {
                #region 常量
                ConstantExpression cExp = (ConstantExpression)exp;
                if (cExp.Value == null)
                    return "null";
                else
                {
                    return cExp.Value.ToString();
                }
                #endregion
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return RouteExpressionHandler(ue.Operand, isRight);
            }
            return null;
        }
        public string ExpressionTypeCast(ExpressionType expType)
        {
            switch (expType)
            {
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    throw new InvalidCastException("不支持的运算符");
            }
        }

    }

}
