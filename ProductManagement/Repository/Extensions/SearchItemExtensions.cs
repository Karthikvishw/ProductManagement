using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository.Extensions
{
    public static class SearchItemExtensions
    {
        private static Expression Like<T>(PropertyInfo propertyInfo, string propertyValue, ParameterExpression parameter)
        {
            ConstantExpression c = Expression.Constant($"%{propertyValue}%", typeof(string));
            MemberExpression memberExpression = Expression.Property(parameter, typeof(T).GetProperty(propertyInfo.Name));
            MethodInfo likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new[] { typeof(DbFunctions), typeof(string), typeof(string) });

            #region if String

            Expression methodCallExpression = null;

            if (IsNumericType(propertyInfo.PropertyType))
            {
                MethodInfo toString = typeof(Object).GetMethod("ToString"); // To String

                #region Fix nullable int

                if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                {
                    methodCallExpression = ToString<T>(propertyInfo, parameter);
                }
                else

                {
                    methodCallExpression = Expression.Call(memberExpression, toString);//Convert To String
                }

                #endregion Fix nullable int

                return Expression.Call(likeMethod, Expression.Constant(EF.Functions), methodCallExpression, c);
            }
            else if (propertyInfo.PropertyType == typeof(DateTime))
            {
                return EqualsDate<T>(propertyInfo, propertyValue, parameter);
            }

            #endregion if String

            return Expression.Call(likeMethod, Expression.Constant(EF.Functions), memberExpression, c);
        }

        public static IQueryable<TSource> SearchInColumns<TSource>(this IQueryable<TSource> source, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return source;

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), source.ElementType.Name);

            // Collect fields

            #region Reable Properties

            List<PropertyInfo> list = new List<PropertyInfo>();
            PropertyInfo[] propertiesInfo = source.ElementType.GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();
            foreach (var propertyInfo in propertiesInfo)
            {
                if ((propertyInfo.PropertyType == typeof(string)) || IsNumericType(propertyInfo.PropertyType) || (propertyInfo.PropertyType == typeof(DateTime)))
                {
                    list.Add(propertyInfo);
                }
            }
            if (list.Count == 0)
                return source;

            #endregion Reable Properties

            #region Or

            var concatenatedField = Like<TSource>(list[0], term, parameter);

            for (int i = 1; i < list.Count; i++)
            {
                Expression likeExpression = Like<TSource>(list[i], term, parameter);
                if (likeExpression != null)
                    concatenatedField = Expression.OrElse(concatenatedField, likeExpression);
            }
            MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { source.ElementType }, source.Expression, Expression.Lambda<Func<TSource, bool>>(concatenatedField, parameter));

            #endregion Or

            return source.Provider.CreateQuery<TSource>(whereCallExpression);
        }

        private static Expression ToString<T>(PropertyInfo propertyInfo, ParameterExpression parameter)
        {
            MemberExpression memberExpression = Expression.Property(parameter, typeof(T).GetProperty(propertyInfo.Name));
            MethodInfo toString = typeof(Object).GetMethod("ToString"); // To String

            #region Fix nullable

            MethodInfo concatMethod = typeof(String).GetMethod("Concat", new Type[] { typeof(object), typeof(object) });
            ConstantExpression empty = Expression.Constant(string.Empty, typeof(string));

            Expression conversion = Expression.Convert(memberExpression, typeof(object));

            Expression methodCallExpression = Expression.Call(concatMethod, conversion, empty);
            methodCallExpression = Expression.Call(methodCallExpression, toString);//Convert To String

            #endregion Fix nullable

            return methodCallExpression;
        }

        private static Expression EqualsDate<T>(PropertyInfo propertyInfo, string propertyValue, ParameterExpression parameter)
        {
            if (DateTime.TryParse(propertyValue, out DateTime dt))
            {
                ConstantExpression c = Expression.Constant(dt.Date, typeof(DateTime));
                var date = Expression.Property(Expression.Property(parameter, propertyInfo.Name), "Date");
                BinaryExpression expression = Expression.MakeBinary(ExpressionType.Equal, date, c);
                return expression; // call the toSting then Contains then with value
            }
            return null;
        }

        public static IQueryable<T> Sort<T>(IQueryable<T> fullquery, SearchItemModels param)
        {
            #region Sorting

            fullquery = fullquery.SortByColumn(param).Skip(param.Start).Take(param.Length);

            #endregion Sorting

            return fullquery;
        }

        public static IQueryable<TSource> SortByColumn<TSource>(this IQueryable<TSource> source, SearchItemModels param)
        {
            var DirectionMethode = param.SortDirection == "desc" ? "OrderBy" : "OrderByDescending";
            if (string.IsNullOrWhiteSpace(param.OrderBy))
                return source;

            ParameterExpression parameter = Expression.Parameter(typeof(TSource), source.ElementType.Name);

            var property = typeof(TSource).GetProperty(param.OrderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            MethodCallExpression resultExp = null;
            if (property != null)
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), DirectionMethode, new Type[] { typeof(TSource), property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }

            if (resultExp is null)
                return source;

            return source.Provider.CreateQuery<TSource>(resultExp);
        }

        public static async Task<ReturnSearchItemModels<T>> SearchAsync<T>(this IQueryable<T> query, SearchItemModels searchItem)
        {
            #region column Filter

            if (!string.IsNullOrWhiteSpace(searchItem.SeachTerm))
                query = query.SearchInColumns(searchItem.SeachTerm);

            #endregion column Filter

            int total = query.ToList().Count;

            #region Sorting

            query = Sort<T>(query, searchItem);

            #endregion Sorting

            int filtered = query.ToList().Count;

            return new ReturnSearchItemModels<T>
            {
                recordsTotal = total,
                recordsFiltered = filtered,
                data = query.ToList(),
            };
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null) return false;

            // from http://stackoverflow.com/a/5182747/172132
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
    }
}
