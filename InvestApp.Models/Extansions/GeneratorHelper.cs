using System;
using System.Collections.Generic;
using System.Linq;
using InvestApp.Models.Base;
using InvestApp.Models.Models;

namespace InvestApp.Models.Extansions
{
    public static class GeneratorHelper
    {
        /// <summary>
        /// Все типы для генерации окон с деталями.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetModelTypesPocos()
        {
            var type = typeof(Operation);
            var typeNamespace = type.Namespace;
            //return typeof(Address).Assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsEnum && x.Namespace == ns && !x.Name.Contains("<"));
            return type.Assembly.GetTypes().Where(type1 => type1.Namespace == typeNamespace && CommonExtansions.GetBaseTypes(type1).Contains(typeof(BaseEntity)));
        }
    }
}