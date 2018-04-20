using System;
using System.Linq;
using System.Reflection;

namespace JokerKS.WLFU.Entities.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Sortable : Attribute
    {
        public readonly string SortablePropertyName;

        public Sortable(string name = null)
        {
            SortablePropertyName = name;
        }
    }

    public static class SortaleExtension
    {
        public static bool IsSortable(this PropertyInfo obj)
        {
            var attrs = obj.GetCustomAttributes().Where(x => x.GetType() == typeof(Sortable));
            return attrs.Count() > 0 ? true : false;
        }
    }
}