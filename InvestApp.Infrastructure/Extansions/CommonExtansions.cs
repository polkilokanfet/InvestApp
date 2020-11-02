using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestApp.Infrastructure.Extansions
{
    public static class CommonExtansions
    {
        /// <summary>
        /// Вернуть атрибуты определенного типа.
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }

    }
}
