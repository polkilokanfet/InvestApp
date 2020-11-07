using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InvestApp.Infrastructure.Attributes;

namespace InvestApp.Domain.Extansions
{
    public static class CommonExtansions
    {

        #region IsType

        public static bool IsType<T>(this PropertyInfo property)
        {
            return typeof(T) == property.PropertyType;
        }

        /// <summary>
        /// Простой ли тип
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimple(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        public static bool IsCollection(this Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static bool IsCollection(this PropertyInfo property)
        {
            return property.PropertyType.IsCollection();
        }

        public static bool IsComplex(this Type type)
        {
            return !type.IsSimple() && !type.IsCollection();
        }

        public static bool IsComplex(this PropertyInfo property)
        {
            return property.PropertyType.IsComplex();
        }

        //коллекция простых типов?
        public static bool CollectionMemberTypeIsSimple(Type genericCollectionType)
        {
            var t = genericCollectionType.GetInterfaces()
                .First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>))
                .GetGenericArguments()[0];

            return IsSimple(t);
        }

        #endregion


        /// <summary>
        /// Собирает все базовые типы, от которого наследуется этот
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var result = new List<Type>();
            while (type.BaseType != null)
            {
                result.Add(type.BaseType);
                type = type.BaseType;
            }
            return result;
        }

        /// <summary>
        /// возвращаем имя типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeName(this Type type)
        {
            if (!type.IsGenericType) return type.FullName;

            var genericArguments = type.GetGenericArguments().Select(GetTypeName).ToArray();
            var typeDefinition = type.GetGenericTypeDefinition().FullName;
            typeDefinition = typeDefinition.Substring(0, typeDefinition.IndexOf('`'));
            return $"{typeDefinition}<{string.Join(",", genericArguments)}>";
        }

        public static string GetCollectionElementTypeName(this PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces()
                .First(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                .GenericTypeArguments[0].Name;
        }

        #region Prop Types

        private static IEnumerable<PropertyInfo> GetProps(Type type)
        {
            return type.GetProperties();
        }

        /// <summary>
        /// Получить свойства string
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> StringProperties(this Type type)
        {
            return GetProps(type).Where(propertyInfo => propertyInfo.PropertyType == typeof(string));
        }

        /// <summary>
        /// Получить свойства string
        /// </summary>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> StringProperties(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(propertyInfo => propertyInfo.PropertyType == typeof(string));
        }



        /// <summary>
        /// Получить числовые свойства
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> DigitProperties(this Type type)
        {
            var propsInt = type.SimpleProperties<int>();
            var propsDouble = type.SimpleProperties<double>();
            return propsInt.Union(propsDouble);
        }

        public static IEnumerable<PropertyInfo> DigitProperties(this IEnumerable<PropertyInfo> propertyInfos)
        {
            var propsInt = propertyInfos.SimpleProperties<int>();
            var propsDouble = propertyInfos.SimpleProperties<double>();
            return propsInt.Union(propsDouble);
        }


        /// <summary>
        /// Получить простые свойства (int, double, DateTime)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> SimpleProperties<T>(this Type type)
            where T : struct
        {
            return GetProps(type).Where(propertyInfo => propertyInfo.PropertyType == typeof(T) || propertyInfo.PropertyType == typeof(T?));
        }

        public static IEnumerable<PropertyInfo> SimpleProperties<T>(this IEnumerable<PropertyInfo> propertyInfos)
            where T : struct
        {
            return propertyInfos.Where(propertyInfo => propertyInfo.PropertyType == typeof(T) || propertyInfo.PropertyType == typeof(T?));
        }

        public static IEnumerable<PropertyInfo> AllSimpleProperties(this Type type)
        {
            return GetProps(type).Where(propertyInfo => propertyInfo.PropertyType.IsSimple());
        }

        public static IEnumerable<PropertyInfo> AllSimpleProperties(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(propertyInfo => propertyInfo.PropertyType.IsSimple());
        }


        public static IEnumerable<PropertyInfo> AllCollectionProperties(this Type type)
        {
            return GetProps(type).Except(type.AllSimpleProperties())
                    .Where(propertyInfo => propertyInfo.PropertyType.GetInterfaces().Any(type1 => type1.IsGenericType && type1.GetGenericTypeDefinition() == typeof(ICollection<>)));
        }

        public static IEnumerable<PropertyInfo> AllCollectionProperties(this IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Except(propertyInfos.AllSimpleProperties())
                    .Where(propertyInfo => propertyInfo.PropertyType.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>)));
        }

        public static IEnumerable<PropertyInfo> CollectionComplexProperties(this Type type)
        {
            return type.AllCollectionProperties().Where(propertyInfo => !CollectionMemberTypeIsSimple(propertyInfo.PropertyType));
        }

        public static IEnumerable<PropertyInfo> AllComplexProperties(this Type type)
        {
            //var allComplexProperties = allProperties.Except(simpleProperties).Where(p => p.PropertyType.IsClass && !typeof(IEnumerable).IsAssignableFrom(p.PropertyType));
            return GetProps(type).Except(type.AllSimpleProperties()).Except(type.AllCollectionProperties()).Except(type.SimpleProperties<double>()).Except(type.SimpleProperties<DateTime>());
        }

        public static IEnumerable<PropertyInfo> AllComplexProperties(this IEnumerable<PropertyInfo> propertyInfos)
        {
            var list = propertyInfos.ToList();
            return list
                .Except(list.AllSimpleProperties())
                .Except(list.AllCollectionProperties())
                .Except(list.SimpleProperties<double>())
                .Except(list.SimpleProperties<DateTime>());
        }

        #endregion

        public static int OrderStatus(this PropertyInfo property)
        {
            var attr = property.GetCustomAttribute<OrderStatusAttribute>();
            return attr?.OrderStatus ?? 1;
        }
    }
}
