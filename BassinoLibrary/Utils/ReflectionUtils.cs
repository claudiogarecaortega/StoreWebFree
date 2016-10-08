using System;
using System.Linq.Expressions;

namespace BassinoLibrary.Utils
{
    public class ReflectionUtils
    {
        private static ReflectionUtils _instance;

        public static ReflectionUtils Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ReflectionUtils();

                return _instance;
            }
        }

        public virtual string GetPropertyName<T, TId>(Expression<Func<T, TId>> expression)
        {
            if (expression.Body is MemberExpression)
                return ((MemberExpression) expression.Body).Member.Name;

            var op = ((UnaryExpression) expression.Body).Operand;
            return ((MemberExpression) op).Member.Name;
        }
    }
}