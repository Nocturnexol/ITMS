using System;
using System.Linq.Expressions;

namespace Bitshare.Common
{
    public class LambdaQuery<T> where T :class, new()
    {
        public string Condition;
        public LambdaQuery<T> Where(Expression<Func<T, bool>> expression) 
        {
            if (expression == null)
                return this;
            string condition;
            var visitor = new ExpressionVisitor<T>();
            if (expression == null)
                condition= "";
            condition = visitor.RouteExpressionHandler(expression.Body);
            Condition += string.IsNullOrEmpty(Condition) ? condition : " and " + condition;
            return this;
        }
        public override string ToString()
        {
            return Condition;
        }
    }
}
